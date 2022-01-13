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
        
        public async Task<bool> PostReport(string message, Type type, int itemId)
        {
            string url;
            if (type == typeof(Post))
            {
                url = Config.Config.Instance().BaseServerApiUrl + "/posts/" + itemId + "/report";
            }
            else
            {
                url = Config.Config.Instance().BaseServerApiUrl + "/user/" + itemId + "/report";
            }
            NewReportInfo info = new NewReportInfo() { Message = message };
            string json = JsonConvert.SerializeObject(info);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(url, httpContent);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                //falta ver que hacemos con el id y el createdAt que nos devuelven
                return true;
            }
            return false;

        }

        public async Task<bool> SolveReport(int reportId)
        {

            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerApiUrl + "/admin/reports/" + reportId, httpContent);
            return response.StatusCode == HttpStatusCode.OK;
        }
        //gets all unsolved reports
        public async Task<IEnumerable<Report>> GetAllReports(INavigation navigation, Page view)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerApiUrl + "/admin/reports");
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var reports = new List<Report>();

                foreach (var item in array)
                {
                    var info = item.ToObject<ReportInfo>();
                    //cambiar campos en el backend
                    var report = new Report(navigation, view)
                    {
                        Id = info.Id,
                        Type = info.Type,
                        ReporterId = info.ReporterId,
                        ItemId = info.ItemId,
                        ItemName = info.ItemName,
                        Message = info.Message,
                        Solved = info.Solved
                    };
                    if (!report.Solved) reports.Add(report);
                }
                return reports;
            }
            return new List<Report>();
        }

        private class NewReportInfo
        {
            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }            
        }
        private class ReportInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "type")]
            public string Type { get; set; }

            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }

            [JsonProperty(PropertyName = "itemName")]
            public string ItemName { get; set; }

            [JsonProperty(PropertyName = "itemId")]
            public int ItemId { get; set; }

            [JsonProperty(PropertyName = "reporterId")]
            public int ReporterId { get; set; }

            [JsonProperty(PropertyName = "solved")]
            public bool Solved { get; set; }

        }


    }
}
