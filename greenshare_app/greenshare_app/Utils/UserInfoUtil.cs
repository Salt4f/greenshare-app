using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.Exceptions;
using greenshare_app.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace greenshare_app.Utils
{
    public class UserInfoUtil
    {
        private static UserInfoUtil instance;

        private UserInfoUtil()
        {
            httpClient = new HttpClient();
        }

        public static UserInfoUtil Instance()
        {
            if (instance is null) instance = new UserInfoUtil();
            return instance;
        }

        

        private readonly HttpClient httpClient;
        public async Task<User> GetUserInfo(int? userId = null)
        {
            HttpRequestMessage request;
            if (userId == null)
            {
                Tuple<int, string> session = await Auth.Instance().GetAuth();
                request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/user/" + session.Item1);
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/user/" + userId);
            }
            if (userId == null) request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                UserInfo info = JsonConvert.DeserializeObject<UserInfo>(json);
                User user;
                if (userId == null)
                {
                    user = new User()
                    {
                        FullName = info.FullName,
                        NickName = info.NickName,
                        Description = info.Description,
                        ProfilePicture = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(info.ProfilePicture); }) },
                        Banned = info.Banned,
                        TotalEcoPoints = info.TotalEcoPoints,
                        TotalGreenCoins = info.TotalGreenCoins,
                        BirthDate = new DateTime(info.BirthDate.Year, info.BirthDate.Month, info.BirthDate.Day),
                        AverageValoration = info.AverageValoration
                    };
                }
                else
                {
                    user = new User()
                    {                        
                        NickName = info.NickName,
                        AverageValoration = info.AverageValoration
                    };
                }
                return user;
            }            
            return null;
            
        }
        
        //edit info of logged user
        public async Task<bool> EditUser(User user)
        {
            EditUserInfo editInfo = new EditUserInfo() { Description = user.Description, NickName = user.NickName };
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            string json = JsonConvert.SerializeObject(editInfo);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PutAsync(Config.Config.Instance().BaseServerUrl + "/user/"+session.Item1, httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> BanUser(int userId)
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/user/" + userId + "/ban", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;

        }


        private class EditUserInfo
        {

            [JsonProperty(PropertyName = "nickname")]
            public string NickName { get; set; }

            [JsonProperty(PropertyName = "aboutMe")]
            public string Description { get; set; }            
        }
        private class UserInfo : EditUserInfo
        {
                       
            [JsonProperty(PropertyName = "fullname")]
            public string FullName { get; set; }

            [JsonProperty(PropertyName = "birthDate")]
            public DateTime BirthDate { get; set; }

            [JsonProperty(PropertyName = "profilePicture")]
            public byte[] ProfilePicture { get; set; }
            
            [JsonProperty(PropertyName = "banned")]
            public bool Banned { get; set; }

            [JsonProperty(PropertyName = "totalEcoPoints")]
            public int TotalEcoPoints { get; set; }

            [JsonProperty(PropertyName = "totalGreenCoins")]
            public int TotalGreenCoins { get; set; }

            [JsonProperty(PropertyName = "valoration")]          
            public double AverageValoration { get; set; }


        }

    }
}
