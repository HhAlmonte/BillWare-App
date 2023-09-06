using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IUserService
    {
        Task<BaseResponseModel<UserModel>> GetUsersPaged(int pageIndex, int pageSize);

        Task<UserModel> UpdateUser(UserModel user);

        Task <bool> DeleteUser(string id);
    }
}
