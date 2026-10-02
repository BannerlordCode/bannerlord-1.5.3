using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Settlements.Locations
{
	// Token: 0x020003E1 RID: 993
	public class LocationCharacter
	{
		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06003BDE RID: 15326 RVA: 0x000F2C35 File Offset: 0x000F0E35
		public CharacterObject Character
		{
			get
			{
				return (CharacterObject)this.AgentData.AgentCharacter;
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06003BDF RID: 15327 RVA: 0x000F2C47 File Offset: 0x000F0E47
		public IAgentOriginBase AgentOrigin
		{
			get
			{
				return this.AgentData.AgentOrigin;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x000F2C54 File Offset: 0x000F0E54
		public AgentData AgentData { get; }

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x06003BE1 RID: 15329 RVA: 0x000F2C5C File Offset: 0x000F0E5C
		public bool UseCivilianEquipment { get; }

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x000F2C64 File Offset: 0x000F0E64
		public string ActionSetCode { get; }

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06003BE3 RID: 15331 RVA: 0x000F2C6C File Offset: 0x000F0E6C
		public string AlarmedActionSetCode { get; }

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06003BE4 RID: 15332 RVA: 0x000F2C74 File Offset: 0x000F0E74
		// (set) Token: 0x06003BE5 RID: 15333 RVA: 0x000F2C7C File Offset: 0x000F0E7C
		public string SpecialTargetTag { get; set; }

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06003BE6 RID: 15334 RVA: 0x000F2C85 File Offset: 0x000F0E85
		// (set) Token: 0x06003BE7 RID: 15335 RVA: 0x000F2C8D File Offset: 0x000F0E8D
		public bool ForceSpawnInSpecialTargetTag { get; set; }

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06003BE8 RID: 15336 RVA: 0x000F2C96 File Offset: 0x000F0E96
		public LocationCharacter.AddBehaviorsDelegate AddBehaviors { get; }

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06003BE9 RID: 15337 RVA: 0x000F2C9E File Offset: 0x000F0E9E
		public LocationCharacter.AfterAgentCreatedDelegate AfterAgentCreated { get; }

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06003BEA RID: 15338 RVA: 0x000F2CA6 File Offset: 0x000F0EA6
		public bool FixedLocation { get; }

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06003BEB RID: 15339 RVA: 0x000F2CAE File Offset: 0x000F0EAE
		// (set) Token: 0x06003BEC RID: 15340 RVA: 0x000F2CB6 File Offset: 0x000F0EB6
		public Alley MemberOfAlley { get; private set; }

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06003BED RID: 15341 RVA: 0x000F2CBF File Offset: 0x000F0EBF
		public ItemObject SpecialItem { get; }

		// Token: 0x06003BEE RID: 15342 RVA: 0x000F2CC8 File Offset: 0x000F0EC8
		public LocationCharacter(AgentData agentData, LocationCharacter.AddBehaviorsDelegate addBehaviorsDelegate, string spawnTag, bool fixedLocation, LocationCharacter.CharacterRelations characterRelation, string actionSetCode, bool useCivilianEquipment, bool isFixedCharacter = false, ItemObject specialItem = null, bool isHidden = false, bool isVisualTracked = false, bool overrideBodyProperties = true, LocationCharacter.AfterAgentCreatedDelegate afterAgentCreated = null, bool forceSpawnOnSpecialTargetTag = false)
		{
			this.AgentData = agentData;
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				int num = -2;
				if (overrideBodyProperties)
				{
					num = (isFixedCharacter ? (Settlement.CurrentSettlement.StringId + "_" + this.Character.StringId).GetDeterministicHashCode() : agentData.AgentEquipmentSeed);
				}
				this.AgentData.BodyProperties(this.Character.GetBodyProperties(this.Character.Equipment, num));
			}
			this.AddBehaviors = addBehaviorsDelegate;
			this.SpecialTargetTag = spawnTag;
			this.FixedLocation = fixedLocation;
			this.ActionSetCode = actionSetCode ?? TaleWorlds.Core.ActionSetCode.GenerateActionSetNameWithSuffix(this.AgentData.AgentMonster, this.AgentData.AgentCharacter.IsFemale, "_villager");
			this.AlarmedActionSetCode = TaleWorlds.Core.ActionSetCode.GenerateActionSetNameWithSuffix(this.AgentData.AgentMonster, this.AgentData.AgentIsFemale, "_villager");
			this.PrefabNamesForBones = new Dictionary<sbyte, string>();
			this.CharacterRelation = characterRelation;
			this.SpecialItem = specialItem;
			this.UseCivilianEquipment = useCivilianEquipment;
			this.AfterAgentCreated = afterAgentCreated;
			this.IsVisualTracked = isVisualTracked;
			if (forceSpawnOnSpecialTargetTag)
			{
				this.ForceSpawnInSpecialTargetTag = true;
			}
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x000F2DF5 File Offset: 0x000F0FF5
		public void SetAlleyOfCharacter(Alley alley)
		{
			this.MemberOfAlley = alley;
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x000F2E00 File Offset: 0x000F1000
		public static LocationCharacter CreateBodyguardHero(Hero hero, MobileParty party, LocationCharacter.AddBehaviorsDelegate addBehaviorsDelegate)
		{
			UniqueTroopDescriptor uniqueTroopDescriptor = new UniqueTroopDescriptor(FlattenedTroopRoster.GenerateUniqueNoFromParty(party, 0));
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(hero.CharacterObject.Race, "_settlement");
			return new LocationCharacter(new AgentData(new PartyAgentOrigin(PartyBase.MainParty, hero.CharacterObject, -1, uniqueTroopDescriptor, false, false)).Monster(monsterWithSuffix).NoHorses(true), addBehaviorsDelegate, "npc_common_limited", false, LocationCharacter.CharacterRelations.Friendly, null, !PlayerEncounter.LocationEncounter.Settlement.IsVillage, false, null, false, false, true, null, false);
		}

		// Token: 0x0400123E RID: 4670
		public bool IsVisualTracked;

		// Token: 0x04001247 RID: 4679
		public Dictionary<sbyte, string> PrefabNamesForBones;

		// Token: 0x04001249 RID: 4681
		public LocationCharacter.CharacterRelations CharacterRelation;

		// Token: 0x020007CE RID: 1998
		// (Invoke) Token: 0x0600660F RID: 26127
		public delegate void AddBehaviorsDelegate(IAgent agent);

		// Token: 0x020007CF RID: 1999
		// (Invoke) Token: 0x06006613 RID: 26131
		public delegate void AfterAgentCreatedDelegate(IAgent agent);

		// Token: 0x020007D0 RID: 2000
		public enum CharacterRelations
		{
			// Token: 0x04002033 RID: 8243
			Neutral,
			// Token: 0x04002034 RID: 8244
			Friendly,
			// Token: 0x04002035 RID: 8245
			Enemy
		}
	}
}
