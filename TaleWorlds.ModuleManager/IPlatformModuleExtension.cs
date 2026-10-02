using System;
using System.Collections.Generic;

namespace TaleWorlds.ModuleManager
{
	// Token: 0x02000004 RID: 4
	public interface IPlatformModuleExtension
	{
		// Token: 0x0600000A RID: 10
		void Initialize(List<string> args);

		// Token: 0x0600000B RID: 11
		void Destroy();

		// Token: 0x0600000C RID: 12
		string[] GetModulePaths();

		// Token: 0x0600000D RID: 13
		void SetLauncherMode(bool isLauncherModeActive);

		// Token: 0x0600000E RID: 14
		bool CheckEntitlement(string title);
	}
}
