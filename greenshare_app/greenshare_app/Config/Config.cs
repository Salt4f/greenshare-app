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
            BaseServerUrl = config.BaseServerUrl;
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

        public string BaseServerUrl { get; private set; }

        private class MainConfig
        {
            [JsonProperty(PropertyName = "baseServerUrl")]
            public string BaseServerUrl { get; set; }
        }

    }
}
