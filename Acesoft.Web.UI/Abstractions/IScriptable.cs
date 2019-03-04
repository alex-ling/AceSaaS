using System.IO;

namespace Acesoft.Web.UI
{
	public interface IScriptable
	{
		bool IsNeedScriptable { get; }
		bool IsOnlyScriptable { get; }

        void WriteScript(TextWriter writer);
	}
}
