using LucidHR.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace LucidHR.Controllers
{
    public class AuthController : Controller
    {

        private readonly HttpClient _client;
        public AuthController()
        {

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _client = new HttpClient(clientHandler);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(userLogin u)
        {
            //HttpContext.Session.SetString("uid", u.);

            var url = $"https://localhost:44383/api/Auth/Login?Email={u.uemail}&password={u.upass}";
            var json = JsonConvert.SerializeObject(u);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _client.PostAsync(url, data).Result;
            if (response.IsSuccessStatusCode)
            {
                var userData = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<userLogin>(userData);

                var claims = new List<Claim>
            {
                new Claim("uid", user.uid.ToString()), // Adding uid as a custom claim
                new Claim(ClaimTypes.Name, user.uname),
                new Claim(ClaimTypes.Email, user.uemail),
                new Claim(ClaimTypes.Role, user.urole), // Assuming user.urole is a string with the role name
                
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("email", user.uemail);
                HttpContext.Session.SetInt32("uid", user.uid);
                //HttpContext.Session.SetInt32("uid", user.uid);





                if (user.urole == "Admin")
                {
                    HttpContext.Session.SetString("FromRole", user.urole);
                    HttpContext.Session.SetString("FromName", user.uname);
                    HttpContext.Session.SetString("FromEmail", user.uemail);

                    ViewBag.Email = user.uemail;


                    return RedirectToAction("Index", "Home");
                }
                else if (user.urole == "User")
                {

                    HttpContext.Session.SetString("FromRole", user.urole);
                    HttpContext.Session.SetString("FromName", user.uname);
                    HttpContext.Session.SetString("FromEmail", user.uemail);

                    ViewBag.Email = user.uemail;

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return Unauthorized("Invalid role");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(u);
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


        public IActionResult Signout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedcookies = Request.Cookies.Keys;
            foreach (var cookie in storedcookies)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Login", "Auth");
        }


    }
}
