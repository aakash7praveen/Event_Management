using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models.Requests
{
    public class SignupRequest
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
