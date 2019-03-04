using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace Acesoft.Web.UI.Html
{
	public interface IHtmlNode
	{
		IList<IHtmlNode> Children
		{
			get;
		}

		IList<IHtmlNode> PrevSibings
		{
			get;
		}

		IList<IHtmlNode> NextSibings
		{
			get;
		}

		TagRenderMode RenderMode
		{
			get;
		}

		string TagName
		{
			get;
		}

		IHtmlNode AddClass(params string[] classes);

		IHtmlNode AppendTo(IHtmlNode parent);

		IHtmlNode AppendTo(IList<IHtmlNode> parent);

		string Attribute(string key);

		IHtmlNode Attribute(string key, string value, bool replaceExisting = true);

		IDictionary<string, string> Attributes();

		IHtmlNode Attributes(object attributes);

		IHtmlNode Attributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting = true);

		IHtmlNode Css(IDictionary<string, object> styles);

		IHtmlNode Css(string key, object value);

		IHtmlNode PrependClass(params string[] classes);

		IHtmlNode RemoveAttribute(string key);

		List<Action<TextWriter>> Template();

		IHtmlNode Text(string text);

		IHtmlNode Template(Action<TextWriter> value);

		IHtmlNode ToggleAttribute(string key, string value, bool condition);

		IHtmlNode ToggleClass(string @class, bool condition);

		IHtmlNode ToggleCss(string key, string value, bool condition);

		void Render(TextWriter writer);
	}
}
