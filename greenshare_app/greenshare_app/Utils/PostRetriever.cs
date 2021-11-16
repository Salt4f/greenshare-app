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


        public async Task<IEnumerable<PostCard>> GetOffers(Location location, int distance = 50, IEnumerable<Tag> tags = null, int? owner = null, int quantity = 20)
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

            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/offers" + query);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());

                var cards = new List<PostCard>();

                foreach (var item in array)
                {
                    var obj = (JObject) item;
                    var card = new PostCard();

                    card.Id = (int) obj.GetValue("id");
                    card.Name = (string) obj.GetValue("name");
                    card.Icon = new Image();

                    var image = Encoding.UTF8.GetBytes((string) obj.GetValue("icon"));
                    card.Icon.Source = ImageSource.FromStream(() => { return new MemoryStream(image); });

                    //card.Tags = 
                    //card.Author = (int) obj.GetValue("id");
                }

                /*id = tokenJson.Value<int>("id");
                token = tokenJson.Value<string>("token");
                this.rememberMe = rememberMe;
                await SaveAuth();
                return true;*/
            }

            return null;
        }

        public async Task<Offer> GetOffer(int owner)
        {            
            var id = owner.ToString();
            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/offers/:" + id);
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
                List<Tag> tags = new List<Tag>();
                IEnumerable<Tuple<string, string>> jsonTags = tokenJson.Value<IEnumerable<Tuple<string, string>>>("tags");
                foreach (Tuple<string, string> tag in jsonTags)
                {
                    ColorTypeConverter converter = new ColorTypeConverter();
                    Tag definitiveTag = new Tag { Color = (Color)converter.ConvertFromInvariantString(tag.Item1), Name = tag.Item2 };
                    tags.Add(definitiveTag);
                }
                post.Tags = tags;
                var icon = Encoding.UTF8.GetBytes((string)tokenJson.Value<string>("icon"));                
                post.Icon.Source = ImageSource.FromStream(() => { return new MemoryStream(icon); });
                post.Photos = new List<Image>();
                //falta coger el array de photos y de tags
                return post;
            }
            return null;
        }

        public async Task<Request> GetRequest(int owner)
        {           
            var id = owner.ToString();
            var response = await httpClient.GetAsync("http://server.vgafib.org/api/posts/requests/:" + id);
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
                IEnumerable<Tuple<string, string>> jsonTags = tokenJson.Value <IEnumerable<Tuple<string, string>>>("tags");
                foreach (Tuple<string,string> tag in jsonTags)
                {
                    ColorTypeConverter converter = new ColorTypeConverter();
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
