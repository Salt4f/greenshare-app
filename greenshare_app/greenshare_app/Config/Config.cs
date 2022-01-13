using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace greenshare_app.Config
{
    public class Config
    {
        private static Config instance;

        private Config()
        {
            var file = ReadConfigFile();
            var config = JsonConvert.DeserializeObject<MainConfig>(file);
            BaseServerApiUrl = config.BaseServerApiUrl;
            BaseServerGoogleUrl = config.BaseServerGoogleUrl;
            AdminId = config.AdminId;
        }

        public static Config Instance()
        {
            if (instance is null) instance = new Config();
            return instance;
        }

        private string ReadConfigFile()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Config)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("greenshare_app.Config.config.json");
            string text;
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }

        public string BaseServerApiUrl { get; private set; }
        public string BaseServerGoogleUrl { get; private set; }
        public int AdminId { get; private set; }

        private class MainConfig
        {
            [JsonProperty(PropertyName = "baseServerApiUrl")]
            public string BaseServerApiUrl { get; set; }

            [JsonProperty(PropertyName = "baseServerGoogleUrl")]
            public string BaseServerGoogleUrl { get; set; }

            [JsonProperty(PropertyName = "adminId")]
            public int AdminId { get; set; }
        }

    }
}
