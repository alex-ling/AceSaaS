using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Acesoft.Util.Helper;
using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI
{
	public static class WidgetFactoryExtensions
	{
        #region html
        public static HtmlString HtmlForBool(this WidgetFactory ace, object obj, string trueStr, string falseStr = "&nbsp;")
        {
            if (obj is bool bObj && bObj)
            {
                return new HtmlString(trueStr);
            }
            return new HtmlString(falseStr);
        }

        public static HtmlString HtmlForNew(this WidgetFactory ace, object obj, int days)
        {
            if (obj is DateTime dateObj)
            {
                if (dateObj.AddDays(days) > DateTime.Now)
                {
                    return new HtmlString($"<img src=\"/images/new.png\" />");
                }
            }
            else if (obj is string strObj)
            {
                if (strObj.HasValue())
                {
                    return ace.HtmlForNew(DateTime.Parse(strObj), days);
                }
            }
            return new HtmlString("&nbsp;");
        }

        public static HtmlString HtmlForDate(this WidgetFactory ace, object obj, string format)
        {
            if (obj is DateTime dateObj)
            {
                return new HtmlString(dateObj.ToString(format));
            }
            else if (obj is string strObj)
            {
                if (strObj.HasValue())
                {
                    return ace.HtmlForDate(DateTime.Parse(strObj), format);
                }
            }
            return new HtmlString("&nbsp;");
        }

        public static HtmlString HtmlForDate(this WidgetFactory ace, object obj)
        {
            if (obj is DateTime dateObj)
            {
                return ace.HtmlForDate(dateObj);
            }
            else if (obj is string strObj)
            {
                if (strObj.HasValue())
                {
                    return ace.HtmlForDate(DateTime.Parse(strObj));
                }
            }
            return new HtmlString("&nbsp;");
        }

        public static HtmlString HtmlForDate(this WidgetFactory ace, DateTime obj)
        {
            var text = "";
            var timeSpan = DateTime.Now - obj;
            int days = timeSpan.Days;
            if (days != 0)
            {
                text = ((days > 3) ? obj.ToString("M月d日") : $"{days}天前");
            }
            else
            {
                timeSpan = DateTime.Now - obj;
                var totalHours = timeSpan.TotalHours;
                text = ((totalHours < 0.5) ? "刚刚发生" : ((!(totalHours < 1.0)) ? $"{(int)Math.Floor(totalHours) - 1}小时前" : "半小时前"));
            }
            return new HtmlString(text);
        }

        public static HtmlString HtmlForPhoto(this WidgetFactory ace, string photos, string width = "150px", string height = "120px", int index = 0, bool link = false)
        {
            if (!photos.HasValue())
            {
                return new HtmlString("&nbsp;");
            }

            var sb = new StringBuilder();
            photos.Split(',').Each(item =>
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

        public static string GetAttachSrc(this WidgetFactory ace, string attachs)
        {
            if (!attachs.HasValue())
            {
                return "#";
            }

            return App.GetWebPath(attachs.Trim(',').Split(',').First());
        }

        public static HtmlString HtmlForAttach(this WidgetFactory ace, string attachs, string cls = "lh20")
        {
            if (!attachs.HasValue())
            {
                return new HtmlString("&nbsp;");
            }

            var sb = new StringBuilder();
            int index = 0;
            attachs.Trim(',').Split(',').Each(delegate (string item)
            {
                if (item.HasValue())
                {
                    string text = App.GetWebPath(item);
                    string attachTitle = ace.GetAttachTitle(item);
                    sb.Append("<div>");
                    sb.Append($"<a class=\"{cls}\" href=\"{text}\" target=\"_blank\" title=\"{attachTitle}\">{++index}.{attachTitle}</a>");
                    sb.Append("</div>");
                }
            });
            return new HtmlString(sb.ToString());
        }

        public static string GetAttachTitle(this WidgetFactory html, string attach)
        {
            string fileName = Path.GetFileName(attach);
            int num = fileName.IndexOf("_");
            if (num >= 0)
            {
                return fileName.Substring(num + 1);
            }
            return fileName;
        }

        public static HtmlString HtmlForMobile(this WidgetFactory html, string mobile, string none = "未绑定")
        {
            mobile = (mobile.HasValue() ? (mobile.Left(3) + "****" + mobile.Right(4)) : none);
            return new HtmlString(mobile);
        }

        public static HtmlString HtmlFormEmpty(this WidgetFactory html, object text, string none = "&nbsp;")
        {
            if (text == null || !text.ToString().HasValue())
            {
                text = none;
            }
            return new HtmlString(text.ToString());
        }

        public static HtmlString HtmlFormEmpty(this WidgetFactory html, string text, string none = "&nbsp;")
        {
            if (!text.HasValue())
            {
                text = none;
            }
            return new HtmlString(text);
        }

        public static HtmlString HtmlForText(this WidgetFactory html, string text, string tag = "p")
        {
            if (!text.HasValue())
            {
                text = "&nbsp;";
            }
            else
            {
                if (!tag.HasValue())
                {
                    text = text.Replace("\n", "");
                }
                else if (tag == "br")
                {
                    text = text.Replace("\n", "<br/>");
                }
                else
                {
                    text = $"<{tag}>{text.Replace("\n", $"</{tag}><{tag}>")}</{tag}>";
                }
            }
            return new HtmlString(text);
        }

        public static string HtmlForSize(this WidgetFactory html, string text, int size)
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
        #endregion

        public static string GetAppUrl(this WidgetFactory ace, string url, params string[] appendQueries)
        {
            url = $"/{ace.AppName}/{url}";
            foreach (var query in appendQueries)
            {
                url = UrlHelper.Append(url, query, App.GetQuery(query, ""));
            }
            return url;
        }

		public static IHtmlContent RenderInitScripts(this WidgetFactory ace, bool renderTags = true)
		{
			var sb = new StringBuilder();
			if (renderTags)
			{
				sb.Append("<script>");
			}

            var settings = App.AppConfig.Settings;
			var loginUrl = settings.GetValue("auth.loginurl", "/plat/account/login");
			var logoutUrl = settings.GetValue("auth.logouturl", "/plat/account/logout");
            var webRoot = App.GetWebRoot();

			sb.Append($"AX.init({{root:'{webRoot}',path:'{ace.Path}',app:'{ace.AppName}'");
			if (ace.RenderAuthOptions)
			{
				sb.Append($",loginUrl:'{loginUrl}',logoutUrl:'{logoutUrl}'");
			}
			sb.Append("});");

            // append script init.
			sb.Append(App.Context.GetInitScripts());

			if (renderTags)
			{
				sb.Append("</script>");
			}
			return new HtmlString(sb.ToString());
		}
	}
}
