using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IO;

namespace Acesoft.Web.UI
{
	public abstract class ContentWidgetBase : WidgetBase, IContentWidget
	{
		public object Data
		{
			get;
			set;
		}

		public IHtmlTemplate Template
		{
			get;
			set;
		}

		public IList<IHtmlContent> Controls
		{
			get;
			private set;
		}

		public virtual void SetTemplate<T>(Func<T, object> template)
		{
			Template = new HtmlTemplate<T>(template);
		}

		public virtual void SetWriter(Action<TextWriter> writer)
		{
			Template = new HtmlTemplate(writer);
		}

		public ContentWidgetBase(WidgetFactory ace)
			: base(ace)
		{
			Controls = new List<IHtmlContent>();
		}
	}
}
