using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class IdBoxBuilder : TextBoxBuilder<IdBox, IdBoxBuilder>
	{
		public IdBoxBuilder(IdBox component)
			: base(component)
		{
		}

        public virtual IdBoxBuilder Seed(string seed)
        {
            base.Component.Seed = seed;
            return this;
        }

        public virtual IdBoxBuilder Prefix(string prefix)
        {
            base.Component.Prefix = prefix;
            return this;
        }

        public virtual IdBoxBuilder DateExp(string dateExp)
        {
            base.Component.DateExp = dateExp;
            return this;
        }

        public virtual IdBoxBuilder Length(int length)
        {
            base.Component.length = length;
            return this;
        }

        public virtual IdBoxBuilder PreEnabled(bool preEnabled = true)
        {
            base.Component.PreEnabled = preEnabled;
            return this;
        }

        public virtual IdBoxBuilder AutoSave(bool autoSave = true)
        {
            base.Component.AutoSave = autoSave;
            return this;
        }

        public virtual IdBoxBuilder Dynamic(bool dynamic = true)
        {
            base.Component.Dynamic = dynamic;
            return this;
        }

        public virtual IdBoxBuilder Nary(int nary)
        {
            base.Component.Nary = nary;
            return this;
        }

        public virtual IdBoxBuilder NeedLoad(bool needLoad = true)
        {
            base.Component.NeedLoad = needLoad;
            return this;
        }

        public IdBoxBuilder Events(Action<TextBoxEventBuilder> clientEventsAction)
        {
            clientEventsAction(new TextBoxEventBuilder(base.Component.Events));
            return this;
        }
    }
}
