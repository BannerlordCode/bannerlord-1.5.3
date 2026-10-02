using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000388 RID: 904
	public class LordNeedsGarrisonTroopsIssueQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06003594 RID: 13716 RVA: 0x000DAE70 File Offset: 0x000D9070
		private static LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest Instance
		{
			get
			{
				LordNeedsGarrisonTroopsIssueQuestBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<LordNeedsGarrisonTroopsIssueQuestBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest lordNeedsGarrisonTroopsIssueQuest;
						if ((lordNeedsGarrisonTroopsIssueQuest = enumerator.Current as LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest) != null)
						{
							campaignBehavior._cachedQuest = lordNeedsGarrisonTroopsIssueQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000DAF08 File Offset: 0x000D9108
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000DAF38 File Offset: 0x000D9138
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			string text = "{=FirEOQaI}Talk to the garrison commander";
			gameStarter.AddGameMenuOption("town", "talk_to_garrison_commander_town", text, new GameMenuOption.OnConditionDelegate(this.talk_to_garrison_commander_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_garrison_commander_on_consequence), false, 0, false, null);
			gameStarter.AddGameMenuOption("town_guard", "talk_to_garrison_commander_town", text, new GameMenuOption.OnConditionDelegate(this.talk_to_garrison_commander_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_garrison_commander_on_consequence), false, 2, false, null);
			gameStarter.AddGameMenuOption("castle_guard", "talk_to_garrison_commander_castle", text, new GameMenuOption.OnConditionDelegate(this.talk_to_garrison_commander_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_garrison_commander_on_consequence), false, 2, false, null);
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000DAFD4 File Offset: 0x000D91D4
		private bool talk_to_garrison_commander_on_condition(MenuCallbackArgs args)
		{
			if (LordNeedsGarrisonTroopsIssueQuestBehavior.Instance != null)
			{
				if (Settlement.CurrentSettlement == LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement)
				{
					Town town = LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement.Town;
					if (((town != null) ? town.GarrisonParty : null) == null)
					{
						args.IsEnabled = false;
						args.Tooltip = new TextObject("{=JmoOJX4e}There is no one in the garrison to receive the troops requested. You should wait until someone arrives.", null);
					}
				}
				args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
				args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveIssue;
				return Settlement.CurrentSettlement == LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement;
			}
			return false;
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000DB050 File Offset: 0x000D9250
		private void talk_to_garrison_commander_on_consequence(MenuCallbackArgs args)
		{
			CharacterObject characterObject = LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement.OwnerClan.Culture.EliteBasicTroop;
			foreach (TroopRosterElement troopRosterElement in LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsInfantry && characterObject.Level < troopRosterElement.Character.Level)
				{
					characterObject = troopRosterElement.Character;
				}
			}
			LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._selectedCharacterToTalk = characterObject;
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false);
			CharacterObject selectedCharacterToTalk = LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._selectedCharacterToTalk;
			Town town = LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement.Town;
			CampaignMapConversation.OpenConversation(conversationCharacterData, new ConversationCharacterData(selectedCharacterToTalk, (town != null) ? town.GarrisonParty.Party : null, false, false, false, false, false, false));
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000DB150 File Offset: 0x000D9350
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x000DB154 File Offset: 0x000D9354
		private bool ConditionsHold(Hero issueGiver, out Settlement selectedSettlement)
		{
			selectedSettlement = null;
			if (issueGiver.IsLord && issueGiver.Clan.Leader == issueGiver && !issueGiver.IsMinorFactionHero && issueGiver.Clan != Clan.PlayerClan)
			{
				foreach (Settlement settlement in issueGiver.Clan.Settlements)
				{
					if (settlement.IsCastle)
					{
						MobileParty garrisonParty = settlement.Town.GarrisonParty;
						if (garrisonParty != null && garrisonParty.MemberRoster.TotalHealthyCount < 120)
						{
							selectedSettlement = settlement;
							break;
						}
					}
					if (settlement.IsTown)
					{
						MobileParty garrisonParty2 = settlement.Town.GarrisonParty;
						if (garrisonParty2 != null && garrisonParty2.MemberRoster.TotalHealthyCount < 150)
						{
							selectedSettlement = settlement;
							break;
						}
					}
				}
				return selectedSettlement != null;
			}
			return false;
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000DB248 File Offset: 0x000D9448
		public void OnCheckForIssue(Hero hero)
		{
			Settlement settlement;
			if (this.ConditionsHold(hero, out settlement))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue), IssueBase.IssueFrequency.Common, settlement));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue), IssueBase.IssueFrequency.Common));
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000DB2B0 File Offset: 0x000D94B0
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue(issueOwner, potentialIssueData.RelatedObject as Settlement);
		}

		// Token: 0x04000F12 RID: 3858
		private const IssueBase.IssueFrequency LordNeedsGarrisonTroopsIssueFrequency = IssueBase.IssueFrequency.Common;

		// Token: 0x04000F13 RID: 3859
		private LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest _cachedQuest;

		// Token: 0x02000765 RID: 1893
		public class LordNeedsGarrisonTroopsIssue : IssueBase
		{
			// Token: 0x06005F59 RID: 24409 RVA: 0x001BCE19 File Offset: 0x001BB019
			internal static void AutoGeneratedStaticCollectObjectsLordNeedsGarrisonTroopsIssue(object o, List<object> collectedObjects)
			{
				((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005F5A RID: 24410 RVA: 0x001BCE27 File Offset: 0x001BB027
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._settlement);
				collectedObjects.Add(this._neededTroopType);
			}

			// Token: 0x06005F5B RID: 24411 RVA: 0x001BCE48 File Offset: 0x001BB048
			internal static object AutoGeneratedGetMemberValue_settlement(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue)o)._settlement;
			}

			// Token: 0x06005F5C RID: 24412 RVA: 0x001BCE55 File Offset: 0x001BB055
			internal static object AutoGeneratedGetMemberValue_neededTroopType(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue)o)._neededTroopType;
			}

			// Token: 0x1700129F RID: 4767
			// (get) Token: 0x06005F5D RID: 24413 RVA: 0x001BCE62 File Offset: 0x001BB062
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x170012A0 RID: 4768
			// (get) Token: 0x06005F5E RID: 24414 RVA: 0x001BCE65 File Offset: 0x001BB065
			private int NumberOfTroopToBeRecruited
			{
				get
				{
					return 3 + (int)(base.IssueDifficultyMultiplier * 18f);
				}
			}

			// Token: 0x170012A1 RID: 4769
			// (get) Token: 0x06005F5F RID: 24415 RVA: 0x001BCE76 File Offset: 0x001BB076
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 5 + MathF.Ceiling(8f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170012A2 RID: 4770
			// (get) Token: 0x06005F60 RID: 24416 RVA: 0x001BCE8B File Offset: 0x001BB08B
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 3 + MathF.Ceiling(4f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170012A3 RID: 4771
			// (get) Token: 0x06005F61 RID: 24417 RVA: 0x001BCEA0 File Offset: 0x001BB0A0
			protected override int RewardGold
			{
				get
				{
					int num = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(this._neededTroopType, Hero.MainHero, false).RoundedResultNumber * this.NumberOfTroopToBeRecruited;
					return (int)(1500f + (float)num * 1.5f);
				}
			}

			// Token: 0x170012A4 RID: 4772
			// (get) Token: 0x06005F62 RID: 24418 RVA: 0x001BCEEC File Offset: 0x001BB0EC
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=ZuTvTGsh}These wars have taken a toll on my men. The bravest often fall first, they say, and fewer and fewer families are willing to let their sons join my banner. But the wars don't stop because I have problems.[if:convo_undecided_closed][ib:closed]", null);
				}
			}

			// Token: 0x170012A5 RID: 4773
			// (get) Token: 0x06005F63 RID: 24419 RVA: 0x001BCEFC File Offset: 0x001BB0FC
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=tTM6nPul}What can I do for you, {?ISSUE_OWNER.GENDER}madam{?}sir{\\?}?", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170012A6 RID: 4774
			// (get) Token: 0x06005F64 RID: 24420 RVA: 0x001BCF30 File Offset: 0x001BB130
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=driH06vI}I need more recruits in {SETTLEMENT}'s garrison. Since I'll be elsewhere... maybe you can recruit {NUMBER_OF_TROOP_TO_BE_RECRUITED} {TROOP_TYPE} and bring them to the garrison for me?[if:convo_undecided_open][ib:normal]", null);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					textObject.SetTextVariable("TROOP_TYPE", this._neededTroopType.EncyclopediaLinkWithName);
					textObject.SetTextVariable("NUMBER_OF_TROOP_TO_BE_RECRUITED", this.NumberOfTroopToBeRecruited);
					return textObject;
				}
			}

			// Token: 0x170012A7 RID: 4775
			// (get) Token: 0x06005F65 RID: 24421 RVA: 0x001BCF88 File Offset: 0x001BB188
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=igXcCqdo}One of your trusted companions who knows how to lead men can go around with {ALTERNATIVE_SOLUTION_MAN_COUNT} horsemen and pick some up. One way or the other I will pay {REWARD_GOLD}{GOLD_ICON} denars in return for your services. What do you say?[if:convo_thinking]", null);
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_MAN_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("REWARD_GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x170012A8 RID: 4776
			// (get) Token: 0x06005F66 RID: 24422 RVA: 0x001BCFD5 File Offset: 0x001BB1D5
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=YHSm72Ln}I'll find your recruits and bring them to {SETTLEMENT} garrison.", null);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170012A9 RID: 4777
			// (get) Token: 0x06005F67 RID: 24423 RVA: 0x001BCFFC File Offset: 0x001BB1FC
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=JPclWyyr}My companion can handle it... So, {NUMBER_OF_TROOP_TO_BE_RECRUITED} {TROOP_TYPE} to {SETTLEMENT}.", null);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					textObject.SetTextVariable("TROOP_TYPE", this._neededTroopType.EncyclopediaLinkWithName);
					textObject.SetTextVariable("NUMBER_OF_TROOP_TO_BE_RECRUITED", this.NumberOfTroopToBeRecruited);
					return textObject;
				}
			}

			// Token: 0x170012AA RID: 4778
			// (get) Token: 0x06005F68 RID: 24424 RVA: 0x001BD054 File Offset: 0x001BB254
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					TextObject textObject = new TextObject("{=lWrmxsYR}I haven't heard any news from {SETTLEMENT}, but I realize it might take some time for your men to deliver the recruits.", null);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170012AB RID: 4779
			// (get) Token: 0x06005F69 RID: 24425 RVA: 0x001BD078 File Offset: 0x001BB278
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=WUWzyzWI}Thank you. Your help will be remembered.", null);
				}
			}

			// Token: 0x170012AC RID: 4780
			// (get) Token: 0x06005F6A RID: 24426 RVA: 0x001BD088 File Offset: 0x001BB288
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=M560TDza}{ISSUE_OWNER.LINK}, the {?ISSUE_OWNER.GENDER}lady{?}lord{\\?} of {QUEST_SETTLEMENT}, told you that {?ISSUE_OWNER.GENDER}she{?}he{\\?} needs more troops in {?ISSUE_OWNER.GENDER}her{?}his{\\?} garrison. {?ISSUE_OWNER.GENDER}She{?}He{\\?} is willing to pay {REWARD}{GOLD_ICON} for your services. You asked your companion to deploy {NUMBER_OF_TROOP_TO_BE_RECRUITED} {TROOP_TYPE} troops to {QUEST_SETTLEMENT}'s garrison.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", this._settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("TROOP_TYPE", this._neededTroopType.EncyclopediaLinkWithName);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("NUMBER_OF_TROOP_TO_BE_RECRUITED", this.NumberOfTroopToBeRecruited);
					return textObject;
				}
			}

			// Token: 0x170012AD RID: 4781
			// (get) Token: 0x06005F6B RID: 24427 RVA: 0x001BD11D File Offset: 0x001BB31D
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170012AE RID: 4782
			// (get) Token: 0x06005F6C RID: 24428 RVA: 0x001BD120 File Offset: 0x001BB320
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170012AF RID: 4783
			// (get) Token: 0x06005F6D RID: 24429 RVA: 0x001BD124 File Offset: 0x001BB324
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=g6Ra6LUY}{ISSUE_OWNER.NAME} Needs Garrison Troops in {SETTLEMENT}", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170012B0 RID: 4784
			// (get) Token: 0x06005F6E RID: 24430 RVA: 0x001BD170 File Offset: 0x001BB370
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=BOAaF6x5}{ISSUE_OWNER.NAME} asks for help to increase troop levels in {SETTLEMENT}", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170012B1 RID: 4785
			// (get) Token: 0x06005F6F RID: 24431 RVA: 0x001BD1BC File Offset: 0x001BB3BC
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=sfFkYm0a}Your companion has successfully brought the troops {ISSUE_OWNER.LINK} requested. You received {REWARD}{GOLD_ICON}.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x06005F70 RID: 24432 RVA: 0x001BD214 File Offset: 0x001BB414
			public LordNeedsGarrisonTroopsIssue(Hero issueOwner, Settlement selectedSettlement)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._settlement = selectedSettlement;
				this._neededTroopType = CharacterHelper.GetTroopTree(base.IssueOwner.Culture.BasicTroop, 3f, 3f).GetRandomElementInefficiently<CharacterObject>();
			}

			// Token: 0x06005F71 RID: 24433 RVA: 0x001BD263 File Offset: 0x001BB463
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -0.5f;
				}
				return 0f;
			}

			// Token: 0x06005F72 RID: 24434 RVA: 0x001BD278 File Offset: 0x001BB478
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Leadership) >= hero.GetSkillValue(DefaultSkills.Steward)) ? DefaultSkills.Leadership : DefaultSkills.Steward, 120);
			}

			// Token: 0x06005F73 RID: 24435 RVA: 0x001BD2A5 File Offset: 0x001BB4A5
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 0, true);
			}

			// Token: 0x06005F74 RID: 24436 RVA: 0x001BD2B6 File Offset: 0x001BB4B6
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.IsMounted;
			}

			// Token: 0x06005F75 RID: 24437 RVA: 0x001BD2BE File Offset: 0x001BB4BE
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 0, true);
			}

			// Token: 0x170012B2 RID: 4786
			// (get) Token: 0x06005F76 RID: 24438 RVA: 0x001BD2D8 File Offset: 0x001BB4D8
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(800f + 900f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06005F77 RID: 24439 RVA: 0x001BD2ED File Offset: 0x001BB4ED
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				this.RelationshipChangeWithIssueOwner = 2;
			}

			// Token: 0x06005F78 RID: 24440 RVA: 0x001BD306 File Offset: 0x001BB506
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Common;
			}

			// Token: 0x06005F79 RID: 24441 RVA: 0x001BD30C File Offset: 0x001BB50C
			public override bool IssueStayAliveConditions()
			{
				bool flag = false;
				if (this._settlement.IsTown)
				{
					MobileParty garrisonParty = this._settlement.Town.GarrisonParty;
					flag = garrisonParty != null && garrisonParty.MemberRoster.TotalRegulars < 200;
				}
				else if (this._settlement.IsCastle)
				{
					MobileParty garrisonParty2 = this._settlement.Town.GarrisonParty;
					flag = garrisonParty2 != null && garrisonParty2.MemberRoster.TotalRegulars < 160;
				}
				return this._settlement.OwnerClan == base.IssueOwner.Clan && flag && !base.IssueOwner.IsDead && base.IssueOwner.Clan != Clan.PlayerClan;
			}

			// Token: 0x06005F7A RID: 24442 RVA: 0x001BD3CC File Offset: 0x001BB5CC
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flags, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				skill = null;
				relationHero = null;
				requiredGold = 0;
				flags = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flags |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (Hero.MainHero.IsKingdomLeader)
				{
					flags |= IssueBase.PreconditionFlags.MainHeroIsKingdomLeader;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flags |= IssueBase.PreconditionFlags.AtWar;
				}
				return flags == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06005F7B RID: 24443 RVA: 0x001BD436 File Offset: 0x001BB636
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06005F7C RID: 24444 RVA: 0x001BD438 File Offset: 0x001BB638
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005F7D RID: 24445 RVA: 0x001BD43A File Offset: 0x001BB63A
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(30f), this.RewardGold, this._settlement, this.NumberOfTroopToBeRecruited, this._neededTroopType);
			}

			// Token: 0x06005F7E RID: 24446 RVA: 0x001BD46A File Offset: 0x001BB66A
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -5;
			}

			// Token: 0x06005F7F RID: 24447 RVA: 0x001BD474 File Offset: 0x001BB674
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001EB9 RID: 7865
			private const int QuestDurationInDays = 30;

			// Token: 0x04001EBA RID: 7866
			private const int CompanionRequiredSkillLevel = 120;

			// Token: 0x04001EBB RID: 7867
			[SaveableField(60)]
			private Settlement _settlement;

			// Token: 0x04001EBC RID: 7868
			[SaveableField(30)]
			private CharacterObject _neededTroopType;
		}

		// Token: 0x02000766 RID: 1894
		public class LordNeedsGarrisonTroopsIssueQuest : QuestBase
		{
			// Token: 0x06005F80 RID: 24448 RVA: 0x001BD476 File Offset: 0x001BB676
			internal static void AutoGeneratedStaticCollectObjectsLordNeedsGarrisonTroopsIssueQuest(object o, List<object> collectedObjects)
			{
				((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005F81 RID: 24449 RVA: 0x001BD484 File Offset: 0x001BB684
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._requestedTroopType);
				collectedObjects.Add(this._playerStartsQuestLog);
			}

			// Token: 0x06005F82 RID: 24450 RVA: 0x001BD4A5 File Offset: 0x001BB6A5
			internal static object AutoGeneratedGetMemberValue_settlementStringID(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._settlementStringID;
			}

			// Token: 0x06005F83 RID: 24451 RVA: 0x001BD4B2 File Offset: 0x001BB6B2
			internal static object AutoGeneratedGetMemberValue_requestedTroopAmount(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._requestedTroopAmount;
			}

			// Token: 0x06005F84 RID: 24452 RVA: 0x001BD4C4 File Offset: 0x001BB6C4
			internal static object AutoGeneratedGetMemberValue_rewardGold(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._rewardGold;
			}

			// Token: 0x06005F85 RID: 24453 RVA: 0x001BD4D6 File Offset: 0x001BB6D6
			internal static object AutoGeneratedGetMemberValue_requestedTroopType(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._requestedTroopType;
			}

			// Token: 0x06005F86 RID: 24454 RVA: 0x001BD4E3 File Offset: 0x001BB6E3
			internal static object AutoGeneratedGetMemberValue_playerStartsQuestLog(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._playerStartsQuestLog;
			}

			// Token: 0x170012B3 RID: 4787
			// (get) Token: 0x06005F87 RID: 24455 RVA: 0x001BD4F0 File Offset: 0x001BB6F0
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=g6Ra6LUY}{ISSUE_OWNER.NAME} Needs Garrison Troops in {SETTLEMENT}", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170012B4 RID: 4788
			// (get) Token: 0x06005F88 RID: 24456 RVA: 0x001BD539 File Offset: 0x001BB739
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170012B5 RID: 4789
			// (get) Token: 0x06005F89 RID: 24457 RVA: 0x001BD53C File Offset: 0x001BB73C
			private TextObject PlayerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=FViaQrbV}{QUEST_GIVER.LINK}, the {?QUEST_GIVER.GENDER}lady{?}lord{\\?} of {QUEST_SETTLEMENT}, told you that {?QUEST_GIVER.GENDER}she{?}he{\\?} needs more troops in {?QUEST_GIVER.GENDER}her{?}his{\\?} garrison. {?QUEST_GIVER.GENDER}She{?}He{\\?} is willing to pay {REWARD}{GOLD_ICON} for your services. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to deliver {NUMBER_OF_TROOP_TO_BE_RECRUITED} {TROOP_TYPE} troops to garrison commander in {QUEST_SETTLEMENT}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TROOP_TYPE", this._requestedTroopType.Name);
					textObject.SetTextVariable("REWARD", this._rewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("NUMBER_OF_TROOP_TO_BE_RECRUITED", this._requestedTroopAmount);
					textObject.SetTextVariable("QUEST_SETTLEMENT", this._settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170012B6 RID: 4790
			// (get) Token: 0x06005F8A RID: 24458 RVA: 0x001BD5D4 File Offset: 0x001BB7D4
			private TextObject SuccessQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=UEn466Y6}You have successfully brought the troops {QUEST_GIVER.LINK} requested. You received {REWARD} gold in return for your service.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this._rewardGold);
					return textObject;
				}
			}

			// Token: 0x170012B7 RID: 4791
			// (get) Token: 0x06005F8B RID: 24459 RVA: 0x001BD618 File Offset: 0x001BB818
			private TextObject QuestGiverLostTheSettlementLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=zS68eOsl}{QUEST_GIVER.LINK} has lost {SETTLEMENT} and your agreement with {?QUEST_GIVER.GENDER}her{?}his{\\?} canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170012B8 RID: 4792
			// (get) Token: 0x06005F8C RID: 24460 RVA: 0x001BD664 File Offset: 0x001BB864
			private TextObject QuestFailedWarDeclaredLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=JIWVeTMD}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} was canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170012B9 RID: 4793
			// (get) Token: 0x06005F8D RID: 24461 RVA: 0x001BD6B0 File Offset: 0x001BB8B0
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170012BA RID: 4794
			// (get) Token: 0x06005F8E RID: 24462 RVA: 0x001BD6E2 File Offset: 0x001BB8E2
			private TextObject TimeOutLogText
			{
				get
				{
					return new TextObject("{=cnaxgN5b}You have failed to bring the troops in time.", null);
				}
			}

			// Token: 0x06005F8F RID: 24463 RVA: 0x001BD6F0 File Offset: 0x001BB8F0
			public LordNeedsGarrisonTroopsIssueQuest(string questId, Hero giverHero, CampaignTime duration, int rewardGold, Settlement selectedSettlement, int requestedTroopAmount, CharacterObject requestedTroopType)
				: base(questId, giverHero, duration, rewardGold)
			{
				this._settlement = selectedSettlement;
				this._settlementStringID = selectedSettlement.StringId;
				this._requestedTroopAmount = requestedTroopAmount;
				this._collectedTroopAmount = 0;
				this._requestedTroopType = requestedTroopType;
				this._rewardGold = rewardGold;
				this.SetDialogs();
				base.AddTrackedObject(this._settlement);
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06005F90 RID: 24464 RVA: 0x001BD754 File Offset: 0x001BB954
			private bool DialogCondition()
			{
				return Hero.OneToOneConversationHero == base.QuestGiver;
			}

			// Token: 0x06005F91 RID: 24465 RVA: 0x001BD764 File Offset: 0x001BB964
			protected override void SetDialogs()
			{
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetGarrisonCommanderDialogFlow(), this);
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=9iZg4vpz}Thank you. You will be rewarded when you are done.[if:convo_mocking_aristocratic]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=o6BunhbE}Have you brought my troops?[if:convo_undecided_open]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition))
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
					})
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=eC4laxrj}I'm still out recruiting.", null), null, null, null)
					.NpcLine(new TextObject("{=TxxbCbUc}Good. I have faith in you...[if:convo_mocking_aristocratic]", null), null, null, null, null)
					.CloseDialog()
					.PlayerOption(new TextObject("{=DbraLcwM}I need more time to find proper men.", null), null, null, null)
					.NpcLine(new TextObject("{=Mw5bJ5Fb}Every day without a proper garrison is a day that we're vulnerable. Do hurry, if you can.[if:convo_normal]", null), null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions();
			}

			// Token: 0x06005F92 RID: 24466 RVA: 0x001BD893 File Offset: 0x001BBA93
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				this._playerStartsQuestLog = base.AddDiscreteLog(this.PlayerStartsQuestLogText, new TextObject("{=WIb9VvEM}Collected Troops", null), this._collectedTroopAmount, this._requestedTroopAmount, null, false);
			}

			// Token: 0x06005F93 RID: 24467 RVA: 0x001BD8C8 File Offset: 0x001BBAC8
			private DialogFlow GetGarrisonCommanderDialogFlow()
			{
				TextObject textObject = new TextObject("{=abda9slW}We were waiting for you, {?PLAYER.GENDER}madam{?}sir{\\?}. Have you brought the troops that our {?ISSUE_OWNER.GENDER}lady{?}lord{\\?} requested?", null);
				StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.QuestGiver.CharacterObject, textObject, false);
				return DialogFlow.CreateDialogFlow("start", 300).NpcLine(textObject, null, null, null, null).Condition(() => CharacterObject.OneToOneConversationCharacter == this._selectedCharacterToTalk)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=ooHbl6JU}Here are your men.", null), null, null, null)
					.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.PlayerGiveTroopsToGarrisonCommanderCondition))
					.NpcLine(new TextObject("{=Ouy4sN5b}Thank you.[if:convo_mocking_aristocratic]", null), null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerTransferredTroopsToGarrisonCommander;
					})
					.CloseDialog()
					.PlayerOption(new TextObject("{=G5tyQj6N}Not yet.", null), null, null, null)
					.NpcLine(new TextObject("{=yPOZd1wb}Very well. We'll keep waiting.[if:convo_normal]", null), null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions();
			}

			// Token: 0x06005F94 RID: 24468 RVA: 0x001BD9AC File Offset: 0x001BBBAC
			private void PlayerTransferredTroopsToGarrisonCommander()
			{
				using (List<TroopRosterElement>.Enumerator enumerator = MobileParty.MainParty.MemberRoster.GetTroopRoster().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Character == this._requestedTroopType)
						{
							MobileParty.MainParty.MemberRoster.AddToCounts(this._requestedTroopType, -this._requestedTroopAmount, false, 0, 0, true, -1);
							break;
						}
					}
				}
				base.AddLog(this.SuccessQuestLogText, false);
				this.RelationshipChangeWithQuestGiver = 2;
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06005F95 RID: 24469 RVA: 0x001BDA70 File Offset: 0x001BBC70
			private bool PlayerGiveTroopsToGarrisonCommanderCondition(out TextObject explanation)
			{
				int num = 0;
				foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character == this._requestedTroopType)
					{
						num = troopRosterElement.Number;
						break;
					}
				}
				if (num < this._requestedTroopAmount)
				{
					explanation = new TextObject("{=VFO2aQ4l}You don't have enough men.", null);
					return false;
				}
				explanation = null;
				return true;
			}

			// Token: 0x06005F96 RID: 24470 RVA: 0x001BDAFC File Offset: 0x001BBCFC
			protected override void InitializeQuestOnGameLoad()
			{
				this._settlement = Settlement.Find(this._settlementStringID);
				this.CalculateTroopAmount();
				this.SetDialogs();
			}

			// Token: 0x06005F97 RID: 24471 RVA: 0x001BDB1C File Offset: 0x001BBD1C
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.OnPartySizeChangedEvent.AddNonSerializedListener(this, new Action<PartyBase>(this.OnPartySizeChanged));
			}

			// Token: 0x06005F98 RID: 24472 RVA: 0x001BDB9C File Offset: 0x001BBD9C
			private void OnPartySizeChanged(PartyBase party)
			{
				if (party.IsMobile && party.MobileParty.IsMainParty)
				{
					this.CalculateTroopAmount();
					this._collectedTroopAmount = MBMath.ClampInt(this._collectedTroopAmount, 0, this._requestedTroopAmount);
					this._playerStartsQuestLog.UpdateCurrentProgress(this._collectedTroopAmount);
				}
			}

			// Token: 0x06005F99 RID: 24473 RVA: 0x001BDBED File Offset: 0x001BBDED
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06005F9A RID: 24474 RVA: 0x001BDC00 File Offset: 0x001BBE00
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement == this._settlement && this._settlement.OwnerClan != base.QuestGiver.Clan)
				{
					base.AddLog(this.QuestGiverLostTheSettlementLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06005F9B RID: 24475 RVA: 0x001BDC38 File Offset: 0x001BBE38
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					this.CalculateTroopAmount();
					this._collectedTroopAmount = MBMath.ClampInt(this._collectedTroopAmount, 0, this._requestedTroopAmount);
					this._playerStartsQuestLog.UpdateCurrentProgress(this._collectedTroopAmount);
				}
			}

			// Token: 0x06005F9C RID: 24476 RVA: 0x001BDC74 File Offset: 0x001BBE74
			private void CalculateTroopAmount()
			{
				foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character == this._requestedTroopType)
					{
						this._collectedTroopAmount = MobileParty.MainParty.MemberRoster.GetTroopCount(troopRosterElement.Character);
						break;
					}
				}
			}

			// Token: 0x06005F9D RID: 24477 RVA: 0x001BDCF4 File Offset: 0x001BBEF4
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestFailedWarDeclaredLogText);
				}
			}

			// Token: 0x06005F9E RID: 24478 RVA: 0x001BDD1E File Offset: 0x001BBF1E
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestFailedWarDeclaredLogText, false);
			}

			// Token: 0x06005F9F RID: 24479 RVA: 0x001BDD36 File Offset: 0x001BBF36
			protected override void OnTimedOut()
			{
				base.AddLog(this.TimeOutLogText, false);
				this.RelationshipChangeWithQuestGiver = -5;
			}

			// Token: 0x04001EBD RID: 7869
			internal Settlement _settlement;

			// Token: 0x04001EBE RID: 7870
			[SaveableField(10)]
			private string _settlementStringID;

			// Token: 0x04001EBF RID: 7871
			private int _collectedTroopAmount;

			// Token: 0x04001EC0 RID: 7872
			[SaveableField(20)]
			private int _requestedTroopAmount;

			// Token: 0x04001EC1 RID: 7873
			[SaveableField(30)]
			private int _rewardGold;

			// Token: 0x04001EC2 RID: 7874
			[SaveableField(40)]
			private CharacterObject _requestedTroopType;

			// Token: 0x04001EC3 RID: 7875
			internal CharacterObject _selectedCharacterToTalk;

			// Token: 0x04001EC4 RID: 7876
			[SaveableField(50)]
			private JournalLog _playerStartsQuestLog;
		}

		// Token: 0x02000767 RID: 1895
		public class LordNeedsGarrisonTroopsIssueQuestTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06005FA2 RID: 24482 RVA: 0x001BDD7A File Offset: 0x001BBF7A
			public LordNeedsGarrisonTroopsIssueQuestTypeDefiner()
				: base(5080000)
			{
			}

			// Token: 0x06005FA3 RID: 24483 RVA: 0x001BDD87 File Offset: 0x001BBF87
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue), 1, null);
				base.AddClassDefinition(typeof(LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest), 2, null);
			}
		}
	}
}
