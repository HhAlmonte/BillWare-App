using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface ICategoryService
    {
        Task<BaseResponseModel<Category>> GetCategories(int pageIndex, int pageSize);
    }
}
