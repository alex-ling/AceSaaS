using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class IdBoxHtmlBuilder : TextBoxHtmlBuilder<IdBox>
	{
		public IdBoxHtmlBuilder(IdBox component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
		}

        public override IHtmlNode Build()
        {
            return base.Build().If(Component.Value.HasValue(), b =>
            {
                b.Attribute("value", Component.Value);
            });
        }
    }
}
