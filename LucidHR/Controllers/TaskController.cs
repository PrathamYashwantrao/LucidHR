using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using LucidHR.Models;

namespace LucidHR.Controllers
{
    public class TaskController : Controller
    {
		public IActionResult Index()
		{
			return View();
		}
		private readonly IHttpClientFactory _httpClientFactory;



		private readonly HttpClient httpClient;
		private readonly IWebHostEnvironment env;


		public TaskController(HttpClient httpClient, IWebHostEnvironment env, IHttpClientFactory httpClientFactory)
		{
			this.httpClient = httpClient;
			this.httpClient.BaseAddress = new Uri("https://localhost:44383/");
			this.env = env;
			_httpClientFactory = httpClientFactory;
		}


		//trainer assign task

		public async Task<IActionResult> AssignTask()
		{
			var response = await httpClient.GetAsync("https://localhost:44383/batches");

			if (response.IsSuccessStatusCode)
			{
				var batches = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
				var viewModel = new AssignTaskViewModel
				{
					Batches = batches
				};
				return View(viewModel);
			}

			return View(new AssignTaskViewModel());
		}

		public void UploadFile(IFormFile file, string path)
		{
			FileStream stream = new FileStream(path, FileMode.Create);
			file.CopyTo(stream);
		}


		//trainer assign task
		[HttpPost]
		public async Task<IActionResult> AssignTask(AssignTaskViewModel model)
		{
			var path = env.WebRootPath;
			var filePath = "Content/Images/" + model.Attachment.FileName; // Placing the image
			var fullPath = Path.Combine(path, filePath);
			UploadFile(model.Attachment, fullPath);

			var newTask = new NewTask
			{
				Batch = model.SelectedBatch,
				Description = model.TaskDescription,
				Students = model.SelectedStudents,
				AttachmentPath = filePath,
				CDate = DateTime.UtcNow
			};

			var json = JsonConvert.SerializeObject(newTask);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await httpClient.PostAsync("https://localhost:44383/AddTask", content);

			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessMessage"] = "Task Added successfully.";
				return RedirectToAction("AssignTask");
			}

			ModelState.AddModelError(string.Empty, "Failed to assign task.");
			return View(model);
		}

		public async Task<List<UserDto>> GetUsersByBatchAsync(string batch)
		{
			HttpResponseMessage response = await httpClient.GetAsync($"GetUsersByBatch/{batch}");
			response.EnsureSuccessStatusCode();
			string responseBody = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<UserDto>>(responseBody);
		}


		[HttpGet]
		public async Task<JsonResult> GetUsersByBatch(string batch)
		{
			HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44383/GetUsersByBatch/{batch}");

			if (response.IsSuccessStatusCode)
			{
				var users = JsonConvert.DeserializeObject<List<UserDto>>(await response.Content.ReadAsStringAsync());
				return Json(users);
			}

			return Json(new List<UserDto>());
		}


