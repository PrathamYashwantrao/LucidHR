using Microsoft.AspNetCore.Mvc;
using LucidHR.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LucidHR.Controllers
{
    public class LeaveController : Controller
    {
        private readonly HttpClient client;
        private readonly IHttpClientFactory _httpClientFactory;
        //private int appliedLeavesInt = 0;
        private int balanceLeavesInt = 0;

        public LeaveController(IHttpClientFactory httpClientFactory)
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            client = new HttpClient(handler);
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ApplyLeave(int uid)
        {
            // Initialize a new LeaveRequest model
            //var model = new LeaveRequest();
            var model2 = new LeaveRequest();
            uid = HttpContext.Session.GetInt32("uid") ?? 0;

            var httpClient = _httpClientFactory.CreateClient();
            // Call API to check if the user has leave requests
            string hasLeaveRequestsUrl = $"https://localhost:44383/api/Leave/HasLeaveRequests/{uid}";
            var hasLeaveRequestsResponse = await httpClient.GetAsync(hasLeaveRequestsUrl);
            hasLeaveRequestsResponse.EnsureSuccessStatusCode();
            var hasLeaveRequests = await hasLeaveRequestsResponse.Content.ReadFromJsonAsync<bool>();

            int balanceLeaves;

            if (hasLeaveRequests)
            {
                // Call API to get the user's latest leave data
                string getUserLeaveDataUrl = $"https://localhost:44383/api/Leave/GetUserLeaveData/{uid}";
                var userLeaveDataResponse = await httpClient.GetAsync(getUserLeaveDataUrl);
                userLeaveDataResponse.EnsureSuccessStatusCode();
                var userLeaveData = await userLeaveDataResponse.Content.ReadFromJsonAsync<LeaveRequest>();

                if (userLeaveData == null)
                {
                    // Handle the case where userLeaveData is null
                    return NotFound("User leave data not found.");
                }

                balanceLeaves = userLeaveData.balleavedays;

                //// Calculate additional leaves for the months since the last update
                //var lastUpdatedMonth = userLeaveData.RequestDate.Month;
                //var currentMonth = DateTime.Now.Month;
                //var additionalLeaves = (currentMonth - lastUpdatedMonth) * 2; // Assuming 2 leaves per month

                //balanceLeaves += additionalLeaves;
                int newBalanceLeaves = balanceLeaves;
                ViewBag.BalanceLeaves = newBalanceLeaves;
            }
            else
            {
                var dateOfJoining = new DateTime(2024, 4, 1);
                balanceLeaves = CalculateBalanceLeaves(dateOfJoining);

                // Store new balance leaves in the session
                HttpContext.Session.SetInt32("BalanceLeaves", balanceLeaves);
                ViewBag.BalanceLeaves = balanceLeaves;

            }

            return View(model2);
        }

        public class UserLeaveData
        {
            public int BalanceLeaves { get; set; }
            public DateTime RequestDate { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLeave(LeaveRequest l, int absentLeaveDays, int uid)
        {
            int balanceLeaves = 0;

            // Initialize a new LeaveRequest model
            var model = new LeaveRequest();
            uid = HttpContext.Session.GetInt32("uid") ?? 0;


            var httpClient = _httpClientFactory.CreateClient();
            // Call API to check if the user has leave requests
            string hasLeaveRequestsUrl = $"https://localhost:44383/api/Leave/HasLeaveRequests/{uid}";
            var hasLeaveRequestsResponse = await httpClient.GetAsync(hasLeaveRequestsUrl);
            hasLeaveRequestsResponse.EnsureSuccessStatusCode();
            var hasLeaveRequests = await hasLeaveRequestsResponse.Content.ReadFromJsonAsync<bool>();

            if (hasLeaveRequests)
            {
                // Call API to get the user's latest leave data
                string getUserLeaveDataUrl = $"https://localhost:44383/api/Leave/GetUserLeaveData/{uid}";
                var userLeaveDataResponse = await httpClient.GetAsync(getUserLeaveDataUrl);
                userLeaveDataResponse.EnsureSuccessStatusCode();
                var userLeaveData = await userLeaveDataResponse.Content.ReadFromJsonAsync<LeaveRequest>();

                if (userLeaveData == null)
                {
                    // Handle the case where userLeaveData is null
                    return NotFound("User leave data not found.");
                }

                balanceLeaves = userLeaveData.balleavedays;

                //// Calculate additional leaves for the months since the last update
                //var lastUpdatedMonth = userLeaveData.RequestDate.Month;
                //var currentMonth = DateTime.Now.Month;
                //var additionalLeaves = (currentMonth - lastUpdatedMonth) * 2; // Assuming 2 leaves per month

                //balanceLeaves += additionalLeaves;
                //int? newBalanceLeaves = HttpContext.Session.GetInt32("NewBalanceLeaves");
                //balanceLeaves = HttpContext.Session.GetInt32("NewBalanceLeaves") ?? 0;

                //// Update the balance leaves in the session
                //if (newBalanceLeaves.HasValue)
                //{
                //    HttpContext.Session.SetInt32("BalanceLeaves", newBalanceLeaves.Value);
                //}

                ViewBag.BalanceLeaves = balanceLeaves;

            }
            else
            {
                var dateOfJoining = new DateTime(2024, 4, 1);
                balanceLeaves = CalculateBalanceLeaves(dateOfJoining);

                // Store new balance leaves in the session
                HttpContext.Session.SetInt32("BalanceLeaves", balanceLeaves);
                ViewBag.BalanceLeaves = balanceLeaves;

            }

            // Add the absentLeaveDays to the model
            l.absentleavedays = absentLeaveDays;
            int appliedLeaves = HttpContext.Session.GetInt32("AppliedLeaves") ?? 0;
            var leaveRequest = new LeaveRequest
            {
                uid = HttpContext.Session.GetInt32("uid") ?? 0,
                uemail = HttpContext.Session.GetString("FromEmail"),
                uname = HttpContext.Session.GetString("FromName"),
                leavefromdate = l.leavefromdate,
                leavetodate = l.leavetodate,
                reason = l.reason,
                applyleavedays = appliedLeaves,
                balleavedays = balanceLeaves,
                absentleavedays = l.absentleavedays,
                lstatus = l.lstatus
            }; 

            string url = "https://localhost:44383/api/Leave/AddLeaveRequest/";
            var jsonData = JsonConvert.SerializeObject(leaveRequest);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Calendar");
            }
            return View(l);
        }

        public IActionResult CalenderView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ApproveLeave()
        {
            string url = "https://localhost:44383/GetAllLeaveRequests/";
            List<LeaveRequest> leaveRequests = new List<LeaveRequest>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<LeaveRequest>>(jsonData);
                if (obj != null)
                {
                    leaveRequests = obj;
                }
            }
            return View(leaveRequests);
        }

        /*
        [HttpPost]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // Fetch the leave request details to keep existing data
            var getResponse = await client.GetAsync($"https://localhost:44383/api/Leave/{id}");
            if (!getResponse.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var leaveRequest = JsonConvert.DeserializeObject<LeaveRequest>(await getResponse.Content.ReadAsStringAsync());
            if (leaveRequest == null)
            {
                return NotFound();
            }

            // Fetch current balance leaves from session
            int? balanceLeavesFromSession = HttpContext.Session.GetInt32("BalanceLeaves");
            int? appliedLeavesFromSession = HttpContext.Session.GetInt32("AppliedLeaves");
            if (balanceLeavesFromSession.HasValue)
            {
                int currentBalanceLeaves = balanceLeavesFromSession.Value; 

                int appliedLeaves = appliedLeavesFromSession.Value;

                // Calculate new balance leaves
                int newBalanceLeaves = currentBalanceLeaves > appliedLeaves ? currentBalanceLeaves - appliedLeaves : 0;

                // Store new balance leaves in the session
                HttpContext.Session.SetInt32("NewBalanceLeaves", newBalanceLeaves);

                // Update leave request status and balance leaves
                leaveRequest.lstatus = "Approved";
                leaveRequest.balleavedays = newBalanceLeaves;

                string url = $"https://localhost:44383/api/Leave/approve";
                var jsonData = JsonConvert.SerializeObject(leaveRequest);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // After approval balance leaves updated

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("rajeshnarkar05@gmail.com"),
                        Subject = "Leave Request Approved",
                        Body = $"Your leave request has been approved. Details:\n\nLeave From: {leaveRequest.leavefromdate.ToShortDateString()}\nLeave To: {leaveRequest.leavetodate.ToShortDateString()}\nReason: {leaveRequest.reason}\n\nRegards,\n\nHR Team\n\nMasstech Business Solutions Pvt. Ltd.",
                        IsBodyHtml = false,
                    };
                    mailMessage.To.Add("rajeshnarkar05@gmail.com"); // Add the recipient's email address here

                    var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("rajeshnarkar05@gmail.com", "xcbxsohmepzvwhyd"),
                        EnableSsl = true,
                    };

                    await smtpClient.SendMailAsync(mailMessage);
                    TempData["SuccessMessage"] = "Mail successfully sent";

                    //// Clear the session values
                    //HttpContext.Session.Remove("BalanceLeaves");
                    //HttpContext.Session.Remove("AppliedLeaves");

                    return RedirectToAction("ViewLeaveRequests"); // Redirect to the appropriate action
                }
            }

            return BadRequest();
        }
        */

        [HttpPost]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {


            var client = _httpClientFactory.CreateClient();

            // Fetch the leave request details to keep existing data
            var getResponse = await client.GetAsync($"https://localhost:44383/api/Leave/{id}");
            if (!getResponse.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var leaveRequest = JsonConvert.DeserializeObject<LeaveRequest>(await getResponse.Content.ReadAsStringAsync());
            if (leaveRequest == null)
            {
                return NotFound();
            }

            int currentBalanceLeaves = leaveRequest.balleavedays;
            int appliedLeaves = leaveRequest.applyleavedays;

            // Calculate new balance leaves
            int newBalanceLeaves = currentBalanceLeaves > appliedLeaves ? currentBalanceLeaves - appliedLeaves : 0;

            // Update leave request status and balance leaves
            leaveRequest.lstatus = "Approved";
            leaveRequest.balleavedays = newBalanceLeaves;

            string url = $"https://localhost:44383/api/Leave/approve";
            var jsonData = JsonConvert.SerializeObject(leaveRequest);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                // Update the empapplyleaves table with the new balance leaves
                leaveRequest.balleavedays = newBalanceLeaves;
                var updateLeaveUrl = $"https://localhost:44383/api/Leave/updateLeave/{leaveRequest.uid}";
                var updateLeaveData = JsonConvert.SerializeObject(leaveRequest);
                StringContent updateLeaveContent = new StringContent(updateLeaveData, Encoding.UTF8, "application/json");
                var updateResponse = await client.PutAsync(updateLeaveUrl, updateLeaveContent);

                if (updateResponse.IsSuccessStatusCode)
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("rajeshnarkar05@gmail.com"),
                        Subject = "Leave Request Approved",
                        Body = $"Your leave request has been approved. Details:\n\nLeave From: {leaveRequest.leavefromdate.ToShortDateString()}\nLeave To: {leaveRequest.leavetodate.ToShortDateString()}\nReason: {leaveRequest.reason}\n\nRegards,\nHR Team\nMasstech Business Solutions Pvt. Ltd.",
                        IsBodyHtml = false,
                    };
                    //var toEmail = HttpContext.Session.GetString("FromEmail");
                    mailMessage.To.Add("pikashoo83@gmail.com"); // Add the recipient's email address here

                    var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("rajeshnarkar05@gmail.com", "xcbxsohmepzvwhyd"),
                        EnableSsl = true,
                    };

                    await smtpClient.SendMailAsync(mailMessage);
                    TempData["SuccessMessage"] = "Mail successfully sent";

                    return RedirectToAction("ApproveLeave"); // Redirect to the appropriate action
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var client = _httpClientFactory.CreateClient();

            string url = $"https://localhost:44383/api/Leave/reject/{id}";
            HttpResponseMessage response = await client.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ApproveLeave"); // Redirect to the appropriate action
            }

            return BadRequest();
        }

        private int CalculateBalanceLeaves(DateTime dateOfJoining)
        {
            var currentDate = DateTime.Now;
            var monthsWorked = ((currentDate.Year - dateOfJoining.Year) * 12 + currentDate.Month - dateOfJoining.Month) + 1;
            return monthsWorked * 2;
        }

        public JsonResult CalculateAppliedLeaves(DateTime fromDate, DateTime toDate)
        {
            // Calculate the number of applied leave days excluding weekends
            int appliedLeaves = 0;
            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                // Exclude weekends (Saturday and Sunday)
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    appliedLeaves++;
                }
            }

            HttpContext.Session.SetInt32("AppliedLeaves", appliedLeaves); // Store in session
            return Json(new { appliedLeaves });
        }

        [HttpGet]
        public IActionResult ViewLeaveRequests(int uid)
        {
            uid = HttpContext.Session.GetInt32("uid") ?? 0;

            //string url = $"https://localhost:44383/GetLeaveRequests/{uid}";
            string url = $"https://localhost:44383/GetAllLeaveRequests";
            List<LeaveRequest> leaveRequests = new List<LeaveRequest>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<LeaveRequest>>(jsonData);
                if (obj != null)
                {
                    leaveRequests = obj;
                }
            }
            return View(leaveRequests);
        }
    }
}