using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x0200006A RID: 106
	public interface IAdminPanelMultiSelectionItem
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600034F RID: 847
		string Value { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000350 RID: 848
		string DisplayName { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000351 RID: 849
		bool IsFallbackValue { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000352 RID: 850
		bool IsDisabled { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000353 RID: 851
		bool CanBeApplied { get; }
	}
}
