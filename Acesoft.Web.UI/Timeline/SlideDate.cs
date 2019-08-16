using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.UI.Timeline
{
    public class SlideDate
    {
        public int Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
        public int? Second { get; set; }
        public int? Millsecond { get; set; }
        public string Display_date { get; set; }
    }
}
