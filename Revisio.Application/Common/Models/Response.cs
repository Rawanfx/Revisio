namespace Revisio.Application.Common.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; }

        public static Response<T> SuccessResponse(T data, string message = "Success")
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null
            };
        }
        public static Response<T> FailResponse(string message, List<string>? errors = null)
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
}
