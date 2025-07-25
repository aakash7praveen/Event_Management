using EventManagementAPI.Helpers;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Auth;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagementAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepo;

        public AuthController(ITokenService tokenService, IUserRepository userRepo)
        {
            _tokenService = tokenService;
            _userRepo = userRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepo.GetUserByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Enc_Password))
                return Unauthorized("Invalid credentials");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user.User_Id);

            RefreshTokenStore.Tokens.Add(refreshToken);

            return Ok(new JwtTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] JwtTokenResponse tokenRequest)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
            if (principal == null)
                return BadRequest("Invalid access token");

            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var stored = RefreshTokenStore.Tokens
                .FirstOrDefault(rt => rt.UserId == userId && rt.Token == tokenRequest.RefreshToken);

            if (stored == null || stored.Expires < DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            var user = new SystemUser
            {
                User_Id = userId,
                Email = principal.FindFirst(ClaimTypes.Email)!.Value,
                Role = (principal.FindFirst(ClaimTypes.Role)?.Value == "Admin" ? 1 : 0)
            };

            var newAccess = _tokenService.GenerateAccessToken(user);
            var newRefresh = _tokenService.GenerateRefreshToken(userId);

            RefreshTokenStore.Tokens.Remove(stored);
            RefreshTokenStore.Tokens.Add(newRefresh);

            return Ok(new JwtTokenResponse
            {
                AccessToken = newAccess,
                RefreshToken = newRefresh.Token
            });
        }
    }
}
