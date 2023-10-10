using BillWare.App.Common;
using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IUserService : IBaseCrudService<UserModel>
    {
        Task<UserAuthResponse> GetCurrentUser();
    }
}
