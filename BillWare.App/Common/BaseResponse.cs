namespace BillWare.App.Common
{
    public class BaseResponse<T>
    {
        public bool IsSuccessFull { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string? Details { get; set; }
        public T? Data { get; set; }


        public static BaseResponse<T> BuildSuccessResponse(T data)
        {
            return new BaseResponse<T>
            {
                IsSuccessFull = true,
                Data = data
            };
        }

        public static BaseResponse<T> BuildErrorResponse(BaseResponse<T> errorResponse)
        {
            return new BaseResponse<T>
            {
                IsSuccessFull = false,
                Message = errorResponse!.Message,
                StatusCode = errorResponse!.StatusCode,
                Details = errorResponse!.Details
            };
        }
    }
}
