using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200020D RID: 525
	public class ConsolesModuleExtension : IPlatformModuleExtension
	{
		// Token: 0x06001E89 RID: 7817 RVA: 0x00068B6C File Offset: 0x00066D6C
		public ConsolesModuleExtension()
		{
			this._modulePaths = new List<string>();
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x00068B80 File Offset: 0x00066D80
		public void Initialize(List<string> args)
		{
			string platformModulePaths = Utilities.GetPlatformModulePaths();
			Debug.Print("ConsolesModuleExtension::Initialize::" + platformModulePaths + "\n", 0, Debug.DebugColor.White, 17592186044416UL);
			if (platformModulePaths.Length > 0)
			{
				this._modulePaths = new List<string>(platformModulePaths.Split(new char[] { '$' }));
				return;
			}
			this._modulePaths = new List<string>();
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x00068BE5 File Offset: 0x00066DE5
		public string[] GetModulePaths()
		{
			return this._modulePaths.ToArray();
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x00068BF2 File Offset: 0x00066DF2
		public void Destroy()
		{
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x00068BF4 File Offset: 0x00066DF4
		public void SetLauncherMode(bool isLauncherModeActive)
		{
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x00068BF6 File Offset: 0x00066DF6
		public bool CheckEntitlement(string title)
		{
			return true;
		}

		// Token: 0x04000A68 RID: 2664
		private List<string> _modulePaths;
	}
}
