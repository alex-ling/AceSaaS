using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;

using Serilog;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;

namespace Acesoft.IotService
{
	internal static class Program
	{
		private static Dictionary<string, Command> CmdHandlers = new Dictionary<string, Command>();
		private static bool setConsoleColor;
        private static ILogger logger;

		private static void Main(string[] args)
		{
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.Console()
                .WriteTo.Async(a => a.File("logs/log.txt", rollingInterval: RollingInterval.Day, shared: true))
                .CreateLogger();
            logger = Log.ForContext(typeof(Program));

            try
            {
                logger.Information("Starting AcesoftIotService...");

                if (!Environment.UserInteractive)
			    {
				    RunAsService();
				    return;
			    }

			    var input = string.Empty;
			    if (args == null || args.Length < 1)
			    {
				    Console.WriteLine("Welcome to Acesoft.IotService!");
				    Console.WriteLine("Please press a key to continue...");
				    Console.WriteLine("[r]: Run this application as a console application");
				    Console.WriteLine("[i]: Install this application as a Windows Service");
				    Console.WriteLine("[u]: Uninstall this Windows Service application");
				    do
				    {
					    input = Console.ReadKey().KeyChar.ToString();
					    Console.WriteLine();
				    }
				    while (!Run(input, null));

				    Console.WriteLine("Press the 'Enter' key to quit this application.");
				    Console.ReadLine();
			    }
			    else
			    {
				    input = args[0];
				    if (!string.IsNullOrEmpty(input))
				    {
					    input = input.TrimStart('-');
				    }
				    Run(input, args);
			    }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "AcesoftIotService terminated unexpectedly");
            }
            finally
            {
                logger.Information("Stopped AcesoftIotService!");
                Log.CloseAndFlush();
            }
        }

        private static void RunAsService()
        {
            ServiceBase.Run(new ServiceBase[1]
            {
                new IotService()
            });
        }

        #region RunAsConsole
        private static bool Run(string exeArg, string[] startArgs)
		{
			switch (exeArg.ToLower())
			{
			    case "i":
				    SelfInstaller.InstallMe();
				    Console.WriteLine("Install this application as a Windows Service successful!");
				    return true;
			    case "u":
				    SelfInstaller.UninstallMe();
				    Console.WriteLine("Uninstall this application from Windows Service successful!");
				    return true;
			    case "r":
				    RunAsConsole();
				    return true;
			    default:
				    Console.WriteLine("Invalid argument!");
				    return false;
			}
		}

		private static void RunAsConsole()
		{
			CheckCanSetConsoleColor();

            logger.Information("Initializing servers...");

			var bootstrap = BootstrapFactory.CreateBootstrap();
			if (!bootstrap.Initialize())
			{
				SetConsoleColor(ConsoleColor.Red);
				Console.WriteLine("Failed to initialize AcesoftIotService!");
				Console.ReadKey();
				return;
			}

			var startResult = bootstrap.Start();
			Console.WriteLine("-------------------------------------------------------------------");
			foreach (var appServer in bootstrap.AppServers)
			{
				if (appServer.State == ServerState.Running)
				{
					SetConsoleColor(ConsoleColor.Green);
					Console.WriteLine("Acesoft.IotService - {0} has been started", appServer.Name);
				}
				else
				{
					SetConsoleColor(ConsoleColor.Red);
					Console.WriteLine("Acesoft.IotService - {0} failed to start", appServer.Name);
				}
			}
			Console.ResetColor();
			Console.WriteLine("-------------------------------------------------------------------");
			switch (startResult)
			{
			    case StartResult.None:
				    SetConsoleColor(ConsoleColor.Red);
				    Console.WriteLine("No server is configured, please check you configuration!");
				    Console.ReadKey();
				    return;
			    case StartResult.Success:
				    Console.WriteLine("The AcesoftIotService has been started!");
				    break;
			    case StartResult.Failed:
				    SetConsoleColor(ConsoleColor.Red);
				    Console.WriteLine("Failed to start the Acesoft.IotService! Please check error log for more information!");
				    Console.ReadKey();
				    return;
			    case StartResult.PartialSuccess:
				    SetConsoleColor(ConsoleColor.Red);
				    Console.WriteLine("Some server instances were started successfully, but the others failed! Please check error log for more information!");
				    break;
			}
			Console.ResetColor();
			Console.WriteLine("Enter key 'quit' to stop the AcesoftIotService.");

			RegisterCommands();
			ReadConsoleCommand(bootstrap);
			bootstrap.Stop();

			Console.WriteLine("The AcesoftIotService has been stopped!");
		}

