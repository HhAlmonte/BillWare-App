using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface ICategoryService
    {
        Task<BaseResponseModel<Category>> GetCategories(int pageIndex, int pageSize);
        Task<BaseResponseModel<Category>> GetCategoryWithSearch(string search, int pageIndex, int pageSize);
        Task<HttpResponseMessage> CreateCategory(Category category);
        Task<Category> EditCategory(Category category);
        Task<HttpResponseMessage> DeleteCategory(int id);
    }
}
