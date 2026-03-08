namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendInvoiceAsync(string toEmail, string toName, byte[] pdfBytes);
    }
}