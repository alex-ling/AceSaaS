using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Acesoft.Data;
using NPOI.XWPF.UserModel;

namespace Acesoft.Platform.Office
{
    public class DocReport : IOfficeReport
    {
        private readonly string tempFile;
        private readonly GridResponse res;
        private bool autoHeight = false;

        public XWPFDocument Document { get; private set; }

        public DocReport(GridResponse res, string tempFile, bool autoHeight)
        {
            this.res = res;
            this.tempFile = tempFile;
            this.autoHeight = autoHeight;
        }

        public byte[] Export()
        {
            using (var fs = File.OpenRead(tempFile))
            {
                Document = new XWPFDocument(fs);
            }

            Render();

            using (var memoryStream = new MemoryStream())
            {
                Document.Write(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private void Render()
        {
            //遍历段落                 
            foreach (var p in Document.Paragraphs)
            {
                ReplaceParagraph(p, null);
            }

            //遍历表格     
            foreach (var table in Document.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var p in cell.Paragraphs)
                        {
                            ReplaceParagraph(p, row);
                        }
                    }
                }
            }
        }

        private void ReplaceParagraph(XWPFParagraph p1, object p2)
        {
            throw new NotImplementedException();
        }
    }
}
