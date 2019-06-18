using Acesoft.Web.UI.Ajax;

namespace Acesoft.Web.UI.Widgets.Html
{
    public class SelectButtonHtmlBuilder : LinkButtonHtmlBuilder<SelectButton>
    {
        public SelectButtonHtmlBuilder(SelectButton component)
            : base(component)
        {
        }

        protected override void PreBuild()
        {
            base.PreBuild();

            if (Component.SelectUrl.HasValue())
            {
                Options["url"] = Component.SelectUrl;
            }
            if (Component.ValueBox.HasValue())
            {
                Options["valBox"] = Component.ValueBox;
            }
            if (Component.TextBox.HasValue())
            {
                Options["txtBox"] = Component.TextBox;
            }
            if (Component.DialogHeight.HasValue)
            {
                Options["dialogHeight"] = Component.DialogHeight;
            }
            if (Component.DialogWidth.HasValue)
            {
                Options["dialogWidth"] = Component.DialogWidth;
            }
        }
    }
}