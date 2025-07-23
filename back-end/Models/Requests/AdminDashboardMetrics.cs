using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models.Requests
{
    public class AdminDashboardMetrics
    {
        public int TotalEvents { get; set; }
        public int UpcomingEvents { get; set; }
        public int TotalUsers { get; set; }
    }
}