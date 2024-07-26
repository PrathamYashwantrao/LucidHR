using com.itextpdf.text.pdf;
using LucidHR.Data;
using LucidHR.Models;
using LucidHR.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace LucidHR.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly EmailSendController _emailService;




        private readonly HrmsRepo repo;
        public DashboardController(HrmsRepo repo, ApplicationDbContext db, EmailSendController emailService)
        {
            _emailService = emailService;
            this.repo = repo;
            this.db = db;
        }
        public IActionResult AllEvent()
        {

            List<Event> events = new List<Event>();
            try
            {
                events = repo.GetEvents();
            }
            catch (Exception ex)
            {

                ViewData["ErrorMessage"] = "Failed to fetch events.";
            }
            return View(events);
        }

        public IActionResult AddEvent()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(Event @event)
        {
            if (@event.Type == "Holiday")
            {
                @event.CustomColor = "Red";
            }
            else
            {
                @event.CustomColor = "rgb(135 103 191)";
                // Compose the email body
                string emailBody = $@"
            <p>Dear User,</p>
            <p>An event has been Organized in Our company.</p>
            <p><strong>Event Details:</strong></p>
            <ul>
                <li>Title: {@event.Title}</li>
                <li>Start Date: {@event.StartDate}</li>
                <li>End Date: {@event.EndDate}</li>
                <li>Type: {@event.Type}</li>
            </ul>
            <p>Thank you.</p>
        ";
                //list of mail
                List<userLogin> users = db.userLogin.Where(t => t.urole != "Admin").ToList();
                List<string> emails = new List<string>();    
                foreach(var user in  users)
                {
                    emails.Add(user.uemail); 
                }
                // Send the email
                await _emailService.SendEmailAsync(emails, "Event Notification", emailBody);

                //return Ok(); // Return appropriate response


            }
            if (ModelState.IsValid)
            {
                try
                {
                    repo.AddEvent(@event);
                    TempData["Message"] = "Event added successfully!";
                    return RedirectToAction(nameof(AllEvent));
                }
                catch (Exception ex)
                {

                    ViewData["ErrorMessage"] = "Failed to add event.";
                    return View();
                }
            }
            return View(@event);
        }

      

        public IActionResult DeleteEvent(int id)
        {
            try
            {
                repo.DeleteEvent(id);
                TempData["Message"] = "Event deleted successfully!";
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "Failed to delete event.";
            }
            return RedirectToAction(nameof(AllEvent));
        }




        public IActionResult EditEvent(int id)
        {
            var @event = repo.GetEventById(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEvent(Event @event)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repo.UpdateEvent(@event);
                    TempData["Message"] = "Event updated successfully!";
                    return RedirectToAction(nameof(AllEvent));
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = "Failed to update event.";
                    return View(@event);
                }
            }
            return View(@event);
        }





        //Employee Implementation



        //public IActionResult Calendar()
        //{
        //    var events = repo.GetEvents();
        //    return View(events);
        //}

        //// GET: Events/GetEvents
        //[HttpGet]
        public IActionResult GetEvents()
        {
           
            var events = db.Events.Select(e => new
            {
                //id = e.Id,
                title = e.Title,
                start = e.StartDate,  // Adjust according to your Event model
                end = e.EndDate.AddDays(1) ,     // Adjust according to your Event model ///////AddDay(),
                extendedProps = new { customColor = e.CustomColor }
            }).ToList();

            return Json(events);
        }
        public IActionResult Index()
        {

           return View();
        }

        public IActionResult EmployeeDashboard()
        {
            return View();
        }
        public IActionResult HrDashboard()
        {
            return View();
        }

        //public IActionResult EmployeeInfo()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult FetchEmployee(string employeeNo)
        //{
        //    var employee = repo.GetEmployeeByNo(employeeNo);
        //    if (employee == null)
        //    {
        //        ViewData["ErrorMessage"] = "Employee not found.";
        //        return View("EmployeeInfo");
        //    }
        //    return View("EmployeeInfo", employee);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult GeneratePayslip(string employeeNo, int absentDays,string payMonth)
        //{
        //    try
        //    {
        //        var payslip = repo.GeneratePayslip(employeeNo, DateTime.Now.AddMonths(-1), DateTime.Now, absentDays,payMonth);
       
        //        TempData["PayslipGenerated"] = "Payslip generated successfully!";
        //        return View("PayslipDetails", payslip); // Ensure payslip is passed to view
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["ErrorMessage"] = ex.Message;
        //        return View("EmployeeInfo");
        //    }
        //}
       //public IActionResult PayslipDetails()
       // {
       //     return View();
       // }


        // Action to download payslip as PDF
        //public IActionResult DownloadPayslipPdf(int payslipId)
        //{
        //    var payslip = repo.GetPayslipById(payslipId);
        //    if (payslip == null)
        //    {
        //        return NotFound();
        //    }

        //    // Render the PDF view as a PDF file and return
        //    var pdf = new ViewAsPdf("PayslipDetails", payslip); // 'PayslipDetails' is the name of your Razor view
        //    return pdf;
        //}



        //public IActionResult DetailsForOfferletter()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DetailsForOfferletter(OfferLetter obj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //       repo.DetailsforLetter(obj);
        //        TempData["msg"] = "Data Added Successfully!!";
        //        return RedirectToAction("GenerateOfferLetter",obj);
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}

        //[HttpGet]
        //public IActionResult GenerateOfferLetter()
        //{
        //    var model = new OfferLetter();
        //    return View(model);  // Pass the model to the view

        //}
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //    public IActionResult GenerateOfferLetter(OfferLetter model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //        ViewBag.Message = TempData["msg"];  // Pass TempData["msg"] to ViewBag for display
        //                                            // Process the data if needed
        //        return View("DetailsForOfferletter", model);
        //        }

        //        // If the model is not valid, redisplay the form with validation errors
        //        return View(model);
        //    }





        //public IActionResult ShowPayslip()
        //{
        //    var employeeNo = User.Identity.Name; // Assuming the employee number is the user identity
        //    var payslips = repo.GetPayslipsByEmployeeNo(employeeNo);
        //    return View(payslips);
        //}

        //public IActionResult DownloadPayslip(int id)
        //{
        //    var payslip = repo.GetPayslipById(id);
        //    if (payslip == null)
        //    {
        //        return NotFound();
        //    }

        //    // Code to generate and download the payslip as a PDF or other format

        //    return View();
        //}


    }



}

