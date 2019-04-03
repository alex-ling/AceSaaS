using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Acesoft.Rbac;
using Acesoft.Web.Mvc;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "RBAC")]
	[Route("api/[controller]/[action]")]
	public class CryptoController : ApiControllerBase
	{
		private readonly ILogger<CryptoController> logger;
        private readonly IRsaService rsaService;

        public CryptoController(ILogger<CryptoController> logger,
            IRsaService rsaService)
		{
			this.logger = logger;
            this.rsaService = rsaService;
		}

		[HttpGet, Action("获取密钥")]
		public IActionResult GetKey()
		{
			return Json(rsaService.GetRsa());
		}

		[HttpGet, Action("加密数据")]
		public IActionResult Encrypt(string key, string data)
		{
			return Json(rsaService.Encrypt(key, data));
		}

		[HttpGet, Action("解密数据")]
		public IActionResult Decrypt(string key, string data)
		{
			return Json(rsaService.Decrypt(key, data));
		}
	}
}
