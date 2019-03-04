using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class ComboBoxHtmlBuilder<Widget> : ComboHtmlBuilder<Widget> where Widget : ComboBox
	{
		public ComboBoxHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.ValueField.HasValue())
			{
				base.Options["ValueField"] = base.Component.ValueField;
			}
			if (base.Component.TextField.HasValue())
			{
				base.Options["TextField"] = base.Component.TextField;
			}
			if (base.Component.GroupField.HasValue())
			{
				base.Options["GroupField"] = base.Component.GroupField;
			}
			if (Enumerable.Any<ComboItem>((IEnumerable<ComboItem>)base.Component.Data))
			{
				base.Options["data"] = base.Component.Data;
			}
			if (base.Component.LimitToList.HasValue)
			{
				base.Options["limitToList"] = base.Component.LimitToList;
			}
			if (base.Component.ShowItemIcon.HasValue)
			{
				base.Options["showItemIcon"] = base.Component.ShowItemIcon;
			}
			if (base.Component.GroupPosition.HasValue)
			{
				base.Options["groupPosition"] = base.Component.GroupPosition;
			}
		}
	}
}
