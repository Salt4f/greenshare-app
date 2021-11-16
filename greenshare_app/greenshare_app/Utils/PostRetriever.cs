using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.Exceptions;
using greenshare_app.Models;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

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
                    var obj = (JObject)item;
                    var card = new PostCard();

                    card.Id = (int)obj.GetValue("id");
                    card.Name = (string)obj.GetValue("name");                  
                    card.Author = (string)obj.GetValue("nickname");
                    List<Tag> definitiveTags = new List<Tag>();
                    ColorTypeConverter converter = new ColorTypeConverter();
                    IEnumerable<Tuple<string, string>> jsonTags = (IEnumerable<Tuple<string, string>>)obj.GetValue("tags");
                    foreach (Tuple<string, string> tag in jsonTags)
                    {

                        Tag definitiveTag = new Tag { Color = (Color)converter.ConvertFromInvariantString(tag.Item1), Name = tag.Item2 };
                        definitiveTags.Add(definitiveTag);
                    }
                    card.Tags = definitiveTags;
                    cards.Add(card);
                }
                return cards;

                /*id = tokenJson.Value<int>("id");
                token = tokenJson.Value<string>("token");
                this.rememberMe = rememberMe;
                await SaveAuth();
                return true;*/
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
                    var obj = (JObject)item;
                    var card = new PostCard();

                    card.Id = (int)obj.GetValue("id");
                    card.Name = (string)obj.GetValue("name");
                    card.Icon = new Image();
                    var image = Encoding.UTF8.GetBytes((string)obj.GetValue("icon"));
                    card.Icon.Source = ImageSource.FromStream(() => { return new MemoryStream(image); });
                    card.Author = (string)obj.GetValue("nickname");
                    List<Tag> definitiveTags = new List<Tag>();
                    ColorTypeConverter converter = new ColorTypeConverter();
                    IEnumerable<Tuple<string, string>> jsonTags = (IEnumerable<Tuple<string, string>>)obj.GetValue("tags");
                    foreach (Tuple<string, string> tag in jsonTags)
                    {

                        Tag definitiveTag = new Tag { Color = (Color)converter.ConvertFromInvariantString(tag.Item1), Name = tag.Item2 };
                        definitiveTags.Add(definitiveTag);
                    }
                    card.Tags = definitiveTags;
                    cards.Add(card);
                }
                return cards;

                /*id = tokenJson.Value<int>("id");
                token = tokenJson.Value<string>("token");
                this.rememberMe = rememberMe;
                await SaveAuth();
                return true;*/
            }

            return null;
        }

        private static string GetQuery(Location location, int distance, IEnumerable<Tag> tags, int? owner, int quantity)
        {
            if (location is null) throw new NullLocationException();
            string query = "?location=" + location.ToString();
            query += "&distance=" + distance;
            query += "&tags=";

            foreach (Tag tag in tags)
            {
                query += tag.Name + ",";
            }
            query.TrimEnd(',');

            if (owner != null) query += "&owner=" + owner;

            query += "&quantity=" + quantity;
            return query;
        }
     
        public async Task<Offer> GetOffer(int owner)
        {            
            var id = owner.ToString();
            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/offers/" + id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());                                            
                var post = new Offer();

                post.OwnerId = tokenJson.Value<int>("ownerId");
                post.Description = tokenJson.Value<string>("description");
                post.Location = tokenJson.Value<Location>("location");
                post.Name = tokenJson.Value<string>("name");
                post.EcoImpact = tokenJson.Value<int>("ecoImpact");
                post.CreatedAt = tokenJson.Value<DateTime>("CreatedAt");
                post.TerminateAt = tokenJson.Value<DateTime>("terminateAt");
                post.Active = tokenJson.Value<bool>("active");

                post.Icon = new Image();
                var icon = Encoding.UTF8.GetBytes((string)tokenJson.Value<string>("icon"));
                post.Icon.Source = ImageSource.FromStream(() => { return new MemoryStream(icon); });


                //Photos
                List<Image> photos = new List<Image>();
                IEnumerable<string> jsonPhotos = tokenJson.Value<IEnumerable<string>>("photos");
                foreach (string photo in jsonPhotos)
                {
                    var encodedPhoto = Encoding.UTF8.GetBytes(photo);
                    Image definitivePhoto = new Image
                    {
                        Source = ImageSource.FromStream(() => { return new MemoryStream(encodedPhoto); })
                    };
                    photos.Add(definitivePhoto);
                }
                post.Photos = photos;


                //Tags
                List<Tag> tags = new List<Tag>();
                ColorTypeConverter converter = new ColorTypeConverter();
                IEnumerable<Tuple<string, string>> jsonTags = tokenJson.Value<IEnumerable<Tuple<string, string>>>("tags");
                foreach (Tuple<string, string> tag in jsonTags)
                {
                    
                    Tag definitiveTag = new Tag { Color = (Color)converter.ConvertFromInvariantString(tag.Item1), Name = tag.Item2 };
                    tags.Add(definitiveTag);
                }
                post.Tags = tags;
                
                //falta coger el array de photos y de tags
                return post;
            }
            return null;
        }

        public async Task<Request> GetRequest(int owner)
        {           
            var id = owner.ToString();
            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/requests/" + id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                var post = new Request();
                post.OwnerId = tokenJson.Value<int>("ownerId");
                post.Description = tokenJson.Value<string>("description");
                post.Location = tokenJson.Value<Location>("location");
                post.Name = tokenJson.Value<string>("name");
                post.EcoImpact = tokenJson.Value<int>("ecoImpact");
                post.CreatedAt = tokenJson.Value<DateTime>("CreatedAt");
                post.TerminateAt = tokenJson.Value<DateTime>("terminateAt");
                post.Active = tokenJson.Value<bool>("active");

                List<Tag> tags = new List<Tag>();
                ColorTypeConverter converter = new ColorTypeConverter();
                IEnumerable<Tuple<string, string>> jsonTags = tokenJson.Value <IEnumerable<Tuple<string, string>>>("tags");
                foreach (Tuple<string,string> tag in jsonTags)
                {
                    
                    Tag definitiveTag = new Tag { Color = (Color)converter.ConvertFromInvariantString(tag.Item1), Name = tag.Item2 };
                    tags.Add(definitiveTag);
                }
                post.Tags = tags;  
                
                return post;
            }
            return null;
        }
    }
}