		private static void AddCommand(string name, string description, Func<IBootstrap, string[], bool> handler)
		{
			Command command = new Command
			{
				Name = name,
				Description = description,
				Handler = handler
			};
			CmdHandlers.Add(command.Name, command);
		}

		private static void RegisterCommands()
		{
			AddCommand("List", "List all server instances", ListCommand);
			AddCommand("Start", "Start a server instance: Start {ServerName}", StartCommand);
			AddCommand("Stop", "Stop a server instance: Stop {ServerName}", StopCommand);
		}

		private static bool ListCommand(IBootstrap bootstrap, string[] arguments)
		{
			foreach (IWorkItem appServer in bootstrap.AppServers)
			{
				IProcessServer processServer = appServer as IProcessServer;
				if (processServer != null && processServer.ProcessId > 0)
				{
					Console.WriteLine("{0}[PID:{1}] - {2}", appServer.Name, processServer.ProcessId, appServer.State);
				}
				else
				{
					Console.WriteLine("{0} - {1}", appServer.Name, appServer.State);
				}
			}
			return false;
		}

		private static bool StopCommand(IBootstrap bootstrap, string[] arguments)
		{
			string name = arguments[1];
			if (string.IsNullOrEmpty(name))
			{
				Console.WriteLine("Server name is required!");
				return false;
			}
			IWorkItem workItem = bootstrap.AppServers.FirstOrDefault((IWorkItem s) => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			if (workItem == null)
			{
				Console.WriteLine("The server was not found!");
				return false;
			}
			workItem.Stop();
			return true;
		}

		private static bool StartCommand(IBootstrap bootstrap, string[] arguments)
		{
			string name = arguments[1];
			if (string.IsNullOrEmpty(name))
			{
				Console.WriteLine("Server name is required!");
				return false;
			}
			IWorkItem workItem = bootstrap.AppServers.FirstOrDefault((IWorkItem s) => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			if (workItem == null)
			{
				Console.WriteLine("The server was not found!");
				return false;
			}
			workItem.Start();
			return true;
		}

		private static void ReadConsoleCommand(IBootstrap bootstrap)
		{
			string text = Console.ReadLine();
			if (string.IsNullOrEmpty(text))
			{
				ReadConsoleCommand(bootstrap);
			}
			else if (!"quit".Equals(text, StringComparison.OrdinalIgnoreCase))
			{
				string[] array = text.Split(' ');
				if (!CmdHandlers.TryGetValue(array[0], out Command value))
				{
					Console.WriteLine("Unknown command");
					ReadConsoleCommand(bootstrap);
				}
				else
				{
					try
					{
						if (value.Handler(bootstrap, array))
						{
							Console.WriteLine("Ok");
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Failed. " + ex.Message + Environment.NewLine + ex.StackTrace);
					}
					ReadConsoleCommand(bootstrap);
				}
			}
		}

		private static void CheckCanSetConsoleColor()
		{
			try
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.ResetColor();
				setConsoleColor = true;
			}
			catch
			{
				setConsoleColor = false;
			}
		}

		private static void SetConsoleColor(ConsoleColor color)
		{
			if (setConsoleColor)
			{
				Console.ForegroundColor = color;
			}
		}
        #endregion
    }
}
