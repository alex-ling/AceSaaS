using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class UploadBox : HiddenBox
	{
		public static readonly ScriptEvent OnBeforeUpload = new ScriptEvent("beforeUpload", "");

		public static readonly ScriptEvent OnUploaded = new ScriptEvent("uploaded", "file,res");

		public string Url
		{
			get;
			set;
		}

		public string chunk_size
		{
			get;
			set;
		}

		public string drop_element
		{
			get;
			set;
		}

		public FileFilters filters
		{
			get;
			set;
		}

		public object headers
		{
			get;
			set;
		}

		public int? max_retries
		{
			get;
			set;
		}

		public object multipart_params
		{
			get;
			set;
		}

		public bool? multi_selection
		{
			get;
			set;
		}

		public PictureResize resize
		{
			get;
			set;
		}

		public bool? send_chunk_number
		{
			get;
			set;
		}

		public bool? send_file_name
		{
			get;
			set;
		}

		public bool? unique_names
		{
			get;
			set;
		}

		public bool? PicView
		{
			get;
			set;
		}

		public int? PicWidth
		{
			get;
			set;
		}

		public int? PicHeight
		{
			get;
			set;
		}

		public int? Max
		{
			get;
			set;
		}

		public string OssBucket
		{
			get;
			set;
		}

		public string Dir
		{
			get;
			set;
		}

		public bool? AutoUpload
		{
			get;
			set;
		}

		public UploadBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "uploadbox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new UploadBoxHtmlBuilder(this);
		}
	}
}
