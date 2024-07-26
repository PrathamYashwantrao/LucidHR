using LucidHR.Models;
using LucidHR.Services;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace LucidHR.Controllers
{
    public class OfferLetterController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IOfferLetterApiClient _apiClient;
        private readonly PdfService _pdfService;
        private readonly ILogger<OfferLetterController> _logger;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly HttpClient _httpClient;

        public OfferLetterController(IEmailService emailService, IOfferLetterApiClient apiClient,
            PdfService pdfService, ILogger<OfferLetterController> logger, ICompositeViewEngine viewEngine, HttpClient httpClient)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            httpClient = new HttpClient(clientHandler);

            _httpClient = httpClient;

            _emailService = emailService;
            _apiClient = apiClient;
            _pdfService = pdfService;
            _logger = logger;
            _viewEngine = viewEngine;
        }

        public IActionResult Generate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Generate(OfferLetterViewModel model)
        {
            _logger.LogInformation($"Received model: {JsonSerializer.Serialize(model)}");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError($"ModelState Error: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }

            try
            {
                string viewName = model.Role == "Trainee" ? "OfferLetter" : "OfferLetterOther";
                _logger.LogInformation($"Rendering view: {viewName}");

                string htmlContent = await RenderViewToStringAsync(viewName, model);
                _logger.LogInformation("HTML content generated successfully");

                byte[] pdfBytes = _pdfService.ConvertHtmlToPdf(htmlContent);
                _logger.LogInformation("PDF converted successfully");

                await _emailService.SendEmailWithAttachmentAsync(model.Email, "Your Offer Letter",
                    "Please find your offer letter attached.", pdfBytes, "OfferLetter.pdf");
                _logger.LogInformation("Email sent successfully");

                //await _apiClient.StorePdfAsync(model.Email, pdfBytes);
                //_logger.LogInformation("PDF stored successfully");

                // Map ViewModel to OfferLetter Entity Model
                var offerLetter = new OfferLetter
                {
                    Role = model.Role,
                    Name = model.Name,
                    Email = model.Email,
                    DateOfJoining = model.DateOfJoining,
                    Salary = model.Salary,
                    Designation = model.Designation,
                    Hra = model.Hra,
                    TravelAllowance = model.TravelAllowance,
                    Bonus = model.Bonus,
                    SpecialAllowance = model.SpecialAllowance,
                    Medical = model.Medical,
                    Pf = model.Pf,
                    BasicSalary = model.BasicSalary,
                    ProfessionalTax = model.ProfessionalTax,
                    Tds = model.Tds,
                    OfferLetterPdf = pdfBytes

                };

                // Send OfferLetter data to HrmsApi
                var response = await _httpClient.PostAsJsonAsync("https://localhost:44383/api/OfferLetter", offerLetter);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Offer letter stored in HrmsApi successfully");
                }
                else
                {
                    _logger.LogError("Failed to store offer letter in HrmsApi");
                }

                return File(pdfBytes, "application/pdf", "OfferLetter.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the offer letter");
                ModelState.AddModelError("", "An error occurred while generating the offer letter. Please try again.");
            }

            return View(model);
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;
            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
