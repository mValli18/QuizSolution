using NUnit.Framework;
using QuizApp.Contexts;
using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Models.DTOs;
using QuizApp.Repositories;
using QuizApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace QuizAppTest
{
    [TestFixture]
    public class QuestionServiceTest
    {
        private IRepository<int, Questions> questionRepository;
        private IRepository<int, Quiz> quizRepository;
        private IRepository<int, QuizResult> quizResultRepository;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<QuizContext>()
                                .UseInMemoryDatabase("dbTestQuestions")
                                .Options;
            QuizContext context = new QuizContext(dbOptions);
            questionRepository = new QuestionRepository(context);
            quizRepository = new QuizRepository(context);
            quizResultRepository = new QuizResultRepository(context);
        }

        [Test]
        public void AddQuestionToQuizTest()
        {
            // Arrange
            IQuestionService questionService = new QuestionService(questionRepository, quizRepository, quizResultRepository);
            var questionDto = new QuestionDTO
            {
                QuizId = 1,
                QuestionTxt = "Test Question",
                Option1 = "Option A",
                Option2 = "Option B",
                Option3 = "Option C",
                Option4 = "Option D",
                Answer = "Option A",
                QuestionId = 1
            };

            // Act
            var result = questionService.AddToQuiz(questionDto);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetQuestionsByQuizIdTest()
        {
            // Arrange
            IQuestionService questionService = new QuestionService(questionRepository, quizRepository, quizResultRepository);
            var quizId = 1;

            // Act
            var result = questionService.GetQuestionsByQuizId(quizId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void UpdateQuestionTest()
        {
            // Arrange
            IQuestionService questionService = new QuestionService(questionRepository, quizRepository, quizResultRepository);
            var quizId = 1;

            // Add a quiz
            var quiz = new Quiz
            {
                QuizId = quizId,
                Title = "Test Quiz",
                Category = "TestCategory",
                Description="TestDescription",
                TimeLimit = 30
            };
            quizRepository.Add(quiz);

            // Add a question to the quiz
            var question = new Questions
            {
                QuizId = quizId,
                QuestionId = 1,
                QuestionTxt = "Test Question",
                Option1 = "Option A",
                Option2 = "Option B",
                Option3 = "Option C",
                Option4 = "Option D",
                Answer = "Option A"
            };
            questionRepository.Add(question);

            var updatedQuestionDto = new QuestionDTO
            {
                QuestionTxt = "Updated Question",
                Option1 = "Updated Option A",
                Option2 = "Updated Option B",
                Option3 = "Updated Option C",
                Option4 = "Updated Option D",
                Answer = "Updated Option A"
            };

            // Act
            questionService.UpdateQuestion(1, updatedQuestionDto); // Use the correct question ID

            // Assert
            var result = questionService.GetQuestionsByQuizId(quizId);
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Question", result[0].QuestionTxt);
        }


        [Test]
        public void RemoveFromQuizTest()
        {
            // Arrange
            IQuestionService questionService = new QuestionService(questionRepository, quizRepository, quizResultRepository);
            var questionId = 1;

            // Act
            var result = questionService.RemoveFromQuiz(questionId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
