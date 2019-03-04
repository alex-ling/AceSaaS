using Acesoft.Web.UI.Builder;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class KindEditorBuilder : WidgetBuilder<KindEditor, KindEditorBuilder>
	{
		public KindEditorBuilder(KindEditor component)
			: base(component)
		{
		}

		public virtual KindEditorBuilder MinWidth(string minWidth)
		{
			base.Component._MinWidth = minWidth;
			return this;
		}

		public virtual KindEditorBuilder MinHeight(string minHeight)
		{
			base.Component._MinHeight = minHeight;
			return this;
		}

		public virtual KindEditorBuilder Items(List<string> items)
		{
			base.Component._Items = items;
			return this;
		}

		public virtual KindEditorBuilder NoDisableItems(List<string> noDisableItems)
		{
			base.Component._NoDisableItems = noDisableItems;
			return this;
		}

		public virtual KindEditorBuilder FilterMode(bool filterMode = true)
		{
			base.Component._FilterMode = filterMode;
			return this;
		}

		public virtual KindEditorBuilder HtmlTags(dynamic htmlTags)
		{
			base.Component._HtmlTags = (object)htmlTags;
			return this;
		}

		public virtual KindEditorBuilder WellFormatMode(bool wellFormatMode = true)
		{
			base.Component._WellFormatMode = wellFormatMode;
			return this;
		}

		public virtual KindEditorBuilder ResizeType(ResizeType resizeType)
		{
			base.Component._ResizeType = resizeType;
			return this;
		}

		public virtual KindEditorBuilder ThemeType(string themeType)
		{
			base.Component._ThemeType = themeType;
			return this;
		}

		public virtual KindEditorBuilder LangType(string langType)
		{
			base.Component._LangType = langType;
			return this;
		}

		public virtual KindEditorBuilder DesignMode(bool designMode = true)
		{
			base.Component._DesignMode = designMode;
			return this;
		}

		public virtual KindEditorBuilder FullscreenMode(bool fullscreenMode = true)
		{
			base.Component._FullscreenMode = fullscreenMode;
			return this;
		}

		public virtual KindEditorBuilder BasePath(string basePath)
		{
			base.Component._BasePath = basePath;
			return this;
		}

		public virtual KindEditorBuilder ThemesPath(string themesPath)
		{
			base.Component._ThemesPath = themesPath;
			return this;
		}

		public virtual KindEditorBuilder PluginsPath(string pluginsPath)
		{
			base.Component._PluginsPath = pluginsPath;
			return this;
		}

		public virtual KindEditorBuilder LangPath(string langPath)
		{
			base.Component._LangPath = langPath;
			return this;
		}

		public virtual KindEditorBuilder MinChangeSize(int minChangeSize)
		{
			base.Component._MinChangeSize = minChangeSize;
			return this;
		}

		public virtual KindEditorBuilder UrlType(string urlType)
		{
			base.Component._UrlType = urlType;
			return this;
		}

		public virtual KindEditorBuilder NewlineTag(string newlineTag)
		{
			base.Component._NewlineTag = newlineTag;
			return this;
		}

		public virtual KindEditorBuilder PasteType(PasteType pasteType)
		{
			base.Component._PasteType = pasteType;
			return this;
		}

		public virtual KindEditorBuilder DialogAlignType(string dialogAlignType)
		{
			base.Component._DialogAlignType = dialogAlignType;
			return this;
		}

		public virtual KindEditorBuilder ShadowMode(bool shadowMode = true)
		{
			base.Component._ShadowMode = shadowMode;
			return this;
		}

		public virtual KindEditorBuilder ZIndex(int zIndex)
		{
			base.Component._ZIndex = zIndex;
			return this;
		}

		public virtual KindEditorBuilder UseContextmenu(bool useContextmenu = true)
		{
			base.Component._UseContextmenu = useContextmenu;
			return this;
		}

		public virtual KindEditorBuilder SyncType(string syncType)
		{
			base.Component._SyncType = syncType;
			return this;
		}

		public virtual KindEditorBuilder IndentChar(string indentChar)
		{
			base.Component._IndentChar = indentChar;
			return this;
		}

		public virtual KindEditorBuilder CssPath(string cssPath)
		{
			base.Component._CssPath = cssPath;
			return this;
		}

		public virtual KindEditorBuilder CssData(string cssData)
		{
			base.Component._CssData = cssData;
			return this;
		}

		public virtual KindEditorBuilder BodyClass(string bodyClass)
		{
			base.Component._BodyClass = bodyClass;
			return this;
		}

		public virtual KindEditorBuilder ColorTable(List<string> colorTable)
		{
			base.Component._ColorTable = colorTable;
			return this;
		}

		public virtual KindEditorBuilder UploadJson(string uploadJson)
		{
			base.Component._UploadJson = uploadJson;
			return this;
		}

		public virtual KindEditorBuilder UploadFolder(string folder)
		{
			if (!base.Component._UploadJson.HasValue())
			{
				base.Component._UploadJson = "/api/file/postkind";
			}
			if (!base.Component._FileManagerJson.HasValue())
			{
				base.Component._FileManagerJson = "/api/file/getkind";
			}
			KindEditor component = base.Component;
			component._UploadJson = component._UploadJson + "?folder=" + folder;
			KindEditor component2 = base.Component;
			component2._FileManagerJson = component2._FileManagerJson + "?folder=" + folder;
			return this;
		}

		public virtual KindEditorBuilder FileManagerJson(string fileManagerJson)
		{
			base.Component._FileManagerJson = fileManagerJson;
			return this;
		}

		public virtual KindEditorBuilder AllowPreviewEmoticons(bool allowPreviewEmoticons = true)
		{
			base.Component._AllowPreviewEmoticons = allowPreviewEmoticons;
			return this;
		}

		public virtual KindEditorBuilder AllowImageUpload(bool allowImageUpload = true)
		{
			base.Component._AllowImageUpload = allowImageUpload;
			return this;
		}

		public virtual KindEditorBuilder AllowFlashUpload(bool allowFlashUpload = true)
		{
			base.Component._AllowFlashUpload = allowFlashUpload;
			return this;
		}

		public virtual KindEditorBuilder AllowMediaUpload(bool allowMediaUpload = true)
		{
			base.Component._AllowMediaUpload = allowMediaUpload;
			return this;
		}

		public virtual KindEditorBuilder AllowFileUpload(bool allowFileUpload = true)
		{
			base.Component._AllowFileUpload = allowFileUpload;
			return this;
		}

		public virtual KindEditorBuilder AllowFileManager(bool allowFileManager = true)
		{
			base.Component._AllowFileManager = allowFileManager;
			return this;
		}

		public virtual KindEditorBuilder FontSizeTable(List<string> fontSizeTable)
		{
			base.Component._FontSizeTable = fontSizeTable;
			return this;
		}

		public virtual KindEditorBuilder ImageTabIndex(int imageTabIndex)
		{
			base.Component._ImageTabIndex = imageTabIndex;
			return this;
		}

		public virtual KindEditorBuilder FormatUploadUrl(bool formatUploadUrl = true)
		{
			base.Component._FormatUploadUrl = formatUploadUrl;
			return this;
		}

		public virtual KindEditorBuilder FullscreenShortcut(bool fullscreenShortcut = true)
		{
			base.Component._FullscreenShortcut = fullscreenShortcut;
			return this;
		}

		public virtual KindEditorBuilder ExtraFileUploadParams(dynamic extraFileUploadParams)
		{
			base.Component._ExtraFileUploadParams = (object)extraFileUploadParams;
			return this;
		}

		public virtual KindEditorBuilder FilePostName(string filePostName)
		{
			base.Component._FilePostName = filePostName;
			return this;
		}

		public virtual KindEditorBuilder FillDescAfterUploadImage(bool fillDescAfterUploadImage = true)
		{
			base.Component._FillDescAfterUploadImage = fillDescAfterUploadImage;
			return this;
		}

		public virtual KindEditorBuilder PagebreakHtml(string pagebreakHtml)
		{
			base.Component._PagebreakHtml = pagebreakHtml;
			return this;
		}

		public virtual KindEditorBuilder AllowImageRemote(bool allowImageRemote = true)
		{
			base.Component._AllowImageRemote = allowImageRemote;
			return this;
		}

		public virtual KindEditorBuilder AutoHeightMode(bool autoHeightMode = true)
		{
			base.Component._AutoHeightMode = autoHeightMode;
			return this;
		}

		public KindEditorBuilder Events(Action<EditorEventBuilder> clientEventsAction)
		{
			clientEventsAction(new EditorEventBuilder(base.Component.Events));
			return this;
		}
	}
}
