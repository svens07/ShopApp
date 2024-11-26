namespace AuthApi.Models.Responses
{
    public class MeResponse
    {
        public MeResponse() { }

        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}
