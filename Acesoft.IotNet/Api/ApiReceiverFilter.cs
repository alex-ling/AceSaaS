using SuperSocket.Facility.Protocol;
using Acesoft.Util;

namespace Acesoft.IotNet.Api
{
	public class ApiReceiverFilter : CountSpliterReceiveFilter<ApiRequest>
	{
		public ApiReceiverFilter() : base((byte)'#', 5)
		{
		}

		protected override ApiRequest ProcessMatchedRequest(byte[] readBuffer, int offset, int length)
		{
			return new ApiRequest(readBuffer.CloneRange(offset, length).ToStr());
		}
	}
}
