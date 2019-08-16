using System;

using SuperSocket.SocketBase;

namespace Acesoft.IotService
{
	internal class Command
	{
		public string Name { get; set; }
		public string Description { get; set; }
        public Func<IBootstrap, string[], bool> Handler { get; set; }
    }
}
