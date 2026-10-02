using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Missions.MissionLogics.Hideout;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Tutorial
{
	// Token: 0x02000016 RID: 22
	public static class TutorialHelper
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00009BD4 File Offset: 0x00007DD4
		public static bool PlayerIsInAnySettlement
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				return currentSettlement != null && (currentSettlement.IsFortification || currentSettlement.IsVillage);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00009BFC File Offset: 0x00007DFC
		public static bool PlayerIsInAnyVillage
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				return currentSettlement != null && currentSettlement.IsVillage;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00009C10 File Offset: 0x00007E10
		public static bool IsOrderingAvailable
		{
			get
			{
				Mission mission = Mission.Current;
				if (((mission != null) ? mission.PlayerTeam : null) != null)
				{
					for (int i = 0; i < Mission.Current.PlayerTeam.FormationsIncludingEmpty.Count; i++)
					{
						Formation formation = Mission.Current.PlayerTeam.FormationsIncludingEmpty[i];
						if (formation.PlayerOwner == Agent.Main && formation.CountOfUnits > 0)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00009C7E File Offset: 0x00007E7E
		public static bool IsCharacterPopUpWindowOpen
		{
			get
			{
				return GauntletTutorialSystem.Current.IsCharacterPortraitPopupOpen;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00009C8A File Offset: 0x00007E8A
		public static EncyclopediaPages CurrentEncyclopediaPage
		{
			get
			{
				return GauntletTutorialSystem.Current.CurrentEncyclopediaPageContext;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00009C96 File Offset: 0x00007E96
		public static TutorialContexts CurrentContext
		{
			get
			{
				return GauntletTutorialSystem.Current.CurrentContext;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00009CA4 File Offset: 0x00007EA4
		public static bool PlayerIsInNonEnemyTown
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				return currentSettlement != null && currentSettlement.IsTown && !FactionManager.IsAtWarAgainstFaction(currentSettlement.MapFaction, MobileParty.MainParty.MapFaction);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00009CDC File Offset: 0x00007EDC
		public static string ActiveVillageRaidGameMenuID
		{
			get
			{
				return "raiding_village";
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00009CE3 File Offset: 0x00007EE3
		public static bool IsActiveVillageRaidGameMenuOpen
		{
			get
			{
				Campaign campaign = Campaign.Current;
				string text;
				if (campaign == null)
				{
					text = null;
				}
				else
				{
					MenuContext currentMenuContext = campaign.CurrentMenuContext;
					if (currentMenuContext == null)
					{
						text = null;
					}
					else
					{
						GameMenu gameMenu = currentMenuContext.GameMenu;
						text = ((gameMenu != null) ? gameMenu.StringId : null);
					}
				}
				return text == TutorialHelper.ActiveVillageRaidGameMenuID;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00009D18 File Offset: 0x00007F18
		public static bool TownMenuIsOpen
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				if (currentSettlement != null && currentSettlement.IsTown)
				{
					MenuContext currentMenuContext = Campaign.Current.CurrentMenuContext;
					string text;
					if (currentMenuContext == null)
					{
						text = null;
					}
					else
					{
						GameMenu gameMenu = currentMenuContext.GameMenu;
						text = ((gameMenu != null) ? gameMenu.StringId : null);
					}
					return text == "town";
				}
				return false;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00009D66 File Offset: 0x00007F66
		public static bool VillageMenuIsOpen
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				return currentSettlement != null && currentSettlement.IsVillage;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00009D78 File Offset: 0x00007F78
		public static bool BackStreetMenuIsOpen
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				if (currentSettlement != null && currentSettlement.IsTown && LocationComplex.Current != null)
				{
					Location locationWithId = LocationComplex.Current.GetLocationWithId("tavern");
					return TutorialHelper.GetMenuLocations.Contains(locationWithId);
				}
				return false;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00009DBC File Offset: 0x00007FBC
		public static bool IsPlayerInABattleMission
		{
			get
			{
				Mission mission = Mission.Current;
				return mission != null && mission.Mode == MissionMode.Battle;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00009DDD File Offset: 0x00007FDD
		public static bool IsOrderOfBattleOpenAndReady
		{
			get
			{
				Mission mission = Mission.Current;
				return mission != null && mission.Mode == MissionMode.Deployment && !LoadingWindow.IsLoadingWindowActive;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00009DFF File Offset: 0x00007FFF
		public static bool IsNavalMission
		{
			get
			{
				Mission mission = Mission.Current;
				return mission != null && mission.IsNavalBattle;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00009E14 File Offset: 0x00008014
		public static bool CanPlayerAssignHimselfToFormation
		{
			get
			{
				if (!TutorialHelper.IsOrderOfBattleOpenAndReady)
				{
					return false;
				}
				Mission mission = Mission.Current;
				if (mission == null)
				{
					return false;
				}
				return mission.PlayerTeam.FormationsIncludingEmpty.Any<Formation>((Formation x) => x.CountOfUnits > 0 && x.Captain == null);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00009E64 File Offset: 0x00008064
		public static bool IsPlayerInAFight
		{
			get
			{
				Mission mission = Mission.Current;
				MissionMode? missionMode = ((mission != null) ? new MissionMode?(mission.Mode) : null);
				if (missionMode != null)
				{
					MissionMode? missionMode2 = missionMode;
					MissionMode missionMode3 = MissionMode.Battle;
					if (!((missionMode2.GetValueOrDefault() == missionMode3) & (missionMode2 != null)))
					{
						missionMode2 = missionMode;
						missionMode3 = MissionMode.Duel;
						if (!((missionMode2.GetValueOrDefault() == missionMode3) & (missionMode2 != null)))
						{
							missionMode2 = missionMode;
							missionMode3 = MissionMode.Tournament;
							return (missionMode2.GetValueOrDefault() == missionMode3) & (missionMode2 != null);
						}
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00009EE4 File Offset: 0x000080E4
		public static bool IsPlayerEncounterLeader
		{
			get
			{
				Mission mission = Mission.Current;
				if (mission == null)
				{
					return false;
				}
				Team playerTeam = mission.PlayerTeam;
				bool? flag = ((playerTeam != null) ? new bool?(playerTeam.IsPlayerGeneral) : null);
				bool flag2 = true;
				return (flag.GetValueOrDefault() == flag2) & (flag != null);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00009F30 File Offset: 0x00008130
		public static bool IsPlayerInAHideoutBattleMission
		{
			get
			{
				Mission mission = Mission.Current;
				return mission != null && mission.HasMissionBehavior<HideoutMissionController>();
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00009F4E File Offset: 0x0000814E
		public static IList<Location> GetMenuLocations
		{
			get
			{
				return Campaign.Current.GameMenuManager.MenuLocations;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00009F5F File Offset: 0x0000815F
		public static bool PlayerIsSafeOnMap
		{
			get
			{
				return !TutorialHelper.IsActiveVillageRaidGameMenuOpen;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00009F6C File Offset: 0x0000816C
		public static bool IsCurrentTownHaveDoableCraftingOrder
		{
			get
			{
				ICraftingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
				CraftingCampaignBehavior.CraftingOrderSlots craftingOrderSlots;
				if (campaignBehavior == null)
				{
					craftingOrderSlots = null;
				}
				else
				{
					IReadOnlyDictionary<Town, CraftingCampaignBehavior.CraftingOrderSlots> craftingOrders = campaignBehavior.CraftingOrders;
					Settlement currentSettlement = Settlement.CurrentSettlement;
					craftingOrderSlots = craftingOrders[(currentSettlement != null) ? currentSettlement.Town : null];
				}
				CraftingCampaignBehavior.CraftingOrderSlots craftingOrderSlots2 = craftingOrderSlots;
				List<CraftingOrder> list;
				if (craftingOrderSlots2 == null)
				{
					list = null;
				}
				else
				{
					list = craftingOrderSlots2.Slots.Where<CraftingOrder>((CraftingOrder x) => x != null).ToList<CraftingOrder>();
				}
				List<CraftingOrder> list2 = list;
				PartyBase mainParty = PartyBase.MainParty;
				MBList<TroopRosterElement> mblist = ((mainParty != null) ? mainParty.MemberRoster.GetTroopRoster() : null);
				if (campaignBehavior == null || craftingOrderSlots2 == null || list2 == null || mblist == null)
				{
					return false;
				}
				for (int i = 0; i < mblist.Count; i++)
				{
					TroopRosterElement troopRosterElement = mblist[i];
					if (troopRosterElement.Character.IsHero)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							if (list2[j].IsOrderAvailableForHero(troopRosterElement.Character.HeroObject))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000A05C File Offset: 0x0000825C
		public static bool CurrentInventoryScreenIncludesBannerItem
		{
			get
			{
				InventoryState activeInventoryState = InventoryScreenHelper.GetActiveInventoryState();
				if (activeInventoryState != null)
				{
					InventoryLogic inventoryLogic = activeInventoryState.InventoryLogic;
					IReadOnlyList<ItemRosterElement> readOnlyList = ((inventoryLogic != null) ? inventoryLogic.GetElementsInRoster(InventoryLogic.InventorySide.OtherInventory) : null);
					if (readOnlyList != null)
					{
						foreach (ItemRosterElement itemRosterElement in readOnlyList)
						{
							if (itemRosterElement.EquipmentElement.Item.IsBannerItem)
							{
								return true;
							}
						}
						return false;
					}
				}
				return false;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000A0E0 File Offset: 0x000082E0
		public static bool PlayerHasUnassignedRolesAndMember
		{
			get
			{
				bool flag = false;
				PartyBase mainParty = PartyBase.MainParty;
				MBList<TroopRosterElement> mblist = ((mainParty != null) ? mainParty.MemberRoster.GetTroopRoster() : null);
				for (int i = 0; i < mblist.Count; i++)
				{
					TroopRosterElement troopRosterElement = mblist[i];
					if (troopRosterElement.Character.IsHero && !troopRosterElement.Character.IsPlayerCharacter && MobileParty.MainParty.GetHeroPartyRoles(troopRosterElement.Character.HeroObject).Count == 0)
					{
						flag = true;
						break;
					}
				}
				bool flag2 = MobileParty.MainParty.GetRoleHolder(PartyRole.Surgeon) == null || MobileParty.MainParty.GetRoleHolder(PartyRole.Engineer) == null || MobileParty.MainParty.GetRoleHolder(PartyRole.Quartermaster) == null || MobileParty.MainParty.GetRoleHolder(PartyRole.Scout) == null;
				return flag && flag2;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000A19C File Offset: 0x0000839C
		public static bool PlayerCanRecruit
		{
			get
			{
				if (TutorialHelper.PlayerIsInAnySettlement && (TutorialHelper.TownMenuIsOpen || TutorialHelper.VillageMenuIsOpen) && !Hero.MainHero.IsPrisoner && MobileParty.MainParty.MemberRoster.TotalManCount < PartyBase.MainParty.PartySizeLimit)
				{
					foreach (Hero hero in Settlement.CurrentSettlement.Notables)
					{
						int num = 0;
						foreach (CharacterObject characterObject in HeroHelper.GetVolunteerTroopsOfHeroForRecruitment(hero))
						{
							if (characterObject != null && HeroHelper.HeroCanRecruitFromHero(hero, Hero.MainHero, num))
							{
								int roundedResultNumber = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(characterObject, Hero.MainHero, false).RoundedResultNumber;
								return Hero.MainHero.Gold >= 5 * roundedResultNumber;
							}
							num++;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000A2D0 File Offset: 0x000084D0
		public static bool IsKingdomDecisionPanelActiveAndHasOptions
		{
			get
			{
				GauntletKingdomScreen gauntletKingdomScreen = ScreenManager.TopScreen as GauntletKingdomScreen;
				if (gauntletKingdomScreen != null)
				{
					KingdomManagementVM dataSource = gauntletKingdomScreen.DataSource;
					bool? flag;
					if (dataSource == null)
					{
						flag = null;
					}
					else
					{
						KingdomDecisionsVM decision = dataSource.Decision;
						flag = ((decision != null) ? new bool?(decision.IsCurrentDecisionActive) : null);
					}
					bool? flag2 = flag;
					bool flag3 = true;
					if ((flag2.GetValueOrDefault() == flag3) & (flag2 != null))
					{
						return gauntletKingdomScreen.DataSource.Decision.CurrentDecision.DecisionOptionsList.Count > 0;
					}
				}
				return false;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000A354 File Offset: 0x00008554
		public static Location CurrentMissionLocation
		{
			get
			{
				ICampaignMission campaignMission = CampaignMission.Current;
				if (campaignMission == null)
				{
					return null;
				}
				return campaignMission.Location;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000A368 File Offset: 0x00008568
		public static bool BuyingFoodBaseConditions
		{
			get
			{
				if ((TutorialHelper.TownMenuIsOpen || TutorialHelper.VillageMenuIsOpen || TutorialHelper.CurrentContext == TutorialContexts.InventoryScreen) && Settlement.CurrentSettlement != null)
				{
					ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("grain");
					if (@object != null)
					{
						ItemRoster itemRoster = Settlement.CurrentSettlement.ItemRoster;
						int num = itemRoster.FindIndexOfItem(@object);
						if (num >= 0)
						{
							int elementUnitCost = itemRoster.GetElementUnitCost(num);
							return Hero.MainHero.Gold >= 5 * elementUnitCost;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000A3E0 File Offset: 0x000085E0
		public static bool AreTroopUpgradesDisabled
		{
			get
			{
				GauntletPartyScreen gauntletPartyScreen;
				return (gauntletPartyScreen = ScreenManager.TopScreen as GauntletPartyScreen) != null && gauntletPartyScreen.IsTroopUpgradesDisabled;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000A404 File Offset: 0x00008604
		public static bool PlayerHasAnyUpgradeableTroop
		{
			get
			{
				foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
				{
					CharacterObject character = troopRosterElement.Character;
					if (!character.IsHero && troopRosterElement.Number > 0)
					{
						for (int i = 0; i < character.UpgradeTargets.Length; i++)
						{
							if (character.GetUpgradeXpCost(PartyBase.MainParty, i) <= troopRosterElement.Xp)
							{
								CharacterObject characterObject = character.UpgradeTargets[i];
								if (characterObject.UpgradeRequiresItemFromCategory == null)
								{
									return true;
								}
								foreach (ItemRosterElement itemRosterElement in MobileParty.MainParty.ItemRoster)
								{
									if (itemRosterElement.EquipmentElement.Item.ItemCategory == characterObject.UpgradeRequiresItemFromCategory && itemRosterElement.Amount > 0)
									{
										return true;
									}
								}
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000A538 File Offset: 0x00008738
		public static bool PlayerIsInAConversation
		{
			get
			{
				return !CharacterObject.ConversationCharacters.IsEmpty<CharacterObject>();
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000A548 File Offset: 0x00008748
		public static bool? IsThereAvailableCompanionInLocation(Location location)
		{
			if (location == null)
			{
				return null;
			}
			return new bool?(location.GetCharacterList().Any<LocationCharacter>((LocationCharacter x) => x.Character.IsHero && x.Character.HeroObject.IsWanderer && !x.Character.HeroObject.IsPlayerCompanion));
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000A591 File Offset: 0x00008791
		public static DateTime CurrentTime
		{
			get
			{
				return DateTime.Now;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000A598 File Offset: 0x00008798
		public static int MinimumGoldForCompanion
		{
			get
			{
				return 999;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000A59F File Offset: 0x0000879F
		public static float MaximumSpeedForPartyForSpeedTutorial
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000A5A6 File Offset: 0x000087A6
		public static float MaxCohesionForCohesionTutorial
		{
			get
			{
				return 30f;
			}
		}
	}
}
