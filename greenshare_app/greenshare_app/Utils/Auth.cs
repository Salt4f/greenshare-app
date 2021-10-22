using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace greenshare_app.Utils
{
    public class Auth
    {
        private static Auth instance;
        private Auth()
        {
            httpClient = new HttpClient();
        }
        public static Auth Instance()
        {
            if (instance is null) instance = new Auth();
            return instance;
        }

        int id;
        string token;
        HttpClient httpClient;

        public async Task<bool> Login(string email, string password)
        {
            LoginInfo login = new LoginInfo { Email = email, Password = password};
            string json = JsonConvert.SerializeObject(login);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync("http://server.vgafib.org/api/auth/login", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                id = tokenJson.Value<int>("id");
                token = tokenJson.Value<string>("token");
                return true;
            }
            return false;
        }

        private class LoginInfo
        {
            [JsonProperty(PropertyName = "email")]
            public string Email { get; set; }

            [JsonProperty(PropertyName = "password")]
            public string Password { get; set; }
        }

        private class RegisterInfo
        {
            [JsonProperty(PropertyName = "email")]
            public string Email { get; set; }

            [JsonProperty(PropertyName = "password")]
            public string Password { get; set; }
        }

    }
}
