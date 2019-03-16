using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Cloud
{
    public interface ICloudService
    {
        IOssService GetOssService(string name = "aliyunoss");
        ISmsService GetSmsService(string name = "aliyunsms");
        IWeatherService GetWeatherService(string name = "mojiweather");
        ILocationService GetLocationService();
        IMailService GetMailService(string name = "mail163");
    }
}
