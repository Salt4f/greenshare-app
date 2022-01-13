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
using Xamarin.Forms;

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

        private readonly HttpClient httpClient;

        public async Task<List<Reward>> GetAllRewards(INavigation navigation, Page view, int greenCoinsAvailable)
        {
            Tuple<int,string> session = await Auth.Instance().GetAuth();
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerApiUrl + "/rewards");
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var cards = new List<Reward>();
                foreach (var item in array)
                {
                    var info = item.ToObject<RewardInfo>();
                    var card = new Reward(navigation, view)
                    {
                        Id = info.Id,
                        Name = info.Name,
                        Description = info.Description,
                        GreenCoins = info.GreenCoins,
                        SponsorName = info.SponsorName,
                        UserId = session.Item1,
                        GreenCoinsAvailable = greenCoinsAvailable,
                        GreenCoinsText = "Cost: "+info.GreenCoins
                    };
                    cards.Add(card);
                }
                return cards;
            }
            return new List<Reward>();
        }

        public async Task<Reward> GetReward(int rewardId, INavigation navigation, Page view)
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerApiUrl + "/rewards/" + rewardId);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                RewardInfo info = JsonConvert.DeserializeObject<RewardInfo>(json);
                var card = new Reward(navigation, view)
                {
                    Id = info.Id,
                    Name = info.Name,
                    Description = info.Description,
                    GreenCoins = info.GreenCoins,
                    SponsorName = info.SponsorName,
                    UserId = session.Item1,
                    GreenCoinsText = "Cost: " + info.GreenCoins
                };
                return card;
            }
            return null;
        }


        //admin only
        public async Task<bool> EditReward(Reward reward)
        {
            RewardInfo requestBody = new RewardInfo { Description = reward.Description, Id = reward.Id, GreenCoins = reward.GreenCoins, SponsorName = reward.SponsorName, Name = reward.Name };
            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PutAsync(Config.Config.Instance().BaseServerApiUrl + "/rewards/" + reward.Id, httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CreateReward(Reward reward)
        {
            RewardInfo requestBody = new RewardInfo { Description = reward.Description, GreenCoins = reward.GreenCoins, SponsorName = reward.SponsorName, Name = reward.Name };
            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerApiUrl + "/rewards", httpContent);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> ExchangeReward(int rewardId)
        {
            ExchangeInfo requestBody = new ExchangeInfo { Id = rewardId };
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerApiUrl + "/user/" + session.Item1 + "redeem?action=rewards", httpContent);
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
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerApiUrl + "/rewards/" + rewardId, httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        private class ExchangeInfo
        {
            [JsonProperty(PropertyName = "rewardId")]
            public int Id { get; set; }
        }

        private class RewardInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "description")]
            public string Description { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "sponsorName")]
            public string SponsorName { get; set; }

            [JsonProperty(PropertyName = "greenCoins")]
            public int GreenCoins { get; set; }
        }

    }
}
