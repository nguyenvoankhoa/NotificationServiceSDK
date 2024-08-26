using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServiceSDK
{
    [JsonObject(IsReference = false)]
    public class Content
    {
        public Content()
        {
        }
      
        public Content(string type, string value)
        {
            this.TemplateType = type;
            this.Value = value;
        }
        
        [JsonProperty(PropertyName = "type")]
        public string TemplateType { get; set; }

       
        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }
    }
}
