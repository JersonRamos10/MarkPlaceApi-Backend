using MarketPlaceApi.Business.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace MarketPlaceApi.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration config)
        {
            _apiKey    = config["SendGrid:ApiKey"]!;
            _fromEmail = config["SendGrid:FromEmail"]!;
            _fromName  = config["SendGrid:FromName"]!;
        }

        public async Task SendOrderStatusAsync(string toEmail, string toName, string subject, string message)
        {
            var client = new SendGridClient(_apiKey);

            var emailMessage = new SendGridMessage
            {
                From = new EmailAddress(_fromEmail, _fromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = $"<p>{message}</p>"
            };

            emailMessage.AddTo(new EmailAddress(toEmail, toName));

            var response = await client.SendEmailAsync(emailMessage);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Body.ReadAsStringAsync();
                throw new Exception($"Error enviando email: {errorBody}");
            }
        }
        
        public async Task SendInvoiceAsync(string toEmail, string toName, byte[] pdfBytes)
        {
            var client = new SendGridClient(_apiKey);

            var message = new SendGridMessage
            {
                From = new EmailAddress(_fromEmail, _fromName),
                Subject = "Tu factura de compra - MarketPlace",
                PlainTextContent = "Gracias por tu compra. Adjuntamos tu factura.",
                HtmlContent = "<p>Gracias por tu compra. Adjuntamos tu <strong>factura</strong>.</p>"
            };

            message.AddTo(new EmailAddress(toEmail, toName));

            message.AddAttachment(
                "factura.pdf",
                Convert.ToBase64String(pdfBytes),
                "application/pdf"
            );

            var response = await client.SendEmailAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Body.ReadAsStringAsync();
                throw new Exception($"Error enviando email: {errorBody}");
            }
        }
    }
}