namespace EventManagementAPI.Dtos.Responses
{
    public class CategoryCountDto
    {
        public string Category { get; set; } = null!;
        public int EventCount { get; set; }
    }
}
