using System.Reflection;

namespace  MarketPlaceApi.Domain.Entities
{
    public class Link
    {
        public int LinkId {get; set;}

        public string Image {get; set;} = string.Empty;

        public string Url {get; set; } = string.Empty;

        public bool IsActive {get;set;}

        public Guid? ProductId {get; set; } 

        public Product Product {get; set;} = null!;

    }
}