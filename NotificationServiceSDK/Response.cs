using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServiceSDK
{
    public class Response
    {
        private HttpStatusCode _statusCode;
        private HttpContent _body;
        private HttpResponseHeaders _headers;
        public Response(HttpStatusCode statusCode, HttpContent responseBody, HttpResponseHeaders responseHeaders)
        {
            this.StatusCode = statusCode;
            this.Body = responseBody;
            this.Headers = responseHeaders;
        }
        public HttpStatusCode StatusCode
        {
            get
            {
                return this._statusCode;
            }

            set
            {
                this._statusCode = value;
            }
        }
        public bool IsSuccessStatusCode
        {
            get { return ((int)StatusCode >= 200) && ((int)StatusCode <= 299); }
        }
        public HttpContent Body
        {
            get
            {
                return this._body;
            }

            set
            {
                this._body = value;
            }
        }
        public HttpResponseHeaders Headers
        {
            get
            {
                return this._headers;
            }

            set
            {
                this._headers = value;
            }
        }

     
        public virtual async Task<Dictionary<string, dynamic>> DeserializeResponseBodyAsync(HttpContent content = null)
        {
            content = content ?? this._body;
            if (content is null)
            {
                return new Dictionary<string, dynamic>();
            }

            var stringContent = await content.ReadAsStringAsync().ConfigureAwait(false);
            var dsContent = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(stringContent);
            return dsContent;
        }

        public virtual Dictionary<string, string> DeserializeResponseHeaders(HttpResponseHeaders headers = null)
        {
            var dsContent = new Dictionary<string, string>();

            headers = headers ?? this._headers;
            if (headers == null)
            {
                return dsContent;
            }

            foreach (var pair in headers)
            {
                dsContent.Add(pair.Key, pair.Value.First());
            }

            return dsContent;
        }
    }
}
