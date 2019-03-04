using System.Collections.Generic;

namespace Acesoft.Web.UI
{
	public class FileFilters
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
		} = true;


		public List<MineType> mime_types
		{
			get;
		} = new List<MineType>();

	}
}
