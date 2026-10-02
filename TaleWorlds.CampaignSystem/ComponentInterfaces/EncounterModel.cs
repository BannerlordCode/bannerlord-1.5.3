using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000191 RID: 401
	public abstract class EncounterModel : MBGameModel<EncounterModel>
	{
		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001C8F RID: 7311
		public abstract float NeededMaximumLandDistanceForEncounteringMobileParty { get; }

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001C90 RID: 7312
		public abstract float NeededMaximumNavalDistanceForEncounteringMobileParty { get; }

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001C91 RID: 7313
		public abstract float MaximumAllowedLandDistanceForEncounteringMobilePartyInArmy { get; }

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001C92 RID: 7314
		public abstract float MaximumAllowedNavalDistanceForEncounteringMobilePartyInArmy { get; }

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001C93 RID: 7315
		public abstract float NeededMaximumDistanceForEncounteringTown { get; }

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06001C94 RID: 7316
		public abstract float NeededMaximumDistanceForEncounteringBlockade { get; }

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06001C95 RID: 7317
		public abstract float NeededMaximumDistanceForEncounteringVillage { get; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06001C96 RID: 7318
		public abstract float GetEncounterJoiningRadius { get; }

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001C97 RID: 7319
		public abstract float GetSettlementBeingNearFieldBattleRadius { get; }

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06001C98 RID: 7320
		public abstract float PlayerParleyDistance { get; }

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06001C99 RID: 7321
		public abstract int MinimumNumberOfMenForAttackingVillageViaScene { get; }

		// Token: 0x06001C9A RID: 7322
		public abstract bool IsEncounterExemptFromHostileActions(PartyBase side1, PartyBase side2);

		// Token: 0x06001C9B RID: 7323
		public abstract bool CanMainHeroDoParleyWithParty(PartyBase partyBase, out TextObject explanation);

		// Token: 0x06001C9C RID: 7324
		public abstract Hero GetLeaderOfSiegeEvent(SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001C9D RID: 7325
		public abstract Hero GetLeaderOfMapEvent(MapEvent mapEvent, BattleSideEnum side);

		// Token: 0x06001C9E RID: 7326
		public abstract int GetCharacterSergeantScore(Hero hero);

		// Token: 0x06001C9F RID: 7327
		public abstract IEnumerable<PartyBase> GetDefenderPartiesOfSettlement(Settlement settlement, MapEvent.BattleTypes mapEventType);

		// Token: 0x06001CA0 RID: 7328
		public abstract PartyBase GetNextDefenderPartyOfSettlement(Settlement settlement, ref int partyIndex, MapEvent.BattleTypes mapEventType);

		// Token: 0x06001CA1 RID: 7329
		public abstract MapEventComponent CreateMapEventComponentForEncounter(PartyBase attackerParty, PartyBase defenderParty, MapEvent.BattleTypes battleType);

		// Token: 0x06001CA2 RID: 7330
		public abstract ExplainedNumber GetBribeChance(MobileParty defenderParty, MobileParty attackerParty);

		// Token: 0x06001CA3 RID: 7331
		public abstract float GetSurrenderChance(MobileParty defenderParty, MobileParty attackerParty);

		// Token: 0x06001CA4 RID: 7332
		public abstract float GetMapEventSideRunAwayChance(MapEventSide mapEventside);

		// Token: 0x06001CA5 RID: 7333
		public abstract void FindNonAttachedNpcPartiesWhoWillJoinPlayerEncounter(List<MobileParty> partiesToJoinPlayerSide, List<MobileParty> partiesToJoinEnemySide);

		// Token: 0x06001CA6 RID: 7334
		public abstract bool CanPlayerForceBanditsToJoin(out TextObject explanation);

		// Token: 0x06001CA7 RID: 7335
		public abstract bool IsPartyUnderPlayerCommand(PartyBase party);

		// Token: 0x06001CA8 RID: 7336
		public abstract MBReadOnlyList<MobileParty> GetPartiesToTeleportOnMapEventFinalize(MapEvent mapEvent);
	}
}
