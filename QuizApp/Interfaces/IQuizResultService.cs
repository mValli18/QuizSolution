 using QuizApp.Models;
using QuizApp.Models.DTOs;
using System.Collections.Generic;

namespace QuizApp.Interfaces
{
    public interface IQuizResultService
    {
        QuizResult AddQuizResult(QuizResult quizResult);
        QuizResult UpdateQuizResult(QuizResult quizResult);
        public IList<QuizResultDTO> GetResultsByUserAndQuiz(string username, int quizId);
        IList<QuizResult> GetResultsByQuiz(int quizId);
        IList<QuizResult> GetAllQuizResults();
        public int[] GetAnsweredQuizIdsForUser(string username);
        bool DeleteQuizResult(int quizResultId);
        QuizResultDTO EvaluateAnswer(int quizId, AnswerDTO answerDTO);
        public int GetTotalScoreForUserInQuiz(int quizId, string username);
        public IList<LeaderboardEntryDTO> GetLeaderboard(int quizId);
    }
}
