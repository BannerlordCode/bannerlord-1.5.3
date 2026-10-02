using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Missions.BattleScore
{
	// Token: 0x020003F9 RID: 1017
	public abstract class BattleScoreContext
	{
		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06003817 RID: 14359
		public abstract bool IsPowerComparisonRelevant { get; }

		// Token: 0x06003818 RID: 14360
		public abstract Banner GetAttackerBanner();

		// Token: 0x06003819 RID: 14361
		public abstract Banner GetDefenderBanner();
	}
}
