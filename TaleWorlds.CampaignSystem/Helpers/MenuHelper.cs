using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000004 RID: 4
	public static class MenuHelper
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		public static bool SetOptionProperties(MenuCallbackArgs args, bool canPlayerDo, bool shouldBeDisabled, TextObject disabledText)
		{
			if (canPlayerDo)
			{
				return true;
			}
			if (!shouldBeDisabled)
			{
				return false;
			}
			args.IsEnabled = false;
			args.Tooltip = disabledText;
			return true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002074 File Offset: 0x00000274
		public static void SetIssueAndQuestDataForHero(MenuCallbackArgs args, Hero hero)
		{
			if (hero.Issue != null && hero.Issue.IssueQuest == null)
			{
				args.OptionQuestData |= GameMenuOption.IssueQuestFlags.AvailableIssue;
			}
			List<QuestBase> list;
			Campaign.Current.QuestManager.TrackedObjects.TryGetValue(hero, out list);
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsTrackEnabled)
					{
						if (list[i].IsSpecialQuest)
						{
							if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.TrackedStoryQuest) == GameMenuOption.IssueQuestFlags.None && list[i].QuestGiver != hero)
							{
								args.OptionQuestData |= GameMenuOption.IssueQuestFlags.TrackedStoryQuest;
							}
							else if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.ActiveStoryQuest) == GameMenuOption.IssueQuestFlags.None && list[i].QuestGiver == hero)
							{
								args.OptionQuestData |= GameMenuOption.IssueQuestFlags.ActiveStoryQuest;
							}
						}
						else if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.TrackedIssue) == GameMenuOption.IssueQuestFlags.None && list[i].QuestGiver != hero)
						{
							args.OptionQuestData |= GameMenuOption.IssueQuestFlags.TrackedIssue;
						}
						else if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.ActiveIssue) == GameMenuOption.IssueQuestFlags.None && list[i].QuestGiver == hero)
						{
							args.OptionQuestData |= GameMenuOption.IssueQuestFlags.ActiveIssue;
						}
					}
				}
			}
			if (hero.PartyBelongedTo != null && ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.ActiveStoryQuest) == GameMenuOption.IssueQuestFlags.None || (args.OptionQuestData & GameMenuOption.IssueQuestFlags.ActiveIssue) == GameMenuOption.IssueQuestFlags.None || (args.OptionQuestData & GameMenuOption.IssueQuestFlags.TrackedIssue) == GameMenuOption.IssueQuestFlags.None || (args.OptionQuestData & GameMenuOption.IssueQuestFlags.TrackedStoryQuest) == GameMenuOption.IssueQuestFlags.None))
			{
				List<QuestBase> list2;
				Campaign.Current.QuestManager.TrackedObjects.TryGetValue(hero.PartyBelongedTo, out list2);
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						if (list2[j].IsTrackEnabled)
						{
							if (list2[j].IsSpecialQuest)
							{
								if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.TrackedStoryQuest) == GameMenuOption.IssueQuestFlags.None && list2[j].QuestGiver != hero)
								{
									args.OptionQuestData |= GameMenuOption.IssueQuestFlags.TrackedStoryQuest;
								}
								else if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.ActiveStoryQuest) == GameMenuOption.IssueQuestFlags.None && list2[j].QuestGiver == hero)
								{
									args.OptionQuestData |= GameMenuOption.IssueQuestFlags.ActiveStoryQuest;
								}
							}
							else if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.TrackedIssue) == GameMenuOption.IssueQuestFlags.None && list2[j].QuestGiver != hero)
							{
								args.OptionQuestData |= GameMenuOption.IssueQuestFlags.TrackedIssue;
							}
							else if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.ActiveIssue) == GameMenuOption.IssueQuestFlags.None && list2[j].QuestGiver == hero)
							{
								args.OptionQuestData |= GameMenuOption.IssueQuestFlags.ActiveIssue;
							}
						}
					}
				}
			}
			if ((args.OptionQuestData & GameMenuOption.IssueQuestFlags.ActiveIssue) == GameMenuOption.IssueQuestFlags.None)
			{
				IssueBase issue = hero.Issue;
				if (((issue != null) ? issue.IssueQuest : null) != null && hero.Issue.IssueQuest.IsTrackEnabled)
				{
					args.OptionQuestData |= GameMenuOption.IssueQuestFlags.ActiveIssue;
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002310 File Offset: 0x00000510
		public static void SetIssueAndQuestDataForLocations(MenuCallbackArgs args, List<Location> locations)
		{
			GameMenuOption.IssueQuestFlags issueQuestFlags = Campaign.Current.IssueManager.CheckIssueForMenuLocations(locations, true);
			args.OptionQuestData |= issueQuestFlags;
			args.OptionQuestData |= Campaign.Current.QuestManager.CheckQuestForMenuLocations(locations);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000235C File Offset: 0x0000055C
		public static bool CheckAndOpenNextLocation(MenuCallbackArgs args)
		{
			if (Campaign.Current.GameMenuManager.NextLocation != null && GameStateManager.Current.ActiveState is MapState)
			{
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, Campaign.Current.GameMenuManager.PreviousLocation, null, null);
				string stringId = Campaign.Current.GameMenuManager.NextLocation.StringId;
				if (!(stringId == "center"))
				{
					if (!(stringId == "tavern"))
					{
						if (!(stringId == "arena"))
						{
							if (!(stringId == "lordshall") && !(stringId == "prison"))
							{
								if (stringId == "port")
								{
									Campaign.Current.GameMenuManager.SetNextMenu("port_menu");
								}
							}
							else
							{
								Campaign.Current.GameMenuManager.SetNextMenu("town_keep");
							}
						}
						else
						{
							Campaign.Current.GameMenuManager.SetNextMenu("town_arena");
						}
					}
					else
					{
						Campaign.Current.GameMenuManager.SetNextMenu("town_backstreet");
					}
				}
				else if (Settlement.CurrentSettlement.IsCastle)
				{
					Campaign.Current.GameMenuManager.SetNextMenu("castle");
				}
				else if (Settlement.CurrentSettlement.IsTown)
				{
					Campaign.Current.GameMenuManager.SetNextMenu("town");
				}
				else if (Settlement.CurrentSettlement.IsVillage)
				{
					Campaign.Current.GameMenuManager.SetNextMenu("village");
				}
				else
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "CheckAndOpenNextLocation", 192);
				}
				Campaign.Current.GameMenuManager.NextLocation = null;
				Campaign.Current.GameMenuManager.PreviousLocation = null;
				return true;
			}
			return false;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002534 File Offset: 0x00000734
		public static void DecideMenuState()
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (!string.IsNullOrEmpty(genericStateMenu))
			{
				GameMenu.SwitchToMenu(genericStateMenu);
				return;
			}
			GameMenu.ExitToLast();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000256C File Offset: 0x0000076C
		public static bool EncounterAttackCondition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			if (MapEvent.PlayerMapEvent == null)
			{
				return false;
			}
			MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
			Settlement mapEventSettlement = playerMapEvent.MapEventSettlement;
			if (mapEventSettlement != null && mapEventSettlement.IsFortification && playerMapEvent.IsSiegeAssault && PlayerSiege.PlayerSiegeEvent != null && !PlayerSiege.PlayerSiegeEvent.BesiegerCamp.IsPreparationComplete)
			{
				return false;
			}
			bool flag = MapEvent.PlayerMapEvent.PartiesOnSide(PartyBase.MainParty.OpponentSide).Any<MapEventParty>((MapEventParty party) => party.Party.NumberOfHealthyMembers > 0);
			if (Hero.MainHero.IsWounded)
			{
				args.Tooltip = new TextObject("{=UL8za0AO}You are wounded.", null);
				args.IsEnabled = false;
			}
			bool flag2 = (playerMapEvent.HasTroopsOnBothSides() || playerMapEvent.IsSiegeAssault) && MapEvent.PlayerMapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide) != null;
			if (!MobileParty.MainParty.IsInRaftState)
			{
				MobileParty mobileParty = playerMapEvent.PartiesOnSide(PlayerEncounter.Current.OpponentSide)[0].Party.MobileParty;
				if (mobileParty == null || !mobileParty.IsInRaftState)
				{
					goto IL_0125;
				}
			}
			args.Tooltip = new TextObject("{=x9ePfpw5}You are on a raft, in desperate circumstances, and cannot fight", null);
			args.IsEnabled = false;
			IL_0125:
			if (flag && !flag2 && !Hero.MainHero.IsWounded)
			{
				Debug.FailedAssert("This encounter case should be investigated", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "EncounterAttackCondition", 275);
				return false;
			}
			if (flag && Game.Current.IsDevelopmentMode && (mapEventSettlement == null || playerMapEvent.IsBlockadeSallyOut || playerMapEvent.IsSallyOut || playerMapEvent.IsSiegeOutside || playerMapEvent.IsBlockade))
			{
				bool flag3 = PlayerEncounter.IsNavalEncounter();
				IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
				CampaignVec2 position = MobileParty.MainParty.Position;
				MapPatchData mapPatchAtPosition = mapSceneWrapper.GetMapPatchAtPosition(in position);
				string battleSceneForMapPatch = Campaign.Current.Models.SceneModel.GetBattleSceneForMapPatch(mapPatchAtPosition, flag3);
				args.Tooltip = new TextObject("{=!}[DEV] Scene: (" + battleSceneForMapPatch + ")", null);
			}
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
				bool flag4;
				if (encounteredBattle == null)
				{
					flag4 = false;
				}
				else
				{
					Settlement mapEventSettlement2 = encounteredBattle.MapEventSettlement;
					bool? flag5 = ((mapEventSettlement2 != null) ? new bool?(mapEventSettlement2.IsVillage) : null);
					bool flag6 = true;
					flag4 = (flag5.GetValueOrDefault() == flag6) & (flag5 != null);
				}
				if (flag4)
				{
					MapEvent encounteredBattle2 = PlayerEncounter.EncounteredBattle;
					if (encounteredBattle2 != null && encounteredBattle2.IsRaid)
					{
						int minimumNumberOfMenForAttackingVillageViaScene = Campaign.Current.Models.EncounterModel.MinimumNumberOfMenForAttackingVillageViaScene;
						if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumNumberOfMenForAttackingVillageViaScene)
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=7b4WZVyU}You should at least have {NUMBER} healthy men in your party to take a hostile action.", null);
							args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
						}
						else if (!ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().AnyQ<Ship>())
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=hd626n2z}You don't have any shallow draft ship.", null);
						}
						else if (Math.Min(MobileParty.MainParty.MemberRoster.TotalHealthyCount, ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().SumQ<Ship>((Ship x) => x.MainDeckCrewCapacity)) < minimumNumberOfMenForAttackingVillageViaScene)
						{
							args.IsEnabled = false;
							args.Tooltip = new TextObject("{=aeUaoEYs}Your shallow ship's crew capacity is too low for a hostile action. A minimum of {NUMBER} crew is required. Use a larger or additional vessel.", null);
							args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000028C0 File Offset: 0x00000AC0
		public static bool EncounterCaptureEnemyCondition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			MapEvent battle = PlayerEncounter.Battle;
			if (battle != null)
			{
				return battle.PartiesOnSide(battle.GetOtherSide(battle.PlayerSide)).All<MapEventParty>((MapEventParty party) => !party.Party.IsSettlement && (party.Party.NumberOfHealthyMembers == 0 || party.Party.MobileParty.IsInRaftState));
			}
			return false;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002918 File Offset: 0x00000B18
		public static void EncounterAttackConsequence(MenuCallbackArgs args)
		{
			MapEvent battle = PlayerEncounter.Battle;
			PartyBase leaderParty = battle.GetLeaderParty(PartyBase.MainParty.OpponentSide);
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, leaderParty);
			if (PlayerEncounter.Current != null)
			{
				Settlement mapEventSettlement = MobileParty.MainParty.MapEvent.MapEventSettlement;
				if (mapEventSettlement != null && !battle.IsBlockadeSallyOut && !battle.IsSallyOut && !battle.IsSiegeOutside && !battle.IsBlockade)
				{
					if (mapEventSettlement.IsFortification)
					{
						if (battle.IsSiegeAmbush)
						{
							PlayerEncounter.StartSiegeAmbushMission();
						}
						else if (battle.IsSiegeAssault)
						{
							if (PlayerSiege.PlayerSiegeEvent == null && PartyBase.MainParty.Side == BattleSideEnum.Attacker)
							{
								PlayerSiege.StartPlayerSiege(MobileParty.MainParty.Party.Side, false, mapEventSettlement);
							}
							else
							{
								if (PlayerSiege.PlayerSiegeEvent != null)
								{
									if (!PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(PlayerSiege.PlayerSide.GetOppositeSide()).GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).Any<PartyBase>((PartyBase party) => party.NumberOfHealthyMembers > 0))
									{
										PlayerEncounter.Update();
										return;
									}
								}
								if (PlayerSiege.BesiegedSettlement != null && PlayerSiege.BesiegedSettlement.CurrentSiegeState == Settlement.SiegeState.InTheLordsHall)
								{
									FlattenedTroopRoster priorityListForLordsHallFightMission = Campaign.Current.Models.SiegeLordsHallFightModel.GetPriorityListForLordsHallFightMission(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, Campaign.Current.Models.SiegeLordsHallFightModel.MaxDefenderSideTroopCount);
									int num = MathF.Max(1, MathF.Min(Campaign.Current.Models.SiegeLordsHallFightModel.MaxAttackerSideTroopCount, MathF.Round((float)priorityListForLordsHallFightMission.Troops.Count<CharacterObject>() * Campaign.Current.Models.SiegeLordsHallFightModel.AttackerDefenderTroopCountRatio)));
									TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
									MobileParty mobileParty = ((MobileParty.MainParty.Army != null) ? MobileParty.MainParty.Army.LeaderParty : MobileParty.MainParty);
									troopRoster.Add(mobileParty.MemberRoster);
									foreach (MobileParty mobileParty2 in mobileParty.AttachedParties)
									{
										troopRoster.Add(mobileParty2.MemberRoster);
									}
									TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
									FlattenedTroopRoster flattenedTroopRoster = troopRoster.ToFlattenedRoster();
									flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => x.IsWounded);
									troopRoster2.Add(MobilePartyHelper.GetStrongestAndPriorTroops(flattenedTroopRoster, num, true));
									int num2 = 1;
									args.MenuContext.OpenTroopSelection(troopRoster, troopRoster2, (CharacterObject character) => !character.IsPlayerCharacter, new Action<TroopRoster>(MenuHelper.LordsHallTroopRosterManageDone), num, num2);
								}
								else
								{
									PlayerSiege.StartSiegeMission(mapEventSettlement);
								}
							}
						}
					}
					else if (mapEventSettlement.IsVillage)
					{
						BattleSideEnum battleSideEnum;
						bool flag;
						bool flag2;
						bool flag3;
						bool flag4;
						bool flag5;
						MapEventHelper.GetRaidContext(battle, out battleSideEnum, out flag, out flag2, out flag3, out flag4, out flag5);
						BattleSideEnum otherSide = battle.GetOtherSide(battleSideEnum);
						if (!flag5)
						{
							if (flag)
							{
								MenuHelper.StartSeaRaidMission(battle, battleSideEnum, args);
							}
							else
							{
								PlayerEncounter.StartVillageBattleMission();
							}
						}
						else if (flag3)
						{
							if (flag && !flag2 && !flag4)
							{
								IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
								CampaignVec2 campaignVec = MobileParty.MainParty.Position;
								MapPatchData mapPatchAtPosition = mapSceneWrapper.GetMapPatchAtPosition(in campaignVec);
								string battleSceneForMapPatch = Campaign.Current.Models.SceneModel.GetBattleSceneForMapPatch(mapPatchAtPosition, true);
								MissionInitializerRecord missionInitializerRecord = new MissionInitializerRecord(battleSceneForMapPatch);
								TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.CurrentNavigationFace);
								missionInitializerRecord.TerrainType = (int)faceTerrainType;
								missionInitializerRecord.DamageToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier();
								missionInitializerRecord.DamageFromPlayerToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier();
								missionInitializerRecord.NeedsRandomTerrain = false;
								missionInitializerRecord.PlayingInCampaignMode = true;
								missionInitializerRecord.RandomTerrainSeed = MBRandom.RandomInt(10000);
								missionInitializerRecord.AtmosphereOnCampaign = Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(MobileParty.MainParty.Position);
								missionInitializerRecord.SceneHasMapPatch = true;
								missionInitializerRecord.DecalAtlasGroup = 2;
								missionInitializerRecord.PatchCoordinates = mapPatchAtPosition.normalizedCoordinates;
								campaignVec = battle.AttackerSide.LeaderParty.Position;
								Vec2 vec = campaignVec.ToVec2();
								campaignVec = battle.DefenderSide.LeaderParty.Position;
								missionInitializerRecord.PatchEncounterDir = (vec - campaignVec.ToVec2()).Normalized();
								CampaignMission.OpenNavalBattleMission(missionInitializerRecord);
							}
							else
							{
								MenuHelper.StartSeaRaidMission(battle, otherSide, args);
							}
						}
						else
						{
							PlayerEncounter.StartVillageBattleMission();
						}
					}
					else if (mapEventSettlement.IsHideout)
					{
						CampaignMission.OpenHideoutBattleMission("sea_bandit_a", null, false);
					}
				}
				else
				{
					bool flag6 = PlayerEncounter.IsNavalEncounter();
					IMapScene mapSceneWrapper2 = Campaign.Current.MapSceneWrapper;
					CampaignVec2 campaignVec = MobileParty.MainParty.Position;
					MapPatchData mapPatchAtPosition2 = mapSceneWrapper2.GetMapPatchAtPosition(in campaignVec);
					string battleSceneForMapPatch2 = Campaign.Current.Models.SceneModel.GetBattleSceneForMapPatch(mapPatchAtPosition2, flag6);
					MissionInitializerRecord missionInitializerRecord2 = new MissionInitializerRecord(battleSceneForMapPatch2);
					TerrainType faceTerrainType2 = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(MobileParty.MainParty.CurrentNavigationFace);
					missionInitializerRecord2.TerrainType = (int)faceTerrainType2;
					missionInitializerRecord2.DamageToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier();
					missionInitializerRecord2.DamageFromPlayerToFriendsMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerTroopsReceivedDamageMultiplier();
					missionInitializerRecord2.NeedsRandomTerrain = false;
					missionInitializerRecord2.PlayingInCampaignMode = true;
					missionInitializerRecord2.RandomTerrainSeed = MBRandom.RandomInt(10000);
					missionInitializerRecord2.AtmosphereOnCampaign = Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(MobileParty.MainParty.Position);
					missionInitializerRecord2.SceneHasMapPatch = true;
					missionInitializerRecord2.DecalAtlasGroup = 2;
					missionInitializerRecord2.PatchCoordinates = mapPatchAtPosition2.normalizedCoordinates;
					campaignVec = battle.AttackerSide.LeaderParty.Position;
					Vec2 vec2 = campaignVec.ToVec2();
					campaignVec = battle.DefenderSide.LeaderParty.Position;
					missionInitializerRecord2.PatchEncounterDir = (vec2 - campaignVec.ToVec2()).Normalized();
					bool flag7 = MapEvent.PlayerMapEvent.PartiesOnSide(BattleSideEnum.Defender).Any<MapEventParty>((MapEventParty involvedParty) => involvedParty.Party.IsMobile && (involvedParty.Party.MobileParty.IsCaravan || (involvedParty.Party.Owner != null && involvedParty.Party.Owner.IsMerchant)));
					bool flag8;
					if (MapEvent.PlayerMapEvent.MapEventSettlement == null)
					{
						flag8 = MapEvent.PlayerMapEvent.PartiesOnSide(BattleSideEnum.Defender).Any<MapEventParty>((MapEventParty involvedParty) => involvedParty.Party.IsMobile && involvedParty.Party.MobileParty.IsVillager);
					}
					else
					{
						flag8 = false;
					}
					bool flag9 = flag8;
					if (flag6)
					{
						CampaignMission.OpenNavalBattleMission(missionInitializerRecord2);
					}
					else if (flag7 || flag9)
					{
						CampaignMission.OpenCaravanBattleMission(missionInitializerRecord2, flag7);
					}
					else
					{
						CampaignMission.OpenBattleMission(missionInitializerRecord2);
					}
				}
				PlayerEncounter.StartAttackMission();
				MapEvent.PlayerMapEvent.BeginWait();
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002FD0 File Offset: 0x000011D0
		private static void StartSeaRaidMission(MapEvent mapEvent, BattleSideEnum navalSide, MenuCallbackArgs args)
		{
			bool flag = mapEvent.PlayerSide == navalSide;
			List<MapEventParty> navalParties = mapEvent.PartiesOnSide(navalSide).ToList<MapEventParty>();
			List<Ship> list = (from x in navalParties.SelectMany<MapEventParty, Ship>((MapEventParty x) => x.Ships)
				where x.ShipHull.CanNavigateShallowWater
				orderby x.ShipHull.MainDeckCrewCapacity descending
				select x).ToList<Ship>();
			List<Ship> list2 = list.Take<Ship>(3).ToList<Ship>();
			int num = list2.Sum<Ship>((Ship x) => x.ShipHull.MainDeckCrewCapacity);
			if (flag)
			{
				TroopRoster strongestAndPriorTroops = MobilePartyHelper.GetStrongestAndPriorTroops(MobileParty.MainParty, Math.Min(num, MobileParty.MainParty.MemberRoster.TotalHealthyCount), true);
				MenuContext menuContext = args.MenuContext;
				TroopRoster memberRoster = MobileParty.MainParty.MemberRoster;
				TroopRoster troopRoster = strongestAndPriorTroops;
				List<Ship> list3 = list;
				List<Ship> list4 = list2;
				Func<CharacterObject, bool> func = (CharacterObject character) => !character.IsPlayerCharacter;
				Action<TroopRoster, List<Ship>> action = delegate(TroopRoster troops, List<Ship> selectedShips)
				{
					int num2 = selectedShips.Sum<Ship>((Ship x) => x.ShipHull.MainDeckCrewCapacity) - troops.TotalHealthyCount;
					foreach (FlattenedTroopRosterElement flattenedTroopRosterElement2 in (from x in navalParties.Where<MapEventParty>((MapEventParty x) => x.Party != PartyBase.MainParty).SelectMany<MapEventParty, FlattenedTroopRosterElement>((MapEventParty x) => x.Troops)
						orderby x.Troop.GetBattlePower() descending
						select x).Take<FlattenedTroopRosterElement>(num2))
					{
						troops.AddToCounts(flattenedTroopRosterElement2.Troop, 1, false, 0, 0, true, -1);
					}
					CampaignMission.OpenNavalRaidMission(troops, navalSide, selectedShips);
				};
				bool flag2 = navalParties.Any<MapEventParty>((MapEventParty x) => x.Party != PartyBase.MainParty);
				menuContext.OpenNavalTroopSelection(memberRoster, troopRoster, list3, list4, func, action, Campaign.Current.Models.EncounterModel.MinimumNumberOfMenForAttackingVillageViaScene, 1, 3, flag2);
				return;
			}
			TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
			foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in (from x in navalParties.SelectMany<MapEventParty, FlattenedTroopRosterElement>((MapEventParty x) => x.Troops)
				orderby x.Troop.GetBattlePower() descending
				select x).Take<FlattenedTroopRosterElement>(num))
			{
				troopRoster2.AddToCounts(flattenedTroopRosterElement.Troop, 1, false, 0, 0, true, -1);
			}
			CampaignMission.OpenNavalRaidMission(troopRoster2, navalSide, list2);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00003224 File Offset: 0x00001424
		private static void LordsHallTroopRosterManageDone(TroopRoster selectedTroops)
		{
			MapEvent.PlayerMapEvent.ResetBattleState();
			int wallLevel = PlayerSiege.BesiegedSettlement.Town.GetWallLevel();
			CampaignMission.OpenSiegeLordsHallFightMission(PlayerSiege.BesiegedSettlement.LocationComplex.GetLocationWithId("lordshall").GetSceneName(wallLevel), selectedTroops.ToFlattenedRoster());
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00003274 File Offset: 0x00001474
		public static void CheckEnemyAttackableHonorably(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				return;
			}
			if (PlayerEncounter.PlayerIsDefender)
			{
				return;
			}
			IFaction mapFaction = PlayerEncounter.EncounteredParty.MapFaction;
			if (mapFaction != null && mapFaction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				args.IsEnabled = false;
				args.Tooltip = GameTexts.FindText("str_enemy_not_attackable_tooltip", null);
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000032E4 File Offset: 0x000014E4
		public static bool EncounterOrderAttackCondition(MenuCallbackArgs args)
		{
			MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
			if (playerMapEvent != null)
			{
				bool flag = playerMapEvent.IsNavalMapEvent || (MapEventHelper.IsNavalRaid(playerMapEvent) && playerMapEvent.PlayerSide == BattleSideEnum.Attacker);
				args.optionLeaveType = (flag ? GameMenuOption.LeaveType.OrderShipsToAttack : GameMenuOption.LeaveType.OrderTroopsToAttack);
				MobileParty mobileParty = MapEvent.PlayerMapEvent.PartiesOnSide(PlayerEncounter.Current.OpponentSide)[0].Party.MobileParty;
				if (mobileParty != null && mobileParty.IsInRaftState)
				{
					return false;
				}
				MenuHelper.CheckEnemyAttackableHonorably(args);
				int num = 0;
				foreach (MapEventParty mapEventParty in MobileParty.MainParty.MapEventSide.Parties)
				{
					if (!mapEventParty.Party.IsMobile || !mapEventParty.Party.MobileParty.IsInRaftState)
					{
						num += mapEventParty.Party.MemberRoster.Sum(delegate(TroopRosterElement x)
						{
							if (!x.Character.IsHero)
							{
								return x.Number - x.WoundedNumber;
							}
							if (x.Character == CharacterObject.PlayerCharacter)
							{
								return 0;
							}
							if (!x.Character.HeroObject.IsWounded)
							{
								return 1;
							}
							return 0;
						});
					}
				}
				if (playerMapEvent.HasTroopsOnBothSides() && playerMapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide) != null && num > 0)
				{
					int num2 = 0;
					if (!MobileParty.MainParty.IsInRaftState)
					{
						num2 = MobileParty.MainParty.MemberRoster.Sum(delegate(TroopRosterElement x)
						{
							if (!x.Character.IsHero)
							{
								return x.Number - x.WoundedNumber;
							}
							if (x.Character == CharacterObject.PlayerCharacter)
							{
								return 0;
							}
							if (!x.Character.HeroObject.IsWounded)
							{
								return 1;
							}
							return 0;
						});
					}
					if (num2 > 0)
					{
						if (flag)
						{
							MBTextManager.SetTextVariable("SEND_TROOPS_TEXT", "{=NFnS5YqQ}Send ships.", false);
						}
						else
						{
							MBTextManager.SetTextVariable("SEND_TROOPS_TEXT", "{=QfMeoKOm}Send troops.", false);
						}
					}
					else
					{
						MBTextManager.SetTextVariable("SEND_TROOPS_TEXT", "{=jo3UHKMD}Leave it to the others.", false);
					}
					if (playerMapEvent.IsInvulnerable)
					{
						playerMapEvent.IsInvulnerable = false;
					}
					if (!MobilePartyHelper.CanPartyAttackWithCurrentMorale(MobileParty.MainParty))
					{
						args.Tooltip = new TextObject("{=xnRtINwH}Your men lack the courage to continue the battle without you. (Low Morale)", null);
						args.IsEnabled = false;
					}
					else
					{
						IFaction mapFaction = PlayerEncounter.EncounteredParty.MapFaction;
						if (mapFaction == null || mapFaction.NotAttackableByPlayerUntilTime.IsPast)
						{
							args.Tooltip = TooltipHelper.GetSendTroopsPowerContextTooltipForMapEvent();
						}
					}
					if (MobileParty.MainParty.IsCurrentlyAtSea)
					{
						MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
						bool flag2;
						if (encounteredBattle == null)
						{
							flag2 = false;
						}
						else
						{
							Settlement mapEventSettlement = encounteredBattle.MapEventSettlement;
							bool? flag3 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
							bool flag4 = true;
							flag2 = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
						}
						if (flag2)
						{
							MapEvent encounteredBattle2 = PlayerEncounter.EncounteredBattle;
							if (encounteredBattle2 != null && encounteredBattle2.IsRaid)
							{
								int minimumNumberOfMenForAttackingVillageViaScene = Campaign.Current.Models.EncounterModel.MinimumNumberOfMenForAttackingVillageViaScene;
								if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumNumberOfMenForAttackingVillageViaScene)
								{
									args.IsEnabled = false;
									args.Tooltip = new TextObject("{=7b4WZVyU}You should at least have {NUMBER} healthy men in your party to take a hostile action.", null);
									args.Tooltip.SetTextVariable("NUMBER", minimumNumberOfMenForAttackingVillageViaScene);
								}
								else if (!ShipHelper.GetOrderedNavalRaidShipsOfPlayerParty().AnyQ<Ship>())
								{
									args.IsEnabled = false;
									args.Tooltip = new TextObject("{=hd626n2z}You don't have any shallow draft ship.", null);
								}
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000035F0 File Offset: 0x000017F0
		private static void EncounterOrderAttack(TroopRoster selectedTroopsForPlayerSide)
		{
			MapEvent battle = PlayerEncounter.Battle;
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				ISiegeEventSide siegeEventSide = PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(PlayerSiege.PlayerSide.GetOppositeSide());
				if (siegeEventSide != null)
				{
					if (!siegeEventSide.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).Any<PartyBase>((PartyBase party) => party.NumberOfHealthyMembers > 0))
					{
						bool flag;
						if (battle != null)
						{
							flag = !battle.GetMapEventSide(battle.GetOtherSide(battle.PlayerSide)).Parties.Any<MapEventParty>((MapEventParty party) => party.Party.NumberOfHealthyMembers > 0);
						}
						else
						{
							flag = true;
						}
						if (flag)
						{
							PlayerEncounter.Update();
							return;
						}
					}
				}
			}
			PartyBase leaderParty = battle.GetLeaderParty(PartyBase.MainParty.OpponentSide);
			MobileParty mobileParty = MobileParty.MainParty.AttachedTo ?? MobileParty.MainParty;
			SiegeEvent siegeEvent = leaderParty.SiegeEvent;
			if (((siegeEvent != null) ? siegeEvent.BesiegerCamp : null) != null && !leaderParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(leaderParty, MapEvent.BattleTypes.Siege) && mobileParty.BesiegerCamp == null)
			{
				mobileParty.BesiegerCamp = leaderParty.SiegeEvent.BesiegerCamp;
			}
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, leaderParty);
			if (PlayerEncounter.Current != null)
			{
				GameMenu.ExitToLast();
				PlayerEncounter.InitSimulation(null, null);
				if (PlayerEncounter.Current != null && PlayerEncounter.Current.BattleSimulation != null)
				{
					((MapState)Game.Current.GameStateManager.ActiveState).StartBattleSimulation();
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003751 File Offset: 0x00001951
		public static void EncounterOrderAttackConsequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterOrderAttack(null);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00003759 File Offset: 0x00001959
		public static void EncounterCaptureTheEnemyOnConsequence(MenuCallbackArgs args)
		{
			MapEvent.PlayerMapEvent.SetOverrideWinner(MapEvent.PlayerMapEvent.PlayerSide);
			PlayerEncounter.Update();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003774 File Offset: 0x00001974
		public static void EncounterLeaveConsequence()
		{
			Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
			MapEvent mapEvent = ((PlayerEncounter.Battle != null) ? PlayerEncounter.Battle : PlayerEncounter.EncounteredBattle);
			int numberOfInvolvedMen = mapEvent.GetNumberOfInvolvedMen(PartyBase.MainParty.Side);
			PlayerEncounter.Finish(MobileParty.MainParty.CurrentSettlement != null && !MobileParty.MainParty.CurrentSettlement.IsFortification);
			if (MobileParty.MainParty.BesiegerCamp != null)
			{
				MobileParty.MainParty.BesiegerCamp = null;
			}
			if (mapEvent != null && !mapEvent.IsFinalized && !mapEvent.IsRaid && numberOfInvolvedMen == PartyBase.MainParty.NumberOfHealthyMembers)
			{
				MapEvent mapEvent2 = mapEvent;
				PlayerEncounter playerEncounter = PlayerEncounter.Current;
				FlattenedTroopRoster[] array;
				if (playerEncounter == null)
				{
					array = null;
				}
				else
				{
					BattleSimulation battleSimulation = playerEncounter.BattleSimulation;
					array = ((battleSimulation != null) ? battleSimulation.SelectedTroops : null);
				}
				mapEvent2.SimulateBattleSetup(array);
				mapEvent.SimulateBattleRound((PartyBase.MainParty.Side == BattleSideEnum.Attacker) ? 1 : 0, (PartyBase.MainParty.Side == BattleSideEnum.Attacker) ? 0 : 1);
			}
			if (currentSettlement != null)
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, currentSettlement);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003868 File Offset: 0x00001A68
		public static string GetEncounterCultureBackgroundMesh(CultureObject encounterCulture)
		{
			if (string.IsNullOrEmpty((encounterCulture != null) ? encounterCulture.EncounterBackgroundMesh : null))
			{
				Debug.FailedAssert("Background mesh is invalid for current encounter", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetEncounterCultureBackgroundMesh", 827);
				return string.Empty;
			}
			string text = encounterCulture.EncounterBackgroundMesh;
			MapEvent mapEvent = PlayerEncounter.Battle ?? PlayerEncounter.EncounteredBattle;
			if (mapEvent != null && mapEvent.IsNavalMapEvent)
			{
				text += "_naval";
			}
			return text;
		}
	}
}
