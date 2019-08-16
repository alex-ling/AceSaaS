using System;

namespace Acesoft.IotNet
{
	public class AqiResult : WeaResult
	{
		public DateTime FetchTime { get; set; }
        public WeaAqi aqi { get; set; }
    }
}
