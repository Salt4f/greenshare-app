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
    public class GoogleUtil
    {
        private static GoogleUtil instance;

        private GoogleUtil()
        {
            httpClient = new HttpClient();
        }

        public static GoogleUtil Instance()
        {
            if (instance is null) instance = new GoogleUtil();
            return instance;
        }

        private readonly HttpClient httpClient;
    }
    public async Task<bool> Login()
    {
        var rand = new Random();
        string token = Crypto.GetHashString((rand.Next()*rand.Next()).ToString());

    }
}
