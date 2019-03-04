using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class UploadBoxBuilder : HiddenBoxBuilder<UploadBox, UploadBoxBuilder>
	{
		public UploadBoxBuilder(UploadBox component)
			: base(component)
		{
		}

		public virtual UploadBoxBuilder ChunkSize(string size)
		{
			base.Component.chunk_size = size;
			return this;
		}

		public virtual UploadBoxBuilder DropElement(string element)
		{
			base.Component.drop_element = element;
			return this;
		}

		public virtual UploadBoxBuilder Filters(Action<FileFilters> action)
		{
			FileFilters fileFilters = new FileFilters();
			action(fileFilters);
			base.Component.filters = fileFilters;
			return this;
		}

		public virtual UploadBoxBuilder Headers(object header)
		{
			base.Component.headers = header;
			return this;
		}

		public virtual UploadBoxBuilder MaxRetries(int max)
		{
			base.Component.max_retries = max;
			return this;
		}

		public virtual UploadBoxBuilder Params(object param)
		{
			base.Component.multipart_params = param;
			return this;
		}

		public virtual UploadBoxBuilder MultiSelect(bool flag = true)
		{
			base.Component.multi_selection = flag;
			return this;
		}

		public virtual UploadBoxBuilder Resize(PictureResize resize)
		{
			base.Component.resize = resize;
			return this;
		}

		public virtual UploadBoxBuilder SendChunkNumber(bool flag = true)
		{
			base.Component.send_chunk_number = flag;
			return this;
		}

		public virtual UploadBoxBuilder SendFileName(bool flag = true)
		{
			base.Component.send_file_name = flag;
			return this;
		}

		public virtual UploadBoxBuilder UniqueNames(bool unique = true)
		{
			base.Component.unique_names = unique;
			return this;
		}

		public virtual UploadBoxBuilder PicView(bool flag = true)
		{
			base.Component.PicView = flag;
			return this;
		}

		public virtual UploadBoxBuilder PicHeight(int picHeight)
		{
			base.Component.PicHeight = picHeight;
			return this;
		}

		public virtual UploadBoxBuilder PicWidth(int picWidth)
		{
			base.Component.PicWidth = picWidth;
			return this;
		}

		public virtual UploadBoxBuilder Max(int max)
		{
			base.Component.Max = max;
			return this;
		}

		public virtual UploadBoxBuilder OssBucket(string bucket)
		{
			base.Component.OssBucket = bucket;
			return this;
		}

		public virtual UploadBoxBuilder Dir(string dir)
		{
			base.Component.Dir = dir;
			return this;
		}

		public virtual UploadBoxBuilder Url(string url)
		{
			base.Component.Url = url;
			return this;
		}

		public virtual UploadBoxBuilder AutoUpload(bool autoUpload = true)
		{
			base.Component.AutoUpload = autoUpload;
			return this;
		}

		public UploadBoxBuilder Events(Action<UploadBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new UploadBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
}
