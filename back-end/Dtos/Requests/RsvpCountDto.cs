namespace EventManagementAPI.Dtos.Responses
{
    public class RsvpCountDto
    {
        public string EventTitle { get; set; } = null!;
        public int RsvpCount { get; set; }
    }
}
