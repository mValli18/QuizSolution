namespace QuizApp.Models.DTOs
{
    public class QuizResultDTO
    {
        public int QuizId { get; set; }
        public string Username { get; set; }
        public int QuestionId { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public int Score { get; set; }
    }
}
