using System;
using System.Collections.Generic;

using Acesoft.Config;
using Acesoft.Web.Cloud.Aliyun;
using Acesoft.Web.Cloud.Mail;
using Acesoft.Web.Cloud.Moji;
using Acesoft.Web.Cloud.Tencent;

namespace Acesoft.Web.Cloud
{
	public class CloudService : ICloudService
	{
        private CloudConfig cloudConfig;

        public CloudService()
        {
            cloudConfig = ConfigContext.GetConfig<CloudConfig>("", (config, key) =>
            {
                cloudConfig = config;
            });
        }

        public IOssService GetOssService(string name = "aliyunoss")
        {
            var access = cloudConfig.Accesses.GetValue(name);
            return new AliyunOss(access);
        }

        public ISmsService GetSmsService(string name = "aliyunsms")
        {
            var access = cloudConfig.Accesses.GetValue(name);
            return new AliyunSms(access);
        }

        public IWeatherService GetWeatherService(string name = "mojiweather")
        {
            var settings = cloudConfig.Settings.GetValue(name);
            var appCode = settings.GetValue("appcode");
            var refreshMinutes = settings.GetValue("refreshMinutes", 120);
            return new WeatherService(appCode, refreshMinutes);
        }

        public ILocationService GetLocationService()
        {
            return new LocationService();
        }

        public IMailService GetMailService(string name = "mail163")
        {
            var mailConfig = cloudConfig.MailConfigs.GetValue(name);
            return new MailService(mailConfig);
        }
    }
}
