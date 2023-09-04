using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> RegisterAsync(RegistrationModel request);
        Task<LoginResponse> LoginAsync(LoginModel request);
    }
}
