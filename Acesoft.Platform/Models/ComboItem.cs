using System;

namespace Acesoft.Platform.Models
{
	public class DictItem
	{
		public string text { get; set; }
		public string value { get; set; }

        public DictItem(string value)
		{
			this.text = value;
			this.value = value;
		}

		public DictItem(string value, string text)
		{
            this.text = text;
            this.value = value;
		}

		public DictItem(long value, string text)
		{
            this.text = text;
            this.value = value.ToString();
		}
	}
}
