namespace TimeFlow.DL.HttpClients
{
    public class OAuthServerApiClient
    {
        private HttpClient _http;

        public OAuthServerApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GetToken()
        {
            var response = await _http.GetAsync("/connect/token");

            return "ok";
        }

        public async Task<string> GetUserInfo()
        {
            var response = await _http.GetAsync("/connect/userinfo");

            return "ok";
        }
    }
}
