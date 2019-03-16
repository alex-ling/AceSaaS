using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class LabelHtmlBuilder : WidgetHtmlBuilder<Label>
	{
		public LabelHtmlBuilder(Label component)
			: base(component, "span")
		{
        }

        public override IHtmlNode Build()
        {
            var html = base.Build();

            if (Component.Text.HasValue())
            {
                html.Text(Component.Text);
            }

            return html;
        }
    }
}
