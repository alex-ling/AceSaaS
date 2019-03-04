using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Script;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;

namespace Acesoft.Web.UI
{
	public abstract class WidgetBase : IWidget, IScriptable
	{
		private IHtmlBuilder htmlBuilder;

		public string Widget { get; protected set; }
		public string Id { get; set; }
		public string Name { get; set; }
        public string TagName { get; set; }
        public HttpContext Context { get; private set; }
        public WidgetFactory Ace { get; private set; }

        public bool IsNeedScriptable { get; set; }
        public bool IsOnlyScriptable { get; set; }
        public bool NoRenderLabel { get; set; }
        public string For { get; set; }
        public IDictionary<string, object> Attributes { get; private set; }
        public IDictionary<string, object> Events { get; private set; }
        public IDictionary<string, object> Styles { get; private set; }
        public IHtmlBuilder HtmlBuilder
		{
			get
			{
				if (htmlBuilder == null)
				{
					htmlBuilder = GetHtmlBuilder();
				}
				return htmlBuilder;
			}
		}

		public IScriptSerializer Serializer => GetSerializer();

		public void Render()
		{
			Ace.Page.Output.Write(ToHtml());
		}

		public string ToHtml()
		{
			return Extensions.ToHtml(this);
		}

		public void WriteScript(TextWriter writer)
		{
		}

		public void WriteTo(TextWriter writer, HtmlEncoder encoder)
		{
			VerifyProperties();
			if (this is IDataBind)
			{
				((IDataBind)this).DataBind();
			}
			if (!IsOnlyScriptable)
			{
				IHtmlNode html = HtmlBuilder.Build();
				RenderHtml(writer, html);
			}
			if (IsNeedScriptable)
			{
				StringWriter stringWriter = new StringWriter();
				WriteInitScript(stringWriter);
				Context.AppendInitScripts(stringWriter.ToString());
			}
		}

		protected abstract IHtmlBuilder GetHtmlBuilder();

		protected virtual IScriptSerializer GetSerializer()
		{
			return Serizlizer.Scriptor;
		}

		public WidgetBase()
		{
			Attributes = new RouteValueDictionary();
			Events = new Dictionary<string, object>();
			Styles = new Dictionary<string, object>();
		}

		public WidgetBase(WidgetFactory ace) : this()
		{
			Initialize(ace);
		}

		public void Initialize(WidgetFactory ace)
		{
			Ace = ace;
			Context = ace.Context;
		}

		public virtual void VerifyProperties()
		{
		}

		private void RenderHtml(TextWriter writer, IHtmlNode html)
		{
			if (html.PrevSibings.Any())
			{
				html.PrevSibings.Each(delegate(IHtmlNode item)
				{
					item.Render(writer);
				});
			}
			html.Render(writer);
			if (html.NextSibings.Any())
			{
				html.NextSibings.Each(delegate(IHtmlNode item)
				{
					item.Render(writer);
				});
			}
		}

		public virtual void WriteInitScript(TextWriter writer)
		{
			IDictionary<string, object> options = HtmlBuilder.BuildOptions();
			writer.Write(Serializer.InitializeFor(Id, Widget, options));
		}
	}
}
