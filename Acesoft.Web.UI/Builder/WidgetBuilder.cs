using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace Acesoft.Web.UI.Builder
{
	public abstract class WidgetBuilder<Widget, Builder> : IBuilder, IRender, IHtmlContent where Widget : WidgetBase where Builder : WidgetBuilder<Widget, Builder>, IBuilder
	{
		public Widget Component
		{
			get;
			private set;
		}

		public WidgetBuilder(Widget component)
		{
			Component = component;
		}

		public Builder AppendTo(IList<IHtmlContent> controls)
		{
			controls.Add(this);
			return this as Builder;
		}

		public Builder AppendTo(IList<Widget> controls)
		{
			controls.Add(Component);
			return this as Builder;
		}

		public void AppendTo(IHtmlNode html)
		{
			new LiteralNode(ToHtml()).AppendTo(html);
		}

		public virtual Builder For(string @for)
		{
			Component.For = @for;
			Component.IsNeedScriptable = true;
			return this as Builder;
		}

		public virtual Builder NoHtml()
		{
			Component.IsOnlyScriptable = true;
			return this as Builder;
		}

		public virtual Builder Id(string id)
		{
			Component.Id = id;
			return this as Builder;
		}

		public virtual Builder Name(string name)
		{
			Component.Id = name;
			Component.Name = name;
			return this as Builder;
		}

		public virtual Builder TagName(string tagName)
		{
			Component.TagName = tagName;
			return this as Builder;
		}

		public virtual Builder Title(string title)
		{
			return Attr("title", title);
		}

		public virtual Builder Css(string cls)
		{
			string text = "class";
			if (Component.Attributes.ContainsKey(text))
			{
				IDictionary<string, object> attributes = Component.Attributes;
				string key = text;
				attributes[key] = attributes[key] + " " + cls;
			}
			else
			{
				Component.Attributes[text] = cls;
			}
			return this as Builder;
		}

		public virtual Builder Css(object cssAttributes)
		{
			Component.Styles.Merge(cssAttributes, true);
			return this as Builder;
		}

		public virtual Builder Css(string name, string value)
		{
			Component.Styles[name] = value;
			return this as Builder;
		}

		public virtual Builder Disabled(bool disabled = true)
		{
			if (disabled)
			{
				Component.Attributes["disabled"] = true;
			}
			return this as Builder;
		}

		public virtual Builder Hidden(bool hidden = true)
		{
			if (hidden)
			{
				return Css("display", "none");
			}
			return this as Builder;
		}

		public virtual Builder Width(string width)
		{
			return Css("width", width);
		}

		public virtual Builder Height(string heigth)
		{
			return Css("height", heigth);
		}

		public virtual Builder Attr(string name, object value)
		{
			Component.Attributes[name] = value;
			return this as Builder;
		}

        public virtual Builder Option(string name, object value)
        {
            Component.Options[name] = value;
            return this as Builder;
        }

		public virtual Builder Build(Action<Widget> action)
		{
			action(Component);
			return this as Builder;
		}

		public virtual void Render()
		{
			Component.Render();
		}

		public string ToHtml()
		{
			return Component.ToHtml();
		}

		public override string ToString()
		{
			return ToHtml();
		}

		public void WriteTo(TextWriter writer, HtmlEncoder encoder)
		{
			Component.WriteTo(writer, encoder);
		}
	}
}
