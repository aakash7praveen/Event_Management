
using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models.Requests
{
    public class RsvpRequest
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        
        [Required]
        public string Status { get; set; }
    }
}