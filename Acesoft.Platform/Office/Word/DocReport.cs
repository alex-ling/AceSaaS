using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using HtmlAgilityPack;
using Acesoft.Data;
using Acesoft.Rbac;
using Acesoft.Util;

namespace Acesoft.Platform.Office
{
    public class DocReport : IOfficeReport
    {
        private readonly string tempPath;
        private readonly IDictionary<string, IEnumerable<dynamic>> ds;
        private readonly IDictionary<string, string> props;
        private readonly IDictionary<string, object> vars;

        private List<string> temp = new List<string>();
        private IDictionary<string, WordTag> tags = new Dictionary<string, WordTag>();
        private MainDocumentPart mainDocumentPart;

        public DocReport(string tempPath, 
            IDictionary<string, IEnumerable<dynamic>> ds, 
            IDictionary<string, string> props,
            IDictionary<string, object> vars = null)
        {
            this.tempPath = tempPath;
            this.ds = ds;
            this.props = props;
            this.vars = vars;
        }

        #region export
        public byte[] Export()
        {
            var bytes = File.ReadAllBytes(tempPath);
            using (var ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                using (var doc = WordprocessingDocument.Open(ms, true))
                {
                    // 书签名称
                    mainDocumentPart = doc.MainDocumentPart;
                    var body = doc.MainDocumentPart.Document.Body;
                    var settingsPart = mainDocumentPart.GetPartsOfType<DocumentSettingsPart>().First();

                    // Create object to update fields on open
                    var updateFields = new UpdateFieldsOnOpen();
                    updateFields.Val = new OnOffValue(true);

                    // Insert object into settings part.
                    settingsPart.Settings.PrependChild(updateFields);
                    settingsPart.Settings.Save();

                    IDictionary<string, object> main = null;
                    WordTag currentTag = null;
                    if (ds.ContainsKey("main"))
                    {
                        main = ds["main"].First();
                    }

                    // 遍历根级段落
                    var current = body.FirstChild;
                    do
                    {
                        var bStart = HasBookMark(current);
                        if (currentTag == null && bStart != null)
                        {
                            // 是书签的话，加入列表
                            currentTag = new WordTag { Bookmark = bStart };
                        }
                        else if (current is BookmarkEnd bEnd && currentTag != null
                            && bEnd.Id == currentTag.Bookmark.Id)
                        {
                            // 此处扩展
                            var bookmark = currentTag.Bookmark.Name;
                            var dataset = ds[props.GetValue($"{bookmark}:dataset")];
                            var parentKey = props.GetNullValue($"{bookmark}:parent");
                            if (parentKey != null)
                            {
                                // 定义了父链字段
                                var parentVal = "";
                                if (parentKey.HasValue())
                                {
                                    parentVal = main.GetValue(parentKey, "");
                                }
                                dataset = dataset.Where(r =>
                                {
                                    var row = r as IDictionary<string, object>;
                                    return row.GetValue("parentid", "") == parentVal;
                                });
                            }
                            ExpandBookmark(body, currentTag, bEnd, dataset);

                            // 设为非书签状态
                            bStart = null;
                            currentTag = null;
                        }
                        else if (currentTag != null)
                        {
                            currentTag.Add(current);

                            var previous = current.PreviousSibling();
                            current.Remove();
                            current = previous;
                        }
                        else if (current is Table table)
                        {
                            ExpandTable(table, main);
                        }
                        else
                        {
                            ReplaceParagraph(current, main);
                        }

                        // 取下一对象
                        current = current.NextSibling();
                    }
                    while (current != null);
                }

                return ms.ToArray();
            }
        }
        #endregion

