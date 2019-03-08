using System;

using System;

namespace Acesoft.Web.Cloud
{
	public class AqiResult : WeaResult
	{
		public DateTime FetchTime { get; set; }
        public WeaAqi aqi { get; set; }
    }
}
