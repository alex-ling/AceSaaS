namespace Acesoft.IotNet
{
	public interface IDispatcher
	{
		bool Dispatch(string mac, string action, string cmd, string data);
	}
}
