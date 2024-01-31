using QuizApp.Models;

namespace QuizApp.Interfaces
{
    public interface IQuizService
    {
        List<Quiz> GetQuizs();
        Quiz Add(Quiz quiz);
        
        Quiz GetQuizById(int id);
        Task<Quiz> GetQuizByIdWithQuestions(int id);
        List<Quiz> GetQuizzesByCategory(string category);
        bool DeleteQuizIfNoQuestions(int quizId);
        List<string> GetCategories();
        List<string> GetTitles();
        int GetId(string title);
        Quiz UpdateQuiz(Quiz updatedQuiz);
        //public Quiz StartQuiz(int quizId);
    }
}
