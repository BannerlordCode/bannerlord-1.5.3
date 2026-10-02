using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000414 RID: 1044
	public class HeroAgentSpawnCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600428C RID: 17036 RVA: 0x0012F808 File Offset: 0x0012DA08
		public override void RegisterEvents()
		{
			CampaignEvents.PrisonersChangeInSettlement.AddNonSerializedListener(this, new Action<Settlement, FlattenedTroopRoster, Hero, bool>(this.OnPrisonersChangeInSettlement));
			CampaignEvents.OnGovernorChangedEvent.AddNonSerializedListener(this, new Action<Town, Hero, Hero>(this.OnGovernorChanged));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x0012F8B6 File Offset: 0x0012DAB6
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x0012F8B8 File Offset: 0x0012DAB8
		private void RefreshLocationOfHeroesForPlayersCurrentSettlement()
		{
			if (LocationComplex.Current != null && Settlement.CurrentSettlement != null && (Settlement.CurrentSettlement.IsFortification || Settlement.CurrentSettlement.IsVillage) && LocationComplex.Current == Settlement.CurrentSettlement.LocationComplex)
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				List<Hero> list = currentSettlement.HeroesWithoutParty.ToList<Hero>();
				Hero hero = (currentSettlement.MapFaction.IsKingdomFaction ? ((Kingdom)currentSettlement.MapFaction).Leader : currentSettlement.OwnerClan.Leader);
				Hero hero2 = ((hero != null) ? hero.Spouse : null);
				if (hero != null)
				{
					list.Add(hero);
				}
				if (hero2 != null)
				{
					list.Add(hero2);
				}
				list.AddRange(Clan.PlayerClan.AliveLords);
				list.AddRange(Hero.MainHero.CompanionsInParty);
				list.AddRange(from x in currentSettlement.SettlementComponent.GetPrisonerHeroes()
					select x.HeroObject);
				foreach (MobileParty mobileParty in currentSettlement.Parties)
				{
					if (mobileParty.LeaderHero != null && mobileParty.LeaderHero != Hero.MainHero)
					{
						list.Add(mobileParty.LeaderHero);
					}
				}
				foreach (Hero hero3 in list)
				{
					this.RefreshLocationOfHeroForSettlement(hero3, currentSettlement);
				}
			}
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x0012FA64 File Offset: 0x0012DC64
		private void RefreshLocationOfHeroForSettlement(Hero hero, Settlement settlement)
		{
			Location locationOfCharacter = settlement.LocationComplex.GetLocationOfCharacter(hero);
			HeroAgentLocationModel.HeroLocationDetail heroLocationDetail;
			Location locationForHero = Campaign.Current.Models.HeroAgentLocationModel.GetLocationForHero(hero, settlement, out heroLocationDetail);
			if (locationOfCharacter == null && locationForHero != null)
			{
				LocationCharacter locationCharacter = this.CreateLocationCharacterForHero(hero, settlement, heroLocationDetail);
				locationForHero.AddCharacter(locationCharacter);
				return;
			}
			if (locationOfCharacter != null && locationOfCharacter != locationForHero)
			{
				LocationCharacter locationCharacterOfHero = settlement.LocationComplex.GetLocationCharacterOfHero(hero);
				settlement.LocationComplex.ChangeLocation(locationCharacterOfHero, locationOfCharacter, locationForHero);
			}
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x0012FAD4 File Offset: 0x0012DCD4
		private void SetAgentDataProperties(Hero hero, HeroAgentLocationModel.HeroLocationDetail locationReason, ref AgentData agentData)
		{
			Monster monster = new Monster();
			if (locationReason == HeroAgentLocationModel.HeroLocationDetail.PlayerClanMember || locationReason == HeroAgentLocationModel.HeroLocationDetail.MainPartyCompanion)
			{
				monster = FaceGen.GetBaseMonsterFromRace(hero.CharacterObject.Race);
			}
			else
			{
				monster = FaceGen.GetMonsterWithSuffix(hero.CharacterObject.Race, "_settlement");
			}
			agentData.Monster(monster);
			agentData.NoHorses(true);
			if (locationReason != HeroAgentLocationModel.HeroLocationDetail.Wanderer)
			{
				IFaction mapFaction = hero.MapFaction;
				uint num = ((mapFaction != null) ? mapFaction.Color : 4291609515U);
				IFaction mapFaction2 = hero.MapFaction;
				uint num2 = ((mapFaction2 != null) ? mapFaction2.Color : 4291609515U);
				agentData.ClothingColor1(num).ClothingColor2(num2);
			}
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x0012FB6C File Offset: 0x0012DD6C
		private LocationCharacter CreateLocationCharacterForHero(Hero hero, Settlement settlement, HeroAgentLocationModel.HeroLocationDetail heroLocationDetail)
		{
			AgentData agentData = null;
			if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.NobleBelongingToNoParty || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.Prisoner)
			{
				agentData = new AgentData(new SimpleAgentOrigin(hero.CharacterObject, -1, null, default(UniqueTroopDescriptor)));
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PlayerClanMember || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.MainPartyCompanion)
			{
				agentData = new AgentData(new PartyAgentOrigin(PartyBase.MainParty, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false, false));
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PartyLeader)
			{
				agentData = new AgentData(new PartyAgentOrigin(hero.PartyBelongedTo.Party, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false, false));
			}
			else
			{
				agentData = new AgentData(new PartyAgentOrigin(null, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false, false));
			}
			this.SetAgentDataProperties(hero, heroLocationDetail, ref agentData);
			LocationCharacter.AddBehaviorsDelegate addBehaviorsDelegate = ((heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PlayerClanMember || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.MainPartyCompanion) ? new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddCompanionBehaviors) : new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors));
			string text = "";
			bool flag = false;
			if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.SettlementKingQueen)
			{
				text = "sp_throne";
				flag = true;
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.Prisoner)
			{
				text = "sp_prisoner";
				flag = true;
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.Notable)
			{
				if (settlement.IsFortification)
				{
					text = (hero.IsArtisan ? "sp_notable_artisan" : (hero.IsMerchant ? "sp_notable_merchant" : (hero.IsPreacher ? "sp_notable_preacher" : (hero.IsGangLeader ? "sp_notable_gangleader" : (hero.IsRuralNotable ? "sp_notable_rural_notable" : ((hero.GovernorOf == hero.CurrentSettlement.Town) ? "sp_governor" : "sp_notable"))))));
					MBReadOnlyList<Workshop> ownedWorkshops = hero.OwnedWorkshops;
					if (ownedWorkshops.Count != 0)
					{
						for (int i = 0; i < ownedWorkshops.Count; i++)
						{
							if (!ownedWorkshops[i].WorkshopType.IsHidden)
							{
								text = text + "_" + ownedWorkshops[i].Tag;
								break;
							}
						}
					}
				}
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PartylessHeroInsideVillage)
			{
				text = "sp_notable_rural_notable";
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.Wanderer || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.MainPartyCompanion)
			{
				text = "npc_common_limited";
			}
			else
			{
				text = "sp_notable";
			}
			bool flag2 = heroLocationDetail != HeroAgentLocationModel.HeroLocationDetail.PartylessHeroInsideVillage;
			LocationCharacter.CharacterRelations characterRelations = LocationCharacter.CharacterRelations.Neutral;
			if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PlayerClanMember || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.MainPartyCompanion)
			{
				characterRelations = LocationCharacter.CharacterRelations.Friendly;
			}
			string text2 = "";
			if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.SettlementKingQueen || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.NobleBelongingToNoParty || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PartyLeader)
			{
				text2 = ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, hero.IsFemale, "_lord");
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.Prisoner)
			{
				text2 = ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, hero.IsFemale, "_villager");
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.Notable)
			{
				if (settlement.IsFortification)
				{
					string text3 = (hero.IsArtisan ? "_villager_artisan" : (hero.IsMerchant ? "_villager_merchant" : (hero.IsPreacher ? "_villager_preacher" : (hero.IsGangLeader ? "_villager_gangleader" : (hero.IsRuralNotable ? "_villager_ruralnotable" : (hero.IsFemale ? "_lord" : "_villager_merchant"))))));
					text2 = ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, hero.IsFemale, text3);
				}
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PartylessHeroInsideVillage)
			{
				text2 = null;
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.Wanderer)
			{
				text2 = ((settlement.Culture.StringId.ToLower() == "aserai" || settlement.Culture.StringId.ToLower() == "khuzait") ? ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, hero.IsFemale, "_warrior_in_aserai_tavern") : ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, hero.IsFemale, "_warrior_in_tavern"));
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.MainPartyCompanion || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PlayerClanMember)
			{
				text2 = null;
			}
			else
			{
				Debug.FailedAssert("action Set Code is not set properly with a location reason!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\HeroAgentSpawnCampaignBehavior.cs", "CreateLocationCharacterForHero", 282);
			}
			bool flag3 = true;
			if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PlayerClanMember || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.MainPartyCompanion)
			{
				flag3 = !PlayerEncounter.LocationEncounter.Settlement.IsVillage;
			}
			else if (heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PartyLeader)
			{
				flag3 = !settlement.IsVillage;
			}
			bool flag4 = heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.PlayerClanMember || heroLocationDetail == HeroAgentLocationModel.HeroLocationDetail.MainPartyCompanion;
			return new LocationCharacter(agentData, addBehaviorsDelegate, text, flag2, characterRelations, text2, flag3, false, null, false, flag4, true, null, flag);
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x0012FF74 File Offset: 0x0012E174
		private void OnGovernorChanged(Town town, Hero oldGovernor, Hero newGovernor)
		{
			if (LocationComplex.Current != null)
			{
				if (oldGovernor != null)
				{
					this.RefreshLocationOfHeroForSettlement(oldGovernor, town.Settlement);
				}
				if (newGovernor != null)
				{
					this.RefreshLocationOfHeroForSettlement(newGovernor, town.Settlement);
				}
			}
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x0012FF9D File Offset: 0x0012E19D
		private void OnMissionEnded(IMission mission)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && Settlement.CurrentSettlement != null && !Hero.MainHero.IsPrisoner && !Settlement.CurrentSettlement.IsUnderSiege)
			{
				this.RefreshLocationOfHeroesForPlayersCurrentSettlement();
			}
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x0012FFD2 File Offset: 0x0012E1D2
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && settlement.LocationComplex == LocationComplex.Current)
			{
				this.RefreshLocationOfHeroesForPlayersCurrentSettlement();
			}
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x0012FFF5 File Offset: 0x0012E1F5
		public void OnSettlementLeft(MobileParty mobileParty, Settlement settlement)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && settlement.LocationComplex == LocationComplex.Current && mobileParty != MobileParty.MainParty && mobileParty.LeaderHero != null)
			{
				this.RefreshLocationOfHeroForSettlement(mobileParty.LeaderHero, settlement);
			}
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x0013002F File Offset: 0x0012E22F
		private void OnGameLoadFinished()
		{
			if (!Hero.MainHero.IsPrisoner && Settlement.CurrentSettlement != null && !Settlement.CurrentSettlement.IsUnderSiege)
			{
				this.RefreshLocationOfHeroesForPlayersCurrentSettlement();
			}
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x00130056 File Offset: 0x0012E256
		private void OnHeroPrisonerTaken(PartyBase capturerParty, Hero prisoner)
		{
			if (capturerParty.IsSettlement)
			{
				this.OnPrisonersChangeInSettlement(capturerParty.Settlement, null, prisoner, false);
			}
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x00130070 File Offset: 0x0012E270
		public void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
			if (settlement != null && settlement.IsFortification && LocationComplex.Current == settlement.LocationComplex)
			{
				if (prisonerHero != null && prisonerHero != Hero.OneToOneConversationHero)
				{
					this.RefreshLocationOfHeroForSettlement(prisonerHero, settlement);
				}
				if (prisonerRoster != null)
				{
					foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in prisonerRoster)
					{
						if (flattenedTroopRosterElement.Troop.IsHero && prisonerHero != Hero.OneToOneConversationHero)
						{
							this.RefreshLocationOfHeroForSettlement(flattenedTroopRosterElement.Troop.HeroObject, settlement);
						}
					}
				}
			}
		}
	}
}
