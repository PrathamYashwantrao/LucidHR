namespace LucidHR.Models
{

    public class OfferLetter
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfJoining { get; set; }
        public byte[] OfferLetterPdf { get; set; }
        public bool IsSent { get; set; }
        public decimal? Salary { get; set; }
        public string? Designation { get; set; }
        public decimal? Hra { get; set; }
        public decimal? TravelAllowance { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? SpecialAllowance { get; set; }
        public decimal? Medical { get; set; }
        public decimal? Pf { get; set; }

        public decimal? BasicSalary { get; set; }

        public decimal? ProfessionalTax { get; set; }

        public decimal? Tds { get; set; }
    }
}
