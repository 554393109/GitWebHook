using System.Web;

namespace WebHook.Utility
{
    public class ApplicationInfo
    {
        private static readonly object _lock = new object();
        static ApplicationInfo()
        {
            if (PathRoot == null)
            {
                lock (_lock)
                {
                    if (PathRoot == null)
                    {
                        PathRoot = HttpContext.Current.Server.MapPath("~");
                    }
                }
            }
        }

        public static string PathRoot { get; private set; }


        public static string TempPath
        {
            get {
                return PathRoot + "UpLoad\\Temp";
            }
        }

        public static string TempPath_virtual
        {
            get {
                return "~\\UpLoad\\Temp\\";
            }
        }

        public static string ConfigPath
        {
            get {
                return PathRoot + "\\Conf.xml";
            }
        }
    }
}
