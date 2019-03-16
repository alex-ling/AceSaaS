using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Acesoft.Cache;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Util;
using Newtonsoft.Json.Linq;

namespace Acesoft.Web.Cloud.Controllers
{
    [ApiExplorerSettings(GroupName = "plat")]
    [Route("api/[controller]/[action]")]
    public class CloudController : ApiControllerBase
    {
        private readonly ILogger<CloudController> logger;
        private readonly ICloudService cloudService;

        public CloudController(ILogger<CloudController> logger, ICloudService cloudService)
        {
            this.logger = logger;
            this.cloudService = cloudService;
        }

        [HttpGet, MultiAuthorize, Action("获取授权")]
        public IActionResult GetOssSign(string bucket, string dir)
        {
            var result = cloudService.GetOssService().GetSignature(bucket, dir);
            return Ok(result);
        }

        [HttpGet, MultiAuthorize, Action("删除文件")]
        public IActionResult DelOssFile(string bucket, string key)
        {
            cloudService.GetOssService().DeleteFile(bucket, key);
            return Ok(null);
        }

        [HttpGet, MultiAuthorize, Action("获取位置")]
        public Location GetLocation(string lng, string lat)
        {
            return cloudService.GetLocationService().GetLoc(lng, lat);
        }

        [HttpGet, DataSource, Action("短信验证码")]
        public IActionResult GetSms(string mobile, string code)
        {
            CheckDataSourceParameter();

            if (SqlMap.Params.GetValue("checkvalid", true))
            {
                string validCode = GetValidCode(App.GetQuery("type", "root"), true);
                if (!code.HasValue() || validCode != code.ToUpper())
                {
                    throw new AceException("验证码输入错误");
                }
            }

            var chkSql = SqlMap.Params.GetValue("checksql", "");
            var chkErr = SqlMap.Params.GetValue("checkerr", "");
            if (chkSql.HasValue())
            {
                chkSql = AppCtx.AC.Replace(chkSql, true);
                if ((int)AppCtx.Session.ExecuteScalar(chkSql, null) > 0)
                {
                    throw new AceException(chkErr);
                }
            }

            var rndStr = RandomHelper.GetRandomNumberString(6);
            var key = "sms_" + mobile;
            var keySending = "sms_sending_" + mobile;
            if (App.Cache.GetString(keySending) != null)
            {
                throw new AceException("不能频繁获取短信验证码");
            }

            var expired = SqlMap.Params.GetValue("sms_expired", 15);
            var sign = SqlMap.Params.GetValue("sms_sign", "");
            var temp = SqlMap.Params.GetValue("sms_temp", "");
            var times = SqlMap.Params.GetValue("sms_retrytimes", 3);

            var success = false;
            var sms = cloudService.GetSmsService();
            for (var i = 0; i < times; i++)
            {
                try
                {
                    sms.Send(mobile, sign, temp, new { code = rndStr }, null);
                    logger.LogDebug($"Send valid code \"{rndStr}\" to \"{mobile}\"");
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Send valid code to \"{mobile}\" with error: {ex.GetMessage()}");
                }
            }
            if (!success)
            {
                throw new AceException(SqlMap.Params.GetValue("sms_errservice", ""));
            }

            App.Cache.SetString(key, rndStr, opts =>
            {
                opts.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expired);
            });
            App.Cache.SetString(keySending, "1", opts =>
            {
                opts.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            });

            return Ok(null);
        }

        [HttpGet, DataSource, Action("邮件验证码")]
        public IActionResult GetMail(string mailto, string code)
        {
            CheckDataSourceParameter();

            if (SqlMap.Params.GetValue("checkvalid", true))
            {
                string validCode = GetValidCode(App.GetQuery("type", "root"), true);
                if (!code.HasValue() || validCode != code.ToUpper())
                {
                    throw new AceException("验证码输入错误");
                }
            }

            var rndStr = RandomHelper.GetRandomNumberString(6);
            var key = "mail_" + mailto;
            var keySending = "mail-send-" + mailto;
            if (App.Cache.GetString(keySending) != null)
            {
                throw new AceException("不能频繁获取验证码！");
            }

            var expired = SqlMap.Params.GetValue("mail_expired", 15);
            var times = SqlMap.Params.GetValue("sms_retrytimes", 5);

            var success = false;
            var mail = cloudService.GetMailService();
            for (var i = 0; i < times; i++)
            {
                try
                {
                    mail.Send(mailto, "邮件验证码", $"这是你的验证码：{rndStr}，请在{expired}分钟内输入提交！");
                    logger.LogDebug($"Send valid code \"{rndStr}\" to \"{mailto}\"");
                    success = true;
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Send valid code to \"{mailto}\" with error: {ex.GetMessage()}");
                }
            }
            if (!success)
            {
                throw new AceException(SqlMap.Params.GetValue("mail_errservice", ""));
            }

            App.Cache.SetString(key, rndStr, opts =>
            {
                opts.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expired);
            });
            App.Cache.SetString(keySending, "1", opts =>
            {
                opts.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            });

            return Ok(null);
        }

        private string GetValidCode(string type, bool clear = true)
        {
            var key = "valid_img_" + type;
            var result = App.Cache.GetString(key);
            if (clear)
            {
                App.Cache.Remove(key);
            }
            return result;
        }
    }
}
