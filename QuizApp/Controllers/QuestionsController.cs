using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IQuizResultService _quizResultService;
        private readonly ILogger _logger;

        // Constructor injection of services
        public QuestionsController(IQuestionService questionService, ILogger<QuestionsController> logger, IQuizResultService quizResultService)
        {
            _questionService = questionService;
            _quizResultService = quizResultService;
            _logger = logger;
        }

        /// <summary>
        ///  Endpoint to add a question to a quiz
        /// </summary>
        /// <param name="questionDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = "Creator")]
        [HttpPost("add")]
        public IActionResult AddToQuiz(QuestionDTO questionDTO)
        {
            string errorMessage = string.Empty;
            try
            {
                // Attempt to add the question to the quiz
                var result = _questionService.AddToQuiz(questionDTO);

                if (result)
                    _logger.LogInformation("Added the question successfully");
                    return Ok(questionDTO); // Return success response
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            _logger.LogError("Failed to add the question");
            return BadRequest(errorMessage); // Return error response
        }

        /// <summary>
        ///  Endpoint to update a question in a quiz
        /// </summary>
        /// <param name="updatedQuestion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Creator")]
        [HttpPut("update")]
        public IActionResult UpdateQuestion([FromBody] QuestionDTO updatedQuestion)
        {
            try
            {
                // Validate input
                if (updatedQuestion == null)
                {
                    return BadRequest("Invalid request body");
                }

                // Attempt to update the question
                _questionService.UpdateQuestion(updatedQuestion.QuestionId, updatedQuestion);
                _logger.LogInformation($"Updated the question with ID {updatedQuestion.QuestionId} successfully.");
                return Ok($"Question with ID {updatedQuestion.QuestionId} updated successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to update the question");
                return BadRequest($"Failed to update the question. {e.Message}");
            }
        }


        /// <summary>
        ///  Endpoint to get all questions
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Creator")]
        [HttpGet("getAll")]
        public IActionResult GetAllQuestions()
        {
            string errorMessage = string.Empty;
            try
            {
                // Get all questions
                var questions = _questionService.GetAllQuestions();
                _logger.LogInformation("Got All the Questions successfully");
                return Ok(questions); // Return the questions
            }
            catch (NoQuestionsAvailableException e)
            {
                errorMessage = e.Message;
            }
            _logger.LogInformation("Failed to get the questions");
            return BadRequest(errorMessage); // Return error response
        }

        /// <summary>
        ///  Endpoint to get questions by quiz ID
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("byquiz/{quizId}")]
        public ActionResult<IEnumerable<Questions>> GetQuestionsByQuizId(int quizId)
        {
            try
            {
                // Get questions by quiz ID
                var questions = _questionService.GetQuestionsByQuizId(quizId);
                _logger.LogInformation("Got the QuestionsByQuizId successfully");
                return Ok(questions); // Return the questions
            }
            catch (NoQuestionsAvailableException)
            {
                _logger.LogError("No questions found in the given quiz");
                return NotFound($"No questions found for Quiz ID {quizId}."); // Return error response
            }
            
        }

        /// <summary>
        ///  Endpoint to remove a question from a quiz
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        [Authorize(Roles = "Creator")]
        [HttpDelete("Remove")]
        public IActionResult RemoveFromQuiz(int questionid)
        {
            // Attempt to remove the question from the quiz
            var result = _questionService.RemoveFromQuiz(questionid);

            if (result)
            {
                _logger.LogInformation("Deleted the Questions successfully");
                return Ok("Deleted Question Successfully"); // Return success response
            }
            _logger.LogError("Couldn't delete the question from the required quiz");
            return BadRequest("Could not remove the Question from quiz"); // Return error response
        }
    }
}
