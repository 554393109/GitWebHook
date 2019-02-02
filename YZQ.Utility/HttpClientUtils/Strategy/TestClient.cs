using System;
using System.Net.Http;
using System.Text;

namespace YZQ.Utility.HttpClientUtils
{
    /// <summary>
    /// Test无证书策略Client
    /// 专用于测试
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class TestClient
        : YZQ.Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_TestClient = new object();
        private static volatile HttpClient httpClient_TestClient = null;
        private const int Timeout = 15;

        public TestClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_TestClient;
        }

        public TestClient(string format)
            : this()
        {
            base.Format = format;
        }

        public TestClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_TestClient == null)
            {
                lock (lockHelper_TestClient)
                {
                    if (httpClient_TestClient == null)
                    {
                        httpClient_TestClient = new HttpClient();
                        httpClient_TestClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_TestClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_TestClient.DefaultRequestHeaders.Add("User-Agent", "Cysoft.PayProxy.Test");
                        httpClient_TestClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
