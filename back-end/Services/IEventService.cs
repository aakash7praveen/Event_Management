using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Dtos.Responses;
using EventManagementAPI.Models.Requests;

namespace EventManagementAPI.Services
{
    public interface IEventService
    {
        Task<int> CreateEvent(CreateEventRequestDto createEventRequestDto);
        Task<EventResponseDto?> GetEventById(int id);
        Task<IEnumerable<EventResponseDto>> GetAllEvents();
        Task<bool> CancelEvent(int eventId);
        Task<IEnumerable<EventResponseDto>> GetTodaysEvents();
        Task<bool> RSVPToEvent(RsvpRequestDto request);
        Task<bool> RemoveRsvp(int userId, int eventId);
        Task<AdminDashboardMetricsDto> GetAdminMetrics();
        Task<IEnumerable<UserResponseDto>> GetUsersByEvent(int eventId);
        Task<bool> UpdateEvent(UpdateEventRequestDto request);
        Task<bool> HasUserRsvped(int userId, int eventId);
        Task<IEnumerable<CategoryCountDto>> GetEventCountByCategory();
        Task<IEnumerable<DailyEventCountDto>> GetEventCountPerDay();
        Task<IEnumerable<TopVenueDto>> GetTopVenues();
        Task<IEnumerable<RsvpCountDto>> GetRsvpCountPerEvent();
    }
}
