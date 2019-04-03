namespace Acesoft.Web.UI.Ajax
{
	public class ScriptHandler
	{
		public string Handler { get; set; }

		public bool HasValue()
		{
			return Handler.HasValue();
		}

		public override string ToString()
		{
			if (HasValue())
			{
				return Handler;
			}
			return "null";
		}

		public string ToCallString()
		{
			if (HasValue() && Handler.StartsWith("function"))
			{
				return "(" + Handler + ")()";
			}
			return ToString();
		}
	}
}
