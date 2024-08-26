using Newtonsoft.Json;


namespace NotificationServiceSDK
{
    public class EmailAddress
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        public EmailAddress(string email, string name = null)
        {
            this.Email = email;
            this.Name = name;
        }

    }
}
