using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class KindEditor : WidgetBase
	{
		public static readonly ScriptEvent OnAfterCreate = new ScriptEvent("afterCreate", "");

		public static readonly ScriptEvent OnAfterChange = new ScriptEvent("afterChange", "");

		public static readonly ScriptEvent OnAfterTab = new ScriptEvent("afterTab", "");

		public static readonly ScriptEvent OnAfterFocus = new ScriptEvent("afterFocus", "");

		public static readonly ScriptEvent OnAfterBlur = new ScriptEvent("afterBlur", "");

		public static readonly ScriptEvent OnAfterUpload = new ScriptEvent("afterUpload", "");

		public static readonly ScriptEvent OnAfterSelectFile = new ScriptEvent("afterSelectFile", "");

		public string _MinWidth
		{
			get;
			set;
		}

		public string _MinHeight
		{
			get;
			set;
		}

		public List<string> _Items
		{
			get;
			set;
		}

		public List<string> _NoDisableItems
		{
			get;
			set;
		}

		public bool? _FilterMode
		{
			get;
			set;
		}

		public dynamic _HtmlTags
		{
			get;
			set;
		}

		public bool? _WellFormatMode
		{
			get;
			set;
		}

		public ResizeType? _ResizeType
		{
			get;
			set;
		}

		public string _ThemeType
		{
			get;
			set;
		}

		public string _LangType
		{
			get;
			set;
		}

		public bool? _DesignMode
		{
			get;
			set;
		}

		public bool? _FullscreenMode
		{
			get;
			set;
		}

		public string _BasePath
		{
			get;
			set;
		}

		public string _ThemesPath
		{
			get;
			set;
		}

		public string _PluginsPath
		{
			get;
			set;
		}

		public string _LangPath
		{
			get;
			set;
		}

		public int? _MinChangeSize
		{
			get;
			set;
		}

		public string _UrlType
		{
			get;
			set;
		}

		public string _NewlineTag
		{
			get;
			set;
		}

		public PasteType? _PasteType
		{
			get;
			set;
		}

		public string _DialogAlignType
		{
			get;
			set;
		}

		public bool? _ShadowMode
		{
			get;
			set;
		}

		public int? _ZIndex
		{
			get;
			set;
		}

		public bool? _UseContextmenu
		{
			get;
			set;
		}

		public string _SyncType
		{
			get;
			set;
		}

		public string _IndentChar
		{
			get;
			set;
		}

		public string _CssPath
		{
			get;
			set;
		}

		public string _CssData
		{
			get;
			set;
		}

		public string _BodyClass
		{
			get;
			set;
		}

		public List<string> _ColorTable
		{
			get;
			set;
		}

		public string _UploadJson
		{
			get;
			set;
		}

		public string _FileManagerJson
		{
			get;
			set;
		}

		public bool? _AllowPreviewEmoticons
		{
			get;
			set;
		}

		public bool? _AllowImageUpload
		{
			get;
			set;
		}

		public bool? _AllowFlashUpload
		{
			get;
			set;
		}

		public bool? _AllowMediaUpload
		{
			get;
			set;
		}

		public bool? _AllowFileUpload
		{
			get;
			set;
		}

		public bool? _AllowFileManager
		{
			get;
			set;
		}

		public List<string> _FontSizeTable
		{
			get;
			set;
		}

		public int? _ImageTabIndex
		{
			get;
			set;
		}

		public bool? _FormatUploadUrl
		{
			get;
			set;
		}

		public bool? _FullscreenShortcut
		{
			get;
			set;
		}

		public dynamic _ExtraFileUploadParams
		{
			get;
			set;
		}

		public string _FilePostName
		{
			get;
			set;
		}

		public bool? _FillDescAfterUploadImage
		{
			get;
			set;
		}

		public string _PagebreakHtml
		{
			get;
			set;
		}

		public bool? _AllowImageRemote
		{
			get;
			set;
		}

		public bool? _AutoHeightMode
		{
			get;
			set;
		}

		public KindEditor(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "kindeditor";
			_Items = new List<string>();
			_NoDisableItems = new List<string>();
			_FontSizeTable = new List<string>();
			_ColorTable = new List<string>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new KindEditorHtmlBuilder(this);
		}
	}
}
