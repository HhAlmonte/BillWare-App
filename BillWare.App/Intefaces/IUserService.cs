using BillWare.App.Common;
using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IUserService
    {
        Task<PaginationResult<UserModel>> GetUsersPaged(int pageIndex, int pageSize);

        Task<UserModel> UpdateUser(UserModel user);

        Task<UserAuthResponse> GetCurrentUser();

        Task<bool> DeleteUser(string id);
    }
}
