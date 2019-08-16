using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Acesoft.IotNet.Redis
{
	public class RedisManager
	{
		private static ConnectionMultiplexer instance;

		public static ConnectionMultiplexer Instance => instance;

		static RedisManager()
		{
			instance = GetManager();
		}

		private static ConnectionMultiplexer GetManager()
		{
			NameValueCollection appSettings = ConfigurationManager.AppSettings;
			ConfigurationOptions configurationOptions = ConfigurationOptions.Parse(appSettings.GetValue("connectstring", "127.0.0.1:6379"));
			configurationOptions.Password = appSettings.GetValue("password", "");
			ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
			connectionMultiplexer.ConnectionFailed += MuxerConnectionFailed;
			connectionMultiplexer.ConnectionRestored += MuxerConnectionRestored;
			connectionMultiplexer.ErrorMessage += MuxerErrorMessage;
			connectionMultiplexer.ConfigurationChanged += MuxerConfigurationChanged;
			connectionMultiplexer.HashSlotMoved += MuxerHashSlotMoved;
			connectionMultiplexer.InternalError += MuxerInternalError;
			return connectionMultiplexer;
		}

		private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
		{
		}

		private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
		{
		}

		private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
		{
		}

		private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
		{
		}

		private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
		{
		}

		private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
		{
		}

		public static long Publish<T>(T value)
		{
			ISubscriber subscriber = Instance.GetSubscriber();
			string value2 = JsonConvert.SerializeObject(value);
			return subscriber.Publish("IOT", value2);
		}

		public static void Subscribe<T>(Action<T> action)
		{
			Instance.GetSubscriber().Subscribe("IOT", delegate(RedisChannel chanel, RedisValue json)
			{
				action(JsonConvert.DeserializeObject<T>(json));
			});
		}
	}
}
