using EventManagementAPI.Models.Auth;

namespace EventManagementAPI.Helpers
{
    public static class RefreshTokenStore
    {
        public static List<RefreshToken> Tokens = new();
    }
}
