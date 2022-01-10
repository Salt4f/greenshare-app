using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace greenshare_app.Models
{
    public class QuizQuestion
    {
        [JsonProperty(PropertyName = "Pregunta")]
        public string Question { get; set; }

        [JsonProperty(PropertyName = "Resposta 1")]
        public string Response1 { get; set; }

        [JsonProperty(PropertyName = "Resposta 2")]
        public string Response2 { get; set; }

        [JsonProperty(PropertyName = "Resposta 3")]
        public string Response3 { get; set; }

        [JsonProperty(PropertyName = "Resposta 4")]
        public string Response4 { get; set; }

    }
}
