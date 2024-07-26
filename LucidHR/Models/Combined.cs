namespace LucidHR.Models
{
    public class Combined
    {
        public List<JoiningForm>? details { get; set; }
        public List<LeaveRequest>? leavess { get; set; }
        public List<Ticket>? ticket { get; set; }
        public List<Event>? events { get; set; }

    }
}
