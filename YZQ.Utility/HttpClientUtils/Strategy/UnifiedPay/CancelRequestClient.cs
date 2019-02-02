using System;
using System.Net.Http;
using System.Text;

namespace YZQ.Utility.HttpClientUtils
{
    /// <summary>
    /// CancelRequest专用Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class CancelRequestClient
        : YZQ.Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_CancelRequestClient = new object();
        private static volatile HttpClient httpClient_CancelRequestClient = null;
        private const int Timeout = 5;

        public CancelRequestClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_CancelRequestClient;
        }

        public CancelRequestClient(string format)
            : this()
        {
            base.Format = format;
        }

        public CancelRequestClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        public CancelRequestClient(string format, Encoding charset, string contentType)
            : this(format: format, charset: charset)
        {
            base.ContentType = contentType;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_CancelRequestClient == null)
            {
                lock (lockHelper_CancelRequestClient)
                {
                    if (httpClient_CancelRequestClient == null)
                    {
                        httpClient_CancelRequestClient = new HttpClient();
                        httpClient_CancelRequestClient.Timeout = new TimeSpan(0, 0, seconds: Timeout);
                        httpClient_CancelRequestClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_CancelRequestClient.DefaultRequestHeaders.Add("User-Agent", "Cysoft.UnifiedPay.CancelRequestClient");
                        httpClient_CancelRequestClient.DefaultRequestHeaders.Add("DNT", "1");

                        httpClient_CancelRequestClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    }
                }
            }
        }
    }
}
