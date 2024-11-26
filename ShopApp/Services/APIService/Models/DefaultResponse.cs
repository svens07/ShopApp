namespace AuthApi.Models.Responses
{
    public class APIResponse
    {
        public bool success { get; set; }
        public string? error { get; set; }

        public APIResponse() { }

        public APIResponse(bool success_, string? error_ = null)
        {
            success = success_;
            error = error_;
        }
    }

    public class APIResponse<T> : APIResponse
    {
        public T? data { get; set; }

        public APIResponse() { }

        public APIResponse(bool success_, string? error_ = null, T? data_ = default)
            : base(success_, error_)
        {
            data = data_;
        }
    }
}
