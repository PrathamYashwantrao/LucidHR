namespace LucidHR.Models
{
    public class StudentTasksModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Batch { get; set; }
        public string Description { get; set; }

        public string AttachmentPath { get; set; }

        public bool IsSubmitted { get; set; }
        public string Status { get; set; }
		public int? Score { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CommitmentStatus { get; set; } = "";
        public string LateReason { get; set; } = "";
    }
}
