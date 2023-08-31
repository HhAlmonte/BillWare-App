using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface ICategoryService
    {
        Task<BaseResponseModel<CategoryModel>> GetCategoriesPaged(int pageIndex, int pageSize);

        Task<BaseResponseModel<CategoryModel>> GetCategoriesPagedWithSearch(int pageIndex, int pageSize, string search);

        Task<HttpResponseMessage> DeleteCategory(int id);

        Task<HttpResponseMessage> CreateCategory(CategoryModel category);

        Task<HttpResponseMessage> EditCategory(CategoryModel category);
    }
}
