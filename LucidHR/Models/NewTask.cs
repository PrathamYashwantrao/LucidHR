namespace LucidHR.Models
{
    public class NewTask
    {
        public int ID { get; set; }
        public string Batch { get; set; }
        public string Description { get; set; }
        public List<string> Students { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime CDate { get; set; }
    }
}
