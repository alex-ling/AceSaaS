using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class ToolbarHtmlBuilder : WidgetHtmlBuilder<Toolbar>
	{
		public ToolbarHtmlBuilder(Toolbar component)
			: base(component, "div")
		{
		}

        protected override void PreBuild()
        {
            base.PreBuild();

            if (Component.Buttons.HasValue())
            {
                Component.Buttons.Split(',').Each(item =>
                {
                    if (item == "-")
                    {
                        Component.Ace.Html("<span class=\"btn-split\"></span>").AppendTo(Component.Controls);
                    }
                    else
                    {
                        var items = item.Split('=');
                        var cmd = items[0].Split('_')[0];
                        var icon = items[0].Split('_').Length > 1 ? items[0].Split('_')[1] : cmd;
                        var text = items[1];

                        Component.Ace.Button().AppendTo(Component.Controls).Text(text).IconCls($"fa fa-{icon}")
                            .Click($"{Component.OnClick}('{cmd}','{text}')");
                    }
                });
            }
        }
    }
}
