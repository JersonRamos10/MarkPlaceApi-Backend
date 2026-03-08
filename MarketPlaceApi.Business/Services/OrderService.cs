
using System.Data;
using MarketPlaceApi.Business.DTOs.Clients;
using MarketPlaceApi.Business.DTOs.Order;
using MarketPlaceApi.Business.DTOs.OrderDetail;
using MarketPlaceApi.Business.DTOs.Products;
using MarketPlaceApi.Business.DTOs.Seller;
using MarketPlaceApi.Business.DTOs.Category;
using MarketPlaceApi.Business.DTOs.Link;
using MarketPlaceApi.Business.Exceptions;
using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Domain.Enums;
using MarketPlaceApi.Business.Services.Interfaces;
namespace MarketPlaceApi.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo; 
        private readonly IClientRepository _clientRepo; 

        private readonly IProductRepository _prodRepo;

        private readonly IEmailService _emailService;


        private readonly ISellerRepository _sellerRepo;

        private readonly IInvoiceService _invoiceService;
        public OrderService (IOrderRepository orderRepo , 
        IClientRepository clientRepo,
        IProductRepository prodRepo,
        ISellerRepository sellerRepo,
        IInvoiceService invoiceService,
        IEmailService emailService){
            _orderRepo = orderRepo;
            _clientRepo = clientRepo;
            _prodRepo = prodRepo;
            _sellerRepo = sellerRepo;
            _invoiceService = invoiceService;
            _emailService = emailService;
            
        }
        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest orderRequest)
        {
            var client = await _clientRepo.GetByEmailAsync(orderRequest.Client.Email);
            
            if(client == null){
                client = await CreateClientAsync(orderRequest.Client);
            }

            var product = await _prodRepo.GetByIdWithDetailsAsync(orderRequest.ProductId);

            if(product == null) 
                throw new NotFoundException("Product not Found");

            var sellerId = product.SellerId;
            var orderNumber = GenerateOrderNumber();

            var order = new Order{
                OrderNumber = orderNumber,
                ClientId = client.ClientId,
                SellerId = sellerId,
                PaymentMethod = orderRequest.PaymentMethod,
                PaymentReceiptUrl = orderRequest.PaymentReceiptUrl,

            };

            var orderDetail = new OrderDetail{
                Order = order,
                ProductId = orderRequest.ProductId,
                UnitPriceAtSale = product.Price,
                Quantity = orderRequest.Quantity,
            };

            order.OrderDetails.Add(orderDetail);
            
            await _orderRepo.AddAsync(order);
            
            
            await _orderRepo.SaveChangesAsync();
            
            // Recargar con todas las relaciones para el mapeo
            var savedOrder = await _orderRepo.GetByIdAsync(order.OrderId)
                ?? throw new NotFoundException("Order not found");

            return MapToOrderDto(savedOrder);
        }

        public async Task<OrderResponse> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) 
                    ?? throw new NotFoundException("Order not found");
            
            return MapToOrderDto(order);
        }

        public async Task<IEnumerable<OrderSummaryResponse>> GetOrdersBySellerAsync(Guid sellerId)
        {
            var sellerExist = await _sellerRepo.GetByIdAsync(sellerId) 
                ?? throw new NotFoundException("Seller Not Found");
            
            var orderBySeller = await _orderRepo.GetBySellerIdAsync(sellerId);

            return orderBySeller.Select(order => MapToOrderSummaryDto(order));
        }

        public async Task<string> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepo.GetByIdForUpdateAsync(orderId)
                ?? throw new NotFoundException("Order not found");

            order.Status = status;

            _orderRepo.Update(order);

            await _orderRepo.SaveChangesAsync();

            var clientFullName = $"{order.Client.FirstName} {order.Client.LastName}";

            switch (order.Status)
            {
                case OrderStatus.Pagada: 
                    var (invoice, pdfBytes) = await _invoiceService.GenerateAsync(order.OrderId);
                    await _emailService.SendInvoiceAsync(
                        order.Client.Email,
                        clientFullName,
                        pdfBytes
                    );
                    break;

                case OrderStatus.Cancelada:
                    await _emailService.SendOrderStatusAsync(
                        order.Client.Email,
                        clientFullName,
                        "Tu pedido ha sido cancelado",
                        "Tu pedido ha sido cancelado. Si tienes dudas, contáctanos."
                    );
                    break;


                case OrderStatus.Entregada:
                    await _emailService.SendOrderStatusAsync(
                        order.Client.Email,
                        clientFullName,
                        "Tu pedido ha sido entregado",
                        $"Tu pedido {order.OrderNumber} ha sido entregado. ¡Gracias por tu compra!"
                    );
                    break;
                case OrderStatus.EnRevision:
                    await _emailService.SendOrderStatusAsync(
                        order.Client.Email,
                        clientFullName,
                        "Tu pedido se encuentra en revicion",
                        $"Tu pedido {order.OrderNumber} esta siendo procesado por nuestro equipo. Su orden encuentra en fase de revision. "
                    );
                    break;

                case OrderStatus.Rechazada:
                    await _emailService.SendOrderStatusAsync(
                        order.Client.Email,
                        clientFullName,
                        "Tu pedido ha sido Rechazado",
                        $"Tu pedido {order.OrderNumber} Tu pedido ha sido Rechazado. Si tienes dudas, contáctanos. "
                    );
                    break;
            }

            return $"Order updated successfully";
        }

        private string GenerateOrderNumber()
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string randomPart = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            return $"ORD-{datePart}-{randomPart}";
        }

        //crea el nuevo cliente 
        private async Task<Client> CreateClientAsync (ClientRequest clientRequest){

            var client = new Client{
                FirstName = clientRequest.FirstName,
                LastName = clientRequest.LastName,
                Email = clientRequest.Email,
                Dui = clientRequest.Dui,
                Address = clientRequest.Address,
                Phone = clientRequest.Phone
            };

            await _clientRepo.AddAsync(client);

            await _clientRepo.SaveChangesAsync();

            return client;
        }

        private OrderResponse MapToOrderDto (Order order){

            var ordeResponse = new OrderResponse(
                OrderId:  order.OrderId,
                OrderNumber: order.OrderNumber,
                OrderDate: order.OrderDate,
                OrderStatus: order.Status,
                PaymentMethod: order.PaymentMethod,
                PaymentReceiptUrl: order.PaymentReceiptUrl,
                Client: MapClientToDto(order.Client),
                OrderDetails: order.OrderDetails.Select(od => MapOrderDetailToDto(od)).ToList()

            );

            return ordeResponse;
        }

        private ClientResponse MapClientToDto(Client client)
        {
            return  new ClientResponse(
                client.ClientId,
                Name: $"{client.FirstName} {client.LastName}",
                client.Address,
                client.Email,
                client.Phone
            );

        }

        private OrderDetailResponse MapOrderDetailToDto(OrderDetail detail)
        {
            return new OrderDetailResponse(
                OrderDetailId: detail.Id,
                Quantity: detail.Quantity,
                UnitPriceAtSale: detail.UnitPriceAtSale,
                Product: MapProductToDto(detail.Product)
            );
        }

    private ProductResponse MapProductToDto(Product product)
    {
        return new ProductResponse(
            product.ProductId,
            product.Name,
            product.NumberReference,
            product.Description,
            product.Stock,
            product.Price,
            product.Warranty,
            product.Categories.Select(c => 
                new CategoryResponse(c.CategoryId, c.Name, c.Description, c.IsActive)).ToList(),
            product.Links.Select(l => 
                new LinkResponse(l.LinkId, l.Url, l.Image)).ToList(),
            new SellerResponse(
                product.Seller.UserId,
                $"{product.Seller.FirstName} {product.Seller.LastName}",
                product.Seller.Phone,
                product.Seller.StoreName
            )
        );
    }
        private  OrderSummaryResponse MapToOrderSummaryDto(Order order)
        {
            return new OrderSummaryResponse(
                OrderId: order.OrderId,
                OrderNumber: order.OrderNumber,
                OrderDate: order.OrderDate,
                OrderStatus: order.Status,
                PaymentMethod: order.PaymentMethod,
                PaymentReceiptUrl: order.PaymentReceiptUrl,
                ClientResponse: MapClientToDto(order.Client)
            );
        }
    }
}