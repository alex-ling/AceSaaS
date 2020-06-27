using System.Data;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Acesoft.Data;

namespace Acesoft.Platform.Office
{
	public class XlsExport : IOfficeReport
    {
		private readonly string tempFile;
		private readonly GridResponse res;
        private bool autoHeight = false;

        public XSSFWorkbook Workbook { get; private set; }

		public XlsExport(GridResponse res, string tempFile, bool autoHeight)
		{
            this.res = res;
			this.tempFile = tempFile;
            this.autoHeight = autoHeight;
		}

		public byte[] Export()
		{
			using (var fs = File.OpenRead(tempFile))
			{
				Workbook = new XSSFWorkbook(fs);
			}

			Render();

			using (var memoryStream = new MemoryStream())
			{
				Workbook.Write(memoryStream);
				return memoryStream.ToArray();
			}
		}

		private void Render()
		{
			ISheet sheetAt = Workbook.GetSheetAt(Workbook.ActiveSheetIndex);
			if (res.Data.Rows.Count > 0)
			{
				Workbook.SetSheetName(0, "共{0}条数据".FormatWith(res.Data.Rows.Count));
			}
			else
			{
				Workbook.SetSheetName(0, "无相关数据");
			}
			ExpandSheet(sheetAt, null);
		}

		private void ExpandSheet(ISheet sheet, DataRow mainRow)
		{
			for (int i = 0; i <= sheet.LastRowNum; i++)
			{
				for (int j = 0; j < sheet.GetRow(0).LastCellNum; j++)
				{
					NamedRange expandRegion = GetExpandRegion(sheet, i, j);
					if (expandRegion == null)
					{
						ReplaceRange(sheet.GetRow(i).GetCell(j), mainRow, 1);
					}
					else
					{
						int rowCount = expandRegion.RowCount;
						int colCount = expandRegion.ColCount;
						DataRowCollection rows = res.Data.Rows;
						if (rows != null)
						{
							for (int k = 0; k < rows.Count; k++)
							{
								Range range = new Range(sheet, i, i + rowCount - 1, j, j + colCount - 1);
								if (j == range.FirstCol && k + 1 < rows.Count)
								{
									sheet.ShiftRows(range.FirstRow, range.LastRow, rowCount, true, false);
									for (int l = range.FirstRow; l <= range.LastRow; l++)
									{
										sheet.CopyRow(l + rowCount, l);
                                        if (!autoHeight)
                                        {
                                            sheet.GetRow(l).Height = sheet.GetRow(l + rowCount).Height;
                                        }
									}
								}
								ExpandRange(range, rows[k], k + 1);
								i += rowCount;
							}
						}
						else
						{
							ExpandRange(expandRegion, null, 1);
						}
						j += colCount - 1;
					}
				}
			}
		}

		private void ExpandRange(Range range, DataRow row, int index)
		{
			CellReference[] cells = range.Cells;
			foreach (CellReference cellReference in cells)
			{
				ReplaceRange(range.Sheet.GetRow(cellReference.Row).GetCell(cellReference.Col), row, index);
			}
		}

		private void ReplaceRange(ICell r, DataRow row, int index = 1)
		{
			// 合并单元格此时为空
			if (r == null) return;

			if (r.StringCellValue != null && row != null)
			{
				var numeric = r.StringCellValue.IndexOf("|num") > 0;
				r.SetCellValue(TagFactory.ReplaceTag(r.StringCellValue, row, index));
				if (numeric && r.StringCellValue.HasValue())
				{
					r.SetCellValue(double.Parse(r.StringCellValue));
				}
			}
            else if (r.StringCellValue != null)
            {
                r.SetCellValue(App.ReplaceQuery(r.StringCellValue));
            }
		}

		private NamedRange GetExpandRegion(ISheet s, int r, int c)
		{
			for (int i = 0; i < Workbook.NumberOfNames; i++)
			{
				NamedRange namedRange = new NamedRange(Workbook, Workbook.GetNameAt(i));
				if (namedRange.Sheet == s 
                    && namedRange.Name.NameName.StartsWith("expand_") 
                    && namedRange.FirstRow <= r 
                    && namedRange.FirstCol <= c 
                    && namedRange.LastRow >= r 
                    && namedRange.LastCol >= c)
				{
					return namedRange;
				}
			}
			return null;
		}
	}
}
