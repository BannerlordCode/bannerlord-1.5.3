using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.Starter.Library;
using TaleWorlds.TwoDimension.Standalone;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000009 RID: 9
	public class Program
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002A5C File Offset: 0x00000C5C
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002A63 File Offset: 0x00000C63
		internal static bool IsShuttingDown { get; private set; }

		// Token: 0x06000048 RID: 72 RVA: 0x00002A6B File Offset: 0x00000C6B
		static Program()
		{
			Common.PlatformFileHelper = new PlatformFileHelperPC("Mount and Blade II Bannerlord");
			Debug.DebugManager = new LauncherDebugManager();
			AppDomain.CurrentDomain.AssemblyResolve += Program.OnAssemblyResolve;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002AA6 File Offset: 0x00000CA6
		public static void NativeMain(string commandLine)
		{
			Program._isTestMode = commandLine.ToLower().Contains("/runtest");
			Program.Main(commandLine.Split(Array.Empty<char>()).ToArray<string>());
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002AD4 File Offset: 0x00000CD4
		public static void Main(string[] args)
		{
			Program._args = args.ToList<string>();
			if (!Program._isTestMode)
			{
				try
				{
					string fileName = Process.GetCurrentProcess().MainModule.FileName;
					string text = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
					text = Path.Combine(text, "Mount and Blade II Bannerlord");
					string name = Directory.GetParent(fileName).Name;
					if (!Program._args.Contains("/no_watchdog"))
					{
						new Watchdog(true, text);
					}
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "Build Source", "122374");
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "Build Target", "Public");
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "Build Version", "v1.5.3.122374");
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "Product Name", "Mount and Blade II Bannerlord");
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "Build Name", name);
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "Launcher", "true");
					Common.PlatformFileHelper = new PlatformFileHelperPC("Mount and Blade II Bannerlord");
					Common.SetInvariantCulture();
					LauncherPlatform.Initialize(Program._args);
					LauncherPlatform.SetLauncherMode(true);
					Watchdog.LogProperty("crash_tags.txt", "Runtime", "Build Platform", LauncherPlatform.PlatformType.ToString());
					ResourceDepot resourceDepot = new ResourceDepot();
					resourceDepot.AddLocation(BasePath.Name, "Modules/Native/LauncherGUI/");
					resourceDepot.CollectResources();
					resourceDepot.StartWatchingChangesInDepot();
					string text2 = "M&B II: Bannerlord";
					Rectangle rectangle;
					User32.GetClientRect(User32.GetDesktopWindow(), out rectangle);
					float num = (float)rectangle.Height / 1350f;
					Program._graphicsForm = new GraphicsForm((int)(num * 1154f), (int)(num * 701f), resourceDepot, true, true, true, text2);
					Program._windowsFramework = new WindowsFramework();
					Program._windowsFramework.ThreadConfig = WindowsFrameworkThreadConfig.NoThread;
					Program._standaloneUIDomain = new StandaloneUIDomain(Program._graphicsForm, resourceDepot);
					Program._windowsFramework.Initialize(new FrameworkDomain[] { Program._standaloneUIDomain });
					Program._windowsFramework.RegisterMessageCommunicator(Program._graphicsForm);
					Program._windowsFramework.Start();
					LauncherPlatform.SetLauncherMode(false);
					LauncherPlatform.Destroy();
					Watchdog.DetachAndClose();
					goto IL_0243;
				}
				catch (Exception ex)
				{
					Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.Print(ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
					throw;
				}
			}
			Program._gameStarted = true;
			IL_0243:
			if (Program._gameStarted)
			{
				LauncherPlatform.SetLauncherMode(false);
				Program.Main(Program._args.ToArray());
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002D60 File Offset: 0x00000F60
		public static void StartGame()
		{
			Program._additionalArgs = Program._standaloneUIDomain.AdditionalArgs;
			Program._args.Add(Program._additionalArgs);
			Program._hasUnofficialModulesSelected = Program._standaloneUIDomain.HasUnofficialModulesSelected;
			Program._gameStarted = true;
			Program.AuxFinalize();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002D9A File Offset: 0x00000F9A
		public static void StartDigitalCompanion()
		{
			Program.AuxFinalize();
			Process.Start(new ProcessStartInfo("..\\..\\DigitalCompanion\\Mount & Blade II Bannerlord - Digital Companion.exe"));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002DB4 File Offset: 0x00000FB4
		private static void AuxFinalize()
		{
			Program.IsShuttingDown = true;
			Program._windowsFramework.UnRegisterMessageCommunicator(Program._graphicsForm);
			Program._windowsFramework.Stop();
			Program._graphicsForm.Destroy();
			Program._windowsFramework = null;
			Program._graphicsForm = null;
			LauncherDebugManager launcherDebugManager = Debug.DebugManager as LauncherDebugManager;
			if (launcherDebugManager != null)
			{
				launcherDebugManager.OnFinalize();
			}
			User32.SetForegroundWindow(Kernel32.GetConsoleWindow());
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002E18 File Offset: 0x00001018
		private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
		{
			Debug.Print("Resolving: " + args.Name, 0, Debug.DebugColor.White, 17592186044416UL);
			if (args.Name.Contains("ManagedStarter"))
			{
				return Assembly.LoadFrom(Program.StarterExecutable);
			}
			return null;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002E64 File Offset: 0x00001064
		public static bool IsDigitalCompanionAvailable()
		{
			return File.Exists("..\\..\\DigitalCompanion\\Mount & Blade II Bannerlord - Digital Companion.exe");
		}

		// Token: 0x04000021 RID: 33
		private static string StarterExecutable = "Bannerlord.exe";

		// Token: 0x04000022 RID: 34
		private const string _pathToDigitalCompanionExe = "..\\..\\DigitalCompanion\\Mount & Blade II Bannerlord - Digital Companion.exe";

		// Token: 0x04000023 RID: 35
		private static WindowsFramework _windowsFramework;

		// Token: 0x04000024 RID: 36
		private static GraphicsForm _graphicsForm;

		// Token: 0x04000025 RID: 37
		private static List<string> _args;

		// Token: 0x04000026 RID: 38
		private static string _additionalArgs;

		// Token: 0x04000027 RID: 39
		private static bool _hasUnofficialModulesSelected;

		// Token: 0x04000028 RID: 40
		private static StandaloneUIDomain _standaloneUIDomain;

		// Token: 0x04000029 RID: 41
		private static bool _isTestMode;

		// Token: 0x0400002A RID: 42
		private static bool _gameStarted;
	}
}
