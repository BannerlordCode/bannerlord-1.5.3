using System;
using System.Collections.Generic;
using Steamworks;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.PlatformService.Steam
{
	// Token: 0x02000004 RID: 4
	public class SteamModuleExtension : IPlatformModuleExtension
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002446 File Offset: 0x00000646
		public SteamModuleExtension()
		{
			this._modulePaths = new List<string>();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000245C File Offset: 0x0000065C
		public void Initialize(List<string> args)
		{
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
			if (numSubscribedItems > 0U)
			{
				PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
				SteamUGC.GetSubscribedItems(array, numSubscribedItems);
				int num = 0;
				while ((long)num < (long)((ulong)numSubscribedItems))
				{
					ulong num2;
					string text;
					uint num3;
					if (SteamUGC.GetItemInstallInfo(array[num], out num2, out text, 4096U, out num3))
					{
						this._modulePaths.Add(text);
					}
					num++;
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024B8 File Offset: 0x000006B8
		public string[] GetModulePaths()
		{
			return this._modulePaths.ToArray();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000024C5 File Offset: 0x000006C5
		public void Destroy()
		{
			this._modulePaths.Clear();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000024D2 File Offset: 0x000006D2
		public void SetLauncherMode(bool isLauncherModeActive)
		{
			SteamUtils.SetGameLauncherMode(isLauncherModeActive);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000024DC File Offset: 0x000006DC
		public bool CheckEntitlement(string title)
		{
			int dlccount = SteamApps.GetDLCCount();
			for (int i = 0; i < dlccount; i++)
			{
				AppId_t appId_t;
				bool flag;
				string text;
				if (SteamApps.BGetDLCDataByIndex(i, out appId_t, out flag, out text, 128) && text == title && SteamApps.BIsSubscribedApp(appId_t))
				{
					return SteamApps.BIsDlcInstalled(appId_t);
				}
			}
			return false;
		}

		// Token: 0x0400000D RID: 13
		private List<string> _modulePaths;
	}
}
