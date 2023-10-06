using BillWare.App.Common;
using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IJWTAuthenticationStateProvider
    {
        Task<HttpResponseMessage> RegisterAsync(RegistrationModel request);

        Task<BaseResponse<LoginResponse>> LoginAsync(LoginModel request);

        Task LogOut();
    }
}
