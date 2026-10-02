using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000074 RID: 116
	public interface IAdminPanelOptionProvider
	{
		// Token: 0x06000378 RID: 888
		MBReadOnlyList<IAdminPanelOptionGroup> GetOptionGroups();

		// Token: 0x06000379 RID: 889
		IAdminPanelOption GetOptionWithId(string id);

		// Token: 0x0600037A RID: 890
		IAdminPanelAction GetActionWithId(string id);

		// Token: 0x0600037B RID: 891
		void ApplyOptions();

		// Token: 0x0600037C RID: 892
		void OnTick(float dt);

		// Token: 0x0600037D RID: 893
		void OnFinalize();
	}
}
