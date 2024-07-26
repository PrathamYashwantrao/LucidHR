using System.ComponentModel.DataAnnotations;

namespace LucidHR.Models
{
    public class userLogin
    {
        [Key]
        public int uid { get; set; }
        public string uname { get; set; }
        public string uemail { get; set; }
        public string urole { get; set; }
        public string upass { get; set; }
    }
}
