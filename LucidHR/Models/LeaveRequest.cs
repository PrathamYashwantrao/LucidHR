
using System.ComponentModel.DataAnnotations;

namespace LucidHR.Models
{
    public class LeaveRequest
    {
        [Key]
        public int sr { get; set; }

        [Required]
        public string? uemail { get; set; }

        [Required]
        public int uid { get; set; }

        [Required]
        public string uname { get; set; }

        [Required]
        [Display(Name = "From Date")]
        public DateTime leavefromdate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime leavetodate { get; set; }

        [Required]
        public string reason { get; set; }

        [Required]
        [Display(Name = "Applied Leave Days")]
        public int applyleavedays { get; set; }


        [Required]
        [Display(Name = "Leave Balance Days")]
        public int balleavedays { get; set; }

        [Required]
        [Display(Name = "Absent Days")]
        public int absentleavedays { get; set; }

        [Required]
        public string lstatus { get; set; } = "Pending";

        //public virtual Emp Employee { get; set; }
    }
}
