using System;

namespace Acesoft.Web.Cloud
{
	public interface IWeatherService
	{
		WeaResult GetWea(string lng, string lat);
		WeaResult GetWea(string cityId);
		AqiResult GetWeaAqi(string lng, string lat);
		AqiResult GetWeaAqi(string cityId);
		AqiResult GetCachedWeaAqi(string cityId);
	}
}
