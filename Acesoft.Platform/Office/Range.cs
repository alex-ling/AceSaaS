using System.Linq;

using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Acesoft.Platform.Office
{
	public class Range
	{
		public ISheet Sheet { get; private set; }
		public AreaReference Area { get; private set; }
        public CellReference[] Cells { get; private set; }
        public int FirstRow { get; private set; }
        public int LastRow { get; private set; }
        public int FirstCol { get; private set; }
        public int LastCol { get; private set; }
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }

        public Range(ISheet sheet, int firstRow, int lastRow, int firstCol, int lastCol)
			: this(sheet, ExcelHelper.ToAddr(firstRow, lastRow, firstCol, lastCol))
		{
		}

		public Range(ISheet sheet, string reference)
		{
			Sheet = sheet;
			Area = new AreaReference(reference);
			Cells = Area.GetAllReferencedCells();
			CellReference cellReference = Cells.First();
			CellReference cellReference2 = Cells.Last();
			FirstRow = cellReference.Row;
			FirstCol = cellReference.Col;
			LastRow = cellReference2.Row;
			LastCol = cellReference2.Col;
			RowCount = LastRow - FirstRow + 1;
			ColCount = LastCol - FirstCol + 1;
		}
	}
}
