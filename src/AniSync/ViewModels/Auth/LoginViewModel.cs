namespace AniSync.ViewModels.Auth
{
    public class LoginViewModel
    {
        public LoginViewModel(string url, string clientId)
        {
            Url = url;
            ClientId = clientId;
        }

        public string Url { get; }

        public string ClientId { get; set; }
    }
}
