/************************************************************************
 * 文件标识：  ddbdc6e0-9dff-49d0-8bc4-66f3eab5b3b1
 * 项目名称：  YZQ.Utility.HttpClientUtils.Strategy  
 * 项目描述：  
 * 类 名 称：  BestpayClient
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/7/2 13:35:10
 * 更新时间：  2018/7/2 13:35:10
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Net.Http;
using System.Text;

namespace YZQ.Utility.HttpClientUtils
{
    public class BestpayPayccClient
        : YZQ.Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_BestpayClient = new object();
        private static volatile HttpClient httpClient_BestpayClient = null;
        private const int Timeout = 8;


        public BestpayPayccClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_BestpayClient;
        }

        public BestpayPayccClient(string format)
            : this()
        {
            base.Format = format;
        }

        public BestpayPayccClient(string format, Encoding charset)
            : this(format: format)
        {
            base.Charset = charset;
        }

        public BestpayPayccClient(string format, Encoding charset, string contentType)
            : this(format: format, charset: charset)
        {
            base.ContentType = contentType;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_BestpayClient == null)
            {
                lock (lockHelper_BestpayClient)
                {
                    if (httpClient_BestpayClient == null)
                    {
                        httpClient_BestpayClient = new HttpClient();
                        httpClient_BestpayClient.Timeout = new TimeSpan(0, 0, seconds: Timeout);
                        httpClient_BestpayClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_BestpayClient.DefaultRequestHeaders.Add("User-Agent", "Cysoft.UnifiedPay");
                        httpClient_BestpayClient.DefaultRequestHeaders.Add("DNT", "1");

                        httpClient_BestpayClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    }
                }
            }
        }
    }
}
