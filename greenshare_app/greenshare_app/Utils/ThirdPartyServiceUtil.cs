using greenshare_app.Exceptions;
using greenshare_app.Models;
using MvvmHelpers;
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
    internal class ThirdPartyServiceUtil
    {
        private static ThirdPartyServiceUtil instance;

        private ThirdPartyServiceUtil()
        {
            httpClient = new HttpClient();
        }

        public static ThirdPartyServiceUtil Instance()
        {
            if (instance is null) instance = new ThirdPartyServiceUtil();
            return instance;
        }
        private async void AddHeaders()
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
        public async Task<List<QuizQuestion>> GetEcoQuiz()
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/user/" + session.Item1 + "/eco-score");
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var cards = new List<QuizQuestion>();

                foreach (var item in array)
                {
                    var info = item.ToObject<QuizQuestion>();
                    cards.Add(info);
                }
                return cards;
            }
            return new List<QuizQuestion>();
        }

        public async Task<bool> PostEcoQuizResults(IList<int> responses)
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            EcoQuizResponseInfo post = new EcoQuizResponseInfo { R1 = responses[0], R2 = responses[1], R3 = responses[2], R4 = responses[3], R5 = responses[4], R6 = responses[5], R7 = responses[6], R8 = responses[7], };
            string json = JsonConvert.SerializeObject(post);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/user/" + session.Item1 + "/eco-score", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        private class EcoQuizResponseInfo
        {
            [JsonProperty(PropertyName = "R1")]
            public int R1 { get; set; }
            [JsonProperty(PropertyName = "R2")]
            public int R2 { get; set; }
            [JsonProperty(PropertyName = "R3")]
            public int R3 { get; set; }
            [JsonProperty(PropertyName = "R4")]
            public int R4 { get; set; }
            [JsonProperty(PropertyName = "R5")]
            public int R5 { get; set; }
            [JsonProperty(PropertyName = "R6")]
            public int R6 { get; set; }
            [JsonProperty(PropertyName = "R7")]
            public int R7 { get; set; }
            [JsonProperty(PropertyName = "R8")]
            public int R8 { get; set; }
        }
    }

    
}
