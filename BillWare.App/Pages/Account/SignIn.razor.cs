using BillWare.App.Helpers;
using BillWare.App.Models;
using Microsoft.AspNetCore.Components;

namespace BillWare.App.Pages.Account
{
    public partial class SignIn
    {
        [Inject] BeamAuthenticationStateProviderHelper AuthenticationStateProvider { get; set; }

        private bool IsLoading { get; set; } = false;
        private bool isUserAuthenticated = false;
        private LoginModel SignInModel = new LoginModel();

        private async Task HandleLogin()
        {
            IsLoading = true;

            var result = await _authenticationStateProvider.LoginAsync(SignInModel);

            if (result.Token != null)
            {
                if (result.Role == "Administrator")
                    _navigationManager.NavigateTo("/");
                else
                    _navigationManager.NavigateTo("/billing/index");
            }
            else
            {
                await SweetAlertServices.ShowErrorAlert("Error", "Usuario o contraseña incorrectos");
            }

            IsLoading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            isUserAuthenticated = authState.User.Identity.IsAuthenticated;

            if (isUserAuthenticated)
            {
                var role = await _localStorageService.GetItem(Configuration.ROLE);

                if (role == "Administrator")
                    _navigationManager.NavigateTo("/");
                else
                    _navigationManager.NavigateTo("/billing/index");
            }
        }
    }
}
