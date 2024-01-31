namespace QuizApp.Exceptions
{
    public class NoQuizsAvailableException:Exception
    {
        string message;
        public NoQuizsAvailableException()
        {
            message = "No Quizs are available To take";
        }
        public override string Message => message;
    }
}
