using System;
using System.Linq;

using SuperSocket.Common;
using SuperSocket.Facility.Protocol;

namespace Acesoft.IotNet.Iot
{
	public class IotReceiveFilter : FixedHeaderReceiveFilter<IotRequest>
	{
        // 8E75A9(3B) + Length(2B)
        public IotReceiveFilter() : base(5)
		{
		}

		protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
		{
			return header[offset + 3] * 256 + header[offset + 4];
		}

		public override IotRequest Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
		{
			var iotRequest = base.Filter(readBuffer, offset, length, toBeCopied, out rest);
			if (iotRequest != null && rest > 0)
			{
				var num = readBuffer.CloneRange(offset + length - rest, rest).ToHex().IndexOf("8E75A9");
				rest = num >= 0 ? (rest - num) : 0;
			}
			return iotRequest;
		}

		protected override IotRequest ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
		{
            var startTag = header.Array.CloneRange(0, IotRequest.StartTag.Length / 2).ToHex();
			if (startTag == IotRequest.StartTag)
			{
				return new IotRequest(startTag, bodyBuffer.CloneRange(offset, length));
			}
			return null;
		}
	}
}
