using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using WebHook.Utility.Extension;

namespace WebHook.Utility
{
    public class AppConfig
    {
        private const string ConfigKeyPrefix = "AppConfig-{0}";
        private const int ConfigTimeoutSec = 600;

        /// <summary>
        /// 配置文件是否存在
        /// </summary>
        private static bool IsExistFile
        {
            get {
                return File.Exists(ApplicationInfo.ConfigPath);
            }
        }

        private AppConfig() { }

        public static bool Create(IDictionary hash)
        {
            var succeed = false;
            hash = hash ?? new Hashtable();

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<settings>");

            try
            {
                foreach (DictionaryEntry keyValue in hash)
                {
                    if (keyValue.Key.IsNullOrWhiteSpace())
                        continue;

                    sb.AppendLine(string.Format("<add key=\"{0}\" value=\"{1}\" /> ", keyValue.Key.ToString(), keyValue.Value.ValueOrEmpty()));
                }

                sb.AppendLine("</settings>");

                using (var stream = new StreamWriter(path: ApplicationInfo.ConfigPath, append: false, encoding: Encoding.UTF8))
                {
                    stream.Write(sb.ToString());
                }

                succeed = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                succeed = false;
            }

            return succeed;
        }

        public static bool Save(IDictionary hash)
        {
            bool succeed = false;
            if (!IsExistFile)
            {
                succeed = Create(hash);
            }
            else
            {
                try
                {
                    XmlDocument xdoc = XmlDoc;
                    foreach (DictionaryEntry item in hash)
                    {
                        XmlNode node = xdoc.SelectSingleNode(string.Format("//settings/add[@key='{0}']", item.Key));
                        if (node == null)
                        {
                            XmlNode xnode = xdoc.SelectSingleNode("/settings");
                            XmlElement xelem = xdoc.CreateElement("add");

                            XmlAttribute xattr = xdoc.CreateAttribute("key");
                            xattr.Value = item.Key.ToString();
                            xelem.Attributes.SetNamedItem(xattr);
                            node = xnode.AppendChild(xelem);
                            xattr = xdoc.CreateAttribute("value");
                            xattr.Value = item.Value.ValueOrEmpty();
                            xelem.Attributes.SetNamedItem(xattr);
                            node = xnode.AppendChild(xelem);
                        }
                        else
                        {
                            node.Attributes["value"].Value = item.Value.ValueOrEmpty();
                        }
                    }
                    xdoc.Save(ApplicationInfo.ConfigPath);
                    succeed = true;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    succeed = false;
                }
            }

            return succeed;
        }

        public static bool SetValue(string key, string newValue)
        {
            if (!IsExistFile)
            {
                return false;
            }
            bool succeed = false;
            try
            {
                XmlDocument xdoc = XmlDoc;
                XmlNode node = xdoc.SelectSingleNode(String.Format("//settings/add[@key='{0}']", key));
                if (node == null)
                {
                    XmlNode root = xdoc.SelectSingleNode("/settings");
                    XmlElement elem = xdoc.CreateElement("add");

                    XmlAttribute attr = xdoc.CreateAttribute("key");
                    attr.Value = key;
                    elem.Attributes.SetNamedItem(attr);
                    node = root.AppendChild(elem);
                    attr = xdoc.CreateAttribute("value");
                    attr.Value = newValue;
                    elem.Attributes.SetNamedItem(attr);
                    node = root.AppendChild(elem);
                }
                else
                {
                    node.Attributes["value"].Value = newValue;
                }
                xdoc.Save(ApplicationInfo.ConfigPath);
                succeed = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                succeed = false;
            }

            return succeed;
        }

        public static string GetValue(string key)
        {
            if (!IsExistFile)
            {
                //return null;
                return string.Empty;
            }

            XmlNode node = GetNode(key);
            if (node == null)
                return string.Empty;

            return node.Attributes["value"].Value;
        }

        /// <summary>
        /// 写入缓存
        /// 缓存时长：10分钟
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue_Cache(string key)
        {
            var value = GetValue(key);

            if (!string.IsNullOrWhiteSpace(value))
                CacheHelper.Insert(string.Format(ConfigKeyPrefix, key), value, ConfigTimeoutSec);

            return value;
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static XmlNode GetNode(string key)
        {
            if (!IsExistFile)
            {
                return null;
            }
            return XmlDoc.SelectSingleNode(string.Format("//settings/add[@key='{0}']", key));
        }

        public static bool ExistNode(string key)
        {
            if (!IsExistFile)
            {
                return false;
            }
            XmlNode node = XmlDoc.SelectSingleNode(String.Format("//settings/add[@key='{0}']", key));
            return node == null ? false : true;
        }

        /// <summary>
        /// 全部
        /// </summary>
        public static SortedDictionary<string, string> KeyValues
        {
            get {
                if (!IsExistFile)
                    return null;

                var sdic = new SortedDictionary<string, string>();
                try
                {
                    XmlNodeList nodeList = XmlDoc.GetElementsByTagName("add");
                    foreach (XmlNode node in nodeList)
                    {
                        sdic.Add(node.Attributes["key"].Value, node.Attributes["value"].Value.ValueOrEmpty());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }

                return sdic;
            }
        }

        public static XmlDocument XmlDoc
        {
            get {
                if (!IsExistFile)
                    return null;

                XmlDocument xdoc = new XmlDocument() { XmlResolver = null };
                xdoc.Load(ApplicationInfo.ConfigPath);
                return xdoc;
            }
        }
    }
}
