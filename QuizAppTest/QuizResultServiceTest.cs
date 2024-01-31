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
    public class QuizResultServiceTest
    {
        private IRepository<int, Quiz> quizRepository;
        private IRepository<int, QuizResult> quizResultRepository;
        private IRepository<int, Questions> questionRepository;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<QuizContext>()
                                .UseInMemoryDatabase("dbTestQuizResults")
                                .Options;
            QuizContext context = new QuizContext(dbOptions);
            quizRepository = new QuizRepository(context);
            quizResultRepository = new QuizResultRepository(context);
            questionRepository = new QuestionRepository(context);
        }

        [Test]
        public void AddQuizResultTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);
            var quizResult = new QuizResult
            {
                QuizId = 1,
                Username = "TestUser",
                QuestionId = 1,
                UserAnswer = "Option A",
                IsCorrect = true,
                Score = 1
            };

            // Act
            var result = quizResultService.AddQuizResult(quizResult);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(quizResult, result);
        }

        [Test]
        public void DeleteQuizResultTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);
            var quizResult = new QuizResult
            {
                QuizId = 1,
                Username = "TestUser",
                QuestionId = 1,
                UserAnswer = "Option A",
                IsCorrect = true,
                Score = 1
            };
            var addedResult = quizResultService.AddQuizResult(quizResult);

            // Act
            var result = quizResultService.DeleteQuizResult(addedResult.QuizResultId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetAllQuizResultsTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);

            // Act
            var result = quizResultService.GetAllQuizResults();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetResultsByQuizTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);
            var quizId = 1;

            // Act
            var result = quizResultService.GetResultsByQuiz(quizId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTotalScoreForUserInQuizTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);
            var quizResult = new QuizResult
            {
                QuizId = 1,
                Username = "TestUser",
                QuestionId = 1,
                UserAnswer = "Option A",
                IsCorrect = true,
                Score = 1
            };
            quizResultService.AddQuizResult(quizResult);

            // Act
            var result = quizResultService.GetTotalScoreForUserInQuiz(3, "TestUser");

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetResultsByUserAndQuizTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);
            var quizResult = new QuizResult
            {
                QuizId = 1,
                Username = "TestUser",
                QuestionId = 1,
                UserAnswer = "Option A",
                IsCorrect = true,
                Score = 1
            };
            quizResultService.AddQuizResult(quizResult);

            // Act
            var result = quizResultService.GetResultsByUserAndQuiz("TestUser", 1);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateQuizResultTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);
            var quizResult = new QuizResult
            {
                QuizId = 1,
                Username = "TestUser",
                QuestionId = 1,
                UserAnswer = "Option A",
                IsCorrect = true,
                Score = 1
            };
            var addedResult = quizResultService.AddQuizResult(quizResult);

            // Act
            addedResult.Score = 2;
            var result = quizResultService.UpdateQuizResult(addedResult);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Score);
        }

        [Test]
        public void EvaluateAnswerTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);

            // Create a quiz with all required properties
            var quiz = new Quiz
            {
                QuizId = 1,
                Title = "Test Quiz",
                Description = "Test Quiz Description",  // Ensure 'Description' is set
                Category = "TestCategory",
                TimeLimit = 30
            };
            quizRepository.Add(quiz);

            var question = new Questions
            {
                QuizId = 1,
                QuestionId = 1,
                QuestionTxt = "Test Question",
                Option1 = "Option A",
                Option2 = "Option B",
                Option3 = "Option C",
                Option4 = "Option D",
                Answer = "Option A"
            };
            questionRepository.Add(question);

            var answerDto = new AnswerDTO
            {
                QuizId = 1,
                QuestionId = 1,
                Username = "TestUser",
                UserAnswer = "Option A"
            };

            // Act
            var result = quizResultService.EvaluateAnswer(1, answerDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Score);
            Assert.IsTrue(result.IsCorrect);
        }


        [Test]
        public void GetLeaderboardTest()
        {
            // Arrange
            IQuizResultService quizResultService = new QuizResultService(quizResultRepository, questionRepository, quizRepository);
            var quizResult1 = new QuizResult
            {
                QuizId = 1,
                Username = "TestUser",
                QuestionId = 1,
                UserAnswer = "Option A",
                IsCorrect = true,
                Score = 1
            };
            var quizResult2 = new QuizResult
            {
                QuizId = 1,
                Username = "TestUser",
                QuestionId = 1,
                UserAnswer = "Option B",
                IsCorrect = false,
                Score = 0
            };
            quizResultService.AddQuizResult(quizResult1);
            quizResultService.AddQuizResult(quizResult2);

            // Act
            var result = quizResultService.GetLeaderboard(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

    }
}
