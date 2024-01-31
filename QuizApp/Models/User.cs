using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models
{
    public class User
    {
        [Key]

        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public string Role { get; set; }
        public byte[] Key { get; set; }
    }
}