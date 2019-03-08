using System;
using System.Text;

using Newtonsoft.Json.Linq;
using Acesoft.Util;

namespace Acesoft.Web.Cloud.Tencent
{
	public class LocationService : ILocationService
	{
		private const string Api = "http://apis.map.qq.com/jsapi?qt=rgeoc&lnglat={0},{1}&output=json&pf=jsapi&ref=jsapi";

		public Location GetLoc(string longitude, string latitude)
		{
            var res = HttpHelper.HttpGet($"http://apis.map.qq.com/jsapi?qt=rgeoc&lnglat={longitude},{latitude}&output=json&pf=jsapi&ref=jsapi", null, 0, Encoding.GetEncoding("gb18030"));
            var json = SerializeHelper.FromJson(res);

			return new Location
			{
				Longitude = longitude,
				Latitude = latitude,
				Province = json["detail"]["poilist"][0]["addr_info"]["p"].Value<string>(),
				City = json["detail"]["poilist"][0]["addr_info"]["c"].Value<string>(),
				District = json["detail"]["poilist"][0]["addr_info"]["d"].Value<string>(),
				Address = json["detail"]["poilist"][0]["addr"].Value<string>(),
				AdCode = json["detail"]["poilist"][0]["addr_info"]["adcode"].Value<string>()
			};
		}
	}
}
