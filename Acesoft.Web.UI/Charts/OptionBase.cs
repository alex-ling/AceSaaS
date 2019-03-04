using System.Collections.Generic;

namespace Acesoft.Web.UI.Charts
{
	public class OptionBase<T> where T : OptionBase<T>
	{
		public IDictionary<string, object> Options
		{
			get;
			set;
		} = new Dictionary<string, object>();


		public T Option(string name, object value)
		{
			Options[name] = value;
			return this as T;
		}
	}
}
