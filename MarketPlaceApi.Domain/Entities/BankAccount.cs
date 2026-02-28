namespace MarketPlaceApi.Domain.Entities
{
    public class BankAccount
    {
        public int Id {get; set;}

        public required string NumberAccount {get; set;}

        public required string Type {get; set;}

        public bool IsActive {get;set;} = true;

        public Guid SellerId {get; set;}

        public Seller Seller {get; set;} = null!;
    }
} 