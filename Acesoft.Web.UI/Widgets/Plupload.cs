using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class Plupload
	{
		public class Resize
		{
			public int width
			{
				get;
				set;
			}

			public int height
			{
				get;
				set;
			}

			public int? quality
			{
				get;
				set;
			}

			public bool? crop
			{
				get;
				set;
			}
		}

		public class Filters
		{
			public string max_file_size
			{
				get;
				set;
			}

			public bool? prevent_duplicates
			{
				get;
				set;
			}

			public List<MineType> mine_types
			{
				get;
			} = new List<MineType>();

		}

		public class MineType
		{
			public string title
			{
				get;
				set;
			}

			public string extensions
			{
				get;
				set;
			}
		}

		public string browse_button
		{
			get;
			set;
		}

		public string chunk_size
		{
			get;
			set;
		} = "0";


		public string container
		{
			get;
			set;
		}

		public string drop_element
		{
			get;
			set;
		}

		public string file_data_name
		{
			get;
			set;
		}

		public Filters filters
		{
			get;
			set;
		}

		public string flash_swf_url
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

		public bool? multipart
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

		public string required_features
		{
			get;
			set;
		}

		public Resize resize
		{
			get;
			set;
		}

		public string runtimes
		{
			get;
			set;
		}

		public string silverlight_xap_url
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

		public string url
		{
			get;
			set;
		}

		public bool? unique_names
		{
			get;
			set;
		}
	}
}
