namespace MarketPlaceApi.Domain.Entities
{
    public class  User
    {
        public Guid UserId {get; set;} = Guid.NewGuid();

        public required string UserName {get; set;}

        public required string Password {get; set;}

        public required string Email {get; set;}

        public Seller Seller {get; set; } = null!; 
    }
}