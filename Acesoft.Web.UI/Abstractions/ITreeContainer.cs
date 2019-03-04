using System.Collections.Generic;

namespace Acesoft.Web.UI
{
	public interface ITreeContainer<T> where T : class, ITreeContainer<T>
	{
		IList<T> Nodes { get; }
    }
}
