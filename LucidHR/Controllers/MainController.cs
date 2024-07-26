using LucidHR.Data;
using LucidHR.Models;
using LucidHR.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LucidHR.Controllers
{
    public class MainController : Controller
    {
        
        private readonly HrmsRepo repo;
      //  private readonly HttpContext httpContext;
        public MainController(HrmsRepo repo)
        {

            this.repo = repo;
           // this.httpContext = httpContext; 
        }
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult AddEmp()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AddEmp(Auth e)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        e.urole = "user"; // Manually set Urole to "Employee"
        //        repo.NewEmp(e);
        //        TempData["msg"] = "Emp Added Successfully!!";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return View();
        //    }
            
        //}

        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var (user, role,email) = repo.Login(model.Email, model.Password);

        //        if (user != null && role != null)
        //        {

        //            if (role == "User")
        //            {

        //                return RedirectToAction("EmployeeDashboard", "Dashboard"); // Update controller name if necessary
        //            }
        //            else if (role == "Admin")
        //               // HttpContext.Session.SetString("useremail", email);
        //            {
        //                return RedirectToAction("HRDashboard", "Dashboard"); // Update controller name if necessary
        //            }
        //        }

        //        TempData["msg"] = "Invalid credentials or role.";
        //        return RedirectToAction("Login");
        //    }
        //    return View(model);
        //}
     
    }
}
