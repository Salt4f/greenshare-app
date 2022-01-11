using greenshare_app.Exceptions;
using greenshare_app.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net; 
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace greenshare_app.Utils
{
    public class RewardsUtil
    {
        private static RewardsUtil instance;

        private RewardsUtil()
        {
            httpClient = new HttpClient();
        }

        public static RewardsUtil Instance()
        {
            if (instance is null) instance = new RewardsUtil();
            return instance;
        }
        public async void AddHeaders()
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
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("id", session.Item1.ToString());
            httpClient.DefaultRequestHeaders.Add("token", session.Item2);
        }

        private readonly HttpClient httpClient;

        public async Task<List<Reward>> GetAllRewards()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/rewards");
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var cards = new List<Reward>();
                foreach (var item in array)
                {
                    var info = item.ToObject<Reward>();
                    info.GreenCoinsText = "Cost: " + info.GreenCoins;
                    cards.Add(info);
                }
                return cards;
            }
            return new List<Reward>();
        }

        public async Task<Reward> GetReward(int rewardId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/rewards/" + rewardId);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                Reward info = JsonConvert.DeserializeObject<Reward>(json);
                info.GreenCoinsText = "Cost: " + info.GreenCoins;
                return info;
            }
            return null;
        }


        //admin only
        public async Task<bool> EditReward(Reward reward)
        {
            PostRewardInfo requestBody = new PostRewardInfo { Description = reward.Description, Id = reward.Id, GreenCoins = reward.GreenCoins, SponsorName = reward.SponsorName };
            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PutAsync(Config.Config.Instance().BaseServerUrl + "/rewards/" + reward.Id, httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeactivateReward(int rewardId)
        {
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/rewards/" + rewardId, httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        private class PostRewardInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "description")]
            public string Description { get; set; }

            [JsonProperty(PropertyName = "sponsorName")]
            public string SponsorName { get; set; }

            [JsonProperty(PropertyName = "greenCoins")]
            public int GreenCoins { get; set; }
        }

    }
}
