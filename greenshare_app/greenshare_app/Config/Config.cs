using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace greenshare_app.Config
{
    public class Config
    {
        private static Config instance;

        private Config()
        {
            var file = File.ReadAllText("./config.json");
            var config = JsonConvert.DeserializeObject<MainConfig>(file);
            BaseServerUrl = config.BaseServerUrl;
        }

        public static Config Instance()
        {
            if (instance is null) instance = new Config();
            return instance;
        }
        public string BaseServerUrl { get; private set; }

        private class MainConfig
        {
            [JsonProperty(PropertyName = "baseServerUrl")]
            public string BaseServerUrl { get; set; }
        }

    }
}
