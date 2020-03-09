using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace WebHook.Utility
{
    public sealed class CacheHelper
    {
        // YZQ修改 读写缓存要用线程锁
        //private static ReaderWriterLock _CacheLock = new ReaderWriterLock();
        private static ReaderWriterLockSlim _CacheLock = new ReaderWriterLockSlim();
        private static Cache _cache;

        private CacheHelper() { }

        static CacheHelper()
        {
            // 如果当前上下文已经创建，则使用HttpContext.Cache,否则使用HttpRuntime.Cache
            // 两者其实同一个Cache
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        public static void Clear()
        {
            _CacheLock.EnterWriteLock();

            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList arr = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                arr.Add(CacheEnum.Key);
            }

            foreach (string key in arr)
            {
                _cache.Remove(key);
            }

            _CacheLock.ExitWriteLock();
        }

        public static void RemoveByPattern(string pattern)
        {
            _CacheLock.EnterWriteLock();

            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

            try
            {
                while (CacheEnum.MoveNext())
                {
                    if (regex.IsMatch(CacheEnum.Key.ToString()))
                    {
                        _cache.Remove(CacheEnum.Key.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                _CacheLock.ExitWriteLock();
            }
        }

        public static void Remove(string key)
        {
            _CacheLock.EnterWriteLock();

            try
            {
                _cache.Remove(key);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                _CacheLock.ExitWriteLock();
            }
        }

        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 10);
        }

        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, null, seconds);
        }

        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                _CacheLock.EnterWriteLock();

                try
                {
                    _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(seconds), TimeSpan.Zero, priority, null);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                finally
                {
                    _CacheLock.ExitWriteLock();
                }
            }

        }

        public static void Insert_sliding(string key, object obj)
        {
            Insert_sliding(key, obj, 10);
        }

        /// <summary>
        /// 插入缓存，使用相对过期时间
        /// </summary>
        /// <param name="key">缓存名称</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="ts">时间间隔</param>
        public static void Insert_sliding(string key, object obj, int seconds)
        {
            if (obj != null)
            {
                _CacheLock.EnterWriteLock();

                try
                {
                    _cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, seconds), CacheItemPriority.Normal, null);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                finally
                {
                    _CacheLock.ExitWriteLock();
                }
            }
        }

        public static void Max(string key, object obj)
        {
            Max(key, obj, null);
        }

        public static void Max(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                _CacheLock.EnterWriteLock();

                try
                {
                    _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                finally
                {
                    _CacheLock.ExitWriteLock();
                }
            }
        }

        public static object Get(string key)
        {
            _CacheLock.EnterReadLock();

            dynamic obj = null;
            try
            {
                obj = _cache[key];
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                _CacheLock.ExitReadLock();
            }

            return obj;
        }

        /// <summary>
        /// 取缓存 为null返回default(T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            _CacheLock.EnterReadLock();

            dynamic obj = default(T);
            try
            {
                object _tmp = _cache[key];
                if (null != _tmp && _tmp is T)
                    obj = (T)_tmp;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                _CacheLock.ExitReadLock();
            }

            return obj;
        }


        public static bool Exists(string key)
        {
            _CacheLock.EnterReadLock();
            bool Exists = false;
            try
            {
                object _tmp = _cache[key];
                if (_tmp != null)
                    Exists = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                _CacheLock.ExitReadLock();
            }

            return Exists;
        }

        public static List<DictionaryEntry> ForEach()
        {
            _CacheLock.EnterReadLock();

            List<DictionaryEntry> list = new List<DictionaryEntry>();

            try
            {
                if (_cache != null && _cache.Count > 0)
                {
                    foreach (DictionaryEntry item in _cache)
                        list.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                _CacheLock.ExitReadLock();
            }

            return list;
        }
    }
}
