using LucidHR.Data;
using LucidHR.Models;
using LucidHR.Repository;
using LucidHR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static LucidHR.Models.Performance;

namespace LucidHR.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;
        private readonly ILogger<AdminController> _logger;


        //-----calandar --
        private readonly HrmsRepo repo;


        //-------offfer ----
        private readonly IOfferLetterApiClient _apiClient;

        public UserController(HrmsRepo repo, ApplicationDbContext context, ILogger<AdminController> logger, IOfferLetterApiClient apiClient)
        {
            _context = context;
            _logger = logger;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _client = new HttpClient(clientHandler);

            //--calandar --
            this.repo = repo;


            //-----offfer ---
            _apiClient = apiClient;

        }
        // GET: Tickets/Index (User portal to view all tickets)


        public IActionResult Index()
        {
            string email = HttpContext.Session.GetString("FromEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = _context.JoiningForm.FirstOrDefault(u => u.Uemail == email) ?? new JoiningForm { Uemail = email };
            //if (!string.IsNullOrEmpty(user.pphoto))
            //{
            //    user.pphoto = Url.Content(user.pphoto);
            //}

            ViewBag.Email = email;
            return View(user);
        }

        //--------------view tickets that any one send you-----------------------------------------------------
        //-------change by hasnain  
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


        //-----------------------raise ticket to any one-------------------------------------------------------------------------

        public IActionResult RaiseTicket()
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

        //--------------see your own solution--------------------------------

        public IActionResult GetTicket()
        {
            string loggedInUserName = HttpContext.Session.GetString("FromName");

            if (string.IsNullOrEmpty(loggedInUserName))
            {
                return RedirectToAction("Login", "Auth");
            }

            var tickets = _context.Tickets.Where(t => t.FromName == loggedInUserName).ToList();

            return View(tickets);
        }




        //---------------------------------------------calandar--------------------------------------
        public IActionResult Calendar()
        {
            var events = repo.GetEvents();
            return View(events);
        }



        //------------------------------------------------------Offer letter -----------------------------
        

       

        public IActionResult ViewOfferLetter()
        {
            return View();
        }

        public IActionResult DownloadOfferLetter()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadOfferLetter(string email)
        {
            var pdfBytes = await _apiClient.GetOfferLetterByEmailAsync(email);

            if (pdfBytes == null)
            {
                return NotFound();
            }

            return File(pdfBytes, "application/pdf", $"OfferLetter_{email}.pdf");
        }


        //-----------------------------------------performance-------------------------------------------------


        public IActionResult AnswerQuestions()
        {
            var uid = HttpContext.Session.GetInt32("uid");

            if (uid == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var latestQuestions = _context.Questions
                .Where(q => q.uid == uid && q.IsActive)
                .OrderByDescending(q => q.QuestionId)
                .Take(10)
                .ToList();

            var reviews = _context.PerformanceReviews
                .Include(r => r.Question)
                .Where(r => r.uid == uid && r.Question.IsActive)
                .OrderByDescending(r => r.Question.QuestionId)
                .Take(10)
                .ToList();

            var isFormSent = latestQuestions.Any();
            var isFormFilled = reviews.Count > 0;
            var message = !isFormSent ? "The form has not been sent to you by your manager. Contact him/her." : isFormFilled ? "You have already filled out the form." : null;

            ViewBag.Uid = uid;
            ViewBag.Reviews = reviews;
            ViewBag.Message = message;
            ViewBag.IsFormSent = isFormSent;
            ViewBag.IsFormFilled = isFormFilled;

            return View(latestQuestions);
        }

        public IActionResult SubmitAnswers(List<PerformanceReview> reviews)
        {
            // Retrieve uid from the session
            int? uid = HttpContext.Session.GetInt32("uid");

            if (uid == null)
            {
                // Handle the case where uid is not found in session
                TempData["ErrorMessage"] = "User session has expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            foreach (var review in reviews)
            {
                var existingReview = _context.PerformanceReviews
                    .FirstOrDefault(r => r.uid == uid && r.QuestionId == review.QuestionId);

                if (existingReview != null)
                {
                    existingReview.AnswerText = review.AnswerText;
                    _context.PerformanceReviews.Update(existingReview);
                }
                else
                {
                    review.uid = uid.Value; // Ensure uid is not nullable here
                    _context.PerformanceReviews.Add(review);
                }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Form filled successfully.";
            return RedirectToAction("Index");
        }

    }
}
