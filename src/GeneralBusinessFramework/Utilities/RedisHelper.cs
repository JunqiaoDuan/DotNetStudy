using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Utilities
{
    public static partial class RedisHelper
    {
        private static readonly string ConnectString = SettingsHelper.GetStringValue("RedisConnString");
        private static Lazy<ConnectionMultiplexer> _lazyConnection;
        private static readonly Object MultiplexerLock = new Object();
        private static readonly IDatabase Cache;

        static RedisHelper()
        {
            var conn = CreateManager.Value;
            Cache = conn.GetDatabase(); //获取实例
        }

        // Singleton
        private static Lazy<ConnectionMultiplexer> CreateManager
        {
            get
            {
                if (_lazyConnection == null)
                {
                    lock (MultiplexerLock)
                    {
                        if (_lazyConnection != null) return _lazyConnection;

                        _lazyConnection = GetManager();
                        return _lazyConnection;
                    }
                }

                return _lazyConnection;
            }
        }

        private static Lazy<ConnectionMultiplexer> GetManager(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = GetDefaultConnectionString();
            }

            return new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
        }

        public static string GetDefaultConnectionString()
        {
            return ConnectString;
        }
    }

    public partial class RedisHelper
    {
        public const string DefaultOrder = "desc";

        #region Keys
        public static bool IsKeyExist(string key)
        {
            var bResult = Cache.KeyExists(key);
            return bResult;
        }

        public static bool SetKeyExpire(string key, DateTime datetime)
        {
            return Cache.KeyExpire(key, datetime);
        }

        public static bool SetKeyExpire(string key, int timeout = 0)
        {
            return Cache.KeyExpire(key, DateTime.Now.AddSeconds(timeout));
        }

        public static bool KeyDelete(string key)
        {
            return Cache.KeyDelete(key);
        }

        public static bool KeyRename(string oldKey, string newKey)
        {
            return Cache.KeyRename(oldKey, newKey);
        }

        #endregion

        #region Strings

        public static bool StringSet(string key, string value, int timeout = 0)
        {
            bool bResult = Cache.StringSet(key, value);
            if (timeout > 0)
            {
                Cache.KeyExpire(key, DateTime.Now.AddSeconds(timeout));
            }
            return bResult;
        }

        public static bool StringSet<T>(string key, T t, int timeout = 0)
        {
            var value = JsonConvert.SerializeObject(t);
            bool bResult = Cache.StringSet(key, value);
            if (timeout > 0)
            {
                Cache.KeyExpire(key, DateTime.Now.AddSeconds(timeout));
            }
            return bResult;
        }

        public static string StringGet(string key)
        {
            string value = Cache.StringGet(key);
            return value;
        }

        public static T StringGet<T>(string key)
        {
            string value = Cache.StringGet(key);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static double StringIncrement(string key, double value)
        {
            return Cache.StringIncrement(key, value);
        }

        public static long StringAppend(string key, string value)
        {
            return Cache.StringAppend(key, value, CommandFlags.None);
        }

        #endregion

        #region Hashes
        public static bool IsHashExist(string key, string field)
        {
            return Cache.HashExists(key, field);
        }

        public static bool HashSet(string key, string field, string value)
        {
            return Cache.HashSet(key, field, value);
        }

        public static string HashGet(string key, string filed)
        {
            string value = Cache.HashGet(key, filed).ToString();
            return value;
        }

        public static List<T> HashGetAll<T>(string key)
        {
            var result = new List<T>();
            var list = Cache.HashGetAll(key).ToList();
            if (list.Count > 0)
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<T>(x.Value);
                    result.Add(value);
                });
            }
            return result;
        }

        public static List<string> GetAllFields(string key)
        {
            var result = new List<string>();
            var list = Cache.HashKeys(key).ToList();
            if (list.Count > 0)
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<string>(x);
                    result.Add(value);
                });
            }
            return result;
        }

        public static bool RemoveHash(string key, string field)
        {
            return Cache.HashDelete(key, field);
        }

        public static long HashIncrement(string key, string filed, long value = 1)
        {
            return Cache.HashIncrement(key, filed, value);
        }

        public static long GetHashLength(string key)
        {
            var length = Cache.HashLength(key);
            return length;
        }

        #endregion

        #region Lists

        public static long AddList<T>(string key, T t)
        {
            var value = JsonConvert.SerializeObject(t);
            return Cache.ListLeftPush(key, value);
        }

        public static List<T> GetList<T>(string key, long start = 0, long stop = -1)
        {
            var result = new List<T>();
            var list = Cache.ListRange(key, start, stop).ToList();
            if (list.Count > 0)
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<T>(x);
                    result.Add(value);
                });
            }
            return result;
        }
        #endregion

    }

}
