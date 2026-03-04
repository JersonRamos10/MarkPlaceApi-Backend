using System.Security.Cryptography.X509Certificates;
using System.Text.Unicode;
using MarketPlaceApi.Domain.Enums;

namespace MarketPlaceApi.Domain.Entities
{
    public class Order
    {
        public Guid OrderId {get ;set; } = Guid.NewGuid();

        public required string OrderNumber {get; set;} 

        public DateTimeOffset OrderDate {get; set;} = DateTime.UtcNow; 

        public  string PaymentReceiptUrl {get; set;} = string.Empty;

        public  PaymentMethod PaymentMethod {get;set;}

        public OrderStatus Status {get; set;} 

        public Guid ClientId {get; set;} 

        public Client Client {get;set;} = null!;

        public Guid SellerId {get;set;}

        public Seller Seller {get; set;} = null!;

        public Invoice Invoice {get;set;} = null!;


        public ICollection<OrderDetail> OrderDetails {get; set;} = new HashSet<OrderDetail>();
    }
}