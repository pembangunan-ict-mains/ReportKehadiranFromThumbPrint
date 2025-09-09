using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;


namespace IdentityAuthentication
{
    public class IdentityAuthenticationLib : IIdentityAuthenticationLib
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly NavigationManager _navMan;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public IdentityAuthenticationLib(IHttpContextAccessor httpContextAccessor, NavigationManager navMan, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _navMan = navMan;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SignInUserAsync(string xUserId, string xRoles, string strName, string xUNIT)
        {
            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.Name, xUserId.ToString()),
                  new Claim(ClaimTypes.GivenName ,strName),
                  new Claim(ClaimTypes.Role, xRoles),
                  new Claim(ClaimTypes.StateOrProvince, xUNIT)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                await httpContext.SignInAsync(principal);
               // _navMan.NavigateTo(_navMan.ToAbsoluteUri("Home").ToString());
            }
        }

        public async Task SignOutUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null && !httpContext.Response.HasStarted)
            {
                await httpContext.SignOutAsync();
            }

        }


        public async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return await _authenticationStateProvider.GetAuthenticationStateAsync();
        }

        public async Task<ClaimsPrincipal> GetUserAsync()
        {
            var authState = await GetAuthenticationStateAsync();
            return authState.User;
        }

      

    }

}