        #region expandt
        private void ExpandTable(Table table, IDictionary<string, object> main)
        {
            WordTag currentTag = null;
            var current = table.FirstChild;

            do
            {
                var bStart = HasBookMark(current);
                if (currentTag == null && bStart != null)
                {
                    // 是书签的话，加入列表
                    currentTag = new WordTag { Bookmark = bStart };
                }
                else if (current is BookmarkEnd bEnd && currentTag != null
                    && bEnd.Id == currentTag.Bookmark.Id)
                {
                    // 此处扩展
                    var bookmark = currentTag.Bookmark.Name;
                    var dataset = ds[props.GetValue($"{bookmark}:dataset")];
                    var parentKey = props.GetNullValue($"{bookmark}:parent");
                    if (parentKey != null)
                    {
                        var parentVal = "";
                        if (parentKey.HasValue())
                        {
                            parentVal = main.GetValue(parentKey, "");
                        }
                        dataset = dataset.Where(r =>
                        {
                            var row = r as IDictionary<string, object>;
                            return row.GetValue("parentid", "") == parentVal;
                        });
                    }

                    var count = dataset.Count();
                    var index = 0;
                    var mergecols = props.GetValue($"{bookmark}:mergecols", "");
                    var mergedict = mergecols.ToDictionary("c", '|', ':');
                    var mergeCache = new Dictionary<string, int>();

                    foreach (IDictionary<string, object> row in dataset)
                    {
                        index++;
                        foreach (var e in currentTag.Elements)
                        {
                            var clone = e.CloneNode(true);
                            table.InsertBefore(clone, bEnd);

                            // 合并单元格
                            if (clone is TableRow tr)
                            {
                                var c = 0;
                                foreach (var cell in tr.Elements<TableCell>())
                                {
                                    c++;
                                    var mergeKey = mergedict.GetValue($"{c}:c", "");
                                    if (mergeKey.HasValue())
                                    {
                                        var mergeRows = row.GetValue(mergeKey, 1);
                                        if (mergeRows > 1)
                                        {
                                            var currentRow = mergeCache.GetAdded($"{c}:r", mergeRows);
                                            var tcp = cell.FirstChild as TableCellProperties;
                                            if (currentRow == mergeRows)
                                            {
                                                var merge = new VerticalMerge() { Val = MergedCellValues.Restart };
                                                tcp.InsertAt(merge, 1);
                                            }
                                            else
                                            {
                                                var merge = new VerticalMerge();
                                                tcp.InsertAt(merge, 1);

                                                // 删除段落内容
                                                cell.Descendants<Run>().Each(ele => ele.Remove());
                                            }

                                            if (currentRow == 1)
                                            {
                                                mergeCache.Remove($"{c}:r");
                                            }
                                            else
                                            {
                                                mergeCache[$"{c}:r"] = mergeCache[$"{c}:r"] - 1;
                                            }
                                        }
                                    }
                                }
                            }

                            ReplaceParagraph(clone, row, index, count);
                        }
                    }

                    // 设为非书签状态
                    bStart = null;
                    currentTag = null;
                }
                else if (currentTag != null)
                {
                    currentTag.Add(current);

                    var previous = current.PreviousSibling();
                    current.Remove();
                    current = previous;
                }
                else
                {
                    ReplaceParagraph(current, main);
                }

                // 取下一对象
                current = current.NextSibling();
            }
            while (current != null);
        }
        #endregion

