namespace AuthApi.Models.Requests
{
    public class ChangePassRequest
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
