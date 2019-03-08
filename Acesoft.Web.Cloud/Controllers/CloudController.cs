using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Acesoft.Web.Controllers;
using Acesoft.Rbac;

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
    }
}
