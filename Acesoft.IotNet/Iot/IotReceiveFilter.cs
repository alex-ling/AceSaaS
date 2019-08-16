using System;
using System.Linq;

using Acesoft.Security;
using SuperSocket.Common;
using SuperSocket.Facility.Protocol;

namespace Acesoft.IotNet.Iot
{
	public class IotReceiveFilter : FixedHeaderReceiveFilter<IotRequest>
	{
        private readonly string header;
        private readonly int headerLength;
        private readonly IByteCrypto crypto;

        public string Header => header;
        public IByteCrypto Crypto => crypto;

        public IotReceiveFilter(string header, int cryptoKey) : base((header.Length + 4) / 2)
		{
            this.header = header;
            this.headerLength = header.Length / 2;
            this.crypto = new SwapByteCrypto(cryptoKey);
		}

		protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
		{
			return header[offset + headerLength] * 256 + header[offset + headerLength + 1];
		}

		public override IotRequest Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
		{
			var iotRequest = base.Filter(readBuffer, offset, length, toBeCopied, out rest);
			if (iotRequest != null && rest > 0)
			{
				var num = readBuffer.CloneRange(offset + length - rest, rest).ToHex().IndexOf(header);
				rest = num >= 0 ? (rest - num) : 0;
			}
			return iotRequest;
		}

		protected override IotRequest ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
		{
            var startTag = header.Array.CloneRange(0, this.header.Length / 2).ToHex();
			if (startTag == this.header)
			{
				return new IotRequest(this, bodyBuffer.CloneRange(offset, length));
			}
			return null;
		}
	}
}
