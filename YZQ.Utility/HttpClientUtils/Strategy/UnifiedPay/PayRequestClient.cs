using System;
using System.Net.Http;
using System.Text;

namespace YZQ.Utility.HttpClientUtils
{
    /// <summary>
    /// PayRequest专用Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class PayRequestClient
        : YZQ.Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_PayRequestClient = new object();
        private static volatile HttpClient httpClient_PayRequestClient = null;
        private const int Timeout = 5;

        public PayRequestClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_PayRequestClient;
        }

        public PayRequestClient(string format)
            : this()
        {
            base.Format = format;
        }

        public PayRequestClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        public PayRequestClient(string format, Encoding charset, string contentType)
            : this(format: format, charset: charset)
        {
            base.ContentType = contentType;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_PayRequestClient == null)
            {
                lock (lockHelper_PayRequestClient)
                {
                    if (httpClient_PayRequestClient == null)
                    {
                        httpClient_PayRequestClient = new HttpClient();
                        httpClient_PayRequestClient.Timeout = new TimeSpan(0, 0, seconds: Timeout);
                        httpClient_PayRequestClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_PayRequestClient.DefaultRequestHeaders.Add("User-Agent", "Cysoft.UnifiedPay.PayRequestClient");
                        httpClient_PayRequestClient.DefaultRequestHeaders.Add("DNT", "1");

                        httpClient_PayRequestClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    }
                }
            }
        }
    }
}
