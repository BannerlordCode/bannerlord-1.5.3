using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Missions.MissionLogics.Hideout;
using StoryMode.StoryModeObjects;
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
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;

namespace StoryMode.Quests.PlayerClanQuests
{
	// Token: 0x02000030 RID: 48
	public class RescueFamilyQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x060002CB RID: 715 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		internal RescueFamilyQuestBehavior()
		{
			this._rescueFamilyQuestReadyToStart = false;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000EF04 File Offset: 0x0000D104
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(RescueFamilyQuestBehavior.OnGameLoadedEvent));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnQuestCompletedEvent.AddNonSerializedListener(this, new Action<QuestBase, QuestBase.QuestCompleteDetails>(this.OnQuestCompleted));
			CampaignEvents.CanHaveCampaignIssuesEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.CanHaveCampaignIssuesInfoIsRequested));
			CampaignEvents.CanHeroDieEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.CanHeroDie));
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EF84 File Offset: 0x0000D184
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<bool>("_rescueFamilyQuestReadyToStart", ref this._rescueFamilyQuestReadyToStart);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EF98 File Offset: 0x0000D198
		private static void OnGameLoadedEvent(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EF9C File Offset: 0x0000D19C
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (this._rescueFamilyQuestReadyToStart && party == MobileParty.MainParty && settlement.IsTown && !settlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction) && GameStateManager.Current.ActiveState is MapState && !Campaign.Current.ConversationManager.IsConversationFlowActive)
			{
				bool flag = false;
				foreach (QuestBase questBase in Campaign.Current.QuestManager.Quests)
				{
					Hero questGiver = questBase.QuestGiver;
					if (((questGiver != null) ? questGiver.CurrentSettlement : null) == settlement)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					new RescueFamilyQuestBehavior.RescueFamilyQuest().StartQuest();
					this._rescueFamilyQuestReadyToStart = false;
					StoryModeHeroes.Radagos.UpdateLastKnownClosestSettlement(Settlement.CurrentSettlement);
					CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, true, false, false, false, false), new ConversationCharacterData(StoryModeHeroes.Radagos.CharacterObject, null, true, true, false, false, false, false));
				}
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000F0BC File Offset: 0x0000D2BC
		private void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			if (quest is RebuildPlayerClanQuest)
			{
				this._rescueFamilyQuestReadyToStart = true;
				return;
			}
			if (quest is RescueFamilyQuestBehavior.RescueFamilyQuest)
			{
				this._rescueFamilyQuestReadyToStart = false;
				StoryModeHeroes.Radagos.CharacterObject.SetTransferableInPartyScreen(true);
				StoryModeHeroes.Radagos.CharacterObject.SetTransferableInHideouts(true);
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000F108 File Offset: 0x0000D308
		private void CanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
		{
			if (!StoryModeManager.Current.MainStoryLine.FamilyRescued && (hero == StoryModeHeroes.Radagos || hero == StoryModeHeroes.RadagosHenchman))
			{
				result = false;
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F130 File Offset: 0x0000D330
		private void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			if (hero == StoryModeHeroes.RadagosHenchman && (!StoryModeManager.Current.MainStoryLine.FamilyRescued || this._rescueFamilyQuestReadyToStart || (Campaign.Current.QuestManager.IsThereActiveQuestWithType(typeof(RescueFamilyQuestBehavior.RescueFamilyQuest)) && causeOfDeath != KillCharacterAction.KillCharacterActionDetail.Executed)))
			{
				result = false;
			}
		}

		// Token: 0x040000E3 RID: 227
		private bool _rescueFamilyQuestReadyToStart;

		// Token: 0x02000078 RID: 120
		public class RescueFamilyQuest : StoryModeQuestBase
		{
			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x06000631 RID: 1585 RVA: 0x00021DF4 File Offset: 0x0001FFF4
			private TextObject _startQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=FyzsAZx8}{RADAGOS.LINK} said that he knows where your siblings are. He offered to attack together. He will wait for you at the hideout that he mentioned about near {SETTLEMENT_LINK}. You can see the hideout marked on the map.", null);
					StringHelpers.SetCharacterProperties("RADAGOS", this._radagos.CharacterObject, textObject, false);
					Town town = SettlementHelper.FindNearestTownToSettlement(this._hideout.SettlementComponent.Settlement, MobileParty.NavigationType.Default, null);
					textObject.SetTextVariable("SETTLEMENT_LINK", town.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x06000632 RID: 1586 RVA: 0x00021E58 File Offset: 0x00020058
			private TextObject _defeatedQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=Ga8mDgab}You've been defeated at {HIDEOUT_BOSS.LINK}'s hideout. You can attack again when you are ready.", null);
					StringHelpers.SetCharacterProperties("HIDEOUT_BOSS", this._hideoutBoss.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x06000633 RID: 1587 RVA: 0x00021E8C File Offset: 0x0002008C
			private TextObject _letGoRadagosEndQuestLogText
			{
				get
				{
					TextObject textObject = GameTexts.FindText("rescue_family_quest_let_go_radagos_quest_log", null);
					StringHelpers.SetCharacterProperties("RADAGOS", this._radagos.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x06000634 RID: 1588 RVA: 0x00021EC0 File Offset: 0x000200C0
			private TextObject _executeRadagosEndQuestLogText
			{
				get
				{
					TextObject textObject = GameTexts.FindText("rescue_family_quest_execute_radagos_quest_log", null);
					StringHelpers.SetCharacterProperties("RADAGOS", this._radagos.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x06000635 RID: 1589 RVA: 0x00021EF2 File Offset: 0x000200F2
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=HPNuqbSf}Rescue Your Family", null);
				}
			}

			// Token: 0x06000636 RID: 1590 RVA: 0x00021F00 File Offset: 0x00020100
			public RescueFamilyQuest()
				: base("rescue_your_family_storymode_quest", null, CampaignTime.Never)
			{
				StoryModeManager.Current.MainStoryLine.FamilyRescued = true;
				this._radagos = StoryModeHeroes.Radagos;
				this._radagos.CharacterObject.SetTransferableInPartyScreen(false);
				this._radagos.CharacterObject.SetTransferableInHideouts(false);
				this._hideoutBoss = StoryModeHeroes.RadagosHenchman;
				this._targetSettlementForSiblings = null;
				this._hideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement s) => !s.IsSettlementBusy(this)).Settlement;
				this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.None;
				this._raiderParties = new List<MobileParty>();
				this.InitializeHideout();
				base.AddTrackedObject(this._hideout);
				this.SetDialogs();
				this.AddGameMenus();
			}

			// Token: 0x06000637 RID: 1591 RVA: 0x00021FC0 File Offset: 0x000201C0
			[LoadInitializationCallback]
			private void OnLoad(MetaData metaData, ObjectLoadData objectLoadData)
			{
				if (objectLoadData.HasMember(2, (int)objectLoadData.TypeDefinition.TypeLevel))
				{
					bool flag = (bool)objectLoadData.GetMemberValueBySaveId(2, (int)objectLoadData.TypeDefinition.TypeLevel);
					bool flag2 = (bool)objectLoadData.GetMemberValueBySaveId(3, (int)objectLoadData.TypeDefinition.TypeLevel);
					bool flag3 = (bool)objectLoadData.GetMemberValueBySaveId(4, (int)objectLoadData.TypeDefinition.TypeLevel);
					bool flag4 = (bool)objectLoadData.GetMemberValueBySaveId(5, (int)objectLoadData.TypeDefinition.TypeLevel);
					if (flag)
					{
						this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.ReunionTalkWithRadagosDone;
					}
					if (flag2)
					{
						this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutTalkWithRadagosDone;
					}
					if (flag3)
					{
						this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.ReunionTalkWithBrotherDone;
					}
					if (flag4)
					{
						this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.GoodbyeTalkWithRadagosDone;
					}
				}
			}

			// Token: 0x06000638 RID: 1592 RVA: 0x0002206C File Offset: 0x0002026C
			protected override void InitializeQuestOnGameLoad()
			{
				this._radagos = StoryModeHeroes.Radagos;
				this._radagos.CharacterObject.SetTransferableInPartyScreen(false);
				this._radagos.CharacterObject.SetTransferableInHideouts(false);
				this._hideoutBoss = StoryModeHeroes.RadagosHenchman;
				StoryModeHeroes.Radagos.CharacterObject.HiddenInEncyclopedia = false;
				StoryModeHeroes.RadagosHenchman.CharacterObject.HiddenInEncyclopedia = false;
				StoryModeHeroes.Radagos.UpdateLastKnownClosestSettlement(this._hideout);
				StoryModeHeroes.RadagosHenchman.UpdateLastKnownClosestSettlement(this._hideout);
				this.SetDialogs();
				this.AddGameMenus();
				this.SelectTargetSettlementForSiblings();
				if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.CurrentVersion.IsOlderThan(ApplicationVersion.FromString("v1.4.0", 0)) && this._rescueFamilyQuestState == RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutTalkWithRadagosDone)
				{
					MapEvent battle = PlayerEncounter.Battle;
					if (((battle != null) ? battle.MapEventSettlement : null) == this._hideout)
					{
						this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutBattleInProgress;
					}
				}
			}

			// Token: 0x06000639 RID: 1593 RVA: 0x00022150 File Offset: 0x00020350
			protected override void OnStartQuest()
			{
				base.OnStartQuest();
				StoryModeHeroes.Radagos.CharacterObject.HiddenInEncyclopedia = false;
				StoryModeHeroes.RadagosHenchman.CharacterObject.HiddenInEncyclopedia = false;
				StoryModeHeroes.Radagos.UpdateLastKnownClosestSettlement(this._hideout);
				StoryModeHeroes.RadagosHenchman.UpdateLastKnownClosestSettlement(this._hideout);
			}

			// Token: 0x0600063A RID: 1594 RVA: 0x000221A3 File Offset: 0x000203A3
			protected override void OnFinalize()
			{
				base.OnFinalize();
				StoryModeHeroes.Radagos.CharacterObject.HiddenInEncyclopedia = true;
				StoryModeHeroes.RadagosHenchman.CharacterObject.HiddenInEncyclopedia = true;
			}

			// Token: 0x0600063B RID: 1595 RVA: 0x000221CB File Offset: 0x000203CB
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == StoryModeHeroes.Radagos && StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted && !StoryModeManager.Current.MainStoryLine.FamilyRescued)
				{
					result = false;
				}
			}

			// Token: 0x0600063C RID: 1596 RVA: 0x00022200 File Offset: 0x00020400
			protected override void OnCompleteWithSuccess()
			{
				if (!this._hideoutBoss.IsDead)
				{
					KillCharacterAction.ApplyByRemove(this._hideoutBoss, false, true);
				}
				if (this._rescueFamilyQuestState == RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.GoodbyeTalkWithRadagosDone && !this._radagos.IsDisabled && !this._radagos.IsDead)
				{
					DisableHeroAction.Apply(this._radagos);
				}
				StoryModeHeroes.ElderBrother.Clan = Clan.PlayerClan;
				StoryModeHeroes.LittleBrother.Clan = Clan.PlayerClan;
				StoryModeHeroes.ElderBrother.ChangeState(Hero.CharacterStates.Active);
				EnterSettlementAction.ApplyForCharacterOnly(StoryModeHeroes.ElderBrother, this._targetSettlementForSiblings);
				if (StoryModeHeroes.LittleBrother.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
				{
					StoryModeHeroes.LittleBrother.ChangeState(Hero.CharacterStates.Active);
					EnterSettlementAction.ApplyForCharacterOnly(StoryModeHeroes.LittleBrother, this._targetSettlementForSiblings);
					StoryModeHelpers.SetPlayerSiblingsSkillsIfNeeded(StoryModeHeroes.LittleBrother);
				}
				else
				{
					StoryModeHeroes.LittleBrother.ChangeState(Hero.CharacterStates.NotSpawned);
				}
				StoryModeHeroes.ElderBrother.UpdateLastKnownClosestSettlement(this._targetSettlementForSiblings);
				StoryModeHeroes.LittleBrother.UpdateLastKnownClosestSettlement(this._targetSettlementForSiblings);
				TextObject textObject = new TextObject("{=PDlaPVIP}{PLAYER_LITTLE_BROTHER.NAME} is the little brother of {PLAYER.LINK}.", null);
				StringHelpers.SetCharacterProperties("PLAYER_LITTLE_BROTHER", StoryModeHeroes.LittleBrother.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				StoryModeHeroes.LittleBrother.EncyclopediaText = textObject;
				TextObject textObject2 = new TextObject("{=LcxfWLgd}{PLAYER_BROTHER.NAME} is the elder brother of {PLAYER.LINK}.", null);
				StringHelpers.SetCharacterProperties("PLAYER_BROTHER", StoryModeHeroes.ElderBrother.CharacterObject, textObject2, false);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject2, false);
				StoryModeHeroes.ElderBrother.EncyclopediaText = textObject2;
				ModuleInfo moduleInfo = ModuleHelper.GetModuleInfo("NavalDLC");
				if (moduleInfo == null || !moduleInfo.IsActive)
				{
					StoryModeHeroes.LittleSister.Clan = Clan.PlayerClan;
					if (StoryModeHeroes.LittleSister.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
					{
						StoryModeHeroes.LittleSister.ChangeState(Hero.CharacterStates.Active);
						EnterSettlementAction.ApplyForCharacterOnly(StoryModeHeroes.LittleSister, this._targetSettlementForSiblings);
						StoryModeHelpers.SetPlayerSiblingsSkillsIfNeeded(StoryModeHeroes.LittleSister);
					}
					else
					{
						StoryModeHeroes.LittleSister.ChangeState(Hero.CharacterStates.NotSpawned);
					}
					StoryModeHeroes.LittleSister.UpdateLastKnownClosestSettlement(this._targetSettlementForSiblings);
					TextObject textObject3 = new TextObject("{=7XTkTi9B}{PLAYER_LITTLE_SISTER.NAME} is the little sister of {PLAYER.LINK}.", null);
					StringHelpers.SetCharacterProperties("PLAYER_LITTLE_SISTER", StoryModeHeroes.LittleSister.CharacterObject, textObject3, false);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject3, false);
					StoryModeHeroes.LittleSister.EncyclopediaText = textObject3;
				}
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x0002244C File Offset: 0x0002064C
			protected override void OnTimedOut()
			{
				base.OnTimedOut();
				KillCharacterAction.ApplyByRemove(StoryModeHeroes.LittleSister, false, true);
				KillCharacterAction.ApplyByRemove(StoryModeHeroes.LittleBrother, false, true);
				KillCharacterAction.ApplyByRemove(StoryModeHeroes.ElderBrother, false, true);
			}

			// Token: 0x0600063E RID: 1598 RVA: 0x00022478 File Offset: 0x00020678
			private void InitializeHideout()
			{
				this.CheckIfHideoutIsReady();
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x00022480 File Offset: 0x00020680
			private void CheckIfHideoutIsReady()
			{
				if (!this._hideout.Hideout.IsInfested)
				{
					for (int i = 0; i < 2; i++)
					{
						if (!this._hideout.Hideout.IsInfested)
						{
							this._raiderParties.Add(this.CreateRaiderParty(i, false));
						}
					}
				}
				this._hideout.IsVisible = true;
			}

			// Token: 0x06000640 RID: 1600 RVA: 0x000224DC File Offset: 0x000206DC
			private void AddRadagosHenchmanToHideout()
			{
				if (!this._hideout.Parties.Any<MobileParty>((MobileParty p) => p.IsBanditBossParty))
				{
					this._raiderParties.Add(this.CreateRaiderParty(3, true));
				}
				foreach (MobileParty mobileParty in this._hideout.Parties)
				{
					if (mobileParty.IsBanditBossParty)
					{
						if (mobileParty.MemberRoster.GetTroopRoster().Any<TroopRosterElement>((TroopRosterElement t) => t.Character == this._hideout.Culture.BanditBoss))
						{
							TroopRosterElement troopRosterElement = mobileParty.MemberRoster.GetTroopRoster().First<TroopRosterElement>((TroopRosterElement t) => t.Character == this._hideout.Culture.BanditBoss);
							mobileParty.MemberRoster.RemoveTroop(troopRosterElement.Character, 1, default(UniqueTroopDescriptor), 0);
						}
						this._hideoutBoss.ChangeState(Hero.CharacterStates.Active);
						if (this._hideoutBoss.PartyBelongedTo == null)
						{
							mobileParty.MemberRoster.AddToCounts(this._hideoutBoss.CharacterObject, 1, true, 0, 0, true, -1);
							break;
						}
						break;
					}
				}
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x00022614 File Offset: 0x00020814
			private MobileParty CreateRaiderParty(int number, bool isBanditBossParty)
			{
				Clan clan = this._hideout.OwnerClan;
				if (clan.StringId.Equals("looters"))
				{
					clan = Clan.All.Where<Clan>((Clan c) => c.IsBanditFaction && c.Culture == this._hideout.Culture).GetRandomElementInefficiently<Clan>();
				}
				MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("rescue_family_quest_raider_party_" + number, clan, this._hideout.Hideout, isBanditBossParty, null, this._hideout.GatePosition);
				CharacterObject @object = Campaign.Current.ObjectManager.GetObject<CharacterObject>(this._hideout.Culture.StringId + "_bandit");
				mobileParty.MemberRoster.AddToCounts(@object, 5, false, 0, 0, true, -1);
				mobileParty.Party.SetCustomName(new TextObject("{=u1Pkt4HC}Raiders", null));
				mobileParty.ActualClan = clan;
				mobileParty.Position = this._hideout.Position;
				mobileParty.Party.SetVisualAsDirty();
				mobileParty.InitializePartyTrade(QuestHelper.CalculateInitialGoldForBanditQuestParty(mobileParty));
				mobileParty.SetMoveGoToSettlement(this._hideout, MobileParty.NavigationType.Default, false);
				mobileParty.Ai.SetDoNotMakeNewDecisions(true);
				mobileParty.SetPartyUsedByQuest(true);
				EnterSettlementAction.ApplyForParty(mobileParty, this._hideout);
				return mobileParty;
			}

			// Token: 0x06000642 RID: 1602 RVA: 0x00022738 File Offset: 0x00020938
			private void SelectTargetSettlementForSiblings()
			{
				Town town = SettlementHelper.FindNearestTownToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement s) => s.OwnerClan.MapFaction == Clan.PlayerClan.MapFaction);
				this._targetSettlementForSiblings = ((town != null) ? town.Settlement : null);
				if (this._targetSettlementForSiblings == null)
				{
					Town town2 = SettlementHelper.FindNearestTownToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement s) => !Clan.PlayerClan.MapFaction.IsAtWarWith(s.OwnerClan.MapFaction));
					this._targetSettlementForSiblings = ((town2 != null) ? town2.Settlement : null);
				}
				if (this._targetSettlementForSiblings == null)
				{
					this._targetSettlementForSiblings = SettlementHelper.FindRandomSettlement((Settlement s) => s.IsTown);
				}
			}

			// Token: 0x06000643 RID: 1603 RVA: 0x000227F8 File Offset: 0x000209F8
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
				CampaignEvents.IsSettlementBusyEvent.AddNonSerializedListener(this, new ReferenceAction<Settlement, object, int>(this.IsSettlementBusy));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.OnHideoutBattleCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, HideoutEventComponent, HideoutEventComponent.HideoutBattleEndState>(this.OnHideoutBattleCompleted));
				CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionStarted));
			}

			// Token: 0x06000644 RID: 1604 RVA: 0x000228BD File Offset: 0x00020ABD
			private void IsSettlementBusy(Settlement settlement, object asker, ref int priority)
			{
				if (asker != this && settlement == this._hideout)
				{
					priority = Math.Max(priority, 400);
				}
			}

			// Token: 0x06000645 RID: 1605 RVA: 0x000228DA File Offset: 0x00020ADA
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (mapEvent.IsHideoutBattle && mapEvent.MapEventSettlement == this._hideout && attackerParty == PartyBase.MainParty)
				{
					this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutBattleInProgress;
				}
			}

			// Token: 0x06000646 RID: 1606 RVA: 0x00022904 File Offset: 0x00020B04
			private void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent, HideoutEventComponent.HideoutBattleEndState battleEndState)
			{
				Settlement mapEventSettlement = hideoutEventComponent.MapEvent.MapEventSettlement;
				if (mapEventSettlement == this._hideout)
				{
					MobileParty lastAttackerParty = mapEventSettlement.LastAttackerParty;
					if (lastAttackerParty != null && lastAttackerParty.IsMainParty && this._rescueFamilyQuestState == RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutBattleInProgress)
					{
						if (battleEndState > HideoutEventComponent.HideoutBattleEndState.Defeated && battleEndState - HideoutEventComponent.HideoutBattleEndState.Victory <= 1)
						{
							CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, true, false, false, false, false), new ConversationCharacterData(StoryModeHeroes.RadagosHenchman.CharacterObject, null, true, true, false, false, false, false));
							return;
						}
						if (!this._hideoutBoss.IsHealthFull())
						{
							this._hideoutBoss.Heal(this._hideoutBoss.CharacterObject.MaxHitPoints(), false);
						}
						base.AddLog(this._defeatedQuestLogText, false);
						DisableHeroAction.Apply(this._radagos);
						if (Hero.MainHero.IsPrisoner && this._raiderParties.Contains(Hero.MainHero.PartyBelongedToAsPrisoner.MobileParty))
						{
							EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
							InformationManager.ShowInquiry(new InquiryData(new TextObject("{=FPhWhjq7}Defeated", null).ToString(), new TextObject("{=WN6aHR6m}You were defeated by the bandits in the hideout but you managed to escape. You need to wait a while before attacking again.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
						}
						if (this._hideout.Parties.Count == 0)
						{
							this.InitializeHideout();
						}
						this._hideout.Hideout.SetNextPossibleAttackTime(StoryModeData.StorylineQuestHideoutHiddenDuration);
					}
				}
			}

			// Token: 0x06000647 RID: 1607 RVA: 0x00022A74 File Offset: 0x00020C74
			private void OnMissionStarted(IMission mission)
			{
				if (Settlement.CurrentSettlement == this._hideout && PlayerEncounter.Current != null)
				{
					Mission mission2 = (Mission)mission;
					HideoutAmbushMissionController missionBehavior = mission2.GetMissionBehavior<HideoutAmbushMissionController>();
					if (missionBehavior != null)
					{
						missionBehavior.SetOverriddenHideoutBossCharacterObject(this._hideoutBoss.CharacterObject);
						return;
					}
					HideoutMissionController missionBehavior2 = mission2.GetMissionBehavior<HideoutMissionController>();
					if (missionBehavior2 != null)
					{
						missionBehavior2.SetOverriddenHideoutBossCharacterObject(this._hideoutBoss.CharacterObject);
						return;
					}
					Debug.FailedAssert("Hideout boss can not be set!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\StoryMode\\Quests\\PlayerClanQuests\\RescueFamilyQuestBehavior.cs", "OnMissionStarted", 574);
				}
			}

			// Token: 0x06000648 RID: 1608 RVA: 0x00022AED File Offset: 0x00020CED
			private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
			{
				if (killer == this._radagos && victim == this._hideoutBoss)
				{
					if (Campaign.Current.CurrentMenuContext != null)
					{
						Campaign.Current.CurrentMenuContext.SwitchToMenu("radagos_goodbye_menu");
						return;
					}
					GameMenu.ActivateGameMenu("radagos_goodbye_menu");
				}
			}

			// Token: 0x06000649 RID: 1609 RVA: 0x00022B2C File Offset: 0x00020D2C
			private void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (party.IsMainParty)
				{
					if (base.IsTrackEnabled && this._rescueFamilyQuestState > RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.None && !base.IsTracked(this._hideout))
					{
						base.AddTrackedObject(this._hideout);
					}
					if (settlement == this._hideout && PartyBase.MainParty.MemberRoster.Contains(this._radagos.CharacterObject))
					{
						PartyBase.MainParty.MemberRoster.RemoveTroop(this._radagos.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					}
				}
			}

			// Token: 0x0600064A RID: 1610 RVA: 0x00022BB8 File Offset: 0x00020DB8
			private void OnGameMenuOpened(MenuCallbackArgs args)
			{
				GameStateManager gameStateManager = GameStateManager.Current;
				if (((gameStateManager != null) ? gameStateManager.ActiveState : null) is MapState)
				{
					if (this._rescueFamilyQuestState < RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutTalkWithRadagosDone && Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == this._hideout)
					{
						CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, true, false, false, false, false), new ConversationCharacterData(StoryModeHeroes.Radagos.CharacterObject, null, true, true, false, false, false, false));
						return;
					}
					if (this._rescueFamilyQuestState == RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.GoodbyeTalkWithRadagosDone && args.MenuContext.GameMenu.StringId == "radagos_goodbye_menu")
					{
						GameMenu.ExitToLast();
						base.CompleteQuestWithSuccess();
					}
				}
			}

			// Token: 0x0600064B RID: 1611 RVA: 0x00022C58 File Offset: 0x00020E58
			private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
			{
				if (this._rescueFamilyQuestState >= RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutTalkWithRadagosDone && settlement == this._hideout && mobileParty != null && mobileParty.IsMainParty)
				{
					if (!PartyBase.MainParty.MemberRoster.Contains(this._radagos.CharacterObject))
					{
						if (this._radagos.HeroState != Hero.CharacterStates.Active)
						{
							this._radagos.ChangeState(Hero.CharacterStates.Active);
						}
						PartyBase.MainParty.MemberRoster.AddToCounts(this._radagos.CharacterObject, 1, false, 0, 0, true, -1);
					}
					this.AddRadagosHenchmanToHideout();
				}
			}

			// Token: 0x0600064C RID: 1612 RVA: 0x00022CDF File Offset: 0x00020EDF
			protected override void HourlyTick()
			{
				this.CheckIfHideoutIsReady();
			}

			// Token: 0x0600064D RID: 1613 RVA: 0x00022CE8 File Offset: 0x00020EE8
			protected override void SetDialogs()
			{
				Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 160).NpcLine(new TextObject("{=1yi00v5w}{PLAYER.NAME}! Good to see you. Believe it or not, I mean that. I've been looking for you...[if:convo_calm_friendly][ib:normal2]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.radagos_reunion_conversation_condition))
					.PlayerLine(new TextObject("{=pCNSEPEP}You escaped? Where's my brother? What happened?", null), null, null, null)
					.NpcLine(new TextObject("{=xknCpvcb}Calm down, now. I'll tell you everything.[ib:closed2][if:convo_grave]", null), null, null, null, null)
					.NpcLine(GameTexts.FindText("rescue_family_quest_radagos_conversation_line_1", null), null, null, null, null)
					.NpcLine(new TextObject("{=UpUqL368}What scum, eh? Even in this profession, double-crossing your comrades is frowned upon.", null), null, null, null, null)
					.NpcLine(new TextObject("{=bJjAqCxk}I escaped - one of his men, a little guiltier than the rest, cut my bonds when the others were sleeping - but I can't let a traitor live. So I decided to find you and offer you a deal.[if:convo_focused_voice][ib:hip]", null), null, null, null, null)
					.NpcLine(new TextObject("{=PlpNTQqf}I know where {HIDEOUT_BOSS.LINK} is now. If you agree, we can attack together and save your kin.", null), null, null, null, null)
					.NpcLine(new TextObject("{=mmQRCHUM}But in return, I will have the pleasure of killing that bastard. So what do you say?[if:convo_snide_voice][ib:confident2]", null), null, null, null, null)
					.PlayerLine(new TextObject("{=ypDmy5Rn}Uh, how can we possibly trust each other?", null), null, null, null)
					.NpcLine(new TextObject("{=VbJvL8yB}Oh you can't trust me. But you need me, and I figure you have enough men that you could easily slit my throat pretty quickly if I lead you into a trap. And I don't need to trust you - you're my vehicle of revenge, not my partner.[if:convo_grave]", null), null, null, null, null)
					.PlayerLine(new TextObject("{=ft6zzDrJ}I can live with that. Let's go.", null), null, null, null)
					.NpcLine(new TextObject("{=HT9hW29s}Splendid! But I have a few things to do. There is a hideout near this city. {HIDEOUT_BOSS.LINK} keeps your siblings there. I will join you right where the path leads up, just out of sight of their scouts.[if:convo_snide_voice][ib:hip]", null), null, null, null, null)
					.PlayerLine(new TextObject("{=GicEcLx2}See you there then. But, remember, if this is a trap or something, that will cost you your life.", null), null, null, null)
					.NpcLine(new TextObject("{=8b4Ndfep}Oh of course. I have no doubts on that score.[if:convo_nonchalant]", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.radagos_reunion_conversation_consequence))
					.CloseDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 160).NpcLine(new TextObject("{=rDuegB1L}You've finally arrived! I have a few things to say before we attack.[ib:confident2][if:convo_nonchalant]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.radagos_hideout_conversation_condition))
					.NpcLine(new TextObject("{=1T7p0O7B}We have to be clever. {HIDEOUT_BOSS.LINK} is a cunning fellow, in a low and base kind of way.[if:convo_normal]", null), null, null, null, null)
					.PlayerLine(new TextObject("{=a29lmPLd}I defeated you before. I know how your gang operates. Less talking, more raiding. C'mon...", null), null, null, null)
					.NpcLine(new TextObject("{=QbsDYITB}That you did, that you did. Lead on, then.[ib:closed2][if:convo_calm_friendly]", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.radagos_hideout_conversation_consequence))
					.CloseDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 160).NpcLine(new TextObject("{=PiKISvfu}{PLAYER.NAME}! I knew you'd come. Great Heaven. Damn, {?PLAYER.GENDER}sister{?}brother{\\?}, nothing can stop you! I love you, {?PLAYER.GENDER}sister{?}brother{\\?}.[if:convo_calm_friendly][ib:aggressive2]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.brother_hideout_conversation_condition))
					.PlayerLine(new TextObject("{=DIKPGwj1}So glad to see you safe. Is everyone okay?", null), null, null, null)
					.NpcLine(GameTexts.FindText("rescue_family_quest_brother_conversation_line_1", null), null, null, null, null)
					.NpcLine(GameTexts.FindText("rescue_family_quest_brother_conversation_line_2", null), null, null, null, null)
					.NpcLine(new TextObject("{=IC9Vg5MA}Meet me there later, when you're ready to tell me everything.[if:convo_normal][ib:normal2]", null), null, null, null, null)
					.PlayerLine(new TextObject("{=LrItHItu}Okay brother, be careful. Take care.", null), null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.brother_hideout_conversation_consequence;
					})
					.CloseDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000015).NpcLine(new TextObject("{=0I9siaQY}Bastards... You're the kin of my captives, right? I saw {RADAGOS.LINK} with you. You know he can't be trusted?[if:convo_confused_annoyed][ib:aggressive]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.bandit_hideout_boss_fight_start_on_condition))
					.PlayerLine(GameTexts.FindText("rescue_family_quest_galter_conversation_player_line_1", null), null, null, null)
					.NpcLine(new TextObject("{=heoCaRIr}Nah... There's no more talking. Kill me or I kill you, that's how this ends.[ib:warrior][if:convo_bared_teeth]", null), null, null, null, null)
					.NpcLine(new TextObject("{=2GeiKTlS}I'll do you the honor of duelling you, and my men will stand down if you win.[if:convo_predatory]", null), null, null, null, null)
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
				Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000015).NpcLine(new TextObject("{=G9iXmhGK}Look, we can still talk. I'll give you a pouch of silver.[ib:weary][if:convo_confused_voice]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.hideout_boss_prisoner_talk_condition))
					.PlayerLine(new TextObject("{=fM4eSVps}You said talking was a waste of time. You are {RADAGOS.NAME}'s property, now.", null), null, null, null)
					.Consequence(delegate
					{
						this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.ExecutionTalkWithGalterDone;
						this.hideout_boss_prisoner_talk_consequence();
					})
					.CloseDialog(), this);
				string text;
				Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 1000015).NpcLine(GameTexts.FindText("rescue_family_quest_radagos_goodbye_conversation_line_1", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.goodbye_conversation_with_radagos_condition))
					.GetOutputToken(out text)
					.NpcLine(new TextObject("{=C79Xxm1b}Don't let your conscience bother you about letting me go, by the way. I won't get back into slaving. Burned too many bridges with my old colleagues, you might say. I'll find some other way to earn my keep - mercenary work, perhaps. Anyway, maybe our paths will cross again.[if:convo_empathic_voice]", null), null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=c1Q2irLi}Your men killed my parents. Did you really think you would not be punished?", null), null, null, null)
					.NpcLine(new TextObject("{=W7hi7jS4}Eh, well, I dared to hope, I suppose. All right then, I'm not going to grovel to you, so get it over with.[ib:hip][if:convo_uncomfortable_voice]", null), null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=kz5PJbV1}I shall. For your many crimes, {RADAGOS.NAME}, your life is forfeit.", null), null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.execute_radagos_consequence;
					})
					.CloseDialog()
					.PlayerOption(GameTexts.FindText("rescue_family_quest_radagos_goodbye_conversation_player_line_1", null), null, null, null)
					.GotoDialogState(text)
					.EndPlayerOptions()
					.PlayerOption(new TextObject("{=RefpTQpr}Maybe. Goodbye, {RADAGOS.NAME}...", null), null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.let_go_radagos_consequence;
					})
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog(), this);
			}

			// Token: 0x0600064E RID: 1614 RVA: 0x00023210 File Offset: 0x00021410
			private bool radagos_reunion_conversation_condition()
			{
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
				StringHelpers.SetCharacterProperties("HIDEOUT_BOSS", this._hideoutBoss.CharacterObject, null, false);
				return this._rescueFamilyQuestState == RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.None && Hero.OneToOneConversationHero == this._radagos;
			}

			// Token: 0x0600064F RID: 1615 RVA: 0x0002325E File Offset: 0x0002145E
			private void radagos_reunion_conversation_consequence()
			{
				this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.ReunionTalkWithRadagosDone;
				base.AddLog(this._startQuestLogText, false);
			}

			// Token: 0x06000650 RID: 1616 RVA: 0x00023275 File Offset: 0x00021475
			private bool radagos_hideout_conversation_condition()
			{
				StringHelpers.SetCharacterProperties("HIDEOUT_BOSS", this._hideoutBoss.CharacterObject, null, false);
				return this._rescueFamilyQuestState < RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutTalkWithRadagosDone && Settlement.CurrentSettlement == this._hideout && Hero.OneToOneConversationHero == this._radagos;
			}

			// Token: 0x06000651 RID: 1617 RVA: 0x000232B4 File Offset: 0x000214B4
			private void radagos_hideout_conversation_consequence()
			{
				this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.HideoutTalkWithRadagosDone;
				if (!PartyBase.MainParty.MemberRoster.Contains(this._radagos.CharacterObject))
				{
					if (this._radagos.HeroState != Hero.CharacterStates.Active)
					{
						this._radagos.ChangeState(Hero.CharacterStates.Active);
					}
					PartyBase.MainParty.MemberRoster.AddToCounts(this._radagos.CharacterObject, 1, false, 0, 0, true, -1);
				}
				this.AddRadagosHenchmanToHideout();
			}

			// Token: 0x06000652 RID: 1618 RVA: 0x00023328 File Offset: 0x00021528
			private bool brother_hideout_conversation_condition()
			{
				if (this._rescueFamilyQuestState < RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.ReunionTalkWithBrotherDone && Hero.OneToOneConversationHero == StoryModeHeroes.ElderBrother)
				{
					this.SelectTargetSettlementForSiblings();
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
					StringHelpers.SetCharacterProperties("LITTLE_SISTER", StoryModeHeroes.LittleSister.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("LITTLE_BROTHER", StoryModeHeroes.LittleBrother.CharacterObject, null, false);
					MBTextManager.SetTextVariable("SETTLEMENT_LINK", this._targetSettlementForSiblings.EncyclopediaLinkWithName, false);
					Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
					{
						if (Campaign.Current.CurrentMenuContext != null)
						{
							Campaign.Current.CurrentMenuContext.SwitchToMenu("radagos_goodbye_menu");
							return;
						}
						GameMenu.ActivateGameMenu("radagos_goodbye_menu");
					};
					return true;
				}
				return false;
			}

			// Token: 0x06000653 RID: 1619 RVA: 0x000233DD File Offset: 0x000215DD
			private void brother_hideout_conversation_consequence()
			{
				this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.ReunionTalkWithBrotherDone;
			}

			// Token: 0x06000654 RID: 1620 RVA: 0x000233E8 File Offset: 0x000215E8
			private bool bandit_hideout_boss_fight_start_on_condition()
			{
				PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
				if (encounteredParty == null || encounteredParty.IsMobile || encounteredParty.MapFaction == null || !encounteredParty.MapFaction.IsBanditFaction)
				{
					return false;
				}
				StringHelpers.SetCharacterProperties("RADAGOS", this._radagos.CharacterObject, null, false);
				return encounteredParty.IsSettlement && encounteredParty.Settlement.IsHideout && encounteredParty.Settlement == this._hideout && Mission.Current != null && Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero == this._hideoutBoss && (Mission.Current.GetMissionBehavior<HideoutAmbushMissionController>() != null || Mission.Current.GetMissionBehavior<HideoutMissionController>() != null);
			}

			// Token: 0x06000655 RID: 1621 RVA: 0x00023490 File Offset: 0x00021690
			private void bandit_hideout_start_duel_fight_on_consequence()
			{
				if (Mission.Current.GetMissionBehavior<HideoutAmbushMissionController>() != null)
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutAmbushMissionController.StartBossFightDuelMode;
					return;
				}
				Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutMissionController.StartBossFightDuelMode;
			}

			// Token: 0x06000656 RID: 1622 RVA: 0x000234E0 File Offset: 0x000216E0
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

			// Token: 0x06000657 RID: 1623 RVA: 0x00023568 File Offset: 0x00021768
			private void bandit_hideout_continue_battle_on_consequence()
			{
				if (Mission.Current.GetMissionBehavior<HideoutAmbushMissionController>() != null)
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutAmbushMissionController.StartBossFightBattleMode;
					return;
				}
				Campaign.Current.ConversationManager.ConversationEndOneShot += HideoutMissionController.StartBossFightBattleMode;
			}

			// Token: 0x06000658 RID: 1624 RVA: 0x000235B8 File Offset: 0x000217B8
			private bool hideout_boss_prisoner_talk_condition()
			{
				StringHelpers.SetCharacterProperties("RADAGOS", this._radagos.CharacterObject, null, false);
				return Hero.OneToOneConversationHero == this._hideoutBoss;
			}

			// Token: 0x06000659 RID: 1625 RVA: 0x000235E0 File Offset: 0x000217E0
			private void hideout_boss_prisoner_talk_consequence()
			{
				MBInformationManager.ShowSceneNotification(HeroExecutionSceneNotificationData.CreateForInformingPlayer(this._radagos, this._hideoutBoss, CampaignTime.Now, SceneNotificationData.RelevantContextType.Map, new Action(this.OnGalterExecutionIsDone), false, false, false, false, null));
				Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
				{
					if (Campaign.Current.CurrentMenuContext != null)
					{
						Campaign.Current.CurrentMenuContext.SwitchToMenu("radagos_goodbye_menu");
						return;
					}
					GameMenu.ActivateGameMenu("radagos_goodbye_menu");
				};
			}

			// Token: 0x0600065A RID: 1626 RVA: 0x00023648 File Offset: 0x00021848
			private void OnGalterExecutionIsDone()
			{
				if (!this._hideoutBoss.IsDead)
				{
					KillCharacterAction.ApplyByExecution(this._hideoutBoss, this._radagos, true, true);
				}
				if (this._rescueFamilyQuestState == RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.ExecutionTalkWithGalterDone && !Campaign.Current.ConversationManager.IsConversationInProgress)
				{
					CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, true, false, false, false, false), new ConversationCharacterData(StoryModeHeroes.ElderBrother.CharacterObject, null, true, true, false, false, false, false));
				}
			}

			// Token: 0x0600065B RID: 1627 RVA: 0x000236BC File Offset: 0x000218BC
			private bool goodbye_conversation_with_radagos_condition()
			{
				if (this._rescueFamilyQuestState == RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.ReunionTalkWithBrotherDone && Hero.OneToOneConversationHero == this._radagos)
				{
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
					StringHelpers.SetCharacterProperties("RADAGOS", this._radagos.CharacterObject, null, false);
					return true;
				}
				return false;
			}

			// Token: 0x0600065C RID: 1628 RVA: 0x0002370C File Offset: 0x0002190C
			private void execute_radagos_consequence()
			{
				base.AddLog(this._executeRadagosEndQuestLogText, false);
				MBInformationManager.ShowSceneNotification(HeroExecutionSceneNotificationData.CreateForInformingPlayer(Hero.MainHero, this._radagos, CampaignTime.Now, SceneNotificationData.RelevantContextType.Map, new Action(this.OnRadagosExecutionIsDone), false, false, false, false, null));
				this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.GoodbyeTalkWithRadagosDone;
			}

			// Token: 0x0600065D RID: 1629 RVA: 0x0002375A File Offset: 0x0002195A
			private void OnRadagosExecutionIsDone()
			{
				if (!this._radagos.IsDead)
				{
					KillCharacterAction.ApplyByExecution(this._radagos, Hero.MainHero, true, true);
				}
			}

			// Token: 0x0600065E RID: 1630 RVA: 0x0002377B File Offset: 0x0002197B
			private void let_go_radagos_consequence()
			{
				base.AddLog(this._letGoRadagosEndQuestLogText, false);
				DisableHeroAction.Apply(this._radagos);
				this._rescueFamilyQuestState = RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum.GoodbyeTalkWithRadagosDone;
			}

			// Token: 0x0600065F RID: 1631 RVA: 0x000237A0 File Offset: 0x000219A0
			private void AddGameMenus()
			{
				TextObject textObject = new TextObject("{=kzgbBrYo}As you leave the hideout, {RADAGOS.LINK} comes to you and asks to talk.", null);
				StringHelpers.SetCharacterProperties("RADAGOS", this._radagos.CharacterObject, textObject, false);
				base.AddGameMenu("radagos_goodbye_menu", textObject, new OnInitDelegate(this.radagos_goodbye_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None);
				base.AddGameMenuOption("radagos_goodbye_menu", "radagos_goodbye_menu_continue", new TextObject("{=DM6luo3c}Continue", null), new GameMenuOption.OnConditionDelegate(this.radagos_goodbye_menu_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.radagos_goodbye_menu_continue_on_consequence), false, -1);
			}

			// Token: 0x06000660 RID: 1632 RVA: 0x00023820 File Offset: 0x00021A20
			private void radagos_goodbye_menu_on_init(MenuCallbackArgs args)
			{
			}

			// Token: 0x06000661 RID: 1633 RVA: 0x00023822 File Offset: 0x00021A22
			private bool radagos_goodbye_menu_continue_on_condition(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				return true;
			}

			// Token: 0x06000662 RID: 1634 RVA: 0x00023830 File Offset: 0x00021A30
			private void radagos_goodbye_menu_continue_on_consequence(MenuCallbackArgs args)
			{
				CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, true, false, false, false, false), new ConversationCharacterData(this._radagos.CharacterObject, null, true, true, false, false, false, false));
			}

			// Token: 0x06000663 RID: 1635 RVA: 0x0002386A File Offset: 0x00021A6A
			[GameMenuInitializationHandler("radagos_goodbye_menu")]
			private static void quest_game_menus_on_init_background(MenuCallbackArgs args)
			{
				args.MenuContext.SetBackgroundMeshName(SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, null).WaitMeshName);
			}

			// Token: 0x06000664 RID: 1636 RVA: 0x00023888 File Offset: 0x00021A88
			internal static void AutoGeneratedStaticCollectObjectsRescueFamilyQuest(object o, List<object> collectedObjects)
			{
				((RescueFamilyQuestBehavior.RescueFamilyQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06000665 RID: 1637 RVA: 0x00023896 File Offset: 0x00021A96
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._hideout);
				collectedObjects.Add(this._raiderParties);
			}

			// Token: 0x06000666 RID: 1638 RVA: 0x000238B7 File Offset: 0x00021AB7
			internal static object AutoGeneratedGetMemberValue_hideout(object o)
			{
				return ((RescueFamilyQuestBehavior.RescueFamilyQuest)o)._hideout;
			}

			// Token: 0x06000667 RID: 1639 RVA: 0x000238C4 File Offset: 0x00021AC4
			internal static object AutoGeneratedGetMemberValue_raiderParties(object o)
			{
				return ((RescueFamilyQuestBehavior.RescueFamilyQuest)o)._raiderParties;
			}

			// Token: 0x06000668 RID: 1640 RVA: 0x000238D1 File Offset: 0x00021AD1
			internal static object AutoGeneratedGetMemberValue_rescueFamilyQuestState(object o)
			{
				return ((RescueFamilyQuestBehavior.RescueFamilyQuest)o)._rescueFamilyQuestState;
			}

			// Token: 0x04000241 RID: 577
			private const int RaiderPartySize = 10;

			// Token: 0x04000242 RID: 578
			private const int RaiderPartyCount = 2;

			// Token: 0x04000243 RID: 579
			private const string RescueFamilyRaiderPartyStringId = "rescue_family_quest_raider_party_";

			// Token: 0x04000244 RID: 580
			private Hero _radagos;

			// Token: 0x04000245 RID: 581
			private Hero _hideoutBoss;

			// Token: 0x04000246 RID: 582
			private Settlement _targetSettlementForSiblings;

			// Token: 0x04000247 RID: 583
			[SaveableField(1)]
			private readonly Settlement _hideout;

			// Token: 0x04000248 RID: 584
			[SaveableField(7)]
			private readonly List<MobileParty> _raiderParties;

			// Token: 0x04000249 RID: 585
			[SaveableField(8)]
			private RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum _rescueFamilyQuestState;

			// Token: 0x0200009C RID: 156
			public class RebuildPlayerClanQuestBehaviorTypeDefiner : SaveableTypeDefiner
			{
				// Token: 0x060006F4 RID: 1780 RVA: 0x00024A11 File Offset: 0x00022C11
				public RebuildPlayerClanQuestBehaviorTypeDefiner()
					: base(4140000)
				{
				}

				// Token: 0x060006F5 RID: 1781 RVA: 0x00024A1E File Offset: 0x00022C1E
				protected override void DefineClassTypes()
				{
					base.AddClassDefinition(typeof(RescueFamilyQuestBehavior.RescueFamilyQuest), 1, null);
				}

				// Token: 0x060006F6 RID: 1782 RVA: 0x00024A32 File Offset: 0x00022C32
				protected override void DefineEnumTypes()
				{
					base.AddEnumDefinition(typeof(RescueFamilyQuestBehavior.RescueFamilyQuest.RescueFamilyQuestStateEnum), 11, null);
				}
			}

			// Token: 0x0200009D RID: 157
			private enum RescueFamilyQuestStateEnum
			{
				// Token: 0x040002C9 RID: 713
				None,
				// Token: 0x040002CA RID: 714
				ReunionTalkWithRadagosDone,
				// Token: 0x040002CB RID: 715
				HideoutTalkWithRadagosDone,
				// Token: 0x040002CC RID: 716
				HideoutBattleInProgress,
				// Token: 0x040002CD RID: 717
				ExecutionTalkWithGalterDone,
				// Token: 0x040002CE RID: 718
				ReunionTalkWithBrotherDone,
				// Token: 0x040002CF RID: 719
				GoodbyeTalkWithRadagosDone
			}
		}
	}
}
