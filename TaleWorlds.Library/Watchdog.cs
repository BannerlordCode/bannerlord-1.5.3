using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace TaleWorlds.Library
{
	// Token: 0x020000A9 RID: 169
	public class Watchdog
	{
		// Token: 0x0600066E RID: 1646 RVA: 0x00016728 File Offset: 0x00014928
		public Watchdog(bool use_coreclr, string dumpdir)
		{
			if (Debugger.IsAttached)
			{
				return;
			}
			if (!File.Exists("Watchdog\\Watchdog.exe"))
			{
				return;
			}
			int id = Process.GetCurrentProcess().Id;
			string text = Path.Combine(dumpdir, "crashes");
			string text2 = Path.Combine(dumpdir, "logs");
			string text3 = "..\\..\\..\\WOTS\\Modules\\Test";
			string text4 = "";
			text4 = text4 + " -p " + id;
			text4 = text4 + " -dd \"" + text + "\"";
			text4 = text4 + " -dl \"" + text2 + "\"";
			text4 = text4 + " -dir-cdb-sos \"" + text3 + "\"";
			text4 = text4 + " --sync-event-name \"" + this.WatchdogMutexName + "\"";
			text4 += " -pu ";
			if (use_coreclr)
			{
				text4 += " --net6-plus ";
			}
			using (EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, this.WatchdogMutexName))
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo
				{
					FileName = "Watchdog\\Watchdog.exe",
					Arguments = text4,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};
				new Process
				{
					StartInfo = processStartInfo
				}.Start();
				eventWaitHandle.WaitOne(15000);
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00016890 File Offset: 0x00014A90
		public static void SetDumpDirectory(string Path)
		{
			string text = "#TW#-dd" + Path;
			Debugger.Log(0, null, text);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000168B4 File Offset: 0x00014AB4
		public static void DetachAndClose()
		{
			string text = "#TW#-dt1";
			Debugger.Log(0, null, text);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000168D0 File Offset: 0x00014AD0
		public static void LogProperty(string FileName, string GroupName, string Key, string Value)
		{
			string text = "";
			text = text + "#TW#" + FileName;
			text = text + "#TW#" + GroupName;
			text = text + "#TW#" + Key;
			text = text + "#TW#" + Value;
			Debugger.Log(0, null, text);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001691F File Offset: 0x00014B1F
		public static bool Attached()
		{
			return Debugger.IsAttached;
		}

		// Token: 0x040001E9 RID: 489
		public string WatchdogMutexName = "Global\\8DBAEBA5-40DB-4E8A-A997-E440DE7D7717";
	}
}
