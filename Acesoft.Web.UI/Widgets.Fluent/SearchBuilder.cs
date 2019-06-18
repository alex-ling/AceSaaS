using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class SearchBuilder : FormBuilder<Search, SearchBuilder>
	{
		public SearchBuilder(Search component)
			: base(component)
		{
		}

		public SearchBuilder Button(bool button = true)
		{
			base.Component.Button = button;
			return this;
		}

		public SearchBuilder Tools(Action<IList<IHtmlContent>> addAction)
		{
			addAction(base.Component.Tools);
			return this;
		}
	}
}
