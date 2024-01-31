using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Exceptions;
using QuizApp.Interfaces;
using QuizApp.Models.DTOs;
using QuizApp.Models;
using QuizApp.Services;
using System;
using System.Collections.Generic;

namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("reactApp")]
    public class QuizResultController : ControllerBase
    {
        private readonly IQuizResultService _quizResultService;
        private readonly ILogger _logger;

        // Inject the quiz result service through constructor injection
        public QuizResultController(IQuizResultService quizResultService, ILogger<QuizResultController> logger)
        {
            _quizResultService = quizResultService;
            _logger = logger;
        }

        /// <summary>
        ///  Endpoint to get quiz results by quiz ID
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Creator")]
        [HttpGet("byQuiz/{quizId}")]
        public ActionResult<IEnumerable<QuizResult>> GetResultsByQuiz(int quizId)
        {
            try
            {
                // Get quiz results for the specified quiz ID
                var results = _quizResultService.GetResultsByQuiz(quizId);
                _logger.LogInformation("Got the QuizResults successfully");
                return Ok(results); // Return the quiz results
            }
            catch (NoQuizResultsAvailableException e)
            {
                _logger.LogError("Could not get quizResults");
                return NotFound($"No quiz results found for Quiz ID {quizId}. {e.Message}");
            }
        }
        /// <summary>
        ///  Endpoint to get all the quiz results.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="Creator")]
        [HttpGet("AllquizResults")]
        public ActionResult<IEnumerable<QuizResult>> GetAllQuizResults()
        {
            try
            {
                // Get quiz results for the specified quiz ID
                var results = _quizResultService.GetAllQuizResults();
                _logger.LogInformation("Got the QuizResults successfully");
                return Ok(results); // Return the quiz results
            }
            catch (NoQuizResultsAvailableException e)
            {
                _logger.LogError("Could not get quizResults");
                return NotFound($"No quiz results found . {e.Message}");
            }
        }

        /// <summary>
        ///  Endpoint to get quiz results with total score by user and quiz ID
        /// </summary>
        /// <param name="username"></param>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("results-with-total-score/{username}/{quizId}")]
        public ActionResult<QuizResultsWithTotalScoreDTO> GetResultsWithTotalScoreByUserAndQuiz(string username, int quizId)
        {
            try
            {
                // Get quiz results for the specified user and quiz ID
                var results = _quizResultService.GetResultsByUserAndQuiz(username, quizId);

                // Get total score for the specified user and quiz ID
                var totalScore = _quizResultService.GetTotalScoreForUserInQuiz(quizId, username);

                // Create DTO containing both quiz results and total score
                var resultsWithTotalScoreDTO = new QuizResultsWithTotalScoreDTO
                {
                    TotalScore = totalScore,
                    QuizResults = results
                };
                _logger.LogInformation("Got the QuizResults along with totalscore");
                return Ok(resultsWithTotalScoreDTO); // Return the DTO
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get quizresults with total score");
                return BadRequest($"Failed to retrieve quiz results with total score. {e.Message}");
            }
        }

        /// <summary>
        ///  Endpoint to get total score by quiz and username
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("totalscore/{quizId}/{username}")]
        public ActionResult<int> GetTotalScoreForUserInQuiz(int quizId, string username)
        {
            try
            {
                // Get total score for the specified user and quiz ID
                var totalScore = _quizResultService.GetTotalScoreForUserInQuiz(quizId, username);
                _logger.LogInformation("Got the total score");
                // Return the total score
                return Ok("The Total Score is:" + totalScore);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get the totalscore");
                return BadRequest($"Failed to get total score. {e.Message}");
            }
        }
        /// <summary>
        ///  Endpoint to get the list of quiz ids that are already answered by the user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("answered-quiz-ids/{username}")]
        public ActionResult<int[]> GetAnsweredQuizIds(string username)
        {
            try
            {
                var answeredQuizIds = _quizResultService.GetAnsweredQuizIdsForUser(username);
                return Ok(answeredQuizIds);
            }
            catch (NoQuizResultsAvailableException)
            {
                return NotFound("No quiz results available for the specified user.");
            }
        }
    }

}
