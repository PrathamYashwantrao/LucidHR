using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using LucidHR.Models;

namespace LucidHR.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmployeeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetPayslipByEmail([FromBody] PayslipViewModel request)
        {
            var pdfBytes = await GeneratePayslipAsync(request.uemail, request.month, request.year);
            if (pdfBytes != null)
            {
                return File(pdfBytes, "application/pdf", $"Payslip_{request.uemail}.pdf");
            }
            return View("Error");
        }

        private async Task<byte[]> GeneratePayslipAsync(string email, string month, int year)
        {
            email = HttpContext.Session.GetString("FromEmail");
            // Ensure email, month, and year are correctly formatted and passed
            var requestUri = $"http://localhost:44383/api/Payslip/GetpayslipByemail?email={Uri.EscapeDataString(email)}&month={Uri.EscapeDataString(month)}&year={year}";

            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                // Return the byte array of the PDF content
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }

        public IActionResult ViewPayslip()
        {
            return View();
        }

        public async Task<IActionResult> DownloadPayslip(string email, string month, int year)
        {
            var pdfBytes = await GeneratePayslipAsync(email, month, year);
            if (pdfBytes != null)
            {
                return File(pdfBytes, "application/pdf", $"Payslip_{email}_{month}_{year}.pdf");
            }
            return View("Error");
        }
    }
}
