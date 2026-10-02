using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E8 RID: 488
	public abstract class TroopSacrificeModel : MBGameModel<TroopSacrificeModel>
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06001F5D RID: 8029
		public abstract int BreakOutArmyLeaderRelationPenalty { get; }

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06001F5E RID: 8030
		public abstract int BreakOutArmyMemberRelationPenalty { get; }

		// Token: 0x06001F5F RID: 8031
		public abstract ExplainedNumber GetLostTroopCountForBreakingInBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent);

		// Token: 0x06001F60 RID: 8032
		public abstract ExplainedNumber GetLostTroopCountForBreakingOutOfBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent, bool isBreakingOutFromPort);

		// Token: 0x06001F61 RID: 8033
		public abstract int GetNumberOfTroopsSacrificedForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent);

		// Token: 0x06001F62 RID: 8034
		public abstract void GetShipsToSacrificeForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent, out MBList<Ship> shipsToCapture, out Ship shipToTakeDamage, out float damageToApplyForLastShip);

		// Token: 0x06001F63 RID: 8035
		public abstract bool CanPlayerGetAwayFromEncounter(out TextObject explanation);
	}
}
