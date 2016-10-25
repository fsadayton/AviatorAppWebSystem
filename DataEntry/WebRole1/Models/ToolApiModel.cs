using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Website.Models
{
    public class ToolApiModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "websiteUrl")]
        public string WebsiteUrl { get; set; }

        [JsonProperty(PropertyName = "appStoreUrl")]
        public string AppStoreUrl { get; set; }
    }
}