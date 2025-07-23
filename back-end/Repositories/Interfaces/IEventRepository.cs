using EventManagementAPI.Models;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Models.Analytics;

namespace EventManagementAPI.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<int> CreateEventAsync(CreateEventRequest request);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<int> CancelEventAsync(int eventId);
        Task<Event?> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetTodaysEventsAsync();
        Task<bool> RsvpToEventAsync(RsvpRequest request);
        Task<bool> RemoveRsvpAsync(int userId, int eventId);
        Task<AdminDashboardMetrics> GetAdminMetricsAsync();
        Task<IEnumerable<SystemUser>> GetUsersByEventAsync(int eventId);
        Task<bool> UpdateEventAsync(UpdateEventRequest request);
        Task<bool> HasUserRsvpedAsync(int userId, int eventId);
        Task<IEnumerable<EventCategoryCount>> GetEventCountByCategoryAsync();
        Task<IEnumerable<DailyEventCount>> GetEventCountPerDayAsync();
        Task<IEnumerable<VenueEventCount>> GetTopVenuesAsync();
        Task<IEnumerable<EventRsvpCount>> GetRsvpCountPerEventAsync();

    }
}
