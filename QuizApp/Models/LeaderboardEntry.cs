using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class LeaderboardEntry
    { 

        [Key]
        public int LeaderboardId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public int Score { get; set; }
    }
}
