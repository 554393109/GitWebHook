using System.Collections;
using System.Collections.Generic;

namespace WebHook.Utility.Extension
{
    public static class DictionaryExtension
    {
        public static Hashtable ToHashtable(this IDictionary dic)
        {
            var hash = new Hashtable();

            if (dic != null && dic.Count > 0)
            {
                foreach (string key in dic.Keys)
                {
                    if (key != null)
                        hash.Add(key, dic[key]);
                }
            }

            return hash;
        }

        public static SortedDictionary<string, string> ToSortedDictionary(this Dictionary<string, object> dic)
        {
            var sdic = new SortedDictionary<string, string>();

            foreach (string iterm in dic.Keys)
            {
                sdic[iterm] = dic[iterm] != null ? dic[iterm].ToString() : string.Empty;
            }
            return sdic;
        }

    }
}
