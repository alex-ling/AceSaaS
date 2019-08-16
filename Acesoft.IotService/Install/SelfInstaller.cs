using System.Configuration.Install;
using System.Reflection;

namespace Acesoft.IotService
{
	public static class SelfInstaller
	{
		private static readonly string exePath = Assembly.GetExecutingAssembly().Location;

		public static bool InstallMe()
		{
			try
			{
				ManagedInstallerClass.InstallHelper(new string[1]
				{
					exePath
				});
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool UninstallMe()
		{
			try
			{
				ManagedInstallerClass.InstallHelper(new string[2]
				{
					"/u",
					exePath
				});
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
