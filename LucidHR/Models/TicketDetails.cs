using System.ComponentModel.DataAnnotations;

namespace LucidHR.Models
{
    public class TicketDetails

    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Ticket details are required")]
        public string TicketDetail { get; set; }
        public string? solution { get; set; }

        

       

    }
}
