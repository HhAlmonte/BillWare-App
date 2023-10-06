using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IJWTAuthenticationStateProvider
    {
        Task<HttpResponseMessage> RegisterAsync(RegistrationModel request);
        Task<LoginResponse> LoginAsync(LoginModel request);

        Task LogOut();
    }
}
