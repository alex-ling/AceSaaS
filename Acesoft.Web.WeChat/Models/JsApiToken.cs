namespace Acesoft.Web.WeChat
{
	public class JsApiToken
	{
		public string AppId { get; set; }
		public string Ticket { get; set; }
        public string Timestamp { get; set; }
        public string Nonce { get; set; }
        public string Signature { get; set; }
    }
}
