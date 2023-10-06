namespace BillWare.App.Common
{
    public class BaseResponse<T>
    {
        public bool IsSuccessFul { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public IDictionary<string, string[]>? Details { get; set; }


        public T Data { get; set; }
    }
}
