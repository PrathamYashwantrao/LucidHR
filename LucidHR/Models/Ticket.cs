using System.ComponentModel.DataAnnotations;

namespace LucidHR.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public string? FromName { get; set; }

        public string? FromEmail { get; set; }
         
        public string? FromRole { get; set; }
        public string? ToName { get; set; }
        public string? ToRole { get; set; }
        public string? TicketDetail { get; set; }

        public string? solution { get; set; }

        public string? status { get; set; }

        public DateTime GetDateTime { get; set; }


        public Ticket()
        {
            GetDateTime = DateTime.Now;
            status = "Pending";
        }
    }
}
