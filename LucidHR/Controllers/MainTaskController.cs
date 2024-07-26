using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using LucidHR.Models;
using System.Xml.Linq;

namespace LucidHR.Controllers
{
    public class MainTaskController : Controller
    {


		private readonly IHttpClientFactory _clientFactory;

		public MainTaskController(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> Register(UserRegistrationViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var registrationDto = new
				{
					Username = model.Username,
					Password = model.Password,
					ConfirmPassword = model.ConfirmPassword,
					Batch = model.Batch,
					Email = model.Email,
					FullName = model.FullName
				};

				var apiUrl = "https://localhost:44383/api/Task/register"; // Replace with your API URL
				var client = _clientFactory.CreateClient();

				var json = JsonSerializer.Serialize(registrationDto);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await client.PostAsync(apiUrl, content);

				if (response.IsSuccessStatusCode)
				{
					ViewBag.Message = "Registration successful.";
					return RedirectToAction("Login");
				}
				else
				{
					var errorMessage = await response.Content.ReadAsStringAsync();
					ModelState.AddModelError(string.Empty, $"Failed to register: {errorMessage}");
					return View(model);
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, $"Failed to register: {ex.Message}");
				return View(model);
			}
		}

		//Login Section


		public async Task<IActionResult> Login()
		{
			return View();

		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModelTask model)
		{
			var apiUrl = "https://localhost:44383/api/Task/login";
			var client = _clientFactory.CreateClient();

			var json = JsonSerializer.Serialize(model);
			var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

			var response = await client.PostAsync(apiUrl, content);


			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				var loginResponse = JsonSerializer.Deserialize<LoginResponseViewModel>(responseContent, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});





				if (loginResponse.Role.Equals("trainee", StringComparison.OrdinalIgnoreCase))
				{


					// Store username in session from LoginViewModel
					HttpContext.Session.SetString("Username", model.Username);


					// Redirect to trainee dashboard
					return RedirectToAction("TraineeDashboard");
				}
				else if (loginResponse.Role.Equals("trainer", StringComparison.OrdinalIgnoreCase))
				{


					// Store username in session from LoginViewModel
					HttpContext.Session.SetString("Username", model.Username);


					// Redirect to trainer dashboard
					return RedirectToAction("AssignTask", "Task");



				}
				else
				{
					// Handle other roles or scenarios
					return RedirectToAction("Index");
				}
			}
			else
			{
				// Handle unsuccessful login
				return View("LoginError");
			}

		}


		// trainer dashboard
		public IActionResult TrainerDashboard()
		{
			return View();
		}

		//Trainee dashboard
		public async Task<IActionResult> TraineeDashboard()
		{
			// Retrieve the username from the session
			string name = HttpContext.Session.GetString("Username");
			if (string.IsNullOrEmpty(name))
			{
				// Handle the case where no name is provided
				return View("Error");
			}

			var client = _clientFactory.CreateClient();

			// Replace with your API endpoint URL
			var apiUrl = $"https://localhost:44383/api/AcceptTesting/GetStudentTasksByName/{name}";

			try
			{
				var response = await client.GetAsync(apiUrl);
				if (response.IsSuccessStatusCode)
				{
					var stream = await response.Content.ReadAsStreamAsync();
					var studentTasks = await JsonSerializer.DeserializeAsync<StudentTasksModel[]>(stream, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					});

					/*var responseContent = await response.Content.ReadAsStringAsync();
                    var studentTasks = JsonSerializer.Deserialize<StudentTasksModel[]>(responseContent);
*/

					return View(studentTasks);
				}


				else
				{
					// Handle unsuccessful API response by returning an empty array
					return View(Array.Empty<StudentTasksModel>());
				}
			}
			catch (HttpRequestException ex)
			{
				// Handle HTTP request exception
				return View("Error");
			}

		}
		public IActionResult LoginError()
		{
			return View();
		}





		//trainee  -->Tasks display
		public async Task<IActionResult> TaskForUser(string name)
		{
			// Retrieve the username from the session
			name = HttpContext.Session.GetString("Username");
			if (string.IsNullOrEmpty(name))
			{
				// Handle the case where no name is provided
				return View("Error");
			}

			var client = _clientFactory.CreateClient();

			// Replace with your API endpoint URL
			var apiUrl = $"https://localhost:44383/GetStudentTasksByName/{name}";

			try
			{
				var response = await client.GetAsync(apiUrl);


				if (response.IsSuccessStatusCode)
				{
					var stream = await response.Content.ReadAsStreamAsync();
					var studentTasks = await JsonSerializer.DeserializeAsync<StudentTasksModel[]>(stream, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					});


					return View(studentTasks);
				}


				else
				{
					// Handle unsuccessful API response by returning an empty array
					return View(Array.Empty<StudentTasksModel>());
				}
			}
			catch (HttpRequestException ex)
			{
				// Handle HTTP request exception
				return View("Error");
			}

		}

		public IActionResult Logout()
		{
			return RedirectToAction(nameof(Login));
		}
	}
}
