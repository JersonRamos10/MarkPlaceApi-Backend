
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Azure.Messaging;
using MarketPlaceApi.Business.DTOs.Clients;
using MarketPlaceApi.Business.DTOs.Order;
using MarketPlaceApi.Business.DTOs.OrderDetail;
using MarketPlaceApi.Business.DTOs.Products;
using MarketPlaceApi.Business.Exceptions;
using MarketPlaceApi.Data.Migrations;
using MarketPlaceApi.Data.Repositories;
using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Domain.Enums;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
namespace MarketPlaceApi.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo; 
        private readonly IClientRepository _clientRepo; 

        private readonly IProductRepository _prodRepo;


        private readonly ISellerRepository _sellerRepo;

        public OrderService (IOrderRepository orderRepo , 
        IClientRepository clientRepo,
        IProductRepository prodRepo,
        ISellerRepository sellerRepo){
            _orderRepo = orderRepo;
            _clientRepo = clientRepo;
            _prodRepo = prodRepo;
            _sellerRepo = sellerRepo;
            
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
                Client = client,
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
            return MapToOrderDto(order);
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
            var order = await _orderRepo.GetByIdAsync(orderId) 
                    ?? throw new NotFoundException("Order not found");

            order.Status = status;

            _orderRepo.Update(order);

            await _orderRepo.SaveChangesAsync();

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
            throw new NotImplementedException();
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