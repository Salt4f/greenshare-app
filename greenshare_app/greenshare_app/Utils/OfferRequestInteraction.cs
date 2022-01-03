﻿using greenshare_app.Exceptions;
using greenshare_app.Models;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<PendingPostInteraction>> GetPendingPosts(string interactionType)
        {
            Tuple<int, string> session = await Auth.Instance().GetAuth();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://server.vgafib.org/api/user/" + session.Item1+"/pending-posts?type="+interactionType);
            request = await Auth.AddHeaders(request);
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var pendingPosts = new ObservableRangeCollection<PendingPostInteraction>();

                return pendingPosts;
            }
            return new ObservableRangeCollection<PendingPostInteraction>();
        }

        public async Task<bool> RequestAnOffer(int offerId, int requestId)
        {
            HttpContent httpContent = new StringContent("");
            httpContent = await Auth.AddHeaders(httpContent);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/offers/"+offerId+"/request/"+requestId, httpContent);
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
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/requests/" + requestId + "/offer/" + offerId, httpContent);
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
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/offers/" + offerId + "/request/" + requestId + "/accept", httpContent);
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
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/offers/" + offerId + "/request/" + requestId + "/reject", httpContent);
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
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/requests/" + requestId + "/offer/" + offerId + "/accept", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        //Completa una offer / request. Això marca la request i la offer com no actives, i indica que s'ha completat la transacció sense problemes.
        public async Task<bool> CompletePostFromRequest(int offerId, int requestId, string valoration = null)
        {           
            CompletionInfo valorationInfo = new CompletionInfo { Valoration = valoration };
            string json = JsonConvert.SerializeObject(valorationInfo);
            HttpContent httpContent = new StringContent(json);
            httpContent = await Auth.AddHeaders(httpContent);
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/offers/" + offerId + "/request/" + requestId + "/completed", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        private class CompletionInfo
        {
            [JsonProperty(PropertyName = "valoration")]
            public string Valoration { get; set; }
        }
    }
}
