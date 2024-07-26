using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Net.Http;
using LucidHR.Services;

namespace LucidHR.Controllers
{
    public class Employee2Controller : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly PayslipService service;
        public Employee2Controller(HttpClient httpClient,PayslipService s)
        {
            _httpClient = httpClient;
            this.service = s;
        }

        public async Task<IActionResult> GetPayslipByEmail()
        {
           string email = HttpContext.Session.GetString("FromEmail");
            
            var pdfFiles = await  service.GetPayslipByEmailAsync(email);
            if (pdfFiles != null)
            {
                // Optionally, you can do something with the pdfFiles list here
                // For example, pass it to a view
                return View(pdfFiles);
            }

            return NotFound();
        }
        public async Task<IActionResult> ViewPayslip(string payslipFileName)
        {
            // Call the API endpoint to get payslip content
            var response = await _httpClient.GetAsync($"https://localhost:44383/api/Payslip/GetPayslipContent?payslipFileName={payslipFileName}");

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                return File(fileBytes, "application/pdf"); // Adjust the content type based on your file type
            }
            else
            {
                return NotFound();
            }
        }


        public async Task<IActionResult> DownloadPayslip(string payslipFileName)
        {
           
            byte[] fileBytes = await FetchPayslipFileBytes(payslipFileName);
            if (fileBytes != null)
            {
                return File(fileBytes, "application/pdf", payslipFileName);
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<byte[]> FetchPayslipFileBytes(string payslipFileName)
        {
            
            var response = await _httpClient.GetAsync($"https://localhost:44383/api/Payslip/GetPayslipContent?payslipFileName={payslipFileName}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }
    }
}

