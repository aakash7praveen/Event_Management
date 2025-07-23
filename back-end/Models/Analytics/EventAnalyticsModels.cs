namespace EventManagementAPI.Models.Analytics
{
    public class EventCategoryCount
    {
        public string Category { get; set; }
        public int Count { get; set; }
    }

    public class DailyEventCount
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class VenueEventCount
    {
        public string Venue { get; set; }
        public int Count { get; set; }
    }

    public class EventRsvpCount
    {
        public string Title { get; set; }
        public int RsvpCount { get; set; }
    }

}
