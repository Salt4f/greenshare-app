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
        //Salta internal server error
        public async Task<User> GetUserInfo()
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://server.vgafib.org/api/user/" + session.Item1);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                UserInfo info = JsonConvert.DeserializeObject<UserInfo>(json);
                User user = new User()
                {
                    FullName = info.FullName,
                    NickName = info.NickName,
                    Description = info.Description,
                    ProfilePicture = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(info.ProfilePicture); }) },
                    Banned = info.Banned,
                    TotalEcoPoints = info.TotalEcoPoints,
                    TotalGreenCoins = info.TotalGreenCoins,
                    BirthDate = info.BirthDate,
                    AverageValoration = info.AverageValoration
                };
                return user;
            }            
            return null;
            
        }
        //No queremos un endpoint de esto, se debe devolver con GetUserInfo
        
        private class UserInfo
        {
           
            [JsonProperty(PropertyName = "nickname")]
            public string NickName { get; set; }

            [JsonProperty(PropertyName = "fullname")]
            public string FullName { get; set; }

            [JsonProperty(PropertyName = "birthDate")]
            public DateTime BirthDate { get; set; }


            [JsonProperty(PropertyName = "profilePicture")]
            public byte[] ProfilePicture { get; set; }

            [JsonProperty(PropertyName = "description")]
            public string Description { get; set; }

            [JsonProperty(PropertyName = "banned")]
            public bool Banned { get; set; }

            [JsonProperty(PropertyName = "totalEcoPoints")]
            public int TotalEcoPoints { get; set; }

            [JsonProperty(PropertyName = "totalGreenCoins")]
            public int TotalGreenCoins { get; set; }

            [JsonProperty(PropertyName = "averageValoration")]          
            public double AverageValoration { get; set; }


        }

    }
}
