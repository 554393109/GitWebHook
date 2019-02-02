using System.Text;

namespace YZQ.Utility.HttpClientUtils
{
    public interface IClientStrategy
        : YZQ.Utility.HttpClientUtils.IClient
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
