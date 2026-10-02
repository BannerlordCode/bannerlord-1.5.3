using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace SandBox.CampaignBehaviors
{
	// Token: 0x020000E5 RID: 229
	public class StealthCharactersCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000B0B RID: 2827 RVA: 0x0005187C File Offset: 0x0004FA7C
		public override void RegisterEvents()
		{
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00051898 File Offset: 0x0004FA98
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedPoints)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			if (settlement.IsHideout)
			{
				return;
			}
			Location location = settlement.LocationComplex.GetListOfLocations().First<Location>();
			int num;
			if (unusedPoints.TryGetValue("stealth_agent", out num) && num > 0)
			{
				location.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateStealthCharacter), settlement.Culture, LocationCharacter.CharacterRelations.Enemy, num);
			}
			if (unusedPoints.TryGetValue("stealth_agent_forced", out num) && num > 0)
			{
				location.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreteForcedStealthCharacter), settlement.Culture, LocationCharacter.CharacterRelations.Enemy, num);
			}
			if (unusedPoints.TryGetValue("disguise_default_agent", out num) && num > 0)
			{
				location.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateDisguiseDefaultCharacter), settlement.Culture, LocationCharacter.CharacterRelations.Enemy, num);
			}
			if (unusedPoints.TryGetValue("disguise_officer_agent", out num) && num > 0)
			{
				location.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateDisguiseOfficerCharacter), settlement.Culture, LocationCharacter.CharacterRelations.Enemy, num);
			}
			if (unusedPoints.TryGetValue("disguise_shadow_agent", out num) && num > 0)
			{
				location.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateDisguiseShadowTargetCharacter), settlement.Culture, LocationCharacter.CharacterRelations.Enemy, num);
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x000519AB File Offset: 0x0004FBAB
		private LocationCharacter CreateStealthCharacter(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			return this.CreateStealthAgentInternal("stealth_agent", "stealth_character");
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x000519BD File Offset: 0x0004FBBD
		private LocationCharacter CreteForcedStealthCharacter(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			LocationCharacter locationCharacter = this.CreateStealthAgentInternal("stealth_agent_forced", "stealth_character");
			locationCharacter.ForceSpawnInSpecialTargetTag = true;
			return locationCharacter;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x000519D6 File Offset: 0x0004FBD6
		private LocationCharacter CreateDisguiseDefaultCharacter(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			return this.CreateStealthAgentInternal("disguise_default_agent", "disguise_default_character");
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x000519E8 File Offset: 0x0004FBE8
		private LocationCharacter CreateDisguiseOfficerCharacter(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			return this.CreateStealthAgentInternal("disguise_officer_agent", "disguise_officer_character");
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000519FA File Offset: 0x0004FBFA
		private LocationCharacter CreateDisguiseShadowTargetCharacter(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			return this.CreateStealthAgentInternal("disguise_shadow_agent", "disguise_shadow_target");
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00051A0C File Offset: 0x0004FC0C
		private LocationCharacter CreateStealthAgentInternal(string spawnTag, string characterId)
		{
			CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>(characterId);
			int num;
			int num2;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(@object, out num, out num2, "");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(@object, -1, null, default(UniqueTroopDescriptor))).Monster(FaceGen.GetMonsterWithSuffix(@object.Race, "_settlement_slow")).Age(MBRandom.RandomInt(num, num2)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddStealthAgentBehaviors), spawnTag, true, LocationCharacter.CharacterRelations.Enemy, null, true, false, null, false, false, true, null, false);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00051AA1 File Offset: 0x0004FCA1
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
