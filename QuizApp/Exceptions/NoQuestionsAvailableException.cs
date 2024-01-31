namespace QuizApp.Exceptions
{
    public class NoQuestionsAvailableException : Exception
    {
        string message;
        public NoQuestionsAvailableException()
        {
            message = "No Questions are available In This Quiz";
        }
        public override string Message => message;
    }
}
