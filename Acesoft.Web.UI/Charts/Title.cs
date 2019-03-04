namespace Acesoft.Web.UI.Charts
{
	public class Title : OptionBase<Title>
	{
		public Title Text(string text)
		{
			base.Options["text"] = text;
			return this;
		}
	}
}
