using System;
using System.Text;

namespace WebHook.Utility.DEncrypt
{
    /// <summary>
    /// 来自支付宝SDK的AES加解密
    /// </summary>
    public class AlipayEncrypt
    {

        /// <summary>
        /// 128位0向量
        /// </summary>
        private static byte[] AES_IV = initIv(16);

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="encryptKey"></param>
        /// <param name="bizContent"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string AesEncrypt(string encryptKey, string bizContent, string charset = null)
        {
            if (string.IsNullOrWhiteSpace(encryptKey))
                encryptKey = SystemKey.AesKey;

            byte[] keyArray = Convert.FromBase64String(encryptKey);
            byte[] toEncryptArray = null;
            string result = null;

            if (string.IsNullOrWhiteSpace(charset))
                toEncryptArray = Encoding.UTF8.GetBytes(bizContent);
            else
                toEncryptArray = Encoding.GetEncoding(charset).GetBytes(bizContent);

            try
            {
                using (System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged())
                {
                    rDel.Key = keyArray;
                    rDel.Mode = System.Security.Cryptography.CipherMode.CBC;
                    rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                    rDel.IV = AES_IV;

                    System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateEncryptor(rDel.Key, rDel.IV);
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    result = Convert.ToBase64String(resultArray);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptKey"></param>
        /// <param name="bizContent"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string AesDencrypt(string encryptKey, string bizContent, string charset = null)
        {
            if (string.IsNullOrWhiteSpace(encryptKey))
                encryptKey = SystemKey.AesKey;

            var arr_key = Convert.FromBase64String(encryptKey);
            var arr_content = Convert.FromBase64String(bizContent);
            var reslut = default(string);

            try
            {
                using (System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged())
                {
                    rDel.Key = arr_key;
                    rDel.Mode = System.Security.Cryptography.CipherMode.CBC;
                    rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                    rDel.IV = AES_IV;

                    System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateDecryptor(rDel.Key, rDel.IV);
                    var arr_result = cTransform.TransformFinalBlock(arr_content, 0, arr_content.Length);

                    if (string.IsNullOrWhiteSpace(charset))
                        reslut = Encoding.UTF8.GetString(arr_result);
                    else
                        reslut = Encoding.GetEncoding(charset).GetString(arr_result);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return reslut;
        }

        /// <summary>
        /// 初始化向量
        /// </summary>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        private static byte[] initIv(int blockSize)
        {
            byte[] iv = new byte[blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                iv[i] = (byte)0x0;
            }
            return iv;
        }
    }
}
