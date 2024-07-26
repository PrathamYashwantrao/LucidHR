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
    public class JoiningFormController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<JoiningFormController> _logger;

        public JoiningFormController(IHttpClientFactory httpClientFactory, ILogger<JoiningFormController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JoiningForm model, IFormFile aadharAttachment, IFormFile panAttachment, IFormFile pphoto)
        {
            // Upload Aadhar Attachment
            if (aadharAttachment != null && aadharAttachment.Length > 0)
            {
                string uploadUrl = "https://localhost:44383/api/Joiningform/upload"; // Update this URL with the correct API endpoint

                using (var formDataContent = new MultipartFormDataContent())
                {
                    // Add aadharAttachment file
                    using (var stream = new MemoryStream())
                    {
                        await aadharAttachment.CopyToAsync(stream);
                        var fileContent = new ByteArrayContent(stream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(aadharAttachment.ContentType);
                        formDataContent.Add(fileContent, "file", aadharAttachment.FileName);
                    }

                    // Send upload request
                    var httpClient = _httpClientFactory.CreateClient();
                    HttpResponseMessage uploadResponse = await httpClient.PostAsync(uploadUrl, formDataContent);
                    if (!uploadResponse.IsSuccessStatusCode)
                    {
                        // Handle upload failure
                        _logger.LogError("Failed to upload Aadhar attachment");
                        ModelState.AddModelError("", "Failed to upload Aadhar attachment.");
                        return View(model);
                    }

                    // Store the file name in the model
                    model.AadharAttachment = aadharAttachment.FileName;
                }
            }

            // Upload PAN Attachment
            if (panAttachment != null && panAttachment.Length > 0)
            {
                string uploadUrl = "https://localhost:44383/api/Joiningform/upload"; // Update this URL with the correct API endpoint

                using (var formDataContent = new MultipartFormDataContent())
                {
                    // Add panAttachment file
                    using (var stream = new MemoryStream())
                    {
                        await panAttachment.CopyToAsync(stream);
                        var fileContent = new ByteArrayContent(stream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(panAttachment.ContentType);
                        formDataContent.Add(fileContent, "file", panAttachment.FileName);
                    }

                    // Send upload request
                    var httpClient = _httpClientFactory.CreateClient();
                    HttpResponseMessage uploadResponse = await httpClient.PostAsync(uploadUrl, formDataContent);
                    if (!uploadResponse.IsSuccessStatusCode)
                    {
                        // Handle upload failure
                        _logger.LogError("Failed to upload PAN attachment");
                        ModelState.AddModelError("", "Failed to upload PAN attachment.");
                        return View(model);
                    }

                    // Store the file name in the model
                    model.PanAttachment = panAttachment.FileName;
                }
            }

            // Upload Photo
            if (pphoto != null && pphoto.Length > 0)
            {
                string uploadUrl = "https://localhost:44383/api/Joiningform/upload"; // Update this URL with the correct API endpoint

                using (var formDataContent = new MultipartFormDataContent())
                {
                    // Add photo file
                    using (var stream = new MemoryStream())
                    {
                        await pphoto.CopyToAsync(stream);
                        var fileContent = new ByteArrayContent(stream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(pphoto.ContentType);
                        formDataContent.Add(fileContent, "file", pphoto.FileName);
                    }

                    // Send upload request
                    var httpClient = _httpClientFactory.CreateClient();
                    HttpResponseMessage uploadResponse = await httpClient.PostAsync(uploadUrl, formDataContent);
                    if (!uploadResponse.IsSuccessStatusCode)
                    {
                        // Handle upload failure
                        _logger.LogError("Failed to upload photo");
                        ModelState.AddModelError("", "Failed to upload photo.");
                        return View(model);
                    }

                    // Store the file name in the model
                    model.pphoto = pphoto.FileName;
                }
            }

            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.PostAsync("https://localhost:44383/api/Joiningform/join", data);
                                                           

                if (response.IsSuccessStatusCode)
                {
                    //return RedirectToAction("Success", "JoiningForm");
                    TempData["success"] = "Joining Form Submitted Successfully";
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error response from API: {ResponseContent}", responseContent);
                    ModelState.AddModelError(string.Empty, $"Server error. Response: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while calling API");
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
