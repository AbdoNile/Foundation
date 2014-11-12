using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;

namespace Foundation.Web.Caching
{
    public class InMemoryCache : ICacheService
    {
        public T Get<T>(string cacheId, CacheType type) where T : class, IFlushable
        {
            var item = HttpRuntime.Cache.Get(JoinKey(cacheId, type)) as T;

            return item;
        }

        public T Get<T>(string cacheId, CacheType type, Func<T> getItemCallback) where T : class, IFlushable
        {
            int seconds = Int32.Parse(ConfigurationManager.AppSettings["Foundation_CacheExpirationInSeconds"]);
            return Get(cacheId, type, getItemCallback, seconds);
        }

        public T Get<T>(string cacheId, CacheType type, Func<T> getItemCallback, CacheItemPriority priority)
            where T : class, IFlushable
        {
            int seconds = Int32.Parse(ConfigurationManager.AppSettings["Foundation_CacheExpirationInSeconds"]);
            return Get(cacheId, type, getItemCallback, seconds, priority);
        }

        public T Get<T>(string cacheId, CacheType type, Func<T> getItemCallback, int secondsToCache,
            CacheItemPriority priority = CacheItemPriority.Normal) where T : class, IFlushable
        {
            var item = HttpRuntime.Cache.Get(JoinKey(cacheId, type)) as T;

            if (item == null)
            {
                item = getItemCallback();
                item.ForceFlush = true;
                HttpRuntime.Cache.Insert(
                    JoinKey(cacheId, type),
                    item,
                    null,
                    Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 0, secondsToCache),
                    priority,
                    null);
            }
            else
            {
                item.ForceFlush = false;
                HttpRuntime.Cache.Insert(
                    JoinKey(cacheId, type),
                    item,
                    null,
                    Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 0, secondsToCache),
                    priority,
                    null);
            }

            return item;
        }

        public void Delete(string cacheId, CacheType type)
        {
            HttpRuntime.Cache.Remove(JoinKey(cacheId, type));
        }

        private string JoinKey(string cacheId, CacheType cacheType)
        {
            string cacheKeyPrefix;
            switch (cacheType)
            {
                case CacheType.User:
                    cacheKeyPrefix = HttpContext.Current.Session.SessionID;
                    break;
                case CacheType.Application:
                    cacheKeyPrefix = cacheType.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("cacheType");
            }
            return cacheKeyPrefix + "." + cacheId;
        }
    }
}