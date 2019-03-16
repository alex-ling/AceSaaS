using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using QRCoder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Acesoft.Web.Mvc;
using Acesoft.Cache;

namespace Acesoft.Web.UI.Controllers
{
	[ApiExplorerSettings(GroupName = "WebUI")]
	[Route("api/[controller]/[action]")]
	public class DrawController : ApiControllerBase
	{
		private readonly ILogger<DrawController> logger;

		public DrawController(ILogger<DrawController> logger)
		{
			this.logger = logger;
		}

		private string CreateValidCode(int length)
		{
			string text = "";
			Random random = new Random();
			for (int i = 0; i < length; i++)
			{
				int num = random.Next();
				char c = (num % 2 != 0) ? ((char)(65 + (ushort)(num % 26))) : ((char)(48 + (ushort)(num % 10)));
				if (c == '0' || c == 'O')
				{
					i--;
				}
				else
				{
					text += c.ToString();
				}
			}
			return text;
		}

		private MemoryStream CreateImage(string code)
		{
			MemoryStream memoryStream = null;
			Random random = new Random();
			Color[] array = new Color[8]
			{
				Color.Black,
				Color.Red,
				Color.DarkBlue,
				Color.Green,
				Color.Orange,
				Color.Brown,
				Color.DarkCyan,
				Color.Purple
			};
			string[] array2 = new string[5]
			{
				"Verdana",
				"Microsoft Sans Serif",
				"Comic Sans MS",
				"Arial",
				"宋体"
			};
			Bitmap bitmap = new Bitmap(code.Length * 15, 32);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.White);
			for (int i = 0; i < 100; i++)
			{
				int x = random.Next(bitmap.Width);
				int y = random.Next(bitmap.Height);
				graphics.DrawRectangle(new Pen(Color.LightGray, 0f), x, y, 1, 1);
			}
			for (int j = 0; j < code.Length; j++)
			{
				int num = random.Next(7);
				int num2 = random.Next(5);
				Font font = new Font(array2[num2], 15f, FontStyle.Bold);
				Brush brush = new SolidBrush(array[num]);
				int num3 = 4;
				if ((j + 1) % 2 == 0)
				{
					num3 = 2;
				}
				graphics.DrawString(code.Substring(j, 1), font, brush, (float)(3 + j * 12), (float)num3);
			}
			memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Png);
			graphics.Dispose();
			bitmap.Dispose();
			return memoryStream;
		}

		[HttpGet, Action("图片验证码")]
		public IActionResult GetValidImage(string type, int length = 5)
		{
			var key = "valid_img_" + type;
			var text = CreateValidCode(length);
			var memoryStream = CreateImage(text);
			if (!type.HasValue())
			{
				type = "root";
			}

			App.Cache.SetString(key, text, opts =>
            {
                opts.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            });
			base.Response.Body.Dispose();

			return File(memoryStream.ToArray(), "image/png");
		}

		[HttpGet, Action("生成二维码")]
		public IActionResult GetQrCode(string text, int size = 10)
		{
			var graphic = new QRCode(new QRCodeGenerator().CreateQrCode(text, QRCodeGenerator.ECCLevel.Q)).GetGraphic(size);
			base.Response.Body.Dispose();

			var memoryStream = new MemoryStream();
			graphic.Save(memoryStream, ImageFormat.Png);
			graphic.Dispose();

			return File(memoryStream.ToArray(), "image/png");
		}
	}
}
