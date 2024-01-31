using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Exceptions;
using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Models.DTOs;
using QuizApp.Services;

namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("reactApp")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;
        private readonly IQuizResultService _quizResultService;
        private readonly ILogger _logger;

        // Constructor injection of services
        public QuizController(IQuizService quizService, IQuestionService questionService, ILogger<QuestionsController> logger,IQuizResultService quizResultService)
        {
            _quizService = quizService;
            _questionService = questionService;
            _quizResultService = quizResultService;
            _logger = logger;
        }

        /// <summary>
        ///  Endpoint to get all quizzes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            string errorMessage = string.Empty;
            try
            {
                // Attempt to get all quizzes
                var result = _quizService.GetQuizs();
                _logger.LogInformation("Got the quizs successfully");
                return Ok(result); // Return success response
            }
            catch (NoQuizsAvailableException e)
            {
                errorMessage = e.Message;
            }
            _logger.LogError("Failed to get the quizs");
            return BadRequest(errorMessage); // Return error response
        }
        /// <summary>
        /// Endpoint to get the list of categories from the quiz table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        public ActionResult<string> GetCategories()
        {
            string errorMessage = string.Empty;
            try
            {
                var result = _quizService.GetCategories();
                _logger.LogInformation("Received list of category");
                return Ok(result);
            }
            catch(Exception e)
            {
                errorMessage = e.Message;
            }
            _logger.LogError("Failed to get the category List");
            return BadRequest(errorMessage);
        }
        /// <summary>
        /// Endpoint to Get the list of titles from the quiz table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("titles")]
        public ActionResult<string> GetTitles()
        {
            string errorMessage = string.Empty;
            try
            {
                var result = _quizService.GetTitles();
                _logger.LogInformation("Received list of titles");
                return Ok(result);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            _logger.LogError("Failed to get the titles List");
            return BadRequest(errorMessage);
        }
        /// <summary>
        /// Endpoint to Get the Id of quiz using the quiz title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("quizId")]
        public ActionResult<string> GetId(string title)
        {
            string errorMessage = string.Empty;
            try
            {
                var result = _quizService.GetId(title);
                _logger.LogInformation("Received id of quiz with Its title");
                return Ok(result);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            _logger.LogError("Failed to get the Id of quiz with given title.");
            return BadRequest(errorMessage);
        }


        /// <summary>
        ///  Endpoint to create a new quiz
        /// </summary>
        /// <param name="quiz"></param>
        /// <returns></returns>
        [Authorize(Roles = "Creator")]
        [HttpPost]
        public ActionResult Create(Quiz quiz)
        {
            string errorMessage = string.Empty;
            try
            {
                // Attempt to add a new quiz
                var result = _quizService.Add(quiz);
                _logger.LogInformation("Created the quiz successfully");
                return Ok(result); // Return success response
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            _logger.LogError("Failed to create Quiz");
            return BadRequest(errorMessage); // Return error response
        }

        /// <summary>
        ///  Endpoint to get quizzes by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet("category/{category}")]
        public ActionResult<IList<QuizDTO>> GetQuizzesByCategory(string category)
        {
            try
            {
                // Attempt to get quizzes by category
                var quizzes = _quizService.GetQuizzesByCategory(category);
                _logger.LogInformation("Got the quizs by category successfully");
                return Ok(quizzes); // Return success response
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get the quizs by category");
                return BadRequest($"Failed to retrieve quizzes. {e.Message}"); // Return error response
            }
        }

        /// <summary>
        ///  Endpoint to get questions for a quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("quiz/{quizId}/questions")]
        public ActionResult<IEnumerable<QuestionDTO>> GetQuestionsForQuiz(int quizId)
        {
            try
            {
                // Attempt to get questions by quiz ID
                var questions = _questionService.GetQuestionsByQuizId(quizId);
                _logger.LogInformation("Got the questions for quizs successfully");
                return Ok(questions); // Return success response
            }
            catch (NoQuestionsAvailableException e)
            {
                _logger.LogError("Failed to get questions for Quiz");
                return NotFound($"No questions found for Quiz ID {quizId}. {e.Message}"); // Return error response
            }
        }

        /// <summary>
        ///  Endpoint to evaluate an answer for a quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="answerDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("evaluate/{quizId}")]
        public ActionResult<QuizResultDTO> EvaluateAnswer(int quizId, [FromBody] AnswerDTO answerDTO)
        {
            try
            {
                // Fetch questions for the given quiz
                var questions = _questionService.GetQuestionsByQuizId(quizId);

                if (questions.Count == 0)
                {
                    return NotFound($"No questions found for Quiz ID {quizId}");
                }

                // Display the questions along with their real question IDs
                var questionsWithIds = questions.Select(q => new QuestionDTO
                {
                    QuestionId = q.QuestionId,
                    QuizId = q.QuizId,
                    QuestionTxt = q.QuestionTxt,
                    Option1 = q.Option1,
                    Option2 = q.Option2,
                    Option3 = q.Option3,
                    Option4 = q.Option4,
                }).ToList();

                // Log or return the questions to the user (this depends on frontend implementation)

                // Evaluating the answers
                var result = _quizResultService.EvaluateAnswer(quizId, answerDTO);
                _logger.LogInformation("Evaluated the questions successfully");
                return Ok(result); // Return success response
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to evaluate the answer");
                return BadRequest($"Failed to evaluate the answer. {e.Message}"); // Return error response
            }
        }

        /// <summary>
        ///  Endpoint to get leaderboard for a quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("leaderboard/{quizId}")]
        public ActionResult<IEnumerable<LeaderboardEntryDTO>> GetLeaderboard(int quizId)
        {
            // Attempt to get the leaderboard for the quiz
            var leaderboard = _quizResultService.GetLeaderboard(quizId);

            if (leaderboard == null || !leaderboard.Any())
            {
                _logger.LogError("Failed to load the leaderboard");
                return NotFound($"No leaderboard found for Quiz ID {quizId}"); // Return error response
            }
            _logger.LogInformation("Successfully loaded the leaderboard");
            return Ok(leaderboard); // Return success response
        }

        /// <summary>
        ///  Endpoint to delete a quiz if it has no associated questions
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Creator")]
        [HttpDelete("{quizId}")]
        public IActionResult DeleteQuiz(int quizId)
        {
            try
            {
                // Attempt to delete the quiz if it has no associated questions
                var deleted = _quizService.DeleteQuizIfNoQuestions(quizId);

                if (deleted)
                {
                    _logger.LogInformation("Deleted the quiz successfully");
                    return Ok($"Quiz with ID {quizId} deleted successfully."); // Return success response
                }
                _logger.LogError("Failed to delete the Quiz since it has questions in it");
                return BadRequest($"Cannot delete the quiz with ID {quizId} as it has questions associated with it."); // Return error response
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete the quiz");
                return StatusCode(500, $"An error occurred: {ex.Message}"); // Return server error response
            }
        }

        /// <summary>
        ///  Endpoint to update a quiz
        /// </summary>
        /// <param name="updatedQuiz"></param>
        /// <returns></returns>
        [Authorize(Roles = "Creator")]
        [HttpPut("update")]
        public IActionResult UpdateQuiz(Quiz updatedQuiz)
        {
            try
            {

                // Get the existing quiz from the service
                var existingQuiz = _quizService.GetQuizById(updatedQuiz.QuizId);

                // Check if the quiz exists
                if (existingQuiz == null)
                {
                    _logger.LogError("Quiz is not found");
                    return NotFound($"Quiz with ID {updatedQuiz.QuizId} not found.");
                }

                // Update the properties of the existing quiz with the values from the updatedQuiz
                existingQuiz.Title = updatedQuiz.Title;
                existingQuiz.Description = updatedQuiz.Description;
                existingQuiz.TimeLimit = updatedQuiz.TimeLimit;

                // Update the quiz in the service
                _quizService.UpdateQuiz(existingQuiz);
                _logger.LogInformation("Updated the quiz successfully");
                return Ok("Quiz updated successfully."); // Return success response
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update the Quiz");
                return StatusCode(500, $"An error occurred: {ex.Message}"); // Return server error response
            }
        }
    }
}
