namespace EventManagementAPI.Dtos.Responses
{
    public class RsvpCountDto
    {
        public int EventId { get; set; }
        public string EventTitle { get; set; } = null!;
        public int RsvpCount { get; set; }
    }
}
