using BillWare.App.Intefaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security;
using System.Security.Claims;

namespace BillWare.App.Helpers
{
    public class BeamAuthenticationStateProviderHelper : AuthenticationStateProvider
    {
        private readonly IUserService _userService;

        public BeamAuthenticationStateProviderHelper(IUserService userService)
        {
            _userService = userService;
        }


        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentUser = await _userService.GetCurrentUser();

            if (!currentUser.IsAuthenticated)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Email, currentUser.Email),
                new Claim(ClaimTypes.Name, currentUser.Name),
                new Claim(ClaimTypes.Role, currentUser.Role),
            }, "Auth");

            var user = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(user));
        }
    }
}
