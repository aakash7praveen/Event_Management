using Microsoft.AspNetCore.Mvc;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Repositories;

namespace EventManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest req)
        {
            if (req.Password != req.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match" });

            if (await _userRepo.EmailExistsAsync(req.Email))
                return BadRequest(new { message = "Email already exists" });

            var success = await _userRepo.RegisterUserAsync(req);
            return success
                ? Ok(new { message = "User registered successfully" })
                : StatusCode(500, new { message = "Failed to register user" });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userRepo.GetUserByEmailAsync(model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Enc_Password))
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new
            {
                userId = user.User_Id,
                firstName = user.First_Name,
                role = user.Role,
                profilePicture = user.Profile_Picture
            });
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            try
            {
                var relativePath = await _userRepo.SaveProfilePictureAsync(file);
                return Ok(new { filePath = relativePath });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/events")]
        public async Task<IActionResult> GetEventsByUser(int id)
        {
            var events = await _userRepo.GetEventsByUserAsync(id);
            return Ok(events);
        }

        [HttpGet("{id}/events/upcoming")]
        public async Task<IActionResult> GetUpcomingEvents(int id)
        {
            var events = await _userRepo.GetUpcomingEventsAsync(id);
            return Ok(events);
        }

        [HttpGet("{id}/events/past")]
        public async Task<IActionResult> GetPastEvents(int id)
        {
            var events = await _userRepo.GetPastEventsAsync(id);
            return Ok(events);
        }

        [HttpGet("{id}/events/accepted")]
        public async Task<IActionResult> GetAcceptedEvents(int id)
        {
            var events = await _userRepo.GetAcceptedEventsAsync(id);
            return Ok(events);
        }


    }
}
