// QuestionService.cs
using QuizApp.Exceptions;
using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Models.DTOs;
using QuizApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizApp.Services
{
    // Implementation of IQuestionService
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<int, Questions> _questionRepository;
        private readonly IRepository<int, Quiz> _quizRepository;
        private readonly IRepository<int, QuizResult> _quizResultRepository;

        // Constructor to inject dependencies
        public QuestionService(IRepository<int, Questions> questionRepository,
            IRepository<int, Quiz> quizRepository, IRepository<int, QuizResult> quizResultRepository)
        {
            Console.WriteLine("QuestionService constructor called");
            _questionRepository = questionRepository;
            _quizRepository = quizRepository;
            _quizResultRepository = quizResultRepository;
        }

        /// <summary>
        /// Add a question to a quiz
        /// </summary>
        /// <param name="questionDTO"></param>
        /// <returns></returns>
        public bool AddToQuiz(QuestionDTO questionDTO)
        {
            var existingQuestion = _questionRepository.GetAll().FirstOrDefault(c => c.QuestionId == questionDTO.QuestionId);
            int questionId = 0;

            if (existingQuestion == null)
            {
                // If the question doesn't exist, add it to the repository
                var question = _questionRepository.Add(new Questions
                {
                    QuizId = questionDTO.QuizId,
                    QuestionTxt = questionDTO.QuestionTxt,
                    Option1 = questionDTO.Option1,
                    Option2 = questionDTO.Option2,
                    Option3 = questionDTO.Option3,
                    Option4 = questionDTO.Option4,
                    Answer = questionDTO.Answer,
                    QuestionId = questionDTO.QuestionId,
            });

                questionId = questionDTO.QuestionId;
            }
            else
            {
                // If the question already exists, use its ID
                questionId = existingQuestion.QuestionId;
            }

            return true;
        }

        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns></returns>
        public IList<QuestionDTO> GetAllQuestions()
        {
            var questions = _questionRepository.GetAll();

            // Map entity list to DTO list
            var questionDTOs = questions.Select(q => new QuestionDTO
            {
                QuestionId = q.QuestionId,
                QuizId = q.QuizId,
                QuestionTxt = q.QuestionTxt,
                Option1 = q.Option1,
                Option2 = q.Option2,
                Option3 = q.Option3,
                Option4 = q.Option4,
                Answer = q.Answer
            }).ToList();

            return questionDTOs;
        }

        /// <summary>
        ///  Get questions for a specific quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        /// <exception cref="NoQuestionsAvailableException"></exception>
        public IList<Questions> GetQuestionsByQuizId(int quizId)
        {
            if (quizId != 0)
            {
                // Retrieve questions for a specific quiz and order them by QuestionId
                var quizQuestions = _questionRepository
                    .GetAll()
                    .Where(q => q.QuizId == quizId)
                    .OrderBy(q => q.QuestionId)
                    .ToList();

                if (quizQuestions.Count != 0)
                {
                    // Map entity list to DTO list with required properties
                    var questions = quizQuestions.Select((q, index) => new Questions
                    {
                        QuestionId = q.QuestionId,
                        QuestionTxt = q.QuestionTxt,
                        Option1 = q.Option1,
                        Option2 = q.Option2,
                        Option3 = q.Option3,
                        Option4 = q.Option4
                    }).ToList();

                    return questions;
                }

                // Throw an exception if no questions are available for the specified quiz
                throw new NoQuestionsAvailableException();
            }

            return null;
        }

        /// <summary>
        /// Get a question by its ID
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public Questions GetQuestionById(int questionId)
        {
            return _questionRepository.GetById(questionId);
        }

        /// <summary>
        ///  Check if a question is already associated with a quiz
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        private bool CheckIfQuestionAlreadyPresent(int questionId)
        {
            var question = _questionRepository.GetAll()
                .FirstOrDefault(ci =>  ci.QuestionId == questionId);

            return question != null;
        }

        /// <summary>
        /// Remove a question from a quiz
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        public bool RemoveFromQuiz( int questionid)
        {
            var questionCheck = _questionRepository.GetAll().FirstOrDefault(c => c.QuestionId == questionid);
            bool CheckQuizQuestion = CheckIfQuestionAlreadyPresent(questionid);

            if (CheckQuizQuestion)
            {
                // If the question is associated with the quiz, delete it from the repository
                var result = _questionRepository.Delete(questionid);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Update a question in a quiz
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="updatedQuestion"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void UpdateQuestion(int questionId, QuestionDTO updatedQuestion)
        {
            if (updatedQuestion == null)
            {
                throw new ArgumentNullException(nameof(updatedQuestion), "Updated question data is null.");
            }

            // Ensure the question with the provided IDs exists
            var existingQuestion = _questionRepository.GetById(questionId);

            if (existingQuestion == null)
            {
                throw new InvalidOperationException($"Question with ID {questionId} not found .");
            }

            // Update the properties of the existing question with the values from the updatedQuestion
            existingQuestion.QuestionTxt = updatedQuestion.QuestionTxt;
            existingQuestion.Option1 = updatedQuestion.Option1;
            existingQuestion.Option2 = updatedQuestion.Option2;
            existingQuestion.Option3 = updatedQuestion.Option3;
            existingQuestion.Option4 = updatedQuestion.Option4;
            existingQuestion.Answer = updatedQuestion.Answer;

            // Update the question in the repository
            _questionRepository.Update(existingQuestion);
        }
    }
}
