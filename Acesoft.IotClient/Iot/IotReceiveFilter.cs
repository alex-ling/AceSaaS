using System;
using System.Linq;

using Acesoft.Security;
using SuperSocket.ProtoBase;

namespace Acesoft.IotNet.Iot
{
	public class IotReceiveFilter : FixedHeaderReceiveFilter<IotRequest>
	{
        private readonly string header;
        private readonly int headerLength;
        private readonly IByteCrypto crypto;

        public string Header => header;
        public IByteCrypto Crypto => crypto;

        public IotReceiveFilter(string header, int cryptoKey) : base((header.Length / 2) + 2)
		{
            this.header = header;
            this.headerLength = header.Length / 2;
            this.crypto = new SwapByteCrypto(cryptoKey);
		}

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            return bufferStream.Skip(headerLength).ReadInt16();
        }

        public override IotRequest ResolvePackage(IBufferStream bufferStream)
        {
            var header = new byte[headerLength + 2];
            bufferStream.Read(header, 0, header.Length);
            var headerHex = header.ToHex();
            if (headerHex.Left(2 * headerLength) == this.header)
            {
                var body = new byte[base.Size - 2 - headerLength];
                bufferStream.Read(body, 0, body.Length);
                return new IotRequest(this, body);
            }
            return null;
        }
    }
}
