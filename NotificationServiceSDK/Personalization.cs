using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NotificationServiceSDK
{
    [JsonObject(IsReference = false)]
    public class Personalization
    {
        [JsonProperty(PropertyName = "to", IsReference = false)]
        [JsonConverter(typeof(RemoveDuplicatesConverter<EmailAddress>))]
        public List<EmailAddress> Tos { get; set; }

        [JsonProperty(PropertyName = "from")]
        public EmailAddress From { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "dynamic_template_data", IsReference = false, ItemIsReference = false)]
        public object TemplateData { get; set; }
    }
}
