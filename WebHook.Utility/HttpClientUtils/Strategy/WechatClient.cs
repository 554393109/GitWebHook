﻿using System;
using System.Net.Http;
using System.Text;

namespace WebHook.Utility.HttpClientUtils
{
    /// <summary>
    /// 微信无证书策略Client
    /// 在当前类中保存该业务的HttpClient
    /// </summary>
    public sealed class WechatClient
        : WebHook.Utility.HttpClientUtils.BaseClient
    {
        private static readonly object lockHelper_WechatClient = new object();
        private static volatile HttpClient httpClient_WechatClient = null;
        private const int Timeout = 10;

        public WechatClient()
        {
            this.Get_ClientInstance();
            base.Client = httpClient_WechatClient;
        }

        public WechatClient(string format)
            : this()
        {
            base.Format = format;
        }

        public WechatClient(string format, Encoding charset)
            : this(format)
        {
            base.Charset = charset;
        }

        private void Get_ClientInstance()
        {
            if (httpClient_WechatClient == null)
            {
                lock (lockHelper_WechatClient)
                {
                    if (httpClient_WechatClient == null)
                    {
                        httpClient_WechatClient = new HttpClient();
                        httpClient_WechatClient.Timeout = new TimeSpan(0, 0, Timeout);
                        httpClient_WechatClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                        httpClient_WechatClient.DefaultRequestHeaders.Add("User-Agent", "WebHook");
                        httpClient_WechatClient.DefaultRequestHeaders.Add("DNT", "1");
                    }
                }
            }
        }
    }
}
