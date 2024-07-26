using LucidHR.Data;
using LucidHR.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LucidHR.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace LucidHR.Repository
{
    public class HrmsServices : HrmsRepo
    {


        private readonly ApplicationDbContext db;
        private readonly ILogger<HrmsServices> _logger; // Declare ILogger
        public HrmsServices(ApplicationDbContext db, ILogger<HrmsServices> logger)
        {

            this.db = db;
            _logger = logger;
            _logger = logger; // Initialize ILogger
        }
        public void NewEmp(userLogin e)
        {
            try
            {
                db.userLogin.Add(e);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving employee data.");
                throw; // Rethrow exception to handle at higher level
            }
        }

        public (userLogin user, string role,string email) Login(string email, string password)
        {
            try
            {
                var user = db.userLogin.FirstOrDefault(e => e.uemail == email && e.upass == password);
                if (user != null)
                {
                    return (user, user.urole,user.uemail); // Return both user object and role
                }
                return (null, null,null); // Return null if user not found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
                throw;
            }
        }


       


        public void AddEvent(Event @event)
        {
            try
            {
                db.Events.Add(@event);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding event.");
                throw;
            }
        }

        public void DeleteEvent(int id)
        {
            try
            {
                var @event = db.Events.Find(id);
                if (@event == null)
                {
                    throw new Exception("Event not found.");
                }

                db.Events.Remove(@event);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event.");
                throw;
            }
        }

        public List<Event> GetEvents()
        {
            try
            {
                return db.Events.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching events.");
                throw;
            }
        }



        public Event GetEventById(int id)
        {
            return db.Events.Find(id);
        }

        public void UpdateEvent(Event @event)
        {
            db.Entry(@event).State = EntityState.Modified;
            db.SaveChanges();
        }

        //public List<Payslip> GetPayslipsByEmployeeNo(string employeeNo)
        //{
        //    try
        //    {
        //        return db.Payslips.Where(p => p.EmployeeNo == employeeNo).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching payslips by employee number.");
        //        throw;
        //    }
        //}








       
    }

}