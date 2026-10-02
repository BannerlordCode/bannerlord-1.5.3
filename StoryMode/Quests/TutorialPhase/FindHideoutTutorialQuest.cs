using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Missions.MissionLogics.Hideout;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;

namespace StoryMode.Quests.TutorialPhase
{
	// Token: 0x0200001E RID: 30
	public class FindHideoutTutorialQuest : StoryModeQuestBase
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000645C File Offset: 0x0000465C
		private TextObject _startQuestLog
		{
			get
			{
				TextObject textObject = new TextObject("{=gSBGpUBm}Find {RADAGOS.LINK}' hideout.", null);
				StringHelpers.SetCharacterProperties("RADAGOS", StoryModeHeroes.Radagos.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00006490 File Offset: 0x00004690
		public override TextObject Title
		{
			get
			{
				TextObject textObject = new TextObject("{=NvkWtb8f}Find the Hideout of {RADAGOS.NAME}' Gang and Defeat Them", null);
				StringHelpers.SetCharacterProperties("RADAGOS", StoryModeHeroes.Radagos.CharacterObject, textObject, false);
				return textObject;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000064C4 File Offset: 0x000046C4
		public FindHideoutTutorialQuest(Settlement hideout)
			: base("find_hideout_tutorial_quest", null, CampaignTime.Never)
		{
			this._hideout = hideout;
			FindHideoutTutorialQuest._activeHideoutStringId = this._hideout.StringId;
			this._hideout.Party.SetCustomName(new TextObject("{=9xaEPyNV}{RADAGOS.NAME}' Hideout", null));
			StringHelpers.SetCharacterProperties("RADAGOS", StoryModeHeroes.Radagos.CharacterObject, this._hideout.Name, false);
			this._raiderParties = new List<MobileParty>();
			this._foughtWithRadagos = false;
			this._talkedWithRadagos = false;
			this._talkedWithBrother = false;
			this._hideoutBattleEndState = FindHideoutTutorialQuest.HideoutBattleEndState.None;
			this.InitializeHideout();
			base.AddTrackedObject(this._hideout);
			this.SetDialogs();
			this.AddGameMenus();
			base.InitializeQuestOnCreation();
			base.AddLog(this._startQuestLog, false);
			TutorialPhase.Instance.SetTutorialFocusSettlement(this._hideout);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000659E File Offset: 0x0000479E
		protected override void HourlyTick()
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000065A0 File Offset: 0x000047A0
		protected override void RegisterEvents()
		{
			CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006609 File Offset: 0x00004809
		public override void OnHeroCanDieInfoIsRequested(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			if (hero == StoryModeHeroes.Radagos)
			{
				result = false;
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006616 File Offset: 0x00004816
		public override void OnHeroCanBeSelectedInInventoryInfoIsRequested(Hero hero, ref bool result)
		{
			if (hero == StoryModeHeroes.Radagos)
			{
				result = false;
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006623 File Offset: 0x00004823
		public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
		{
			if (hero == StoryModeHeroes.Radagos)
			{
				result = false;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006630 File Offset: 0x00004830
		protected override void InitializeQuestOnGameLoad()
		{
			FindHideoutTutorialQuest._activeHideoutStringId = this._hideout.StringId;
			this._hideout.Party.SetCustomName(new TextObject("{=9xaEPyNV}{RADAGOS.NAME}' Hideout", null));
			StringHelpers.SetCharacterProperties("RADAGOS", StoryModeHeroes.Radagos.CharacterObject, this._hideout.Name, false);
			StoryModeHeroes.Radagos.CharacterObject.HiddenInEncyclopedia = false;
			StoryModeHeroes.Radagos.UpdateLastKnownClosestSettlement(this._hideout);
			this.SetDialogs();
			this.AddGameMenus();
			if (this._raiderParties.Count > 2)
			{
				for (int i = this._raiderParties.Count - 1; i >= 0; i--)
				{
					if (this._raiderParties[i].MapEvent == null && !this._raiderParties[i].IsActive)
					{
						this._raiderParties.Remove(this._raiderParties[i]);
					}
				}
				for (int j = this._raiderParties.Count - 1; j >= 0; j--)
				{
					if (!this._raiderParties[j].IsBanditBossParty && this._raiderParties[j].MapEvent == null)
					{
						if (!this._raiderParties[j].IsActive)
						{
							this._raiderParties.Remove(this._raiderParties[j]);
						}
						else
						{
							DestroyPartyAction.Apply(null, this._raiderParties[j]);
						}
					}
					if (this._raiderParties.Count <= 2)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000067A8 File Offset: 0x000049A8
		protected override void OnStartQuest()
		{
			MapState mapState;
			if ((mapState = GameStateManager.Current.ActiveState as MapState) != null)
			{
				mapState.Handler.StartCameraAnimation(this._hideout.GatePosition, 1f);
			}
			StoryModeHeroes.Radagos.CharacterObject.HiddenInEncyclopedia = false;
			StoryModeHeroes.Radagos.UpdateLastKnownClosestSettlement(this._hideout);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006804 File Offset: 0x00004A04
		private void InitializeHideout()
		{
			this._hideout.IsVisible = true;
			if (!this._hideout.Hideout.IsInfested)
			{
				int num = 0;
				while (num < 2 && !this._hideout.Hideout.IsInfested)
				{
					this._raiderParties.Add(this.CreateRaiderParty(this._raiderParties.Count + 1, false));
					num++;
				}
			}
			if (!this._hideout.Parties.Any<MobileParty>((MobileParty p) => p.IsBanditBossParty))
			{
				this._raiderParties.Add(this.CreateRaiderParty(this._raiderParties.Count + 1, true));
			}
			foreach (MobileParty mobileParty in this._hideout.Parties)
			{
				if (mobileParty.IsBanditBossParty)
				{
					int totalRegulars = mobileParty.MemberRoster.TotalRegulars;
					mobileParty.MemberRoster.Clear();
					if (StoryModeHeroes.Radagos.HeroState != Hero.CharacterStates.Active)
					{
						StoryModeHeroes.Radagos.ChangeState(Hero.CharacterStates.Active);
					}
					mobileParty.MemberRoster.AddToCounts(StoryModeHeroes.Radagos.CharacterObject, 1, false, 0, 0, true, -1);
					CharacterObject @object = Campaign.Current.ObjectManager.GetObject<CharacterObject>("storymode_quest_raider");
					mobileParty.MemberRoster.AddToCounts(@object, totalRegulars, false, 0, 0, true, -1);
					StoryModeHeroes.Radagos.Heal(100, false);
					break;
				}
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006998 File Offset: 0x00004B98
		private MobileParty CreateRaiderParty(int number, bool isBanditBossParty)
		{
			MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("radagos_raider_party_" + number, this._hideout.OwnerClan, this._hideout.Hideout, isBanditBossParty, null, this._hideout.GatePosition);
			CharacterObject @object = Campaign.Current.ObjectManager.GetObject<CharacterObject>("storymode_quest_raider");
			mobileParty.MemberRoster.AddToCounts(@object, 4, false, 0, 0, true, -1);
			mobileParty.Party.SetCustomName(new TextObject("{=u1Pkt4HC}Raiders", null));
			mobileParty.ActualClan = this._hideout.OwnerClan;
			mobileParty.Position = this._hideout.Position;
			mobileParty.Party.SetCustomOwner(StoryModeHeroes.Radagos);
			mobileParty.Party.SetVisualAsDirty();
			EnterSettlementAction.ApplyForParty(mobileParty, this._hideout);
			mobileParty.InitializePartyTrade(QuestHelper.CalculateInitialGoldForBanditQuestParty(mobileParty));
			mobileParty.SetMoveGoToSettlement(this._hideout, MobileParty.NavigationType.Default, false);
			EnterSettlementAction.ApplyForParty(mobileParty, this._hideout);
			mobileParty.SetPartyUsedByQuest(true);
			return mobileParty;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006A94 File Offset: 0x00004C94
		protected override void SetDialogs()
		{
			StringHelpers.SetCharacterProperties("TACTEOS", StoryModeHeroes.Tacitus.CharacterObject, null, false);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000015).NpcLine(new TextObject("{=R3CnF55p}So... Who's this that comes through my place of business, killing my employees?[if:convo_confused_voice][ib:warrior2]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.bandit_hideout_boss_fight_start_on_condition))
				.PlayerLine(new TextObject("{=itRoeaJf}We heard you took our little brother and sister. Where are they?", null), null, null, null)
				.NpcLine(GameTexts.FindText("find_hideout_quest_radagos_conversation_line_1", null), null, null, null, null)
				.NpcLine(new TextObject("{=wWLnZ6G4}Since your hunt for your kin is fruitless, how about you clear off and save your own lives? Either that or I force you to lick up all the blood you've spilled here with your tongues. Or... You and I could settle this, one on one.[if:convo_angry_voice]", null), null, null, null, null)
				.BeginPlayerOptions(null, false)
				.PlayerOption(new TextObject("{=ImLQNYWC}Very well - I'll duel you.", null), null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.bandit_hideout_start_duel_fight_on_consequence))
				.CloseDialog()
				.PlayerOption(new TextObject("{=MMv3hsmI}I don't duel slavers. Men, attack!", null), null, null, null)
				.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.bandit_hideout_continue_battle_on_clickable_condition))
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.bandit_hideout_continue_battle_on_consequence))
				.CloseDialog()
				.EndPlayerOptions()
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000010).NpcLine(new TextObject("{=ZhZ7MCeh}Well. I recognize defeat when I see it. If I'm going to be your captive, let me introduce myself. I'm Radagos.[ib:weary2][if:convo_uncomfortable_voice]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.radagos_meeting_conversation_condition))
				.NpcLine(new TextObject("{=w0CUaEU7}You haven't cut my throat yet, which was a wise move. I'm sure I can find a way to be worth more to you alive than dead.[if:convo_calm_friendly]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=vDRRsed8}You'd better help us get our brother and sister back, or you'll swing from a tree.", null), null, null, null)
				.NpcLine(GameTexts.FindText("find_hideout_quest_radagos_conversation_line_2", null), null, null, null, null)
				.NpcLine(GameTexts.FindText("find_hideout_quest_radagos_conversation_line_3", null), null, null, null, null)
				.NpcLine(new TextObject("{=FWSwngVX}Shall we get on the road? Remember - if I drop dead of exhaustion, or drown in some river, that's it for your little dears. I don't expect a cozy palanquin, now, but you'd best not make it too hard a trip for me.[if:convo_uncomfortable_voice]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.radagos_meeting_conversation_consequence))
				.CloseDialog(), this);
			Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000020).NpcLine(new TextObject("{=qp2zYfua}I was hoping to find more treasure here, but I think business wasn't going too well for {RADAGOS.NAME} and his gang.[ib:closed2][if:convo_pondering]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.brother_farewell_conversation_condition))
				.NpcLine(new TextObject("{=J4qetbZb}I found this strange looking metal piece though. It doesn't look too valuable, but it could be the artifact {TACTEOS.NAME} was talking about. Maybe we can sell it to one of the noble clans for a hefty price.[if:convo_astonished]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=OffNcRby}All right then. Let's get on the road.", null), null, null, null)
				.NpcLine(GameTexts.FindText("find_hideout_quest_brother_conversation_line_1", null), null, null, null, null)
				.NpcLine(GameTexts.FindText("find_hideout_quest_brother_conversation_line_2", null), null, null, null, null)
				.NpcLine(new TextObject("{=fp6QBO7l}I'll need to take these men with us. {RADAGOS.NAME} is a slippery one. I don't want him getting away.[if:convo_confused_voice]", null), null, null, null, null)
				.PlayerLine(new TextObject("{=RJ9NbuYr}So you want me to raise the money to ransom the little ones?", null), null, null, null)
				.NpcLine(new TextObject("{=4OUnPjZc}Indeed. You'll have to find a way to do that. Maybe this bronze thing can help.[if:convo_empathic_voice]", null), null, null, null, null)
				.NpcLine(new TextObject("{=5soUEFEJ}{TACTEOS.NAME} said it could be worth a fortune to the right person, if you manage not to get killed. If he's telling the truth, you must be careful. Never reveal that you have it. Try to understand its value, and how it can be sold.[if:convo_pondering]", null), null, null, null, null)
				.NpcLine(new TextObject("{=jPKIN2r4}One more thing. When you are talking to nobles and other people of importance, make sure you present yourself as someone from a distant but distinguished family.[if:convo_thinking]", null), null, null, null, null)
				.NpcLine(new TextObject("{=GVMGXfxS}You can use our family name if you like or make up a new one. You will have a better chance of obtaining an audience with nobles and it'll be easier for me to find you by asking around.[if:convo_normal]", null), null, null, null, null)
				.Consequence(new ConversationSentence.OnConsequenceDelegate(this.SelectClanName))
				.NpcLine(GameTexts.FindText("find_hideout_quest_brother_conversation_line_3", null), null, null, null, null)
				.CloseDialog(), this);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006D9C File Offset: 0x00004F9C
		private bool bandit_hideout_boss_fight_start_on_condition()
		{
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			return encounteredParty != null && !encounteredParty.IsMobile && encounteredParty.MapFaction != null && encounteredParty.MapFaction.IsBanditFaction && (!this._foughtWithRadagos && encounteredParty.IsSettlement && encounteredParty.Settlement.IsHideout && encounteredParty.Settlement == this._hideout && Mission.Current != null && Mission.Current.GetMissionBehavior<HideoutMissionController>() != null && Hero.OneToOneConversationHero != null) && Hero.OneToOneConversationHero == StoryModeHeroes.Radagos;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006E24 File Offset: 0x00005024
		private void bandit_hideout_start_duel_fight_on_consequence()
		{
			Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutMissionController.StartBossFightDuelMode;
			this._dueledRadagos = true;
			this._foughtWithRadagos = true;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006E50 File Offset: 0x00005050
		private bool bandit_hideout_continue_battle_on_clickable_condition(out TextObject explanation)
		{
			bool flag = false;
			foreach (Agent agent in Mission.Current.PlayerTeam.ActiveAgents)
			{
				if (!agent.IsMount && agent.Character != CharacterObject.PlayerCharacter)
				{
					flag = true;
					break;
				}
			}
			explanation = TextObject.GetEmpty();
			if (!flag)
			{
				explanation = new TextObject("{=F9HxO1iS}You don't have any men.", null);
			}
			return flag;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006ED8 File Offset: 0x000050D8
		private void bandit_hideout_continue_battle_on_consequence()
		{
			Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutMissionController.StartBossFightBattleMode;
			this._foughtWithRadagos = true;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006EFC File Offset: 0x000050FC
		private bool radagos_meeting_conversation_condition()
		{
			return this._foughtWithRadagos && Hero.OneToOneConversationHero == StoryModeHeroes.Radagos;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006F14 File Offset: 0x00005114
		private void radagos_meeting_conversation_consequence()
		{
			StoryModeHeroes.Radagos.SetHasMet();
			MobileParty partyBelongedTo = StoryModeHeroes.Radagos.PartyBelongedTo;
			DisableHeroAction.Apply(StoryModeHeroes.Radagos);
			this._talkedWithRadagos = true;
			Campaign.Current.ConversationManager.ConversationEndOneShot += this.OpenBrotherConversationMenu;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006F62 File Offset: 0x00005162
		private void OpenBrotherConversationMenu()
		{
			GameMenu.ActivateGameMenu("brother_chest_menu");
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006F70 File Offset: 0x00005170
		private bool brother_farewell_conversation_condition()
		{
			StringHelpers.SetCharacterProperties("RADAGOS", StoryModeHeroes.Radagos.CharacterObject, null, false);
			StringHelpers.SetCharacterProperties("TACTEOS", StoryModeHeroes.Tacitus.CharacterObject, null, false);
			StringHelpers.SetCharacterProperties("LITTLE_BROTHER", StoryModeHeroes.LittleBrother.CharacterObject, null, false);
			StringHelpers.SetCharacterProperties("LITTLE_SISTER", StoryModeHeroes.LittleSister.CharacterObject, null, false);
			return Hero.OneToOneConversationHero == StoryModeHeroes.ElderBrother && this._talkedWithRadagos;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006FF0 File Offset: 0x000051F0
		private void SelectClanName()
		{
			InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=RSn1j3tA}Choose your family name: ", null).ToString(), string.Empty, true, false, GameTexts.FindText("str_done", null).ToString(), null, new Action<string>(this.OnChangeClanNameDone), null, false, new Func<string, Tuple<bool, string>>(FactionHelper.IsClanNameApplicable), "", Clan.PlayerClan.Name.ToString()), false, false);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007060 File Offset: 0x00005260
		private void OnChangeClanNameDone(string newClanName)
		{
			TextObject textObject = GameTexts.FindText("str_generic_clan_name", null);
			textObject.SetTextVariable("CLAN_NAME", new TextObject(newClanName, null));
			Clan.PlayerClan.ChangeClanName(textObject, textObject);
			Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<BannerEditorState>(), 0);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000070B7 File Offset: 0x000052B7
		private bool OpenBannerSelectionScreen()
		{
			return true;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000070BC File Offset: 0x000052BC
		private void OnGameMenuOpened(MenuCallbackArgs menuCallbackArgs)
		{
			StoryModeHeroes.Radagos.Heal(StoryModeHeroes.Radagos.MaxHitPoints, false);
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == this._hideout && this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.None && menuCallbackArgs.MenuContext.GameMenu.StringId != "radagos_hideout" && menuCallbackArgs.MenuContext.GameMenu.StringId != "brother_chest_menu")
			{
				GameMenu.SwitchToMenu("radagos_hideout");
			}
			if (this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Victory && this._talkedWithRadagos && menuCallbackArgs.MenuContext.GameMenu.StringId != "brother_chest_menu")
			{
				Campaign.Current.GameMenuManager.SetNextMenu("brother_chest_menu");
				return;
			}
			if (this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Defeated || this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Retreated)
			{
				foreach (MobileParty mobileParty in this._hideout.Parties)
				{
					foreach (TroopRosterElement troopRosterElement in mobileParty.MemberRoster.GetTroopRoster())
					{
						if (troopRosterElement.Character.IsHero)
						{
							troopRosterElement.Character.HeroObject.Heal(50 - troopRosterElement.Character.HeroObject.HitPoints, false);
						}
						else
						{
							int elementWoundedNumber = mobileParty.MemberRoster.GetElementWoundedNumber(mobileParty.MemberRoster.FindIndexOfTroop(troopRosterElement.Character));
							if (elementWoundedNumber > 0)
							{
								mobileParty.MemberRoster.AddToCounts(troopRosterElement.Character, 0, false, -elementWoundedNumber, 0, true, -1);
							}
						}
					}
					if (!mobileParty.IsBanditBossParty && mobileParty.MemberRoster.TotalManCount < 4)
					{
						int totalManCount = mobileParty.MemberRoster.TotalManCount;
						CharacterObject @object = Campaign.Current.ObjectManager.GetObject<CharacterObject>("storymode_quest_raider");
						mobileParty.MemberRoster.AddToCounts(@object, 4 - totalManCount, false, 0, 0, true, -1);
					}
					if (MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.1.1", 0) && mobileParty.IsBanditBossParty && mobileParty.MemberRoster.GetTroopCount(StoryModeHeroes.Radagos.CharacterObject) <= 0)
					{
						if (StoryModeHeroes.Radagos.HeroState != Hero.CharacterStates.Active)
						{
							StoryModeHeroes.Radagos.ChangeState(Hero.CharacterStates.Active);
						}
						mobileParty.MemberRoster.AddToCounts(StoryModeHeroes.Radagos.CharacterObject, 1, false, 0, 0, true, -1);
					}
				}
				if (Hero.MainHero.IsPrisoner)
				{
					EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
					Hero elderBrother = StoryModeHeroes.ElderBrother;
					if (elderBrother.PartyBelongedToAsPrisoner != null)
					{
						EndCaptivityAction.ApplyByPeace(elderBrother, null);
					}
					if (!elderBrother.IsActive)
					{
						elderBrother.ChangeState(Hero.CharacterStates.Active);
					}
					if (elderBrother.PartyBelongedTo == null)
					{
						AddHeroToPartyAction.Apply(elderBrother, MobileParty.MainParty, false);
					}
				}
				if (this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Defeated || this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Retreated)
				{
					TextObject textObject = new TextObject("{=Zq9qXcCk}You are defeated by the {RADAGOS.NAME}' Party, but your brother saved you. It doesn't look like they're going anywhere, though, so you should attack again once you're ready. You must have at least {NUMBER} members in your party. If you don't, go back to {QUEST_VILLAGE} and recruit some more troops.", null);
					StringHelpers.SetCharacterProperties("RADAGOS", StoryModeHeroes.Radagos.CharacterObject, textObject, false);
					textObject.SetTextVariable("NUMBER", 4);
					textObject.SetTextVariable("QUEST_VILLAGE", Settlement.Find("village_ES3_2").Name);
					InformationManager.ShowInquiry(new InquiryData(((this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Defeated) ? new TextObject("{=FPhWhjq7}Defeated", null) : new TextObject("{=w6Wa3lSL}Retreated", null)).ToString(), textObject.ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, delegate
					{
						this._hideout.IsVisible = true;
					}, null, "", 0f, null, null, null), false, false);
				}
				if (menuCallbackArgs.MenuContext.GameMenu.StringId == "radagos_hideout" && this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Retreated)
				{
					PlayerEncounter.Finish(true);
				}
				if (this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Defeated || this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Retreated)
				{
					if (Hero.MainHero.HitPoints < 50)
					{
						Hero.MainHero.Heal(50 - Hero.MainHero.HitPoints, false);
					}
					Hero elderBrother2 = StoryModeHeroes.ElderBrother;
					if (elderBrother2.HitPoints < 50)
					{
						elderBrother2.Heal(50 - elderBrother2.HitPoints, false);
					}
					if (elderBrother2.PartyBelongedToAsPrisoner != null)
					{
						EndCaptivityAction.ApplyByPeace(elderBrother2, null);
					}
					if (elderBrother2.PartyBelongedTo == null)
					{
						PartyBase.MainParty.MemberRoster.AddToCounts(elderBrother2.CharacterObject, 1, false, 0, 0, true, -1);
					}
				}
				this._hideoutBattleEndState = FindHideoutTutorialQuest.HideoutBattleEndState.None;
				this._foughtWithRadagos = false;
				foreach (MobileParty mobileParty2 in this._hideout.Parties)
				{
					foreach (TroopRosterElement troopRosterElement2 in mobileParty2.PrisonRoster.GetTroopRoster())
					{
						if (this._mainPartyTroopBackup.Contains(troopRosterElement2.Character))
						{
							int num = mobileParty2.PrisonRoster.FindIndexOfTroop(troopRosterElement2.Character);
							int elementWoundedNumber2 = mobileParty2.PrisonRoster.GetElementWoundedNumber(num);
							int num2 = mobileParty2.PrisonRoster.GetTroopCount(troopRosterElement2.Character) - elementWoundedNumber2;
							if (num2 > 0)
							{
								mobileParty2.PrisonRoster.AddToCounts(troopRosterElement2.Character, -num2, false, 0, 0, true, -1);
								PartyBase.MainParty.MemberRoster.AddToCounts(troopRosterElement2.Character, num2, false, 0, 0, true, -1);
							}
						}
					}
				}
				List<CharacterObject> mainPartyTroopBackup = this._mainPartyTroopBackup;
				if (mainPartyTroopBackup == null)
				{
					return;
				}
				mainPartyTroopBackup.Clear();
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000769C File Offset: 0x0000589C
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.Victory && !this._talkedWithRadagos)
			{
				CampaignMission.OpenConversationMission(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, false, false, false, false, false), new ConversationCharacterData(StoryModeHeroes.Radagos.CharacterObject, null, true, true, false, false, false, false), "", "", false);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000076F2 File Offset: 0x000058F2
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._raiderParties.Contains(mobileParty))
			{
				this._raiderParties.Remove(mobileParty);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007710 File Offset: 0x00005910
		private void OnGameLoadFinished()
		{
			for (int i = this._hideout.Parties.Count - 1; i >= 0; i--)
			{
				MobileParty mobileParty = this._hideout.Parties[i];
				if (mobileParty.IsBandit && mobileParty.MapEvent == null)
				{
					while (mobileParty.MemberRoster.TotalManCount > 4)
					{
						foreach (TroopRosterElement troopRosterElement in mobileParty.MemberRoster.GetTroopRoster())
						{
							if (!troopRosterElement.Character.IsHero)
							{
								mobileParty.MemberRoster.RemoveTroop(troopRosterElement.Character, 1, default(UniqueTroopDescriptor), 0);
							}
							if (mobileParty.MemberRoster.TotalManCount <= 4)
							{
								break;
							}
						}
					}
				}
			}
			while (this._hideout.Party.MemberRoster.TotalManCount > 4 && this._hideout.Party.MapEvent == null)
			{
				foreach (TroopRosterElement troopRosterElement2 in this._hideout.Party.MemberRoster.GetTroopRoster())
				{
					if (!troopRosterElement2.Character.IsHero)
					{
						this._hideout.Party.MemberRoster.RemoveTroop(troopRosterElement2.Character, 1, default(UniqueTroopDescriptor), 0);
					}
					if (this._hideout.Party.MemberRoster.TotalManCount <= 4)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000078C0 File Offset: 0x00005AC0
		private void AddGameMenus()
		{
			StringHelpers.SetCharacterProperties("TACTEOS", StoryModeHeroes.Tacitus.CharacterObject, null, false);
			StringHelpers.SetCharacterProperties("RADAGOS", StoryModeHeroes.Radagos.CharacterObject, null, false);
			base.AddGameMenu("radagos_hideout", new TextObject("{=z8LQn2Uh}You have arrived at the hideout.", null), new OnInitDelegate(this.radagos_hideout_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None);
			base.AddGameMenuOption("radagos_hideout", "enter_hideout", new TextObject("{=zxMOqlhs}Attack", null), new GameMenuOption.OnConditionDelegate(this.enter_radagos_hideout_condition), new GameMenuOption.OnConsequenceDelegate(this.enter_radagos_hideout_on_consequence), false, -1);
			base.AddGameMenuOption("radagos_hideout", "leave_hideout", new TextObject("{=3sRdGQou}Leave", null), new GameMenuOption.OnConditionDelegate(this.leave_radagos_hideout_condition), new GameMenuOption.OnConsequenceDelegate(this.leave_radagos_hideout_on_consequence), true, -1);
			base.AddGameMenu("brother_chest_menu", new TextObject("{=bhQ6Jbom}You come across a chest with an old piece of bronze in it. It's so battered and corroded that it could have been anything from a cup to a crown. This must be the chest {TACTEOS.NAME} mentioned to you, that had something to do with 'Neretzes' Folly'.", null), new OnInitDelegate(this.brother_chest_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None);
			base.AddGameMenuOption("brother_chest_menu", "brother_chest_menu_continue", new TextObject("{=DM6luo3c}Continue", null), new GameMenuOption.OnConditionDelegate(this.brother_chest_menu_on_condition), new GameMenuOption.OnConsequenceDelegate(this.brother_chest_menu_on_consequence), false, -1);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000079E2 File Offset: 0x00005BE2
		private void brother_chest_menu_on_init(MenuCallbackArgs menuCallbackArgs)
		{
			if (this._talkedWithBrother)
			{
				this._hideoutBattleEndState = FindHideoutTutorialQuest.HideoutBattleEndState.None;
				PlayerEncounter.Finish(true);
				base.CompleteQuestWithSuccess();
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000079FF File Offset: 0x00005BFF
		private bool brother_chest_menu_on_condition(MenuCallbackArgs menuCallbackArgs)
		{
			menuCallbackArgs.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return base.IsOngoing;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007A10 File Offset: 0x00005C10
		private void brother_chest_menu_on_consequence(MenuCallbackArgs menuCallbackArgs)
		{
			this._talkedWithBrother = true;
			CampaignMission.OpenConversationMission(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, false, false, false, false, false), new ConversationCharacterData(StoryModeHeroes.ElderBrother.CharacterObject, null, true, true, false, false, false, false), "", "", false);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007A5C File Offset: 0x00005C5C
		private void radagos_hideout_menu_on_init(MenuCallbackArgs menuCallbackArgs)
		{
			menuCallbackArgs.MenuTitle = new TextObject("{=8OIwHZF1}Hideout", null);
			StringHelpers.SetCharacterProperties("RADAGOS", StoryModeHeroes.Radagos.CharacterObject, null, false);
			if (PlayerEncounter.Current != null)
			{
				MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
				if (playerMapEvent != null)
				{
					if (playerMapEvent.WinningSide == playerMapEvent.PlayerSide)
					{
						if (this._dueledRadagos)
						{
							AchievementsCampaignBehavior behavior = Campaign.Current.CampaignBehaviorManager.GetBehavior<AchievementsCampaignBehavior>();
							if (behavior != null)
							{
								behavior.OnRadagosDuelWon();
							}
						}
						this._hideoutBattleEndState = FindHideoutTutorialQuest.HideoutBattleEndState.Victory;
					}
					else if (playerMapEvent.WinningSide == BattleSideEnum.None)
					{
						this._hideoutBattleEndState = FindHideoutTutorialQuest.HideoutBattleEndState.Retreated;
					}
					else
					{
						this._hideoutBattleEndState = FindHideoutTutorialQuest.HideoutBattleEndState.Defeated;
					}
					this._dueledRadagos = false;
				}
				if (this._hideoutBattleEndState != FindHideoutTutorialQuest.HideoutBattleEndState.None)
				{
					PlayerEncounter.Update();
				}
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007B08 File Offset: 0x00005D08
		private bool enter_radagos_hideout_condition(MenuCallbackArgs menuCallbackArgs)
		{
			menuCallbackArgs.optionLeaveType = GameMenuOption.LeaveType.Mission;
			if (MobileParty.MainParty.MemberRoster.TotalManCount < 4)
			{
				menuCallbackArgs.IsEnabled = false;
				menuCallbackArgs.Tooltip = new TextObject("{=kaZ1XtDX}You are not strong enough to attack. Recruit more troops from the village.", null);
			}
			return base.IsOngoing && this._hideoutBattleEndState == FindHideoutTutorialQuest.HideoutBattleEndState.None;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007B5C File Offset: 0x00005D5C
		private void enter_radagos_hideout_on_consequence(MenuCallbackArgs menuCallbackArgs)
		{
			this._hideoutBattleEndState = FindHideoutTutorialQuest.HideoutBattleEndState.None;
			this._mainPartyTroopBackup = new List<CharacterObject>();
			foreach (TroopRosterElement troopRosterElement in PartyBase.MainParty.MemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero)
				{
					this._mainPartyTroopBackup.Add(troopRosterElement.Character);
				}
			}
			if (!this._hideout.Hideout.IsInfested || this._hideout.Parties.Count < 3)
			{
				this.InitializeHideout();
			}
			foreach (MobileParty mobileParty in this._hideout.Parties)
			{
				if (mobileParty.IsBanditBossParty && mobileParty.MemberRoster.Contains(mobileParty.Party.Culture.BanditBoss))
				{
					mobileParty.MemberRoster.RemoveTroop(mobileParty.Party.Culture.BanditBoss, 1, default(UniqueTroopDescriptor), 0);
				}
			}
			if (PlayerEncounter.Battle == null)
			{
				PlayerEncounter.StartBattle();
				PlayerEncounter.Update();
			}
			CampaignMission.OpenHideoutBattleMission("forest_hideout_003", null, true);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00007CB8 File Offset: 0x00005EB8
		private bool leave_radagos_hideout_condition(MenuCallbackArgs menuCallbackArgs)
		{
			menuCallbackArgs.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return base.IsOngoing;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007CC8 File Offset: 0x00005EC8
		private void leave_radagos_hideout_on_consequence(MenuCallbackArgs menuCallbackArgs)
		{
			this._hideoutBattleEndState = FindHideoutTutorialQuest.HideoutBattleEndState.None;
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007CD8 File Offset: 0x00005ED8
		[GameMenuInitializationHandler("radagos_hideout")]
		[GameMenuInitializationHandler("brother_chest_menu")]
		private static void quest_game_menus_on_init_background(MenuCallbackArgs args)
		{
			Settlement settlement = Settlement.Find(FindHideoutTutorialQuest._activeHideoutStringId);
			args.MenuContext.SetBackgroundMeshName(settlement.Hideout.WaitMeshName);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007D06 File Offset: 0x00005F06
		protected override void OnCompleteWithSuccess()
		{
			this._hideout.Party.SetCustomName(null);
			this._hideout.Party.SetVisualAsDirty();
			StoryModeHeroes.Radagos.Heal(100, false);
			StoryModeManager.Current.MainStoryLine.CompleteTutorialPhase(false);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007D46 File Offset: 0x00005F46
		internal static void AutoGeneratedStaticCollectObjectsFindHideoutTutorialQuest(object o, List<object> collectedObjects)
		{
			((FindHideoutTutorialQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007D54 File Offset: 0x00005F54
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this._hideout);
			collectedObjects.Add(this._raiderParties);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007D75 File Offset: 0x00005F75
		internal static object AutoGeneratedGetMemberValue_hideout(object o)
		{
			return ((FindHideoutTutorialQuest)o)._hideout;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007D82 File Offset: 0x00005F82
		internal static object AutoGeneratedGetMemberValue_raiderParties(object o)
		{
			return ((FindHideoutTutorialQuest)o)._raiderParties;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007D8F File Offset: 0x00005F8F
		internal static object AutoGeneratedGetMemberValue_talkedWithRadagos(object o)
		{
			return ((FindHideoutTutorialQuest)o)._talkedWithRadagos;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007DA1 File Offset: 0x00005FA1
		internal static object AutoGeneratedGetMemberValue_talkedWithBrother(object o)
		{
			return ((FindHideoutTutorialQuest)o)._talkedWithBrother;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007DB3 File Offset: 0x00005FB3
		internal static object AutoGeneratedGetMemberValue_hideoutBattleEndState(object o)
		{
			return ((FindHideoutTutorialQuest)o)._hideoutBattleEndState;
		}

		// Token: 0x04000080 RID: 128
		private const string RaiderPartyStringId = "radagos_raider_party_";

		// Token: 0x04000081 RID: 129
		private const string TutorialHideoutSceneName = "forest_hideout_003";

		// Token: 0x04000082 RID: 130
		private const int RaiderPartyCount = 2;

		// Token: 0x04000083 RID: 131
		private const int RaiderPartySize = 4;

		// Token: 0x04000084 RID: 132
		private const int MainPartyHealHitPointLimit = 50;

		// Token: 0x04000085 RID: 133
		private const int MaximumHealth = 100;

		// Token: 0x04000086 RID: 134
		private const int PlayerPartySizeMinLimitToAttack = 4;

		// Token: 0x04000087 RID: 135
		[SaveableField(1)]
		private readonly Settlement _hideout;

		// Token: 0x04000088 RID: 136
		[SaveableField(2)]
		private List<MobileParty> _raiderParties;

		// Token: 0x04000089 RID: 137
		private bool _foughtWithRadagos;

		// Token: 0x0400008A RID: 138
		private bool _dueledRadagos;

		// Token: 0x0400008B RID: 139
		[SaveableField(4)]
		private bool _talkedWithRadagos;

		// Token: 0x0400008C RID: 140
		[SaveableField(5)]
		private bool _talkedWithBrother;

		// Token: 0x0400008D RID: 141
		[SaveableField(6)]
		private FindHideoutTutorialQuest.HideoutBattleEndState _hideoutBattleEndState;

		// Token: 0x0400008E RID: 142
		private List<CharacterObject> _mainPartyTroopBackup;

		// Token: 0x0400008F RID: 143
		private static string _activeHideoutStringId;

		// Token: 0x0200005D RID: 93
		public enum HideoutBattleEndState
		{
			// Token: 0x040001FA RID: 506
			None,
			// Token: 0x040001FB RID: 507
			Retreated,
			// Token: 0x040001FC RID: 508
			Defeated,
			// Token: 0x040001FD RID: 509
			Victory
		}
	}
}
