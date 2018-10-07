namespace AniSync.Responses
{
    public class ApiResponse
    {
        public ApiResponseError[] Errors { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }
}
