using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventManagementAPI.Models
{
    public class Event
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("startDt")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endDt")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("duration")]
        public TimeSpan Duration => EndTime - StartTime;

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("maxAttendees")]
        public int? MaxAttendees { get; set; }

        [JsonPropertyName("createdBy")]
        public int Created_By { get; set; }

        [JsonPropertyName("createdTs")]
        public DateTime Created_Ts { get; set; }

        [JsonPropertyName("updatedTs")]
        public DateTime Updated_Ts { get; set; }

        [JsonPropertyName("deleteInd")]
        public int Delete_Ind { get; set; } = 0;
    }
}
