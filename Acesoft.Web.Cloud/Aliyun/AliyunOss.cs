using System;

using Aliyun.OSS;
using Newtonsoft.Json.Linq;
using Acesoft.Util;
using Acesoft.Web.Cloud.Config;

namespace Acesoft.Web.Cloud.Aliyun
{
	public class AliyunOss : AliyunClientBase, IOssService
	{
		public OssClient Client { get; private set; }

        public AliyunOss(CloudAccess access) : base(access)
		{
			Client = new OssClient(access.Endpoint, access.AccessKeyId, access.AccessKeySecret);
		}

		public JObject GetSignature(string bucket, string dir)
		{
			DateTime now = DateTime.Now;
			DateTime dateTime = now.AddSeconds(60.0);
			var policyConditions = new PolicyConditions();
			policyConditions.AddConditionItem("content-length-range", 0L, 1048576000L);
			policyConditions.AddConditionItem(MatchMode.StartWith, "key", dir);

            var policy = EncodingHelper.ToBase64(Client.GeneratePostPolicy(dateTime, policyConditions));
			var value = CryptoHelper.ComputeSHA1Signature(Access.AccessKeySecret, policy);
			var scheme = App.Context.Request.GetScheme();
			var jObject = new JObject();
			jObject["accessid"] = Access.AccessKeyId;
			jObject["policy"] = policy;
			jObject["signature"] = value;
			jObject["dir"] = dir + "_" + now.ToYMD();
			jObject["host"] = scheme + "://" + bucket + "." + Access.Endpoint;
			jObject["expire"] = dateTime.ToUnix();

			return jObject;
		}

		public void DeleteFile(string bucket, string key)
		{
			Client.DeleteObject(bucket, key);
		}
	}
}
