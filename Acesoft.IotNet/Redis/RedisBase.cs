using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesoft.IotNet.Redis
{
	public class RedisBase
	{
		private static ConnectionMultiplexer db = null;

		private static string key = string.Empty;

		private int DbIndex
		{
			get;
		}

		public RedisBase(int dbIndex)
		{
			DbIndex = dbIndex;
			db = RedisManager.Instance;
		}

		public string AddKey(string old)
		{
			return key + old;
		}

		public T DoAction<T>(Func<ConnectionMultiplexer, T> func)
		{
			return func(db);
		}

		public T DoSave<T>(Func<IDatabase, T> func)
		{
			return func(db.GetDatabase(DbIndex));
		}

		public string ConvertJson<T>(T value)
		{
			if (!(((object)value) is string))
			{
				return JsonConvert.SerializeObject(value);
			}
			return value.ToString();
		}

		public T ConvertObj<T>(RedisValue value)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		public List<T> ConvertList<T>(RedisValue[] values)
		{
			List<T> list = new List<T>();
			foreach (RedisValue value in values)
			{
				T item = ConvertObj<T>(value);
				list.Add(item);
			}
			return list;
		}

		public RedisKey[] ConvertRedisKeys(List<string> values)
		{
            return (from k in values select (RedisKey)k).ToArray();
		}
	}
}
