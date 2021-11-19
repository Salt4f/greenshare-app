﻿using System;
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

        public async Task<User> GetUserInfo()
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();            
            var response = await httpClient.GetAsync("http://server.vgafib.org/api/user/" + session.Item1);
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
                    TotalEcoPoints = 0,
                    TotalGreenCoins = 0,
                    AverageValoration = 0.0,
                };
                return user;
            }            
            return null;
            
        }

        private class UserInfo
        {
           
            [JsonProperty(PropertyName = "nickname")]
            public string NickName { get; set; }

            [JsonProperty(PropertyName = "fullname")]
            public string FullName { get; set; }

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