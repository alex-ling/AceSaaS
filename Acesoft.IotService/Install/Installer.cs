using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.ServiceProcess;

using Acesoft.Util;

namespace Acesoft.IotService
{
	[RunInstaller(true)]
	public class Installer : System.Configuration.Install.Installer
	{
		private ServiceInstaller serviceInstaller;
		private ServiceProcessInstaller processInstaller;
		private IContainer components;

		public Installer()
		{
			InitializeComponent();

			processInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();

			processInstaller.Account = ServiceAccount.LocalSystem;
			serviceInstaller.StartType = ServiceStartMode.Automatic;

			serviceInstaller.ServiceName = ConfigHelper.GetAppSetting<string>("ServiceName");
            serviceInstaller.DisplayName = ConfigHelper.GetAppSetting<string>("ServiceDisplayName", serviceInstaller.ServiceName);
            serviceInstaller.Description = ConfigHelper.GetAppSetting<string>("ServiceDescription", serviceInstaller.ServiceName);
            serviceInstaller.ServicesDependedOn = ConfigHelper.GetAppSetting<string>("ServicesDependedOn", "tcpip").Split(',');

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);

            AfterInstall += (sender, e) =>
            {
                new ServiceController(serviceInstaller.ServiceName).Start();
            };
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
		}
	}
}
