using System.Collections.Concurrent;

namespace CacheDemoLib
{
    /// <summary>
    /// Concurrent cache pomocí ConcurrentDictionary, místo hodnoty očekává funkci, která vrací hodnotu.
    /// </summary>
    public class ConcurrentCache
    {
        private TimeSpan _expirationTime;

        /// <summary>
        /// Konstruktor s nastavením expirace cache.
        /// </summary>
        /// <param name="expiration">čas v sekundách</param>
        public ConcurrentCache(int expiration)
        {
            _expirationTime = TimeSpan.FromSeconds(expiration);
        }

        private ConcurrentDictionary<string,CacheItem> _cache = new();

        /// <summary>
        /// Přidání hodnoty do cache pomocí funkce s parametrem.
        /// </summary>
        /// <param name="key">klíč</param>
        /// <param name="value">funkce s parametrem</param>
        /// <param name="param">parametr typu object</param>
        public void Add(string key, Func<object?,object> value, object? param = null)
        {
            object newValue = value(param);
            CacheItem item = new() { Value = newValue, Expiration = DateTime.Now.Add(_expirationTime), FuncParam = value, Param = param };
            _cache.AddOrUpdate(key, item, (s,x) => item);
        }

        /// <summary>
        /// Přidání hodnoty do cache pomocí funkce bez parametru.
        /// </summary>
        /// <param name="key">klíč</param>
        /// <param name="value">funkce bez parametru</param>
        public void Add(string key, Func<object> value)
        {
            object newValue = value();
            CacheItem item = new() { Value = newValue, Expiration = DateTime.Now.Add(_expirationTime), Func = value };
            _cache.AddOrUpdate(key, item, (s, x) => item);
        }

        /// <summary>
        /// Vrácení hodnoty z cache, pokud je v cache už neplatná, tak ji zkusí získat pomocí funkce.
        /// </summary>
        /// <param name="key">požadovaný klíč</param>
        /// <returns>hodnota z cache nebo nově získaná</returns>
        public object? Get(string key)
        {
            CacheItem? item = _cache.TryGetValue(key, out CacheItem? value) ? value : null;

            if (item == null)
            {
                return null;
            }

            if (item.Expiration < DateTime.Now)
            {
                if (item.FuncParam != null)
                {
                    object newValue = item.FuncParam(item.Param);
                    item = new CacheItem { Value = newValue, Expiration = DateTime.Now.Add(_expirationTime), FuncParam = item.FuncParam };
                    _cache.AddOrUpdate(key, item, (s, x) => item);
                }
                else if (item.Func != null)
                {
                    object newValue = item.Func();
                    item = new CacheItem { Value = newValue, Expiration = DateTime.Now.Add(_expirationTime), Func = item.Func };
                    _cache.AddOrUpdate(key, item, (s, x) => item);
                }
            }

            return item.Value; 
        }

        private record class CacheItem
        {
            public object Value { get; init; }
            public DateTime Expiration { get; init; }

            public Func<object?,object>? FuncParam { get; init; }

            public object? Param { get; init; }

            public Func<object>? Func { get; init; }
        }
    }
}
