using System;
using System.Collections.Generic;
using System.Text;

using SuperSocket.ProtoBase;

namespace Acesoft.Web.IoT.Client
{
    public class IotReceiveFilter : CountSpliterReceiveFilter<IotResponse>
    {
        public IotReceiveFilter() : base(new byte[] { (byte)'#' }, 4)
        {
        }

        public override IotResponse ResolvePackage(IBufferStream bufferStream)
        {
            return new IotResponse(bufferStream.ReadString((int)bufferStream.Length, Encoding.UTF8));
        }
    }
}
