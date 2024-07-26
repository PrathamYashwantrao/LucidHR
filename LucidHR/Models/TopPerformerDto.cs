namespace LucidHR.Models
{
    public class TopPerformerDto
    {
        public string Name { get; set; }
        public string Batch { get; set; }
        public int TotalScore { get; set; }
    }

    public class TopPerformersViewModel
    {
        public List<TopPerformerDto> TopPerformers { get; set; }
    }
}