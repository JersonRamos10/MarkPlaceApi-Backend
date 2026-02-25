using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Domain.Entities
{
    public class Client
    {
        public Guid ClientId {get; set;} = Guid.NewGuid();

        public required string FirstName {get; set;}

        public required string LastName {get;set;}

        public required string Email {get; set;}

        public required string Dui {get; set;}

        public required string Address {get; set;}

        public required string Phone {get; set;}

        public ICollection<Order> Orders {get; set;} = new HashSet<Order>();
    }
}