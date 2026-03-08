using MarketPlaceApi.Business.Services.Interfaces;
using MarketPlaceApi.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace MarketPlaceApi.Business.Services
{
    public class PdfService: IPdfService
    {
        public byte[] GenerateInvoicePdf(Invoice invoice, Order order)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    // Header
                    page.Header().Column(col =>
                    {
                        col.Item().Text(order.Seller.StoreName)
                            .FontSize(20).Bold();
                        col.Item().Text($"Factura: {invoice.InvoiceNumber}");
                        col.Item().Text($"Fecha: {invoice.IssueDate:dd/MM/yyyy}");
                    });

                    // Content
                    page.Content().Column(col =>
                    {
                        // Datos del cliente
                        col.Item().Text("Datos del Cliente").Bold();
                        col.Item().Text($"Nombre: {order.Client.FirstName} {order.Client.LastName}");
                        col.Item().Text($"Email: {order.Client.Email}");
                        col.Item().Text($"DUI: {order.Client.Dui}");

                        // Tabla de productos
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3); // Producto
                                cols.RelativeColumn();  // Cantidad
                                cols.RelativeColumn();  // Precio
                                cols.RelativeColumn();  // Subtotal
                            });

                            // Headers de la tabla
                            table.Header(header =>
                            {
                                header.Cell().Text("Producto").Bold();
                                header.Cell().Text("Cant.").Bold();
                                header.Cell().Text("Precio").Bold();
                                header.Cell().Text("Subtotal").Bold();
                            });

                            // Filas de productos
                            foreach (var detail in order.OrderDetails)
                            {
                                table.Cell().Text(detail.Product.Name);
                                table.Cell().Text(detail.Quantity.ToString());
                                table.Cell().Text($"${detail.UnitPriceAtSale}");
                                table.Cell().Text($"${detail.UnitPriceAtSale * detail.Quantity}");
                            }
                        });

                        // Totales
                        col.Item().Text($"SubTotal: ${invoice.SubTotal}");
                        col.Item().Text($"IVA 13%: ${invoice.TaxTotal}");
                        col.Item().Text($"Total: ${invoice.GranTotal}").Bold();
                    });

                    // Footer
                    page.Footer().Text("Gracias por su compra.").FontSize(10);
                });
            })
            .GeneratePdf();
        }
    }
}