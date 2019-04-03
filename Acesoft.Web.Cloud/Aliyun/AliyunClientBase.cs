using System;

using Acesoft.Web.Cloud.Config;

namespace Acesoft.Web.Cloud.Aliyun
{
	public abstract class AliyunClientBase
	{
		public CloudAccess Access { get; private set; }

        public AliyunClientBase(CloudAccess access)
		{
            this.Access = access;
		}
	}
}
