using System;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x02000301 RID: 769
	public enum PlayerEncounterState
	{
		// Token: 0x04000C51 RID: 3153
		Begin,
		// Token: 0x04000C52 RID: 3154
		Wait,
		// Token: 0x04000C53 RID: 3155
		PrepareResults,
		// Token: 0x04000C54 RID: 3156
		ApplyResults,
		// Token: 0x04000C55 RID: 3157
		PlayerVictory,
		// Token: 0x04000C56 RID: 3158
		PlayerTotalDefeat,
		// Token: 0x04000C57 RID: 3159
		CaptureHeroes,
		// Token: 0x04000C58 RID: 3160
		FreeHeroes,
		// Token: 0x04000C59 RID: 3161
		LootParty,
		// Token: 0x04000C5A RID: 3162
		LootInventory,
		// Token: 0x04000C5B RID: 3163
		LootShips,
		// Token: 0x04000C5C RID: 3164
		End
	}
}
