using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace QuizApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<string, User> _repository;
        private readonly ITokenService _tokenService;

        public UserService(IRepository<string, User> repository, ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }
        /// <summary>
        /// Login to the quiz app if the user credentials are valid.
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public UserDTO Login(UserDTO userDTO)
        {
            var user = _repository.GetById(userDTO.Username);
            if (user != null)
            {
                HMACSHA512 hmac = new HMACSHA512(user.Key);
                var userpass = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
                for (int i = 0; i < userpass.Length; i++)
                {
                    if (user.Password[i] != userpass[i])
                        return null;
                }

                // Fetch user's role from the database
                var userFromDatabase = _repository.GetById(userDTO.Username);
                if (userFromDatabase != null)
                {
                    userDTO.Role = userFromDatabase.Role;
                    userDTO.Email=userFromDatabase.Email;
                    userDTO.Token = _tokenService.GetToken(userDTO);
                    userDTO.Password = "";
                    return userDTO;
                }
            }
            return null;
        }
        /// <summary>
        /// Register to the quiz app validating the details given by user.
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public UserDTO Register(UserDTO userDTO)
        {
            HMACSHA512 hmac = new HMACSHA512();
            User user = new User()
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password)),
                Key = hmac.Key,
                Role = userDTO.Role
            };
            var result = _repository.Add(user);
            if (result != null)
            {
                userDTO.Password = "";
                return userDTO;
            }
            return null;
        }
    }
}
