using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Dtos.Responses;
using EventManagementAPI.DTOs;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequestDto req)
        {
            try
            {
                var success = await _userService.SignupAsync(req);
                if (success)
                {
                    return Ok(new { message = "User registered successfully" });
                }
                return BadRequest(new { message = "Failed to register user" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem("An unexpected error occurred while signing up.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
        {
            var userDto = await _userService.LoginAsync(req);
            if (userDto == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new
            {
                userId = userDto.UserId,
                firstName = userDto.FirstName,
                role = userDto.Role,
                profilePicture = userDto.ProfilePicture
            });
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            try
            {
                var relativePath = await _userService.SaveProfilePictureAsync(file);
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
            try
            {
                var events = await _userService.GetEventsByUserAsync(id);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/events/upcoming")]
        public async Task<IActionResult> GetUpcomingEvents(int id)
        {
            try
            {
                var events = await _userService.GetUpcomingEventsAsync(id);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/events/past")]
        public async Task<IActionResult> GetPastEvents(int id)
        {
            try
            {
                var events = await _userService.GetPastEventsAsync(id);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/events/accepted")]
        public async Task<IActionResult> GetAcceptedEvents(int id)
        {
            try
            {
                var events = await _userService.GetAcceptedEventsAsync(id);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
