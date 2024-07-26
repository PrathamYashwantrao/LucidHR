namespace LucidHR.Models
{
    public class Event
    {

        // Event.cs
      
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Type { get; set; } // Example: Holiday, Birthday

           public string CustomColor { get; set; } // Add this property


    }
}
