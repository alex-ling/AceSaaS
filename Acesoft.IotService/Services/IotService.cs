using Serilog;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using System.ComponentModel;
using System.ServiceProcess;

namespace Acesoft.IotService
{
	public class IotService : ServiceBase
	{
		private IBootstrap bootstrap;
		private IContainer components;

		public IotService()
		{
			InitializeComponent();

            bootstrap = BootstrapFactory.CreateBootstrap();
		}

		protected override void OnStart(string[] args)
		{
			if (bootstrap.Initialize())
			{
				bootstrap.Start();
			}
		}

		protected override void OnStop()
		{
			bootstrap.Stop();
            base.OnStop();
		}

		protected override void OnShutdown()
		{
			bootstrap.Stop();
			base.OnShutdown();
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
