using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class Slider : WidgetBase
	{
		public static readonly ScriptEvent OnTipFormatter = new ScriptEvent("tipFormatter", "");

		public static readonly ScriptEvent OnConverter = new ScriptEvent("converter", "");

		public static readonly ScriptEvent OnChange = new ScriptEvent("onChange", "nv,ov");

		public static readonly ScriptEvent OnSlideStart = new ScriptEvent("onSlideStart", "value");

		public static readonly ScriptEvent OnSlideEnd = new ScriptEvent("onSlideEnd", "value");

		public static readonly ScriptEvent OnComplete = new ScriptEvent("onComplete", "value");

		public int? Width
		{
			get;
			set;
		}

		public int? Height
		{
			get;
			set;
		}

		public Direction? Mode
		{
			get;
			set;
		}

		public bool? Reversed
		{
			get;
			set;
		}

		public bool? ShowTip
		{
			get;
			set;
		}

		public bool? Disabled
		{
			get;
			set;
		}

		public bool? Range
		{
			get;
			set;
		}

		public int? Value
		{
			get;
			set;
		}

		public int? Min
		{
			get;
			set;
		}

		public int? Max
		{
			get;
			set;
		}

		public int? Step
		{
			get;
			set;
		}

		public IList<string> Rule
		{
			get;
			set;
		}

		public Slider(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "slider";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			throw new NotImplementedException();
		}
	}
}
