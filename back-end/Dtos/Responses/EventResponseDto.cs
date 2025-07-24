namespace EventManagementAPI.Dtos.Responses
{
    public class EventResponseDto
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string? Category { get; set; }
        public int CreatedBy { get; set; }
        public int MaxAttendees { get; set; }
    }
}
