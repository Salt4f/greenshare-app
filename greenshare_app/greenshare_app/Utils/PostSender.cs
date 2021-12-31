using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using greenshare_app.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using greenshare_app.Exceptions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace greenshare_app.Utils
{
    public class PostSender
    {
        private static PostSender instance;

        private PostSender()
        {
            httpClient = new HttpClient();
        }

        public static PostSender Instance()
        {
            if (instance is null) instance = new PostSender();
            return instance;
        }

        private readonly HttpClient httpClient;

        public async Task<bool> DeactivatePost(int postId, string postType)
        {
            string url;
            if (postType == "OFFER") url = "http://server.vgafib.org/api/posts/offers/" + postId + "/deactivate";
            else if (postType == "REQUEST") url = "http://server.vgafib.org/api/posts/requests/" + postId + "/deactivate";
            else return false;
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(url, httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                //falta ver que hacemos con el id y el createdAt que nos devuelven
                return true;
            }
            return false;
        }
        public async Task<bool> PostRequest(string name, string description, DateTime terminateAt, Location location, IEnumerable<Tag> tags)
        {
            RequestInfo post = new RequestInfo { Name = name, Description = description, TerminateAt = terminateAt, Location = location, Tags = tags };
            string json = JsonConvert.SerializeObject(post);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/requests", httpContent);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                //falta ver que hacemos con el id y el createdAt que nos devuelven
                return true;
            }            
            return false;

        }

        public async Task<bool> PostOffer(string name, string description, DateTime terminateAt, Location location, IEnumerable<Tag> tags, IEnumerable<byte[]> photos, byte[] icon)
        {
            OfferInfo post = new OfferInfo {  Name = name, Description = description, TerminateAt = terminateAt, Location = location, Tags = tags, Photos = photos, Icon = icon };
            string json = JsonConvert.SerializeObject(post);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/offers", httpContent);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                //falta ver que hacemos con el id y el createdAt que nos devuelven
                return true;
            }
            return false;

        }

        public async Task<bool> EditOffer(int offerId, string name, string description, DateTime terminateAt, Location location, IEnumerable<Tag> tags, IEnumerable<byte[]> photos, byte[] icon)
        {
            OfferInfo post = new OfferInfo { 
                Name = name, 
                Description = description, 
                TerminateAt = terminateAt, 
                Location = location, 
                Tags = tags, 
                Photos = photos, 
                Icon = icon };

            string json = JsonConvert.SerializeObject(post);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PutAsync("http://server.vgafib.org/api/posts/offers/"+ offerId.ToString(), httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {               
                return true;
            }
            return false;

        }

        public async Task<bool> EditRequest(int requestId, string name, string description, DateTime terminateAt, Location location, IEnumerable<Tag> tags)
        {
            RequestInfo post = new RequestInfo
            {                
                Name = name,
                Description = description,
                TerminateAt = terminateAt,
                Location = location,
                Tags = tags,
                
            };

            string json = JsonConvert.SerializeObject(post);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PutAsync("http://server.vgafib.org/api/posts/requests/" + requestId.ToString(), httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;

        }
        private class PostInfo
        {
            
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            
            [JsonProperty(PropertyName = "description")]
            public string Description { get; set; }

            [JsonProperty(PropertyName = "terminateAt")]
            public DateTime TerminateAt { get; set; }

            [JsonProperty(PropertyName = "location")]
            [JsonConverter(typeof(Converters.LocationConverter))]
            public Location Location { get; set; }

            [JsonProperty(PropertyName = "tags")]
            public IEnumerable<Tag> Tags { get; set; }

        }
        private class OfferInfo : PostInfo
        {
            [JsonProperty(PropertyName = "icon")]
            public byte[] Icon { get; set; }

            [JsonProperty(PropertyName = "photos")]
            public IEnumerable<byte[]> Photos { get; set; }
        }
        private class RequestInfo : PostInfo
        {
            
        }
    }
}
