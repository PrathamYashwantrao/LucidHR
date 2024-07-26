using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LucidHR.Models
{
    public class Performance
    {

    }

    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public int uid { get; set; }
        public bool IsActive { get; set; }
        public string QuestionText { get; set; }
    }

    public class PerformanceReview
    {
        [Key]
        public int ReviewId { get; set; }
        public int uid { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        [AllowNull]
        public string AnswerText { get; set; }
        public Question Question { get; set; }
    }

    
}
