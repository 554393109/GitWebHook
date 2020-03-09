using System.Text;

namespace WebHook.Utility.HttpClientUtils
{
    public interface IClientStrategy
        : WebHook.Utility.HttpClientUtils.IClient
    {
        ///// <summary>
        ///// 请求超时
        ///// 单位：秒
        ///// </summary>
        //int Timeout { get; set; }

        /// <summary>
        /// 内容编码
        /// 默认UTF-8
        /// </summary>
        Encoding Charset { get; set; }

        /// <summary>
        /// 参数格式
        /// 默认hash
        /// </summary>
        string Format { get; set; }


        //HttpClient Client { get; set; }
    }
}
