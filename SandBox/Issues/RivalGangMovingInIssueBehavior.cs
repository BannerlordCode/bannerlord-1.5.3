using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Missions.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace SandBox.Issues
{
	// Token: 0x020000BA RID: 186
	public class RivalGangMovingInIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000341E0 File Offset: 0x000323E0
		private static RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest Instance
		{
			get
			{
				RivalGangMovingInIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<RivalGangMovingInIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest rivalGangMovingInIssueQuest;
						if ((rivalGangMovingInIssueQuest = enumerator.Current as RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest) != null)
						{
							campaignBehavior._cachedQuest = rivalGangMovingInIssueQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00034278 File Offset: 0x00032478
		private void OnCheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(RivalGangMovingInIssueBehavior.RivalGangMovingInIssue), IssueBase.IssueFrequency.Common, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(RivalGangMovingInIssueBehavior.RivalGangMovingInIssue), IssueBase.IssueFrequency.Common));
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000342DC File Offset: 0x000324DC
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			Hero rivalGangLeader = this.GetRivalGangLeader(issueOwner);
			return new RivalGangMovingInIssueBehavior.RivalGangMovingInIssue(issueOwner, rivalGangLeader);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000342F8 File Offset: 0x000324F8
		private static void rival_gang_wait_duration_is_over_menu_on_init(MenuCallbackArgs args)
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			TextObject textObject = new TextObject("{=9Kr9pjGs}{QUEST_GIVER.LINK} has prepared {?QUEST_GIVER.GENDER}her{?}his{\\?} men and is waiting for you.", null);
			StringHelpers.SetCharacterProperties("QUEST_GIVER", RivalGangMovingInIssueBehavior.Instance.QuestGiver.CharacterObject, null, false);
			MBTextManager.SetTextVariable("MENU_TEXT", textObject, false);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00034344 File Offset: 0x00032544
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsGangLeader && issueGiver.CurrentSettlement != null && issueGiver.CurrentSettlement.IsTown && issueGiver.CurrentSettlement.Town.Security <= 60f && this.GetRivalGangLeader(issueGiver) != null;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00034394 File Offset: 0x00032594
		private void rival_gang_quest_wait_duration_is_over_yes_consequence(MenuCallbackArgs args)
		{
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, true, true, false, false, false, false), new ConversationCharacterData(RivalGangMovingInIssueBehavior.Instance.QuestGiver.CharacterObject, null, true, true, false, false, false, false));
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000343D4 File Offset: 0x000325D4
		private Hero GetRivalGangLeader(Hero issueOwner)
		{
			Hero hero = null;
			foreach (Hero hero2 in issueOwner.CurrentSettlement.Notables)
			{
				if (hero2 != issueOwner && hero2.IsGangLeader && hero2.CanHaveCampaignIssues())
				{
					hero = hero2;
					break;
				}
			}
			return hero;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00034440 File Offset: 0x00032640
		private bool rival_gang_quest_wait_duration_is_over_yes_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0003444B File Offset: 0x0003264B
		private bool rival_gang_quest_wait_duration_is_over_no_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00034456 File Offset: 0x00032656
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00034488 File Offset: 0x00032688
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu("rival_gang_quest_before_fight", "", new OnInitDelegate(RivalGangMovingInIssueBehavior.rival_gang_quest_before_fight_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenu("rival_gang_quest_after_fight", "", new OnInitDelegate(RivalGangMovingInIssueBehavior.rival_gang_quest_after_fight_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenu("rival_gang_quest_wait_duration_is_over", "{MENU_TEXT}", new OnInitDelegate(RivalGangMovingInIssueBehavior.rival_gang_wait_duration_is_over_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenuOption("rival_gang_quest_wait_duration_is_over", "rival_gang_quest_wait_duration_is_over_yes", "{=aka03VdU}Meet {?QUEST_GIVER.GENDER}her{?}him{\\?} now", new GameMenuOption.OnConditionDelegate(this.rival_gang_quest_wait_duration_is_over_yes_condition), new GameMenuOption.OnConsequenceDelegate(this.rival_gang_quest_wait_duration_is_over_yes_consequence), false, -1, false, null);
			gameStarter.AddGameMenuOption("rival_gang_quest_wait_duration_is_over", "rival_gang_quest_wait_duration_is_over_no", "{=NIzQb6nT}Leave and meet {?QUEST_GIVER.GENDER}her{?}him{\\?} later", new GameMenuOption.OnConditionDelegate(this.rival_gang_quest_wait_duration_is_over_no_condition), new GameMenuOption.OnConsequenceDelegate(this.rival_gang_quest_wait_duration_is_over_no_consequence), true, -1, false, null);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00034554 File Offset: 0x00032754
		private void rival_gang_quest_wait_duration_is_over_no_consequence(MenuCallbackArgs args)
		{
			Campaign.Current.CurrentMenuContext.SwitchToMenu("town_wait_menus");
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0003456A File Offset: 0x0003276A
		private static void rival_gang_quest_before_fight_init(MenuCallbackArgs args)
		{
			if (RivalGangMovingInIssueBehavior.Instance != null && RivalGangMovingInIssueBehavior.Instance._isFinalStage)
			{
				RivalGangMovingInIssueBehavior.Instance.StartAlleyBattle();
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0003458C File Offset: 0x0003278C
		private static void rival_gang_quest_after_fight_init(MenuCallbackArgs args)
		{
			if (RivalGangMovingInIssueBehavior.Instance != null && RivalGangMovingInIssueBehavior.Instance._isReadyToBeFinalized)
			{
				bool flag = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Battle.PlayerSide;
				PlayerEncounter.Current.FinalizeBattle();
				RivalGangMovingInIssueBehavior.Instance.HandlePlayerEncounterResult(flag);
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x000345D8 File Offset: 0x000327D8
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000345DC File Offset: 0x000327DC
		[GameMenuInitializationHandler("rival_gang_quest_after_fight")]
		[GameMenuInitializationHandler("rival_gang_quest_wait_duration_is_over")]
		private static void game_menu_rival_gang_quest_end_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement != null)
			{
				args.MenuContext.SetBackgroundMeshName(currentSettlement.SettlementComponent.WaitMeshName);
			}
		}

		// Token: 0x0400041B RID: 1051
		private const IssueBase.IssueFrequency RivalGangLeaderIssueFrequency = IssueBase.IssueFrequency.Common;

		// Token: 0x0400041C RID: 1052
		private RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest _cachedQuest;

		// Token: 0x020001D8 RID: 472
		public class RivalGangMovingInIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060011B0 RID: 4528 RVA: 0x00071BF2 File Offset: 0x0006FDF2
			public RivalGangMovingInIssueTypeDefiner()
				: base(310000)
			{
			}

			// Token: 0x060011B1 RID: 4529 RVA: 0x00071BFF File Offset: 0x0006FDFF
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(RivalGangMovingInIssueBehavior.RivalGangMovingInIssue), 1, null);
				base.AddClassDefinition(typeof(RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest), 2, null);
			}
		}

		// Token: 0x020001D9 RID: 473
		public class RivalGangMovingInIssue : IssueBase
		{
			// Token: 0x170001BC RID: 444
			// (get) Token: 0x060011B2 RID: 4530 RVA: 0x00071C25 File Offset: 0x0006FE25
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x170001BD RID: 445
			// (get) Token: 0x060011B3 RID: 4531 RVA: 0x00071C29 File Offset: 0x0006FE29
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=pzvQ1DkE}Your companion has defeated the rival gang and protected the interests of {QUEST_GIVER.LINK} in {SETTLEMENT}.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueOwner.CurrentSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170001BE RID: 446
			// (get) Token: 0x060011B4 RID: 4532 RVA: 0x00071C69 File Offset: 0x0006FE69
			// (set) Token: 0x060011B5 RID: 4533 RVA: 0x00071C71 File Offset: 0x0006FE71
			[SaveableProperty(207)]
			public Hero RivalGangLeader { get; private set; }

			// Token: 0x170001BF RID: 447
			// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00071C7A File Offset: 0x0006FE7A
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 4 + MathF.Ceiling(6f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001C0 RID: 448
			// (get) Token: 0x060011B7 RID: 4535 RVA: 0x00071C8F File Offset: 0x0006FE8F
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 3 + MathF.Ceiling(5f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001C1 RID: 449
			// (get) Token: 0x060011B8 RID: 4536 RVA: 0x00071CA4 File Offset: 0x0006FEA4
			protected override int RewardGold
			{
				get
				{
					return (int)(600f + 1700f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001C2 RID: 450
			// (get) Token: 0x060011B9 RID: 4537 RVA: 0x00071CB9 File Offset: 0x0006FEB9
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(750f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001C3 RID: 451
			// (get) Token: 0x060011BA RID: 4538 RVA: 0x00071CD0 File Offset: 0x0006FED0
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=GXk6f9ah}I've got a problem... [ib:confident][if:convo_undecided_closed]And {?TARGET_NOTABLE.GENDER}her{?}his{\\?} name is {TARGET_NOTABLE.LINK}. {?TARGET_NOTABLE.GENDER}Her{?}His{\\?} people have been coming around outside the walls, robbing the dice-players and the drinkers enjoying themselves under our protection. Me and my boys are eager to teach them a lesson but I figure some extra muscle wouldn't hurt.", null);
					if (base.IssueOwner.RandomInt(2) == 0)
					{
						textObject = new TextObject("{=rgTGzfzI}Yeah. I have a problem all right. [ib:confident][if:convo_undecided_closed]{?TARGET_NOTABLE.GENDER}Her{?}His{\\?} name is {TARGET_NOTABLE.LINK}. {?TARGET_NOTABLE.GENDER}Her{?}His{\\?} people have been bothering shop owners under our protection, demanding money and making threats. Let me tell you something - those shop owners are my cows, and no one else gets to milk them. We're ready to teach these interlopers a lesson, but I could use some help.", null);
					}
					if (this.RivalGangLeader != null)
					{
						StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this.RivalGangLeader.CharacterObject, textObject, false);
					}
					return textObject;
				}
			}

			// Token: 0x170001C4 RID: 452
			// (get) Token: 0x060011BB RID: 4539 RVA: 0x00071D24 File Offset: 0x0006FF24
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=kc6vCycY}What exactly do you want me to do?", null);
				}
			}

			// Token: 0x170001C5 RID: 453
			// (get) Token: 0x060011BC RID: 4540 RVA: 0x00071D31 File Offset: 0x0006FF31
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=tyyAfWRR}We already had a small scuffle with them recently. [if:convo_mocking_revenge]They'll be waiting for us to come down hard. Instead, we'll hold off for {NUMBER} days. Let them think that we're backing off… Then, after {NUMBER} days, your men and mine will hit them in the middle of the night when they least expect it. I'll send you a messenger when the time comes and we'll strike them down together.", null);
					textObject.SetTextVariable("NUMBER", 2);
					return textObject;
				}
			}

			// Token: 0x170001C6 RID: 454
			// (get) Token: 0x060011BD RID: 4541 RVA: 0x00071D4B File Offset: 0x0006FF4B
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=sSIjPCPO}If you'd rather not go into the fray yourself, [if:convo_mocking_aristocratic]you can leave me one of your companions together with {TROOP_COUNT} or so good men. If they stuck around for {RETURN_DAYS} days or so, I'd count it a very big favor.", null);
					textObject.SetTextVariable("TROOP_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170001C7 RID: 455
			// (get) Token: 0x060011BE RID: 4542 RVA: 0x00071D7C File Offset: 0x0006FF7C
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=ymbVPod1}{ISSUE_GIVER.LINK}, a gang leader from {SETTLEMENT}, has told you about a new gang that is trying to get a hold on the town. You asked {COMPANION.LINK} to take {TROOP_COUNT} of your best men to stay with {ISSUE_GIVER.LINK} and help {?ISSUE_GIVER.GENDER}her{?}him{\\?} in the coming gang war. They should return to you in {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("TROOP_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170001C8 RID: 456
			// (get) Token: 0x060011BF RID: 4543 RVA: 0x00071E0D File Offset: 0x0007000D
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=LdCte9H0}I'll fight the other gang with you myself.", null);
				}
			}

			// Token: 0x170001C9 RID: 457
			// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00071E1A File Offset: 0x0007001A
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=AdbiUqtT}I'm busy, but I will leave a companion and some men.", null);
				}
			}

			// Token: 0x170001CA RID: 458
			// (get) Token: 0x060011C1 RID: 4545 RVA: 0x00071E27 File Offset: 0x00070027
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=0enbhess}Thank you. [ib:normal][if:convo_approving]I'm sure your guys are worth their salt..", null);
				}
			}

			// Token: 0x170001CB RID: 459
			// (get) Token: 0x060011C2 RID: 4546 RVA: 0x00071E34 File Offset: 0x00070034
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=QR0V8Ae5}Our lads are well hidden nearby,[ib:normal][if:convo_excited] waiting for the signal to go get those bastards. I won't forget this little favor you're doing me.", null);
				}
			}

			// Token: 0x170001CC RID: 460
			// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00071E41 File Offset: 0x00070041
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170001CD RID: 461
			// (get) Token: 0x060011C4 RID: 4548 RVA: 0x00071E44 File Offset: 0x00070044
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170001CE RID: 462
			// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00071E47 File Offset: 0x00070047
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=vAjgn7yx}Rival Gang Moving in at {SETTLEMENT}", null);
					string text = "SETTLEMENT";
					Settlement issueSettlement = base.IssueSettlement;
					textObject.SetTextVariable(text, ((issueSettlement != null) ? issueSettlement.Name : null) ?? base.IssueOwner.HomeSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170001CF RID: 463
			// (get) Token: 0x060011C6 RID: 4550 RVA: 0x00071E86 File Offset: 0x00070086
			public override TextObject Description
			{
				get
				{
					return new TextObject("{=H4EVfKAh}Gang leader needs help to beat the rival gang.", null);
				}
			}

			// Token: 0x170001D0 RID: 464
			// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00071E94 File Offset: 0x00070094
			public override TextObject IssueAsRumorInSettlement
			{
				get
				{
					TextObject textObject = new TextObject("{=C9feTaca}I hear {QUEST_GIVER.LINK} is going to sort it out with {RIVAL_GANG_LEADER.LINK} once and for all.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("RIVAL_GANG_LEADER", this.RivalGangLeader.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001D1 RID: 465
			// (get) Token: 0x060011C8 RID: 4552 RVA: 0x00071EDE File Offset: 0x000700DE
			protected override bool IssueQuestCanBeDuplicated
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060011C9 RID: 4553 RVA: 0x00071EE1 File Offset: 0x000700E1
			public RivalGangMovingInIssue(Hero issueOwner, Hero rivalGangLeader)
				: base(issueOwner, CampaignTime.DaysFromNow(15f))
			{
				this.RivalGangLeader = rivalGangLeader;
			}

			// Token: 0x060011CA RID: 4554 RVA: 0x00071EFB File Offset: 0x000700FB
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this.RivalGangLeader)
				{
					result = false;
				}
			}

			// Token: 0x060011CB RID: 4555 RVA: 0x00071F09 File Offset: 0x00070109
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -0.5f;
				}
				return 0f;
			}

			// Token: 0x060011CC RID: 4556 RVA: 0x00071F2C File Offset: 0x0007012C
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.RelationshipChangeWithIssueOwner = 5;
				ChangeRelationAction.ApplyPlayerRelation(this.RivalGangLeader, -5, true, true);
				base.IssueOwner.AddPower(10f);
				this.RivalGangLeader.AddPower(-10f);
			}

			// Token: 0x060011CD RID: 4557 RVA: 0x00071F64 File Offset: 0x00070164
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -5;
				base.IssueSettlement.Town.Security += -10f;
				base.IssueOwner.AddPower(-10f);
			}

			// Token: 0x060011CE RID: 4558 RVA: 0x00071F9C File Offset: 0x0007019C
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				int skillValue = hero.GetSkillValue(DefaultSkills.OneHanded);
				int skillValue2 = hero.GetSkillValue(DefaultSkills.TwoHanded);
				int skillValue3 = hero.GetSkillValue(DefaultSkills.Polearm);
				int skillValue4 = hero.GetSkillValue(DefaultSkills.Roguery);
				if (skillValue >= skillValue2 && skillValue >= skillValue3 && skillValue >= skillValue4)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.OneHanded, 150);
				}
				if (skillValue2 >= skillValue3 && skillValue2 >= skillValue4)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.TwoHanded, 150);
				}
				if (skillValue3 < skillValue4)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.Roguery, 120);
				}
				return new ValueTuple<SkillObject, int>(DefaultSkills.Polearm, 150);
			}

			// Token: 0x060011CF RID: 4559 RVA: 0x0007202D File Offset: 0x0007022D
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x060011D0 RID: 4560 RVA: 0x00072047 File Offset: 0x00070247
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x060011D1 RID: 4561 RVA: 0x00072058 File Offset: 0x00070258
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x060011D2 RID: 4562 RVA: 0x00072066 File Offset: 0x00070266
			protected override void OnGameLoad()
			{
			}

			// Token: 0x060011D3 RID: 4563 RVA: 0x00072068 File Offset: 0x00070268
			protected override void HourlyTick()
			{
			}

			// Token: 0x060011D4 RID: 4564 RVA: 0x0007206A File Offset: 0x0007026A
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest(questId, base.IssueOwner, this.RivalGangLeader, 8, this.RewardGold, base.IssueDifficultyMultiplier);
			}

			// Token: 0x060011D5 RID: 4565 RVA: 0x0007208B File Offset: 0x0007028B
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Common;
			}

			// Token: 0x060011D6 RID: 4566 RVA: 0x00072090 File Offset: 0x00070290
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				flag = IssueBase.PreconditionFlags.None;
				relationHero = null;
				requiredGold = 0;
				skill = null;
				if (Hero.MainHero.IsWounded)
				{
					flag |= IssueBase.PreconditionFlags.Wounded;
				}
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					flag |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (base.IssueOwner.CurrentSettlement.OwnerClan == Clan.PlayerClan)
				{
					flag |= IssueBase.PreconditionFlags.PlayerIsOwnerOfSettlement;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x060011D7 RID: 4567 RVA: 0x00072118 File Offset: 0x00070318
			public override bool IssueStayAliveConditions()
			{
				return this.RivalGangLeader.IsAlive && base.IssueOwner.CurrentSettlement.OwnerClan != Clan.PlayerClan && base.IssueOwner.CurrentSettlement.Town.Security <= 80f;
			}

			// Token: 0x060011D8 RID: 4568 RVA: 0x0007216A File Offset: 0x0007036A
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x060011D9 RID: 4569 RVA: 0x0007216C File Offset: 0x0007036C
			internal static void AutoGeneratedStaticCollectObjectsRivalGangMovingInIssue(object o, List<object> collectedObjects)
			{
				((RivalGangMovingInIssueBehavior.RivalGangMovingInIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060011DA RID: 4570 RVA: 0x0007217A File Offset: 0x0007037A
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this.RivalGangLeader);
			}

			// Token: 0x060011DB RID: 4571 RVA: 0x0007218F File Offset: 0x0007038F
			internal static object AutoGeneratedGetMemberValueRivalGangLeader(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssue)o).RivalGangLeader;
			}

			// Token: 0x04000878 RID: 2168
			private const int AlternativeSolutionRelationChange = 5;

			// Token: 0x04000879 RID: 2169
			private const int AlternativeSolutionFailRelationChange = -5;

			// Token: 0x0400087A RID: 2170
			private const int AlternativeSolutionQuestGiverPowerChange = 10;

			// Token: 0x0400087B RID: 2171
			private const int AlternativeSolutionRivalGangLeaderPowerChange = -10;

			// Token: 0x0400087C RID: 2172
			private const int AlternativeSolutionFailQuestGiverPowerChange = -10;

			// Token: 0x0400087D RID: 2173
			private const int AlternativeSolutionFailSecurityChange = -10;

			// Token: 0x0400087E RID: 2174
			private const int AlternativeSolutionRivalGangLeaderRelationChange = -5;

			// Token: 0x0400087F RID: 2175
			private const int AlternativeSolutionMinimumTroopTier = 2;

			// Token: 0x04000880 RID: 2176
			private const int IssueDuration = 15;

			// Token: 0x04000881 RID: 2177
			private const int MinimumRequiredMenCount = 5;

			// Token: 0x04000882 RID: 2178
			private const int IssueQuestDuration = 8;

			// Token: 0x04000883 RID: 2179
			private const int MeleeSkillValueThreshold = 150;

			// Token: 0x04000884 RID: 2180
			private const int RoguerySkillValueThreshold = 120;

			// Token: 0x04000885 RID: 2181
			private const int PreparationDurationInDays = 2;
		}

		// Token: 0x020001DA RID: 474
		public class RivalGangMovingInIssueQuest : QuestBase
		{
			// Token: 0x170001D2 RID: 466
			// (get) Token: 0x060011DC RID: 4572 RVA: 0x0007219C File Offset: 0x0007039C
			private TextObject OnQuestStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=dav5rmDd}{QUEST_GIVER.LINK}, a gang leader from {SETTLEMENT} has told you about a rival that is trying to get a foothold in {?QUEST_GIVER.GENDER}her{?}his{\\?} town. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to wait {DAY_COUNT} days so that the other gang lets its guard down.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("DAY_COUNT", 2);
					return textObject;
				}
			}

			// Token: 0x170001D3 RID: 467
			// (get) Token: 0x060011DD RID: 4573 RVA: 0x000721F4 File Offset: 0x000703F4
			private TextObject OnQuestFailedWithRejectionLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=aXMg9M7t}You didn't respond to the messenger {QUEST_GIVER.LINK} sent you. {?QUEST_GIVER.GENDER}She{?}He{\\?} will certainly lose to the rival gang without your help.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001D4 RID: 468
			// (get) Token: 0x060011DE RID: 4574 RVA: 0x00072228 File Offset: 0x00070428
			private TextObject OnQuestFailedWithBetrayalLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=Rf0QqRIX}You have chosen to side with the rival gang leader, {RIVAL_GANG_LEADER.LINK}. {QUEST_GIVER.LINK} must be furious.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("RIVAL_GANG_LEADER", this._rivalGangLeader.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001D5 RID: 469
			// (get) Token: 0x060011DF RID: 4575 RVA: 0x00072274 File Offset: 0x00070474
			private TextObject OnQuestFailedWithDefeatLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=du3dpMaV}You were unable to defeat {RIVAL_GANG_LEADER.LINK}'s gang, and thus failed to fulfill your commitment to {QUEST_GIVER.LINK}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("RIVAL_GANG_LEADER", this._rivalGangLeader.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001D6 RID: 470
			// (get) Token: 0x060011E0 RID: 4576 RVA: 0x000722C0 File Offset: 0x000704C0
			private TextObject OnQuestSucceededLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=vpUl7xcy}You have defeated the rival gang and protected the interests of {QUEST_GIVER.LINK} in {SETTLEMENT}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001D7 RID: 471
			// (get) Token: 0x060011E1 RID: 4577 RVA: 0x0007230C File Offset: 0x0007050C
			private TextObject OnQuestPreperationsCompletedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=OIBiRTRP}{QUEST_GIVER.LINK} is waiting for you at {SETTLEMENT}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001D8 RID: 472
			// (get) Token: 0x060011E2 RID: 4578 RVA: 0x00072358 File Offset: 0x00070558
			private TextObject OnQuestCancelledDueToWarLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=vaUlAZba}Your clan is now at war with {QUEST_GIVER.LINK}. Your agreement with {QUEST_GIVER.LINK} was canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001D9 RID: 473
			// (get) Token: 0x060011E3 RID: 4579 RVA: 0x0007238C File Offset: 0x0007058C
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001DA RID: 474
			// (get) Token: 0x060011E4 RID: 4580 RVA: 0x000723C0 File Offset: 0x000705C0
			private TextObject OnQuestCancelledDueToSiegeLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=s1GWSE9Y}{QUEST_GIVER.LINK} cancels your plans due to the siege of {SETTLEMENT}. {?QUEST_GIVER.GENDER}She{?}He{\\?} has worse troubles than {?QUEST_GIVER.GENDER}her{?}his{\\?} quarrel with the rival gang.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001DB RID: 475
			// (get) Token: 0x060011E5 RID: 4581 RVA: 0x0007240C File Offset: 0x0007060C
			private TextObject PlayerStartedAlleyFightWithRivalGangLeader
			{
				get
				{
					TextObject textObject = new TextObject("{=OeKgpuAv}After your attack on the rival gang's alley, {QUEST_GIVER.LINK} decided to change {?QUEST_GIVER.GENDER}her{?}his{\\?} plans, and doesn't need your assistance anymore. Quest is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001DC RID: 476
			// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00072440 File Offset: 0x00070640
			private TextObject PlayerStartedAlleyFightWithQuestgiver
			{
				get
				{
					TextObject textObject = new TextObject("{=VPGkIqlh}Your attack on {QUEST_GIVER.LINK}'s gang has angered {?QUEST_GIVER.GENDER}her{?}him{\\?} and {?QUEST_GIVER.GENDER}she{?}he{\\?} broke off the agreement that you had.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001DD RID: 477
			// (get) Token: 0x060011E7 RID: 4583 RVA: 0x00072474 File Offset: 0x00070674
			private TextObject OwnerOfQuestSettlementIsPlayerClanLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=KxEnNEoD}Your clan is now owner of the settlement. As the {?PLAYER.GENDER}lady{?}lord{\\?} of the settlement you cannot get involved in gang wars anymore. Your agreement with the {QUEST_GIVER.LINK} has canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x060011E8 RID: 4584 RVA: 0x000724B8 File Offset: 0x000706B8
			public RivalGangMovingInIssueQuest(string questId, Hero questGiver, Hero rivalGangLeader, int duration, int rewardGold, float issueDifficulty)
				: base(questId, questGiver, CampaignTime.DaysFromNow((float)duration), rewardGold)
			{
				this._rivalGangLeader = rivalGangLeader;
				this._rewardGold = rewardGold;
				this._issueDifficulty = issueDifficulty;
				this._timeoutDurationInDays = (float)duration;
				this._preparationCompletionTime = CampaignTime.DaysFromNow(2f);
				this._questTimeoutTime = CampaignTime.DaysFromNow(this._timeoutDurationInDays);
				this._sentTroops = new List<CharacterObject>();
				this._allPlayerTroops = new List<TroopRosterElement>();
				this.InitializeQuestSettlement();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x170001DE RID: 478
			// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00072540 File Offset: 0x00070740
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=vAjgn7yx}Rival Gang Moving in at {SETTLEMENT}", null);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170001DF RID: 479
			// (get) Token: 0x060011EA RID: 4586 RVA: 0x00072564 File Offset: 0x00070764
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060011EB RID: 4587 RVA: 0x00072568 File Offset: 0x00070768
			protected override void InitializeQuestOnGameLoad()
			{
				this.InitializeQuestSettlement();
				this.SetDialogs();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRivalGangLeaderDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetQuestGiverPreparationCompletedDialogFlow(), this);
				MobileParty rivalGangLeaderParty = this._rivalGangLeaderParty;
				if (rivalGangLeaderParty != null)
				{
					rivalGangLeaderParty.SetPartyUsedByQuest(true);
				}
				this._sentTroops = new List<CharacterObject>();
				this._allPlayerTroops = new List<TroopRosterElement>();
			}

			// Token: 0x060011EC RID: 4588 RVA: 0x000725D5 File Offset: 0x000707D5
			private void InitializeQuestSettlement()
			{
				this._questSettlement = base.QuestGiver.CurrentSettlement;
			}

			// Token: 0x060011ED RID: 4589 RVA: 0x000725E8 File Offset: 0x000707E8
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine("{=Fwm0PwVb}Great. As I said we need minimum of {NUMBER} days,[ib:normal][if:convo_mocking_revenge] so they'll let their guard down. I will let you know when it's time. Remember, we wait for the dark of the night to strike.", null, null, null, null).Condition(delegate
				{
					MBTextManager.SetTextVariable("SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName, false);
					MBTextManager.SetTextVariable("NUMBER", 2);
					return Hero.OneToOneConversationHero == base.QuestGiver;
				})
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.OnQuestAccepted))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine("{=z43j3Tzq}I'm still gathering my men for the fight. I'll send a runner for you when the time comes.", null, null, null, null).Condition(delegate
				{
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
					return Hero.OneToOneConversationHero == base.QuestGiver && !this._isFinalStage && !this._preparationsComplete;
				})
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=4IHRAmnA}All right. I am waiting for your runner.", null, null, null)
					.NpcLine("{=xEs830bT}You'll know right away once the preparations are complete.[ib:closed][if:convo_mocking_teasing] Just don't leave town.", null, null, null, null)
					.CloseDialog()
					.PlayerOption("{=6g8qvD2M}I can't just hang on here forever. Be quick about it.", null, null, null)
					.NpcLine("{=lM7AscLo}I'm getting this together as quickly as I can.[ib:closed][if:convo_nervous]", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060011EE RID: 4590 RVA: 0x000726C0 File Offset: 0x000708C0
			private DialogFlow GetRivalGangLeaderDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=IfeN8lYd}Coming to fight us, eh? Did {QUEST_GIVER.LINK} put you up to this?[ib:aggressive2][if:convo_confused_annoyed] Look, there's no need for bloodshed. This town is big enough for all of us. But... if bloodshed is what you want, we will be happy to provide.", null, null, null, null).Condition(delegate
				{
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
					return Hero.OneToOneConversationHero == this._rivalGangLeaderHenchmanHero && this._isReadyToBeFinalized;
				})
					.NpcLine("{=WSJxl2Hu}What I want to say is... [if:convo_mocking_teasing]You don't need to be a part of this. My boss will double whatever {?QUEST_GIVER.GENDER}she{?}he{\\?} is paying you if you join us.", null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=GPBja02V}I gave my word to {QUEST_GIVER.LINK}, and I won't be bought.", null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
						{
							CombatMissionWithDialogueController missionBehavior = Mission.Current.GetMissionBehavior<CombatMissionWithDialogueController>();
							if (missionBehavior == null)
							{
								return;
							}
							missionBehavior.StartFight(false);
						};
					})
					.NpcLine("{=OSgBicif}You will regret this![ib:warrior][if:convo_furious]", null, null, null, null)
					.CloseDialog()
					.PlayerOption("{=RB4uQpPV}You're going to pay me a lot then, {REWARD}{GOLD_ICON} to be exact. But at that price, I agree.", null, null, null)
					.Condition(delegate
					{
						MBTextManager.SetTextVariable("REWARD", this._rewardGold * 2);
						return true;
					})
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
						{
							this._hasBetrayedQuestGiver = true;
							CombatMissionWithDialogueController missionBehavior2 = Mission.Current.GetMissionBehavior<CombatMissionWithDialogueController>();
							if (missionBehavior2 == null)
							{
								return;
							}
							missionBehavior2.StartFight(true);
						};
					})
					.NpcLine("{=5jW4FVDc}Welcome to our ranks then. [ib:warrior][if:convo_evil_smile]Let's kill those bastards!", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060011EF RID: 4591 RVA: 0x000727A0 File Offset: 0x000709A0
			private DialogFlow GetQuestGiverPreparationCompletedDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).BeginNpcOptions(null, false).NpcOption(new TextObject("{=hM7LSuB1}Good to see you. But we still need to wait until after dusk. {HERO.LINK}'s men may be watching, so let's keep our distance from each other until night falls.", null), delegate
				{
					StringHelpers.SetCharacterProperties("HERO", this._rivalGangLeader.CharacterObject, null, false);
					return Hero.OneToOneConversationHero == base.QuestGiver && !this._isFinalStage && this._preparationCompletionTime.IsPast && (!this._preparationsComplete || !CampaignTime.Now.IsNightTime);
				}, null, null, null, null)
					.CloseDialog()
					.NpcOption("{=JxNlB547}Are you ready for the fight?[ib:normal][if:convo_undecided_open]", () => Hero.OneToOneConversationHero == base.QuestGiver && this._preparationsComplete && !this._isFinalStage && CampaignTime.Now.IsNightTime, null, null, null, null)
					.EndNpcOptions()
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=NzMX0s21}I am ready.", null, null, null)
					.Condition(() => !Hero.MainHero.IsWounded)
					.NpcLine("{=dNjepcKu}Let's finish this![ib:hip][if:convo_mocking_revenge]", null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.rival_gang_start_fight_on_consequence;
					})
					.CloseDialog()
					.PlayerOption("{=B2Donbwz}I need more time.", null, null, null)
					.Condition(() => !Hero.MainHero.IsWounded)
					.NpcLine("{=advPT3WY}You'd better hurry up![ib:closed][if:convo_astonished]", null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.rival_gang_need_more_time_on_consequence;
					})
					.CloseDialog()
					.PlayerOption("{=QaN26CZ5}My wounds are still fresh. I need some time to recover.", null, null, null)
					.Condition(() => Hero.MainHero.IsWounded)
					.NpcLine("{=s0jKaYo0}We must attack before the rival gang hears about our plan. You'd better hurry up![if:convo_astonished]", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060011F0 RID: 4592 RVA: 0x00072903 File Offset: 0x00070B03
			public override void OnHeroCanDieInfoIsRequested(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
			{
				if (hero == base.QuestGiver || hero == this._rivalGangLeader)
				{
					result = false;
				}
			}

			// Token: 0x060011F1 RID: 4593 RVA: 0x0007291A File Offset: 0x00070B1A
			private void rival_gang_start_fight_on_consequence()
			{
				this._isFinalStage = true;
				if (Mission.Current != null)
				{
					Mission.Current.EndMission();
				}
				Campaign.Current.GameMenuManager.SetNextMenu("rival_gang_quest_before_fight");
			}

			// Token: 0x060011F2 RID: 4594 RVA: 0x00072948 File Offset: 0x00070B48
			private void rival_gang_need_more_time_on_consequence()
			{
				if (Campaign.Current.CurrentMenuContext.GameMenu.StringId == "rival_gang_quest_wait_duration_is_over")
				{
					Campaign.Current.GameMenuManager.SetNextMenu("town_wait_menus");
				}
			}

			// Token: 0x060011F3 RID: 4595 RVA: 0x00072980 File Offset: 0x00070B80
			private void AddQuestGiverGangLeaderOnSuccessDialogFlow()
			{
				Campaign.Current.ConversationManager.AddDialogFlow(DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=zNPzh5jO}Ah! Now that was as good a fight as any I've had. Here, take this purse, It is all yours as {QUEST_GIVER.LINK} has promised.[ib:hip2][if:convo_huge_smile]", null, null, null, null).Condition(delegate
				{
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
					return base.IsOngoing && Hero.OneToOneConversationHero == this._allyGangLeaderHenchmanHero;
				})
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.OnQuestSucceeded;
					})
					.CloseDialog(), null);
			}

			// Token: 0x060011F4 RID: 4596 RVA: 0x000729E0 File Offset: 0x00070BE0
			private CharacterObject GetTroopTypeTemplateForDifficulty()
			{
				int difficultyRange = MBMath.ClampInt(MathF.Ceiling(this._issueDifficulty / 0.1f), 1, 10);
				CharacterObject characterObject;
				if (difficultyRange == 1)
				{
					characterObject = CharacterObject.All.FirstOrDefault<CharacterObject>((CharacterObject t) => t.StringId == "looter");
				}
				else if (difficultyRange == 10)
				{
					characterObject = CharacterObject.All.FirstOrDefault<CharacterObject>((CharacterObject t) => t.StringId == "mercenary_8");
				}
				else
				{
					characterObject = CharacterObject.All.FirstOrDefault<CharacterObject>((CharacterObject t) => t.StringId == "mercenary_" + (difficultyRange - 1));
				}
				if (characterObject == null)
				{
					Debug.FailedAssert("Can't find troop in rival gang leader quest", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Issues\\RivalGangMovingInIssueBehavior.cs", "GetTroopTypeTemplateForDifficulty", 806);
					characterObject = CharacterObject.All.First<CharacterObject>((CharacterObject t) => t.IsBasicTroop && t.IsSoldier);
				}
				return characterObject;
			}

			// Token: 0x060011F5 RID: 4597 RVA: 0x00072ADC File Offset: 0x00070CDC
			internal void StartAlleyBattle()
			{
				this.CreateRivalGangLeaderParty();
				this.CreateAllyGangLeaderParty();
				this.PreparePlayerParty();
				PlayerEncounter.RestartPlayerEncounter(this._rivalGangLeaderParty.Party, PartyBase.MainParty, false, false);
				PlayerEncounter.StartBattle();
				this._allyGangLeaderParty.MapEventSide = PlayerEncounter.Battle.GetMapEventSide(PlayerEncounter.Battle.PlayerSide);
				GameMenu.ActivateGameMenu("rival_gang_quest_after_fight");
				this._isReadyToBeFinalized = true;
				PlayerEncounter.StartCombatMissionWithDialogueInTownCenter(this._rivalGangLeaderHenchmanHero.CharacterObject);
			}

			// Token: 0x060011F6 RID: 4598 RVA: 0x00072B58 File Offset: 0x00070D58
			private void CreateRivalGangLeaderParty()
			{
				TextObject textObject = new TextObject("{=u4jhIFwG}{GANG_LEADER}'s Party", null);
				textObject.SetTextVariable("RIVAL_GANG_LEADER", this._rivalGangLeader.Name);
				textObject.SetTextVariable("GANG_LEADER", this._rivalGangLeader.Name);
				Hideout closestHideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement x) => x.IsActive);
				Clan clan = Clan.BanditFactions.FirstOrDefaultQ<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
				this._rivalGangLeaderParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(this._questSettlement.GatePosition, 1f, this._questSettlement, textObject, clan, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), null, "", "", 0f, false);
				this._rivalGangLeaderParty.SetPartyUsedByQuest(true);
				CharacterObject troopTypeTemplateForDifficulty = this.GetTroopTypeTemplateForDifficulty();
				this._rivalGangLeaderParty.MemberRoster.AddToCounts(troopTypeTemplateForDifficulty, 15, false, 0, 0, true, -1);
				CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("gangster_3");
				this._rivalGangLeaderHenchmanHero = HeroCreator.CreateSpecialHero(@object, null, null, null, -1);
				TextObject textObject2 = new TextObject("{=zJqEdDiq}Henchman of {GANG_LEADER}", null);
				textObject2.SetTextVariable("GANG_LEADER", this._rivalGangLeader.Name);
				this._rivalGangLeaderHenchmanHero.SetName(textObject2, textObject2);
				this._rivalGangLeaderHenchmanHero.HiddenInEncyclopedia = true;
				this._rivalGangLeaderHenchmanHero.Culture = this._rivalGangLeader.Culture;
				this._rivalGangLeaderHenchmanHero.SetNewOccupation(Occupation.Special);
				this._rivalGangLeaderHenchmanHero.ChangeState(Hero.CharacterStates.Active);
				this._rivalGangLeaderParty.MemberRoster.AddToCounts(this._rivalGangLeaderHenchmanHero.CharacterObject, 1, false, 0, 0, true, -1);
				this._rivalGangLeaderParty.IgnoreByOtherPartiesTill(CampaignTime.Never);
				EnterSettlementAction.ApplyForParty(this._rivalGangLeaderParty, this._questSettlement);
			}

			// Token: 0x060011F7 RID: 4599 RVA: 0x00072D2C File Offset: 0x00070F2C
			private void CreateAllyGangLeaderParty()
			{
				TextObject textObject = new TextObject("{=u4jhIFwG}{GANG_LEADER}'s Party", null);
				textObject.SetTextVariable("GANG_LEADER", base.QuestGiver.Name);
				Hideout closestHideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.All, (Settlement x) => x.IsActive);
				Clan clan = Clan.BanditFactions.FirstOrDefaultQ<Clan>((Clan t) => t.Culture == closestHideout.Settlement.Culture);
				this._allyGangLeaderParty = CustomPartyComponent.CreateCustomPartyWithTroopRoster(this._questSettlement.GatePosition, 1f, this._questSettlement, textObject, clan, TroopRoster.CreateDummyTroopRoster(), TroopRoster.CreateDummyTroopRoster(), null, "", "", 0f, false);
				this._allyGangLeaderParty.SetPartyUsedByQuest(true);
				CharacterObject troopTypeTemplateForDifficulty = this.GetTroopTypeTemplateForDifficulty();
				this._allyGangLeaderParty.MemberRoster.AddToCounts(troopTypeTemplateForDifficulty, 20, false, 0, 0, true, -1);
				CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("gangster_2");
				this._allyGangLeaderHenchmanHero = HeroCreator.CreateSpecialHero(@object, null, null, null, -1);
				TextObject textObject2 = new TextObject("{=zJqEdDiq}Henchman of {GANG_LEADER}", null);
				textObject2.SetTextVariable("GANG_LEADER", base.QuestGiver.Name);
				this._allyGangLeaderHenchmanHero.SetName(textObject2, textObject2);
				this._allyGangLeaderHenchmanHero.HiddenInEncyclopedia = true;
				this._allyGangLeaderHenchmanHero.Culture = base.QuestGiver.Culture;
				this._allyGangLeaderHenchmanHero.ChangeState(Hero.CharacterStates.Active);
				this._allyGangLeaderParty.MemberRoster.AddToCounts(this._allyGangLeaderHenchmanHero.CharacterObject, 1, false, 0, 0, true, -1);
				this._allyGangLeaderParty.IgnoreByOtherPartiesTill(CampaignTime.Never);
				EnterSettlementAction.ApplyForParty(this._allyGangLeaderParty, this._questSettlement);
			}

			// Token: 0x060011F8 RID: 4600 RVA: 0x00072EDC File Offset: 0x000710DC
			private void PreparePlayerParty()
			{
				this._allPlayerTroops.Clear();
				foreach (TroopRosterElement troopRosterElement in PartyBase.MainParty.MemberRoster.GetTroopRoster())
				{
					if (!troopRosterElement.Character.IsPlayerCharacter)
					{
						this._allPlayerTroops.Add(troopRosterElement);
					}
				}
				this._partyEngineer = MobileParty.MainParty.GetRoleHolder(PartyRole.Engineer);
				this._partyScout = MobileParty.MainParty.GetRoleHolder(PartyRole.Scout);
				this._partyQuartermaster = MobileParty.MainParty.GetRoleHolder(PartyRole.Quartermaster);
				this._partySurgeon = MobileParty.MainParty.GetRoleHolder(PartyRole.Surgeon);
				PartyBase.MainParty.MemberRoster.RemoveIf((TroopRosterElement t) => !t.Character.IsPlayerCharacter);
				if (!this._allPlayerTroops.IsEmpty<TroopRosterElement>())
				{
					this._sentTroops.Clear();
					int num = 5;
					foreach (TroopRosterElement troopRosterElement2 in this._allPlayerTroops.OrderByDescending<TroopRosterElement, int>((TroopRosterElement t) => t.Character.Level))
					{
						if (num <= 0)
						{
							break;
						}
						int num2 = 0;
						while (num2 < troopRosterElement2.Number - troopRosterElement2.WoundedNumber && num > 0)
						{
							this._sentTroops.Add(troopRosterElement2.Character);
							num--;
							num2++;
						}
					}
					foreach (CharacterObject characterObject in this._sentTroops)
					{
						PartyBase.MainParty.MemberRoster.AddToCounts(characterObject, 1, false, 0, 0, true, -1);
					}
				}
			}

			// Token: 0x060011F9 RID: 4601 RVA: 0x000730D4 File Offset: 0x000712D4
			internal void HandlePlayerEncounterResult(bool hasPlayerWon)
			{
				PlayerEncounter.Finish(false);
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, this._questSettlement);
				TroopRoster troopRoster = PartyBase.MainParty.MemberRoster.CloneRosterData();
				PartyBase.MainParty.MemberRoster.RemoveIf((TroopRosterElement t) => !t.Character.IsPlayerCharacter);
				using (List<TroopRosterElement>.Enumerator enumerator = this._allPlayerTroops.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TroopRosterElement playerTroop = enumerator.Current;
						int num = troopRoster.FindIndexOfTroop(playerTroop.Character);
						int num2 = playerTroop.Number;
						int num3 = playerTroop.WoundedNumber;
						int num4 = playerTroop.Xp;
						if (num >= 0)
						{
							TroopRosterElement elementCopyAtIndex = troopRoster.GetElementCopyAtIndex(num);
							num2 -= this._sentTroops.Count<CharacterObject>((CharacterObject t) => t == playerTroop.Character) - elementCopyAtIndex.Number;
							num3 += elementCopyAtIndex.WoundedNumber;
							num4 += elementCopyAtIndex.Xp;
						}
						else if (this._sentTroops.Contains(playerTroop.Character))
						{
							num2 -= this._sentTroops.Count<CharacterObject>((CharacterObject t) => t == playerTroop.Character);
						}
						PartyBase.MainParty.MemberRoster.AddToCounts(playerTroop.Character, num2, false, num3, num4, true, -1);
					}
				}
				MobileParty.MainParty.SetPartyEngineer(this._partyEngineer);
				MobileParty.MainParty.SetPartyScout(this._partyScout);
				MobileParty.MainParty.SetPartyQuartermaster(this._partyQuartermaster);
				MobileParty.MainParty.SetPartySurgeon(this._partySurgeon);
				if (this._rivalGangLeader.PartyBelongedTo == this._rivalGangLeaderParty)
				{
					this._rivalGangLeaderParty.MemberRoster.AddToCounts(this._rivalGangLeader.CharacterObject, -1, false, 0, 0, true, -1);
				}
				if (hasPlayerWon)
				{
					if (!this._hasBetrayedQuestGiver)
					{
						this.AddQuestGiverGangLeaderOnSuccessDialogFlow();
						this.SpawnAllyHenchmanAfterMissionSuccess();
						PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationOfCharacter(this._allyGangLeaderHenchmanHero), null, this._allyGangLeaderHenchmanHero.CharacterObject, null);
						return;
					}
					this.OnBattleWonWithBetrayal();
					return;
				}
				else
				{
					if (!this._hasBetrayedQuestGiver)
					{
						this.OnQuestFailedWithDefeat();
						return;
					}
					this.OnBattleLostWithBetrayal();
					return;
				}
			}

			// Token: 0x060011FA RID: 4602 RVA: 0x00073340 File Offset: 0x00071540
			protected override void RegisterEvents()
			{
				CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
				CampaignEvents.AlleyClearedByPlayer.AddNonSerializedListener(this, new Action<Alley>(this.OnAlleyClearedByPlayer));
				CampaignEvents.AlleyOccupiedByPlayer.AddNonSerializedListener(this, new Action<Alley, TroopRoster>(this.OnAlleyOccupiedByPlayer));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			}

			// Token: 0x060011FB RID: 4603 RVA: 0x000733F0 File Offset: 0x000715F0
			private void SpawnAllyHenchmanAfterMissionSuccess()
			{
				Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(this._allyGangLeaderHenchmanHero.CharacterObject.Race, "_settlement");
				LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new SimpleAgentOrigin(this._allyGangLeaderHenchmanHero.CharacterObject, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common", true, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true, null, false);
				LocationComplex.Current.GetLocationWithId("center").AddCharacter(locationCharacter);
			}

			// Token: 0x060011FC RID: 4604 RVA: 0x00073480 File Offset: 0x00071680
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement == base.QuestGiver.CurrentSettlement && newOwner == Hero.MainHero)
				{
					base.AddLog(this.OwnerOfQuestSettlementIsPlayerClanLogText, false);
					base.QuestGiver.AddPower(-10f);
					ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -5, true, true);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x060011FD RID: 4605 RVA: 0x000734D7 File Offset: 0x000716D7
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._rivalGangLeader)
				{
					result = false;
				}
			}

			// Token: 0x060011FE RID: 4606 RVA: 0x000734E5 File Offset: 0x000716E5
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.OnQuestCancelledDueToWarLogText);
				}
			}

			// Token: 0x060011FF RID: 4607 RVA: 0x00073514 File Offset: 0x00071714
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.OnQuestCancelledDueToWarLogText, false);
			}

			// Token: 0x06001200 RID: 4608 RVA: 0x0007352C File Offset: 0x0007172C
			private void OnSiegeEventStarted(SiegeEvent siegeEvent)
			{
				if (siegeEvent.BesiegedSettlement == this._questSettlement)
				{
					base.AddLog(this.OnQuestCancelledDueToSiegeLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06001201 RID: 4609 RVA: 0x00073554 File Offset: 0x00071754
			protected override void HourlyTick()
			{
				if (RivalGangMovingInIssueBehavior.Instance != null && RivalGangMovingInIssueBehavior.Instance.IsOngoing && (2f - RivalGangMovingInIssueBehavior.Instance._preparationCompletionTime.RemainingDaysFromNow) / 2f >= 1f && !this._preparationsComplete && CampaignTime.Now.IsNightTime)
				{
					this.OnGuestGiverPreparationsCompleted();
				}
			}

			// Token: 0x06001202 RID: 4610 RVA: 0x000735B8 File Offset: 0x000717B8
			private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
			{
				if (victim == this._rivalGangLeader)
				{
					TextObject textObject = ((detail == KillCharacterAction.KillCharacterActionDetail.Lost) ? this.TargetHeroDisappearedLogText : this.TargetHeroDiedLogText);
					StringHelpers.SetCharacterProperties("QUEST_TARGET", this._rivalGangLeader.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					base.AddLog(textObject, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06001203 RID: 4611 RVA: 0x00073621 File Offset: 0x00071821
			private void OnPlayerAlleyFightEnd(Alley alley)
			{
				if (!this._isReadyToBeFinalized)
				{
					if (alley.Owner == this._rivalGangLeader)
					{
						this.OnPlayerAttackedRivalGangAlley();
						return;
					}
					if (alley.Owner == base.QuestGiver)
					{
						this.OnPlayerAttackedQuestGiverAlley();
					}
				}
			}

			// Token: 0x06001204 RID: 4612 RVA: 0x00073654 File Offset: 0x00071854
			private void OnAlleyClearedByPlayer(Alley alley)
			{
				this.OnPlayerAlleyFightEnd(alley);
			}

			// Token: 0x06001205 RID: 4613 RVA: 0x0007365D File Offset: 0x0007185D
			private void OnAlleyOccupiedByPlayer(Alley alley, TroopRoster troops)
			{
				this.OnPlayerAlleyFightEnd(alley);
			}

			// Token: 0x06001206 RID: 4614 RVA: 0x00073666 File Offset: 0x00071866
			private void OnPlayerAttackedRivalGangAlley()
			{
				base.AddLog(this.PlayerStartedAlleyFightWithRivalGangLeader, false);
				base.CompleteQuestWithCancel(null);
			}

			// Token: 0x06001207 RID: 4615 RVA: 0x00073680 File Offset: 0x00071880
			private void OnPlayerAttackedQuestGiverAlley()
			{
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -150)
				});
				base.QuestGiver.AddPower(-10f);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -8, true, true);
				this._questSettlement.Town.Security += -10f;
				base.AddLog(this.PlayerStartedAlleyFightWithQuestgiver, false);
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06001208 RID: 4616 RVA: 0x00073700 File Offset: 0x00071900
			protected override void OnTimedOut()
			{
				this.OnQuestFailedWithRejectionOrTimeout();
			}

			// Token: 0x06001209 RID: 4617 RVA: 0x00073708 File Offset: 0x00071908
			private void OnGuestGiverPreparationsCompleted()
			{
				this._preparationsComplete = true;
				if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == this._questSettlement && Campaign.Current.CurrentMenuContext != null && Campaign.Current.CurrentMenuContext.GameMenu.StringId == "town_wait_menus")
				{
					Campaign.Current.CurrentMenuContext.SwitchToMenu("rival_gang_quest_wait_duration_is_over");
				}
				TextObject textObject = new TextObject("{=DUKbtlNb}{QUEST_GIVER.LINK} has finally sent a messenger telling you it's time to meet {?QUEST_GIVER.GENDER}her{?}him{\\?} and join the fight.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				base.AddLog(this.OnQuestPreperationsCompletedLogText, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}

			// Token: 0x0600120A RID: 4618 RVA: 0x000737B0 File Offset: 0x000719B0
			private void OnQuestAccepted()
			{
				base.StartQuest();
				this._onQuestStartedLog = base.AddLog(this.OnQuestStartedLogText, false);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRivalGangLeaderDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetQuestGiverPreparationCompletedDialogFlow(), this);
			}

			// Token: 0x0600120B RID: 4619 RVA: 0x00073804 File Offset: 0x00071A04
			private void OnQuestSucceeded()
			{
				this._onQuestSucceededLog = base.AddLog(this.OnQuestSucceededLogText, false);
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 50)
				});
				base.QuestGiver.AddPower(10f);
				this._rivalGangLeader.AddPower(-10f);
				this.RelationshipChangeWithQuestGiver = 5;
				ChangeRelationAction.ApplyPlayerRelation(this._rivalGangLeader, -5, true, true);
				GameMenu.ExitToLast();
				GameMenu.ActivateGameMenu("town");
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x0600120C RID: 4620 RVA: 0x000738B1 File Offset: 0x00071AB1
			private void OnQuestFailedWithRejectionOrTimeout()
			{
				base.AddLog(this.OnQuestFailedWithRejectionLogText, false);
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -20)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				this.ApplyQuestFailConsequences();
			}

			// Token: 0x0600120D RID: 4621 RVA: 0x000738F0 File Offset: 0x00071AF0
			private void OnBattleWonWithBetrayal()
			{
				base.AddLog(this.OnQuestFailedWithBetrayalLogText, false);
				this.RelationshipChangeWithQuestGiver = -15;
				if (!this._rivalGangLeader.IsDead)
				{
					ChangeRelationAction.ApplyPlayerRelation(this._rivalGangLeader, 5, true, true);
				}
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold * 2, false);
				TraitLevelingHelper.OnIssueSolvedThroughBetrayal(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				this._rivalGangLeader.AddPower(10f);
				GameMenu.SwitchToMenu("town");
				this.ApplyQuestFailConsequences();
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x0600120E RID: 4622 RVA: 0x0007398C File Offset: 0x00071B8C
			private void OnBattleLostWithBetrayal()
			{
				base.AddLog(this.OnQuestFailedWithBetrayalLogText, false);
				this.RelationshipChangeWithQuestGiver = -10;
				if (!this._rivalGangLeader.IsDead)
				{
					ChangeRelationAction.ApplyPlayerRelation(this._rivalGangLeader, -5, true, true);
				}
				this._rivalGangLeader.AddPower(-10f);
				TraitLevelingHelper.OnIssueSolvedThroughBetrayal(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				GameMenu.SwitchToMenu("town");
				this.ApplyQuestFailConsequences();
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x0600120F RID: 4623 RVA: 0x00073A12 File Offset: 0x00071C12
			private void OnQuestFailedWithDefeat()
			{
				this.RelationshipChangeWithQuestGiver = -5;
				GameMenu.SwitchToMenu("town");
				base.AddLog(this.OnQuestFailedWithDefeatLogText, false);
				this.ApplyQuestFailConsequences();
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06001210 RID: 4624 RVA: 0x00073A44 File Offset: 0x00071C44
			private void ApplyQuestFailConsequences()
			{
				base.QuestGiver.AddPower(-10f);
				this._questSettlement.Town.Security += -10f;
				if (this._rivalGangLeaderParty != null && this._rivalGangLeaderParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._rivalGangLeaderParty);
				}
			}

			// Token: 0x06001211 RID: 4625 RVA: 0x00073AA0 File Offset: 0x00071CA0
			protected override void OnFinalize()
			{
				if (this._rivalGangLeaderParty != null && this._rivalGangLeaderParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._rivalGangLeaderParty);
				}
				if (this._allyGangLeaderParty != null && this._allyGangLeaderParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._allyGangLeaderParty);
				}
				if (this._allyGangLeaderHenchmanHero != null && this._allyGangLeaderHenchmanHero.IsAlive)
				{
					this._allyGangLeaderHenchmanHero.SetNewOccupation(Occupation.Special);
					KillCharacterAction.ApplyByRemove(this._allyGangLeaderHenchmanHero, false, true);
				}
				if (this._rivalGangLeaderHenchmanHero != null && this._rivalGangLeaderHenchmanHero.IsAlive)
				{
					this._rivalGangLeaderHenchmanHero.SetNewOccupation(Occupation.NotAssigned);
					KillCharacterAction.ApplyByRemove(this._rivalGangLeaderHenchmanHero, false, true);
				}
			}

			// Token: 0x06001212 RID: 4626 RVA: 0x00073B4C File Offset: 0x00071D4C
			internal static void AutoGeneratedStaticCollectObjectsRivalGangMovingInIssueQuest(object o, List<object> collectedObjects)
			{
				((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06001213 RID: 4627 RVA: 0x00073B5C File Offset: 0x00071D5C
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._rivalGangLeader);
				collectedObjects.Add(this._rivalGangLeaderParty);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this._preparationCompletionTime, collectedObjects);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this._questTimeoutTime, collectedObjects);
			}

			// Token: 0x06001214 RID: 4628 RVA: 0x00073BAA File Offset: 0x00071DAA
			internal static object AutoGeneratedGetMemberValue_rivalGangLeader(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._rivalGangLeader;
			}

			// Token: 0x06001215 RID: 4629 RVA: 0x00073BB7 File Offset: 0x00071DB7
			internal static object AutoGeneratedGetMemberValue_timeoutDurationInDays(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._timeoutDurationInDays;
			}

			// Token: 0x06001216 RID: 4630 RVA: 0x00073BC9 File Offset: 0x00071DC9
			internal static object AutoGeneratedGetMemberValue_isFinalStage(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._isFinalStage;
			}

			// Token: 0x06001217 RID: 4631 RVA: 0x00073BDB File Offset: 0x00071DDB
			internal static object AutoGeneratedGetMemberValue_isReadyToBeFinalized(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._isReadyToBeFinalized;
			}

			// Token: 0x06001218 RID: 4632 RVA: 0x00073BED File Offset: 0x00071DED
			internal static object AutoGeneratedGetMemberValue_hasBetrayedQuestGiver(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._hasBetrayedQuestGiver;
			}

			// Token: 0x06001219 RID: 4633 RVA: 0x00073BFF File Offset: 0x00071DFF
			internal static object AutoGeneratedGetMemberValue_rivalGangLeaderParty(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._rivalGangLeaderParty;
			}

			// Token: 0x0600121A RID: 4634 RVA: 0x00073C0C File Offset: 0x00071E0C
			internal static object AutoGeneratedGetMemberValue_preparationCompletionTime(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._preparationCompletionTime;
			}

			// Token: 0x0600121B RID: 4635 RVA: 0x00073C1E File Offset: 0x00071E1E
			internal static object AutoGeneratedGetMemberValue_questTimeoutTime(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._questTimeoutTime;
			}

			// Token: 0x0600121C RID: 4636 RVA: 0x00073C30 File Offset: 0x00071E30
			internal static object AutoGeneratedGetMemberValue_preparationsComplete(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._preparationsComplete;
			}

			// Token: 0x0600121D RID: 4637 RVA: 0x00073C42 File Offset: 0x00071E42
			internal static object AutoGeneratedGetMemberValue_rewardGold(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._rewardGold;
			}

			// Token: 0x0600121E RID: 4638 RVA: 0x00073C54 File Offset: 0x00071E54
			internal static object AutoGeneratedGetMemberValue_issueDifficulty(object o)
			{
				return ((RivalGangMovingInIssueBehavior.RivalGangMovingInIssueQuest)o)._issueDifficulty;
			}

			// Token: 0x04000887 RID: 2183
			private const int QuestGiverRelationChangeOnSuccess = 5;

			// Token: 0x04000888 RID: 2184
			private const int RivalGangLeaderRelationChangeOnSuccess = -5;

			// Token: 0x04000889 RID: 2185
			private const int QuestGiverNotablePowerChangeOnSuccess = 10;

			// Token: 0x0400088A RID: 2186
			private const int RivalGangLeaderPowerChangeOnSuccess = -10;

			// Token: 0x0400088B RID: 2187
			private const int RenownChangeOnSuccess = 1;

			// Token: 0x0400088C RID: 2188
			private const int QuestGiverRelationChangeOnFail = -5;

			// Token: 0x0400088D RID: 2189
			private const int QuestGiverRelationChangeOnTimedOut = -5;

			// Token: 0x0400088E RID: 2190
			private const int NotablePowerChangeOnFail = -10;

			// Token: 0x0400088F RID: 2191
			private const int TownSecurityChangeOnFail = -10;

			// Token: 0x04000890 RID: 2192
			private const int RivalGangLeaderRelationChangeOnSuccessfulBetrayal = 5;

			// Token: 0x04000891 RID: 2193
			private const int QuestGiverRelationChangeOnSuccessfulBetrayal = -15;

			// Token: 0x04000892 RID: 2194
			private const int RivalGangLeaderPowerChangeOnSuccessfulBetrayal = 10;

			// Token: 0x04000893 RID: 2195
			private const int QuestGiverRelationChangeOnFailedBetrayal = -10;

			// Token: 0x04000894 RID: 2196
			private const int PlayerAttackedQuestGiverHonorChange = -150;

			// Token: 0x04000895 RID: 2197
			private const int PlayerAttackedQuestGiverPowerChange = -10;

			// Token: 0x04000896 RID: 2198
			private const int NumberOfRegularEnemyTroops = 15;

			// Token: 0x04000897 RID: 2199
			private const int PlayerAttackedQuestGiverRelationChange = -8;

			// Token: 0x04000898 RID: 2200
			private const int PlayerAttackedQuestGiverSecurityChange = -10;

			// Token: 0x04000899 RID: 2201
			private const int NumberOfRegularAllyTroops = 20;

			// Token: 0x0400089A RID: 2202
			private const int MaxNumberOfPlayerOwnedTroops = 5;

			// Token: 0x0400089B RID: 2203
			private const string AllyGangLeaderHenchmanStringId = "gangster_2";

			// Token: 0x0400089C RID: 2204
			private const string RivalGangLeaderHenchmanStringId = "gangster_3";

			// Token: 0x0400089D RID: 2205
			private const int PreparationDurationInDays = 2;

			// Token: 0x0400089E RID: 2206
			[SaveableField(10)]
			internal readonly Hero _rivalGangLeader;

			// Token: 0x0400089F RID: 2207
			[SaveableField(20)]
			private MobileParty _rivalGangLeaderParty;

			// Token: 0x040008A0 RID: 2208
			private Hero _rivalGangLeaderHenchmanHero;

			// Token: 0x040008A1 RID: 2209
			[SaveableField(30)]
			private readonly CampaignTime _preparationCompletionTime;

			// Token: 0x040008A2 RID: 2210
			private Hero _allyGangLeaderHenchmanHero;

			// Token: 0x040008A3 RID: 2211
			private MobileParty _allyGangLeaderParty;

			// Token: 0x040008A4 RID: 2212
			[SaveableField(40)]
			private readonly CampaignTime _questTimeoutTime;

			// Token: 0x040008A5 RID: 2213
			[SaveableField(60)]
			internal readonly float _timeoutDurationInDays;

			// Token: 0x040008A6 RID: 2214
			[SaveableField(70)]
			internal bool _isFinalStage;

			// Token: 0x040008A7 RID: 2215
			[SaveableField(80)]
			internal bool _isReadyToBeFinalized;

			// Token: 0x040008A8 RID: 2216
			[SaveableField(90)]
			internal bool _hasBetrayedQuestGiver;

			// Token: 0x040008A9 RID: 2217
			private List<TroopRosterElement> _allPlayerTroops;

			// Token: 0x040008AA RID: 2218
			private List<CharacterObject> _sentTroops;

			// Token: 0x040008AB RID: 2219
			private Hero _partyEngineer;

			// Token: 0x040008AC RID: 2220
			private Hero _partyScout;

			// Token: 0x040008AD RID: 2221
			private Hero _partyQuartermaster;

			// Token: 0x040008AE RID: 2222
			private Hero _partySurgeon;

			// Token: 0x040008AF RID: 2223
			[SaveableField(110)]
			private bool _preparationsComplete;

			// Token: 0x040008B0 RID: 2224
			[SaveableField(120)]
			private int _rewardGold;

			// Token: 0x040008B1 RID: 2225
			[SaveableField(130)]
			private float _issueDifficulty;

			// Token: 0x040008B2 RID: 2226
			private Settlement _questSettlement;

			// Token: 0x040008B3 RID: 2227
			private JournalLog _onQuestStartedLog;

			// Token: 0x040008B4 RID: 2228
			private JournalLog _onQuestSucceededLog;
		}
	}
}
