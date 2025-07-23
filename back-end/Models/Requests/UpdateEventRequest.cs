namespace EventManagementAPI.Models.Requests
{
    public class UpdateEventRequest
    {
        public int EventId { get; set; }  
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public int? MaxAttendees { get; set; }
    }

}
