using BillWare.App.Helpers;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.Account
{
    public partial class SignIn
    {
        [Inject] BeamAuthenticationStateProviderHelper? AuthenticationStateProvider { get; set; }
        [Inject] NavigationManager? _navigationManager { get; set; }
        [Inject] LocalStorageHelper? _localStorageService { get; set; }
        [Inject] IJWTAuthenticationStateProvider? _authenticationStateProvider { get; set; }

        private bool IsLoading { get; set; } = false;
        private bool isUserAuthenticated = false;

        private async Task HandleLogin(LoginArgs args)
        {
            var loginPayload = new LoginModel
            {
                Email = args.Username,
                Password = args.Password
            };

            IsLoading = true;

            var response = await _authenticationStateProvider!.LoginAsync(loginPayload);

            if (!response.IsSuccessFul)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                IsLoading = false;
                return;
            }

            var urlToSend = response.Data!.Role == "Administrator" ? "/" : "/billing/index";
            _navigationManager!.NavigateTo(urlToSend);

            IsLoading = false;
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();

            isUserAuthenticated = authState.User.Identity.IsAuthenticated;

            if (isUserAuthenticated)
            {
                var role = await _localStorageService!.GetItem(Configuration.ROLE);

                if (role == "Administrator")
                    _navigationManager!.NavigateTo("/");
                else
                    _navigationManager!.NavigateTo("/billing/index");
            }
        }
    }
}
