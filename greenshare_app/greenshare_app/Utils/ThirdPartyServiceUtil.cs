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
        public async void addHeaders()
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
        
    }

    
}
