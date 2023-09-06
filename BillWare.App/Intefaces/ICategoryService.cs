using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface ICategoryService
    {
        Task<BaseResponseModel<CategoryModel>> GetCategoriesPaged(int pageIndex, int pageSize);

        Task<BaseResponseModel<CategoryModel>> GetCategoriesPagedWithSearch(int pageIndex, int pageSize, string search);

        Task<bool> DeleteCategory(int id);

        Task<CategoryModel> CreateCategory(CategoryModel category);

        Task<CategoryModel> EditCategory(CategoryModel category);
    }
}
