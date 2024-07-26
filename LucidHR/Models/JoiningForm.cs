using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LucidHR.Models
{
    public class JoiningForm
    {
        [Key]
        public int Jdid { get; set; }

        [Required]
        [StringLength(100)]
        public string Fullname { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Designation { get; set; }

        public DateTime Dateofjoining { get; set; }

        public int Totalyearsofexperience { get; set; }

        [StringLength(15)]
        public string Contact { get; set; }

        [StringLength(100)]
        public string Emergencycontactname { get; set; }

        [StringLength(15)]
        public string Emergencycontactnumber { get; set; }

        [StringLength(50)]
        public string Relationwithperson { get; set; }

        [StringLength(100)]
        public string Uemail { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(100)]
        public string Fathersname { get; set; }

        [StringLength(50)]
        public string Maritalstatus { get; set; }

        [StringLength(10)]
        public string Bloodgroup { get; set; }

        [StringLength(10)]
        public string Pan { get; set; }

        [StringLength(12)]
        public string Aadharno { get; set; }

        public DateTime Dob { get; set; }

        [StringLength(100)]
        public string Placeofbirth { get; set; }

        [StringLength(100)]
        public string Bankname { get; set; }

        [StringLength(11)]
        public string Ifsccode { get; set; }

        [StringLength(20)]
        public string Accountnumber { get; set; }

        [StringLength(255)]
        public string AadharAttachment { get; set; }

        [StringLength(255)]
        public string PanAttachment { get; set; }

        //public int? Uid { get; set; }

        //[ForeignKey("Uid")]
        //public userLogin Auth { get; set; }

        public String pphoto { get; set; }
    }
}
