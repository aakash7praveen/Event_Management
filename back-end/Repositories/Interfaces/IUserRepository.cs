using EventManagementAPI.Models;
using EventManagementAPI.Models.Requests;

namespace EventManagementAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> RegisterUserAsync(SignupRequest request);
        Task<IEnumerable<SystemUser>> GetAllUsersAsync();
        Task<string> SaveProfilePictureAsync(IFormFile file);
        Task<SystemUser?> GetUserByEmailAsync(string email);
        Task<IEnumerable<Event>> GetEventsByUserAsync(int userId);
        Task<IEnumerable<Event>> GetUpcomingEventsAsync(int userId);
        Task<IEnumerable<Event>> GetPastEventsAsync(int userId);
        Task<IEnumerable<Event>> GetAcceptedEventsAsync(int userId);

    }
}
