using System.Collections.Generic;

namespace Acesoft.Web.UI.Script
{
	public interface IScriptSerializer
	{
		string Serialize(object value);

		string Serialize(string str, bool addQuotes = true);

		string Serialize(IDictionary<string, object> options, bool removeQutes = true);

		string Initialize(string scripts);

		string InitializeFor(string id, string widget, IDictionary<string, object> options = null);
	}
}
