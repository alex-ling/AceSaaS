using System;

using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Aliyun.Net.SDK.Core;
using Aliyun.Net.SDK.Core.Exceptions;
using Aliyun.Net.SDK.Core.Profile;
using Microsoft.Extensions.Logging;
using Acesoft.Logger;
using Acesoft.Util;

namespace Acesoft.Web.Cloud.Aliyun
{
	public class AliyunSms : AliyunClientBase, ISmsService
	{
		private const string SMS_Product = "Dysmsapi";
		private const string Domain = "dysmsapi.aliyuncs.com";
		private readonly ILogger<AliyunSms> logger = LoggerContext.GetLogger<AliyunSms>();

		public DefaultAcsClient Client { get; private set; }

		public AliyunSms(CloudAccess access) : base(access)
		{
			DefaultProfile.AddEndpoint(access.Endpoint, access.Endpoint, "Dysmsapi", "dysmsapi.aliyuncs.com");

			var profile = DefaultProfile.GetProfile(access.Endpoint, access.AccessKeyId, access.AccessKeySecret);
			Client = new DefaultAcsClient(profile);
		}

		public void Send(string phone, string signName, string templateCode, object param, string outId = null)
		{
			SendSmsRequest sendSmsRequest = new SendSmsRequest();
			try
			{
				sendSmsRequest.PhoneNumbers = phone;
				sendSmsRequest.SignName = signName;
				sendSmsRequest.TemplateCode = templateCode;
				sendSmsRequest.TemplateParam = SerializeHelper.ToJson(param);
				sendSmsRequest.OutId = outId;
				SendSmsResponse acsResponse = Client.GetAcsResponse(sendSmsRequest);
				if (!acsResponse.HttpResponse.isSuccess())
				{
					throw new AceException("Send sms failed: " + acsResponse.Message);
				}
			}
			catch (ServerException ex)
			{
				logger.LogError(ex, "Send sms to [" + phone + "," + signName + "," + templateCode + "] failed!");
				throw new AceException("Send sms failed: " + ex.Message);
			}
			catch (ClientException ex2)
			{
				logger.LogError(ex2, "Send sms to [" + phone + "," + signName + "," + templateCode + "] failed!");
				throw new AceException("Send sms failed: " + ex2.Message);
			}
		}
	}
}
