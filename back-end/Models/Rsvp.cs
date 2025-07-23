using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models
{
    public class Rsvp
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int EventId { get; set; }

        public string Status { get; set; } = "pending";

        public DateTime RsvpDate { get; set; } = DateTime.UtcNow;

        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? EventTitle { get; set; }
    }
}
