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
    public class OfferRequestInteraction
    {
        private static OfferRequestInteraction instance;

        private OfferRequestInteraction()
        {
            httpClient = new HttpClient();
        }

        public static OfferRequestInteraction Instance()
        {
            if (instance is null) instance = new OfferRequestInteraction();
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

        public async Task<List<PendingPostInteraction>> GetPendingPosts(string interactionType, INavigation navigation, Page view)
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/user/" + session.Item1+"/pending-posts?type="+interactionType);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var pendingPosts = new List<PendingPostInteraction>();
                foreach (var item in array)
                {                    
                    if (interactionType == "incoming")
                    {
                        var info = item.ToObject<PendingPostInteractionInfo>();
                        foreach (var postsArray in info.Posts)
                        {
                            var pending = new PendingPostInteraction(navigation, view)
                            {
                                OwnPostId = info.OwnPostId,
                                UserName = postsArray.NickName,
                                UserId = postsArray.UserId,
                                PostId = postsArray.Id,
                            };
                            if (postsArray.PostType == "offer")
                            {
                                pending.PostType = "request";
                                pending.PostName = postsArray.PostName;
                                pending.InteractionText = pending.UserName + " is offering you a " + pending.PostName;
                            }
                            else
                            {
                                pending.PostType = "offer";
                                pending.PostName = info.OwnPostName;
                                pending.InteractionText = pending.UserName + " is requesting your " + pending.PostName;

                            }
                            pendingPosts.Add(pending);
                        }

                    }
                    else if (interactionType == "outgoing")
                    {
                        var info = item.ToObject<IncomingPostsInfo>();
                        var pending = new PendingPostInteraction(navigation,view)
                        {
                            OwnPostId = info.OwnPostId,
                            UserName = info.NickName,
                            UserId = info.UserId,
                            PostId = info.Id,
                            PostName = info.PostName,
                        };
                        if (info.PostType == "offer")
                        {
                            pending.PostType = "request";
                            pending.InteractionText = "Waiting for " + pending.UserName + " to answer your request on " + pending.PostName;
                        }
                        else
                        {
                            pending.PostType = "offer";
                            pending.InteractionText = "Waiting for " + pending.UserName + " to answer your offer on " + pending.PostName;
                        }
                        pendingPosts.Add(pending);
                    }
                }
                return pendingPosts;
            }
            return new List<PendingPostInteraction>();
        }

        public async Task<List<AcceptedPostInteraction>> GetAcceptedPosts(string interactionType, INavigation navigation, Page view)
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            var request = new HttpRequestMessage(HttpMethod.Get, Config.Config.Instance().BaseServerUrl + "/user/" + session.Item1 + "/accepted-posts?type=" + interactionType);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(await response.Content.ReadAsStringAsync());
                var acceptedPosts = new List<AcceptedPostInteraction>();
                foreach (var item in array)
                {
                    var info = item.ToObject<AcceptedPostInteractionInfo>();
                    var accepted = new AcceptedPostInteraction(navigation,view)
                    {
                        OfferId = info.OfferId,
                        OfferName = info.OfferName,
                        UserId = info.UserId,
                        UserName = info.UserName,
                        RequestId = info.RequestId,
                    };
                }
                return acceptedPosts;
            }
            return new List<AcceptedPostInteraction>();
        }

        public async Task<bool> RequestAnOffer(int offerId, int requestId)
        {
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/posts/offers/" + offerId+"/request/"+requestId, httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {                
                return true;
            }
            return false;
        }


        public async Task<bool> OfferARequest(int offerId, int requestId)
        {
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/posts/requests/" + requestId + "/offer/" + offerId, httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;

        }
        //Una oferta accepta la petició d'un altre usuari
        public async Task<bool> AcceptRequest(int offerId, int requestId)
        {
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/posts/offers/" + offerId + "/request/" + requestId + "/accept", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }


        //Una oferta denega la petició d'un altre usuari
        public async Task<bool> RejectRequest(int offerId, int requestId)
        {
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/posts/offers/" + offerId + "/request/" + requestId + "/reject", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RejectOffer(int offerId, int requestId)
        {
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/posts/requests/" + requestId + "/offer/" + offerId + "/reject", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AcceptOffer(int offerId, int requestId)
        {
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl + "/posts/requests/" + requestId + "/offer/" + offerId + "/accept", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        //Completa una offer / request. Això marca la request i la offer com no actives, i indica que s'ha completat la transacció sense problemes.
        public async Task<bool> CompletePostFromOffer(int offerId, int requestId, int valoration)
        {           
            CompletionInfo valorationInfo = new CompletionInfo { Valoration = valoration };
            string json = JsonConvert.SerializeObject(valorationInfo);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            var response = await httpClient.PostAsync(Config.Config.Instance().BaseServerUrl +"/posts/offers/" + offerId + "/request/" + requestId + "/completed", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        private class CompletionInfo
        {
            [JsonProperty(PropertyName = "valoration")]
            public int Valoration { get; set; }
        }

        private class AcceptedPostInteractionInfo
        {
            [JsonProperty(PropertyName = "offerId")]
            public int OfferId { get; set; }

            [JsonProperty(PropertyName = "offerName")]
            public string OfferName { get; set; }

            [JsonProperty(PropertyName = "requestId")]
            public int RequestId { get; set; }

            [JsonProperty(PropertyName = "userName")]
            public string UserName { get; set; }

            [JsonProperty(PropertyName = "userId")]
            public int UserId { get; set; }
        }
        private class PendingPostInteractionInfo
        {
            [JsonProperty(PropertyName = "ownPostName")]
            public string OwnPostName { get; set; }

            [JsonProperty(PropertyName = "ownPostId")]
            public int OwnPostId { get; set; }            

            [JsonProperty(PropertyName = "posts")]
            public IEnumerable<IncomingPostsInfo> Posts { get; set; }
        }                  
        private class IncomingPostsInfo : PendingPostInteractionInfo
        {
            [JsonProperty(PropertyName = "postId")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "postName")]
            public string PostName { get; set; }

            [JsonProperty(PropertyName = "postType")]
            public string PostType { get; set; }

            [JsonProperty(PropertyName = "nickname")]
            public string NickName { get; set; }

            [JsonProperty(PropertyName = "userId")]   //outgoing only
            public int UserId { get; set; }
        }
    }
}
