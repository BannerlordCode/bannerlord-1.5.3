using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F1 RID: 497
	public abstract class SiegeLordsHallFightModel : MBGameModel<SiegeLordsHallFightModel>
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06001FA8 RID: 8104
		public abstract float AreaLostRatio { get; }

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06001FA9 RID: 8105
		public abstract float AttackerDefenderTroopCountRatio { get; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06001FAA RID: 8106
		public abstract int DefenderTroopNumberForSuccessfulPullBack { get; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06001FAB RID: 8107
		public abstract float DefenderMaxArcherRatio { get; }

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06001FAC RID: 8108
		public abstract int MaxDefenderSideTroopCount { get; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06001FAD RID: 8109
		public abstract int MaxDefenderArcherCount { get; }

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06001FAE RID: 8110
		public abstract int MaxAttackerSideTroopCount { get; }

		// Token: 0x06001FAF RID: 8111
		public abstract FlattenedTroopRoster GetPriorityListForLordsHallFightMission(MapEvent playerMapEvent, BattleSideEnum side, int troopCount);
	}
}
