using System;
using System.Linq;
using Helpers;
using StoryMode.Quests.FirstPhase;
using StoryMode.Quests.PlayerClanQuests;
using StoryMode.Quests.TutorialPhase;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x0200004E RID: 78
	public class FirstPhaseCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x0001B128 File Offset: 0x00019328
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.BeforeMissionOpenedEvent.AddNonSerializedListener(this, new Action(this.OnBeforeMissionOpened));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			StoryModeEvents.OnBannerPieceCollectedEvent.AddNonSerializedListener(this, new Action(this.OnBannerPieceCollected));
			StoryModeEvents.OnStoryModeTutorialEndedEvent.AddNonSerializedListener(this, new Action(this.OnStoryModeTutorialEnded));
			StoryModeEvents.OnMainStoryLineSideChosenEvent.AddNonSerializedListener(this, new Action<MainStoryLineSide>(this.OnMainStoryLineSideChosen));
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001B204 File Offset: 0x00019404
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Location>("_imperialMentorHouse", ref this._imperialMentorHouse);
			dataStore.SyncData<Location>("_antiImperialMentorHouse", ref this._antiImperialMentorHouse);
			dataStore.SyncData<bool>("_popUpShowed", ref this._popUpShowed);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001B23C File Offset: 0x0001943C
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.SpawnMentorsIfNeeded();
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001B244 File Offset: 0x00019444
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter campaignGameStarter)
		{
			Settlement settlement = SettlementHelper.FindRandomSettlement((Settlement s) => s.IsTown && !s.IsUnderSiege && s.Culture.StringId == "empire");
			this._imperialMentorHouse = this.ReserveHouseForMentor(StoryModeHeroes.ImperialMentor, settlement);
			Settlement settlement2 = SettlementHelper.FindRandomSettlement((Settlement s) => s.IsTown && !s.IsUnderSiege && s.Culture.StringId == "battania");
			this._antiImperialMentorHouse = this.ReserveHouseForMentor(StoryModeHeroes.AntiImperialMentor, settlement2);
			StoryModeManager.Current.MainStoryLine.SetMentorSettlements(settlement, settlement2);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001B2D0 File Offset: 0x000194D0
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (detail == QuestBase.QuestCompleteDetails.Success)
			{
				if (quest is BannerInvestigationQuest)
				{
					new MeetWithIstianaQuest(StoryModeManager.Current.MainStoryLine.ImperialMentorSettlement).StartQuest();
					new MeetWithArzagosQuest(StoryModeManager.Current.MainStoryLine.AntiImperialMentorSettlement).StartQuest();
					return;
				}
				if (quest is MeetWithIstianaQuest)
				{
					Hero imperialMentor = StoryModeHeroes.ImperialMentor;
					new IstianasBannerPieceQuest(imperialMentor, this.FindSuitableHideout(imperialMentor)).StartQuest();
					return;
				}
				if (quest is MeetWithArzagosQuest)
				{
					Hero antiImperialMentor = StoryModeHeroes.AntiImperialMentor;
					new ArzagosBannerPieceQuest(antiImperialMentor, this.FindSuitableHideout(antiImperialMentor)).StartQuest();
				}
			}
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001B35D File Offset: 0x0001955D
		private void OnGameMenuOpened(MenuCallbackArgs args)
		{
			this.SpawnMentorsIfNeeded();
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001B365 File Offset: 0x00019565
		private void OnBeforeMissionOpened()
		{
			this.SpawnMentorsIfNeeded();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001B370 File Offset: 0x00019570
		private void SpawnMentorsIfNeeded()
		{
			if (this._imperialMentorHouse != null && this._antiImperialMentorHouse != null && Settlement.CurrentSettlement != null && (StoryModeHeroes.ImperialMentor.CurrentSettlement == Settlement.CurrentSettlement || StoryModeHeroes.AntiImperialMentor.CurrentSettlement == Settlement.CurrentSettlement))
			{
				this.SpawnMentorInHouse(Settlement.CurrentSettlement);
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001B3C4 File Offset: 0x000195C4
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (settlement.StringId == "tutorial_training_field" && party == MobileParty.MainParty && TutorialPhase.Instance.TutorialQuestPhase == TutorialQuestPhase.Finalized && !this._popUpShowed && TutorialPhase.Instance.IsSkipped)
			{
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=EWD4Op6d}Notification", null).ToString(), GameTexts.FindText("main_storyline_skip_tutorial_notification_text", null).ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, delegate
				{
					this._popUpShowed = true;
					CampaignEventDispatcher.Instance.RemoveListeners(Campaign.Current.GetCampaignBehavior<TutorialPhaseCampaignBehavior>());
					MBInformationManager.ShowSceneNotification(new FindingFirstBannerPieceSceneNotificationItem(Hero.MainHero, new Action(this.OnPieceFoundAction)));
				}, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001B468 File Offset: 0x00019668
		private void ShowStealthTutorialInquiry()
		{
			object obj = new TextObject("{=DhMge68x}Stealth Tutorial", null);
			TextObject textObject = new TextObject("{=bU88a6lW}You and your brother part ways. As he rides over the crest of a hill, he lifts his arm in salute, then disappears from view. A few days ago you were a family of six. Now, you are alone, and you realize that despite your courage and determination you and your brother may never see each other again.{newline}However, you are not left long in your solitude. As you make the final preparations to set out, a young boy staggers into your camp. Once he regains his breath, he tells you that a small group of bandits raided his village and seized the headman as a hostage. The villagers saw you riding through the countryside, and thought you might be able to help them.", null);
			InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, false, GameTexts.FindText("str_continue", null).ToString(), string.Empty, new Action(this.StartStealthTutorial), null, "", 0f, null, null, null), true, false);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001B4D4 File Offset: 0x000196D4
		private void StartStealthTutorial()
		{
			new VillagersInNeed().StartQuest();
			StoryModeEvents.Instance.OnStealthTutorialActivated();
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001B4EA File Offset: 0x000196EA
		private void OnPieceFoundAction()
		{
			this.SelectClanName();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001B4F2 File Offset: 0x000196F2
		private void OnStoryModeTutorialEnded()
		{
			new RebuildPlayerClanQuest().StartQuest();
			new BannerInvestigationQuest().StartQuest();
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001B508 File Offset: 0x00019708
		private void OnBannerPieceCollected()
		{
			TextObject textObject = new TextObject("{=Pus87ZW2}You've found the {BANNER_PIECE_COUNT} banner piece!", null);
			if (FirstPhase.Instance == null || FirstPhase.Instance.CollectedBannerPieceCount == 1)
			{
				textObject.SetTextVariable("BANNER_PIECE_COUNT", new TextObject("{=oAoTaAWg}first", null));
			}
			else if (FirstPhase.Instance.CollectedBannerPieceCount == 2)
			{
				textObject.SetTextVariable("BANNER_PIECE_COUNT", new TextObject("{=9ZyXl25X}second", null));
			}
			else if (FirstPhase.Instance.CollectedBannerPieceCount == 3)
			{
				textObject.SetTextVariable("BANNER_PIECE_COUNT", new TextObject("{=4cw169Kb}third and the final", null));
			}
			MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001B5A6 File Offset: 0x000197A6
		private void OnMainStoryLineSideChosen(MainStoryLineSide side)
		{
			this._imperialMentorHouse.RemoveReservation();
			this._imperialMentorHouse = null;
			this._antiImperialMentorHouse.RemoveReservation();
			this._antiImperialMentorHouse = null;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001B5CC File Offset: 0x000197CC
		private void SelectClanName()
		{
			InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=RSn1j3tA}Choose your family name: ", null).ToString(), string.Empty, true, false, GameTexts.FindText("str_done", null).ToString(), null, new Action<string>(this.OnChangeClanNameDone), null, false, new Func<string, Tuple<bool, string>>(FactionHelper.IsClanNameApplicable), "", Clan.PlayerClan.Name.ToString()), false, false);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001B63C File Offset: 0x0001983C
		private void OnChangeClanNameDone(string newClanName)
		{
			TextObject textObject = GameTexts.FindText("str_generic_clan_name", null);
			textObject.SetTextVariable("CLAN_NAME", new TextObject(newClanName, null));
			Clan.PlayerClan.ChangeClanName(textObject, textObject);
			this.OpenBannerSelectionScreen(new Action(this.ShowStealthTutorialInquiry));
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001B686 File Offset: 0x00019886
		private void OpenBannerSelectionScreen(Action endAction)
		{
			Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<BannerEditorState>(new object[] { endAction }), 0);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001B6B4 File Offset: 0x000198B4
		private Settlement FindSuitableHideout(Hero questGiver)
		{
			Settlement settlement = null;
			float num = float.MaxValue;
			foreach (Hideout hideout in Hideout.All)
			{
				if (!hideout.Settlement.IsSettlementBusy(this))
				{
					float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(hideout.Settlement, questGiver.CurrentSettlement, false, false, MobileParty.NavigationType.Default);
					if (distance < num)
					{
						num = distance;
						settlement = hideout.Settlement;
					}
				}
			}
			return settlement;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001B74C File Offset: 0x0001994C
		private void SpawnMentorInHouse(Settlement settlement)
		{
			Hero hero = ((StoryModeHeroes.ImperialMentor.CurrentSettlement == settlement) ? StoryModeHeroes.ImperialMentor : StoryModeHeroes.AntiImperialMentor);
			Location location = ((StoryModeHeroes.ImperialMentor.CurrentSettlement == settlement) ? this._imperialMentorHouse : this._antiImperialMentorHouse);
			CharacterObject characterObject = hero.CharacterObject;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(characterObject.Race, "_settlement");
			LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new SimpleAgentOrigin(characterObject, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common", true, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true, null, false);
			location.AddCharacter(locationCharacter);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001B7F4 File Offset: 0x000199F4
		private Location ReserveHouseForMentor(Hero mentor, Settlement settlement)
		{
			if (settlement == null)
			{
				Debug.Print("There is null settlement in ReserveHouseForMentor", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			MBList<Location> mblist = new MBList<Location>();
			mblist.Add(settlement.LocationComplex.GetLocationWithId("house_1"));
			mblist.Add(settlement.LocationComplex.GetLocationWithId("house_2"));
			mblist.Add(settlement.LocationComplex.GetLocationWithId("house_3"));
			object obj = mblist.First<Location>((Location h) => !h.IsReserved) ?? mblist.GetRandomElement<Location>();
			TextObject textObject = new TextObject("{=EZ19JOGj}{MENTOR.NAME}'s House", null);
			StringHelpers.SetCharacterProperties("MENTOR", mentor.CharacterObject, textObject, false);
			object obj2 = obj;
			obj2.ReserveLocation(textObject, textObject);
			return obj2;
		}

		// Token: 0x040001D0 RID: 464
		private Location _imperialMentorHouse;

		// Token: 0x040001D1 RID: 465
		private Location _antiImperialMentorHouse;

		// Token: 0x040001D2 RID: 466
		private bool _popUpShowed;
	}
}
