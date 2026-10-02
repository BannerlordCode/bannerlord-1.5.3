using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000083 RID: 131
	[Flags]
	public enum CharacterRestrictionFlags : uint
	{
		// Token: 0x040004F5 RID: 1269
		None = 0U,
		// Token: 0x040004F6 RID: 1270
		NotTransferableInPartyScreen = 1U,
		// Token: 0x040004F7 RID: 1271
		CanNotGoInHideout = 2U
	}
}
