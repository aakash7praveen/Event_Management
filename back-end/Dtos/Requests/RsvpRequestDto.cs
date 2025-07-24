using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Dtos.Requests
{
    public class RsvpRequestDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } 
    }
}
