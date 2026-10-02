using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000E1 RID: 225
	public class RecruitmentAgentSpawnBehavior : CampaignBehaviorBase
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0004FABA File Offset: 0x0004DCBA
		private RecruitmentCampaignBehavior RecruitmentBehavior
		{
			get
			{
				return Campaign.Current.CampaignBehaviorManager.GetBehavior<RecruitmentCampaignBehavior>();
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0004FACC File Offset: 0x0004DCCC
		public override void RegisterEvents()
		{
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
			CampaignEvents.MercenaryNumberChangedInTown.AddNonSerializedListener(this, new Action<Town, int, int>(this.OnMercenaryNumberChanged));
			CampaignEvents.MercenaryTroopChangedInTown.AddNonSerializedListener(this, new Action<Town, CharacterObject, CharacterObject>(this.OnMercenaryTroopChanged));
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0004FB1E File Offset: 0x0004DD1E
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0004FB20 File Offset: 0x0004DD20
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			Location locationWithId = settlement.LocationComplex.GetLocationWithId("tavern");
			if (CampaignMission.Current.Location == locationWithId)
			{
				this.AddMercenaryCharacterToTavern(settlement);
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0004FB60 File Offset: 0x0004DD60
		private void CheckIfMercenaryCharacterNeedsToRefresh(Settlement settlement, CharacterObject oldTroopType)
		{
			if (settlement.IsTown && settlement == Settlement.CurrentSettlement && PlayerEncounter.LocationEncounter != null && settlement.LocationComplex != null && (CampaignMission.Current == null || GameStateManager.Current.ActiveState != CampaignMission.Current.State))
			{
				if (oldTroopType != null)
				{
					Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("tavern").RemoveAllCharacters((LocationCharacter x) => x.Character.Occupation == oldTroopType.Occupation);
				}
				this.AddMercenaryCharacterToTavern(settlement);
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0004FBEA File Offset: 0x0004DDEA
		private void OnMercenaryNumberChanged(Town town, int oldNumber, int newNumber)
		{
			if (this.RecruitmentBehavior != null)
			{
				this.CheckIfMercenaryCharacterNeedsToRefresh(town.Owner.Settlement, this.RecruitmentBehavior.GetMercenaryData(town).TroopType);
			}
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0004FC16 File Offset: 0x0004DE16
		private void OnMercenaryTroopChanged(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
			this.CheckIfMercenaryCharacterNeedsToRefresh(town.Owner.Settlement, oldTroopType);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0004FC2C File Offset: 0x0004DE2C
		private void AddMercenaryCharacterToTavern(Settlement settlement)
		{
			if (settlement.LocationComplex != null && settlement.IsTown && this.RecruitmentBehavior != null && this.RecruitmentBehavior.GetMercenaryData(settlement.Town).HasAvailableMercenary(Occupation.NotAssigned))
			{
				Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("tavern");
				if (locationWithId != null)
				{
					locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateMercenary), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
				}
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0004FC9C File Offset: 0x0004DE9C
		private LocationCharacter CreateMercenary(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject troopType = this.RecruitmentBehavior.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).TroopType;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(troopType.Race, "_settlement");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(troopType, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).NoHorses(true), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "spawnpoint_mercenary", true, relation, null, false, false, null, false, false, true, null, false);
		}
	}
}
