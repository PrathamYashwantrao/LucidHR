using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using LucidHR.Models;

namespace LucidHR.Controllers
{
    public class AcceptController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;



        private readonly HttpClient httpClient;
        private readonly IWebHostEnvironment env;


        public AcceptController(HttpClient httpClient, IWebHostEnvironment env, IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://localhost:44383/");
            this.env = env;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }
        // testing fo accept and reject

        public async Task<IActionResult> AcceptAndReject()
        {
            try
            {
                var apiUrl = "https://localhost:44383/api/AcceptTesting/GetAllSubmittedTasks";
                var response = await httpClient.GetAsync(apiUrl);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<List<UpdateScoreViewModel>>(responseBody);

                return View(tasks); // Pass tasks to view for display
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, new { Message = $"Failed to retrieve submitted tasks: {ex.Message}" });
            }
        }

        //      [HttpPost]
        //    public async Task<IActionResult> AcceptTask(int Id)
        //    {
        //        var response = await httpClient.PostAsync($"https://localhost:44382/api/AcceptTesting/AcceptTask/{Id}", null);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("AcceptAndReject"); // Redirect to your view after acceptance
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
        //        }

        //        return RedirectToAction("ErrorPage");

        //}

        //    [HttpPost]
        //    public async Task<IActionResult> RejectTask(int Id)
        //    {
        //        var response = await httpClient.PostAsync($"https://localhost:44382/api/AcceptTesting/RejectTask/{Id}", null);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("AcceptAndReject"); // Redirect to your view after rejection
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
        //        }

        //        return RedirectToAction("ErrorPage");
        //    }


        [HttpPost]
        public async Task<IActionResult> AcceptTask(int Id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync($"https://localhost:44383/api/AcceptTesting/AcceptTask/{Id}", null);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AcceptAndReject"); // Redirect to your view after acceptance
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to accept task. Please try again.");
                    return RedirectToAction("ErrorPage");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error accepting task: {ex.Message}");
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RejectTask(int Id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync($"https://localhost:44383/api/AcceptTesting/RejectTask/{Id}", null);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AcceptAndReject"); // Redirect to your view after rejection
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to reject task. Please try again.");
                    return RedirectToAction("ErrorPage");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error rejecting task: {ex.Message}");
                return RedirectToAction("ErrorPage");
            }
        }

        //update score view  for trainer
        [HttpPost]
        public async Task<IActionResult> UpdateScore(int taskId, int score)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var apiEndpoint = $"https://localhost:44383/api/AcceptTesting/UpdateScore/{taskId}";
                var content = new StringContent(score.ToString(), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiEndpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    return RedirectToAction(nameof(AcceptAndReject));
                }
                else
                {
                    // Handle error

                    return StatusCode(500, new { Message = $"Failed to update score" });
                }
            }


        }
    }
}
