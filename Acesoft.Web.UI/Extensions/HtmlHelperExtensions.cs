using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Text;

namespace Acesoft.Web.UI
{
	public static class HtmlHelperExtensions
	{
		public static HtmlString HtmlForDate(this IHtmlHelper html, object date, string format)
		{
			object obj;
			if ((obj = date) is DateTime)
			{
				return new HtmlString(((DateTime)obj).ToString(format));
			}
			string text;
			if ((text = (date as string)) != null && text.HasValue())
			{
				return html.HtmlForDate(DateTime.Parse(text), format);
			}
			return new HtmlString("");
		}

		public static HtmlString HtmlForDate(this IHtmlHelper html, object date)
		{
			object obj;
			if ((obj = date) is DateTime)
			{
				DateTime date2 = (DateTime)obj;
				return html.HtmlForDate(date2);
			}
			string text;
			if ((text = (date as string)) != null && text.HasValue())
			{
				return html.HtmlForDate(DateTime.Parse(text));
			}
			return new HtmlString("");
		}

		public static HtmlString HtmlForDate(this IHtmlHelper html, DateTime date)
		{
			string text = "";
			TimeSpan timeSpan = DateTime.Now - date;
			int days = timeSpan.Days;
			if (days != 0)
			{
				text = ((days > 3) ? date.ToString("M月d日") : $"{days}天前");
			}
			else
			{
				timeSpan = DateTime.Now - date;
				double totalHours = timeSpan.TotalHours;
				text = ((totalHours < 0.5) ? "刚刚发生" : ((!(totalHours < 1.0)) ? $"{(int)Math.Floor(totalHours) - 1}小时前" : "半小时前"));
			}
			return new HtmlString(text);
		}

		public static HtmlString HtmlForPhoto(this IHtmlHelper html, string photos, string width = "150px", string height = "120px", int index = 0, bool link = false)
		{
			StringBuilder sb = new StringBuilder();
			photos.Split(',').Each(delegate(string item)
			{
				if (item.HasValue())
				{
					string text = App.GetWebPhoto(item, false, "images/none.jpg");
					if (link)
					{
						sb.Append("<a href=\"" + text + "\" title=\"点击查看大图\" target=\"_blank\">");
						sb.Append("<img src=\"" + text + "\" style=\"width:" + width + ";height:" + height + "\" />");
						sb.Append("</a>");
					}
					else
					{
						sb.Append($"<img src=\"{text}\" class=\"hand\" style=\"width:{width};height:{height}\" onclick=\"AX.wxPhoto({index})\" />");
					}
				}
			});
			return new HtmlString(sb.ToString());
		}

		public static HtmlString HtmlForAttach(this IHtmlHelper html, string attachs, string cls = "lh20")
		{
			StringBuilder sb = new StringBuilder();
			int index = 0;
			attachs.Trim(',').Split(',').Each(delegate(string item)
			{
				if (item.HasValue())
				{
					string text = App.GetWebPath(item, false);
					string attachTitle = html.GetAttachTitle(item);
					sb.Append("<div>");
					sb.Append($"<a class=\"{cls}\" href=\"{text}\" target=\"_blank\" title=\"{attachTitle}\">{++index}.{attachTitle}</a>");
					sb.Append("</div>");
				}
			});
			return new HtmlString(sb.ToString());
		}

		public static string GetAttachTitle(this IHtmlHelper html, string attach)
		{
			string fileName = Path.GetFileName(attach);
			int num = fileName.IndexOf("_");
			if (num >= 0)
			{
				return fileName.Substring(num + 1);
			}
			return fileName;
		}

		public static HtmlString HtmlForMobile(this IHtmlHelper html, string mobile, string none = "未绑定")
		{
			mobile = (mobile.HasValue() ? (mobile.Left(3) + "****" + mobile.Right(4)) : none);
			return new HtmlString(mobile);
		}

		public static HtmlString HtmlFormEmpty(this IHtmlHelper html, string text, string none = "未设置")
		{
			if (!text.HasValue())
			{
				text = none;
			}
			return new HtmlString(text);
		}

		public static HtmlString HtmlForText(this IHtmlHelper html, string text, string tag = "p")
		{
			if (!text.HasValue())
			{
				text = "";
			}
			text = ((!(tag == "br")) ? "<{0}>{1}</{0}>".FormatWith(tag, text.Replace("\n", "</{0}><{0}>".FormatWith(tag))) : text.Replace("\n", "<{0}/>".FormatWith(tag)));
			return new HtmlString(text);
		}

		public static string HtmlForSize(this IHtmlHelper html, string text, int size)
		{
			if (!text.HasValue())
			{
				return text;
			}
			if (text.GetBytesLength() <= size)
			{
				return text;
			}
			return text.LeftOfBytes(size) + "…";
		}
	}
}
