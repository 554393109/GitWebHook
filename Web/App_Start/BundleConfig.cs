using System.Web.Optimization;
using YZQ.Utility;

namespace Web
{
    public class BundleConfig
    {
        /// <summary>
        /// 打包合并规则
        /// CDN不合并，每项独立
        /// 本地资源合并
        /// </summary>
        /// <param name="bundles"></param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            BundleTable.EnableOptimizations = true;
        }
    }
}