using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.Launcher.Library
{
	// Token: 0x02000004 RID: 4
	public static class LauncherPlatform
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000241F File Offset: 0x0000061F
		public static LauncherPlatformType PlatformType
		{
			get
			{
				return LauncherPlatform._platformType;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002428 File Offset: 0x00000628
		public static void Initialize(List<string> args)
		{
			LauncherPlatform._platformType = LauncherPlatform.ReadWindowsPlatformFromFile();
			Assembly assembly = null;
			if (LauncherPlatform.PlatformType == LauncherPlatformType.Steam)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.MountAndBlade.Launcher.Steam.dll", true);
			}
			else if (LauncherPlatform.PlatformType == LauncherPlatformType.Epic)
			{
				assembly = AssemblyLoader.LoadFrom(ManagedDllFolder.Name + "TaleWorlds.MountAndBlade.Launcher.Epic.dll", true);
			}
			if (assembly != null)
			{
				Type[] types = assembly.GetTypes();
				Type type = null;
				foreach (Type type2 in types)
				{
					if (type2.GetInterfaces().Contains(typeof(IPlatformModuleExtension)))
					{
						type = type2;
						break;
					}
				}
				LauncherPlatform._platformModuleExtension = (IPlatformModuleExtension)type.GetConstructor(new Type[0]).Invoke(new object[0]);
			}
			if (LauncherPlatform._platformModuleExtension != null)
			{
				ModuleHelper.InitializePlatformModuleExtension(LauncherPlatform._platformModuleExtension, args);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000024F4 File Offset: 0x000006F4
		public static void Destroy()
		{
			ModuleHelper.ClearPlatformModuleExtension();
			LauncherPlatform._platformModuleExtension = null;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002504 File Offset: 0x00000704
		private static LauncherPlatformType ReadWindowsPlatformFromFile()
		{
			LauncherPlatformType launcherPlatformType = LauncherPlatformType.None;
			if (LauncherPlatform.IsGdk())
			{
				launcherPlatformType = LauncherPlatformType.Gdk;
			}
			else if (LauncherPlatform.IsSteam())
			{
				launcherPlatformType = LauncherPlatformType.Steam;
			}
			else if (LauncherPlatform.IsEpic())
			{
				launcherPlatformType = LauncherPlatformType.Epic;
			}
			else if (LauncherPlatform.IsGog())
			{
				launcherPlatformType = LauncherPlatformType.Gog;
			}
			else
			{
				Debug.ShowMessageBox("There is no target file! Please verify your files.", "ERROR", 4U);
				Environment.Exit(0);
			}
			return launcherPlatformType;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002556 File Offset: 0x00000756
		private static bool IsSteam()
		{
			return File.Exists(BasePath.Name + "Modules/Native/" + "steam.target");
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002576 File Offset: 0x00000776
		private static bool IsGog()
		{
			return File.Exists(BasePath.Name + "Modules/Native/" + "gog.target");
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002596 File Offset: 0x00000796
		private static bool IsGdk()
		{
			return false;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002599 File Offset: 0x00000799
		private static bool IsEpic()
		{
			return File.Exists(BasePath.Name + "Modules/Native/" + "epic.target");
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000025B9 File Offset: 0x000007B9
		public static void SetLauncherMode(bool isLauncherModeActive)
		{
			IPlatformModuleExtension platformModuleExtension = LauncherPlatform._platformModuleExtension;
			if (platformModuleExtension == null)
			{
				return;
			}
			platformModuleExtension.SetLauncherMode(isLauncherModeActive);
		}

		// Token: 0x0400000A RID: 10
		private static LauncherPlatformType _platformType;

		// Token: 0x0400000B RID: 11
		private static IPlatformModuleExtension _platformModuleExtension;
	}
}
