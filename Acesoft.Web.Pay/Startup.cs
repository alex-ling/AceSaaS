using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.UnionPay;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Acesoft.Config;
using Acesoft.Web.Pay.Services;
using Acesoft.Web.Multitenancy;

namespace Acesoft.Web.Pay
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<IAlipayService, AlipayService>();

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
            services.AddWeChatPay();
            services.AddUnionPay();
            /*services.AddJDPay();
            services.AddQPay();
            services.AddLianLianPay();*/

            // config
            services.AddJsonConfig(opts =>
            {
                opts.ConfigFile = "pay.config.json";
                opts.IsTenantConfig = true;
            },
            (opts, config) =>
            {
                // 添加alipays
                var tenant = (Tenant as Tenant).Name;
                foreach (var section in config.GetSection(tenant).GetChildren())
                {
                    if (section.Key == "alipay")
                    {
                        services.Configure<AlipayOptions>(section);
                    }
                    else if (section.Key.StartsWith("alipay"))
                    {
                        services.Configure<AlipayOptions>(section.Key, section);
                    }
                    else if (section.Key == "wepay")
                    {
                        services.Configure<WeChatPayOptions>(section);
                    }
                    else if (section.Key.StartsWith("wepay"))
                    {
                        services.Configure<WeChatPayOptions>(section.Key, section);
                    }
                    else if (section.Key == "unionpay")
                    {
                        services.Configure<UnionPayOptions>(section);
                    }
                    else if (section.Key.StartsWith("unionpay"))
                    {
                        services.Configure<UnionPayOptions>(section.Key, section);
                    }
                }
            });

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
