using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Web.Services
{
    public class AuthService(AuthenticationStateProvider authenticationStateProvider)
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.Identity!.IsAuthenticated;
        }

        public async Task<string?> GetUserNameAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.FindFirst("unique_name")?.Value;
        }

        public async Task<string?> GetNameAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.FindFirst("Nombre")?.Value;
        }

        public async Task<string?> GetUserRoleAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;
        }

        public async Task<int> GetUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return 0;
        }
    }
}
