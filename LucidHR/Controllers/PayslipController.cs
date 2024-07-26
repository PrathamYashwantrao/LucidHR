using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using LucidHR.Models;
using LucidHR.Models;

namespace LucidHR.Controllers
{
    public class PayslipController : Controller
    {
        private readonly HttpClient httpClient;

        public PayslipController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GeneratePayslip(string uemail, string month, int year)
        {
            // Adjust parameters if needed
            //uemail = "swapnaliasjadhav@gmail.com"; // Corrected email
            //month = "May";
            //year = 2024;

            var requestUri = $"https://localhost:44383/api/Payslip/generate?email={Uri.EscapeDataString(uemail)}&month={Uri.EscapeDataString(month)}&year={year}";
                               
            try
            {
                var response = await httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Log or inspect the response
                    // Console.WriteLine("Response: " + jsonResponse);

                    var payslipData = JsonConvert.DeserializeObject<PayslipData>(jsonResponse);

                    if (payslipData != null)
                    {
                        return View(payslipData);
                    }
                    else
                    {
                        // Log or handle case where deserialization returns null
                        return View("Error"); // Or another appropriate error view
                    }
                }
                else
                {
                    // Log or handle response failure
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    // Console.WriteLine($"Error: {response.StatusCode}, {errorMessage}");
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception
                // Console.WriteLine($"Exception: {ex.Message}");
                return View("Error");
            }
        }



    }
}