		//submit task--trainee
		[HttpPost]
		public async Task<IActionResult> SubmitTask([FromForm] TaskSubmissionViewModel model)
		{
			try
			{
				if (model.File == null || model.File.Length == 0)
				{
					ModelState.AddModelError("File", "Please select a file to upload.");
					return BadRequest(new { Message = "Please select a file to upload." });
				}

				// Handle file upload
				var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
				if (!Directory.Exists(uploadDir))
				{
					Directory.CreateDirectory(uploadDir);
				}

				var fileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(model.File.FileName)}";
				var filePath = Path.Combine(uploadDir, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.File.CopyToAsync(stream);
				}

				if (model.LateReason == null)
				{
					model.LateReason = " ";
				}

				// Simulate API call to another endpoint
				using (var client = _httpClientFactory.CreateClient())
				{
					var apiEndpoint = "https://localhost:44383/SubmitTask"; // Replace with actual API endpoint
					var content = new MultipartFormDataContent();
					content.Add(new StreamContent(System.IO.File.OpenRead(filePath)), "File", fileName);
					content.Add(new StringContent(model.TaskId.ToString()), "TaskId");
					content.Add(new StringContent(model.CommitmentStatus.ToString()), "CommitmentStatus");
					content.Add(new StringContent(model.LateReason.ToString()), "LateReason");

					var response = await client.PostAsync(apiEndpoint, content);

					if (!response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();

						if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
						{
							return StatusCode(404, new { Message = "API endpoint not found." });
						}
						else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
						{
							return StatusCode(401, new { Message = "Unauthorized access to API." });
						}
						else
						{
							return StatusCode((int)response.StatusCode, new { Message = "500:Failed to submit task" });
						}
					}

					var responseBody = await response.Content.ReadAsStringAsync();

					return Ok(new { Message = "Task submitted successfully." });
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = $"Failed to submit task: {ex.Message}" });
			}
		}




		//displayt privious task for user--trainee
		public async Task<IActionResult> DisplayPreviousTask(string userName)
		{
			userName = HttpContext.Session.GetString("Username");

			var apiUrl = $"https://localhost:44383/GetPreviousTasksByName/{userName}";

			using (var client = _httpClientFactory.CreateClient())
			{
				var response = await client.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					var responseBody = await response.Content.ReadAsStringAsync();
					var tasks = JsonConvert.DeserializeObject<List<StudentTasksModel>>(responseBody);

					return View(tasks); // Pass tasks to view for display
				}
				else
				{
					// Handle unsuccessful API call
					var errorMessage = $"Failed to retrieve tasks for user '{userName}'.";
					// Optionally, handle different status codes (e.g., NotFound, Unauthorized)
					return BadRequest(errorMessage);
				}
			}
		}


		//update score view  for trainer
		public async Task<IActionResult> UpdateScore()
		{
			try
			{
				var apiUrl = "https://localhost:44383/GetAllSubmittedTasks"; // Replace with your actual API endpoint
				var response = await httpClient.GetAsync(apiUrl);

				response.EnsureSuccessStatusCode(); // Ensure response is successful

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


		//view submitted attachment to trainer

		[HttpGet]
		public IActionResult ViewAttachment(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return BadRequest("File path is required.");
			}

			var absolutePath = Path.Combine(env.WebRootPath, filePath.TrimStart('/'));

			if (!System.IO.File.Exists(absolutePath))
			{
				return NotFound("File not found.");
			}

			var fileExtension = Path.GetExtension(absolutePath).ToLowerInvariant();
			var mimeType = fileExtension switch
			{
				".jpg" or ".jpeg" => "image/jpeg",
				".png" => "image/png",
				".pdf" => "application/pdf",
				_ => "application/octet-stream",
			};

			return PhysicalFile(absolutePath, mimeType);
		}



		//update score on trainer side


		[HttpPost]
		public async Task<IActionResult> UpdateScoreForStudent(int taskId, int score)
		{
			using (var client = _httpClientFactory.CreateClient())
			{
				var apiEndpoint = $"https://localhost:44383/UpdateScore/{taskId}";
				var content = new StringContent(score.ToString(), Encoding.UTF8, "application/json");

				var response = await client.PostAsync(apiEndpoint, content);
				if (response.IsSuccessStatusCode)
				{
					// Handle success
					return RedirectToAction("UpdateScore");
				}
				else
				{
					// Handle error

					return StatusCode(500, new { Message = $"Failed to update score" });
				}
			}
		}
        public async Task<IActionResult> TopPerformers()
        {
            try
            {
                var response = await httpClient.GetAsync("https://localhost:44383/top-performers");
                response.EnsureSuccessStatusCode();

                var topPerformers = await response.Content.ReadFromJsonAsync<List<TopPerformerDto>>();

                var viewModel = new TopPerformersViewModel
                {
                    TopPerformers = topPerformers
                };

                return View(viewModel);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it appropriately
                // For example: _logger.LogError(ex, "An error occurred while fetching top performers.");
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For example: _logger.LogError(ex, "An unexpected error occurred.");
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }

    }
}
