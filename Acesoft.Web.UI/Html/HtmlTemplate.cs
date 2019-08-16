using System;
using System.Data;
using System.IO;

using Microsoft.AspNetCore.Mvc.Razor;
using Acesoft.Data;
using System.Collections.Generic;

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
                if (data is DataTable dt)
                {
                    int index = 1;
                    dt.Rows.Each<DataRow>(row =>
                    {
                        WriteItem(writer, row.ToObject<T>(index++));
                    });
                }
                else if (data is IEnumerable<DataRow> list)
                {
                    int index = 1;
                    list.Each(row =>
                    {
                        WriteItem(writer, row.ToObject<T>(index++));
                    });
                }
                else if (data is DataRow row)
                {
                    WriteItem(writer, row.ToObject<T>());
                }
                else if (data is IEnumerable<T> list2)
                {
                    list2.Each(item =>
                    {
                        WriteItem(writer, item);
                    });
                }
                else
                {
                    WriteItem(writer, (T)data);
                }
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
