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
    public class UserInfoUtil
    {
        private static UserInfoUtil instance;

        private UserInfoUtil()
        {
            httpClient = new HttpClient();
        }

        public static UserInfoUtil Instance()
        {
            if (instance is null) instance = new UserInfoUtil();
            return instance;
        }

        private readonly HttpClient httpClient;


    }
}
