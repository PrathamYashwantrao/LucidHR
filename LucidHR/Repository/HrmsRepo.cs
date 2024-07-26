using LucidHR.Models;

namespace LucidHR.Repository
{
    public interface HrmsRepo
    {
        void NewEmp(userLogin e);
        (userLogin user, string role ,string email) Login(string email, string password); // Update the return type


        //Payslip GeneratePayslip(string employeeNo, DateTime startDate, DateTime endDate,int absentDays,string payMonth);
        //Emp GetEmployeeByNo(string employeeNo); // New method to fetch employee by EmployeeNo

        //Payslip GetPayslipById(int payslipId); // Define the method to get payslip by ID


        ///void DetailsforLetter(OfferLetter obj);



        // Event operations
        void AddEvent(Event @event);
        void DeleteEvent(int id);
        List<Event> GetEvents();


       
        Event GetEventById(int id);
        void UpdateEvent(Event @event);



        //List<Payslip> GetPayslipsByEmployeeNo(string employeeNo);

    }
}
