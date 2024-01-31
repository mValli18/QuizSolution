namespace QuizApp.Models.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
        public string Password { get; set; }
    }
}