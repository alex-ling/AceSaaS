using Acesoft.Web.UI.Ajax;

namespace Acesoft.Web.UI
{
	public interface IDataSourceWidget : IWidget
	{
		DataSource DataSource { get; set; }
	}
}
