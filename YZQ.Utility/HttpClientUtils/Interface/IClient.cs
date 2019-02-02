using System.Collections.Generic;
using System.Threading.Tasks;

namespace YZQ.Utility.HttpClientUtils
{
    public interface IClient
    {
        string Post(string url, object content);

        string Post(string url, object content, IDictionary<string, FileItem> fileParams);

        //Task PostAsync(string requestUri, object content);
        void PostAsync(string url, object content);

        //TResult PostAsync<TResult>(string url, object content);
    }
}
