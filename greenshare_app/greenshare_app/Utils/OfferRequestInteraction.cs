﻿using greenshare_app.Exceptions;
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

        private readonly HttpClient httpClient;
      
        public async Task<bool> RequestAnOffer(int offerId, int requestId)
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
            SessionInfo sessionInfo = new SessionInfo { Id = session.Item1, Token = session.Item2 };
            string json = JsonConvert.SerializeObject(sessionInfo);
            var httpContent = new StringContent(json);
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
            Tuple<int, string> session;
            try
            {
                session = await Auth.Instance().GetAuth();

            }
            catch (Exception)
            {
                throw new InvalidLoginException();
            }
            SessionInfo sessionInfo = new SessionInfo { Id = session.Item1, Token = session.Item2 };
            string json = JsonConvert.SerializeObject(sessionInfo);
            var httpContent = new StringContent(json);
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
            Tuple<int, string> session;
            try
            {
                session = await Auth.Instance().GetAuth();

            }
            catch (Exception)
            {
                throw new InvalidLoginException();
            }
            SessionInfo sessionInfo = new SessionInfo { Id = session.Item1, Token = session.Item2 };
            string json = JsonConvert.SerializeObject(sessionInfo);
            var httpContent = new StringContent(json);
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
            Tuple<int, string> session;
            try
            {
                session = await Auth.Instance().GetAuth();

            }
            catch (Exception)
            {
                throw new InvalidLoginException();
            }
            SessionInfo sessionInfo = new SessionInfo { Id = session.Item1, Token = session.Item2 };
            string json = JsonConvert.SerializeObject(sessionInfo);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/offers/" + offerId + "/request/" + requestId + "/reject", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        //Completa una offer / request. Això marca la request i la offer com no actives, i indica que s'ha completat la transacció sense problemes.
        public async Task<bool> CompletePost(int offerId, int requestId)
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
            SessionInfo sessionInfo = new SessionInfo { Id = session.Item1, Token = session.Item2 };
            string json = JsonConvert.SerializeObject(sessionInfo);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync("http://server.vgafib.org/api/posts/offers/" + offerId + "/request/" + requestId + "/completed", httpContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        private class SessionInfo
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "token")]
            public string Token { get; set; }
        }
    }
}
