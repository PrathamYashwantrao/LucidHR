using LucidHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace LucidHR.Controllers
{
    public class F16Controller : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<F16Controller> _logger;

        private readonly string form16Url;

        public F16Controller(IHttpClientFactory httpClientFactory, ILogger<F16Controller> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _client = new HttpClient(clientHandler);

            form16Url = "https://localhost:44383/api/f16";
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(f16 model, IFormFile attachment)
        //{
        //    // Upload Attachment
        //    if (attachment != null && attachment.Length > 0)
        //    {
        //        string uploadUrl = "https://localhost:44383/api/f16/upload"; // Update this URL with the correct API endpoint

        //        using (var formDataContent = new MultipartFormDataContent())
        //        {
        //            // Add attachment file
        //            using (var stream = new MemoryStream())
        //            {
        //                await attachment.CopyToAsync(stream);
        //                var fileContent = new ByteArrayContent(stream.ToArray());
        //                fileContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
        //                formDataContent.Add(fileContent, "file", attachment.FileName);
        //            }

        //            // Send upload request
        //            var httpClient = _httpClientFactory.CreateClient();
        //            HttpResponseMessage uploadResponse = await httpClient.PostAsync(uploadUrl, formDataContent);
        //            if (!uploadResponse.IsSuccessStatusCode)
        //            {
        //                // Handle upload failure
        //                _logger.LogError("Failed to upload attachment");
        //                ModelState.AddModelError("", "Failed to upload attachment.");
        //                return View(model);
        //            }

        //            // Get the file name from the attachment
        //            model.attachment = attachment.FileName;
        //        }
        //    }

        //    var json = JsonConvert.SerializeObject(model);
        //    _logger.LogInformation("Sending JSON payload to API: {JsonPayload}", json);
        //    var data = new StringContent(json, Encoding.UTF8, "application/json");

        //    try
        //    {
        //        var httpClient = _httpClientFactory.CreateClient();
        //        var response = await httpClient.PostAsync("https://localhost:44383/api/f16", data);

        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        _logger.LogInformation("API response: {ResponseContent}", responseContent);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            _logger.LogError("Error response from API: {ResponseContent}", responseContent);
        //            ModelState.AddModelError(string.Empty, $"Server error. Response: {responseContent}");
        //            return View(model);
        //        }

        //        return RedirectToAction("Success", "F16");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Exception while calling API");
        //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
        //    }

        //    return View(model);
        //}




        public IActionResult Success()
        {
            return View();
        }


        public IActionResult ViewForm16()
        {
            List<f16> form16List = null;

            string userEmail = HttpContext.Session.GetString("FromEmail");

            HttpResponseMessage response = _client.GetAsync(form16Url).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                form16List = JsonConvert.DeserializeObject<List<f16>>(data);
            }

            if (form16List == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var userForm16List = form16List.Where(f => f.email == userEmail).ToList();

            return View(userForm16List);
        }

    }
}