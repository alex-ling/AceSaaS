using System.IO;

using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace Acesoft.Web.UI.Widgets
{
	public class Form : ContentWidgetBase, IDataSourceWidget, IDataBind<object>
	{
		public static readonly ScriptEvent OnSubmit = new ScriptEvent("onSubmit", "param");

		public static readonly ScriptEvent OnProgress = new ScriptEvent("onProgress", "percent");

		public static readonly ScriptEvent OnSuccess = new ScriptEvent("success", "data");

		public static readonly ScriptEvent OnChange = new ScriptEvent("onChange", "target");

		public bool? Novalidate
		{
			get;
			set;
		}

		public bool? Iframe
		{
			get;
			set;
		}

		public bool? Ajax
		{
			get;
			set;
		}

		public bool? Dirty
		{
			get;
			set;
		}

		public DataSource DataSource
		{
			get;
			set;
		}

		public void DataBind()
		{
			if (DataSource.FormData != null)
			{
				base.Data = (object)DataSource.FormData;
			}
		}

		public Form(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "form";
			DataSource = new DataSource(this);
			base.IsNeedScriptable = true;
			Ajax = false;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new FormHtmlBuilder<Form>(this);
		}

		public override void WriteInitScript(TextWriter writer)
		{
			if (DataSource.Url.HasValue())
			{
				base.WriteInitScript(writer);
			}
			if (DataSource.FormData != null)
			{
				object arg = JsonConvert.SerializeObject(DataSource.FormData/*, new LongConverter(), new BoolConverter()*/);
				writer.Write(base.Serializer.Initialize($"AX.formLoad('#{base.Id}',{arg});"));
			}
		}
	}
}
