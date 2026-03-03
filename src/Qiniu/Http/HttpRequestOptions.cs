using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace Qiniu.Http
{
    public class HttpRequestOptions
    {
        // Options of HttpWebRequest
        public bool? AllowAutoRedirect { get; set; }
        public bool? AllowReadStreamBuffering { get; set; }
        public bool? AllowWriteStreamBuffering { get; set; }
        public AuthenticationLevel? AuthenticationLevel { get; set; }
        public DecompressionMethods? AutomaticDecompression { get; set; }
        public RequestCachePolicy CachePolicy { get; set; }
        public X509CertificateCollection ClientCertificates { get; set; }
        public string ConnectionGroupName { get; set; }
        public HttpContinueDelegate ContinueDelegate { get; set; }
        public int? ContinueTimeout { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public ICredentials Credentials { get; set; }
        public TokenImpersonationLevel? ImpersonationLevel { get; set; }
        public bool? KeepAlive { get; set; }
        public int? MaximumAutomaticRedirections { get; set; }
        public int? MaximumResponseHeadersLength { get; set; }
        public string MediaType { get; set; }
        public string Method { get; set; }
        public bool? Pipelined { get; set; }
        public bool? PreAuthenticate { get; set; }
        public IWebProxy Proxy { get; set; }
        public int? ReadWriteTimeout { get; set; }
        public bool? SendChunked { get; set; }
        public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }
        public int? Timeout { get; set; }
        public bool? UnsafeAuthenticatedConnectionSharing { get; set; }
        public bool? UseDefaultCredentials { get; set; }
        public HttpContent? RequestContent { get; set; }

        // Custom Options
        public string Url { get; set; }
        public StringDictionary Headers { get; set; }
        public Stream? RequestStream { get; set; }
        public byte[]? RequestData { get; set; }

        public HttpRequestOptions()
        {
            Headers = new StringDictionary();
        }

        public HttpRequestMessage CreateHttpRequestMessage()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new InvalidOperationException("Failed to create HttpRequestMessage because URL is empty.");
            }

            if (Method == null)
            {
                throw new InvalidOperationException("Failed to create HttpRequestMessage because HTTP method is empty.");
            }

            var message = new HttpRequestMessage(new HttpMethod(Method), Url);
            SetHeaders(message);
            SetBody(message);
            return message;
        }

        public HttpClientHandler CreateHttpClientHandler()
        {
            var handler = new HttpClientHandler();

            if (AllowAutoRedirect.HasValue)
            {
                handler.AllowAutoRedirect = AllowAutoRedirect.Value;
            }

            if (AutomaticDecompression.HasValue)
            {
                handler.AutomaticDecompression = AutomaticDecompression.Value;
            }

            if (ClientCertificates != null)
            {
                handler.ClientCertificates.AddRange(ClientCertificates);
            }

            if (CookieContainer != null)
            {
                handler.CookieContainer = CookieContainer;
            }

            if (Credentials != null)
            {
                handler.Credentials = Credentials;
            }

            if (PreAuthenticate.HasValue)
            {
                handler.PreAuthenticate = PreAuthenticate.Value;
            }

            if (Proxy != null)
            {
                handler.Proxy = Proxy;
            }

            if (UseDefaultCredentials.HasValue)
            {
                handler.UseDefaultCredentials = UseDefaultCredentials.Value;
            }

            if (ServerCertificateValidationCallback != null)
            {
                handler.ServerCertificateCustomValidationCallback = (message, certificate, chain, errors) =>
                    ServerCertificateValidationCallback(message?.RequestUri?.Host ?? string.Empty, certificate, chain, errors);
            }

            return handler;
        }

        private void SetHeaders(HttpRequestMessage request)
        {
            if (Headers == null)
            {
                return;
            }

            foreach (string fieldName in Headers.Keys)
            {
                string fieldVal = Headers[fieldName];
                if (!request.Headers.TryAddWithoutValidation(fieldName, fieldVal))
                {
                    if (request.Content == null)
                    {
                        request.Content = new ByteArrayContent(Array.Empty<byte>());
                    }

                    request.Content.Headers.TryAddWithoutValidation(fieldName, fieldVal);
                }
            }
        }

        private void SetBody(HttpRequestMessage request)
        {
            if (RequestContent != null)
            {
                request.Content = RequestContent;
            }
            else if (RequestData != null)
            {
                request.Content = new ByteArrayContent(RequestData);
            }
            else if (RequestStream != null)
            {
                request.Content = new StreamContent(RequestStream);
            }
        }
    }
}