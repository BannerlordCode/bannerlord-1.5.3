using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006D RID: 109
	public interface IAdminPanelOption
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600035B RID: 859
		string UniqueId { get; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600035C RID: 860
		string Name { get; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600035D RID: 861
		string Description { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600035E RID: 862
		bool RequiresMissionRestart { get; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600035F RID: 863
		bool IsRequired { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000360 RID: 864
		bool IsDirty { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000361 RID: 865
		bool CanRevertToDefaultValue { get; }

		// Token: 0x06000362 RID: 866
		bool GetIsDisabled(out string reason);

		// Token: 0x06000363 RID: 867
		bool GetIsAvailable();

		// Token: 0x06000364 RID: 868
		void RevertChanges();

		// Token: 0x06000365 RID: 869
		void RestoreDefaults();

		// Token: 0x06000366 RID: 870
		void SetOnRefreshCallback(Action callback);
	}
}