        #region expandp
        private void ExpandBookmark(Body body, WordTag tag, BookmarkEnd element, IEnumerable<dynamic> datasets)
        {
            WordTag currentTag = null;
            var count = datasets.Count();
            var index = 0;

            foreach (IDictionary<string, object> row in datasets)
            {
                index++;

                foreach (var e in tag.Elements)
                {
                    var bStart = HasBookMark(e);
                    if (currentTag == null && bStart != null)
                    {
                        // 是书签的话，加入列表
                        currentTag = new WordTag { Bookmark = bStart };
                    }
                    else if (e is BookmarkEnd bEnd && bEnd.Id == currentTag.Bookmark.Id)
                    {
                        // 此处扩展
                        var bookmark = currentTag.Bookmark.Name;
                        var dataset = ds[props.GetValue($"{bookmark}:dataset")];
                        var parentKey = props.GetValue($"{bookmark}:parent", "");
                        if (parentKey.HasValue())
                        {
                            var parentVal = row.GetValue(parentKey, "");
                            if (parentVal.HasValue())
                            {
                                dataset = dataset.Where(r =>
                                {
                                    var rowc = r as IDictionary<string, object>;
                                    return rowc.GetValue("parentid", "") == parentVal;
                                });
                            }
                        }
                        ExpandBookmark(body, currentTag, element, dataset);

                        // 设为非书签状态
                        bStart = null;
                        currentTag = null;
                    }
                    else if (currentTag != null)
                    {
                        currentTag.Add(e);
                    }
                    else
                    {
                        var clone = e.CloneNode(true);
                        if (clone.InnerText == "{text}")
                        {
                            // 段落
                            var text = row.GetValue("text", "");
                            if (text.HasValue())
                            {
                                if (text.IndexOf("<p>") >= 0)
                                {
                                    var html = new HtmlDocument();
                                    html.LoadHtml(text);
                                    foreach (var p in html.DocumentNode.SelectNodes("./p | ./div"))
                                    {
                                        var imgs = p.SelectNodes(".//img");
                                        if (imgs != null && imgs.Any())
                                        {
                                            foreach (var img in imgs)
                                            {
                                                var src = img.Attributes["src"].Value;
                                                int w = 500, h = 200;
                                                if (img.Attributes["width"] != null)
                                                {
                                                    w = int.Parse(img.Attributes["width"].Value);
                                                }
                                                if (img.Attributes["height"] != null)
                                                {
                                                    h = int.Parse(img.Attributes["height"].Value);
                                                }
                                                var widthInEmus = w * 914400L / 96;
                                                var heightInEmus = h * 914400L / 96;
                                                var c = e.CloneNode(true);
                                                body.InsertBefore(c, element);
                                                ReplaceImage(c, src, widthInEmus, heightInEmus);
                                            }
                                        }
                                        else
                                        {
                                            var innerText = p.InnerText;
                                            var c = e.CloneNode(true);
                                            var data = new Dictionary<string, object> { { "text", innerText } };
                                            body.InsertBefore(c, element);
                                            ReplaceParagraph(c, data, index, count);
                                        }
                                    }
                                }
                                else
                                {
                                    text.Split('\n').Each(t =>
                                    {
                                        var c = e.CloneNode(true);
                                        var data = new Dictionary<string, object> { { "text", t } };
                                        body.InsertBefore(c, element);
                                        ReplaceParagraph(c, data, index, count);
                                    });
                                }
                            }
                        }
                        else if (clone.InnerText == "{attach}")
                        {
                            // 图片附件
                            var attachs = row.GetValue("attach", "").Trim(',');
                            attachs.Split(',').Each(file =>
                            {
                                var c = e.CloneNode(true);
                                body.InsertBefore(c, element);
                                ReplaceImage(c, file, 5229225L, 7465757L);
                            });

                            // 小于页数时插入分页符
                            if (index < count)
                            {
                                var p = new Paragraph(new Run(new Break { Type = BreakValues.Page }));
                                body.InsertBefore(p, element);
                            }
                        }
                        else
                        {
                            body.InsertBefore(clone, element);
                            ReplaceParagraph(clone, row, index, count);
                        }
                    }
                }
            }
        }
        #endregion

        #region GetTags
        private BookmarkStart HasBookMark(OpenXmlElement e)
        {
            if (e is BookmarkStart bs)
            {
                if (bs.Name.Value.StartsWith("data_") || bs.Name.Value.StartsWith("down_"))
                {
                    return bs;
                }
            }
            return null;
        }

        private bool HasTag(OpenXmlElement ele)
        {
            return Regex.IsMatch(ele.InnerText, TagFactory.REG_Tag);
        }
        #endregion

        #region Replace
        private string DoVarsReplace(string str)
        {
            if (str.HasValue() && vars != null)
            {
                return str.Replace(vars);
            }
            return str;
        }

        private void ReplaceImage(OpenXmlElement ele, string file, long width, long height)
        {
            ImagePart imagePart = null;
            var picType = Path.GetExtension(file).Trim('.');
            if (picType.ToLower() == "jpg")
            {
                picType = "jpeg";
            }

            if (Enum.TryParse(picType, true, out ImagePartType imagePartType))
            {
                imagePart = mainDocumentPart.AddImagePart(imagePartType);
            }

            if (imagePart != null)
            {
                // 读取图片二进制流
                using (var fs = File.Open(App.GetLocalPath(file), FileMode.Open))
                {
                    imagePart.FeedData(fs);
                }

                // 加到文档Body
                AddImageToParagraph(ele, mainDocumentPart.GetIdOfPart(imagePart), width, height);
            }
        }

