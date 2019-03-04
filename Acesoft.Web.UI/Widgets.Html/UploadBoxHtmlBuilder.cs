using System.Linq;
using System.Text;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class UploadBoxHtmlBuilder : HiddenBoxHtmlBuilder<UploadBox>
	{
		public UploadBoxHtmlBuilder(UploadBox component)
			: base(component)
		{
			base.RenderType = "aceui";
			base.EventsToOption = true;
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Url.HasValue())
			{
				base.Options["url"] = base.Component.Url;
			}
			if (base.Component.chunk_size.HasValue())
			{
				base.Options["chunk_size"] = base.Component.chunk_size;
			}
			if (base.Component.drop_element.HasValue())
			{
				base.Options["drop_element"] = base.Component.drop_element;
			}
			bool? nullable;
			if (base.Component.filters != null)
			{
				FileFilters filters = base.Component.filters;
				StringBuilder stringBuilder = new StringBuilder();
				if (filters.max_file_size.HasValue())
				{
					stringBuilder.Append(",max_file_size:'" + filters.max_file_size + "'");
				}
				nullable = filters.prevent_duplicates;
				if (nullable.HasValue)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					nullable = filters.prevent_duplicates;
					stringBuilder2.Append(",prevent_duplicates:" + nullable.ToString().ToLower());
				}
				if (filters.mime_types.Any())
				{
					stringBuilder.Append(",mime_types:[");
					for (int i = 0; i < filters.mime_types.Count; i++)
					{
						MineType mineType = filters.mime_types[i];
						stringBuilder.Append(((i > 0) ? "," : "") ?? "");
						stringBuilder.Append("{title:'" + mineType.title + "',extensions:'" + mineType.extensions + "'}");
					}
					stringBuilder.Append("]");
				}
				stringBuilder.Append("}");
				stringBuilder.Remove(1, false).Insert(0, "{");
				base.Options["filters"] = stringBuilder;
			}
			if (base.Component.headers != null)
			{
				base.Options["headers"] = base.Component.headers;
			}
			if (base.Component.max_retries.HasValue)
			{
				base.Options["max_retries"] = base.Component.max_retries;
			}
			if (base.Component.multipart_params != null)
			{
				base.Options["multipart_params"] = base.Component.multipart_params;
			}
			nullable = base.Component.multi_selection;
			if (nullable.HasValue)
			{
				base.Options["multi_selection"] = base.Component.multi_selection;
			}
			if (base.Component.resize != null)
			{
				base.Options["resize"] = base.Component.resize;
			}
			nullable = base.Component.send_chunk_number;
			if (nullable.HasValue)
			{
				base.Options["send_chunk_number"] = base.Component.send_chunk_number;
			}
			nullable = base.Component.send_file_name;
			if (nullable.HasValue)
			{
				base.Options["send_file_name"] = base.Component.send_file_name;
			}
			nullable = base.Component.unique_names;
			if (nullable.HasValue)
			{
				base.Options["unique_names"] = base.Component.unique_names;
			}
			nullable = base.Component.PicView;
			if (nullable.HasValue)
			{
				base.Options["picView"] = base.Component.PicView;
			}
			if (base.Component.PicWidth.HasValue)
			{
				base.Options["picWidth"] = base.Component.PicWidth;
			}
			if (base.Component.PicHeight.HasValue)
			{
				base.Options["picHeight"] = base.Component.PicHeight;
			}
			if (base.Component.Max.HasValue)
			{
				base.Options["max"] = base.Component.Max;
			}
			if (base.Component.OssBucket.HasValue())
			{
				base.Options["bucket"] = base.Component.OssBucket;
			}
			if (base.Component.Dir.HasValue())
			{
				base.Options["dir"] = base.Component.Dir;
			}
			nullable = base.Component.AutoUpload;
			if (nullable.HasValue)
			{
				base.Options["autoUpload"] = base.Component.AutoUpload;
			}
		}
	}
}
