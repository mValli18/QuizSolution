using System.ComponentModel.DataAnnotations;

public class AnswerDTO
{
    [Required(ErrorMessage = "The Username field is required.")]
    public string Username { get; set; }
    public int QuestionId { get; set; }
    public string UserAnswer { get; set; }
    public int? QuizId { get; set; }
}