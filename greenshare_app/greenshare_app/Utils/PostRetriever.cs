using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.Exceptions;
using greenshare_app.Models;
using MvvmHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace greenshare_app.Utils
{
    class PostRetriever
    {
        private static PostRetriever instance;

        private PostRetriever()
        {
            httpClient = new HttpClient();
        }

        public static PostRetriever Instance()
        {
            if (instance is null) instance = new PostRetriever();
            return instance;
        }
        
        private readonly HttpClient httpClient;

        public async Task<IEnumerable<PostCard>> GetRequests(Location location, int distance = 50, IEnumerable<Tag> tags = null, int? owner = null, int quantity = 20)
        {
            string query = GetQuery(location, distance, tags, owner, quantity);
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/posts/requests" + query);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject body = JObject.Parse(json);
                var array = body.Value<JArray>("infoMessage");
                var cards = new List<PostCard>();

                foreach (var item in array)
                {
                    var info = item.ToObject<PostCardInfo>();
                    var card = new PostCard()
                    {
                        Id = info.Id,
                        Name = info.Name,
                        Author = info.Author,
                        Tags = new MvvmHelpers.ObservableRangeCollection<Tag>(info.Tags),
                        Icon = null
                    };
                    cards.Add(card);
                }
                return cards;
            }
            return null;
        }

        public async Task<IEnumerable<PostCard>> GetOffers(Location location, int distance = 200, IEnumerable<Tag> tags = null, int? owner = null, int quantity = 20)
        {
            string query = GetQuery(location, distance, tags, owner, quantity);
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/posts/offers" + query);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var cards = new List<PostCard>();

                foreach (var item in array)
                {
                    var info = item.ToObject<PostCardInfo>();
                    var card = new PostCard()
                    {
                        Id = info.Id,
                        Name = info.Name,
                        Author = info.Author,
                        Tags = new ObservableRangeCollection<Tag>(info.Tags),
                        Icon = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(info.Icon); }) }
                    };
                    cards.Add(card);
                }
                return cards;
            }
            return null;
        }

        public async Task<IEnumerable<PostCard>> SearchOffers(Location location, int distance, string searchWord)
        {
            string query = "?location=" + location.Latitude + ";" + location.Longitude;
            query += "&q=" + searchWord + "&distance=" + distance;
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/posts/offers" + query);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var cards = new List<PostCard>();

                foreach (var item in array)
                {
                    var info = item.ToObject<PostCardInfo>();
                    var card = new PostCard()
                    {
                        Id = info.Id,
                        Name = info.Name,
                        Author = info.Author,
                        Tags = new ObservableRangeCollection<Tag>(info.Tags),
                        Icon = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(info.Icon); }) }
                    };
                    cards.Add(card);
                }
                return cards;
            }
            return null;
        }
        public async Task<IEnumerable<PostCard>> SearchRequests(Location location, int distance, string searchWord)
        {
            string query = "?location=" + location.Latitude + ";" + location.Longitude;
            query += "&q=" + searchWord + "&distance=" + distance;
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/posts/requests" + query);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var cards = new List<PostCard>();

                foreach (var item in array)
                {
                    var info = item.ToObject<PostCardInfo>();
                    var card = new PostCard()
                    {
                        Id = info.Id,
                        Name = info.Name,
                        Author = info.Author,
                        Tags = new ObservableRangeCollection<Tag>(info.Tags),
                    };
                    cards.Add(card);
                }
                return cards;
            }
            return null;
        }

        private static string GetQuery(Location location, int distance, IEnumerable<Tag> tags, int? owner, int quantity)
        {
            if (location is null) throw new NullLocationException();
            string query = "?location=" + location.Latitude + ";" + location.Longitude;
            query += "&distance=" + distance;
            if (tags != null)
            {
                query += "&tags=";

                foreach (Tag tag in tags)
                {
                    query += tag.Name + ",";
                }
                query.TrimEnd(',');
            }

            if (owner != null) query += "&owner=" + owner;

            query += "&quantity=" + quantity;
            return query;
        }
     
        public async Task<Offer> GetOffer(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/posts/offers/" + id);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                OfferInfo info = JsonConvert.DeserializeObject<OfferInfo>(json);

                Offer post = new Offer()
                {
                    Active = info.Active,
                    Id = info.Id,
                    //Status = Enum.Parse(typeof(Status), info.Status),
                    OwnerId = info.OwnerId,
                    Name = info.Name,
                    Description = info.Description,
                    Location = info.Location,
                    CreatedAt = info.CreatedAt,
                    TerminateAt = new DateTime(info.TerminateAt.Year, info.TerminateAt.Month, info.TerminateAt.Day),
                    EcoImpact = 0,
                    Tags = info.Tags,
                    Icon = info.Icon,
                    Photos = info.Photos
                };

                /*
                //Photos
                var photos = new List<Image>();
                foreach (byte[] photo in info.Photos)
                {
                    Image definitivePhoto = new Image
                    {
                        Source = ImageSource.FromStream(() => { return new MemoryStream(photo); })
                    };
                    photos.Add(definitivePhoto);
                }
                post.Photos = photos;
                */
                return post;
            }
            return null;
        }

        public async Task<Request> GetRequest(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/posts/requests/" + id);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                RequestInfo info = JsonConvert.DeserializeObject<RequestInfo>(json);
                Request post = new Request()
                {
                    Active = info.Active,
                    Id = info.Id,
                    OwnerId = info.OwnerId,
                    Name = info.Name,
                    Description = info.Description,
                    Location = info.Location,
                    CreatedAt = info.CreatedAt,
                    TerminateAt = new DateTime(info.TerminateAt.Year, info.TerminateAt.Month, info.TerminateAt.Day),
                    EcoImpact = 0,
                    Tags = info.Tags,
                };
                return post;
            }
            return null;
        }

        public async Task<IEnumerable<PostStatus>> GetPostsByUserId(string type)
        {
            string query = "?type=" + type;
            var session = await Auth.Instance().GetAuth();
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/user/" + session.Item1 + "/posts" + query);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var cards = new List<PostStatus>();

                foreach (var item in array)
                {
                    var info = item.ToObject<PostStatusInfo>();
                    var card = new PostStatus()
                    {
                        Id = info.Id,
                        Name = info.Name,
                        Tags = new ObservableRangeCollection<Tag>(info.Tags),
                        Description = info.Description,
                        Active = info.Active,
                        Status = info.Status,
                    };
                    if (type == "offers")
                    {
                        card.IsOffer = true;
                        card.Icon = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(info.Icon); }) };
                    }
                    else card.IsOffer = false;
                    cards.Add(card);
                }
                return cards;
            }
            return null;
        }

        private class PostCardInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "tags")]
            public IEnumerable<Tag> Tags { get; set; }

            [JsonProperty(PropertyName = "icon")]
            public byte[] Icon { get; set; }

            [JsonProperty(PropertyName = "author")]
            public string Author { get; set; }
        }
        private class PostStatusInfo : PostCardInfo 
        {
            [JsonProperty(PropertyName = "description")]
            public string Description { get; set; }

            [JsonProperty(PropertyName = "active")]
            public bool Active { get; set; }

            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }

        }

        private class PostInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "active")]
            public bool Active { get; set; }

            //[JsonProperty(PropertyName = "status")]
            //public string Status { get; set; }

            [JsonProperty(PropertyName = "ownerId")]
            public int OwnerId { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "description")]
            public string Description { get; set; }

            [JsonProperty(PropertyName = "terminateAt")]
            public DateTime TerminateAt { get; set; }

            [JsonProperty(PropertyName = "createdAt")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty(PropertyName = "updatedAt")]
            public DateTime UpdatedAt { get; set; }

            [JsonProperty(PropertyName = "location")]
            [JsonConverter(typeof(Converters.LocationConverter))]
            public Location Location { get; set; }

            [JsonProperty(PropertyName = "tags")]
            public IList<Tag> Tags { get; set; }

        }

        private class OfferInfo : PostInfo
        {
            [JsonProperty(PropertyName = "icon")]
            public byte[] Icon { get; set; }

            [JsonProperty(PropertyName = "photos")]
            public IList<byte[]> Photos { get; set; }
        }

        private class RequestInfo : PostInfo { }

    }
}
