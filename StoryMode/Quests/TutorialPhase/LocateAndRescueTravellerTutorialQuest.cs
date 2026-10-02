using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.TutorialPhase
{
	// Token: 0x0200001F RID: 31
	public class LocateAndRescueTravellerTutorialQuest : StoryModeQuestBase
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00007DD3 File Offset: 0x00005FD3
		private TextObject _startQuestLogText
		{
			get
			{
				return new TextObject("{=JJo0i8an}Look around the village to find the party that captured the traveller whom the headman told you about.", null);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00007DE0 File Offset: 0x00005FE0
		public override TextObject Title
		{
			get
			{
				return new TextObject("{=ACyYhA2s}Locate and Rescue Traveller", null);
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007DF0 File Offset: 0x00005FF0
		public LocateAndRescueTravellerTutorialQuest()
			: base("locate_and_rescue_traveler_tutorial_quest", null, CampaignTime.Never)
		{
			this._raiderParties = new List<MobileParty>();
			this._defeatedRaiderPartyCount = 0;
			this.SetDialogs();
			this.AddGameMenus();
			base.InitializeQuestOnCreation();
			this._raiderPartyCount = 0;
			this._startQuestLog = base.AddDiscreteLog(this._startQuestLogText, new TextObject("{=UkNUuyr1}Defeated Parties", null), this._defeatedRaiderPartyCount, 3, null, false);
			if (MobileParty.MainParty.MemberRoster.TotalManCount >= 4)
			{
				this.SpawnRaiderParties();
			}
			TutorialPhase.Instance.SetTutorialFocusSettlement(Settlement.Find("village_ES3_2"));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007E8C File Offset: 0x0000608C
		protected override void RegisterEvents()
		{
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007EF5 File Offset: 0x000060F5
		protected override void InitializeQuestOnGameLoad()
		{
			this.SetDialogs();
			this.AddGameMenus();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007F04 File Offset: 0x00006104
		private MobileParty CreateRaiderParty()
		{
			Settlement settlement = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement x) => x.IsActive).Settlement;
			Settlement @object = MBObjectManager.Instance.GetObject<Settlement>("village_ES3_2");
			CampaignVec2 campaignVec = NavigationHelper.FindReachablePointAroundPosition(@object.GatePosition, MobileParty.NavigationType.Default, MobileParty.MainParty.SeeingRange * 0.75f, 1f, false);
			MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("locate_and_rescue_traveller_quest_raider_party_" + this._raiderPartyCount, settlement.OwnerClan, settlement.Hideout, false, null, campaignVec);
			CharacterObject object2 = Campaign.Current.ObjectManager.GetObject<CharacterObject>("storymode_quest_raider");
			mobileParty.MemberRoster.AddToCounts(object2, 6, false, 0, 0, true, -1);
			CharacterObject object3 = MBObjectManager.Instance.GetObject<CharacterObject>("tutorial_placeholder_volunteer");
			mobileParty.PrisonRoster.AddToCounts(object3, (MBRandom.RandomFloat >= 0.5f) ? 1 : 2, false, 0, 0, true, -1);
			mobileParty.Party.SetCustomName(new TextObject("{=u1Pkt4HC}Raiders", null));
			mobileParty.InitializePartyTrade(QuestHelper.CalculateInitialGoldForBanditQuestParty(mobileParty));
			mobileParty.ActualClan = settlement.OwnerClan;
			SetPartyAiAction.GetActionForPatrollingAroundSettlement(mobileParty, @object, MobileParty.NavigationType.Default, false, false);
			mobileParty.Ai.SetDoNotMakeNewDecisions(true);
			mobileParty.IgnoreByOtherPartiesTill(CampaignTime.Never);
			mobileParty.Party.SetVisualAsDirty();
			base.AddTrackedObject(mobileParty);
			Campaign.Current.MapTrackerManager.AddMapTracker(mobileParty);
			mobileParty.IsActive = true;
			this._raiderPartyCount++;
			mobileParty.SetPartyUsedByQuest(true);
			return mobileParty;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000808C File Offset: 0x0000628C
		private void DespawnRaiderParties()
		{
			if (this._raiderParties.IsEmpty<MobileParty>())
			{
				return;
			}
			foreach (MobileParty mobileParty in this._raiderParties.ToList<MobileParty>())
			{
				base.RemoveTrackedObject(mobileParty);
				Campaign.Current.MapTrackerManager.RemoveMapTracker(mobileParty);
				DestroyPartyAction.Apply(null, mobileParty);
			}
			this._raiderParties.Clear();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00008114 File Offset: 0x00006314
		private void SpawnRaiderParties()
		{
			if (!this._raiderParties.IsEmpty<MobileParty>())
			{
				return;
			}
			for (int i = this._defeatedRaiderPartyCount; i < 3; i++)
			{
				this._raiderParties.Add(this.CreateRaiderParty());
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008154 File Offset: 0x00006354
		protected override void SetDialogs()
		{
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=BdYaRvhm}I don't know who you are, but I'm in your debt. These brigands would've marched us to our deaths.[ib:nervous2][if:convo_uncomfortable_voice]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.meeting_tacitus_on_condition))
				.NpcLine(new TextObject("{=9VxUSDQ7}My name's Tacteos. I'm a doctor by trade. I was on, well, a bit of a quest, but now I'm thinking I'm not really made for this kind of thing.[ib:nervous][if:convo_pondering]", null), null, null, null, null)
				.NpcLine(new TextObject("{=5LJTeOBT}I was with a caravan and they just came out of the brush. We were surrounded and outnumbered, so we gave up. I figured they'd keep us alive, if just for the ransom. But then they started flogging us along at top speed, without any water, and I was just about ready to drop.[ib:nervous2]", null), null, null, null, null)
				.NpcLine(new TextObject("{=XdDQdSsW}I could feel the signs of heat-stroke creeping up and I told them but they just flogged me more... If your group hadn't come along... Maybe I have a way to thank you properly.[ib:normal][if:convo_thinking]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=bkZFbCRx}We're looking for two children captured by the raiders. Can you tell us anything?", null), null, null, null)
				.NpcLine(new TextObject("{=ehnbi5yD}I am afraid I haven't seen any children. But after our caravan was attacked, the chief of the raiders, the one they call Radagos, took and rode off with our more valuable belongings, including a chest that I had.[ib:closed][if:convo_empathic_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=RF3NoR3d}He seemed to be controlling more than one band raiding around this area. If this lot has your kin, then I think he'd be the one to know.[if:convo_pondering]", null), null, null, null, null)
				.NpcLine(new TextObject("{=K75sH3vW}And since I have nothing of value left to repay your help, I'll tell you this. If you do catch up with and defeat that ruffian, you may be able to recover my chest. It contains a valuable ornament which I was told could be of great value, if you knew where to sell it.[if:convo_pondering]", null), null, null, null, null)
				.NpcLine(new TextObject("{=8GCW5IRO}I was trying to find out more about it, but, as I say, I've had all my urge for travelling flogged out of me. Right now I don't think I'd venture more than 20 paces from a well as long as I live.[ib:closed2][if:convo_shocked]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=Zyn5FrTR}We'll keep that in mind.", null), null, null, null)
				.NpcLine(new TextObject("{=vJyTsFdU}It doesn't look like much and I suspect this lot would give it away for a few coins, but I got it from a mercenary whom I treated once, and swore it was related to 'Neretzes's Folly'. I don't know what that means, except that Neretzes was, of course, the emperor who died in battle some years back. Maybe you can find out its true value.[if:convo_calm_friendly]", null), null, null, null, null)
				.NpcLine(new TextObject("{=tsjQtWsO}Thanks for saving me again. I hope our paths will cross again![ib:normal2][if:convo_calm_friendly]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.meeting_tacitus_on_consequence))
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=!}Start encounter.", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.meeting_with_raider_party_on_condition))
				.CloseDialog(), this);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000082DE File Offset: 0x000064DE
		private bool meeting_tacitus_on_condition()
		{
			return Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == StoryModeHeroes.Tacitus && !Hero.OneToOneConversationHero.HasMet;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008304 File Offset: 0x00006504
		private void meeting_tacitus_on_consequence()
		{
			foreach (MobileParty mobileParty in this._raiderParties)
			{
				if (mobileParty.IsActive)
				{
					DestroyPartyAction.Apply(null, mobileParty);
				}
			}
			DisableHeroAction.Apply(StoryModeHeroes.Tacitus);
			base.CompleteQuestWithSuccess();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00008370 File Offset: 0x00006570
		private bool meeting_with_raider_party_on_condition()
		{
			return this._raiderParties.Any<MobileParty>((MobileParty p) => ConversationHelper.GetConversationCharacterPartyLeader(p.Party) == CharacterObject.OneToOneConversationCharacter);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000839C File Offset: 0x0000659C
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement == null && PlayerEncounter.EncounteredMobileParty != null)
			{
				if (this._raiderParties.Any<MobileParty>((MobileParty p) => p == PlayerEncounter.EncounteredMobileParty) && args.MenuContext.GameMenu.StringId != "encounter_meeting" && args.MenuContext.GameMenu.StringId != "encounter" && args.MenuContext.GameMenu.StringId != "encounter_raiders_quest")
				{
					GameMenu.SwitchToMenu("encounter_raiders_quest");
				}
			}
			if (Hero.MainHero.HitPoints < 50 && MobileParty.MainParty.MapEvent == null)
			{
				Hero.MainHero.Heal(50 - Hero.MainHero.HitPoints, false);
			}
			Hero elderBrother = StoryModeHeroes.ElderBrother;
			if (elderBrother.HitPoints < 50)
			{
				elderBrother.Heal(50 - elderBrother.HitPoints, false);
			}
			if (Hero.MainHero.IsPrisoner)
			{
				EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
				if (elderBrother.IsPrisoner)
				{
					EndCaptivityAction.ApplyByPeace(elderBrother, null);
				}
				if (elderBrother.PartyBelongedTo != MobileParty.MainParty)
				{
					if (elderBrother.HeroState == Hero.CharacterStates.Fugitive || elderBrother.HeroState == Hero.CharacterStates.Released)
					{
						elderBrother.ChangeState(Hero.CharacterStates.Active);
					}
					AddHeroToPartyAction.Apply(elderBrother, MobileParty.MainParty, false);
				}
				DisableHeroAction.Apply(StoryModeHeroes.Tacitus);
				TextObject textObject = new TextObject("{=ORnjaMlM}You were defeated by the raiders, but your brother saved you. It doesn't look like they're going anywhere, though, so you should attack again once you're ready.{newline}You must have at least {NUMBER} members in your party. If you don't, go back to the village and recruit some more troops.", null);
				textObject.SetTextVariable("NUMBER", 4);
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=FPhWhjq7}Defeated", null).ToString(), textObject.ToString(), true, false, new TextObject("{=lmG7uRK2}Okay", null).ToString(), null, delegate
				{
					PartyBase mainParty = PartyBase.MainParty;
					if (mainParty != null && mainParty.MemberRoster.TotalManCount >= 4)
					{
						this.SpawnRaiderParties();
						return;
					}
					Campaign campaign = Campaign.Current;
					if (campaign == null)
					{
						return;
					}
					campaign.VisualTrackerManager.RegisterObject(MBObjectManager.Instance.GetObject<Settlement>("village_ES3_2"));
				}, null, "", 0f, null, null, null), false, false);
				this.DespawnRaiderParties();
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000856C File Offset: 0x0000676C
		private void AddGameMenus()
		{
			base.AddGameMenu("encounter_raiders_quest", new TextObject("{=mU1bC1mp}You encountered the raider party.", null), new OnInitDelegate(this.game_menu_encounter_on_init), GameMenu.MenuOverlayType.Encounter, GameMenu.MenuFlags.None);
			base.AddGameMenuOption("encounter_raiders_quest", "encounter_raiders_quest_attack", new TextObject("{=1r0tDsrR}Attack!", null), new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_attack_on_consequence), false, -1);
			base.AddGameMenuOption("encounter_raiders_quest", "encounter_raiders_quest_send_troops", new TextObject("{=z3VamNrX}Send in your troops.", null), new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_send_troops_on_condition), null, false, -1);
			base.AddGameMenuOption("encounter_raiders_quest", "encounter_raiders_quest_leave", new TextObject("{=2YYRyrOO}Leave...", null), new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_leave_on_consequence), true, -1);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008631 File Offset: 0x00006831
		private void game_menu_encounter_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Battle == null)
			{
				PlayerEncounter.StartBattle();
			}
			PlayerEncounter.Update();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008645 File Offset: 0x00006845
		private bool game_menu_encounter_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008650 File Offset: 0x00006850
		private void game_menu_encounter_leave_on_consequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterLeaveConsequence();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008658 File Offset: 0x00006858
		private bool game_menu_encounter_attack_on_condition(MenuCallbackArgs args)
		{
			if (PartyBase.MainParty.MemberRoster.TotalManCount < 4)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=DyE3luNM}You need to have at least {NUMBER} member in your party to deal with the raider party. Go back to village to recruit more troops.", null);
				args.Tooltip.SetTextVariable("NUMBER", 4);
			}
			return MenuHelper.EncounterAttackCondition(args);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000086A7 File Offset: 0x000068A7
		internal void game_menu_encounter_attack_on_consequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterAttackConsequence(args);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000086AF File Offset: 0x000068AF
		private bool game_menu_encounter_send_troops_on_condition(MenuCallbackArgs args)
		{
			args.IsEnabled = false;
			args.Tooltip = new TextObject("{=hnFkhPhp}This option is disabled during tutorial stage.", null);
			args.optionLeaveType = GameMenuOption.LeaveType.OrderTroopsToAttack;
			return true;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000086D2 File Offset: 0x000068D2
		[GameMenuInitializationHandler("encounter_raiders_quest")]
		private static void game_menu_encounter_on_init_background(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("encounter_looter");
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000086E4 File Offset: 0x000068E4
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (party == MobileParty.MainParty)
			{
				if (4 > MobileParty.MainParty.MemberRoster.TotalManCount)
				{
					this.DespawnRaiderParties();
					this.OpenRecruitMoreTroopsPopUp();
					return;
				}
				this.SpawnRaiderParties();
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00008714 File Offset: 0x00006914
		private void OpenRecruitMoreTroopsPopUp()
		{
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=y3fn2vWY}Recruit Troops", null).ToString(), new TextObject("{=taOCFKtZ}You need to recruit more troops to deal with the raider party. Go back to village to recruit more troops.", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00008770 File Offset: 0x00006970
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent)
			{
				if (mapEvent.PlayerSide == mapEvent.WinningSide)
				{
					using (List<MobileParty>.Enumerator enumerator = this._raiderParties.ToList<MobileParty>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MobileParty party = enumerator.Current;
							if (mapEvent.InvolvedParties.Any<PartyBase>((PartyBase p) => p == party.Party))
							{
								this._defeatedRaiderPartyCount++;
								this._startQuestLog.UpdateCurrentProgress(this._defeatedRaiderPartyCount);
								party.MemberRoster.Clear();
								if (this._raiderParties.Count > 1)
								{
									Campaign.Current.MapTrackerManager.RemoveMapTracker(party);
									this._raiderParties.Remove(party);
								}
							}
							if (party.MemberRoster.TotalManCount == 0 && this._raiderParties.Count > 1)
							{
								Campaign.Current.MapTrackerManager.RemoveMapTracker(party);
								this._raiderParties.Remove(party);
							}
						}
					}
					if (this._defeatedRaiderPartyCount >= 3)
					{
						MobileParty mobileParty = this._raiderParties[0];
						Hero tacitus = StoryModeHeroes.Tacitus;
						TakePrisonerAction.Apply(mobileParty.Party, tacitus);
						mobileParty.PrisonRoster.AddToCounts(Campaign.Current.ObjectManager.GetObject<CharacterObject>("villager_empire"), 2, false, 0, 0, true, -1);
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=EWD4Op6d}Notification", null).ToString(), new TextObject("{=OMrnTIe0}You rescue several prisoners that the raiders had been dragging along. They look parched and exhausted. You give them a bit of water and bread, and after a short while one staggers to his feet and comes over to you.", null).ToString(), true, false, new TextObject("{=lmG7uRK2}Okay", null).ToString(), null, delegate
						{
							CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, true, false, false, false, false), new ConversationCharacterData(StoryModeHeroes.Tacitus.CharacterObject, null, true, true, false, false, false, false));
						}, null, "", 0f, null, null, null), false, false);
					}
				}
				if (4 > MobileParty.MainParty.MemberRoster.TotalManCount)
				{
					this.DespawnRaiderParties();
					this.OpenRecruitMoreTroopsPopUp();
				}
			}
			if (Hero.MainHero.HitPoints < 50)
			{
				Hero.MainHero.Heal(50 - Hero.MainHero.HitPoints, false);
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000089B4 File Offset: 0x00006BB4
		protected override void HourlyTick()
		{
			if (4 > MobileParty.MainParty.MemberRoster.TotalManCount && MathF.Floor(Campaign.Current.Models.CampaignTimeModel.CampaignStartTime.ElapsedHoursUntilNow) % 12 == 0)
			{
				this.DespawnRaiderParties();
				this.OpenRecruitMoreTroopsPopUp();
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008A10 File Offset: 0x00006C10
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._raiderParties.Contains(mobileParty))
			{
				Campaign.Current.MapTrackerManager.RemoveMapTracker(mobileParty);
				this._raiderParties.Remove(mobileParty);
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00008A3D File Offset: 0x00006C3D
		protected override void OnCompleteWithSuccess()
		{
			TutorialPhase.Instance.RemoveTutorialFocusSettlement();
			TutorialPhase.Instance.RemoveTutorialFocusMobileParty();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008A53 File Offset: 0x00006C53
		internal static void AutoGeneratedStaticCollectObjectsLocateAndRescueTravellerTutorialQuest(object o, List<object> collectedObjects)
		{
			((LocateAndRescueTravellerTutorialQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008A61 File Offset: 0x00006C61
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._raiderParties);
			collectedObjects.Add(this._startQuestLog);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008A82 File Offset: 0x00006C82
		internal static object AutoGeneratedGetMemberValue_raiderPartyCount(object o)
		{
			return ((LocateAndRescueTravellerTutorialQuest)o)._raiderPartyCount;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008A94 File Offset: 0x00006C94
		internal static object AutoGeneratedGetMemberValue_raiderParties(object o)
		{
			return ((LocateAndRescueTravellerTutorialQuest)o)._raiderParties;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008AA1 File Offset: 0x00006CA1
		internal static object AutoGeneratedGetMemberValue_defeatedRaiderPartyCount(object o)
		{
			return ((LocateAndRescueTravellerTutorialQuest)o)._defeatedRaiderPartyCount;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008AB3 File Offset: 0x00006CB3
		internal static object AutoGeneratedGetMemberValue_startQuestLog(object o)
		{
			return ((LocateAndRescueTravellerTutorialQuest)o)._startQuestLog;
		}

		// Token: 0x04000090 RID: 144
		private const int MainPartyHealHitPointLimit = 50;

		// Token: 0x04000091 RID: 145
		private const int PlayerPartySizeMinLimitToSpawnRaiders = 4;

		// Token: 0x04000092 RID: 146
		private const int RaiderPartySize = 6;

		// Token: 0x04000093 RID: 147
		private const int RaiderPartyCount = 3;

		// Token: 0x04000094 RID: 148
		private const string RaiderPartyStringId = "locate_and_rescue_traveller_quest_raider_party_";

		// Token: 0x04000095 RID: 149
		[SaveableField(1)]
		private int _raiderPartyCount;

		// Token: 0x04000096 RID: 150
		[SaveableField(2)]
		private readonly List<MobileParty> _raiderParties;

		// Token: 0x04000097 RID: 151
		[SaveableField(3)]
		private int _defeatedRaiderPartyCount;

		// Token: 0x04000098 RID: 152
		[SaveableField(4)]
		private readonly JournalLog _startQuestLog;
	}
}
