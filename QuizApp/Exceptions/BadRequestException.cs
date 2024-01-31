namespace QuizApp.Exceptions
{
    public class BadRequestException : Exception
    {
        string message;
        public BadRequestException()
        {
            message = "The provided question does not belong to the specified quiz.";
        }
        public override string Message => message;
    }
}
