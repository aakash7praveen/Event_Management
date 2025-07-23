using Dapper;
using EventManagementAPI.Helpers;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Repositories.Interfaces;

namespace EventManagementAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbHelper _dbHelper;

        public UserRepository(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var sql = "SELECT COUNT(1) FROM [user] WHERE email = @Email";
            var count = await _dbHelper.QuerySingleAsync<int>(sql, new { Email = email });
            return count > 0;
        }

        public async Task<bool> RegisterUserAsync(SignupRequest req)
        {
            var sql = @"
                INSERT INTO [user] 
                (first_name, middle_name, last_name, email, enc_password, phone_number, profile_picture, role, created_ts)
                VALUES
                (@FirstName, @MiddleName, @LastName, @Email, @HashedPassword, @PhoneNumber, @ProfilePicture, 0, GETDATE())";

            var hashed = BCrypt.Net.BCrypt.HashPassword(req.Password);

            var result = await _dbHelper.ExecuteAsync(sql, new
            {
                req.FirstName,
                req.MiddleName,
                req.LastName,
                req.Email,
                HashedPassword = hashed,
                req.PhoneNumber,
                req.ProfilePicture
            });

            return result > 0;
        }

        public async Task<IEnumerable<SystemUser>> GetAllUsersAsync()
        {
            var sql = @"
            SELECT 
            user_id AS User_Id,
            first_name AS First_Name,
            middle_name AS Middle_Name,
            last_name AS Last_Name,
            email AS Email,
            phone_number AS Phone_Number,
            profile_picture AS Profile_Picture,
            role AS Role,
            created_ts AS Created_Ts,
            enc_password AS Enc_Password
            FROM [user]
            WHERE role = 0"; 

            var users = await _dbHelper.QueryAsync<SystemUser>(sql);
            return users;
        }

        public async Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "ProfilePics");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/Uploads/ProfilePics/{fileName}";
        }

        public async Task<SystemUser?> GetUserByEmailAsync(string email)
        {
            var sql = "SELECT * FROM [user] WHERE email = @Email";
            return await _dbHelper.QuerySingleAsync<SystemUser>(sql, new { Email = email });
        }

        public async Task<IEnumerable<Event>> GetEventsByUserAsync(int userId)
        {
            var sql = @"SELECT 
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
                e.delete_ind AS Delete_Ind FROM [event] e
                INNER JOIN rsvp r ON e.event_id = r.event_id
                WHERE r.user_id = @UserId AND r.status = 'confirmed' AND e.end_dt >= GETDATE()";
            return await _dbHelper.QueryAsync<Event>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync(int userId)
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
            WHERE e.start_dt > GETDATE()
            AND e.delete_ind = 0
            AND NOT EXISTS (
              SELECT 1 FROM rsvp r
              WHERE r.event_id = e.event_id AND r.user_id = @UserId AND r.status = 'confirmed'
            )";

            return await _dbHelper.QueryAsync<Event>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<Event>> GetPastEventsAsync(int userId)
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
            JOIN rsvp r ON e.event_id = r.event_id
            WHERE r.user_id = @UserId AND r.status = 'confirmed'
            AND e.end_dt < GETDATE()
            AND e.delete_ind = 0";

            return await _dbHelper.QueryAsync<Event>(sql, new { UserId = userId });
        }


        public async Task<IEnumerable<Event>> GetAcceptedEventsAsync(int userId)
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
            JOIN rsvp r ON e.event_id = r.event_id
            WHERE r.user_id = @UserId AND r.status = 'confirmed'
            AND e.start_dt > GETDATE()
            AND e.delete_ind = 0";

            return await _dbHelper.QueryAsync<Event>(sql, new { UserId = userId });
        }

    }
}
