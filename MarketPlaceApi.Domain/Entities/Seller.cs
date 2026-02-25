using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Domain.Entities
{
    public class Seller
    {
        public Guid SellerId {get; set;}

        public  string? FirstName {get; set;} 

        public  string? LastName {get; set;}

        public string? StoreName {get;set;}
        public required string Phone {get; set;}

        public string Direction {get; set;} = string.Empty;

        public bool IsActive { get; set; } = true; 

        //propertys navigation
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<BankAccount> BankAccounts {get; set;} = new HashSet<BankAccount> ();

        public ICollection<Product> Products {get; set;} = new List<Product>();
    }
}