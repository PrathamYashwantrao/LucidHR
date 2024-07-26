using LucidHR.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using LucidHR.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static LucidHR.Models.Performance;
using System.Net.Mail;
using System.Net;

namespace LucidHR.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        //--adi
        private readonly string form16Url;
        private readonly string joiningFormUrl;
        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger, IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;

            _context = context;
            _logger = logger;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _client = new HttpClient(clientHandler);

            //--adi 
            //form16Url = "https://localhost:44383/api/f16";
            joiningFormUrl = "https://localhost:44383/api/joiningform";
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(userLogin u)
        {

            string url = "https://localhost:44383/api/Auth/Register";
            var jsondata = JsonConvert.SerializeObject(u);
            StringContent content = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        //public IActionResult Login()
        //{

        //    return View();
        //}



        //public IActionResult Register()
        //{
        //    return View();
        //}

        public IActionResult LockScreen()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Events()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Employees()
        {
            List<JoiningForm> employees = new List<JoiningForm>();
            string url = "https://localhost:44383/api/Auth/fetchEmployees";
            HttpResponseMessage response = _client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<JoiningForm>>(jsondata);
                if (obj != null)
                {
                    employees = obj;
                }
            }
            return View(employees);
        }

        public IActionResult LeaveRequests()
        {
            return View();
        }

        public IActionResult Tickets()
        {
            string loggedInUserName = HttpContext.Session.GetString("FromName");

            if (string.IsNullOrEmpty(loggedInUserName))
            {
                return RedirectToAction("Login", "Auth");
            }

            var tickets = _context.Tickets.Where(t => t.ToName == loggedInUserName).ToList();

            return View(tickets);
        }


        public IActionResult DelEmp(int id)
        {
            string url = $"https://localhost:44383/api/Auth/delEmpById/{id}";
            HttpResponseMessage response = _client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Employees", "Admin");
            }
            return View();
        }




        //------tickets section ----------------
        public IActionResult raiseticket()
        {
            ViewBag.Name = HttpContext.Session.GetString("FromName");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetNamesByRole(string role)
        {
            var url = $" https://localhost:44383/api/Auth/GetUsersByRole?role={role}";
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var names = await response.Content.ReadAsStringAsync();
                return Json(names); // Return the names as JSON
            }
            return Json(new List<string>());
        }
        public async Task<IActionResult> SubmitTicket([FromBody] TicketDetails ticketDetails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Example assuming FromName and FromRole are stored in session
                    string fromName = HttpContext.Session.GetString("FromName");
                    string fromRole = HttpContext.Session.GetString("FromRole");
                    string fromEmail = HttpContext.Session.GetString("FromEmail");

                    var ticket = new Ticket
                    {
                        FromName = fromName,
                        FromRole = fromRole,
                        FromEmail = fromEmail,
                        ToName = ticketDetails.Name,
                        ToRole = ticketDetails.Role,
                        TicketDetail = ticketDetails.TicketDetail,
                        solution = ticketDetails.solution ?? ""
                    };

                    _context.Tickets.Add(ticket);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Ticket submitted successfully!" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = $"Exception: {ex.Message}" });
                }
            }

            return BadRequest(new { message = "Invalid ticket details." });
        }
        //-------------------------------------------------get your solution---------------------------------------------------------
        public IActionResult getticket()
        {
            string loggedInUserName = HttpContext.Session.GetString("FromName");

            if (string.IsNullOrEmpty(loggedInUserName))
            {
                return RedirectToAction("Login", "Auth");
            }

            var tickets = _context.Tickets.Where(t => t.FromName == loggedInUserName).ToList();

            return View(tickets);
        }

        //--------------------------------------get view raise ticket by any other-----------------------------------------
        //public IActionResult viewticket()
        //{
        //    string loggedInUserName = HttpContext.Session.GetString("FromName");

        //    if (string.IsNullOrEmpty(loggedInUserName))
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }

        //    var tickets = _context.Tickets.Where(t => t.ToName == loggedInUserName).ToList();

        //    return View(tickets);
        //}


        [HttpPost]
        public IActionResult UpdateSolution(int id, string solution)
        {
            var ticket = _context.Tickets.Find(id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.solution = solution;
            ticket.status = "Completed";

            _context.SaveChanges();

            return Ok();
        }



        public IActionResult History() 
 {
     List<Ticket> tickets = _context.Tickets.ToList();
     return View(tickets);
 }




        //----------------------------------------------------------------adi-------------------------------------------
        public IActionResult AdminDashboard()
        {
            return View();
        }

        //public IActionResult ViewForm16()
        //{
        //    List<f16> form16List = null;

        //    HttpResponseMessage response = _client.GetAsync(form16Url).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        form16List = JsonConvert.DeserializeObject<List<f16>>(data);
        //    }

        //    if (form16List == null)
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }

        //    return View(form16List);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(f16 model, IFormFile attachment)
        {
            // Upload Attachment
            if (attachment != null && attachment.Length > 0)
            {
                string uploadUrl = "https://localhost:44383/api/f16/upload"; // Update this URL with the correct API endpoint

                using (var formDataContent = new MultipartFormDataContent())
                {
                    // Add attachment file
                    using (var stream = new MemoryStream())
                    {
                        await attachment.CopyToAsync(stream);
                        var fileContent = new ByteArrayContent(stream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                        formDataContent.Add(fileContent, "file", attachment.FileName);
                    }

                    // Send upload request
                    var httpClient = _httpClientFactory.CreateClient();
                    HttpResponseMessage uploadResponse = await httpClient.PostAsync(uploadUrl, formDataContent);
                    if (!uploadResponse.IsSuccessStatusCode)
                    {
                        // Handle upload failure
                        _logger.LogError("Failed to upload attachment");
                        ModelState.AddModelError("", "Failed to upload attachment.");
                        return View(model);
                    }

                    // Get the file name from the attachment
                    model.attachment = attachment.FileName;
                }
            }

            var json = JsonConvert.SerializeObject(model);
            _logger.LogInformation("Sending JSON payload to API: {JsonPayload}", json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.PostAsync("https://localhost:44383/api/f16", data);

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API response: {ResponseContent}", responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error response from API: {ResponseContent}", responseContent);
                    ModelState.AddModelError(string.Empty, $"Server error. Response: {responseContent}");
                    return View(model);
                }

                return RedirectToAction("Success", "F16");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while calling API");
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View(model);
        }



        public IActionResult ViewJoiningForm()
        {
            List<JoiningForm> joiningForms = null;

            HttpResponseMessage response = _client.GetAsync(joiningFormUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                joiningForms = JsonConvert.DeserializeObject<List<JoiningForm>>(data);
            }

            if (joiningForms == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(joiningForms);
        }






        //-------------------------------------------------------------performance-----------------------
        [HttpGet]
        public IActionResult SendQuestions()
        {
            var employees = _context.userLogin.Where(u => u.urole == "User").ToList();
            ViewBag.Employees = new SelectList(employees, "uid", "uname");
            return View(new userLogin());
        }

        [HttpPost]
        public IActionResult SendQuestions(int uid)
        {
            // Deactivate previous questions
            var previousQuestions = _context.Questions
                .Where(q => q.uid == uid && q.IsActive)
                .ToList();

            foreach (var question in previousQuestions)
            {
                question.IsActive = false;
                _context.Questions.Update(question);
            }

            // Add new questions
            var questions = new List<string>
    {
        "How do you rate your overall performance this quarter?",
        "What were your key achievements during this period?",
        "Did you meet your goals and objectives?",
        "How did you overcome challenges or obstacles in your work?",
        "How do you feel about your workload and the tasks assigned to you?",
        "In what areas do you think you need improvement?",
        "How do you plan to enhance your skills and knowledge?",
        "How well do you collaborate with your team members?",
        "Do you have any feedback for your manager or the company?",
        "What are your goals for the next quarter?"
    };

            foreach (var questionText in questions)
            {
                _context.Questions.Add(new Question
                {
                    uid = uid,
                    QuestionText = questionText,
                    IsActive = true
                });
            }

            _context.SaveChanges();

            var employee = _context.userLogin.FirstOrDefault(e => e.uid == uid);
            if (employee != null)
            {
                var recipientEmail = employee.uemail;
                var subject = "Performance Review Questions";
                var body = "Here are the questions for your performance review:\n\n" +
                           "Fill out by logging in on the HRMS Portal.\n\n" +
                           string.Join("\n", questions);

                SendEmail(recipientEmail, subject, body);
            }

            return RedirectToAction("SendQuestions");
        }

        public IActionResult ReviewAnswers(int uid)
        {
            var reviews = _context.PerformanceReviews
                .Include(r => r.Question)
                .Where(r => r.uid == uid)
                .OrderByDescending(r => r.Question.QuestionId)
                .Take(10)
                .ToList();

            var employees = _context.userLogin.Where(u => u.urole == "User").ToList();
            ViewBag.Employees = employees;
            ViewBag.SelectedUid = uid;

            return View(reviews);
        }



        //public IActionResult ReviewAnswers(int uid)
        //{
        //    var reviews = _context.PerformanceReviews
        //        .Include(r => r.Question)
        //        .Where(r => r.uid == uid && r.Question.IsActive)
        //        .OrderByDescending(r => r.Question.QuestionId)
        //        .Take(10)
        //        .ToList();

        //    var employees = _context.users.Where(u => u.urole == "User").ToList();
        //    ViewBag.Employees = employees;
        //    ViewBag.SelectedUid = uid;

        //    return View(reviews);
        //}



        [HttpPost]
        public IActionResult ProvideFeedback(int uid, string feedback)
        {
            var employee = _context.userLogin.FirstOrDefault(e => e.uid == uid);
            if (employee != null)
            {
                var recipientEmail = employee.uemail;
                var subject = "Feedback from Performance Review";
                var body = $"Dear {employee.uname},\n\n" +
                           $"Here is the feedback provided:\n\n" +
                           $"{feedback}\n\n" +
                           $"Regards,\n" +
                           $"Masstech";

                SendEmail(recipientEmail, subject, body);
            }

            //return RedirectToAction("Index");
            return RedirectToAction("ReviewAnswers");
        }

        private void SendEmail(string recipientEmail, string subject, string body)
        {
            var senderEmail = "kawalehritik07@gmail.com";
            var senderPassword = "ormccxsjlmwylngv";
            var smtpHost = "smtp.gmail.com";
            var smtpPort = 587;

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage(senderEmail, recipientEmail, subject, body);
                mailMessage.IsBodyHtml = false;

                try
                {
                    client.Send(mailMessage);
                    ViewBag.Message = "Email sent successfully.";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"Error sending email: {ex.Message}";
                }
            }
        }

    }
}
