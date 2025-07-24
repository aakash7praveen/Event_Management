using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Dtos.Responses;
using EventManagementAPI.DTOs;
using EventManagementAPI.Models;

namespace EventManagementAPI.Services
{
    public interface IUserService
    {
        Task<bool> SignupAsync(SignupRequestDto req);
        Task<UserResponseDto?> LoginAsync(LoginRequestDto req);
        Task<bool> RegisterUserAsync(SignupRequestDto req);
        Task<bool> EmailExistsAsync(string email);
        Task<UserResponseDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<string> SaveProfilePictureAsync(IFormFile file);
        Task<IEnumerable<EventResponseDto>> GetEventsByUserAsync(int userId);
        Task<IEnumerable<EventResponseDto>> GetUpcomingEventsAsync(int userId);
        Task<IEnumerable<EventResponseDto>> GetPastEventsAsync(int userId);
        Task<IEnumerable<EventResponseDto>> GetAcceptedEventsAsync(int userId);
    }
}
