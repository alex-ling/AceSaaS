using Acesoft.Util;

namespace Acesoft.IotNet.Iot
{
	public class IotDevice
	{
		public string Mac { get; set; }
        public byte[] Bytes { get; set; }

        private IotDevice()
		{
		}

		public static IotDevice Load(string mac)
		{
			return new IotDevice
			{
				Mac = mac,
				Bytes = EncodingHelper.HexToBytes(mac)
			};
		}

		public static IotDevice Load(string mac, byte[] bytes)
		{
			return new IotDevice
			{
				Mac = mac,
				Bytes = bytes
			};
		}
	}
}
