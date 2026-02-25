using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Domain.Entities
{
    public class OrderDetail
    {
        public Guid Id {get; set;} = Guid.NewGuid();

        public decimal UnitPriceAtSale {get; set;}

        public int Quantity {get; set;}

        public Guid OrderId {get;set;}

        public Order Order {get; set;} = null!;

        public Guid ProductId {get; set;}

        public Product Product {get; set;} = null!;

    }
}