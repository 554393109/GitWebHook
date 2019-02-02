using System;
using System.Net.Http;
using System.Text;

namespace YZQ.Utility.HttpClientUtils
{
    /// <summary>
    /// OrderQueryRequest专用Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class OrderQueryRequestClient
        : YZQ.Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_OrderQueryRequestClient = new object();
        private static volatile HttpClient httpClient_OrderQueryRequestClient = null;
        private const int Timeout = 3;

        public OrderQueryRequestClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_OrderQueryRequestClient;
        }

        public OrderQueryRequestClient(string format)
            : this()
        {
            base.Format = format;
        }

        public OrderQueryRequestClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        public OrderQueryRequestClient(string format, Encoding charset, string contentType)
            : this(format: format, charset: charset)
        {
            base.ContentType = contentType;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_OrderQueryRequestClient == null)
            {
                lock (lockHelper_OrderQueryRequestClient)
                {
                    if (httpClient_OrderQueryRequestClient == null)
                    {
                        httpClient_OrderQueryRequestClient = new HttpClient();
                        httpClient_OrderQueryRequestClient.Timeout = new TimeSpan(0, 0, seconds: Timeout);
                        httpClient_OrderQueryRequestClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_OrderQueryRequestClient.DefaultRequestHeaders.Add("User-Agent", "Cysoft.UnifiedPay.OrderQueryRequestClient");
                        httpClient_OrderQueryRequestClient.DefaultRequestHeaders.Add("DNT", "1");

                        httpClient_OrderQueryRequestClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    }
                }
            }
        }
    }
}
