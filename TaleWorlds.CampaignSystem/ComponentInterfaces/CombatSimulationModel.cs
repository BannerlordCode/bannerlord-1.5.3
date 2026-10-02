using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A8 RID: 424
	public abstract class CombatSimulationModel : MBGameModel<CombatSimulationModel>
	{
		// Token: 0x06001D38 RID: 7480
		public abstract ExplainedNumber SimulateHit(CharacterObject strikerTroop, CharacterObject struckTroop, PartyBase strikerParty, PartyBase struckParty, float strikerAdvantage, MapEvent battle, BattleEnvironment battleEnvironment, float strikerSideMorale, float struckSideMorale);

		// Token: 0x06001D39 RID: 7481
		public abstract ExplainedNumber SimulateHit(Ship strikerShip, Ship struckShip, PartyBase strikerParty, PartyBase struckParty, SiegeEngineType siegeEngine, float strikerAdvantage, MapEvent battle, out int troopCasualties);

		// Token: 0x06001D3A RID: 7482
		[return: TupleElementNames(new string[] { "defenderRounds", "attackerRounds" })]
		public abstract ValueTuple<int, int> GetSimulationTicksForBattleRound(MapEvent mapEvent);

		// Token: 0x06001D3B RID: 7483
		public abstract int GetNumberOfEquipmentsBuilt(Settlement settlement);

		// Token: 0x06001D3C RID: 7484
		public abstract float GetMaximumSiegeEquipmentProgress(Settlement settlement);

		// Token: 0x06001D3D RID: 7485
		public abstract float GetSettlementAdvantage(Settlement settlement);

		// Token: 0x06001D3E RID: 7486
		public abstract void GetBattleAdvantage(MapEvent mapEvent, out ExplainedNumber defenderAdvantage, out ExplainedNumber attackerAdvantage);

		// Token: 0x06001D3F RID: 7487
		public abstract float GetShipSiegeEngineHitChance(Ship ship, SiegeEngineType siegeEngineType, BattleSideEnum battleSide);

		// Token: 0x06001D40 RID: 7488
		public abstract int GetPursuitRoundCount(MapEvent mapEvent);

		// Token: 0x06001D41 RID: 7489
		public abstract float GetBluntDamageChance(CharacterObject strikerTroop, CharacterObject strikedTroop, PartyBase strikerParty, PartyBase strikedParty, MapEvent battle);

		// Token: 0x06001D42 RID: 7490
		public abstract CampaignTime GetSimulationTickInterval(MapEvent mapEvent);

		// Token: 0x06001D43 RID: 7491
		public abstract int GetParticipatingTroopCount(MapEventSide side);

		// Token: 0x06001D44 RID: 7492
		public abstract float GetShipCombatImportance(Ship ship);

		// Token: 0x06001D45 RID: 7493
		public abstract float GetShipCombatScore(Ship ship);
	}
}
