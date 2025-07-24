using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models.Requests
{
    public class CreateEventRequest
    {
        // Implement Data annotations
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int CreatedBy { get; set; }
        public int? MaxAttendees { get; set; }
    }
}