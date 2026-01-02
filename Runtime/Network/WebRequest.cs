using System;
using System.Collections.Generic;
using System.Text;

namespace Spyke.Services.Network
{
    /// <summary>
    /// Fluent builder for HTTP requests.
    /// </summary>
    public class WebRequest
    {
        public string Url { get; private set; }
        public HttpMethod Method { get; private set; } = HttpMethod.GET;
        public Dictionary<string, string> Headers { get; private set; }
        public Dictionary<string, string> QueryParameters { get; private set; }
        public string StringBody { get; private set; }
        public byte[] RawBody { get; private set; }
        public int TimeoutSeconds { get; private set; } = 30;

        public WebRequest(string url)
        {
            Url = url;
        }

        public WebRequest SetMethod(HttpMethod method)
        {
            Method = method;
            return this;
        }

        public WebRequest Get() => SetMethod(HttpMethod.GET);
        public WebRequest Post() => SetMethod(HttpMethod.POST);
        public WebRequest Put() => SetMethod(HttpMethod.PUT);
        public WebRequest Delete() => SetMethod(HttpMethod.DELETE);

        public WebRequest AddHeader(string name, string value)
        {
            Headers ??= new Dictionary<string, string>();
            Headers[name] = value;
            return this;
        }

        public WebRequest AddQueryParameter(string name, string value)
        {
            QueryParameters ??= new Dictionary<string, string>();
            QueryParameters[name] = value;
            return this;
        }

        public WebRequest SetBody(string json)
        {
            StringBody = json;
            return this;
        }

        public WebRequest SetBody(byte[] data)
        {
            RawBody = data;
            return this;
        }

        public WebRequest SetTimeout(int seconds)
        {
            TimeoutSeconds = seconds;
            return this;
        }

        public WebRequest Build()
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentException("URL cannot be null or empty");
            }

            if (QueryParameters != null && QueryParameters.Count > 0)
            {
                var sb = new StringBuilder(Url);
                sb.Append(Url.Contains("?") ? "&" : "?");

                var first = true;
                foreach (var param in QueryParameters)
                {
                    if (!first) sb.Append("&");
                    sb.Append(Uri.EscapeDataString(param.Key));
                    sb.Append("=");
                    sb.Append(Uri.EscapeDataString(param.Value));
                    first = false;
                }

                Url = sb.ToString();
            }

            return this;
        }
    }
}
