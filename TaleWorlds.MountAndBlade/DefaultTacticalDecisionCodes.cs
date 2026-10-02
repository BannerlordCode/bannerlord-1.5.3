using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000187 RID: 391
	public static class DefaultTacticalDecisionCodes
	{
		// Token: 0x04000598 RID: 1432
		public const byte FormationMoveToPoint = 0;

		// Token: 0x04000599 RID: 1433
		public const byte FormationDefendPoint = 1;

		// Token: 0x0400059A RID: 1434
		public const byte FormationMoveToObject = 10;

		// Token: 0x0400059B RID: 1435
		public const byte FormationAttackObject = 11;

		// Token: 0x0400059C RID: 1436
		public const byte FormationDefendObject = 12;

		// Token: 0x0400059D RID: 1437
		public const byte FormationDefendFormation = 20;

		// Token: 0x0400059E RID: 1438
		public const byte FormationAttackFormation = 21;

		// Token: 0x0400059F RID: 1439
		public const byte TeamCharge = 30;

		// Token: 0x040005A0 RID: 1440
		public const byte TeamFallbackToKeep = 31;

		// Token: 0x040005A1 RID: 1441
		public const byte TeamRetreat = 32;
	}
}
