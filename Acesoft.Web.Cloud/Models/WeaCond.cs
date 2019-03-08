using System;

namespace Acesoft.Web.Cloud
{
	public class WeaCond
	{
		public string condition { get; set; }
        public string humidity { get; set; }
        public string icon { get; set; }
        public string temp { get; set; }
        public string windDir { get; set; }
        public string windLevel { get; set; }
        public DateTime updatetime { get; set; }

        public string url => "http://h5tq.moji.com/tianqi/assets/images/weather/w" + icon + ".png";
	}
}
