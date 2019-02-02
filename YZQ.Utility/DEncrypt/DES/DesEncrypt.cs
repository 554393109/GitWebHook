//888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888
//888888888888888888888888888888888888!ooooo*88888888888888888888888888888888888888888888888888888
//8888888888888888888888888888888888o!!!;;;!!o!888888888888888888888888888888888888888888888888888
//888888888888888888888888888888888!!!;;;;;;;!!!$8888888888888888888888888888888888888888888888888
//888888888888888888888888888888888!!;;;;;;;;;;!!88888888888888888888888888$!!!$888888888888888888
//888888888888888888888888888888888o!!;;;;;;;;;!oo888888888$!!oo!!!oo!$&!o!o!!!oo!!!88888888888888
//888888888888888888888o!!!!$888888&!o;;;;;;;;;;o!&o!!oo!!!o!!!!!!;;!!o!o;;;;;;;;;;!o!888888888888
//888888888888888888&!!!!;!!!o!o8888o!!;;;;!;!!!ooo!!;!;;;;;;;;;;;;;;;!!;;;;;;;;;;;;;!!!8888888888
//888888888888888888oo;;;;;;;;;o!o888o!o;!oo!o!!;;;;;;;;;;;;;;;;;;;;;;!!;;;;;;;;;!!o!;!oo888888888
//88888888888888888$!!;;;;;;;;;;;!o!88!o!!!;;;;;;;;;;;;ooo!!!oo;;;;;;;oo!;;;;!;;!;oo;;;!o!88888888
//888888888888888888o!;;;;;;;;;;;;!!ooo;;;;;;;;;;;;;;;!o;.....o!!;;;;;;o!;;;o!!!;;;;;;;;oo&8888888
//888888888888888888&oo;;;;;;;;;!o!!;;;;;;;;;!ooo!;;;!o!8888..;!o;;;;;;;o!!;;;!;;;;;;;;;!!o8888888
//88888888888888888888o!!;;;;;!oo!;;;;;;;;;o!o;;!o!o;!o!!88o..!o!;;;;;;;;!!!;;;;;;;;;;;;!!*8888888
//888888888888888888888$!!o;o!!!;;;;;;;;;!!o;.;...;oo;;!oooo!o!;;;;;;;;;;;!!o!;;;;;;;;;!!o88888888
//888888888888888888888888o!!!;;;;;;;;;;;!!!8888...!!!;;;;;;!;;;;;;;;;;;;;;;!ooo!;;;;;!oo888888888
//88888888888888888888888o!!;;;;;;;;;;;;;;!o&88...;oo;;;;;;;;;;;;;;;;;;;;;;;;;;!ooooooo&8888888888
//888888888888888888888*!!;;;;;;;;;;;;;;;;;!o!ooo!o!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;ooo&888888888888
//88888888888888888888!oo;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!ooo888888888888888
//8888888888888888888!!!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;oo!*88888888888888888
//888888888888888888!!!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!o!o!&8888888888888888888
//88888888888888888$oo;;;;;;;;;;;o!o!!!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;oo!o88888888888888888888888
//88888888888888888!!!;;;;;;;;o!o!ooooooo;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!!!88888888888888888888888
//8888888888888888ooo;;;;;;;!!!!!ooooooooo!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!!o8888888888888888888888
//8888888888888888o!!;;;;;;;!!!!ooooooooooo;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!oo888888888888888888888
//8888888888888888!o;;;;;;;;!!ooooooooooooo;;;;;;;;;;;;;;;;;;;;;;;o$$;;;;;;!o888888888888888888888
//8888888888888888!!;;;;;;;;;!oooooooooooo!;;;;;;;;;;;;;;;;;;;;;;;o$$;;;;;;!!888888888888888888888
//8888888888888888o!;;;;;;;;;;!o!oooooooo;;;;;;;;;;;;;;;;;;;;;;;;!$$*;;;;;;!!888888888888888888888
//8888888888888888!o!;;;;;;;;;;;;!!oo!;;;;;;;o$o!;;;;;;;;;;;;;;;;$$$;;;;;;;!!888888888888888888888
//8888888888888888&!o;;;;;;;;;;;;;;;;;;;;;;;;;$$$$!;;;;;;;;;!;;$$$o;;;;;;;;!!888888888888888888888
//88888888888888888!!!;;;;;;;;;;;;;;;;;;;;;;;;;;!$$$$$$$$$$$$$$$!;;;;;;;;;!!$888888888888888888888
//888888888888888888o!!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!!oo!!!;;;;;;;;;;;;;!!!8888888888888888888888
//8888888888888888888!!!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!oo88888888888888888888888
//88888888888888888888!!o;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!!o888888888888888888888888
//888888888888888888888&o!!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!!o88888888888888888888888888
//88888888888888888888888&!!!!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;!!o!8888888888888888888888888888
//8888888888888888888888888;!ooo!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;ooooooo!888888888888888888888888888
//888888888888888888888888ooo**o!oo!o!;;;;;;;;;;;;;;;;;;;!!!!o!o******!!!8888888888888888888888888
//88888888888888888888888!o*********o!!oooo!oo!!!!!!ooo!!oo************o!!888888888888888888888888
//8888888888888888888888!!**********************oo***********************!!88888888888888888888888
//88888888888888888888o!!*************************************************o!8888888888888888888888
//888888888888888888!!oo***************************************************!o!88888888888888888888

/************************************************************************
 * 文件标识：  4f43a3bf-0326-4f5d-97c1-f4e0afeee6be
 * 项目名称：  YZQ.Utility.DEncrypt.DES  
 * 项目描述：  
 * 类 名 称：  DesEncrypt
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/9/20 17:19:44
 * 更新时间：  2018/9/20 17:19:44
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace YZQ.Utility.DEncrypt
{
    public class DesEncrypt
    {
        /// <summary> 
        /// 加密
        /// </summary> 
        /// <param name="contentSource"></param> 
        /// <param name="encryptKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string contentSource, string encryptKey)
        {
            using (var provider = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(contentSource);
                provider.Key = Encoding.UTF8.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptKey, "md5").Substring(0, 8));
                provider.IV = Encoding.UTF8.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptKey, "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                var ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                    ret.AppendFormat("{0:X2}", b);

                return ret.ToString();
            }
        }

        /// <summary> 
        /// 解密
        /// </summary> 
        /// <param name="contentEncrypt"></param> 
        /// <param name="encryptKey"></param> 
        /// <returns></returns>
        public static string Decrypt(string contentEncrypt, string encryptKey)
        {
            using (var provider = new DESCryptoServiceProvider())
            {
                int len;
                len = contentEncrypt.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(contentEncrypt.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                provider.Key = Encoding.UTF8.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptKey, "md5").Substring(0, 8));
                provider.IV = Encoding.UTF8.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptKey, "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
