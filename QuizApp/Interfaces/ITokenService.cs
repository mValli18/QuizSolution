using QuizApp.Models.DTOs;

namespace QuizApp.Interfaces
{
    public interface ITokenService
    {
        string GetToken(UserDTO user);
    }
}
