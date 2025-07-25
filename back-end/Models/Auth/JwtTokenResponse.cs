namespace EventManagementAPI.Models.Auth
{
    public class JwtTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
