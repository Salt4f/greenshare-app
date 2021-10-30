using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace greenshare_app.Utils
{
    public class Auth
    {
        private static Auth instance;
        private Auth()
        {
            LoadAuth();
            httpClient = new HttpClient();
        }
        public static Auth Instance()
        {
            if (instance is null) instance = new Auth();
            return instance;
        }

        int id;
        string token;
        bool rememberMe;
        HttpClient httpClient;

        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            LoginInfo login = new LoginInfo { Email = email, Password = password };
            string json = JsonConvert.SerializeObject(login);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync("http://server.vgafib.org/api/auth/login", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                id = tokenJson.Value<int>("id");
                token = tokenJson.Value<string>("token");
                this.rememberMe = rememberMe;
                await SaveAuth();
                return true;
            }
            return false;
        }

        

        public async Task<bool> CheckLoggedIn()
        {
            if (rememberMe)
            {
                var login = new ValidationInfo { Id = id, Token = token };
                string json = JsonConvert.SerializeObject(login);
                var httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = await httpClient.PostAsync("http://server.vgafib.org/api/auth/validate", httpContent);
                if (response.StatusCode == HttpStatusCode.OK) return true;
            }
            return false;
        }

        public async Task<bool> Register(string email, string password, string nickname, DateTime birthDate, string fullName, string dni)
        {
            RegisterInfo register = new RegisterInfo { Email = email, Password = password, Nickname = nickname, Dni = dni, BirthDate = birthDate, FullName = fullName };
            string json = JsonConvert.SerializeObject(register);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync("http://server.vgafib.org/api/auth/register", httpContent);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                id = tokenJson.Value<int>("id");
                token = tokenJson.Value<string>("token");
                rememberMe = true;
                await SaveAuth();
                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            rememberMe = false;
            await SaveAuth();
        }

        private async Task SaveAuth()
        {
            Application.Current.Properties["userId"] = id;
            Application.Current.Properties["userToken"] = token;
            Application.Current.Properties["userRememberMe"] = rememberMe;
            await Application.Current.SavePropertiesAsync();
        }

        private void LoadAuth()
        {
            try
            {
                rememberMe = (bool)Application.Current.Properties["userRememberMe"];
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                rememberMe = false;
                return;
            }
            
            if (rememberMe)
            {
                id = (int) Application.Current.Properties["userId"];
                token = (string) Application.Current.Properties["userToken"];
                return;
            }
            else
            {
                Application.Current.Properties["userId"] = null;
                Application.Current.Properties["userToken"] = null;
            }
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

            [JsonProperty(PropertyName = "nickname")]
            public string Nickname { get; set; }

            [JsonProperty(PropertyName = "dni")]
            public string Dni { get; set; }

            [JsonProperty(PropertyName = "birthDate")]
            public DateTime BirthDate { get; set; }

            [JsonProperty(PropertyName = "fullName")]
            public string FullName { get; set; }

        }

        private class ValidationInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "token")]
            public string Token { get; set; }
        }

    }
}
