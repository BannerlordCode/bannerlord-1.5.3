using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000375 RID: 885
	public class BettingFraudIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06003505 RID: 13573 RVA: 0x000D8F90 File Offset: 0x000D7190
		private static BettingFraudIssueBehavior.BettingFraudQuest Instance
		{
			get
			{
				BettingFraudIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<BettingFraudIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BettingFraudIssueBehavior.BettingFraudQuest bettingFraudQuest;
						if ((bettingFraudQuest = enumerator.Current as BettingFraudIssueBehavior.BettingFraudQuest) != null)
						{
							campaignBehavior._cachedQuest = bettingFraudQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000D9028 File Offset: 0x000D7228
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.CheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000D9058 File Offset: 0x000D7258
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu("menu_town_tournament_join_betting_fraud", "{=5Adr6toM}{MENU_TEXT}", new OnInitDelegate(this.game_menu_tournament_join_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenuOption("menu_town_tournament_join_betting_fraud", "mno_tournament_event_1", "{=es0Y3Bxc}Join", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Mission;
				args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveIssue;
				return true;
			}, new GameMenuOption.OnConsequenceDelegate(this.game_menu_tournament_join_current_game_on_consequence), false, -1, false, null);
			gameStarter.AddGameMenuOption("menu_town_tournament_join_betting_fraud", "mno_tournament_leave", "{=3sRdGQou}Leave", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("town_arena");
			}, true, -1, false, null);
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000D9120 File Offset: 0x000D7320
		private void game_menu_tournament_join_on_init(MenuCallbackArgs args)
		{
			TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
			tournamentGame.UpdateTournamentPrize(true, false);
			GameTexts.SetVariable("MENU_TEXT", tournamentGame.GetMenuText());
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000D9160 File Offset: 0x000D7360
		private void game_menu_tournament_join_current_game_on_consequence(MenuCallbackArgs args)
		{
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(BettingFraudIssueBehavior.Instance._thug, null, false, false, false, false, false, false));
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000D919C File Offset: 0x000D739C
		[GameMenuInitializationHandler("menu_town_tournament_join_betting_fraud")]
		private static void game_menu_ui_town_ui_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Town.WaitMeshName);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000D91C5 File Offset: 0x000D73C5
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000D91C8 File Offset: 0x000D73C8
		private void CheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(BettingFraudIssueBehavior.BettingFraudIssue), IssueBase.IssueFrequency.Rare, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(BettingFraudIssueBehavior.BettingFraudIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000D922C File Offset: 0x000D742C
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsGangLeader && issueGiver.CurrentSettlement != null && issueGiver.CurrentSettlement.Town != null && issueGiver.CurrentSettlement.Town.Security < 45f;
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000D9264 File Offset: 0x000D7464
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			return new BettingFraudIssueBehavior.BettingFraudIssue(issueOwner);
		}

		// Token: 0x04000EF0 RID: 3824
		private const IssueBase.IssueFrequency BettingFraudIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x04000EF1 RID: 3825
		private const string JoinTournamentMenuId = "menu_town_tournament_join";

		// Token: 0x04000EF2 RID: 3826
		private const string JoinTournamentForBettingFraudQuestMenuId = "menu_town_tournament_join_betting_fraud";

		// Token: 0x04000EF3 RID: 3827
		private const int SettlementSecurityLimit = 55;

		// Token: 0x04000EF4 RID: 3828
		private const int SettlementSecurityMin = 45;

		// Token: 0x04000EF5 RID: 3829
		private BettingFraudIssueBehavior.BettingFraudQuest _cachedQuest;

		// Token: 0x02000724 RID: 1828
		public class BettingFraudIssue : IssueBase
		{
			// Token: 0x0600586E RID: 22638 RVA: 0x001A1E64 File Offset: 0x001A0064
			internal static void AutoGeneratedStaticCollectObjectsBettingFraudIssue(object o, List<object> collectedObjects)
			{
				((BettingFraudIssueBehavior.BettingFraudIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600586F RID: 22639 RVA: 0x001A1E72 File Offset: 0x001A0072
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x17001060 RID: 4192
			// (get) Token: 0x06005870 RID: 22640 RVA: 0x001A1E7B File Offset: 0x001A007B
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=kru5Vpog}Yes. I'm glad to have the chance to talk to you. I keep an eye on the careers of champions like yourself for professional reasons, and I have a proposal that might interest a good fighter like you. Interested?[ib:confident3][if:convo_bemused]", null);
				}
			}

			// Token: 0x17001061 RID: 4193
			// (get) Token: 0x06005871 RID: 22641 RVA: 0x001A1E88 File Offset: 0x001A0088
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=YWXkgDSd}What kind of a partnership are we talking about?", null);
				}
			}

			// Token: 0x17001062 RID: 4194
			// (get) Token: 0x06005872 RID: 22642 RVA: 0x001A1E95 File Offset: 0x001A0095
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=vLaoZhkF}I follow tournaments, you see, and like to both place and take bets. But of course I need someone who can not only win those tournaments but lose if necessary... if you understand what I mean. Not all the time. That would be too obvious. Here's what I propose. We enter into a partnership for five tournaments. Don't bother memorizing which ones you win and which ones you lose. Before each fight, an associate of my mine will let you know how you should place. Follow my instructions and I promise you will be rewarded handsomely. What do you say?[if:convo_bemused][ib:demure2]", null);
				}
			}

			// Token: 0x17001063 RID: 4195
			// (get) Token: 0x06005873 RID: 22643 RVA: 0x001A1EA2 File Offset: 0x001A00A2
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=cL9BX7ph}As long as the payment is good, I agree.", null);
				}
			}

			// Token: 0x17001064 RID: 4196
			// (get) Token: 0x06005874 RID: 22644 RVA: 0x001A1EAF File Offset: 0x001A00AF
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001065 RID: 4197
			// (get) Token: 0x06005875 RID: 22645 RVA: 0x001A1EB2 File Offset: 0x001A00B2
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001066 RID: 4198
			// (get) Token: 0x06005876 RID: 22646 RVA: 0x001A1EB5 File Offset: 0x001A00B5
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=xhVrxgC4}Betting Fraud", null);
				}
			}

			// Token: 0x17001067 RID: 4199
			// (get) Token: 0x06005877 RID: 22647 RVA: 0x001A1EC2 File Offset: 0x001A00C2
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=3j8pV58L}{ISSUE_GIVER.NAME} offers you a deal to fix {TOURNAMENT_COUNT} tournaments and share the profit from the bet winnings.", null);
					textObject.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, false);
					textObject.SetTextVariable("TOURNAMENT_COUNT", 5);
					return textObject;
				}
			}

			// Token: 0x06005878 RID: 22648 RVA: 0x001A1EF3 File Offset: 0x001A00F3
			public BettingFraudIssue(Hero issueOwner)
				: base(issueOwner, CampaignTime.DaysFromNow(45f))
			{
			}

			// Token: 0x06005879 RID: 22649 RVA: 0x001A1F06 File Offset: 0x001A0106
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x0600587A RID: 22650 RVA: 0x001A1F1B File Offset: 0x001A011B
			protected override void OnGameLoad()
			{
			}

			// Token: 0x0600587B RID: 22651 RVA: 0x001A1F1D File Offset: 0x001A011D
			protected override void HourlyTick()
			{
			}

			// Token: 0x0600587C RID: 22652 RVA: 0x001A1F1F File Offset: 0x001A011F
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new BettingFraudIssueBehavior.BettingFraudQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(45f), 0);
			}

			// Token: 0x0600587D RID: 22653 RVA: 0x001A1F38 File Offset: 0x001A0138
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x0600587E RID: 22654 RVA: 0x001A1F3C File Offset: 0x001A013C
			protected override bool CanPlayerTakeQuestConditions(Hero issueOwner, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				relationHero = null;
				skill = null;
				requiredGold = 0;
				flag = IssueBase.PreconditionFlags.None;
				if (Clan.PlayerClan.Renown < 50f)
				{
					flag |= IssueBase.PreconditionFlags.Renown;
				}
				if (issueOwner.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueOwner;
				}
				if (Hero.MainHero.GetSkillValue(DefaultSkills.OneHanded) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.TwoHanded) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.Polearm) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.Bow) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.Crossbow) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.Throwing) < 50)
				{
					if (Hero.MainHero.GetSkillValue(DefaultSkills.OneHanded) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.OneHanded;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.TwoHanded) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.TwoHanded;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.Polearm) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.Polearm;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.Bow) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.Bow;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.Crossbow) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.Crossbow;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.Throwing) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.Throwing;
					}
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x0600587F RID: 22655 RVA: 0x001A20DC File Offset: 0x001A02DC
			public override bool IssueStayAliveConditions()
			{
				return base.IssueOwner.CurrentSettlement.Town.Security < 55f;
			}

			// Token: 0x06005880 RID: 22656 RVA: 0x001A20FA File Offset: 0x001A02FA
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001D47 RID: 7495
			private const int NeededTournamentCount = 5;

			// Token: 0x04001D48 RID: 7496
			private const int IssueDuration = 45;

			// Token: 0x04001D49 RID: 7497
			private const int MainHeroSkillLimit = 50;

			// Token: 0x04001D4A RID: 7498
			private const int MainClanRenownLimit = 50;

			// Token: 0x04001D4B RID: 7499
			private const int RelationLimitWithIssueOwner = -10;

			// Token: 0x04001D4C RID: 7500
			private const float IssueOwnerPowerPenaltyForIssueEffect = -0.2f;
		}

		// Token: 0x02000725 RID: 1829
		public class BettingFraudQuest : QuestBase
		{
			// Token: 0x06005881 RID: 22657 RVA: 0x001A20FC File Offset: 0x001A02FC
			internal static void AutoGeneratedStaticCollectObjectsBettingFraudQuest(object o, List<object> collectedObjects)
			{
				((BettingFraudIssueBehavior.BettingFraudQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005882 RID: 22658 RVA: 0x001A210A File Offset: 0x001A030A
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._thug);
				collectedObjects.Add(this._startLog);
				collectedObjects.Add(this._counterOfferNotable);
			}

			// Token: 0x06005883 RID: 22659 RVA: 0x001A2137 File Offset: 0x001A0337
			internal static object AutoGeneratedGetMemberValue_thug(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._thug;
			}

			// Token: 0x06005884 RID: 22660 RVA: 0x001A2144 File Offset: 0x001A0344
			internal static object AutoGeneratedGetMemberValue_startLog(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._startLog;
			}

			// Token: 0x06005885 RID: 22661 RVA: 0x001A2151 File Offset: 0x001A0351
			internal static object AutoGeneratedGetMemberValue_counterOfferNotable(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._counterOfferNotable;
			}

			// Token: 0x06005886 RID: 22662 RVA: 0x001A215E File Offset: 0x001A035E
			internal static object AutoGeneratedGetMemberValue_fixedTournamentCount(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._fixedTournamentCount;
			}

			// Token: 0x06005887 RID: 22663 RVA: 0x001A2170 File Offset: 0x001A0370
			internal static object AutoGeneratedGetMemberValue_minorOffensiveCount(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._minorOffensiveCount;
			}

			// Token: 0x06005888 RID: 22664 RVA: 0x001A2182 File Offset: 0x001A0382
			internal static object AutoGeneratedGetMemberValue_counterOfferConversationDone(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._counterOfferConversationDone;
			}

			// Token: 0x17001068 RID: 4200
			// (get) Token: 0x06005889 RID: 22665 RVA: 0x001A2194 File Offset: 0x001A0394
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=xhVrxgC4}Betting Fraud", null);
				}
			}

			// Token: 0x17001069 RID: 4201
			// (get) Token: 0x0600588A RID: 22666 RVA: 0x001A21A1 File Offset: 0x001A03A1
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700106A RID: 4202
			// (get) Token: 0x0600588B RID: 22667 RVA: 0x001A21A4 File Offset: 0x001A03A4
			private TextObject StartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=6rweIvZS}{QUEST_GIVER.LINK}, a gang leader from {SETTLEMENT} offers you to fix 5 tournaments together and share the profit.{newline}{?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to enter 5 tournaments and follow the instructions given by {?QUEST_GIVER.GENDER}her{?}his{\\?} associate.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700106B RID: 4203
			// (get) Token: 0x0600588C RID: 22668 RVA: 0x001A21E4 File Offset: 0x001A03E4
			private TextObject CurrentDirectiveLog
			{
				get
				{
					TextObject textObject = new TextObject("{=dnZekyZI}Directive from {QUEST_GIVER.LINK}: {DIRECTIVE}", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("DIRECTIVE", this.GetDirectiveText());
					return textObject;
				}
			}

			// Token: 0x1700106C RID: 4204
			// (get) Token: 0x0600588D RID: 22669 RVA: 0x001A221C File Offset: 0x001A041C
			private TextObject QuestFailedWithTimeOutLog
			{
				get
				{
					TextObject textObject = new TextObject("{=2brAaeFh}You failed to complete tournaments in time. {QUEST_GIVER.LINK} will certainly be disappointed.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x0600588E RID: 22670 RVA: 0x001A2250 File Offset: 0x001A0450
			public BettingFraudQuest(string questId, Hero questGiver, CampaignTime duration, int rewardGold)
				: base(questId, questGiver, duration, rewardGold)
			{
				this._counterOfferNotable = null;
				this._fixedTournamentCount = 0;
				this._minorOffensiveCount = 0;
				this._counterOfferAccepted = false;
				this._readyToStartTournament = false;
				this._startTournamentEndConversation = false;
				this._counterOfferConversationDone = false;
				this._currentDirective = BettingFraudIssueBehavior.BettingFraudQuest.Directives.None;
				this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.None;
				this._thug = MBObjectManager.Instance.GetObject<CharacterObject>((MBRandom.RandomFloat > 0.5f) ? "betting_fraud_thug_male" : "betting_fraud_thug_female");
				this._startLog = null;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x0600588F RID: 22671 RVA: 0x001A22E2 File Offset: 0x001A04E2
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x06005890 RID: 22672 RVA: 0x001A22EA File Offset: 0x001A04EA
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005891 RID: 22673 RVA: 0x001A22EC File Offset: 0x001A04EC
			private void SelectCounterOfferNotable(Settlement settlement)
			{
				this._counterOfferNotable = settlement.Notables.GetRandomElement<Hero>();
			}

			// Token: 0x06005892 RID: 22674 RVA: 0x001A22FF File Offset: 0x001A04FF
			private void IncreaseMinorOffensive()
			{
				this._minorOffensiveCount++;
				this._currentDirective = BettingFraudIssueBehavior.BettingFraudQuest.Directives.None;
				if (this._minorOffensiveCount >= 2)
				{
					this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SecondMinorOffense;
					return;
				}
				this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MinorOffense;
			}

			// Token: 0x06005893 RID: 22675 RVA: 0x001A232E File Offset: 0x001A052E
			private void IncreaseFixedTournamentCount()
			{
				this._fixedTournamentCount++;
				this._startLog.UpdateCurrentProgress(this._fixedTournamentCount);
				this._currentDirective = BettingFraudIssueBehavior.BettingFraudQuest.Directives.None;
				if (this._fixedTournamentCount >= 5)
				{
					this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.BigReward;
					return;
				}
				this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SmallReward;
			}

			// Token: 0x06005894 RID: 22676 RVA: 0x001A236E File Offset: 0x001A056E
			private void SetCurrentDirective()
			{
				this._currentDirective = ((MBRandom.RandomFloat <= 0.33f) ? BettingFraudIssueBehavior.BettingFraudQuest.Directives.LoseAt3RdRound : ((MBRandom.RandomFloat < 0.5f) ? BettingFraudIssueBehavior.BettingFraudQuest.Directives.LoseAt4ThRound : BettingFraudIssueBehavior.BettingFraudQuest.Directives.WinTheTournament));
				base.AddLog(this.CurrentDirectiveLog, false);
			}

			// Token: 0x06005895 RID: 22677 RVA: 0x001A23A4 File Offset: 0x001A05A4
			private void StartTournamentMission()
			{
				TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
				GameMenu.SwitchToMenu("town");
				tournamentGame.PrepareForTournamentGame(true);
				Campaign.Current.TournamentManager.OnPlayerJoinTournament(tournamentGame.GetType(), Settlement.CurrentSettlement);
			}

			// Token: 0x06005896 RID: 22678 RVA: 0x001A23F8 File Offset: 0x001A05F8
			protected override void RegisterEvents()
			{
				CampaignEvents.PlayerEliminatedFromTournament.AddNonSerializedListener(this, new Action<int, Town>(this.OnPlayerEliminatedFromTournament));
				CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinished));
				CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			}

			// Token: 0x06005897 RID: 22679 RVA: 0x001A244A File Offset: 0x001A064A
			private void OnPlayerEliminatedFromTournament(int round, Town town)
			{
				this._startTournamentEndConversation = true;
				if (round == (int)this._currentDirective)
				{
					this.IncreaseFixedTournamentCount();
					return;
				}
				if (round < (int)this._currentDirective)
				{
					this.IncreaseMinorOffensive();
					return;
				}
				if (round > (int)this._currentDirective)
				{
					this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MajorOffense;
				}
			}

			// Token: 0x06005898 RID: 22680 RVA: 0x001A2484 File Offset: 0x001A0684
			private void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
			{
				if (participants.Contains(CharacterObject.PlayerCharacter) && this._currentDirective != BettingFraudIssueBehavior.BettingFraudQuest.Directives.None)
				{
					this._startTournamentEndConversation = true;
					if (this._currentDirective == BettingFraudIssueBehavior.BettingFraudQuest.Directives.WinTheTournament)
					{
						if (winner == CharacterObject.PlayerCharacter)
						{
							this.IncreaseFixedTournamentCount();
							return;
						}
						this.IncreaseMinorOffensive();
						return;
					}
					else if (winner == CharacterObject.PlayerCharacter)
					{
						this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MajorOffense;
					}
				}
			}

			// Token: 0x06005899 RID: 22681 RVA: 0x001A24DC File Offset: 0x001A06DC
			private void OnGameMenuOpened(MenuCallbackArgs args)
			{
				if (args.MenuContext.GameMenu.StringId == "menu_town_tournament_join")
				{
					GameMenu.SwitchToMenu("menu_town_tournament_join_betting_fraud");
				}
				if (args.MenuContext.GameMenu.StringId == "menu_town_tournament_join_betting_fraud")
				{
					if (this._readyToStartTournament)
					{
						if (this._fixedTournamentCount == 4 && !this._counterOfferConversationDone && this._counterOfferNotable != null && this._currentDirective != BettingFraudIssueBehavior.BettingFraudQuest.Directives.WinTheTournament)
						{
							CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(this._counterOfferNotable.CharacterObject, null, false, false, false, false, false, false));
						}
						else
						{
							this.StartTournamentMission();
							this._readyToStartTournament = false;
						}
					}
					if (this._fixedTournamentCount == 4 && (this._counterOfferNotable == null || this._counterOfferNotable.CurrentSettlement != Settlement.CurrentSettlement))
					{
						this.SelectCounterOfferNotable(Settlement.CurrentSettlement);
					}
				}
				if (this._startTournamentEndConversation)
				{
					CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(this._thug, null, false, false, false, false, false, false));
				}
			}

			// Token: 0x0600589A RID: 22682 RVA: 0x001A25F2 File Offset: 0x001A07F2
			protected override void OnTimedOut()
			{
				base.OnTimedOut();
				this.PlayerDidNotCompleteTournaments();
			}

			// Token: 0x0600589B RID: 22683 RVA: 0x001A2600 File Offset: 0x001A0800
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = this.GetOfferDialogFlow();
				this.DiscussDialogFlow = this.GetDiscussDialogFlow();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogWithThugStart(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogWithThugEnd(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCounterOfferDialog(), this);
			}

			// Token: 0x0600589C RID: 22684 RVA: 0x001A2668 File Offset: 0x001A0868
			private DialogFlow GetOfferDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=sp52g5AQ}Very good, very good. Try to enter five tournaments over the next 45 days or so. Right before the fight you'll hear from my associate how far I want you to go in the rankings before you lose.[if:convo_delighted][ib:hip]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.NpcLine(new TextObject("{=ADIYnC4u}Now, I know you can't win every fight, so if you underperform once or twice, I'd understand. But if you lose every time, or worse, if you overperform, well, then I'll be a bit angry.[if:convo_nonchalant][ib:normal2]", null), null, null, null, null)
					.NpcLine(new TextObject("{=1hOPCf8I}But I'm sure you won't disappoint me. Enjoy your riches![if:convo_focused_happy][ib:confident]", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.OfferDialogFlowConsequence))
					.CloseDialog();
			}

			// Token: 0x0600589D RID: 22685 RVA: 0x001A26E4 File Offset: 0x001A08E4
			private void OfferDialogFlowConsequence()
			{
				base.StartQuest();
				this._startLog = base.AddDiscreteLog(this.StartLog, new TextObject("{=dLfWFa61}Fix 5 Tournaments", null), 0, 5, null, false);
			}

			// Token: 0x0600589E RID: 22686 RVA: 0x001A2710 File Offset: 0x001A0910
			private DialogFlow GetDiscussDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=!}{RESPONSE_TEXT}", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DiscussDialogCondition))
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=abLgPWzf}I will continue to honor our deal. Do not forget to do your end, that's all.", null), null, null, null)
					.BeginNpcOptions(null, false)
					.NpcOption(new TextObject("{=ZLPEsMUx}Well, there are tournament happening in {NEARBY_TOURNAMENTS_LIST} right now. You can go there and do the job. Your denars will be waiting for you.", null), new ConversationSentence.OnConditionDelegate(this.NpcTournamentLocationCondition), null, null, null, null)
					.CloseDialog()
					.NpcDefaultOption("{=sUfSCLQx}Sadly, I've heard no news of an upcoming tournament. I am sure one will be held before too long.")
					.CloseDialog()
					.EndNpcOptions()
					.CloseDialog()
					.PlayerOption(new TextObject("{=XUS5wNsD}I feel like I do all the job and you get your denars.", null), null, null, null)
					.BeginNpcOptions(null, false)
					.NpcOption(new TextObject("{=ZLPEsMUx}Well, there are tournament happening in {NEARBY_TOURNAMENTS_LIST} right now. You can go there and do the job. Your denars will be waiting for you.", null), new ConversationSentence.OnConditionDelegate(this.NpcTournamentLocationCondition), null, null, null, null)
					.CloseDialog()
					.NpcDefaultOption("{=sUfSCLQx}Sadly, I've heard no news of an upcoming tournament. I am sure one will be held before too long.")
					.CloseDialog()
					.EndNpcOptions()
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x0600589F RID: 22687 RVA: 0x001A2810 File Offset: 0x001A0A10
			private bool DiscussDialogCondition()
			{
				bool flag = Hero.OneToOneConversationHero == base.QuestGiver;
				if (flag)
				{
					if (this._minorOffensiveCount > 0)
					{
						MBTextManager.SetTextVariable("RESPONSE_TEXT", new TextObject("{=7SPwGYvf}I had expected better of you. But even the best can fail sometimes. Just make sure it does not happen again.[if:convo_bored][ib:closed2] ", null), false);
						return flag;
					}
					MBTextManager.SetTextVariable("RESPONSE_TEXT", new TextObject("{=vo0uhUsZ}I have high hopes for you, friend. Just follow my directives and we will be rich.[if:convo_relaxed_happy][ib:demure2]", null), false);
				}
				return flag;
			}

			// Token: 0x060058A0 RID: 22688 RVA: 0x001A2864 File Offset: 0x001A0A64
			private bool NpcTournamentLocationCondition()
			{
				List<Town> list = Town.AllTowns.Where<Town>((Town x) => Campaign.Current.TournamentManager.GetTournamentGame(x) != null && x != Settlement.CurrentSettlement.Town).ToList<Town>();
				list = list.OrderBy<Town, float>((Town x) => DistanceHelper.FindClosestDistanceFromSettlementToSettlement(x.Settlement, Settlement.CurrentSettlement, MobileParty.NavigationType.Default)).ToList<Town>();
				if (list.Count > 0)
				{
					MBTextManager.SetTextVariable("NEARBY_TOURNAMENTS_LIST", list[0].Name, false);
					return true;
				}
				return false;
			}

			// Token: 0x060058A1 RID: 22689 RVA: 0x001A28F0 File Offset: 0x001A0AF0
			private DialogFlow GetDialogWithThugStart()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=!}{GREETING_LINE}", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogWithThugStartCondition))
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=!}{POSITIVE_OPTION}", null), null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.PositiveOptionCondition))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.PositiveOptionConsequences))
					.CloseDialog()
					.PlayerOption(new TextObject("{=!}{NEGATIVE_OPTION}", null), null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.NegativeOptionCondition))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.NegativeOptionConsequence))
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060058A2 RID: 22690 RVA: 0x001A29B4 File Offset: 0x001A0BB4
			private bool DialogWithThugStartCondition()
			{
				bool flag = CharacterObject.OneToOneConversationCharacter == this._thug && !this._startTournamentEndConversation;
				if (flag)
				{
					this.SetCurrentDirective();
					if (this._fixedTournamentCount < 2)
					{
						TextObject textObject = new TextObject("{=xYu4yVRU}Hey there friend. So... You don't need to know my name, but suffice to say that we're both friends of {QUEST_GIVER.LINK}. Here's {?QUEST_GIVER.GENDER}her{?}his{\\?} message for you: {DIRECTIVE}.[ib:confident][if:convo_nonchalant]", null);
						textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
						textObject.SetTextVariable("DIRECTIVE", this.GetDirectiveText());
						MBTextManager.SetTextVariable("GREETING_LINE", textObject, false);
						return flag;
					}
					if (this._fixedTournamentCount < 4)
					{
						TextObject textObject2 = new TextObject("{=cQE9tQOy}My friend! Good to see you. You did very well in that last fight. People definitely won't be expecting you to \"{DIRECTIVE}\". What a surprise that would be. Well, I should not keep you from your tournament. You know what to do.[if:convo_happy][ib:closed2]", null);
						textObject2.SetTextVariable("DIRECTIVE", this.GetDirectiveText());
						MBTextManager.SetTextVariable("GREETING_LINE", textObject2, false);
						return flag;
					}
					TextObject textObject3 = new TextObject("{=RVLPQ4rm}My friend. I am almost sad that these meetings are going to come to an end. Well, a deal is a deal. I won't beat around the bush. Here's your final message: {DIRECTIVE}. I wish you luck, right up until the moment that you have to go down.[if:convo_mocking_teasing][ib:closed]", null);
					textObject3.SetTextVariable("DIRECTIVE", this.GetDirectiveText());
					MBTextManager.SetTextVariable("GREETING_LINE", textObject3, false);
				}
				return flag;
			}

			// Token: 0x060058A3 RID: 22691 RVA: 0x001A2A90 File Offset: 0x001A0C90
			private bool PositiveOptionCondition()
			{
				if (this._fixedTournamentCount < 2)
				{
					MBTextManager.SetTextVariable("POSITIVE_OPTION", new TextObject("{=PrUauabl}As long as the payment is as we talked, you got nothing to worry about.", null), false);
				}
				else if (this._fixedTournamentCount < 4)
				{
					MBTextManager.SetTextVariable("POSITIVE_OPTION", new TextObject("{=TKRsPVMU}Yes, I did. Be around when the tournament is over.", null), false);
				}
				else
				{
					MBTextManager.SetTextVariable("POSITIVE_OPTION", new TextObject("{=26XPQw2v}I will miss this little deal we had. See you at the end", null), false);
				}
				return true;
			}

			// Token: 0x060058A4 RID: 22692 RVA: 0x001A2AF6 File Offset: 0x001A0CF6
			private void PositiveOptionConsequences()
			{
				this._readyToStartTournament = true;
			}

			// Token: 0x060058A5 RID: 22693 RVA: 0x001A2AFF File Offset: 0x001A0CFF
			private bool NegativeOptionCondition()
			{
				bool flag = this._fixedTournamentCount >= 4;
				if (flag)
				{
					MBTextManager.SetTextVariable("NEGATIVE_OPTION", new TextObject("{=vapdvRQO}This deal was a mistake. We will not talk again after this last tournament.", null), false);
				}
				return flag;
			}

			// Token: 0x060058A6 RID: 22694 RVA: 0x001A2B26 File Offset: 0x001A0D26
			private void NegativeOptionConsequence()
			{
				this._readyToStartTournament = true;
			}

			// Token: 0x060058A7 RID: 22695 RVA: 0x001A2B30 File Offset: 0x001A0D30
			private TextObject GetDirectiveText()
			{
				if (this._currentDirective == BettingFraudIssueBehavior.BettingFraudQuest.Directives.LoseAt3RdRound)
				{
					return new TextObject("{=aHlcBLYB}Lose this tournament at 3rd round", null);
				}
				if (this._currentDirective == BettingFraudIssueBehavior.BettingFraudQuest.Directives.LoseAt4ThRound)
				{
					return new TextObject("{=hc1mnqOx}Lose this tournament at 4th round", null);
				}
				if (this._currentDirective == BettingFraudIssueBehavior.BettingFraudQuest.Directives.WinTheTournament)
				{
					return new TextObject("{=hl4pTsaO}Win this tournament", null);
				}
				return TextObject.GetEmpty();
			}

			// Token: 0x060058A8 RID: 22696 RVA: 0x001A2B84 File Offset: 0x001A0D84
			private DialogFlow GetDialogWithThugEnd()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=!}{GREETING_LINE}", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogWithThugEndCondition))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.DialogWithThugEndConsequence))
					.CloseDialog();
			}

			// Token: 0x060058A9 RID: 22697 RVA: 0x001A2BD8 File Offset: 0x001A0DD8
			private bool DialogWithThugEndCondition()
			{
				bool flag = CharacterObject.OneToOneConversationCharacter == this._thug && this._startTournamentEndConversation;
				if (flag)
				{
					TextObject textObject = TextObject.GetEmpty();
					switch (this._afterTournamentConversationState)
					{
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SmallReward:
						textObject = new TextObject("{=ZM8t4ZW2}We are very impressed, my friend. Here is the payment as promised. I hope we can continue this profitable partnership. See you at the next tournament.[if:convo_happy][ib:demure]", null);
						GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 250, false);
						break;
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.BigReward:
						textObject = new TextObject("{=9vOZWY25}What an exciting result! I will definitely miss these tournaments. Well, maybe after some time goes by and memories get a little hazy we can continue. Here is the last payment. Very well deserved.[if:convo_happy][ib:demure]", null);
						break;
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MinorOffense:
						textObject = new TextObject("{=d8bGHJnZ}This was not we were expecting. We lost some money. Well, Lady Fortune always casts her ballot too in these contests. But try to reassure us that this was her plan, and not yours, eh?[if:convo_grave][ib:closed2]", null);
						break;
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SecondMinorOffense:
						textObject = new TextObject("{=bNAG2t8S}Well, my friend, either you're playing us false or you're just not very good at this. Either way, {QUEST_GIVER.LINK} wishes to tell you that {?QUEST_GIVER.GENDER}her{?}his{\\?} association with you is over.[if:convo_predatory][ib:closed2]", null);
						textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
						break;
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MajorOffense:
						textObject = new TextObject("{=Lyqx3NYE}Well... What happened back there... That wasn't bad luck or incompetence. {QUEST_GIVER.LINK} trusted in you and {?QUEST_GIVER.GENDER}She{?}He{\\?} doesn't take well to betrayal.[if:convo_angry][ib:warrior]", null);
						textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
						break;
					default:
						Debug.FailedAssert("After tournament conversation state is not set!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Issues\\BettingFraudIssueBehavior.cs", "DialogWithThugEndCondition", 723);
						break;
					}
					MBTextManager.SetTextVariable("GREETING_LINE", textObject, false);
				}
				return flag;
			}

			// Token: 0x060058AA RID: 22698 RVA: 0x001A2CDC File Offset: 0x001A0EDC
			private void DialogWithThugEndConsequence()
			{
				this._startTournamentEndConversation = false;
				switch (this._afterTournamentConversationState)
				{
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SmallReward:
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MinorOffense:
					break;
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.BigReward:
					this.MainHeroSuccessfullyFixedTournaments();
					return;
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SecondMinorOffense:
					this.MainHeroFailToFixTournaments();
					return;
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MajorOffense:
					if (this._counterOfferAccepted)
					{
						this.MainHeroAcceptsCounterOffer();
						return;
					}
					this.MainHeroChooseNotToFixTournaments();
					break;
				default:
					return;
				}
			}

			// Token: 0x060058AB RID: 22699 RVA: 0x001A2D38 File Offset: 0x001A0F38
			private DialogFlow GetCounterOfferDialog()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=bUfBHSsz}Hold on a moment, friend. I need to talk to you.[ib:aggressive]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.CounterOfferConversationStartCondition))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.CounterOfferConversationStartConsequence))
					.PlayerLine(new TextObject("{=PZfR7hEK}What do you want? I have a tournament to prepare for.", null), null, null, null)
					.NpcLine(new TextObject("{=GN9F316V}Oh of course you do. {QUEST_GIVER.LINK}'s people have been running around placing bets - we know all about your arrangement, you see. And let me tell you something: as these arrangements go, {QUEST_GIVER.LINK} is getting you cheap. Do you want to see real money? Win this tournament and I will pay you what you're worth. And isn't it better to win than to lose?[if:convo_mocking_aristocratic][ib:confident2]", null), null, null, null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.AccusationCondition))
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=MacG8ikN}I will think about it.", null), null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.CounterOfferAcceptedConsequence))
					.CloseDialog()
					.PlayerOption(new TextObject("{=bT279pk9}I have no idea what you talking about. Be on your way, friend.", null), null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060058AC RID: 22700 RVA: 0x001A2E11 File Offset: 0x001A1011
			private bool CounterOfferConversationStartCondition()
			{
				return this._counterOfferNotable != null && CharacterObject.OneToOneConversationCharacter == this._counterOfferNotable.CharacterObject;
			}

			// Token: 0x060058AD RID: 22701 RVA: 0x001A2E2F File Offset: 0x001A102F
			private void CounterOfferConversationStartConsequence()
			{
				this._counterOfferConversationDone = true;
			}

			// Token: 0x060058AE RID: 22702 RVA: 0x001A2E38 File Offset: 0x001A1038
			private bool AccusationCondition()
			{
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
				return true;
			}

			// Token: 0x060058AF RID: 22703 RVA: 0x001A2E53 File Offset: 0x001A1053
			private void CounterOfferAcceptedConsequence()
			{
				this._counterOfferAccepted = true;
			}

			// Token: 0x060058B0 RID: 22704 RVA: 0x001A2E5C File Offset: 0x001A105C
			private void MainHeroSuccessfullyFixedTournaments()
			{
				TextObject textObject = new TextObject("{=aCA83avL}You have placed in the tournaments as {QUEST_GIVER.LINK} wished.", null);
				textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
				base.AddLog(textObject, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 2500, false);
				Clan.PlayerClan.AddRenown(2f, true);
				base.QuestGiver.AddPower(10f);
				base.QuestGiver.CurrentSettlement.Town.Security += -20f;
				this.RelationshipChangeWithQuestGiver = 5;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x060058B1 RID: 22705 RVA: 0x001A2EF4 File Offset: 0x001A10F4
			private void MainHeroFailToFixTournaments()
			{
				TextObject textObject = new TextObject("{=ETbToaZC}You have failed to place in the tournaments as {QUEST_GIVER.LINK} wished.", null);
				textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
				base.AddLog(textObject, false);
				base.QuestGiver.AddPower(-10f);
				base.QuestGiver.CurrentSettlement.Town.Security += 10f;
				this.RelationshipChangeWithQuestGiver = -5;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060058B2 RID: 22706 RVA: 0x001A2F70 File Offset: 0x001A1170
			private void MainHeroChooseNotToFixTournaments()
			{
				TextObject textObject = new TextObject("{=52smwnzz}You have chosen not to place in the tournaments as {QUEST_GIVER.LINK} wished.", null);
				textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
				base.AddLog(textObject, false);
				base.QuestGiver.AddPower(-15f);
				base.QuestGiver.CurrentSettlement.Town.Security += 15f;
				this.RelationshipChangeWithQuestGiver = -10;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060058B3 RID: 22707 RVA: 0x001A2FEC File Offset: 0x001A11EC
			private void MainHeroAcceptsCounterOffer()
			{
				TextObject textObject = new TextObject("{=nb0wqaGA}You have made a deal with {NOTABLE.LINK} to betray {QUEST_GIVER.LINK}.", null);
				textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
				textObject.SetCharacterProperties("NOTABLE", this._counterOfferNotable.CharacterObject, false);
				base.AddLog(textObject, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 4500, false);
				base.QuestGiver.AddPower(-15f);
				base.QuestGiver.CurrentSettlement.Town.Security += 15f;
				ChangeRelationAction.ApplyPlayerRelation(this._counterOfferNotable, 2, true, true);
				this.RelationshipChangeWithQuestGiver = -10;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060058B4 RID: 22708 RVA: 0x001A309B File Offset: 0x001A129B
			private void PlayerDidNotCompleteTournaments()
			{
				base.AddLog(this.QuestFailedWithTimeOutLog, false);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -5, true, true);
			}

			// Token: 0x04001D4D RID: 7501
			private const int TournamentFixCount = 5;

			// Token: 0x04001D4E RID: 7502
			private const int MinorOffensiveLimit = 2;

			// Token: 0x04001D4F RID: 7503
			private const int SmallReward = 250;

			// Token: 0x04001D50 RID: 7504
			private const int BigReward = 2500;

			// Token: 0x04001D51 RID: 7505
			private const int CounterOfferReward = 4500;

			// Token: 0x04001D52 RID: 7506
			private const string MaleThug = "betting_fraud_thug_male";

			// Token: 0x04001D53 RID: 7507
			private const string FemaleThug = "betting_fraud_thug_female";

			// Token: 0x04001D54 RID: 7508
			[SaveableField(100)]
			private JournalLog _startLog;

			// Token: 0x04001D55 RID: 7509
			[SaveableField(1)]
			private Hero _counterOfferNotable;

			// Token: 0x04001D56 RID: 7510
			[SaveableField(10)]
			internal readonly CharacterObject _thug;

			// Token: 0x04001D57 RID: 7511
			[SaveableField(20)]
			private int _fixedTournamentCount;

			// Token: 0x04001D58 RID: 7512
			[SaveableField(30)]
			private int _minorOffensiveCount;

			// Token: 0x04001D59 RID: 7513
			private BettingFraudIssueBehavior.BettingFraudQuest.Directives _currentDirective;

			// Token: 0x04001D5A RID: 7514
			private BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState _afterTournamentConversationState;

			// Token: 0x04001D5B RID: 7515
			private bool _counterOfferAccepted;

			// Token: 0x04001D5C RID: 7516
			private bool _readyToStartTournament;

			// Token: 0x04001D5D RID: 7517
			private bool _startTournamentEndConversation;

			// Token: 0x04001D5E RID: 7518
			[SaveableField(40)]
			private bool _counterOfferConversationDone;

			// Token: 0x0200091A RID: 2330
			private enum Directives
			{
				// Token: 0x04002708 RID: 9992
				None,
				// Token: 0x04002709 RID: 9993
				LoseAt3RdRound = 2,
				// Token: 0x0400270A RID: 9994
				LoseAt4ThRound,
				// Token: 0x0400270B RID: 9995
				WinTheTournament
			}

			// Token: 0x0200091B RID: 2331
			private enum AfterTournamentConversationState
			{
				// Token: 0x0400270D RID: 9997
				None,
				// Token: 0x0400270E RID: 9998
				SmallReward,
				// Token: 0x0400270F RID: 9999
				BigReward,
				// Token: 0x04002710 RID: 10000
				MinorOffense,
				// Token: 0x04002711 RID: 10001
				SecondMinorOffense,
				// Token: 0x04002712 RID: 10002
				MajorOffense
			}
		}

		// Token: 0x02000726 RID: 1830
		public class BettingFraudIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060058B6 RID: 22710 RVA: 0x001A30C9 File Offset: 0x001A12C9
			public BettingFraudIssueTypeDefiner()
				: base(600327)
			{
			}

			// Token: 0x060058B7 RID: 22711 RVA: 0x001A30D6 File Offset: 0x001A12D6
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(BettingFraudIssueBehavior.BettingFraudIssue), 1, null);
				base.AddClassDefinition(typeof(BettingFraudIssueBehavior.BettingFraudQuest), 2, null);
			}
		}
	}
}
