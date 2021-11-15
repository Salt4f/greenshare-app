using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace greenshare_app.Utils
{
    class Get
    {
        private static Get instance;
        private Get()
        {
            httpClient = new HttpClient();
        }
        public static Get Instance()
        {
            if (instance is null) instance = new Get();
            return instance;
        }

        private HttpClient httpClient;

    }
}
