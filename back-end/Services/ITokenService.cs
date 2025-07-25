using EventManagementAPI.Models;
using EventManagementAPI.Models.Auth;
using System.Security.Claims;

namespace EventManagementAPI.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(SystemUser user);
        RefreshToken GenerateRefreshToken(int userId);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }

}
