using System.Net.Http.Headers;

namespace MarketPlaceApi.Domain.Entities
{
    public class Category
    {
        public int CategoryId {get;set;}

        public required string Name {get;set;} 
        public string Description {get; set;} = string.Empty; 

        public bool IsActive {get; set;} 

        public ICollection<Product> Products {get; set;} = new List<Product>();
    }
}