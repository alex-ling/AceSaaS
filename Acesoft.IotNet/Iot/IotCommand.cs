using System.Linq;

using Acesoft.Util;

namespace Acesoft.IotNet.Iot
{
	public class IotCommand
	{
		public string Name { get; set; }
		public int Value { get; set; }
        public byte[] Data { get; set; }
        public string DataHex { get; set; }
        public string DataStr => Data.ToStr();
		public bool IsResponse => (Value & 0x8000) > 0;
		public bool IsSuccess => DataHex.StartsWith("00");
		public string Key => $"{Name}";

		private IotCommand()
		{
		}

		public IotCommand(string command, string dataHex, byte[] data = null)
		{
			Name = command;
			DataHex = dataHex;
			Data = data ?? EncodingHelper.HexToBytes(dataHex);
			Value = NaryHelper.HexToInt(command);
		}

		public IotCommand MakeBack(string dataHex)
		{
			int value = Value | 0x8000;
			return new IotCommand
			{
				Name = value.ToHex(4),
				Data = EncodingHelper.HexToBytes(dataHex),
				DataHex = dataHex,
				Value = value
			};
		}

		public byte[] GetBytes()
		{
			return EncodingHelper.HexToBytes(Name).Concat(Data).ToArray();
		}

		public override string ToString()
		{
			return $"{Name}-{Data.Length.ToString("{00}")}-{DataHex}";
		}
	}
}
