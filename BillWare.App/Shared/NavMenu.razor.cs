using BillWare.App.Helpers;
using BillWare.App.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BillWare.App.Shared
{
    [Authorize]
    public partial class NavMenu
    {
        [Inject] BeamAuthenticationStateProviderHelper? AuthenticationStateProvider { get; set; }
        [Inject] NavigationManager? navigationManager { get; set; }
        [Inject] IJWTAuthenticationStateProvider? _jwtAuthenticationStateProvider { get; set; }

        private bool IsMenuOpen = false;
        private string DashboardClass = "menu-dashboard";
        private string Role = string.Empty;
        private int ActiveLinkIndex = -1;
        private MenuConfiguration MenuConfiguration = new MenuConfiguration();

        private void ToggleMenu()
        {
            IsMenuOpen = !IsMenuOpen;
            DashboardClass = IsMenuOpen ? "menu-dashboard open" : "menu-dashboard";
        }
        private async Task LogOut()
        {
            await _jwtAuthenticationStateProvider.LogOut();

            navigationManager.NavigateTo("account/signin");
        }
        private void NavigateTo(string url, int index)
        {
            IsMenuOpen = false;
            DashboardClass = "menu-dashboard";
            navigationManager!.NavigateTo(url);
            ActiveLinkIndex = index;
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();

            Role = authState
                .User
                .Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value;

            var currentLocation = navigationManager!.ToBaseRelativePath(navigationManager.Uri);

            ActiveLinkIndex = MenuConfiguration
                       .MenuItems!
                       .FindIndex(x => x.Url == currentLocation);
        }
    }
}
