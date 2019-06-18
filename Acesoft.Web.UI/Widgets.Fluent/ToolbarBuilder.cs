using Acesoft.Web.UI.Builder;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ToolbarBuilder : ContentBuilder<Toolbar, ToolbarBuilder>
	{
		public ToolbarBuilder(Toolbar component)
			: base(component)
		{
		}

        public virtual ToolbarBuilder Buttons(string buttons)
        {
            Component.Buttons = buttons;
            return this;
        }

        public virtual ToolbarBuilder Click(string handler)
        {
            Component.OnClick = handler;
            return this;
        }
	}
}
