using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models.DTOs
{
    public class QuestionDTO
    {
        public int QuizId { get; set; }

        [Required(ErrorMessage = "Question Id is empty")]
        public int QuestionId { get; set; }
        public string QuestionTxt { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string Answer { get; set; }
    }
}
