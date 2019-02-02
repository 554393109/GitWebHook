using System;
using System.Text;

namespace YZQ.Utility.Extension
{
    public static class ByteExtension
    {
        public static string ByteToHexStr(this byte[] source, string format = "X2")
        {
            var returnStr = new StringBuilder();
            if (source != null)
            {
                if (!"X2".Equals(format, StringComparison.OrdinalIgnoreCase))
                    format = "X2";

                for (int i = 0; i < source.Length; i++)
                {
                    //returnStr.Append(source[i].ToString("X2"));            //大写
                    //returnStr.Append(source[i].ToString("x2"));              //小写
                    returnStr.Append(source[i].ToString(format));              //小写
                }
            }
            return returnStr.ToString();
        }
    }
}
