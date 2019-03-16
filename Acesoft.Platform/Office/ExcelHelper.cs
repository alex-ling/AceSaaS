using NPOI.SS.Util;

namespace Acesoft.Platform.Office
{
	public class ExcelHelper
	{
		public static string ToAddr(int row, int col)
		{
			return new CellReference(row, col).FormatAsString();
		}

		public static string ToAddr(int firstRow, int lastRow, int firstCol, int lastCol)
		{
			return new CellRangeAddress(firstRow, lastRow, firstCol, lastCol).FormatAsString();
		}
	}
}
