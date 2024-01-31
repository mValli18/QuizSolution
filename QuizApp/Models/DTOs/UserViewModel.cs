using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models.DTOs
{
    public class UserViewModel : UserDTO
    {
        [Required(ErrorMessage = "Confirm password cannot be empty")]
        [Compare("Password", ErrorMessage = "Password and Confirm password do not match")]
        public string ConfirmPassword { get; set; }

    }
}