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

            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/requests" + query);
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
                        Tags = new MvvmHelpers.ObservableRangeCollection<Tag>(info.Tags),
                        Icon = null
                    };
                    cards.Add(card);
                }
                return cards;
            }
            return null;
        }

        public async Task<IEnumerable<PostCard>> GetOffers(Location location, int distance = 50, IEnumerable<Tag> tags = null, int? owner = null, int quantity = 20)
        {
            string query = GetQuery(location, distance, tags, owner, quantity);

            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/offers" + query);

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
                        Tags = new MvvmHelpers.ObservableRangeCollection<Tag>(info.Tags),
                        Icon = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(info.Icon); }) }
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
            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/offers/" + id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                OfferInfo info = JsonConvert.DeserializeObject<OfferInfo>(json);

                Offer post = new Offer()
                {
                    Active = info.Active,
                    Id = info.Id,
                    OwnerId = info.OwnerId,
                    Name = info.Name,
                    Description = info.Description,
                    Location = info.Location,
                    CreatedAt = info.CreatedAt,
                    TerminateAt = info.TerminateAt,
                    EcoImpact = 0,
                    Tags = info.Tags,
                    Icon = new Image() { Source = ImageSource.FromStream(() => { return new MemoryStream(info.Icon); }) }
                };

                post.Icon.Source = ImageSource.FromStream(() => { return new MemoryStream(info.Icon); });

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

                return post;
            }
            return null;
        }

        public async Task<Request> GetRequest(int id)
        {
            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/requests/" + id);
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
                    TerminateAt = info.TerminateAt,
                    EcoImpact = 0,
                    Tags = info.Tags,
                };
                return post;
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

        private class PostInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "active")]
            public bool Active { get; set; }

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
