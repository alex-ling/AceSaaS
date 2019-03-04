using Acesoft.Web.UI.Html;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class KindEditorHtmlBuilder : EasyUIHtmlBuilder<KindEditor>
	{
		public KindEditorHtmlBuilder(KindEditor component)
			: base(component, "textarea")
		{
			base.RenderType = "aceui";
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component._MinWidth.HasValue())
			{
				base.Options["minWidth"] = base.Component._MinWidth;
			}
			if (base.Component._MinHeight.HasValue())
			{
				base.Options["minHeight"] = base.Component._MinHeight;
			}
			if (base.Component._Items.Any())
			{
				base.Options["items"] = base.Component._Items;
			}
			if (base.Component._NoDisableItems.Any())
			{
				base.Options["noDisableItems"] = base.Component._NoDisableItems;
			}
			if (base.Component._FilterMode.HasValue)
			{
				base.Options["filterMode"] = base.Component._FilterMode;
			}
			if (base.Component._HtmlTags != null)
			{
				base.Options["htmlTags"] = (object)base.Component._HtmlTags;
			}
			if (base.Component._WellFormatMode.HasValue)
			{
				base.Options["wellFormatMode"] = base.Component._WellFormatMode;
			}
			if (base.Component._ResizeType.HasValue)
			{
				base.Options["resizeType"] = (int)base.Component._ResizeType.Value;
			}
			if (base.Component._ThemeType.HasValue())
			{
				base.Options["themeType"] = base.Component._ThemeType;
			}
			if (base.Component._LangType.HasValue())
			{
				base.Options["langType"] = base.Component._LangType;
			}
			if (base.Component._DesignMode.HasValue)
			{
				base.Options["designMode"] = base.Component._DesignMode;
			}
			if (base.Component._FullscreenMode.HasValue)
			{
				base.Options["fullscreenMode"] = base.Component._FullscreenMode;
			}
			if (base.Component._BasePath.HasValue())
			{
				base.Options["basePath"] = base.Component._BasePath;
			}
			if (base.Component._ThemesPath.HasValue())
			{
				base.Options["themesPath"] = base.Component._ThemesPath;
			}
			if (base.Component._PluginsPath.HasValue())
			{
				base.Options["pluginsPath"] = base.Component._PluginsPath;
			}
			if (base.Component._LangPath.HasValue())
			{
				base.Options["langPath"] = base.Component._LangPath;
			}
			if (base.Component._MinChangeSize.HasValue)
			{
				base.Options["minChangeSize"] = base.Component._MinChangeSize;
			}
			if (base.Component._UrlType.HasValue())
			{
				base.Options["urlType"] = base.Component._UrlType;
			}
			if (base.Component._NewlineTag.HasValue())
			{
				base.Options["newlineTag"] = base.Component._NewlineTag;
			}
			if (base.Component._PasteType.HasValue)
			{
				base.Options["pasteType"] = (int)base.Component._PasteType.Value;
			}
			if (base.Component._DialogAlignType.HasValue())
			{
				base.Options["dialogAlignType"] = base.Component._DialogAlignType;
			}
			if (base.Component._ShadowMode.HasValue)
			{
				base.Options["shadowMode"] = base.Component._ShadowMode;
			}
			if (base.Component._ZIndex.HasValue)
			{
				base.Options["zIndex"] = base.Component._ZIndex;
			}
			if (base.Component._UseContextmenu.HasValue)
			{
				base.Options["useContextmenu"] = base.Component._UseContextmenu;
			}
			if (base.Component._SyncType.HasValue())
			{
				base.Options["syncType"] = base.Component._SyncType;
			}
			if (base.Component._IndentChar.HasValue())
			{
				base.Options["indentChar"] = base.Component._IndentChar;
			}
			if (base.Component._CssPath.HasValue())
			{
				base.Options["cssPath"] = base.Component._CssPath;
			}
			if (base.Component._CssData.HasValue())
			{
				base.Options["cssData"] = base.Component._CssData;
			}
			if (base.Component._BodyClass.HasValue())
			{
				base.Options["bodyClass"] = base.Component._BodyClass;
			}
			if (base.Component._ColorTable.Any())
			{
				base.Options["colorTable"] = base.Component._ColorTable;
			}
			if (base.Component._UploadJson.HasValue())
			{
				base.Options["uploadJson"] = base.Component._UploadJson;
			}
			if (base.Component._FileManagerJson.HasValue())
			{
				base.Options["fileManagerJson"] = base.Component._FileManagerJson;
			}
			if (base.Component._AllowPreviewEmoticons.HasValue)
			{
				base.Options["allowPreviewEmoticons"] = base.Component._AllowPreviewEmoticons;
			}
			if (base.Component._AllowImageUpload.HasValue)
			{
				base.Options["allowImageUpload"] = base.Component._AllowImageUpload;
			}
			if (base.Component._AllowFlashUpload.HasValue)
			{
				base.Options["allowFlashUpload"] = base.Component._AllowFlashUpload;
			}
			if (base.Component._AllowMediaUpload.HasValue)
			{
				base.Options["allowMediaUpload"] = base.Component._AllowMediaUpload;
			}
			if (base.Component._AllowFileUpload.HasValue)
			{
				base.Options["allowFileUpload"] = base.Component._AllowFileUpload;
			}
			if (base.Component._AllowFileManager.HasValue)
			{
				base.Options["allowFileManager"] = base.Component._AllowFileManager;
			}
			if (base.Component._FontSizeTable.Any())
			{
				base.Options["fontSizeTable"] = base.Component._FontSizeTable;
			}
			if (base.Component._ImageTabIndex.HasValue)
			{
				base.Options["imageTabIndex"] = base.Component._ImageTabIndex;
			}
			if (base.Component._FormatUploadUrl.HasValue)
			{
				base.Options["formatUploadUrl"] = base.Component._FormatUploadUrl;
			}
			if (base.Component._FullscreenShortcut.HasValue)
			{
				base.Options["fullscreenShortcut"] = base.Component._FullscreenShortcut;
			}
			if (base.Component._ExtraFileUploadParams != null)
			{
				base.Options["extraFileUploadParams"] = (object)base.Component._ExtraFileUploadParams;
			}
			if (base.Component._FilePostName.HasValue())
			{
				base.Options["filePostName"] = base.Component._FilePostName;
			}
			if (base.Component._FillDescAfterUploadImage.HasValue)
			{
				base.Options["fillDescAfterUploadImage"] = base.Component._FillDescAfterUploadImage;
			}
			if (base.Component._PagebreakHtml.HasValue())
			{
				base.Options["pagebreakHtml"] = base.Component._PagebreakHtml;
			}
			if (base.Component._AllowImageRemote.HasValue)
			{
				base.Options["allowImageRemote"] = base.Component._AllowImageRemote;
			}
			if (base.Component._AutoHeightMode.HasValue)
			{
				base.Options["autoHeightMode"] = base.Component._AutoHeightMode;
			}
		}
	}
}
