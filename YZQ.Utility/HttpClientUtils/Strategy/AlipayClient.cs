using System;
using System.Net.Http;
using System.Text;

namespace YZQ.Utility.HttpClientUtils
{
    /// <summary>
    /// Alipay无证书策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class AlipayClient
        : YZQ.Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_AlipayClient = new object();
        private static volatile HttpClient httpClient_AlipayClient = null;
        private const int Timeout = 10;

        public AlipayClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_AlipayClient;
        }

        public AlipayClient(string format)
            : this()
        {
            base.Format = format;
        }

        public AlipayClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_AlipayClient == null)
            {
                lock (lockHelper_AlipayClient)
                {
                    if (httpClient_AlipayClient == null)
                    {
                        httpClient_AlipayClient = new HttpClient();
                        httpClient_AlipayClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_AlipayClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_AlipayClient.DefaultRequestHeaders.Add("User-Agent", "Cysoft.UnifiedPay");
                        httpClient_AlipayClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
