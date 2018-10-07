namespace AniSync.Responses
{
    public class ApiResponseError
    {
        public ApiResponseError(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}