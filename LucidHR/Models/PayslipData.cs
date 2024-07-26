namespace LucidHR.Models
{
 
        public class PayslipData
        {
            public string EmployeeName { get; set; }
            public string Designation { get; set; }
            public string BankName { get; set; }
            public string EmployeeNo { get; set; }
            public string IFSCCode { get; set; }
            public DateTime DateOfJoining { get; set; }
            public string BankAccountNo { get; set; }
            public int DaysPaid { get; set; }
            public string PAN { get; set; }
            public int DaysInMonth { get; set; }
            public string Aadhar { get; set; }
            public string UAN { get; set; }
            public int BalanceLeave { get; set; }
            public decimal Basic { get; set; }
            public decimal HRA { get; set; }
            public decimal TravelAllowance { get; set; }
            public decimal Bonus { get; set; }
            public decimal SpecialAllowance { get; set; }
            public decimal MedicalReimbursement { get; set; }
            public decimal ProfessionalTax { get; set; }
            public decimal TDS { get; set; }
            public decimal GrossSalary => Basic + HRA + TravelAllowance + Bonus + SpecialAllowance + MedicalReimbursement;
            public decimal TotalDeduction => ProfessionalTax + TDS;
        public decimal NetSalary;
        }
    }

