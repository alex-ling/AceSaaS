using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Acesoft.Web.UI.Html
{
	public class HtmlNode : IHtmlNode
	{
		private readonly TagBuilder tagBuilder;

		private List<Action<TextWriter>> templates;

		public TagRenderMode RenderMode
		{
			get;
			private set;
		}

		public string TagName => tagBuilder.TagName;

		public IList<IHtmlNode> Children
		{
			get;
			private set;
		}

		public IList<IHtmlNode> PrevSibings
		{
			get;
			private set;
		}

		public IList<IHtmlNode> NextSibings
		{
			get;
			private set;
		}

		public HtmlNode(string tagName)
			: this(tagName, TagRenderMode.Normal)
		{
		}

		public HtmlNode(string tagName, TagRenderMode renderMode)
		{
			tagBuilder = new TagBuilder(tagName);
			templates = new List<Action<TextWriter>>();
			Children = new List<IHtmlNode>();
			PrevSibings = new List<IHtmlNode>();
			NextSibings = new List<IHtmlNode>();
			RenderMode = renderMode;
		}

		public IDictionary<string, string> Attributes()
		{
			return tagBuilder.Attributes;
		}

		public string Attribute(string key)
		{
			return Attributes()[key];
		}

		public IHtmlNode ToggleAttribute(string key, string value, bool condition)
		{
			if (condition)
			{
				Attribute(key, value, true);
			}
			return this;
		}

		public IHtmlNode Attribute(string key, string value, bool replaceExisting = true)
		{
			tagBuilder.MergeAttribute(key, value, replaceExisting);
			return this;
		}

		public IHtmlNode RemoveAttribute(string key)
		{
			tagBuilder.Attributes.Remove(key);
			return this;
		}

		public IHtmlNode Attributes(object attributes)
		{
			Attributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes), true);
			return this;
		}

		public IHtmlNode Attributes<TKey, TValue>(IDictionary<TKey, TValue> values, bool replaceExisting = true)
		{
			tagBuilder.MergeAttributes(values, replaceExisting);
			return this;
		}

		public IHtmlNode AddClass(params string[] classes)
		{
			classes.Each(delegate(string cls)
			{
				tagBuilder.AddCssClass(cls);
			});
			return this;
		}

		public IHtmlNode PrependClass(params string[] classes)
		{
			classes.Reverse().Each(delegate(string cls)
			{
				tagBuilder.AddCssClass(cls);
			});
			return this;
		}

		public IHtmlNode ToggleClass(string @class, bool condition)
		{
			if (condition)
			{
				AddClass(@class);
			}
			return this;
		}

		public IHtmlNode Css(IDictionary<string, object> styles)
		{
			styles.Each(delegate(KeyValuePair<string, object> p)
			{
				Css(p.Key, p.Value);
			});
			return this;
		}

		public IHtmlNode Css(string key, object value)
		{
			if (value != null)
			{
				if (Attributes().TryGetValue("style", out string value2))
				{
					Attributes()["style"] = value2 + ";" + key + ":" + value;
				}
				else
				{
					Attributes()["style"] = key + ":" + value;
				}
			}
			return this;
		}

		public IHtmlNode ToggleCss(string key, string value, bool condition)
		{
			if (condition)
			{
				Css(key, value);
			}
			return this;
		}

		public IHtmlNode AppendTo(IHtmlNode parent)
		{
			parent.Children.Add(this);
			return this;
		}

		public IHtmlNode AppendTo(IList<IHtmlNode> list)
		{
			list.Add(this);
			return this;
		}

		public IHtmlNode Text(string text)
		{
			return Template(delegate(TextWriter writer)
			{
				writer.Write(text);
			});
		}

		public IHtmlNode Template(Action<TextWriter> value)
		{
			templates.Add(value);
			return this;
		}

		public List<Action<TextWriter>> Template()
		{
			return templates;
		}

		public void Render(TextWriter writer)
		{
			if (RenderMode != TagRenderMode.SelfClosing)
			{
				writer.Write(tagBuilder.RenderStartTag().ToHtml());
				if (templates.Any())
				{
					foreach (Action<TextWriter> template in templates)
					{
						template(writer);
					}
				}
				if (Children.Any())
				{
					Children.Each(delegate(IHtmlNode child)
					{
						child.Render(writer);
					});
				}
				else if (tagBuilder.HasInnerHtml)
				{
					writer.Write(tagBuilder.InnerHtml.ToHtml());
				}
				else
				{
					writer.Write(tagBuilder.RenderBody().ToHtml());
				}
				writer.Write(tagBuilder.RenderEndTag().ToHtml());
			}
			else
			{
				writer.Write(tagBuilder.RenderSelfClosingTag().ToHtml());
			}
		}
	}
}
