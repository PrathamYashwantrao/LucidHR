namespace LucidHR.Models
{
    public class Career
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Contact { get; set; }
        public string address { get; set; }
        public string role { get; set; }
        public int?yrofexp { get; set; }
        public int?noticeperiod { get; set; }

        public DateTime doj { get; set; }
        public string ctc { get; set; }
        public string expctc { get; set; }
        public string reason { get; set; }
        public string city { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }




    }
}
