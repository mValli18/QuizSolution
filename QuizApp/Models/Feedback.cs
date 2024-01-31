namespace QuizApp.Models
{
    public class Feedback
    {
        public string QuestionText { get; set; }
        public string SelectedOption { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
