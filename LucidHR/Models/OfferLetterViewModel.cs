using ExpressiveAnnotations.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LucidHR.Models
{
    public class OfferLetterViewModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfJoining { get; set; }

        [RequiredIf("Role == 'Other'", ErrorMessage = "Salary is required when the Role is 'Other'.")]
        public decimal? Salary { get; set; }

        [RequiredIf("Role == 'Other'", ErrorMessage = "desination is required when the Role is 'Other'.")]
        public string Designation { get; set; }


        [RequiredIf("Role == 'Other'", ErrorMessage = "Hra is required when the Role is 'Other'.")]
        public decimal? Hra { get; set; }

        [RequiredIf("Role == 'Other'", ErrorMessage = "TravelAllowance is required when the Role is 'Other'.")]
        public decimal? TravelAllowance { get; set; }

        [RequiredIf("Role == 'Other'", ErrorMessage = "Bonus is required when the Role is 'Other'.")]
        public decimal? Bonus { get; set; }

        [RequiredIf("Role == 'Other'", ErrorMessage = "SpecialAllowance is required when the Role is 'Other'.")]
        public decimal? SpecialAllowance { get; set; }

        [RequiredIf("Role == 'Other'", ErrorMessage = "Medical is required when the Role is 'Other'.")]
        public decimal? Medical { get; set; }

        [RequiredIf("Role == 'Other'", ErrorMessage = "Pf is required when the Role is 'Other'.")]
        public decimal? Pf { get; set; }


        [RequiredIf("Role == 'Other'", ErrorMessage = "BasicSalary is required when the Role is 'Other'.")]
        public decimal? BasicSalary { get; set; }

        [RequiredIf("Role == 'Other'", ErrorMessage = "ProfessionalTax is required when the Role is 'Other'.")]
        public decimal? ProfessionalTax { get; set; }


        [RequiredIf("Role == 'Other'", ErrorMessage = "Tds is required when the Role is 'Other'.")]
        public decimal? Tds { get; set; }
    }
}
