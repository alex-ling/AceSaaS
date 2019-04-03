using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.IoT.Models
{
    public class IotParam
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        /// <summary>I(整形) F(数字) B(布尔) C(字节位) P(百分比)</summary>
        public string Type { get; set; }
        public string Unit { get; set; }
        public decimal? Rate { get; set; }
    }
}