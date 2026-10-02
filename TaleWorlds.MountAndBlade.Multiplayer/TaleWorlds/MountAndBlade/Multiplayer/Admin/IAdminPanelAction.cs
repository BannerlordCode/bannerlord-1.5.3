using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006B RID: 107
	public interface IAdminPanelAction
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000354 RID: 852
		string UniqueId { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000355 RID: 853
		string Name { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000356 RID: 854
		string Description { get; }

		// Token: 0x06000357 RID: 855
		bool GetIsDisabled(out string reason);

		// Token: 0x06000358 RID: 856
		void OnActionExecuted();

		// Token: 0x06000359 RID: 857
		bool GetIsAvailable();
	}
}
