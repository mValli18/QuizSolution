// QuizService.cs
using QuizApp.Exceptions;
using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Models.DTOs;
using QuizApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Services
{
    // Implementation of IQuizService
    public class QuizService : IQuizService
    {
        private readonly IRepository<int, Quiz> _quizRepository;
        private readonly IRepository<int, Questions> _questionRepository;

        /// <summary>
        /// Constructor to inject dependencies
        /// </summary>
        /// <param name="quizRepository"></param>
        /// <param name="questionRepository"></param>
        public QuizService(IRepository<int, Quiz> quizRepository, IRepository<int, Questions> questionRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
        }

        /// <summary>
        ///  Add a quiz
        /// </summary>
        /// <param name="quiz"></param>
        /// <returns></returns>
        public Quiz Add(Quiz quiz)
        {
            var result = _quizRepository.Add(quiz);
            return result;
        }

        /// <summary>
        ///  Get all quizzes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NoQuizsAvailableException"></exception>
        public List<Quiz> GetQuizs()
        {
            var quizs = _quizRepository.GetAll();

            if (quizs != null)
            {
                return quizs.ToList();
            }

            throw new NoQuizsAvailableException();
        }
        /// <summary>
        /// Gets the list of all distinct catogeries of quizs.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NoQuizsAvailableException"></exception>
        public List<string> GetCategories()
        {
            var quizzes = _quizRepository.GetAll();

            if (quizzes != null && quizzes.Count > 0)
            {
                // Extract the distinct Category values from all Quiz entities
                List<string> categories = quizzes.Select(q => q.Category).Distinct().ToList();
                return categories;
            }

            throw new NoQuizsAvailableException();
        }
        /// <summary>
        /// Gets the list of all distinct titles present in the quiz table.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NoQuizsAvailableException"></exception>
        public List<string> GetTitles()
        {
            var quizzes = _quizRepository.GetAll();

            if (quizzes != null && quizzes.Count > 0)
            {
                // Extract the distinct Title values from all Quiz entities
                List<string> titles = quizzes.Select(q => q.Title).Distinct().ToList();
                return titles;
            }

            throw new NoQuizsAvailableException();
        }
        /// <summary>
        /// Gets the QuizId for the respective quiz title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public int GetId(string title)
        {
            var quiz = _quizRepository.GetAll().FirstOrDefault(q => q.Title == title);

            if (quiz != null)
            {
                return quiz.QuizId;
            }

            throw new KeyNotFoundException($"Quiz with title '{title}' not found");
        }
        /// <summary>
        /// Get quizzes by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <exception cref="NoQuizsAvailableException"></exception>
        public List<Quiz> GetQuizzesByCategory(string category)
        {
            var quizzes = _quizRepository.GetAll();

            if (quizzes != null)
            {
                return quizzes
                    .Where(quiz => quiz.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            throw new NoQuizsAvailableException();
        }

        /// <summary>
        /// Gets the Quiz by using the Id property.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NoQuizsAvailableException"></exception>
        public Quiz GetQuizById(int id)
        {
            var res = _quizRepository.GetById(id);

            if (res != null)
            {
                return res;
            }

            throw new NoQuizsAvailableException();
        }
        /// <summary>
        ///  Get a quiz by its ID with associated questions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Quiz> GetQuizByIdWithQuestions(int id)
        {
            var quiz = await Task.Run(() => GetQuizById(id));

            if (quiz != null && quiz.Questions != null)
            {
                quiz.Questions = quiz.Questions.OrderBy(q => q.QuestionId).ToList(); // Order by QuestionId

                foreach (var question in quiz.Questions)
                {
                    question.QuestionId = 0; // Reset QuestionId to start from 1
                }
            }

            return quiz;
        }

        /// <summary>
        ///  Delete a quiz if it has no associated questions
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        /// <exception cref="NoQuizsAvailableException"></exception>
        public bool DeleteQuizIfNoQuestions(int quizId)
        {
            var questionsCount = _questionRepository.GetAll().Count(q => q.QuizId == quizId);

            if (questionsCount == 0)
            {
                var deletedQuiz = _quizRepository.Delete(quizId);

                if (deletedQuiz == null)
                {
                    throw new NoQuizsAvailableException();
                }

                return true; // Quiz deleted successfully
            }

            return false; // Quiz has questions, cannot delete
        }

        /// <summary>
        /// Update quiz details
        /// </summary>
        /// <param name="updatedQuiz"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Quiz UpdateQuiz(Quiz updatedQuiz)
        {
            if (updatedQuiz != null)
            {
                ValidateQuizTitle(updatedQuiz.Title);
                ValidateQuizTimeLimit(updatedQuiz.TimeLimit);

                var existingQuiz = _quizRepository.GetById(updatedQuiz.QuizId);

                if (existingQuiz == null)
                {
                    throw new InvalidOperationException($"Quiz with ID {updatedQuiz.QuizId} not found.");
                }
                existingQuiz.Title = updatedQuiz.Title;
                existingQuiz.Description = updatedQuiz.Description;
                existingQuiz.TimeLimit = updatedQuiz.TimeLimit;

                var updatedQuizResult = _quizRepository.Update(existingQuiz);

                return updatedQuizResult;
            }
            return null;
        }

        /// <summary>
        /// Validate quiz title
        /// </summary>
        /// <param name="title"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateQuizTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Quiz title cannot be empty or whitespace.", nameof(title));
            }
        }

        /// <summary>
        /// Validate quiz time limit (Example: Ensure it's a positive value)
        /// </summary>
        /// <param name="timeLimit"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateQuizTimeLimit(int? timeLimit)
        {
            if (timeLimit < 0)
            {
                throw new ArgumentException("Time limit must be a positive value.", nameof(timeLimit));
            }
        }
    }
}
