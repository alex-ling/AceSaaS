using SuperSocket.Common;
using SuperSocket.SocketBase.Protocol;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Acesoft.Util;
using System.Linq;

namespace Acesoft.IotNet.Iot
{
	public class IotRequest : IRequestInfo
	{
        private readonly IotReceiveFilter filter;

		public string Key { get; set; }
		public byte[] Header { get; set; }
        public string HeaderHex { get; set; }
        public byte[] EncryptedBody { get; set; }
        public byte[] Body { get; set; }
        public string BodyHex { get; set; }
        public int Length { get; set; }
        public string SessionId { get; set; }
        public IotDevice Device { get; set; }
        public IotCommand Command { get; set; }
        public string Crc16 { get; set; }

        private IotRequest(IotReceiveFilter filter)
		{
            this.filter = filter;

            HeaderHex = filter.Header;
			Header = EncodingHelper.HexToBytes(HeaderHex);
		}

		public IotRequest(IotReceiveFilter filter, byte[] body) : this(filter)
		{
			EncryptedBody = body; 

			LoadData();
		}

		private void LoadData()
		{
			Body = filter.Crypto.Decrypt(EncryptedBody);
			BodyHex = Body.ToHex();
			Length = Body.Length;

			SessionId = BodyHex.Substring(0, 4);                        //2b
			var mac = BodyHex.Substring(4, 12);                         //6b
			var bytes = Body.CloneRange(2, 6);                          //6b
			Device = IotDevice.Load(mac, bytes);                        //mac

			var command = BodyHex.Substring(16, 4);                     //2b
			var data = Body.CloneRange(10, Length - 12);                //-12b
			var dataHex = BodyHex.Substring(20, 2 * data.Length);
			Command = new IotCommand(command, dataHex, data);           //cmd

			Crc16 = BodyHex.Right(4);
			Key = Command.Key;
		}

		public bool CheckCrc16()
		{
			var list = new List<byte>();
			list.AddRange(Header);
			list.AddRange(EncodingHelper.HexToBytes(Length.ToHex(4)));
			list.AddRange(Body.CloneRange(0, Length - 2));
			return CrcHelper.GetCrc16_ModBus(list.ToArray()) == Crc16;
		}

		public bool CheckValid()
		{
			return true;
		}

		public bool CheckSession()
		{
			return SessionId == Device.Mac.Right(4);
		}

		public bool CheckSession(ConcurrentDictionary<string, IotSession> sessions, IotSession session)
		{
			IotSession value = null;
			if (sessions.TryGetValue(Device.Mac, out value) && SessionId == value.SessionId)
			{
				if (value != session)
				{
					session.SessionId = value.SessionId;
					session.Device = Device;
					sessions[Device.Mac] = session;
					value.Device = null;
					value.Close();
				}
				return true;
			}
			return false;
		}

		public byte[] BuildBytes()
		{
			var list = new List<byte>();
			list.AddRange(Header);
			list.AddRange(EncodingHelper.HexToBytes(Length.ToHex(4)));
			list.AddRange(EncodingHelper.HexToBytes(SessionId));
			list.AddRange(Device.Bytes);
			list.AddRange(Command.GetBytes());

			Crc16 = CrcHelper.GetCrc16(list.ToArray()).ToHex(4);
			list.AddRange(EncodingHelper.HexToBytes(Crc16));

			Body = list.CloneRange(Header.Length + 2, Length).ToArray();
			BodyHex = Body.ToHex();
			EncryptedBody = filter.Crypto.Encrypt(Body);

			list.RemoveRange(Header.Length + 2, Length);
			list.AddRange(EncryptedBody);

			return list.ToArray();
		}

		public static IotRequest CreateRequest(IotReceiveFilter filter, string mac, string command, string dataHex = "")
		{
			var request = new IotRequest(filter);
			request.Device = IotDevice.Load(mac);
			request.SessionId = request.Device.Mac.Right(4);
			request.Command = new IotCommand(command, dataHex);
			request.Key = request.Command.Key;
			request.Length = request.Command.Data.Length + 12;
			return request;
		}

		public IotRequest CreateRequest(string command, string dataHex = "")
		{
			var request = new IotRequest(filter);
			request.SessionId = SessionId;
			request.Device = Device;
			request.Command = new IotCommand(command, dataHex);
			request.Key = request.Command.Key;
			request.Length = request.Command.Data.Length + 24;
			return request;
        }

        public IotRequest CreateResponse(string dataHex)
        {
            var request = new IotRequest(filter);
            request.SessionId = SessionId;
            request.Device = Device;
            request.Command = Command.MakeBack(dataHex);
            request.Key = request.Command.Key;
            request.Length = request.Command.Data.Length + 12;
            return request;
        }

        public IotRequest Ok(string dataHex = "")
		{
			return CreateResponse($"00{dataHex}");
		}

		public IotRequest ErrorCrc()
		{
			return CreateResponse("01");
		}

		public IotRequest ErrorSession()
		{
			return CreateResponse("02");
		}

		public IotRequest ErrorDevice()
		{
			return CreateResponse("03");
		}

		public IotRequest Error()
		{
			return CreateResponse("FF");
		}
	}
}
