using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServiceSDK
{
    public class Client
    {
        private readonly HttpClient client;
        private readonly string baseUrl = "http://localhost:8080/api/";
        public Client()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public Client(string apiKey) : this() {
            this.apiKey = apiKey;
        }

        public enum Method
        {
            DELETE,
            GET,
            PATCH,
            POST,
            PUT,
        }

        public string apiKey { get; set; }

        public async Task<Response> MakeRequest(HttpRequestMessage request)
        {
            HttpResponseMessage response = await this.client.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            // Correct invalid UTF-8 charset values returned by API
            if (response.Content?.Headers?.ContentType?.CharSet == "utf8")
            {
                response.Content.Headers.ContentType.CharSet = Encoding.UTF8.WebName;
            }

            return new Response(response.StatusCode, response.Content, response.Headers);
        }
        public async Task<string> SendEmailAsync(Message msg)
        {
            var json = JsonConvert.SerializeObject(msg);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(baseUrl + "notification/send", content);

            // Process the response as needed
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }



        public async Task<Response> RequestAsync(
            Method method,
            string requestBody = null,
            string queryParams = null,
            string urlPath = null)
        {
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(method.ToString()),
                RequestUri = new Uri(client.BaseAddress, BuildUrl(urlPath, queryParams)),
                Content = requestBody == null ? null : new StringContent(requestBody, Encoding.UTF8),
            };

            return await this.MakeRequest(request).ConfigureAwait(false);
        }

        private string BuildUrl(string urlPath, string queryParams = null)
        {
            string url = null;


            if (queryParams != null)
            {
                var ds_query_params = this.ParseJson(queryParams);
                string query = "?";
                foreach (var pair in ds_query_params)
                {
                    foreach (var element in pair.Value)
                    {
                        if (query != "?")
                        {
                            query += "&";
                        }

                        query = query + pair.Key + "=" + element;
                    }
                }

                url += query;
            }

            return url;
        }

        private Dictionary<string, List<object>> ParseJson(string json)
        {
            var dict = new Dictionary<string, List<object>>();

            using (var sr = new StringReader(json))
            using (var reader = new JsonTextReader(sr))
            {
                var propertyName = string.Empty;
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.PropertyName:
                            {
                                propertyName = reader.Value.ToString();
                                if (!dict.ContainsKey(propertyName))
                                {
                                    dict.Add(propertyName, new List<object>());
                                }

                                break;
                            }

                        case JsonToken.Boolean:
                        case JsonToken.Integer:
                        case JsonToken.Float:
                        case JsonToken.Bytes:
                        case JsonToken.String:
                        case JsonToken.Date:
                            {
                                dict[propertyName].Add(reader.Value);
                                break;
                            }
                    }
                }
            }

            return dict;
        }

    }
}
