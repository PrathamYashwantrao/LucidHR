using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Logging;
using System;

namespace LucidHR.Services
{
    public class PdfService
    {
        private readonly ILogger<PdfService> _logger;
        private readonly IConverter _converter;

        public PdfService(ILogger<PdfService> logger, IConverter converter)
        {
            _logger = logger;
            _converter = converter;
        }

        public byte[] ConvertHtmlToPdf(string htmlContent)
        {
            try
            {
                _logger.LogInformation("Starting HTML to PDF conversion");

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10, Bottom = 10 }
                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = null }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                byte[] pdfBytes = _converter.Convert(pdf);

                _logger.LogInformation("PDF conversion completed");
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during HTML to PDF conversion");
                throw;
            }
        }
    }
}