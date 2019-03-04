namespace Acesoft.Web.UI.Widgets
{
	public class ComboItem
	{
		public string text
		{
			get;
		}

		public string value
		{
			get;
		}

		public ComboItem(string value)
		{
			this.text = value;
			this.value = value;
		}

		public ComboItem(string value, string text)
		{
            this.text = text;
            this.value = value;
		}

		public ComboItem(long value, string text)
		{
            this.text = text;
            this.value = value.ToString();
		}
	}
}
