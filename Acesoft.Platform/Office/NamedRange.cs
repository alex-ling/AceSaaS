using NPOI.SS.UserModel;

namespace Acesoft.Platform.Office
{
	public class NamedRange : Range
	{
		public IName Name { get; private set; }

		public NamedRange(IWorkbook wb, IName name)
			: base(wb.GetSheet(name.SheetName), name.RefersToFormula)
		{
			Name = name;
		}
	}
}
