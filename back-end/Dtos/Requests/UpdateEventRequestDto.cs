using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Dtos.Requests
{
    public class UpdateEventRequestDto
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        [MaxLength(255)]
        public string Location { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        public int? MaxAttendees { get; set; }
    }
}
