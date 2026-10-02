using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EF RID: 495
	public abstract class SiegeEventModel : MBGameModel<SiegeEventModel>
	{
		// Token: 0x06001F8E RID: 8078
		public abstract int GetSiegeEngineDestructionCasualties(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType destroyedSiegeEngine);

		// Token: 0x06001F8F RID: 8079
		public abstract float GetCasualtyChance(MobileParty siegeParty, SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001F90 RID: 8080
		public abstract int GetColleteralDamageCasualties(SiegeEngineType attackerSiegeEngine, MobileParty attackerParty);

		// Token: 0x06001F91 RID: 8081
		public abstract float GetSiegeEngineHitChance(SiegeEngineType siegeEngineType, BattleSideEnum battleSide, SiegeBombardTargets target, Town town);

		// Token: 0x06001F92 RID: 8082
		public abstract string GetSiegeEngineMapPrefabName(SiegeEngineType siegeEngineType, int wallLevel, BattleSideEnum side);

		// Token: 0x06001F93 RID: 8083
		public abstract string GetSiegeEngineMapProjectilePrefabName(SiegeEngineType siegeEngineType);

		// Token: 0x06001F94 RID: 8084
		public abstract string GetSiegeEngineMapReloadAnimationName(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001F95 RID: 8085
		public abstract string GetSiegeEngineMapFireAnimationName(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001F96 RID: 8086
		public abstract sbyte GetSiegeEngineMapProjectileBoneIndex(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001F97 RID: 8087
		public abstract float GetSiegeStrategyScore(SiegeEvent siege, BattleSideEnum side, SiegeStrategy strategy);

		// Token: 0x06001F98 RID: 8088
		public abstract float GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side);

		// Token: 0x06001F99 RID: 8089
		public abstract MobileParty GetEffectiveSiegePartyForSide(SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001F9A RID: 8090
		public abstract float GetAvailableManDayPower(ISiegeEventSide side);

		// Token: 0x06001F9B RID: 8091
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerRangedSiegeEngines(PartyBase party);

		// Token: 0x06001F9C RID: 8092
		public abstract IEnumerable<SiegeEngineType> GetAvailableDefenderSiegeEngines(PartyBase party);

		// Token: 0x06001F9D RID: 8093
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerRamSiegeEngines(PartyBase party);

		// Token: 0x06001F9E RID: 8094
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerTowerSiegeEngines(PartyBase party);

		// Token: 0x06001F9F RID: 8095
		public abstract IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSettlement(Settlement settlement);

		// Token: 0x06001FA0 RID: 8096
		public abstract IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSiegeCamp(BesiegerCamp camp);

		// Token: 0x06001FA1 RID: 8097
		public abstract float GetSiegeEngineHitPoints(SiegeEvent siegeEvent, SiegeEngineType siegeEngine, BattleSideEnum battleSide);

		// Token: 0x06001FA2 RID: 8098
		public abstract int GetRangedSiegeEngineReloadTime(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine);

		// Token: 0x06001FA3 RID: 8099
		public abstract float GetSiegeEngineDamage(SiegeEvent siegeEvent, BattleSideEnum battleSide, SiegeEngineType siegeEngine, SiegeBombardTargets target);

		// Token: 0x06001FA4 RID: 8100
		public abstract FlattenedTroopRoster GetPriorityTroopsForSallyOutAmbush();
	}
}
