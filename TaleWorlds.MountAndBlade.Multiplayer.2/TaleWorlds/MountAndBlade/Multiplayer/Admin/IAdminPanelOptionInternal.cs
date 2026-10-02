using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000071 RID: 113
	internal interface IAdminPanelOptionInternal
	{
		// Token: 0x0600036C RID: 876
		MultiplayerOptions.OptionType GetOptionType();

		// Token: 0x0600036D RID: 877
		MultiplayerOptions.MultiplayerOptionsAccessMode GetOptionAccessMode();

		// Token: 0x0600036E RID: 878
		void OnApplyChanges();

		// Token: 0x0600036F RID: 879
		void AddValueChangedCallback(Action callback);

		// Token: 0x06000370 RID: 880
		void RemoveValueChangedCallback(Action callback);

		// Token: 0x06000371 RID: 881
		void OnFinalize();
	}
}
