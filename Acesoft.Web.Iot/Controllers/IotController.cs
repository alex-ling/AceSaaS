using Acesoft.NetCore.Cloud;
using Acesoft.NetCore.Cloud.Moji;
using Acesoft.NetCore.Cloud.Tencent;
using Acesoft.NetCore.Data;
using Acesoft.NetCore.Data.SqlMapper;
using Acesoft.NetCore.Iot;
using Acesoft.NetCore.Iot.Client;
using Acesoft.NetCore.Iot.Data;
using Acesoft.NetCore.Iot.WsClient;
using Acesoft.NetCore.Rbac;
using Acesoft.NetCore.Rbac.Entity;
using Acesoft.NetCore.Rbac.Filters;
using Acesoft.NetCore.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Acesoft.NetCore.Api
{
	/// <summary>
	/// 提供物联网关开启、停止、重启，智能设备绑定、状态、命令操作等
	/// </summary>
	[ApiExplorerSettings(GroupName = "iot")]
	[Route("api/[controller]/[action]")]
	[Consumes("application/json", new string[]
	{
		"multipart/form-data"
	})]
	public class IotController : ControllerBase
	{
		private readonly ILogger<IotController> logger;

		public IotController(ILogger<IotController> logger)
		{
			this.logger = logger;
		}

		/// <summary>
		/// 获取设备清单，该API仅适用于Web端
		/// </summary>
		/// <param name="request">请求的GRID参数</param>
		/// <returns>设备清单JSON，含有ONLINE标志</returns>
		[HttpGet]
		[MAuthorize]
		[DataSource]
		[LogFilter(Desc = "设备清单")]
		public async Task<IActionResult> Grid([FromQuery] GridRequest request)
		{
			CheckRequestDsParameter();
			RequestContext requestContext = new RequestContext(base.SqlScope, base.SqlId, request, OpType.gets);
			requestContext.ExtraParams = Membership.GetParams();
			GridResponse res = DbContext.Current.QueryPageTable(requestContext, request);
			foreach (DataRow row in res.Data.Rows)
			{
				row["online"] = (await IotContext.GetIotData((string)row["sbno"], false)).Online;
			}
			return Json(res);
		}

		[HttpGet]
		[MAuthorize]
		[DataSource]
		[LogFilter(Desc = "设备清单")]
		public async Task<IActionResult> List()
		{
			CheckRequestDsParameter();
			RequestContext requestContext = new RequestContext(base.SqlScope, base.SqlId);
			requestContext.ExtraParams = Membership.GetParams();

			var res = DbContext.Current.Query<dynamic>(requestContext);
			foreach (var item in res)
			{
                item.online = (await IotContext.GetIotData((string)item.sbno, false)).Online;
			}
			return Json(res);
		}

		/// <summary>
		/// 上传MAC文件进行设备批量注册，该API仅适用于Web端
		/// </summary>
		/// <param name="form">提交的上传文件</param>
		/// <returns>成功注册数</returns>
		[HttpPost]
		[MAuthorize]
		[LogFilter(Desc = "上传注册")]
		public async Task<IActionResult> Upload(IFormCollection form)
		{
			int count = 0;
			int rows = 0;
			int num = 0;
			IFormFile formFile = form.Files["file"];
			string proId = App.GetForm("productid", "");
			string sbno = App.GetForm("sbno", "");
			using (Stream stream = formFile.OpenReadStream())
			{
				XSSFWorkbook xSSFWorkbook = new XSSFWorkbook(stream);
				ISheet sheet = xSSFWorkbook.GetSheetAt(0);
				int maxRow = sheet.LastRowNum;
				for (int row = 1; row <= maxRow; row++)
				{
					count++;
					string text = sheet.GetRow(row).GetCell(0).StringCellValue.Trim();
					if (text.Length == 12)
					{
						rows++;
						int num2 = num;
						num = num2 + await InsertIotDevice(new
						{
							product_id = proId,
							user_id = base.AC.User.Id,
							sbno = sbno,
							mac = text,
							rkrq = DateTime.Now.ToDate("yyyy-MM-dd"),
							sbzt = "0"
						});
					}
				}
			}
			return Ok(new
			{
				count = count,
				macs = rows,
				success = num
			});
		}

		/// <summary>
		/// 根据MAC进行设备单个注册，该API仅适用于Web端
		/// </summary>
		/// <param name="data">提交的数据</param>
		/// <returns>无返回值</returns>
		[HttpPost]
		[MAuthorize]
		[DataSource]
		[LogFilter(Desc = "设备注册")]
		public async Task<IActionResult> Post([FromBody] JObject data)
		{
			CheckRequestDsParameter();
			RequestContext ctx = new RequestContext(base.SqlScope, base.SqlId, data.ToParam(), OpType.ins);
			if (await DbContext.Current.ExecuteAsync(ctx) > 0)
			{
				string text = data.GetValue("sbno", "");
				if (ctx.Params.ContainsKey("mac"))
				{
					text = IotContext.GetIotSbno(ctx.Params["mac"].ToString());
				}
				if (text.Length == 12)
				{
					await IotContext.UpdateDeviceInfo(text);
				}
			}
			return Ok(null);
		}

		/// <summary>
		/// 修改用户设备数据，该API仅适用于Web端
		/// </summary>
		/// <param name="data">提交的数据</param>
		/// <returns>无返回值</returns>
		[HttpPut]
		[MAuthorize]
		[DataSource]
		[LogFilter(Desc = "编辑数据")]
		public async Task<IActionResult> Put([FromBody] JObject data)
		{
			RequestContext ctx = new RequestContext(base.SqlScope, base.SqlId, data.ToParam(), OpType.upd);
			if (await DbContext.Current.ExecuteAsync(ctx) > 0 && ctx.Params.ContainsKey("sbno"))
			{
				await IotContext.UpdateDeviceInfo(ctx.Params["sbno"].ToString());
			}
			return Ok(null);
		}

		/// <summary>
		/// 删除MAC及取消对应的设备注册，该API仅适用于Web端
		/// </summary>
		/// <param name="id">设备Mac编号</param>
		/// <returns>无返回值</returns>
		[HttpDelete]
		[MAuthorize]
		[DataSource]
		[LogFilter(Desc = "删除设备")]
		public async Task<IActionResult> Delete(string id)
		{
			CheckRequestDsParameter();
			string[] array = id.Split(',', StringSplitOptions.None);
			foreach (string id2 in array)
			{
				RequestContext ctx = new RequestContext(base.SqlScope, base.SqlId, new
				{
					id = id2
				}, OpType.del);
				if (await DbContext.Current.ExecuteAsync(ctx) > 0)
				{
					IotContext.ClearIotClear(id);
				}
			}
			return Ok(null);
		}

		/// <summary>
		/// 恢复设备为出厂状态，该API仅适用于Web端
		/// </summary>
		/// <param name="id">设备的Mac编号</param>
		/// <returns></returns>
		[HttpDelete]
		[MAuthorize]
		[LogFilter(Desc = "恢复出厂")]
		public async Task<IActionResult> Reset(string id)
		{
			string[] array = id.Split(',', StringSplitOptions.None);
			foreach (string mac in array)
			{
				RequestContext ctx = new RequestContext("iot", "exe_iot_clear", new
				{
					mac
				}, OpType.exe);
				if (await DbContext.Current.ExecuteAsync(ctx) > 0)
				{
					IotContext.ClearIotClear(mac);
				}
			}
			return Ok(null);
		}

		private async Task<int> InsertIotDevice(object param)
		{
			try
			{
				RequestContext ctx = new RequestContext("iot", "device", param, OpType.ins);
				if (await DbContext.Current.ExecuteAsync(ctx) > 0)
				{
					IotContext.GetIotSbno(ctx.Params["mac"].ToString());
				}
				return 1;
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// 下发一个命令到智能设备
		/// </summary>
		/// <param name="data">提交的Json数据体
		/// <![CDATA[
		/// {
		///   "sbno": "HOD1183HJCS0001X",
		///   "cmd": "00A1",
		///   "body": "01"
		/// }
		/// ]]>
		/// </param>
		/// <returns>成功返回命令执行结果，16进制字符串</returns>
		private IActionResult SetResponse(string result, Func<string, object> res = null)
		{
			string a = result.Substring(0, 2);
			if (a == "00")
			{
				if (res != null)
				{
					return Ok(res(result.Substring(2)));
				}
				return Ok(null);
			}
			if (a == "03")
			{
				throw new AceException("非法设备");
			}
			if (a == "FF")
			{
				throw new AceException("非法请求，请检查参数是否正确");
			}
			if (a == "02")
			{
				throw new AceException("会话错误");
			}
			if (a == "01")
			{
				throw new AceException("校验错误");
			}
			throw new AceException("未知错误：" + result);
		}

		/// <summary>
		/// 设置上传频率
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <param name="second">时长，必须&gt;=5秒</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置上传频率")]
		public async Task<IActionResult> SetUpInterval([Required] string sbno, int second)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置上传频率", sbno, iotData.Mac, "00F2", second.ToHex(2)), null);
		}

		/// <summary>
		/// 设置实时上传
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <param name="flag">1：开启，0：关闭</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置实时上传")]
		public async Task<IActionResult> SetUpRealTime([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "开启实时上传" : "关闭实时上传";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00F3", (flag >= 1) ? "01" : "00"), null);
		}

		/// <summary>
		/// 设置电源
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <param name="flag">1：开启，0：关闭</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置电源")]
		public async Task<IActionResult> SetPower([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "开机" : "关机";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00A1", (flag >= 1) ? "01" : "00"), null);
		}

		/// <summary>
		/// 设置风速
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <param name="speed">1-8档</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置风速")]
		public async Task<IActionResult> SetSpeed([Required] string sbno, int speed)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置风速", sbno, iotData.Mac, "00A2", speed.ToHex(2)), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置湿度")]
		public async Task<IActionResult> SetHumidity([Required] string sbno, int humidity)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置湿度", sbno, iotData.Mac, "00A5", humidity.ToHex(2)), null);
		}

		/// <summary>
		/// 设置模式
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <param name="mode">0-5：自动/手动/睡眠/净化/强净/超净</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置模式")]
		public async Task<IActionResult> SetMode([Required] string sbno, int mode)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置模式", sbno, iotData.Mac, "00A3", mode.ToHex(2)), null);
		}

		/// <summary>
		/// 设置网络（配网）
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <param name="flag">0：正常运行，1：使用airkiss配网</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置网络")]
		public async Task<IActionResult> SetNetwork([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("开启配网", sbno, iotData.Mac, "00A4", flag.ToHex(2)), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置键盘")]
		public async Task<IActionResult> SetLock([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "锁定" : "无锁";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00AA", (flag >= 1) ? "01" : "00"), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "启用辅热")]
		public async Task<IActionResult> SetHot([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "启用" : "关闭";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00A6", (flag >= 1) ? "01" : "00"), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "启用除湿")]
		public async Task<IActionResult> SetDehumidity([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "启用" : "关闭";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00A7", (flag >= 1) ? "01" : "00"), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "启用阀门")]
		public async Task<IActionResult> SetDoor([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "启用" : "关闭";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00A8", (flag >= 1) ? "01" : "00"), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "启用UV灯")]
		public async Task<IActionResult> SetUvLight([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "启用" : "关闭";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00AB", (flag >= 1) ? "01" : "00"), null);
		}

		/// <summary>
		/// 设置屏幕
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <param name="flag">0：息屏，1：亮屏</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置屏幕")]
		public async Task<IActionResult> SetScreen([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "亮屏" : "息屏";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00A5", (flag >= 1) ? "01" : "00"), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置新风")]
		public async Task<IActionResult> SetWind([Required] string sbno, int flag)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			string name = (flag >= 1) ? "循环" : "新风";
			return SetResponse(await IotClient.Instance.SendCmd(name, sbno, iotData.Mac, "00AB", (flag >= 1) ? "01" : "00"), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置滤网时间")]
		public async Task<IActionResult> SetNetDays([Required] string sbno, int days)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置滤网时间", sbno, iotData.Mac, "00AC", "02" + days.ToHex(2)), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置滤网复位")]
		public async Task<IActionResult> ResetNet([Required] string sbno)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置滤网复位", sbno, iotData.Mac, "00AC", "03"), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "查询滤网天数")]
		public async Task<IActionResult> GetNetDays([Required] string sbno)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("查询滤网天数", sbno, iotData.Mac, "00AC", "01"), (string data) => data.Substring(0, 2).ToInt());
		}

		/// <summary>
		/// 设置关机定时
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <param name="minutes">关机的分钟0-480分钟</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置关机定时")]
		public async Task<IActionResult> SetPowerOffPlan([Required] string sbno, int minutes)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置关机定时", sbno, iotData.Mac, "00A6", "01" + minutes.ToHex(4)), null);
		}

		/// <summary>
		/// 查询关机定时
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <returns>返回定时信息，包含关机小时和分钟</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "查询关机定时")]
		public async Task<IActionResult> GetPowerOffPlan([Required] string sbno)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("查询关机定时", sbno, iotData.Mac, "00A6", "02"), (string data) => data.Substring(0, 4).ToInt());
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "查询定时计划")]
		public async Task<IActionResult> GetTimePlan([Required] string sbno)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("查询定时计划", sbno, iotData.Mac, "00AD", "01"), (string data) => data);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置定时计划")]
		public async Task<IActionResult> SetTimePlan([Required] string sbno, string plan)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置定时计划", sbno, iotData.Mac, "00AD", "02" + plan), null);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "查询风速电压")]
		public async Task<IActionResult> GetSpeedVol([Required] string sbno)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("查询风速电压", sbno, iotData.Mac, "00AE", "01"), (string data) => data);
		}

		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置风速电压")]
		public async Task<IActionResult> SetSpeedVol([Required] string sbno, string content)
		{
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SendCmd("设置风速电压", sbno, iotData.Mac, "00AE", "02" + content), null);
		}

		/// <summary>
		/// 设置网络时间
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置网络时间")]
		public async Task<IActionResult> SetTime([Required] string sbno)
		{
			DateTime now = DateTime.Now;
			IotData iotData = await IotContext.GetIotData(sbno, false);
			return SetResponse(await IotClient.Instance.SyncTime("同步时间", sbno, iotData.Mac, "00A9"), null);
		}

		/// <summary>
		/// 获得当前用户的设备列表
		/// </summary>
		/// <returns>设备列表JSON，包含设备的最新状态数据</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设备列表")]
		public async Task<IActionResult> GetDevices()
		{
			return Ok(await IotContext.GetDevices());
		}

		/// <summary>
		/// 获取指定设备的状态数据
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <returns>状态数据JSON</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "获取状态")]
		public async Task<IActionResult> GetData(string sbno)
		{
			return Ok(await IotContext.GetIotData(sbno, true));
		}

		/// <summary>
		/// 绑定某一设备到当前用户，设备必须未绑定，已绑定设备提示错误
		/// </summary>
		/// <param name="data">设备编号、是否可操控（可选）参数
		/// <![CDATA[
		/// {
		///   "sbno": "XXXXXXXXXXXX",
		///   "alias": "可选，设备别名",
		///   "lon": "经度",
		///   "lat": "纬度",
		///   "loc": "可选，定位详细地址",
		///   "province": "可选，省份",
		///   "city": "可选，城市",
		///   "district": "可选，地区"，
		///   "adcode": "可选，地址编码"
		/// }
		/// ]]>
		/// </param>
		/// <returns>无返回值</returns>
		[HttpPost]
		[MAuthorize]
		[LogFilter(Desc = "绑定设备")]
		public async Task<IActionResult> PostDevice([FromBody] JObject data)
		{
			string value = data.GetValue("sbno", "");
			string value2 = data.GetValue("lon", "");
			string value3 = data.GetValue("lat", "");
			string value4 = data.GetValue("adcode", "");
			Location location = value4.HasValue() ? new Location
			{
				Longitude = value2,
				Latitude = value3,
				AdCode = value4,
				Address = data.GetValue("loc", ""),
				Province = data.GetValue("province", ""),
				City = data.GetValue("city", ""),
				District = data.GetValue("district", "")
			} : GeoCoder.GetLoc(value2, value3);
			WeaResult wea = App.GetService<IWeaService>().GetWea(value2, value3);
			var param = new
			{
				newid = App.IdWorker.NextId(),
				userid = base.AC.User.Id,
				sbno = value,
				alias = data.GetValue("alias", ""),
				lon = value2,
				lat = value3,
				loc = location.Address,
				sf = location.Province,
				cs = location.City,
				dq = location.District,
				adcode = location.AdCode,
				cityid = wea.city.cityid
			};
			RequestContext ctx = new RequestContext("iot", "exe_iot_add", param, OpType.exe);
			DbContext.Current.Execute(ctx);
			await IotContext.UpdateDeviceInfo(value);
			return Ok(null);
		}

		/// <summary>
		/// 解除当前用户对某个设备的绑定
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <returns>无返回值</returns>
		[HttpDelete]
		[MAuthorize]
		[LogFilter(Desc = "解除绑定")]
		public async Task<IActionResult> DelDevice(string sbno)
		{
			RequestContext ctx = new RequestContext("iot", "exe_iot_unbind", new
			{
				userid = base.AC.User.Id,
				sbno = sbno
			}, OpType.exe);
			await DbContext.Current.ExecuteAsync(ctx);
			await IotContext.UpdateDeviceInfo(sbno);
			return Ok(null);
		}

		/// <summary>
		/// 解除某个用户对某个设备的绑定，该API仅适用于Web端
		/// </summary>
		/// <param name="id">用户Id</param>
		/// <param name="sbno">设备编号</param>
		/// <returns>无返回值</returns>
		[HttpDelete]
		[MAuthorize]
		[LogFilter(Desc = "解除绑定")]
		public async Task<IActionResult> DelDeviceUser(long id, string sbno)
		{
			RequestContext ctx = new RequestContext("iot", "exe_iot_unbind", new
			{
				userid = id,
				sbno = sbno
			}, OpType.exe);
			await DbContext.Current.ExecuteAsync(ctx);
			await IotContext.UpdateDeviceInfo(sbno);
			return Ok(null);
		}

		/// <summary>
		/// 修改设备，如设置别名、转移设备
		/// </summary>
		/// <param name="data">设备编号、接收者的手机号等信息
		/// <![CDATA[
		/// {
		///   "sbno": "XXXXXXXXXXXX",
		///   "mobile": "可选，接收者手机号",
		///   "alias": "可选，设备别名",
		///   "netmin": "滤网更换时间",
		///   "netalert": "启用/关闭滤网提醒",
		///   "plandata": "定时设置数据"
		/// }
		/// ]]>
		/// </param>
		/// <returns>无返回值</returns>
		[HttpPut]
		[MAuthorize]
		[LogFilter(Desc = "修改设备")]
		public async Task<IActionResult> PutDevice([FromBody] JObject data)
		{
			string sbno = data.GetValue("sbno", "");
			long ownerId = await IotContext.CheckDeviceOwner(sbno, base.AC.User.Id);
			string planData = data.GetNullValue<string>("plandata", null);
			int? value = data.GetValue<int?>("planenabled", null);
			int? planEnabled = await IotContext.CheckPlanEnabled(sbno, planData, value);
			RequestContext ctx = new RequestContext("iot", "exe_iot_put", new
			{
				sbno = sbno,
				ownerid = ownerId,
				userid = CheckMobileUser(data, true),
				alias = data.GetValue<string>("alias", null),
				netmin = data.GetValue<int?>("netmin", null),
				netalert = data.GetValue<int?>("netalert", null),
				plandata = planData,
				planenabled = planEnabled
			}, OpType.exe);
			DbContext.Current.Execute(ctx);
			await IotContext.UpdateDeviceInfo(sbno);
			if (planEnabled.HasValue)
			{
				int? nullable = planEnabled;
				if ((nullable.GetValueOrDefault() > 0) & nullable.HasValue)
				{
					await SetPowerOffPlan(sbno, 0);
				}
			}
			return Ok(null);
		}

		/// <summary>
		/// 获取设备用户清单
		/// </summary>
		/// <param name="sbno">设备编号</param>
		/// <returns>设备用户列表（不包含自己，拥有者）</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "获取设备用户")]
		public async Task<IActionResult> GetAuths(string sbno)
		{
			await IotContext.CheckDeviceOwner(sbno, base.AC.User.Id);
			var param = new
			{
				sbno
			};
			RequestContext ctx = new RequestContext("iot", "iot_get_auths", param, OpType.gets);
			IEnumerable<object> value = DbContext.Current.Query(ctx);
			return Ok(value);
		}

		/// <summary>
		/// 绑定某一用户和设备，只能由设备拥有者操作该接口
		/// </summary>
		/// <param name="data">用户手机、设备编号、是否可操控（可选）参数
		/// <![CDATA[
		/// {
		///   "mobile": "13XXXXXXXXX",
		///   "sbno": "XXXXXXXXXXXX",
		///   "alias": "别名",
		///   "write": true/false
		/// }
		/// ]]>
		/// </param>
		/// <returns>无返回值</returns>
		[HttpPost]
		[MAuthorize]
		[LogFilter(Desc = "授权用户")]
		public IActionResult PostAuth([FromBody] JObject data)
		{
			var param = new
			{
				newid = App.IdWorker.NextId(),
				ownerid = base.AC.User.Id,
				userid = CheckMobileUser(data, false),
				sbno = data.GetValue("sbno", ""),
				alias = data.GetValue("alias", ""),
				write = data.GetValue("write", true)
			};
			RequestContext ctx = new RequestContext("iot", "exe_iot_bind", param, OpType.exe);
			DbContext.Current.Execute(ctx);
			return Ok(null);
		}

		/// <summary>
		/// 修改某个授权记录，注意只能修改设置别名
		/// </summary>
		/// <param name="data">授权Id、别名参数
		/// <![CDATA[
		/// {
		///   "id": "",
		///   "alias": "别名"
		/// }
		/// ]]>
		/// </param>
		/// <returns>无返回值</returns>
		[HttpPut]
		[MAuthorize]
		[LogFilter(Desc = "授权修改")]
		public IActionResult PutAuth([FromBody] JObject data)
		{
			RequestContext ctx = new RequestContext("iot", "userdevice", data.ToParam(), OpType.upd);
			DbContext.Current.Execute(ctx);
			return Ok(null);
		}

		/// <summary>
		/// 删除某个授权记录
		/// </summary>
		/// <param name="id">授权记录Id</param>
		/// <returns></returns>
		[HttpDelete]
		[MAuthorize]
		[LogFilter(Desc = "删除授权")]
		public IActionResult DelAuth(string id)
		{
			IList<long> ids = id.ToList<long>(',');
			RequestContext ctx = new RequestContext("iot", "userdevice", new
			{
				ids,
				id
			}, OpType.del);
			DbContext.Current.Execute(ctx);
			return Ok(null);
		}

		private long CheckMobileUser(JObject data, bool empty = false)
		{
			string value = data.GetValue("mobile", "");
			if (!empty && !value.HasValue())
			{
				throw new AceException("必须提交绑定用户的手机号码！");
			}
			if (empty && !value.HasValue())
			{
				return 0L;
			}
			RBAC_User byMobile = new RBAC_UserRep().GetByMobile(value);
			if (byMobile == null)
			{
				throw new AceException("手机号[" + value + "]对应用户不存在！");
			}
			return byMobile.Id;
		}

		/// <summary>
		/// 启动客户端服务程序
		/// </summary>
		/// <returns>无返回值</returns>
		[HttpGet]
		[LogFilter(Desc = "启动客户端")]
		public async Task<IActionResult> StartClient()
		{
			await IotClient.Instance.Open();
			return Ok(null);
		}

		/// <summary>
		/// 关闭客户端服务程序
		/// </summary>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "关闭客户端")]
		public IActionResult StopClient()
		{
			IotClient.Instance.Close();
			return Ok(null);
		}

		/// <summary>
		/// 设置缓存状态开关，如打开/关闭日志
		/// </summary>
		/// <param name="key">缓存Key</param>
		/// <param name="value">缓存Value</param>
		/// <returns>无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "设置缓存状态")]
		public IActionResult SetStatus(string key, string value)
		{
			IotContext.SetStatus(key, value.ToObject<bool>());
			return Ok(null);
		}

		/// <summary>
		/// 启动IoT网关服务
		/// </summary>
		/// <param name="server">服务名称</param>
		/// <returns>成功无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "启动服务")]
		public IActionResult StartServer(string server)
		{
			IotWsClient.Instance.Start(server);
			return Ok(null);
		}

		/// <summary>
		/// 停止IoT网关服务
		/// </summary>
		/// <param name="server">服务名称</param>
		/// <returns>成功无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "停止服务")]
		public IActionResult StopServer(string server)
		{
			IotWsClient.Instance.Stop(server);
			return Ok(null);
		}

		/// <summary>
		/// 重启IoT网关服务
		/// </summary>
		/// <param name="server">服务名称</param>
		/// <returns>成功无返回值</returns>
		[HttpGet]
		[MAuthorize]
		[LogFilter(Desc = "重启服务")]
		public IActionResult RestartServer(string server)
		{
			IotWsClient.Instance.Restart(server);
			return Ok(null);
		}
	}
}
