namespace Acesoft.Web.UI.Html
{
	public interface IHtmlTemplate
	{
		void Apply(object data, IHtmlNode node);
	}
}
