using System;
using System.Net.Http;
using System.Text;

namespace YZQ.Utility.HttpClientUtils
{
    /// <summary>
    /// 经销商策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class JxsClient
        : YZQ.Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_JxsClient = new object();
        private static volatile HttpClient httpClient_JxsClient = null;
        private const int Timeout = 15;

        public JxsClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_JxsClient;
        }

        public JxsClient(string format)
            : this()
        {
            base.Format = format;
        }

        public JxsClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        public JxsClient(string format, Encoding charset, string contentType)
            : this(format: format, charset: charset)
        {
            base.ContentType = contentType;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_JxsClient == null)
            {
                lock (lockHelper_JxsClient)
                {
                    if (httpClient_JxsClient == null)
                    {
                        httpClient_JxsClient = new HttpClient();
                        httpClient_JxsClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_JxsClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_JxsClient.DefaultRequestHeaders.Add("User-Agent", "Cysoft.UnifiedPay");
                        httpClient_JxsClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