        // Cx = 5229225L/*, Cy = 7465757L*/
        private void AddImageToParagraph(OpenXmlElement ele, string relationshipId, long width, long height)
        {
            var element = new Drawing(
                new DW.Inline(
                    new DW.Extent() { Cx = width, Cy = height },
                    new DW.EffectExtent()
                    {
                        LeftEdge = 0L,
                        TopEdge = 0L,
                        RightEdge = 0L,
                        BottomEdge = 0L
                    },
                    new DW.DocProperties()
                    {
                        Id = (UInt32Value)1U,
                        Name = "Picture 1"
                    },
                    new DW.NonVisualGraphicFrameDrawingProperties(
                        new A.GraphicFrameLocks() 
                        { 
                            NoChangeAspect = true 
                        }
                    ),
                    new A.Graphic(
                        new A.GraphicData(
                            new PIC.Picture(
                                new PIC.NonVisualPictureProperties(
                                    new PIC.NonVisualDrawingProperties()
                                    {
                                        Id = (UInt32Value)0U,
                                        Name = "Image.jpg"
                                    },
                                    new PIC.NonVisualPictureDrawingProperties()
                                ),
                                new PIC.BlipFill(
                                    new A.Blip(
                                        new A.BlipExtensionList(
                                            new A.BlipExtension()
                                            {
                                                Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                            }
                                        )
                                    )
                                    {
                                        Embed = relationshipId,
                                        CompressionState = A.BlipCompressionValues.Print
                                    },
                                    new A.Stretch(
                                        new A.FillRectangle()
                                    )
                                ),
                                new PIC.ShapeProperties(
                                    new A.Transform2D(
                                        new A.Offset() { X = 0L, Y = 0L },
                                        new A.Extents() { Cx = width, Cy = height }),
                                    new A.PresetGeometry(
                                        new A.AdjustValueList()
                                    )
                                    { 
                                        Preset = A.ShapeTypeValues.Rectangle 
                                    }
                                )
                            )
                        )
                        { 
                            Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" 
                        }
                    )
                )
                {
                    DistanceFromTop = (UInt32Value)0U,
                    DistanceFromBottom = (UInt32Value)0U,
                    DistanceFromLeft = (UInt32Value)0U,
                    DistanceFromRight = (UInt32Value)0U,
                    EditId = "50D07946"
                }
            );

            var run = ele.Descendants<Run>().First();
            run.Elements<Text>().Each(t => t.Remove());
            run.Append(element);
        }

        private void ReplaceParagraph(OpenXmlElement ele, IDictionary<string, object> row, int index = 1, int count = 1)
        {
            foreach (var e in ele.Elements<OpenXmlElement>())
            {
                if (e is Text)
                {
                    var text = e as Text;
                    if (HasTag(e))
                    {
                        TextReplace(text, row, index, count);
                    }
                    else
                    {
                        text.Text = DoVarsReplace(text.Text);
                    }
                }
                else
                {
                    ReplaceParagraph(e, row, index, count);
                }
            }
        }

        private void TextReplace(Text text, IDictionary<string, object> row, int index, int count)
        {
            var tagValue = string.Empty;
            var matchs = Regex.Matches(text.Text, TagFactory.REG_Tag);

            foreach (Match match in matchs)
            {
                if (match.Value.StartsWith("#"))
                {
                    // 行变量
                    text.Text = text.Text
                        .Replace("{#index}", index.ToString());

                    // 全局变量
                    if (vars != null && vars.ContainsKey(match.Value))
                    {
                        text.Text = text.Text.Replace($"{{{match.Value}}}", vars[match.Value].ToString());
                    }
                }
                else if (row != null)
                {
                    var output = true;
                    var replaceStr = "";
                    var matchStrs = match.Value.Split('*');
                    var matchStr = matchStrs[0];

                    // 字段 {FIELD|TYPE|EXPR*1*、}
                    var tag = new DataTag(row, matchStr, index);
                    tagValue = tag.Output();

                    if (matchStrs.Length > 2)
                    {
                        if (index > int.Parse(matchStrs[1]))
                        {
                            if (!temp.Contains(tagValue))
                            {
                                temp.Add(tagValue);
                                tagValue = "，" + tagValue;
                            }
                            else
                            {
                                output = false;
                                if (matchStrs.Length > 2) replaceStr = matchStrs[2];
                            }
                        }
                        else
                        {
                            temp.Add(tagValue);
                        }
                    }
                    else if (matchStrs.Length > 1)
                    {
                        if (index < count)
                        {
                            tagValue = tagValue + matchStrs[1];
                        }
                    }

                    if (output)
                    {
                        var lines = tagValue.Split('\n');
                        OpenXmlElement lastInsert = text;
                        text.Text = text.Text.Replace("{" + match.Value + "}", lines[0]);
                        for (int i = 1; i < lines.Length; i++)
                        {
                            lastInsert = text.Parent.InsertAfter(new Break(), lastInsert);
                            lastInsert = text.Parent.InsertAfter(new Text(lines[i]), lastInsert);
                        }
                    }
                    else
                    {
                        text.Text = text.Text.Replace("{" + match.Value + "}", replaceStr);
                    }
                }
            }
        }
        #endregion
    }
}
