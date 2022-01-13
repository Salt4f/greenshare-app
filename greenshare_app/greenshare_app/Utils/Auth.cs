using greenshare_app.Exceptions;
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
        private readonly HttpClient httpClient;

        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            LoginInfo login = new LoginInfo { Email = email, Password = password };
            string json = JsonConvert.SerializeObject(login);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerApiUrl + "/auth/login", httpContent);
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

        public async static Task<HttpContent> AddHeaders(HttpContent httpContent)
        {

            Tuple<int, string> session;
            try
            {
                session = await Auth.Instance().GetAuth();

            }
            catch (Exception)
            {
                throw new InvalidLoginException();
            }
            httpContent.Headers.Add("id", session.Item1.ToString());
            httpContent.Headers.Add("token", session.Item2);
            return httpContent;
        }

        public async static Task<HttpRequestMessage> AddHeaders(HttpRequestMessage httpContent)
        {

            Tuple<int, string> session;
            try
            {
                session = await Auth.Instance().GetAuth();

            }
            catch (Exception)
            {
                throw new InvalidLoginException();
            }
            httpContent.Headers.Add("id", session.Item1.ToString());
            httpContent.Headers.Add("token", session.Item2);
            return httpContent;
        }

        public async Task<bool> CheckLoggedIn()
        {
            if (rememberMe)
            {
                return await ValidateLogin();
            }
            return false;
        }

        private async Task<bool> ValidateLogin()
        {
            
            HttpContent httpContent = new StringContent("");
            httpContent.Headers.Add("id", id.ToString());
            httpContent.Headers.Add("token", token);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerApiUrl + "/auth/validate", httpContent);
            if (response.StatusCode == HttpStatusCode.OK) return true;
            return false;
        }

        public async Task<bool> Register(string email, string password, string nickname, DateTime birthDate, string fullName, string dni)
        {
            RegisterInfo register = new RegisterInfo { Email = email, Password = password, Nickname = nickname, Dni = dni, BirthDate = birthDate, FullName = fullName };
            string json = JsonConvert.SerializeObject(register);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerApiUrl + "/auth/register", httpContent);
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

        public string GetGoogleLoginToken()
        {
            var rand = new Random();
            string token = Crypto.GetHashString((rand.Next() * rand.Next()).ToString());
            return token;
        }

        public async Task<Tuple<bool, bool>> CheckGoogleLogin(string _token)
        {
            var response = await httpClient.GetAsync(Config.Config.Instance().BaseServerGoogleUrl + "/login-status?token=" + _token);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                var info = JsonConvert.DeserializeObject<GoogleInfo>(json);
                id = info.Id;
                token = info.Token;
                rememberMe = true;
                await SaveAuth();
                return Tuple.Create(true, info.NewUser);
            }
            return Tuple.Create(false, false);
        }

        public async Task<Tuple<int, string>> GetAuth()
        {
            if (!await ValidateLogin()) throw new InvalidLoginException();
            return new Tuple<int, string>(id, token);
        }

        public async Task<bool> IsAdmin()
        {
            if (!await ValidateLogin()) throw new InvalidLoginException();
            return id == Config.Config.Instance().AdminId;
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

        private class GoogleInfo : ValidationInfo
        {
            [JsonProperty(PropertyName = "newUser")]
            public bool NewUser { get; set; }
        }

    }
}
