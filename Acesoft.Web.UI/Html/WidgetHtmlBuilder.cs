using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Html
{
	public class WidgetHtmlBuilder<Widget> : IHtmlBuilder where Widget : WidgetBase
	{
		public Widget Component { get; private set; }
		public string TagName { get; set; }
		public string RenderType { get; set; }
        public TagRenderMode RenderMode { get; set; }
        protected bool PreBuildExecuted { get; private set; }
        public bool EventsToOption { get; set; }
        public IDictionary<string, object> Options { get; set; }

        public WidgetHtmlBuilder(Widget component, string tagName)
			: this(component, tagName, TagRenderMode.Normal)
		{
		}

		public WidgetHtmlBuilder(Widget component, string tagName, TagRenderMode renderMode)
		{
			Component = component;
			TagName = (component.TagName ?? tagName);
			RenderMode = renderMode;
			RenderType = "aceui";
			EventsToOption = true;
			Options = new Dictionary<string, object>();
		}

		protected virtual void OnLoaded()
		{
		}

		protected virtual void PreBuild()
		{
			BuildIdAndName();
			PreBuildExecuted = true;
		}

		protected virtual void BuildIdAndName()
		{
			if (Component.Id.HasValue())
			{
				Component.Attributes["id"] = Component.Id;
			}
			if (Component.Name.HasValue())
			{
				Component.Attributes["name"] = Component.Name;
			}
		}

		public virtual IHtmlNode Build()
		{
			OnLoaded();
			if (!PreBuildExecuted)
			{
				PreBuild();
			}

			var htmlNode = new HtmlNode(TagName, RenderMode).Attributes(Component.Attributes).Css(Component.Styles);
			if (Component.Widget.HasValue())
			{
				htmlNode.AddClass(RenderType + "-" + Component.Widget);
			}
			if (!EventsToOption && Component.Events.Any())
			{
				htmlNode.Attributes(Component.Events);
			}
			if (Component is IContentWidget contentWidget)
			{
				BuildContent(contentWidget, htmlNode);
			}
			return htmlNode;
		}

		protected virtual void BuildContent(IContentWidget content, IHtmlNode html)
		{
			if (content.Template != null)
			{
				content.Template.Apply(content.Model, html);
			}
			else if (content.Controls.Any())
			{
                content.Controls.Each(c =>
				{
					new LiteralNode(c.ToHtml()).AppendTo(html);
				});
			}
		}

		public virtual IDictionary<string, object> BuildOptions()
		{
			if (!PreBuildExecuted)
			{
				PreBuild();
			}

			var dictionary = new Dictionary<string, object>(Options);
            if (Component.Options.Any())
            {
                dictionary.Merge(Component.Options);
            }

			if (EventsToOption && Component.Events.Any())
			{
                dictionary.Merge(Component.Events);
			}

			var dsWidget = Component as IDataSourceWidget;
			if (dsWidget != null && dsWidget.DataSource.Dynamic)
			{
				dictionary.Merge(dsWidget.DataSource.ToJson());
			}
			return dictionary;
		}
	}
}
