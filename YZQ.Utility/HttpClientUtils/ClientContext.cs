using System.Collections.Generic;

namespace YZQ.Utility.HttpClientUtils
{
    public class ClientContext
    {
        private Utility.HttpClientUtils.IClient client;

        public ClientContext(IClient _client)
        {
            this.client = _client;
        }

        public string Post(string url, object content)
        {
            return this.client.Post(url, content);
        }

        public string Post(string url, object content, IDictionary<string, FileItem> fileParams)
        {
            return this.client.Post(url, content, fileParams);
        }

        public void PostAsync(string url, object content)
        {
            this.client.PostAsync(url, content);
        }
    }
}
