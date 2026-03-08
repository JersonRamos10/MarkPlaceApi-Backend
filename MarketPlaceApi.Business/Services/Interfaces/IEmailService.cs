namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderStatusAsync(string toEmail, string toName, string subject, string message);
        Task SendInvoiceAsync(string toEmail, string toName, byte[] pdfBytes);
    }
}