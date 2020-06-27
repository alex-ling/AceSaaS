using System;
using System.Collections.Generic;
using System.Text;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Acesoft.Platform.Office
{
    public class WordTag
    {
        public BookmarkStart Bookmark { get; set; }
        public IList<OpenXmlElement> Elements { get; }

        public WordTag()
        {
            this.Elements = new List<OpenXmlElement>();
        }

        public WordTag Add(OpenXmlElement element)
        {
            Elements.Add(element);
            return this;
        }
    }
}
