using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Interfaces;
using QuizApp.Models.DTOs;
namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("reactApp")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Drives the control of the user to make registration.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public ActionResult Register(UserDTO viewModel)
        {
            string message = "";
            try
            {
                var user = _userService.Register(viewModel);
                if (user != null)
                {
                    return Ok(user);
                }
            }
            catch (DbUpdateException)
            {
                message = "Duplicate username";
            }
            catch (Exception)
            {

            }


            return BadRequest(message);
        }
        /// <summary>
        /// Drives the user to login to the quiz app.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public ActionResult Login(UserDTO viewModel)
        {
            string message = "";
            var result = _userService.Login(viewModel);
            if (result != null)
            {
                return Ok(result);
            }
            message = "Invalid username or password";
            return BadRequest(message);
        }
    }
}
