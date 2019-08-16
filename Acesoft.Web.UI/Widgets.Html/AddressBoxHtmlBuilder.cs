namespace Acesoft.Web.UI.Widgets.Html
{
    public class AddressBoxHtmlBuilder : ComboHtmlBuilder<AddressBox>
    {
        public AddressBoxHtmlBuilder(AddressBox component)
            : base(component)
        {
            this.RenderType = "aceui";
        }

        protected override void PreBuild()
        {
            base.PreBuild();

            if (Component.Level.HasValue)
            {
                Options["level"] = Component.Level;
            }
            if (Component.IsInitClick.HasValue)
            {
                Options["isInitClick"] = Component.IsInitClick;
            }
        }
    }
}