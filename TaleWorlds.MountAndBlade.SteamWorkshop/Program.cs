using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Steamworks;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.SteamWorkshop
{
	// Token: 0x02000004 RID: 4
	internal class Program
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021E4 File Offset: 0x000003E4
		public static string BannerlordSteamAppIdAsString
		{
			get
			{
				return 261550.ToString();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021FE File Offset: 0x000003FE
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002205 File Offset: 0x00000405
		public static PublishedFileId_t ItemId { get; set; }

		// Token: 0x0600000B RID: 11 RVA: 0x0000220D File Offset: 0x0000040D
		private static void CreateSteamAppIdFile()
		{
			File.WriteAllText("steam_appid.txt", Program.BannerlordSteamAppIdAsString);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000221E File Offset: 0x0000041E
		public static void ExitProgram(int exitCode)
		{
			Program.Log("Finished...");
			File.WriteAllText("steam_workshop_uploader.txt", Program._output);
			Console.ReadKey();
			Environment.Exit(0);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002245 File Offset: 0x00000445
		public static void Log(string log)
		{
			Console.WriteLine(log);
			Program._output = Program._output + log + "\n";
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002264 File Offset: 0x00000464
		private static void LoadTasks(string configurationFile)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(configurationFile);
			Program._tasks = new List<ToolTask>();
			foreach (object obj in xmlDocument.FirstChild.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				ToolTask toolTask = null;
				if (xmlNode.Name == "CreateItem")
				{
					toolTask = new CreateItemTask();
				}
				else if (xmlNode.Name == "UpdateItem")
				{
					toolTask = new UpdateItemTask();
				}
				else if (xmlNode.Name == "GetItem")
				{
					toolTask = new GetItemTask();
				}
				toolTask.LoadFrom(xmlNode);
				Program._tasks.Add(toolTask);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002330 File Offset: 0x00000530
		private static void Main(string[] args)
		{
			try
			{
				Debug.DebugManager = new ToolDebugManager();
				Common.SetInvariantCulture();
				Program.Log("Starting...");
				if (args == null || args.Length != 1)
				{
					Program.Log("Wrong argument count");
					Program.ExitProgram(-78);
				}
				string text = args[0];
				if (!File.Exists(text))
				{
					Program.Log("Couldn't find configuration file");
					Program.ExitProgram(-55);
				}
				Program.CreateSteamAppIdFile();
				if (!SteamAPI.Init())
				{
					Program.Log("Could not initialize Steam");
					Program.ExitProgram(-1);
				}
				if (!SteamUser.BLoggedOn())
				{
					Program.Log("Steam user is not logged in. Please log in to Steam");
					Program.ExitProgram(-2);
				}
				if (!SteamRemoteStorage.IsCloudEnabledForAccount())
				{
					Program.Log("Cloud is not enabled for your account");
					Program.ExitProgram(-3);
				}
				if (!SteamRemoteStorage.IsCloudEnabledForApp())
				{
					Program.Log("Cloud is not enabled for this app");
					Program.ExitProgram(-4);
				}
				Program.LoadTasks(text);
				foreach (ToolTask toolTask in Program._tasks)
				{
					toolTask.DoJob();
				}
				SteamAPI.Shutdown();
				Program.ExitProgram(0);
			}
			catch (Exception ex)
			{
				Program.Log("Application crashed with exception: " + ex);
				Program.ExitProgram(-50);
			}
		}

		// Token: 0x04000003 RID: 3
		public const int BannerlordSteamAppId = 261550;

		// Token: 0x04000005 RID: 5
		private static string _output = "";

		// Token: 0x04000006 RID: 6
		private static List<ToolTask> _tasks;
	}
}
