using Acesoft.NetCore.Iot.Api;
using Acesoft.NetCore.Rbac.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Acesoft.NetCore.Api
{
	/// <summary>
	/// 提供物联网智能设备的远程监测、控制和管理
	/// </summary>
	[ApiExplorerSettings(GroupName = "iot")]
	[Route("api/[controller]/[action]")]
	public class IotApiController : ControllerBase
	{
		private readonly ILogger<IotApiController> logger;

		public IotApiController(ILogger<IotApiController> logger)
		{
			this.logger = logger;
		}

		private IActionResult SetResponse(string result)
		{
			return Ok(result);
		}

		/// <summary>
		/// 单独打开智能开关某一输出端口
		/// </summary>
		/// <param name="addr">地址</param>
		/// <param name="pos">输出端口，从0开始</param>
		/// <param name="on">打开/关闭</param>
		/// <returns>返回数据</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "操作单一开关")]
		public async Task<IActionResult> SwitchOne([Required] string addr, int pos, bool on)
		{
			ApiRequest req = new ApiRequest
			{
				Name = "IotSwitch",
				Addr = addr,
				Action = "one",
				Body = $"{pos}={on}"
			};
			return SetResponse(await ApiClient.Instance.Send(req));
		}

		/// <summary>
		/// 全开/全关智能开关所有输出端口
		/// </summary>
		/// <param name="addr">地址</param>
		/// <param name="num">操作开关数量</param>
		/// <param name="on">打开/关闭</param>
		/// <returns>返回数据</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "操作全部开关")]
		public async Task<IActionResult> SwitchAll([Required] string addr, int num, bool on)
		{
			ApiRequest req = new ApiRequest
			{
				Name = "IotSwitch",
				Addr = addr,
				Action = "all",
				Body = $"{num}={on}"
			};
			return SetResponse(await ApiClient.Instance.Send(req));
		}

		/// <summary>
		/// 读取智能开关全部状态
		/// </summary>
		/// <param name="addr">地址</param>
		/// <param name="num">操作开关数量</param>
		/// <returns>返回数据</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "读取开关状态")]
		public async Task<IActionResult> SwitchRead([Required] string addr, int num)
		{
			ApiRequest req = new ApiRequest
			{
				Name = "IotSwitch",
				Addr = addr,
				Action = "read",
				Body = $"{num}"
			};
			return SetResponse(await ApiClient.Instance.Send(req));
		}

		/// <summary>
		/// 写入智能开关地址
		/// </summary>
		/// <param name="addr">地址</param>
		/// <param name="newAddr">新地址</param>
		/// <returns>返回数据</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "写入地址")]
		public async Task<IActionResult> WriteAddr([Required] string addr, int newAddr)
		{
			ApiRequest req = new ApiRequest
			{
				Name = "IotSwitch",
				Addr = addr,
				Action = "write",
				Body = $"{newAddr}"
			};
			return SetResponse(await ApiClient.Instance.Send(req));
		}
	}
}
