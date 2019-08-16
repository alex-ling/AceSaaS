using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acesoft.IotNet.Redis
{
	public class RedisStringCache : IRedisCaching
	{
		private RedisBase redis;

		public RedisStringCache(int dbIndex)
		{
			redis = new RedisBase(dbIndex);
		}

		public bool StringSet(string key, string val, TimeSpan? exp = default(TimeSpan?))
		{
			key = redis.AddKey(key);
			return redis.DoSave((IDatabase db) => db.StringSet(key, val, exp));
		}

		public bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> KeyVal)
		{
			List<KeyValuePair<RedisKey, RedisValue>> newkey = (from k in KeyVal
			select new KeyValuePair<RedisKey, RedisValue>(redis.AddKey(k.Key), k.Value)).ToList();
			return redis.DoSave((IDatabase db) => db.StringSet(newkey.ToArray()));
		}

		public bool StringSet<T>(string key, T obj, TimeSpan? exp = default(TimeSpan?))
		{
			key = redis.AddKey(key);
			string json = redis.ConvertJson(obj);
			return redis.DoSave((IDatabase db) => db.StringSet(key, json, exp));
		}

		public string StringGet(string key)
		{
			key = redis.AddKey(key);
			return redis.DoSave((IDatabase db) => db.StringGet(key));
		}

		public T StringGet<T>(string key)
		{
			key = redis.AddKey(key);
			RedisValue value = redis.DoSave((IDatabase db) => db.StringGet(key));
			return redis.ConvertObj<T>(value);
		}

		public double StringIncrement(string key, double val = 1.0)
		{
			key = redis.AddKey(key);
			return redis.DoSave((IDatabase db) => db.StringIncrement(key, val));
		}

		public double StringDecrement(string key, double val = 1.0)
		{
			key = redis.AddKey(key);
			return redis.DoSave((IDatabase db) => db.StringDecrement(key, val));
		}

		public async Task<bool> StringSetAsync(string key, string val, TimeSpan? exp = default(TimeSpan?))
		{
			key = redis.AddKey(key);
			return await redis.DoSave((IDatabase db) => db.StringSetAsync(key, val, exp));
		}

		public async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> KeyVal)
		{
			List<KeyValuePair<RedisKey, RedisValue>> newkey = (from k in KeyVal
			select new KeyValuePair<RedisKey, RedisValue>(redis.AddKey(k.Key), k.Value)).ToList();
			return await redis.DoSave((IDatabase db) => db.StringSetAsync(newkey.ToArray()));
		}

		public async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? exp = default(TimeSpan?))
		{
			key = redis.AddKey(key);
			string json = redis.ConvertJson(obj);
			return await redis.DoSave((IDatabase db) => db.StringSetAsync(key, json, exp));
		}

		public async Task<string> StringGetAsync(string key)
		{
			key = redis.AddKey(key);
			return await redis.DoSave((IDatabase db) => db.StringGetAsync(key));
		}

		public async Task<T> StringGetAsync<T>(string key)
		{
			key = redis.AddKey(key);
			RedisValue value = await redis.DoSave((IDatabase db) => db.StringGetAsync(key));
			return redis.ConvertObj<T>(value);
		}

		public async Task<double> StringIncrementAsync(string key, double val = 1.0)
		{
			key = redis.AddKey(key);
			return await redis.DoSave((IDatabase db) => db.StringIncrementAsync(key, val));
		}

		public async Task<double> StringDecrementAsync(string key, double val = 1.0)
		{
			key = redis.AddKey(key);
			return await redis.DoSave((IDatabase db) => db.StringDecrementAsync(key, val));
		}
	}
}
