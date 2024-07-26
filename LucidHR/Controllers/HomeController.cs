using LucidHR.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using LucidHR.Data;

namespace LucidHR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string email = HttpContext.Session.GetString("email");
            if (string.IsNullOrEmpty(email))
            {
                // Handle the case where the session data does not exist
                // For example, redirect to the login page
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Email = email;
            ///
            var combinedModel = new Combined
            {
                details = _context.JoiningForm.ToList(),
                leavess = _context.LeaveRequest.ToList(),
                ticket = _context.Tickets.ToList(),
                events = _context.Events.ToList()
            };

            // ViewBag.Email = "user@example.com"; // Example email, replace with actual data

            return View(combinedModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
