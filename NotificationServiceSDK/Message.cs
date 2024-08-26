using Newtonsoft.Json;
using System.Globalization;
using System.Text;
namespace NotificationServiceSDK
{
    public class Message
    {

        [JsonProperty(PropertyName = "from")]
        public EmailAddress From { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public string? Subject { get; set; }


        [JsonProperty(PropertyName = "contents", IsReference = false)]
        public List<Content> Contents { get; set; }



        [JsonProperty(PropertyName = "templateId")]
        public string? TemplateId { get; set; }


        [JsonProperty(PropertyName = "tos")]
        public List<string> To { get; set; }

        [JsonProperty(PropertyName = "targetOutput")]
        public TargetOutput TargetOutput { get; set; }


        public void SetTarget(TargetOutput targetOutput)
        {
            this.TargetOutput = targetOutput;
        }
        public string Serialize(bool useDefaultSerialization = true)
        {
            if (this.Contents != null)
            {
                if (this.Contents.Count > 1)
                {
                    // MimeType.Text > MimeType.Html > Everything Else
                    for (var i = 0; i < this.Contents.Count; i++)
                    {
                        if (string.IsNullOrEmpty(this.Contents[i].TemplateType.ToString()) || Contents[i].Value != null)
                        {
                            this.Contents.RemoveAt(i);
                            i--;
                            continue;
                        }

                        if (Contents[i].TemplateType.Equals(MimeType.Html))
                        {
                            var tempContent = new Content();
                            tempContent = this.Contents[i];
                            this.Contents.RemoveAt(i);
                            this.Contents.Insert(0, tempContent);
                        }

                        if (Contents[i].TemplateType.Equals(MimeType.Text))
                        {
                            var tempContent = new Content();
                            tempContent = this.Contents[i];
                            this.Contents.RemoveAt(i);
                            this.Contents.Insert(0, tempContent);
                        }
                    }
                }
            }

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                Formatting = Formatting.None
            };

            if (useDefaultSerialization)
            {
                return JsonConvert.SerializeObject(
                                                   this,
                                                   Formatting.None,
                                                   jsonSerializerSettings);
            }

            var jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);

            var stringBuilder = new StringBuilder(256);
            var textWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                jsonWriter.Formatting = jsonSerializer.Formatting;
                jsonSerializer.Serialize(jsonWriter, this);
            }

            return textWriter.ToString();
        }

        public static Message Deserialize(string json)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                Formatting = Formatting.None
            };

            var jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);

            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            Message message = jsonSerializer.Deserialize<Message>(reader);

            return message;
        }

        public void AddTo(List<string> tos)
        {
            if (this.To == null)
            {
                this.To = new List<string>();
                this.To = tos;
            }
            else
            {
                this.To.AddRange(tos);
            }
        }


        public void SetFrom(string email, string name = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException("email");
            }

            this.SetFrom(new EmailAddress(email, name));
        }

        public void SetFrom(EmailAddress email)
        {
            this.From = email;
        }

        public void AddContent(string mimeType, object text)
        {
            var content = new Content()
            {
                TemplateType = mimeType,
                Value = text,
            };

            if (this.Contents == null)
            {
                this.Contents = new List<Content>()
                {
                    content,
                };
            }
            else
            {
                this.Contents.Add(content);
            }

            return;
        }


        public void AddContents(Dictionary<string, object> contents)
        {
            foreach (var item in contents)
            {
                AddContent(item.Key, item.Value);
            }
        }

        public void SetTemplateId(string? templateID)
        {
            this.TemplateId = templateID;
        }


        public void SetSubject(string subject)
        {
            this.Subject = subject;
        }
    }


}
