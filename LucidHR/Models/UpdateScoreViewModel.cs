namespace LucidHR.Models
{
    public class UpdateScoreViewModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Batch { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string StudentName { get; set; }
        public int? Score { get; set; }
		public string Status { get; set; }
        public string CommitmentStatus { get; set; } = "";
        public string LateReason { get; set; } = "";
    }
}
