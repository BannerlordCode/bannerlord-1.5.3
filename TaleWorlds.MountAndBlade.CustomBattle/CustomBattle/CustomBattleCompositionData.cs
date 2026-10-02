using System;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x02000017 RID: 23
	public struct CustomBattleCompositionData
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00008716 File Offset: 0x00006916
		public CustomBattleCompositionData(float rangedPercentage, float cavalryPercentage, float rangedCavalryPercentage)
		{
			this.RangedPercentage = rangedPercentage;
			this.CavalryPercentage = cavalryPercentage;
			this.RangedCavalryPercentage = rangedCavalryPercentage;
			this.IsValid = true;
		}

		// Token: 0x040000AF RID: 175
		public readonly bool IsValid;

		// Token: 0x040000B0 RID: 176
		public readonly float RangedPercentage;

		// Token: 0x040000B1 RID: 177
		public readonly float CavalryPercentage;

		// Token: 0x040000B2 RID: 178
		public readonly float RangedCavalryPercentage;
	}
}
