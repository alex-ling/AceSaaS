using System;
using System.Collections.Generic;
using System.Text;

using SuperSocket.ProtoBase;

namespace Acesoft.Web.IoT.Client
{
    public class IotReceiveFilter : CountSpliterReceiveFilter<IotRequest>
    {
        public IotReceiveFilter() : base(new byte[] { (byte)'#' }, 5)
        {
        }

        public override IotRequest ResolvePackage(IBufferStream bufferStream)
        {
            return new IotRequest(bufferStream.ReadString((int)bufferStream.Length, Encoding.UTF8));
        }
    }
}
