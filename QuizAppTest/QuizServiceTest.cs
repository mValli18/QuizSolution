using NUnit.Framework;
using QuizApp.Contexts;
using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Models.DTOs;
using QuizApp.Repositories;
using QuizApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace QuizAppTest
{
    [TestFixture]
    public class QuizServiceTest
    {
        private IRepository<int, Quiz> repository;
        private IRepository<int, Questions> questionRepository;

        [SetUp]
        public void Setup()
        {
            var dbOptions = new DbContextOptionsBuilder<QuizContext>()
                                .UseInMemoryDatabase("dbTestCustomer")
                                .Options;
            QuizContext context = new QuizContext(dbOptions);
            repository = new QuizRepository(context);
            questionRepository = new QuestionRepository(context);
        }

        [Test]
        public void AddQuizTest()
        {
            // Arrange
            IQuizService quizService = new QuizService(repository, questionRepository);
            var quiz = new Quiz
            {
                Title = "TestAddQuiz",
                Description = "TestTheDescription",
                Category = "TestCategory",
                TimeLimit = 30
            };

            // Act
            var result = quizService.Add(quiz);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(quiz, result);
        }

        [Test]
        public void GetQuizTest()
        {
            // Arrange
            IQuizService quizService = new QuizService(repository, questionRepository);
            var quiz = new Quiz
            {
                Title = "TestAddQuiz",
                Description = "TestTheDescription",
                Category = "TestCategory",
                TimeLimit = 30
            };

            // Act
            quizService.Add(quiz);
            var result = quizService.GetQuizs();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void UpdateTest()
        {
            // Arrange
            IQuizService quizService = new QuizService(repository, questionRepository);
            var quiz = new Quiz
            {
                QuizId = 1,
                Title = "TestAddQuiz",
                Description = "TestTheDescription",
                Category = "TestCategory",
                TimeLimit = 10
            };

            // Act
            var result = quizService.UpdateQuiz(quiz);

            // Assert
            Assert.That(result, Is.EqualTo(quiz).Using(new QuizComparer()));
        }

        // Create a custom comparer for the Quiz class
        public class QuizComparer : IEqualityComparer<Quiz>
        {
            public bool Equals(Quiz x, Quiz y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;

                // Compare properties
                return x.QuizId == y.QuizId &&
                       x.Title == y.Title &&
                       x.Description == y.Description &&
                       x.Category == y.Category &&
                       x.TimeLimit == y.TimeLimit;
            }

            public int GetHashCode(Quiz obj)
            {
                // Implement GetHashCode if needed
                throw new NotImplementedException();
            }
        }


        [Test]
        public void DeleteTest()
        {
            // Arrange
            IQuizService quizService = new QuizService(repository, questionRepository);
            var quiz = new Quiz
            {
                Title = "TestAddQuiz",
                Description = "TestTheDescription",
                Category = "TestCategory",
                TimeLimit = 10
            };
            var addedQuiz = quizService.Add(quiz);

            // Act
            var result = quizService.DeleteQuizIfNoQuestions(addedQuiz.QuizId);

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void GetQuizzesByCategoryTest()
        {
            // Arrange
            IQuizService quizService = new QuizService(repository, questionRepository);
            var category = "TestCategory";

            // Act
            var result = quizService.GetQuizzesByCategory(category);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2,result.Count); // Assuming there is at least one quiz with the specified category
        }

        [Test]
        public void GetQuizByIdTest()
        {
            // Arrange
            IQuizService quizService = new QuizService(repository, questionRepository);
            var quizId = 1;

            // Act
            var result = quizService.GetQuizById(quizId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(quizId, result.QuizId);
        }

        [Test]
        public async Task GetQuizByIdWithQuestionsTest()
        {
            // Arrange
            IQuizService quizService = new QuizService(repository, questionRepository);
            var quizId = 1;

            // Act
            var result = await quizService.GetQuizByIdWithQuestions(quizId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(quizId, result.QuizId);
            Assert.IsNotNull(result.Questions);
            // Add more assertions based on your specific logic
        }
    }
}
