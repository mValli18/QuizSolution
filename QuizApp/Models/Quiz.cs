using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuizApp.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required(ErrorMessage = "Title of the Quiz cannot be empty")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get;  set; }
        public int? TimeLimit { get; set; }
        [JsonIgnore]
        public ICollection<Questions>? Questions { get; set; }
        
    }
}