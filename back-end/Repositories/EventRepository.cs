using Dapper;
using EventManagementAPI.Helpers;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Models.Analytics;

namespace EventManagementAPI.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly DbHelper _db;

        public EventRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<int> CreateEventAsync(CreateEventRequest request)
        {
            var sql = @"INSERT INTO [event] (title, description, start_dt, end_dt, location, category, created_by, max_attendees, delete_ind)
                        VALUES (@Title, @Description, @StartDateTime, @EndDateTime, @Location, @Category, @CreatedBy, @MaxAttendees, 0)";
            return await _db.ExecuteAsync(sql, request);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            var sql = @"SELECT event_id AS Id,
            title AS Title,
            description AS Description,
            start_dt AS StartTime,
            end_dt AS EndTime,
            location AS Location,
            category AS Category,
            created_by AS Created_By,
            created_ts AS Created_Ts,
            updated_ts AS Updated_Ts,
            max_attendees AS MaxAttendees,
            delete_ind AS Delete_Ind FROM [event] WHERE delete_ind = 0 ORDER BY start_dt";
            return await _db.QueryAsync<Event>(sql);
        }

        public async Task<int> CancelEventAsync(int eventId)
        {
            var sql = "UPDATE [event] SET delete_ind = 1, updated_ts = GETDATE() WHERE event_id = @EventId";
            return await _db.ExecuteAsync(sql, new { EventId = eventId });
        }


        public async Task<IEnumerable<Event>> GetTodaysEventsAsync()
        {
            var sql = @"
        SELECT 
            e.event_id AS Id,
            e.title AS Title,
            e.description AS Description,
            e.start_dt AS StartTime,
            e.end_dt AS EndTime,
            e.location AS Location,
            e.category AS Category,
            e.max_attendees AS MaxAttendees,
            e.created_by AS Created_By,
            e.created_ts AS Created_Ts,
            e.updated_ts AS Updated_Ts,
            e.delete_ind AS Delete_Ind
            FROM [event] e
            WHERE CAST(e.start_dt AS DATE) = CAST(GETDATE() AS DATE) AND e.delete_ind = 0";

            return await _db.QueryAsync<Event>(sql);
        }


        public async Task<Event?> GetEventByIdAsync(int id)
        {
            var sql = @"SELECT event_id AS Id,
            title AS Title,
            description AS Description,
            start_dt AS StartTime,
            end_dt AS EndTime,
            location AS Location,
            category AS Category,
            created_by AS Created_By,
            created_ts AS Created_Ts,
            updated_ts AS Updated_Ts,
            max_attendees AS MaxAttendees,
            delete_ind AS Delete_Ind FROM [event] WHERE event_id = @Id";
            return await _db.QuerySingleAsync<Event>(sql, new { Id = id });
        }

        public async Task<bool> RsvpToEventAsync(RsvpRequest request)
        {
            var sql = @"INSERT INTO rsvp (user_id, event_id, status) VALUES (@UserId, @EventId, @Status)";
            var result = await _db.ExecuteAsync(sql, request);
            return result > 0;
        }

        public async Task<bool> RemoveRsvpAsync(int userId, int eventId)
        {
            var sql = @"DELETE FROM rsvp WHERE user_id = @UserId AND event_id = @EventId";
            return await _db.ExecuteAsync(sql, new { UserId = userId, EventId = eventId }) > 0;
        }

        public async Task<AdminDashboardMetrics> GetAdminMetricsAsync()
        {
            var sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM [event] WHERE delete_ind = 0) AS TotalEvents,
                    (SELECT COUNT(*) FROM [event] WHERE start_dt > GETDATE() AND delete_ind = 0) AS UpcomingEvents,
                    (SELECT COUNT(*) FROM [user] WHERE role = 0) AS TotalUsers";

            var result = await _db.QuerySingleAsync<AdminDashboardMetrics>(sql);
            return result ?? throw new InvalidOperationException("Failed to retrieve admin metrics.");
        }

        public async Task<IEnumerable<SystemUser>> GetUsersByEventAsync(int eventId)
        {
            var sql = @"SELECT DISTINCT u.* FROM [user] u
                INNER JOIN rsvp r ON u.user_id = r.user_id
                WHERE r.event_id = @EventId AND r.status = 'confirmed'";
            return await _db.QueryAsync<SystemUser>(sql, new { EventId = eventId });
        }

        public async Task<bool> UpdateEventAsync(UpdateEventRequest request)
        {
            var sql = @"UPDATE [event]
                SET title = @Title,
                    description = @Description,
                    start_dt = @StartDt,
                    end_dt = @EndDt,
                    location = @Location,
                    category = @Category,
                    max_attendees = @MaxAttendees,
                    updated_ts = GETDATE()
                WHERE event_id = @EventId AND delete_ind = 0";

            return await _db.ExecuteAsync(sql, request) > 0;
        }

        public async Task<bool> HasUserRsvpedAsync(int userId, int eventId)
        {
            var sql = @"SELECT COUNT(1) FROM rsvp 
                WHERE user_id = @UserId AND event_id = @EventId AND status = 'confirmed'";
            var count = await _db.QuerySingleAsync<int>(sql, new { UserId = userId, EventId = eventId });
            return count > 0;
        }

        public async Task<IEnumerable<EventCategoryCount>> GetEventCountByCategoryAsync()
        {
            var sql = @"SELECT category AS Category, COUNT(*) AS Count FROM [event] WHERE delete_ind = 0 GROUP BY category";
            return await _db.QueryAsync<EventCategoryCount>(sql);
        }

        public async Task<IEnumerable<DailyEventCount>> GetEventCountPerDayAsync()
        {
            var sql = @"SELECT CAST(start_dt AS DATE) AS Date, COUNT(*) AS Count FROM [event] WHERE start_dt >= GETDATE() GROUP BY CAST(start_dt AS DATE) ORDER BY Date";
            return await _db.QueryAsync<DailyEventCount>(sql);
        }

        public async Task<IEnumerable<VenueEventCount>> GetTopVenuesAsync()
        {
            var sql = @"SELECT location AS Venue, COUNT(*) AS Count FROM [event] WHERE delete_ind = 0 GROUP BY location ORDER BY Count DESC";
            return await _db.QueryAsync<VenueEventCount>(sql);
        }

        public async Task<IEnumerable<EventRsvpCount>> GetRsvpCountPerEventAsync()
        {
            var sql = @"SELECT e.title AS Title, COUNT(r.user_id) AS RsvpCount
                FROM [event] e
                LEFT JOIN rsvp r ON e.event_id = r.event_id AND r.status = 'confirmed'
                WHERE e.delete_ind = 0
                GROUP BY e.title
                ORDER BY RsvpCount DESC";
            return await _db.QueryAsync<EventRsvpCount>(sql);
        }


    }
}
