using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200043D RID: 1085
	public class NotableHelperCharacterCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060045E0 RID: 17888 RVA: 0x001535B8 File Offset: 0x001517B8
		public override void RegisterEvents()
		{
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
			CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x0015360A File Offset: 0x0015180A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x0015360C File Offset: 0x0015180C
		private void OnMissionEnded(IMission mission)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && Settlement.CurrentSettlement != null && !Hero.MainHero.IsPrisoner && !Settlement.CurrentSettlement.IsUnderSiege)
			{
				this._addNotableHelperCharacters = true;
			}
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x00153642 File Offset: 0x00151842
		private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && mobileParty != null && mobileParty == MobileParty.MainParty)
			{
				this._addNotableHelperCharacters = true;
			}
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x00153664 File Offset: 0x00151864
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			Location locationWithId = LocationComplex.Current.GetLocationWithId("center");
			Location locationWithId2 = LocationComplex.Current.GetLocationWithId("village_center");
			if (this._addNotableHelperCharacters && (CampaignMission.Current.Location == locationWithId || CampaignMission.Current.Location == locationWithId2))
			{
				this.SpawnNotableHelperCharacters(settlement);
				this._addNotableHelperCharacters = false;
			}
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x001536CC File Offset: 0x001518CC
		private void SpawnNotableHelperCharacters(Settlement settlement)
		{
			int num = settlement.Notables.Count<Hero>((Hero x) => x.IsGangLeader);
			int num2 = settlement.Notables.Count<Hero>((Hero x) => x.IsPreacher);
			int num3 = settlement.Notables.Count<Hero>((Hero x) => x.IsArtisan);
			int num4 = settlement.Notables.Count<Hero>((Hero x) => x.IsRuralNotable || x.IsHeadman);
			int num5 = settlement.Notables.Count<Hero>((Hero x) => x.IsMerchant);
			this.SpawnNotableHelperCharacter(settlement.Culture.GangleaderBodyguard, "_gangleader_bodyguard", "sp_gangleader_bodyguard", num * 2);
			this.SpawnNotableHelperCharacter(settlement.Culture.PreacherNotary, "_merchant_notary", "sp_preacher_notary", num2);
			this.SpawnNotableHelperCharacter(settlement.Culture.ArtisanNotary, "_merchant_notary", "sp_artisan_notary", num3);
			this.SpawnNotableHelperCharacter(settlement.Culture.RuralNotableNotary, "_merchant_notary", "sp_rural_notable_notary", num4);
			this.SpawnNotableHelperCharacter(settlement.Culture.MerchantNotary, "_merchant_notary", "sp_merchant_notary", num5);
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x00153840 File Offset: 0x00151A40
		private void SpawnNotableHelperCharacter(CharacterObject character, string actionSetSuffix, string tag, int characterToSpawnCount)
		{
			Location location = LocationComplex.Current.GetLocationWithId("center") ?? LocationComplex.Current.GetLocationWithId("village_center");
			while (characterToSpawnCount > 0)
			{
				Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(character.Race, "_settlement");
				int num;
				int num2;
				Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(character, out num, out num2, "Notary");
				AgentData agentData = new AgentData(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).NoHorses(true).Age(MBRandom.RandomInt(num, num2));
				LocationCharacter locationCharacter = new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), tag, true, LocationCharacter.CharacterRelations.Neutral, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, actionSetSuffix), true, false, null, false, false, true, null, false);
				location.AddCharacter(locationCharacter);
				characterToSpawnCount--;
			}
		}

		// Token: 0x04001420 RID: 5152
		private bool _addNotableHelperCharacters;
	}
}
