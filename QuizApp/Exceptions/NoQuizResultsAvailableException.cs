namespace QuizApp.Exceptions
{
    public class NoQuizResultsAvailableException : Exception
    {
        string message;
        public NoQuizResultsAvailableException()
        {
            message = "No QuizResults are available";
        }
        public override string Message => message;
    }
}
