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
        public static bool KeyExists(string key)
        {
            var bResult = Cache.KeyExists(key);
            return bResult;
        }

        public static bool SetExpire(string key, DateTime datetime)
        {
            return Cache.KeyExpire(key, datetime);
        }

        public static bool SetExpire(string key, int timeout = 0)
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

        #region Hashes
        public static bool IsExist(string key, string field)
        {
            return Cache.HashExists(key, field);
        }

        public static bool SetHash<T>(string key, string field, T t)
        {
            var value = JsonConvert.SerializeObject(t);
            return Cache.HashSet(key, field, value);
        }

        public static bool Remove(string key, string field)
        {
            return Cache.HashDelete(key, field);
        }

        public static long StringIncrement(string key, string filed, long value = 1)
        {
            return Cache.HashIncrement(key, filed, value);
        }

        public static T Get<T>(string key, string filed)
        {
            string value = Cache.HashGet(key, filed);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static long GetHashCount(string key)
        {
            var length = Cache.HashLength(key);
            return length;
        }

        public static string Get(string key, string filed)
        {
            string value = Cache.HashGet(key, filed).ToString();
            return value;
        }

        public static List<T> GetAll<T>(string key)
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
        #endregion

        #region Sorted Sets
        public static bool SortedSetItemIsExist(string key, string item)
        {
            var value = GetItemScoreFromSortedSet(key, item);
            if (value != null)
            {
                return true;
            }
            return false;
        }

        public static bool SortedSetAdd(string key, string item, double score, int timeout = 0)
        {
            return Cache.SortedSetAdd(key, item, score);
        }

        public static List<string> GetSortedSetRangeByRank(string key, long fromRank, long toRank, string order = DefaultOrder)
        {
            var result = new List<string>();
            var list = Cache.SortedSetRangeByRank(key, fromRank, toRank, order == Order.Descending.ToString().ToLower() ? Order.Descending : Order.Ascending).ToList();
            if (list.Any())
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<string>(x);
                    result.Add(value);
                });
            }
            return result;
        }

        public static Dictionary<string, double> GetSortedSetRangeByRankWithScores(string key, long fromRank, long toRank, string order = DefaultOrder)
        {
            var result = new Dictionary<string, double>();
            var list = Cache.SortedSetRangeByRankWithScores(key, fromRank, toRank, order == Order.Descending.ToString().ToLower() ? Order.Descending : Order.Ascending).ToList();
            if (list.Any())
            {
                list.ForEach(x =>
                {
                    result.Add(x.Element, x.Score);
                });
            }
            return result;
        }

        public static List<string> GetSortedSetRangeByValue(string key, long minValue, long maxValue)
        {
            var result = new List<string>();
            var list = Cache.SortedSetRangeByValue(key, minValue, maxValue).ToList();
            if (list.Any())
            {
                list.ForEach(x =>
                {
                    var value = JsonConvert.DeserializeObject<string>(x);
                    result.Add(value);
                });
            }
            return result;
        }

        public static long GetSortedSetLength(string key)
        {
            return Cache.SortedSetLength(key);
        }

        public static long GetSortedSetLength(string key, double minValue, double maxValue)
        {
            return Cache.SortedSetLength(key, minValue, maxValue);
        }

        public static long? GetItemRankFromSortedSet(string key, string item, string order = DefaultOrder)
        {
            return Cache.SortedSetRank(key, item, order == Order.Descending.ToString().ToLower() ? Order.Descending : Order.Ascending);
        }

        public static double? GetItemScoreFromSortedSet(string key, string item)
        {
            return Cache.SortedSetScore(key, item);
        }

        public static double SetSortedSetItemIncrement(string key, string item, double score = 1)
        {
            return Cache.SortedSetIncrement(key, item, score);
        }

        public static double SortedSetItemDecrement(string key, string item, double score = -1)
        {
            return Cache.SortedSetDecrement(key, item, score);
        }

        public static bool RemoveItemFromSortedSet(string key, string item)
        {
            return Cache.SortedSetRemove(key, item);
        }

        public static long RemoveByRankFromSortedSet(string key, long fromRank, long toRank)
        {
            return Cache.SortedSetRemoveRangeByRank(key, fromRank, toRank);
        }

        public static long RemoveByScoreFromSortedSet(string key, double minValue, double maxValue)
        {
            return Cache.SortedSetRemoveRangeByScore(key, minValue, maxValue);
        }

        public static long RemoveByLexFromSortedSet(string key, int minValue, int maxValue)
        {
            //TODO： Don't know its meaning
            //return Cache.SortedSetRemoveRangeByValue(key, minValue, maxValue);
            return 0;
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

        #region Strings

        public static string Get(string key)
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

        public static bool Set<T>(string key, T t, int timeout = 0)
        {
            var value = JsonConvert.SerializeObject(t);
            bool bResult = Cache.StringSet(key, value);
            if (timeout > 0)
            {
                Cache.KeyExpire(key, DateTime.Now.AddSeconds(timeout));
            }
            return bResult;
        }

        #endregion
    }

}
