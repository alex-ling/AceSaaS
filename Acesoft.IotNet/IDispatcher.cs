namespace Acesoft.IotNet
{
	public interface IDispatcher
	{
		bool Dispatch(string server, string mac, string cmd, string data);
	}
}