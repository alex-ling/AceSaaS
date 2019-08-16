using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.UI.Timeline
{
    public class TimelineV3
    {
        public List<Slide> Events { get; set; }
        public Slide Title { get; set; }
        public List<Era> Eras { get; set; }

        public TimelineV3()
        {
            this.Events = new List<Slide>();
            this.Eras = new List<Era>();
        }
    }
}
