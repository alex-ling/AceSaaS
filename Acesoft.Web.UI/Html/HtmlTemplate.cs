using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.IO;

namespace Acesoft.Web.UI.Html
{
	public class HtmlTemplate<T> : IHtmlTemplate
	{
		public Func<T, object> Tempate
		{
			get;
			private set;
		}

		public HtmlTemplate(Func<T, object> template)
		{
			Tempate = template;
		}

		public void Apply(object data, IHtmlNode node)
		{
			node.Template(delegate(TextWriter writer)
			{
				WriteItem(writer, (T)data);
			});
		}

		private void WriteItem(TextWriter writer, T item)
		{
			object obj = Tempate(item);
			HelperResult helperResult = obj as HelperResult;
			if (helperResult != null)
			{
				helperResult.Render(writer);
			}
			else if (obj != null)
			{
				writer.Write(obj.ToString());
			}
		}
	}
	public class HtmlTemplate : IHtmlTemplate
	{
		public Action<TextWriter> Action
		{
			get;
			private set;
		}

		public HtmlTemplate(Action<TextWriter> action)
		{
			Action = action;
		}

		public void Apply(object data, IHtmlNode node)
		{
			node.Template(delegate(TextWriter writer)
			{
				Action(writer);
			});
		}
	}
}
