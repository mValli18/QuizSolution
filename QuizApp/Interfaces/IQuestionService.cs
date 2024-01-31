using QuizApp.Models;
using QuizApp.Models.DTOs;

namespace QuizApp.Interfaces
{
    public interface IQuestionService
    {
        bool AddToQuiz(QuestionDTO questionDTO);
        public void UpdateQuestion(int questionId, QuestionDTO updatedQuestion);

        bool RemoveFromQuiz(int questionid);
        IList<QuestionDTO> GetAllQuestions();
        IList<Questions> GetQuestionsByQuizId(int quizId);
    }
}
