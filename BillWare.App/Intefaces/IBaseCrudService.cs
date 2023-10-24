using BillWare.App.Common;

namespace BillWare.App.Intefaces
{
    public interface IBaseCrudService<T> where T : BaseModel
    {
        Task<BaseResponse<T>> CreateAsync(object entity);
        Task<BaseResponse<T>> UpdateAsync(object entity);
        Task<BaseResponse<T>> DeleteAsync(object identity);
        Task<BaseResponse<PaginationResult<T>>> GetEntitiesPagedAsync(int pageIndex, int pageSize);
        Task<BaseResponse<PaginationResult<T>>> GetEntitiesPagedWithSearchAsync(int pageIndex, int pageSize, string search);
        Task<BaseResponse<T>> GetEntityById(object identity);
    }
}
