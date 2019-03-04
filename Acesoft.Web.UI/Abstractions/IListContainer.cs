using System.Collections.Generic;

namespace Acesoft.Web.UI
{
	public interface IListContainer<T> where T : class
	{
		IList<T> Items { get; }
	}
}
