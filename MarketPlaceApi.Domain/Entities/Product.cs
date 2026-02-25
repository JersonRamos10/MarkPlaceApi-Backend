
namespace MarketPlaceApi.Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public required string Name {get; set;}

        public string Description {get; set;} = string.Empty;
        public string NumberReference {get;set;}= string.Empty;

        public required decimal Price {get; set;}

        public bool Warranty {get; set;} 

        public required int Stock {get; set;}

        public bool IsActive { get; set; } = true;

        //propertys navigation

        public Guid SellerId {get; set;}

        public Seller Seller {get; set; }  = null!;

        public ICollection<Link> Links {get; set;} = new List<Link> ();
        public ICollection<Category> Categories {get; set;} = new List<Category> ();

        public ICollection<OrderDetail> OrderDetails {get; set;} = new HashSet<OrderDetail> ();
    
    }
}