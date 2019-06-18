using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

using Essensoft.AspNetCore.Payment.Alipay;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Acesoft.Config;

namespace Acesoft.Web.Pay
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // config
            services.AddJsonConfig(opts => 
            {
                opts.ConfigFile = "cloud.config.json";
                opts.IsTenantConfig = true;
            }, 
            (opts, config) =>
            {
                // 添加alipays
                foreach (var section in config.GetSection("alipays").GetChildren())
                {
                    if (section.Key == "alipay")
                    {
                        services.Configure<AlipayOptions>(section);
                    }
                    else
                    {
                        services.Configure<AlipayOptions>(section.Key, section);
                    }
                }
            });

            //引入HttpClient API证书的使用(仅QPay / WeChatPay的部分API使用到)
            //services.AddHttpClient("qpayCertificateName").ConfigurePrimaryHttpMessageHandler(() =>
            //{
            //    var certificate = new X509Certificate2("", "", X509KeyStorageFlags.MachineKeySet);
            //    var handler = new HttpClientHandler();
            //    handler.ClientCertificates.Add(certificate);
            //    return handler;
            //});

            //services.AddHttpClient("wechatpayCertificateName").ConfigurePrimaryHttpMessageHandler(() =>
            //{
            //    var certificate = new X509Certificate2("", "", X509KeyStorageFlags.MachineKeySet);
            //    var handler = new HttpClientHandler();
            //    handler.ClientCertificates.Add(certificate);
            //    return handler;
            //});

            // 引入Payment 依赖注入
            services.AddAlipay();
            /*services.AddJDPay();
            services.AddQPay();
            services.AddUnionPay();
            services.AddWeChatPay();
            services.AddLianLianPay();*/

            services.AddWebEncoders(opt =>
            {
                opt.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
        }
    }
}
