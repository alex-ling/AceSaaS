using System;

namespace Acesoft.Web.UI
{
	public interface IDataBind
	{
		void DataBind();
	}

	public interface IDataBind<T> : IDataBind
	{
		T Data { get; set; }
	}
}
