using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IO;

namespace Acesoft.Web.UI.Builder
{
	public class ContentBuilder<Widget, Builder> : WidgetBuilder<Widget, Builder> where Widget : ContentWidgetBase where Builder : WidgetBuilder<Widget, Builder>
	{
		public ContentBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder DataItem(dynamic dataItem)
		{
			base.Component.Model = (object)dataItem;
			return this as Builder;
		}

		public virtual Builder Content(string content)
		{
			base.Component.SetWriter(delegate(TextWriter writer)
			{
				writer.Write(content);
			});
			return this as Builder;
		}

		public virtual Builder Content<T>(Func<T, object> content)
		{
			((ContentWidgetBase)base.Component).SetTemplate<T>(content);
			return this as Builder;
		}

		public virtual Builder Content(Func<object, object> content)
		{
			((ContentWidgetBase)base.Component).SetTemplate<object>(content);
			return this as Builder;
		}

		public virtual Builder Write(Action<TextWriter> content)
		{
			base.Component.SetWriter(content);
			return this as Builder;
		}

		public virtual Builder Controls(Action<IList<IHtmlContent>> addAction)
		{
			addAction(base.Component.Controls);
			return this as Builder;
		}
	}
}
