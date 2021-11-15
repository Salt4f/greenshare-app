﻿using System;
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
    }
}
