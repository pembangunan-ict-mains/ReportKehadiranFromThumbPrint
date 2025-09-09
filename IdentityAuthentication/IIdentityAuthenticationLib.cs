
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace IdentityAuthentication
{
    public interface IIdentityAuthenticationLib
    {
        Task SignInUserAsync(string xUserId, string xRoles, string strName, string xUNIT);
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task<ClaimsPrincipal> GetUserAsync();
        Task SignOutUserAsync();
    }
}