using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000E7 RID: 231
	public class TownMerchantsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000B50 RID: 2896 RVA: 0x00053961 File Offset: 0x00051B61
		public override void RegisterEvents()
		{
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0005397A File Offset: 0x00051B7A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0005397C File Offset: 0x00051B7C
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Location locationWithId = PlayerEncounter.LocationEncounter.Settlement.LocationComplex.GetLocationWithId("center");
			if (CampaignMission.Current.Location == locationWithId && Campaign.Current.IsDay)
			{
				this.AddTradersToCenter(unusedUsablePointCount);
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x000539C4 File Offset: 0x00051BC4
		private void AddTradersToCenter(Dictionary<string, int> unusedUsablePointCount)
		{
			Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("center");
			int num;
			if (unusedUsablePointCount.TryGetValue("sp_merchant", out num))
			{
				locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(TownMerchantsCampaignBehavior.CreateMerchant), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, num);
			}
			if (unusedUsablePointCount.TryGetValue("sp_horse_merchant", out num))
			{
				locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(TownMerchantsCampaignBehavior.CreateHorseTrader), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, num);
			}
			if (unusedUsablePointCount.TryGetValue("sp_armorer", out num))
			{
				locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(TownMerchantsCampaignBehavior.CreateArmorer), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, num);
			}
			if (unusedUsablePointCount.TryGetValue("sp_weaponsmith", out num))
			{
				locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(TownMerchantsCampaignBehavior.CreateWeaponsmith), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, num);
			}
			if (unusedUsablePointCount.TryGetValue("sp_blacksmith", out num))
			{
				locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(TownMerchantsCampaignBehavior.CreateBlacksmith), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, num);
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00053AC8 File Offset: 0x00051CC8
		private static LocationCharacter CreateBlacksmith(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject blacksmith = culture.Blacksmith;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(blacksmith.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(blacksmith, out num, out num2, "");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(blacksmith, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_blacksmith", true, relation, null, true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00053B60 File Offset: 0x00051D60
		private static LocationCharacter CreateMerchant(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject merchant = culture.Merchant;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(merchant.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(merchant, out num, out num2, "");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(merchant, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_merchant", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_seller"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00053C14 File Offset: 0x00051E14
		private static LocationCharacter CreateHorseTrader(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject horseMerchant = culture.HorseMerchant;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(horseMerchant.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(horseMerchant, out num, out num2, "");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(horseMerchant, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_horse_merchant", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_seller"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00053CC8 File Offset: 0x00051EC8
		private static LocationCharacter CreateArmorer(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject armorer = culture.Armorer;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(armorer.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(armorer, out num, out num2, "");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(armorer, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_armorer", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_seller"), true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00053D7C File Offset: 0x00051F7C
		private static LocationCharacter CreateWeaponsmith(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject weaponsmith = culture.Weaponsmith;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(weaponsmith.Race, "_settlement");
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(weaponsmith, out num, out num2, "");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(weaponsmith, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(num, num2));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_weaponsmith", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_weaponsmith"), true, false, null, false, false, true, null, false);
		}
	}
}
