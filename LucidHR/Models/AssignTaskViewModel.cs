namespace LucidHR.Models
{
    public class AssignTaskViewModel
    {
        public AssignTaskViewModel()
        {
            Batches = new List<string>();
        }

        public List<string> Batches { get; set; }
        public string SelectedBatch { get; set; }
        public string TaskDescription { get; set; }
        public List<string> SelectedStudents { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
