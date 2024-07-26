namespace LucidHR.Models
{
    public class TaskSubmissionViewModel
    {
        public int TaskId { get; set; }
        public IFormFile File { get; set; }

        public string LateReason { get; set; }
        public string CommitmentStatus { get; set; }
    }
   
}
