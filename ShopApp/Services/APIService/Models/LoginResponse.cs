namespace AuthApi.Models.Responses
{
    public class LoginResponse
    {
        public LoginResponse() { }

        public LoginResponse(string jwtToken_)
        {
            jwtToken = jwtToken_;
        }

        public string jwtToken { get; set; }
    }
}
