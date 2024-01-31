using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models.DTOs
{
    public class QuizStateDTO
    {
        [Key]
        public int QuizStatusId { get; set; } // Primary key for QuizStatus

        public int QuizId { get; set; }
        [ForeignKey("QuizId")]// Foreign key referencing the Quiz table
        public Quiz Quiz { get; set; }
        public bool IsStarted { get; set; }
        public string Status { get; set; }
        public bool IsCompleted { get; set; }
        public bool InProgress { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
