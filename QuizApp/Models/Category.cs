using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
