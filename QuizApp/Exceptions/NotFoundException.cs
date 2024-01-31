namespace QuizApp.Exceptions
{
    public class NotFoundException : Exception
    {
        string message;
        public NotFoundException()
        {
            message = "Quiz or Question Not found";
        }
        public override string Message => message;
    }
}
