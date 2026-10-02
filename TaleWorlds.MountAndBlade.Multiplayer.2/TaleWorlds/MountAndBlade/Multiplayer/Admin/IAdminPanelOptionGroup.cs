using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000073 RID: 115
	public interface IAdminPanelOptionGroup
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000372 RID: 882
		string UniqueId { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000373 RID: 883
		bool RequiresRestart { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000374 RID: 884
		TextObject Name { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000375 RID: 885
		MBReadOnlyList<IAdminPanelOption> Options { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000376 RID: 886
		MBReadOnlyList<IAdminPanelAction> Actions { get; }

		// Token: 0x06000377 RID: 887
		void OnFinalize();
	}
}
