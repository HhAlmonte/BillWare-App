using BillWare.App.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BillWare.App.Shared
{
    [Authorize]
    public partial class NavMenu
    {
        [Inject] BeamAuthenticationStateProviderHelper AuthenticationStateProvider { get; set; }

        private bool IsMenuOpen = false;
        private string DashboardClass = "menu-dashboard";
        private string Role = string.Empty;
        private List<MenuItem> MenuItems = new List<MenuItem>();
        private int ActiveLinkIndex = -1;

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
            IsMenuOpen = !IsMenuOpen;
            DashboardClass = "menu-dashboard";
            navigationManager.NavigateTo(url);
            ActiveLinkIndex = index;
        }

        private void LoadMenuItem()
        {
            MenuItems.Add(new MenuItem { Name = "Dashboard", Url = "/", Icon = "bx bx-category", Role = "Administrator" });
            MenuItems.Add(new MenuItem { Name = "Servicios", Url = "service/index", Icon = "bx bx-leaf", Role = "Both" });
            MenuItems.Add(new MenuItem { Name = "Inventario", Url = "inventory/index", Icon = "bx bx-store-alt", Role = "Both" });
            MenuItems.Add(new MenuItem { Name = "Categoría", Url = "category/index", Icon = "bx bx-category", Role = "Both" });
            MenuItems.Add(new MenuItem { Name = "Cliente", Url = "costumer/index", Icon = "bx bx-user", Role = "Both" });
            MenuItems.Add(new MenuItem { Name = "Facturación", Url = "billing/index", Icon = "bx bxs-badge-dollar", Role = "Both" });
            MenuItems.Add(new MenuItem { Name = "Usuarios", Url = "user/index", Icon = "bx bx-user-circle", Role = "Administrator" });
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            LoadMenuItem();
            Role = authState.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
        }
    }
}
