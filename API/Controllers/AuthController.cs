using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Helpers;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Authorisation _repo;

        public AuthController(Authorisation repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        /// <summary>
        /// The main registration method for new users
        /// </summary>
        /// <param name="userForRegisterDto">A data transfer object for the user</param>
        /// <returns>Returns 201 for success, or Bad Request if user already exists</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            System.Console.WriteLine("Registering...");
            System.Console.WriteLine("Username: " + userForRegisterDto.Username);

            // Get the username from the dto and convert it to lowercase for consistency
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            // Check that user doesn't already exist, if so return Bad Request
            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            // Create a new User object
            var newUser = new User { Username = userForRegisterDto.Username };

            // Use the Authorisation register method to create the user in the database
            var createdUser = await _repo.Register(newUser, userForRegisterDto.Password);

            return StatusCode(201,"User created!");

        }

        /// <summary>
        /// The main Login method for the application
        /// </summary>
        /// <param name="userForLoginDto">The data transfer object provided from the request</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // Use the authorisation login method to try to get the user from the database
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            // Test if user is admin
            bool isAdmin = false;
            if (userFromRepo.Role == 1)
                isAdmin = true;
                

            // If the user wasn't returned from the above method, return an unauthorised response
            if (userFromRepo == null)
                return Unauthorized();

            System.Console.WriteLine("Logging in user: " + userFromRepo.Username.ToString());
            
            // Success - return a token created from the helper class
            return Ok(new {
                token = JwtHelper.JwtToken(userFromRepo, _config, isAdmin)
            });


        }

    }
}