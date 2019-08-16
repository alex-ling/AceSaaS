using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.UI.Timeline
{
    public class Slide
    {
        public SlideDate Start_date { get; set; }
        public SlideDate End_date { get; set; }
        public SlideText Text { get; set; }
        public SlideMedia Media { get; set; }
        public string Group { get; set; }
        public string Display_date { get; set; }
        public Background Background { get; set; }
        public bool? Autolink { get; set; }
        public string Unique_id { get; set; }
    }
}
