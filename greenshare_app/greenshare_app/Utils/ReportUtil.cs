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
    public class ReportUtil
    {
        private static ReportUtil instance;

        private ReportUtil()
        {
            httpClient = new HttpClient();
        }

        public static ReportUtil Instance()
        {
            if (instance is null) instance = new ReportUtil();
            return instance;
        }

        private readonly HttpClient httpClient;
        public async Task<bool> PostReport(string message, string type, int itemId)
        {
            string url;
            if (type == "POST")
            {
                url = "http://server.vgafib.org/api/posts/" + itemId + "/report";
            }
            else url = "http://server.vgafib.org/api/user/" + itemId + "/report";
            Tuple<int, string> session;
            try
            {
                session = await Auth.Instance().GetAuth();

            }
            catch (Exception)
            {
                throw new InvalidLoginException();
            }
            string json = JsonConvert.SerializeObject(message);
            var httpContent = new StringContent(json);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("id", session.Item1.ToString());
            httpClient.DefaultRequestHeaders.Add("token", session.Item2);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(url, httpContent) ;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                //falta ver que hacemos con el id y el createdAt que nos devuelven
                return true;
            }
            return false;

        }

        private class ReportInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "type")]
            public string Type { get; set; }

            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }

            [JsonProperty(PropertyName = "itemId")]
            public int ItemId { get; set; }

            [JsonProperty(PropertyName = "reporterId")]
            public int ReporterId { get; set; }

            [JsonProperty(PropertyName = "solved")]
            public bool Solved { get; set; }

        }


    }
}
