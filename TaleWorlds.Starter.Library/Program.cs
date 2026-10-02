using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;

namespace TaleWorlds.Starter.Library
{
	// Token: 0x02000002 RID: 2
	public class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		private static void WriteErrorLog(string text)
		{
			string text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Mount and Blade II Bannerlord");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			text2 = Path.Combine(text2, "logs");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			File.WriteAllText(Path.Combine(text2, "starter_log.txt"), text);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A4 File Offset: 0x000002A4
		private static int Starter()
		{
			string text = "";
			try
			{
				Assembly.LoadFrom("TaleWorlds.DotNet.dll").GetType("TaleWorlds.DotNet.Controller").GetMethod("SetEngineMethodsAsDotNet")
					.Invoke(null, new object[]
					{
						new Program.ControllerDelegate(MBDotNet.PassControllerMethods),
						new Program.InitializerDelegate(MBDotNet.PassManagedInitializeMethodPointerDotNet),
						new Program.InitializerDelegate(MBDotNet.PassManagedEngineCallbackMethodPointersDotNet)
					});
				for (int i = 0; i < Program._args.Length; i++)
				{
					string text2 = Program._args[i];
					text += text2;
					if (i + 1 < Program._args.Length)
					{
						text += " ";
					}
				}
				MBDotNet.SetCurrentDirectory(Directory.GetCurrentDirectory());
			}
			catch (FileNotFoundException ex)
			{
				string text3 = "Exception: " + ex;
				text3 = text3 + "Fusion Log: " + ex.FusionLog + "\n";
				text3 = text3 + "Exception detailed: " + ex.ToString() + "\n";
				if (ex.InnerException != null)
				{
					text3 = string.Concat(new object[] { text3, "Inner Exception: ", ex.InnerException, "\n" });
				}
				Program.WriteErrorLog(text3);
				return 25;
			}
			catch (Exception ex2)
			{
				string text4 = "Exception: " + ex2;
				if (ex2.InnerException != null)
				{
					text4 = text4 + "Inner Exception: " + ex2.InnerException;
				}
				Program.WriteErrorLog(text4);
				return 25;
			}
			return MBDotNet.WotsMainDotNet(text);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000223C File Offset: 0x0000043C
		[STAThread]
		public static int Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
			Program._args = args;
			return Program.Starter();
		}

		// Token: 0x04000001 RID: 1
		private static string[] _args;

		// Token: 0x02000004 RID: 4
		// (Invoke) Token: 0x0600000B RID: 11
		private delegate void ControllerDelegate(Delegate currentDomainInitializer);

		// Token: 0x02000005 RID: 5
		// (Invoke) Token: 0x0600000F RID: 15
		private delegate void InitializerDelegate(Delegate argument);

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000013 RID: 19
		private delegate void StartMethodDelegate(string args);
	}
}
