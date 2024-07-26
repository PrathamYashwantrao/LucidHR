using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using LucidHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LucidHR.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CalendarController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            int userId = HttpContext.Session.GetInt32("uid") ?? 0;

            var model = GetCalendarViewModel(currentMonth, currentYear, userId);
            return View(model);
        }

        public async Task<IActionResult> Fetch(int month, int year, int uid)
        {
            uid = HttpContext.Session.GetInt32("uid") ?? 0;
            var model = GetCalendarViewModel(month, year, uid);
            return Json(model);
        }

        public CalendarViewModel GetCalendarViewModel(int month, int year, int uid)
        {
            uid = HttpContext.Session.GetInt32("uid") ?? 0;
            var days = GetDaysInMonth(month, year);
            var leaveDates = GetLeaveDates(year, month, uid);
            var approvedLeaveDates = GetApprovedLeaveDates(year, month, uid);
            var pendingLeaveDates = GetPendingLeaveDates(year, month, uid);
            var rejectedLeaveDates = GetRejectedLeaveDates(year, month, uid);

            foreach (var day in days)
            {
                var currentDate = new DateTime(year, month, day.Day);
                day.IsLeaveDay = leaveDates.Any(ld => ld.LeaveFromDate.Date <= currentDate && ld.LeaveToDate.Date >= currentDate);
                day.IsApprovedLeaveDay = approvedLeaveDates.Any(ld => ld.LeaveFromDate.Date <= currentDate && ld.LeaveToDate.Date >= currentDate);
                day.IsPendingLeaveDay = pendingLeaveDates.Any(ld => ld.LeaveFromDate.Date <= currentDate && ld.LeaveToDate.Date >= currentDate);
                day.IsRejectedLeaveDay = rejectedLeaveDates.Any(ld => ld.LeaveFromDate.Date <= currentDate && ld.LeaveToDate.Date >= currentDate);
            }

            return new CalendarViewModel
            {
                CurrentYear = year,
                CurrentMonth = month,
                MinYear = 2020,
                MaxYear = 2030,
                Days = days,
                LeaveDates = leaveDates,
                ApprovedLeaveDates = approvedLeaveDates,
                PendingLeaveDates = pendingLeaveDates,
                RejectedLeaveDates = rejectedLeaveDates
            };
        }

        private List<CalendarDay> GetDaysInMonth(int month, int year)
        {
            var days = new List<CalendarDay>();
            var firstDay = new DateTime(year, month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            for (var day = firstDay; day <= lastDay; day = day.AddDays(1))
            {
                days.Add(new CalendarDay
                {
                    Day = day.Day,
                    IsWeekend = day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday,
                    IsLeaveDay = false,
                    IsApprovedLeaveDay = false,
                    IsPendingLeaveDay = false,
                    IsRejectedLeaveDay = false
                });
            }

            return days;
        }

        private List<LeaveDate> GetLeaveDates(int year, int month, int uid)
        {
            uid = HttpContext.Session.GetInt32("uid") ?? 0;
            var client = _httpClientFactory.CreateClient();
            string url = $"https://localhost:44383/api/Leave/leaveDatesForMonth/{uid}?year={year}&month={month}";

            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<LeaveDate>>(jsonData);
            }

            return new List<LeaveDate>();
        }

        private List<LeaveDate> GetApprovedLeaveDates(int year, int month, int uid)
        {

            // Original code to fetch approved leave dates from API
            uid = HttpContext.Session.GetInt32("uid") ?? 0;
            var client = _httpClientFactory.CreateClient();
            string url = $"https://localhost:44383/api/Leave/approvedLeavesForMonth/{uid}?year={year}&month={month}";

            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<LeaveDate>>(jsonData);
            }

            return new List<LeaveDate>();
        }

        private List<LeaveDate> GetPendingLeaveDates(int year, int month, int uid)
        {

            // Original code to fetch approved leave dates from API
            uid = HttpContext.Session.GetInt32("uid") ?? 0;
            var client = _httpClientFactory.CreateClient();
            string url = $"https://localhost:44383/api/Leave/pendingLeavesForMonth/{uid}?year={year}&month={month}";

            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<LeaveDate>>(jsonData);
            }

            return new List<LeaveDate>();
        }

        private List<LeaveDate> GetRejectedLeaveDates(int year, int month, int uid)
        {

            // Original code to fetch approved leave dates from API
            uid = HttpContext.Session.GetInt32("uid") ?? 0;

            var client = _httpClientFactory.CreateClient();
            string url = $"https://localhost:44383/api/Leave/rejectedLeavesForMonth/{uid}?year={year}&month={month}";

            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<LeaveDate>>(jsonData);
            }

            return new List<LeaveDate>();
        }
    }

}