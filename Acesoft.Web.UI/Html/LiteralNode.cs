using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace Acesoft.Web.UI.Html
{
	public class LiteralNode : IHtmlNode
	{
		public string Content { get; set; }

		public IList<IHtmlNode> Children
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public TagRenderMode RenderMode
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string TagName
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IList<IHtmlNode> NextSibings
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IList<IHtmlNode> PrevSibings
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public LiteralNode(string content)
		{
			Content = content;
		}

		public override string ToString()
		{
			return Content;
		}

		public IHtmlNode AddClass(params string[] classes)
		{
			throw new NotImplementedException();
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

		public void Render(TextWriter writer)
		{
			writer.Write(Content);
		}

		public string Attribute(string key)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Attribute(string key, string value)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Attribute(string key, string value, bool replaceExisting)
		{
			throw new NotImplementedException();
		}

		public IDictionary<string, string> Attributes()
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Attributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Attributes(object attributes)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Attributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Css(IDictionary<string, object> styles)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Css(string key, object value)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Html(string value)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode PrependClass(params string[] classes)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode RemoveAttribute(string key)
		{
			throw new NotImplementedException();
		}

		public List<Action<TextWriter>> Template()
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Template(Action<TextWriter> value)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode Text(string value)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode ToggleAttribute(string key, string value, bool condition)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode ToggleClass(string @class, bool condition)
		{
			throw new NotImplementedException();
		}

		public IHtmlNode ToggleCss(string key, string value, bool condition)
		{
			throw new NotImplementedException();
		}
	}
}
