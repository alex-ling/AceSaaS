using System.Collections.Generic;
using System.IO;
using System.Linq;
using Acesoft.Util;
using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Newtonsoft.Json;

namespace Acesoft.Web.UI.Widgets
{
	public class Form : ContentWidgetBase, IDataSourceWidget, IDataBind<object>
	{
		public static readonly ScriptEvent OnSubmit = new ScriptEvent("onSubmit", "param");

		public static readonly ScriptEvent OnProgress = new ScriptEvent("onProgress", "percent");

		public static readonly ScriptEvent OnSuccess = new ScriptEvent("success", "data");

		public static readonly ScriptEvent OnChange = new ScriptEvent("onChange", "target");

		public bool? Novalidate { get; set; }

		public bool? Iframe { get; set; }

		public bool? Ajax { get; set; }

		public bool? Dirty { get; set; }

		public DataSource DataSource { get; set; }

		public void DataBind()
		{
			if (DataSource.FormData != null)
			{
				base.Model = (object)DataSource.FormData;
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
                //#ADD.BGN# 2019-04-17 处理从数据库生成的值转化为JSON
                var dict = ConvertHelper.ObjectToDictionary((object)DataSource.FormData)
                    .ToDictionary(p => p.Key, p =>
                    {
                        if (p.Key.StartsWith("a__"))
                        {
                            // a__表示数据为数组
                            if (p.Value != null && p.Value.ToString().HasValue())
                            {
                                return p.Value.ToString().Split(',');
                            }
                            return new string[0];
                        }
                        return p.Value;
                    });
                //#ADD.END#

				var data = JsonConvert.SerializeObject(dict, new LongConverter(), new BoolConverter());
				writer.Write(base.Serializer.Initialize($"AX.formLoad('#{base.Id}',{data});"));
			}
		}
	}
}
