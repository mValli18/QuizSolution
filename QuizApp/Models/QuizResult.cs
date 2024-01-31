// QuizResult.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class QuizResult
    {
        [Key]
        public int QuizResultId { get; set; }

        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }

        public string Username { get; set; } 
        [ForeignKey("Username")]
        public User User { get; set; }

        public int Score { get; set; }
        public int QuestionId { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
