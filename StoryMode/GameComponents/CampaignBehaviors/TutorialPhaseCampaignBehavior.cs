using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.Quests.TutorialPhase;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.ActivitySystem;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000057 RID: 87
	public class TutorialPhaseCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x0001EEE8 File Offset: 0x0001D0E8
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.TickEvent.AddNonSerializedListener(this, new Action<float>(this.Tick));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action<int>(this.OnCharacterCreationIsOver));
			CampaignEvents.CanHaveCampaignIssuesEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.CanHaveCampaignIssuesInfoIsRequested));
			CampaignEvents.CanHeroMarryEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.CanHeroMarry));
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001EFF4 File Offset: 0x0001D1F4
		private void AddDialogAndGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("storymode_conversation_blocker", "start", "close_window", "{=9XnFlRR0}Interaction with this person is disabled during tutorial stage.", new ConversationSentence.OnConditionDelegate(this.storymode_conversation_blocker_on_condition), null, 1000000, null);
			campaignGameStarter.AddGameMenu("storymode_game_menu_blocker", "{=pVKkclVk}Interactions are limited during tutorial phase. This interaction is disabled.", new OnInitDelegate(this.storymode_game_menu_blocker_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("storymode_game_menu_blocker", "game_menu_blocker_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("storymode_tutorial_village_game_menu", "{=7VFLb3Qj}You have arrived at the village.", new OnInitDelegate(this.storymode_tutorial_village_game_menu_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("storymode_tutorial_village_game_menu", "storymode_tutorial_village_hostile_action", "{=GM3tAYMr}Take a hostile action", new GameMenuOption.OnConditionDelegate(this.raid_village_menu_option_condition), null, false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("storymode_tutorial_village_game_menu", "storymode_tutorial_village_recruit", "{=E31IJyqs}Recruit troops", new GameMenuOption.OnConditionDelegate(this.recruit_troops_village_menu_option_condition), new GameMenuOption.OnConsequenceDelegate(this.storymode_recruit_volunteers_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("storymode_tutorial_village_game_menu", "storymode_tutorial_village_buy", "{=VN4ctHIU}Buy products", new GameMenuOption.OnConditionDelegate(this.buy_products_village_menu_option_condition), new GameMenuOption.OnConsequenceDelegate(this.storymode_ui_village_buy_good_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("storymode_tutorial_village_game_menu", "storymode_tutorial_village_enter", "{=Xrz05hYE}Take a walk around", new GameMenuOption.OnConditionDelegate(this.storymode_tutorial_village_enter_on_condition), new GameMenuOption.OnConsequenceDelegate(this.storymode_tutorial_village_enter_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("storymode_tutorial_village_game_menu", "storymode_tutorial_village_wait", "{=zEoHYEUS}Wait here for some time", new GameMenuOption.OnConditionDelegate(this.wait_village_menu_option_condition), null, false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("storymode_tutorial_village_game_menu", "storymode_tutorial_village_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_leave_on_consequence), true, -1, false, null);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001F1B0 File Offset: 0x0001D3B0
		private void InitializeTutorial()
		{
			Hero elderBrother = StoryModeHeroes.ElderBrother;
			elderBrother.ChangeState(Hero.CharacterStates.Active);
			AddHeroToPartyAction.Apply(elderBrother, MobileParty.MainParty, false);
			elderBrother.SetHasMet();
			DisableHeroAction.Apply(StoryModeHeroes.Tacitus);
			DisableHeroAction.Apply(StoryModeHeroes.LittleBrother);
			DisableHeroAction.Apply(StoryModeHeroes.LittleSister);
			DisableHeroAction.Apply(StoryModeHeroes.Radagos);
			DisableHeroAction.Apply(StoryModeHeroes.ImperialMentor);
			DisableHeroAction.Apply(StoryModeHeroes.AntiImperialMentor);
			DisableHeroAction.Apply(StoryModeHeroes.RadagosHenchman);
			Settlement settlement = Settlement.Find("village_ES3_2");
			this.CreateHeadman(settlement);
			PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 1);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001F248 File Offset: 0x0001D448
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Equipment[]>("_mainHeroEquipmentBackup", ref this._mainHeroEquipmentBackup);
			dataStore.SyncData<Equipment[]>("_brotherEquipmentBackup", ref this._brotherEquipmentBackup);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001F270 File Offset: 0x0001D470
		private void Tick(float dt)
		{
			if (TutorialPhase.Instance.TutorialFocusSettlement == null && TutorialPhase.Instance.TutorialFocusMobileParty == null)
			{
				return;
			}
			float num = -1f;
			CampaignVec2 campaignVec = CampaignVec2.Invalid;
			if (TutorialPhase.Instance.TutorialFocusSettlement != null)
			{
				num = TutorialPhase.Instance.TutorialFocusSettlement.GatePosition.Distance(MobileParty.MainParty.Position);
				campaignVec = TutorialPhase.Instance.TutorialFocusSettlement.GatePosition;
			}
			else if (TutorialPhase.Instance.TutorialFocusMobileParty != null)
			{
				num = TutorialPhase.Instance.TutorialFocusMobileParty.Position.Distance(MobileParty.MainParty.Position);
				campaignVec = TutorialPhase.Instance.TutorialFocusMobileParty.Position;
			}
			if (num > this._distanceThresholdForQuestFocusTarget)
			{
				this._controlledByBrother = true;
				MobileParty.MainParty.SetMoveGoToPoint(campaignVec, MobileParty.NavigationType.Default);
			}
			if (this._controlledByBrother && !this._notifyPlayerAboutPosition)
			{
				this._notifyPlayerAboutPosition = true;
				MBInformationManager.AddQuickInformation(new TextObject("{=hadftxlO}We have strayed too far from our path. I'll take the lead for some time. You follow me.", null), 0, StoryModeHeroes.ElderBrother.CharacterObject, null, "");
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.StoppablePlay;
			}
			if (this._controlledByBrother && num < MobileParty.MainParty.SeeingRange)
			{
				this._controlledByBrother = false;
				this._notifyPlayerAboutPosition = false;
				MobileParty.MainParty.SetMoveModeHold();
				MobileParty.MainParty.SetMoveGoToPoint(MobileParty.MainParty.Position, MobileParty.NavigationType.Default);
				MBInformationManager.AddQuickInformation(new TextObject("{=4vsvniPd}I think we are on the right path now. You are the better rider so you should take the lead.", null), 0, StoryModeHeroes.ElderBrother.CharacterObject, null, "");
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001F3E2 File Offset: 0x0001D5E2
		private void OnGameLoadFinished()
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.StringId == "village_ES3_2" && !TutorialPhase.Instance.IsCompleted)
			{
				this.SpawnYourBrotherInLocation(StoryModeHeroes.ElderBrother, "village_center");
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001F41D File Offset: 0x0001D61D
		private void DailyTick()
		{
			Campaign.Current.IssueManager.ToggleAllIssueTracks(false);
			this.CheckIfMainPartyStarving();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001F438 File Offset: 0x0001D638
		private void CanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
		{
			Settlement settlement = Settlement.Find("village_ES3_2");
			if (!StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted && settlement.Notables.Contains(hero))
			{
				result = false;
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001F477 File Offset: 0x0001D677
		private void CanHeroMarry(Hero hero, ref bool result)
		{
			if (!TutorialPhase.Instance.IsCompleted && hero.Clan == Clan.PlayerClan)
			{
				result = false;
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001F498 File Offset: 0x0001D698
		private void OnCharacterCreationIsOver(int index)
		{
			if (index == 1)
			{
				ActivityManager.SetActivityAvailability("CompleteMainQuest", true);
				ActivityManager.StartActivity("CompleteMainQuest");
				this._mainHeroEquipmentBackup[0] = Hero.MainHero.BattleEquipment.Clone(false);
				this._mainHeroEquipmentBackup[1] = Hero.MainHero.CivilianEquipment.Clone(false);
				this._brotherEquipmentBackup[0] = StoryModeHeroes.ElderBrother.BattleEquipment.Clone(false);
				this._brotherEquipmentBackup[1] = StoryModeHeroes.ElderBrother.CivilianEquipment.Clone(false);
				Settlement settlement = Settlement.Find("village_ES3_2");
				StoryModeHeroes.LittleBrother.UpdateLastKnownClosestSettlement(settlement);
				StoryModeHeroes.LittleSister.UpdateLastKnownClosestSettlement(settlement);
				StoryModeHeroes.MainHeroMother.UpdateLastKnownClosestSettlement(settlement);
				StoryModeHeroes.MainHeroFather.UpdateLastKnownClosestSettlement(settlement);
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001F55A File Offset: 0x0001D75A
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter campaignGameStarter, int i)
		{
			if (i == 99)
			{
				PartyBase.MainParty.ItemRoster.Clear();
				this._distanceThresholdForQuestFocusTarget = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default) / 1.5f;
				this.AddDialogAndGameMenus(campaignGameStarter);
				this.InitializeTutorial();
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001F594 File Offset: 0x0001D794
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this._distanceThresholdForQuestFocusTarget = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default) / 1.5f;
			this.AddDialogAndGameMenus(campaignGameStarter);
			Settlement settlement = Settlement.Find("village_ES3_2");
			if (settlement.Notables.IsEmpty<Hero>())
			{
				this.CreateHeadman(settlement);
				return;
			}
			TutorialPhase.Instance.TutorialVillageHeadman = settlement.Notables[0];
			if (!TutorialPhase.Instance.TutorialVillageHeadman.FirstName.Equals(new TextObject("{=Sb46O8WO}Orthos", null)))
			{
				TextObject textObject = new TextObject("{=JWLBKIkR}Headman {HEADMAN.FIRSTNAME}", null);
				TextObject textObject2 = new TextObject("{=Sb46O8WO}Orthos", null);
				TutorialPhase.Instance.TutorialVillageHeadman.SetName(textObject, textObject2);
				StringHelpers.SetCharacterProperties("HEADMAN", TutorialPhase.Instance.TutorialVillageHeadman.CharacterObject, textObject, false);
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001F65C File Offset: 0x0001D85C
		public void FinalizeTutorialPhase()
		{
			Settlement settlement = Settlement.Find("village_ES3_2");
			if (settlement.Notables.Count > 1)
			{
				Debug.FailedAssert("There are more than one notable in tutorial phase, control it.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\GameComponents\\CampaignBehaviors\\TutorialPhaseCampaignBehavior.cs", "FinalizeTutorialPhase", 266);
				using (List<Hero>.Enumerator enumerator = settlement.Notables.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Hero hero = enumerator.Current;
						hero.SetPersonalRelation(Hero.MainHero, 0);
					}
					goto IL_008A;
				}
			}
			Hero hero2 = settlement.Notables[0];
			hero2.SetPersonalRelation(Hero.MainHero, 0);
			KillCharacterAction.ApplyByRemove(hero2, false, true);
			IL_008A:
			this.SpawnAllNotablesForVillage(settlement.Village);
			VolunteerModel volunteerModel = Campaign.Current.Models.VolunteerModel;
			foreach (Hero hero3 in settlement.Notables)
			{
				if (hero3.IsAlive && volunteerModel.CanHaveRecruits(hero3))
				{
					CharacterObject basicVolunteer = volunteerModel.GetBasicVolunteer(hero3);
					for (int i = 0; i < hero3.VolunteerTypes.Length; i++)
					{
						if (hero3.VolunteerTypes[i] == null && MBRandom.RandomFloat < 0.5f)
						{
							hero3.VolunteerTypes[i] = basicVolunteer;
						}
					}
				}
			}
			DisableHeroAction.Apply(StoryModeHeroes.ElderBrother);
			StoryModeHeroes.ElderBrother.Clan = null;
			foreach (TroopRosterElement troopRosterElement in PartyBase.MainParty.MemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsPlayerCharacter)
				{
					PartyBase.MainParty.MemberRoster.RemoveTroop(troopRosterElement.Character, PartyBase.MainParty.MemberRoster.GetTroopCount(troopRosterElement.Character), default(UniqueTroopDescriptor), 0);
				}
			}
			foreach (TroopRosterElement troopRosterElement2 in PartyBase.MainParty.PrisonRoster.GetTroopRoster())
			{
				if (troopRosterElement2.Character.IsHero)
				{
					DisableHeroAction.Apply(troopRosterElement2.Character.HeroObject);
				}
				else
				{
					PartyBase.MainParty.PrisonRoster.RemoveTroop(troopRosterElement2.Character, PartyBase.MainParty.PrisonRoster.GetTroopCount(troopRosterElement2.Character), default(UniqueTroopDescriptor), 0);
				}
			}
			TutorialPhase.Instance.RemoveTutorialFocusSettlement();
			PartyBase.MainParty.ItemRoster.Clear();
			Hero.MainHero.BattleEquipment.FillFrom(this._mainHeroEquipmentBackup[0], true);
			Hero.MainHero.CivilianEquipment.FillFrom(this._mainHeroEquipmentBackup[1], true);
			StoryModeHeroes.ElderBrother.BattleEquipment.FillFrom(this._brotherEquipmentBackup[0], true);
			StoryModeHeroes.ElderBrother.CivilianEquipment.FillFrom(this._brotherEquipmentBackup[1], true);
			PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 2);
			Hero.MainHero.Heal(Hero.MainHero.MaxHitPoints, false);
			Hero.MainHero.Gold = 1000;
			if (TutorialPhase.Instance.TutorialQuestPhase == TutorialQuestPhase.Finalized && !TutorialPhase.Instance.IsSkipped)
			{
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=EWD4Op6d}Notification", null).ToString(), new TextObject("{=GCbqpeDs}Tutorial is over. You are now free to explore Calradia.", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, delegate
				{
					MBInformationManager.ShowSceneNotification(new FindingFirstBannerPieceSceneNotificationItem(Hero.MainHero, new Action(this.ShowStealthTutorialInquiry)));
					CampaignEventDispatcher.Instance.RemoveListeners(this);
				}, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001FA10 File Offset: 0x0001DC10
		private void ShowStealthTutorialInquiry()
		{
			object obj = new TextObject("{=DhMge68x}Stealth Tutorial", null);
			TextObject textObject = new TextObject("{=lVfxJkYb}You and your brother part ways. As he rides over the crest of a hill, he lifts his arm in salute, then disappears from view. A few days ago you were a family of six. Now, you are alone, and you realize that despite your courage and determination you and your brother may never see each other again.{newline}However, you are not left long in your solitude. As you make the final preparations to set out, a young boy whom you recognize from Tevea staggers into your camp. Once he regains his breath, he explains: a small band of Radagos's men escaped the showdown at his hideout, returned to the village, and seized the headman as a hostage. He begs you to come back with him and rescue their elder.", null);
			InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, false, GameTexts.FindText("str_continue", null).ToString(), string.Empty, new Action(this.StartStealthTutorial), null, "", 0f, null, null, null), true, false);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001FA7C File Offset: 0x0001DC7C
		private void StartStealthTutorial()
		{
			new VillagersInNeed().StartQuest();
			StoryModeEvents.Instance.OnStealthTutorialActivated();
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001FA94 File Offset: 0x0001DC94
		private void CreateHeadman(Settlement settlement)
		{
			Hero hero = HeroCreator.CreateNotable(Occupation.Headman, settlement);
			TextObject textObject = new TextObject("{=JWLBKIkR}Headman {HEADMAN.FIRSTNAME}", null);
			TextObject textObject2 = new TextObject("{=Sb46O8WO}Orthos", null);
			hero.SetName(textObject, textObject2);
			StringHelpers.SetCharacterProperties("HEADMAN", hero.CharacterObject, textObject, false);
			hero.AddPower((float)(Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit * 2));
			TutorialPhase.Instance.TutorialVillageHeadman = hero;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001FB08 File Offset: 0x0001DD08
		private bool recruit_troops_village_menu_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Recruit;
			bool flag = TutorialPhase.Instance.TutorialQuestPhase >= TutorialQuestPhase.RecruitAndPurchaseStarted && (Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(RecruitTroopsTutorialQuest)) || !Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(PurchaseGrainTutorialQuest)));
			args.IsEnabled = flag;
			args.Tooltip = (flag ? null : new TextObject("{=TeMExjrH}This option is disabled during current active quest.", null));
			return true;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001FB88 File Offset: 0x0001DD88
		private bool buy_products_village_menu_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Trade;
			bool flag = TutorialPhase.Instance.TutorialQuestPhase >= TutorialQuestPhase.RecruitAndPurchaseStarted && (!Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(PurchaseGrainTutorialQuest)) || !Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(RecruitTroopsTutorialQuest)));
			args.IsEnabled = flag;
			args.Tooltip = (flag ? null : new TextObject("{=TeMExjrH}This option is disabled during current active quest.", null));
			return true;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001FC07 File Offset: 0x0001DE07
		private bool raid_village_menu_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return this.PlaceholderOptionsClickableCondition(args);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001FC18 File Offset: 0x0001DE18
		private bool wait_village_menu_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return this.PlaceholderOptionsClickableCondition(args);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001FC29 File Offset: 0x0001DE29
		private bool PlaceholderOptionsClickableCondition(MenuCallbackArgs args)
		{
			args.IsEnabled = false;
			args.Tooltip = new TextObject("{=F7VxtCSd}This option is disabled during tutorial phase.", null);
			return true;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001FC44 File Offset: 0x0001DE44
		private void storymode_recruit_volunteers_on_consequence(MenuCallbackArgs args)
		{
			TutorialPhase.Instance.PrepareRecruitOptionForTutorial();
			args.MenuContext.OpenRecruitVolunteers();
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001FC5B File Offset: 0x0001DE5B
		private void storymode_ui_village_buy_good_on_consequence(MenuCallbackArgs args)
		{
			InventoryScreenHelper.OpenScreenAsTrade(TutorialPhase.Instance.GetAndPrepareBuyProductsOptionForTutorial(Settlement.CurrentSettlement.Village), Settlement.CurrentSettlement.Village, InventoryScreenHelper.InventoryCategoryType.None, null);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001FC82 File Offset: 0x0001DE82
		[GameMenuInitializationHandler("storymode_tutorial_village_game_menu")]
		private static void storymode_tutorial_village_game_menu_on_init_background(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.Village.WaitMeshName);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001FC9E File Offset: 0x0001DE9E
		[GameMenuInitializationHandler("storymode_game_menu_blocker")]
		private static void storymode_tutorial_blocker_game_menu_on_init_background(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(SettlementHelper.FindNearestVillageToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, null).WaitMeshName);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001FCBC File Offset: 0x0001DEBC
		private void storymode_game_menu_blocker_on_init(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.StringId == "village_ES3_2")
			{
				GameMenu.SwitchToMenu("storymode_tutorial_village_game_menu");
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001FCE8 File Offset: 0x0001DEE8
		private void storymode_tutorial_village_game_menu_on_init(MenuCallbackArgs args)
		{
			if (!StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted)
			{
				GameMenu.SwitchToMenu("village_outside");
				return;
			}
			Settlement currentSettlement = Settlement.CurrentSettlement;
			Campaign.Current.GameMenuManager.MenuLocations.Clear();
			Campaign.Current.GameMenuManager.MenuLocations.AddRange(currentSettlement.LocationComplex.GetListOfLocations());
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001FD4A File Offset: 0x0001DF4A
		private bool storymode_conversation_blocker_on_condition()
		{
			return StoryModeManager.Current.MainStoryLine.IsPlayerInteractionRestricted;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001FD5C File Offset: 0x0001DF5C
		private bool storymode_tutorial_village_enter_on_condition(MenuCallbackArgs args)
		{
			List<Location> list = Settlement.CurrentSettlement.LocationComplex.GetListOfLocations().ToList<Location>();
			GameMenuOption.IssueQuestFlags issueQuestFlags = Campaign.Current.IssueManager.CheckIssueForMenuLocations(list, true);
			args.OptionQuestData |= issueQuestFlags;
			args.OptionQuestData |= Campaign.Current.QuestManager.CheckQuestForMenuLocations(list);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			args.IsEnabled = !TutorialPhase.Instance.LockTutorialVillageEnter;
			if (!args.IsEnabled)
			{
				args.Tooltip = new TextObject("{=tWwXEWh6}Use the portrait to talk and enter the mission.", null);
			}
			return true;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001FDF0 File Offset: 0x0001DFF0
		private void storymode_tutorial_village_enter_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
			VillageEncounter villageEncounter = PlayerEncounter.LocationEncounter as VillageEncounter;
			if (TutorialPhase.Instance.TutorialQuestPhase == TutorialQuestPhase.TravelToVillageStarted)
			{
				villageEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("village_center"), null, StoryModeHeroes.ElderBrother.CharacterObject, null);
				return;
			}
			villageEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("village_center"), null, null, null);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001FE61 File Offset: 0x0001E061
		private bool game_menu_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001FE6C File Offset: 0x0001E06C
		private bool game_menu_leave_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001FE77 File Offset: 0x0001E077
		private void game_menu_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001FE80 File Offset: 0x0001E080
		private void SpawnYourBrotherInLocation(Hero hero, string locationId)
		{
			if (LocationComplex.Current != null)
			{
				Location locationWithId = LocationComplex.Current.GetLocationWithId(locationId);
				Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(hero.CharacterObject.Race);
				AgentData agentData = new AgentData(new PartyAgentOrigin(PartyBase.MainParty, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false, false)).Monster(baseMonsterFromRace).NoHorses(true);
				locationWithId.AddCharacter(new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), null, true, LocationCharacter.CharacterRelations.Friendly, null, true, false, null, false, true, true, null, false));
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001FF0C File Offset: 0x0001E10C
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (quest is TravelToVillageTutorialQuest)
			{
				new TalkToTheHeadmanTutorialQuest(Settlement.CurrentSettlement.Notables.First<Hero>((Hero n) => n.IsHeadman)).StartQuest();
				TutorialPhase.Instance.SetTutorialQuestPhase(TutorialQuestPhase.TalkToTheHeadmanStarted);
				return;
			}
			if (quest is TalkToTheHeadmanTutorialQuest)
			{
				new LocateAndRescueTravellerTutorialQuest().StartQuest();
				TutorialPhase.Instance.SetTutorialQuestPhase(TutorialQuestPhase.LocateAndRescueTravellerStarted);
				return;
			}
			if (quest is LocateAndRescueTravellerTutorialQuest)
			{
				new FindHideoutTutorialQuest(SettlementHelper.FindNearestHideoutToSettlement(Settlement.Find("village_ES3_2"), MobileParty.NavigationType.Default, null).Settlement).StartQuest();
				TutorialPhase.Instance.SetTutorialQuestPhase(TutorialQuestPhase.FindHideoutStarted);
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001FFB8 File Offset: 0x0001E1B8
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (settlement.StringId == "village_ES3_2" && !TutorialPhase.Instance.IsCompleted)
			{
				if (party != null)
				{
					if (party.IsMainParty)
					{
						this.SpawnYourBrotherInLocation(StoryModeHeroes.ElderBrother, "village_center");
					}
					else if (!party.IsMilitia)
					{
						party.SetMoveGoToSettlement(SettlementHelper.FindNearestSettlementToMobileParty(party, party.NavigationCapability, (Settlement s) => s != settlement && (s.IsFortification || s.IsVillage) && settlement != s && settlement.MapFaction == s.MapFaction), MobileParty.NavigationType.Default, false);
					}
				}
				if (party == null && hero != null && !hero.IsNotable)
				{
					TeleportHeroAction.ApplyImmediateTeleportToSettlement(hero, SettlementHelper.FindNearestSettlementToSettlement(settlement, MobileParty.NavigationType.Default, (Settlement s) => s != settlement && (s.IsFortification || s.IsVillage) && settlement != s && settlement.MapFaction == s.MapFaction));
				}
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00020068 File Offset: 0x0001E268
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (settlement.StringId == "tutorial_training_field" && party == MobileParty.MainParty && TutorialPhase.Instance.TutorialQuestPhase == TutorialQuestPhase.None)
			{
				new TravelToVillageTutorialQuest().StartQuest();
				TutorialPhase.Instance.SetTutorialQuestPhase(TutorialQuestPhase.TravelToVillageStarted);
				Campaign.Current.IssueManager.ToggleAllIssueTracks(false);
			}
			if (party == MobileParty.MainParty)
			{
				this.CheckIfMainPartyStarving();
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000200CF File Offset: 0x0001E2CF
		private void CheckIfMainPartyStarving()
		{
			if (!TutorialPhase.Instance.IsCompleted && PartyBase.MainParty.IsStarving)
			{
				PartyBase.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 1);
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00020100 File Offset: 0x0001E300
		private void SpawnAllNotablesForVillage(Village village)
		{
			int targetNotableCountForSettlement = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(village.Settlement, Occupation.RuralNotable);
			for (int i = 0; i < targetNotableCountForSettlement; i++)
			{
				HeroCreator.CreateNotable(Occupation.RuralNotable, village.Settlement);
			}
		}

		// Token: 0x040001E4 RID: 484
		private bool _controlledByBrother;

		// Token: 0x040001E5 RID: 485
		private bool _notifyPlayerAboutPosition;

		// Token: 0x040001E6 RID: 486
		private Equipment[] _mainHeroEquipmentBackup = new Equipment[2];

		// Token: 0x040001E7 RID: 487
		private Equipment[] _brotherEquipmentBackup = new Equipment[2];

		// Token: 0x040001E8 RID: 488
		private float _distanceThresholdForQuestFocusTarget;
	}
}
