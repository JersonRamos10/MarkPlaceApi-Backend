namespace MarketPlaceApi.Domain.Entities
{
    public class Invoice
    {
        public int Id {get;set;}

        public required string InvoiceNumber {get;set;}  

        public DateTimeOffset IssueDate{get;set;} = DateTime.UtcNow;

        public decimal GranTotal {get;set;}

        public decimal TaxTotal {get;set;}

        public Guid OrderId {get; set;}

        public Order Order {get; set;} = null!;
    }
}