using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
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
	// Token: 0x0200038D RID: 909
	public class MerchantNeedsHelpWithOutlawsIssueQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x060035C4 RID: 13764 RVA: 0x000DBDCA File Offset: 0x000D9FCA
		private static float ValidBanditPartyDistance
		{
			get
			{
				return MerchantNeedsHelpWithOutlawsIssueQuestBehavior.NeededHideoutDistanceToSpawnTheQuest * 0.75f;
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x000DBDD7 File Offset: 0x000D9FD7
		private static float NeededHideoutDistanceToSpawnTheQuest
		{
			get
			{
				return Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay;
			}
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000DBDEC File Offset: 0x000D9FEC
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameEarlyLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameEarlyLoaded));
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000DBE3E File Offset: 0x000DA03E
		private void OnGameEarlyLoaded(CampaignGameStarter obj)
		{
			this.InitializeCache();
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000DBE46 File Offset: 0x000DA046
		private void OnNewGameCreated(CampaignGameStarter obj)
		{
			this.InitializeCache();
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000DBE50 File Offset: 0x000DA050
		private void InitializeCache()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown || settlement.IsVillage)
				{
					foreach (Hideout hideout in Hideout.All)
					{
						if (Campaign.Current.Models.MapDistanceModel.GetDistance(hideout.Settlement, settlement, false, false, MobileParty.NavigationType.Default) < Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default) * 1.25f)
						{
							if (!this._closestHideoutsToSettlements.ContainsKey(settlement))
							{
								this._closestHideoutsToSettlements.Add(settlement, new List<Hideout> { hideout });
							}
							else
							{
								this._closestHideoutsToSettlements[settlement].Add(hideout);
							}
						}
					}
				}
			}
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000DBF5C File Offset: 0x000DA15C
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000DBF60 File Offset: 0x000DA160
		private bool ConditionsHold(Hero issueGiver, out Hideout hideout)
		{
			hideout = null;
			List<Hideout> list;
			if ((issueGiver.IsMerchant || issueGiver.IsRuralNotable) && this._closestHideoutsToSettlements.TryGetValue(issueGiver.CurrentSettlement, out list))
			{
				foreach (Hideout hideout2 in list)
				{
					if (hideout2.IsInfested && !hideout2.Settlement.IsSettlementBusy(this))
					{
						hideout = hideout2;
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000DBFF0 File Offset: 0x000DA1F0
		public void OnCheckForIssue(Hero hero)
		{
			Hideout hideout;
			if (this.ConditionsHold(hero, out hideout))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssue), IssueBase.IssueFrequency.VeryCommon, hideout));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssue), IssueBase.IssueFrequency.VeryCommon));
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000DC058 File Offset: 0x000DA258
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			Hideout hideout = potentialIssueData.RelatedObject as Hideout;
			return new MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssue(issueOwner, hideout);
		}

		// Token: 0x04000F1E RID: 3870
		private const IssueBase.IssueFrequency MerchantNeedsHelpWithOutlawsIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x04000F1F RID: 3871
		private readonly Dictionary<Settlement, List<Hideout>> _closestHideoutsToSettlements = new Dictionary<Settlement, List<Hideout>>();

		// Token: 0x02000776 RID: 1910
		public class MerchantNeedsHelpWithOutlawsIssue : IssueBase
		{
			// Token: 0x0600612E RID: 24878 RVA: 0x001C476C File Offset: 0x001C296C
			internal static void AutoGeneratedStaticCollectObjectsMerchantNeedsHelpWithOutlawsIssue(object o, List<object> collectedObjects)
			{
				((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600612F RID: 24879 RVA: 0x001C477A File Offset: 0x001C297A
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this.RelatedHideout);
			}

			// Token: 0x06006130 RID: 24880 RVA: 0x001C478F File Offset: 0x001C298F
			internal static object AutoGeneratedGetMemberValueRelatedHideout(object o)
			{
				return ((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssue)o).RelatedHideout;
			}

			// Token: 0x17001327 RID: 4903
			// (get) Token: 0x06006131 RID: 24881 RVA: 0x001C479C File Offset: 0x001C299C
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x17001328 RID: 4904
			// (get) Token: 0x06006132 RID: 24882 RVA: 0x001C47A0 File Offset: 0x001C29A0
			private int TotalPartyCount
			{
				get
				{
					return (int)(2f + 6f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17001329 RID: 4905
			// (get) Token: 0x06006133 RID: 24883 RVA: 0x001C47B5 File Offset: 0x001C29B5
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 8 + MathF.Ceiling(11f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700132A RID: 4906
			// (get) Token: 0x06006134 RID: 24884 RVA: 0x001C47CA File Offset: 0x001C29CA
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 5 + MathF.Ceiling(7f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700132B RID: 4907
			// (get) Token: 0x06006135 RID: 24885 RVA: 0x001C47DF File Offset: 0x001C29DF
			protected override int RewardGold
			{
				get
				{
					return (int)(400f + 1500f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700132C RID: 4908
			// (get) Token: 0x06006136 RID: 24886 RVA: 0x001C47F4 File Offset: 0x001C29F4
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=ib6ltlM0}Yes... We've always had trouble with bandits, but recently we've had a lot more than our share. The hills outside of town are infested. A lot of us are afraid to take their goods to market. Some have been murdered. People tell me, 'I'm getting so desperate, maybe I'll turn bandit myself.' It's bad...[ib:demure2][if:convo_dismayed]", null);
				}
			}

			// Token: 0x1700132D RID: 4909
			// (get) Token: 0x06006137 RID: 24887 RVA: 0x001C4801 File Offset: 0x001C2A01
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=qNxdWLFY}So you want me to hunt them down?", null);
				}
			}

			// Token: 0x1700132E RID: 4910
			// (get) Token: 0x06006138 RID: 24888 RVA: 0x001C4810 File Offset: 0x001C2A10
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=DlRMT7XD}Well, {?PLAYER.GENDER}my lady{?}sir{\\?}, you'll never get all those outlaws,[if:convo_thinking] but if word gets around that you took down some of the most vicious ones - let's say {TOTAL_COUNT} bands of brigands - robbing us wouldn't seem so lucrative. Maybe the rest would go bother someone else... Do you think you can help us?", null);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					textObject.SetTextVariable("TOTAL_COUNT", this.TotalPartyCount);
					return textObject;
				}
			}

			// Token: 0x1700132F RID: 4911
			// (get) Token: 0x06006139 RID: 24889 RVA: 0x001C484E File Offset: 0x001C2A4E
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=5RjvnQ3d}I bet even a party of {ALTERNATIVE_COUNT} properly trained men accompanied by one of your lieutenants can handle any band they find. Give them {TOTAL_DAYS} days, say... That will make a difference.[if:convo_undecided_open]", null);
					textObject.SetTextVariable("ALTERNATIVE_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("TOTAL_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17001330 RID: 4912
			// (get) Token: 0x0600613A RID: 24890 RVA: 0x001C487F File Offset: 0x001C2A7F
			public override TextObject IssuePlayerResponseAfterAlternativeExplanation
			{
				get
				{
					return new TextObject("{=BPfuSkCl}That depends. How many men do you think are required to get the job done?", null);
				}
			}

			// Token: 0x17001331 RID: 4913
			// (get) Token: 0x0600613B RID: 24891 RVA: 0x001C488C File Offset: 0x001C2A8C
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=2ApU6iCB}I'll hunt down {TOTAL_COUNT} bands of brigands for you.", null);
					textObject.SetTextVariable("TOTAL_COUNT", this.TotalPartyCount);
					return textObject;
				}
			}

			// Token: 0x17001332 RID: 4914
			// (get) Token: 0x0600613C RID: 24892 RVA: 0x001C48AB File Offset: 0x001C2AAB
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=DLbFbYkR}I will have one of my companions and {ALTERNATIVE_COUNT} of my men patrol the area for {TOTAL_DAYS} days.", null);
					textObject.SetTextVariable("ALTERNATIVE_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("TOTAL_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17001333 RID: 4915
			// (get) Token: 0x0600613D RID: 24893 RVA: 0x001C48DC File Offset: 0x001C2ADC
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=PexmGuOd}{?PLAYER.GENDER}Madam{?}Sir{\\?}, I am happy to tell that the men you left are patrolling, and already we feel safer. Thank you again.[ib:demure][if:convo_grateful]", null);
				}
			}

			// Token: 0x17001334 RID: 4916
			// (get) Token: 0x0600613E RID: 24894 RVA: 0x001C48EC File Offset: 0x001C2AEC
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=FYfZFve3}Thank you, {?PLAYER.GENDER}my lady{?}my lord{\\?}. Hopefully, we can travel safely again.", null);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001335 RID: 4917
			// (get) Token: 0x0600613F RID: 24895 RVA: 0x001C4918 File Offset: 0x001C2B18
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001336 RID: 4918
			// (get) Token: 0x06006140 RID: 24896 RVA: 0x001C491B File Offset: 0x001C2B1B
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001337 RID: 4919
			// (get) Token: 0x06006141 RID: 24897 RVA: 0x001C491E File Offset: 0x001C2B1E
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(600f + 800f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17001338 RID: 4920
			// (get) Token: 0x06006142 RID: 24898 RVA: 0x001C4934 File Offset: 0x001C2B34
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=Bdt41knf}You have accepted {QUEST_GIVER.LINK}'s request to find at least {TOTAL_COUNT} different parties of brigands around {QUEST_SETTLEMENT} and sent {COMPANION.LINK} and with {?COMPANION.GENDER}her{?}his{\\?} {ALTERNATIVE_COUNT} of your men to deal with them. They should return with the reward of {GOLD_AMOUNT}{GOLD_ICON} denars as promised by {QUEST_GIVER.LINK} after dealing with them in {RETURN_DAYS} days.", null);
					textObject.SetTextVariable("TOTAL_COUNT", this.TotalPartyCount);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("ALTERNATIVE_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("GOLD_AMOUNT", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17001339 RID: 4921
			// (get) Token: 0x06006143 RID: 24899 RVA: 0x001C49FC File Offset: 0x001C2BFC
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=ABmCO23x}{QUEST_GIVER.NAME} Needs Help With Brigands", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700133A RID: 4922
			// (get) Token: 0x06006144 RID: 24900 RVA: 0x001C4A2E File Offset: 0x001C2C2E
			public override TextObject Description
			{
				get
				{
					return new TextObject("{=sAobCa9U}Brigands are disturbing travelers outside the town. Someone needs to hunt them down.", null);
				}
			}

			// Token: 0x06006145 RID: 24901 RVA: 0x001C4A3B File Offset: 0x001C2C3B
			public MerchantNeedsHelpWithOutlawsIssue(Hero issueOwner, Hideout relatedHideout)
				: base(issueOwner, CampaignTime.DaysFromNow(15f))
			{
				this.RelatedHideout = relatedHideout;
			}

			// Token: 0x06006146 RID: 24902 RVA: 0x001C4A55 File Offset: 0x001C2C55
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementProsperity)
				{
					return -0.2f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.1f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -1f;
				}
				return 0f;
			}

			// Token: 0x06006147 RID: 24903 RVA: 0x001C4A86 File Offset: 0x001C2C86
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Tactics) >= hero.GetSkillValue(DefaultSkills.Scouting)) ? DefaultSkills.Tactics : DefaultSkills.Scouting, 120);
			}

			// Token: 0x06006148 RID: 24904 RVA: 0x001C4AB3 File Offset: 0x001C2CB3
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x06006149 RID: 24905 RVA: 0x001C4AC4 File Offset: 0x001C2CC4
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x0600614A RID: 24906 RVA: 0x001C4ADE File Offset: 0x001C2CDE
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x0600614B RID: 24907 RVA: 0x001C4AEC File Offset: 0x001C2CEC
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.RelationshipChangeWithIssueOwner = 3;
				if (base.IssueOwner.CurrentSettlement.IsVillage && base.IssueOwner.CurrentSettlement.Village.TradeBound != null)
				{
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security += 5f;
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Prosperity += 5f;
				}
				else if (base.IssueOwner.CurrentSettlement.IsTown)
				{
					base.IssueOwner.CurrentSettlement.Town.Security += 5f;
					base.IssueOwner.CurrentSettlement.Town.Prosperity += 5f;
				}
				Hero.MainHero.Clan.AddRenown(1f, true);
			}

			// Token: 0x0600614C RID: 24908 RVA: 0x001C4BEC File Offset: 0x001C2DEC
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				if (base.IssueOwner.CurrentSettlement.IsVillage)
				{
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Prosperity -= 10f;
				}
				else if (base.IssueOwner.CurrentSettlement.IsTown)
				{
					base.IssueOwner.CurrentSettlement.Town.Prosperity -= 10f;
				}
				this.RelationshipChangeWithIssueOwner = -5;
			}

			// Token: 0x0600614D RID: 24909 RVA: 0x001C4C73 File Offset: 0x001C2E73
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x0600614E RID: 24910 RVA: 0x001C4C78 File Offset: 0x001C2E78
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				flag = IssueBase.PreconditionFlags.None;
				relationHero = null;
				requiredGold = 0;
				skill = null;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					flag |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x0600614F RID: 24911 RVA: 0x001C4CE8 File Offset: 0x001C2EE8
			public override bool IssueStayAliveConditions()
			{
				return !base.IssueOwner.CurrentSettlement.IsRaided && !base.IssueOwner.CurrentSettlement.IsUnderRaid && this.RelatedHideout != null && this.RelatedHideout.IsInfested;
			}

			// Token: 0x06006150 RID: 24912 RVA: 0x001C4D23 File Offset: 0x001C2F23
			protected override void OnGameLoad()
			{
				if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.9", 0) && this.RelatedHideout == null)
				{
					base.CompleteIssueWithCancel(null);
				}
			}

			// Token: 0x06006151 RID: 24913 RVA: 0x001C4D52 File Offset: 0x001C2F52
			public override void IsSettlementBusy(Settlement settlement, object asker, ref int priority)
			{
				if (asker != this && settlement == this.RelatedHideout.Settlement)
				{
					priority = Math.Max(priority, 100);
				}
			}

			// Token: 0x06006152 RID: 24914 RVA: 0x001C4D71 File Offset: 0x001C2F71
			protected override void HourlyTick()
			{
			}

			// Token: 0x06006153 RID: 24915 RVA: 0x001C4D73 File Offset: 0x001C2F73
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(20f), this.RewardGold, this.TotalPartyCount, this.RelatedHideout);
			}

			// Token: 0x06006154 RID: 24916 RVA: 0x001C4D9D File Offset: 0x001C2F9D
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x06006155 RID: 24917 RVA: 0x001C4D9F File Offset: 0x001C2F9F
			protected override void OnIssueFinalized()
			{
			}

			// Token: 0x04001F05 RID: 7941
			private const int IssueDuration = 15;

			// Token: 0x04001F06 RID: 7942
			private const int QuestTimeLimit = 20;

			// Token: 0x04001F07 RID: 7943
			private const int MinimumRequiredMenCount = 5;

			// Token: 0x04001F08 RID: 7944
			private const int AlternativeSolutionMinimumSkillValue = 120;

			// Token: 0x04001F09 RID: 7945
			private const int AlternativeSolutionTroopTierRequirement = 2;

			// Token: 0x04001F0A RID: 7946
			[SaveableField(10)]
			private Hideout RelatedHideout;
		}

		// Token: 0x02000777 RID: 1911
		public class MerchantNeedsHelpWithOutlawsIssueQuest : QuestBase
		{
			// Token: 0x06006156 RID: 24918 RVA: 0x001C4DA1 File Offset: 0x001C2FA1
			internal static void AutoGeneratedStaticCollectObjectsMerchantNeedsHelpWithOutlawsIssueQuest(object o, List<object> collectedObjects)
			{
				((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006157 RID: 24919 RVA: 0x001C4DAF File Offset: 0x001C2FAF
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._validPartiesList);
				collectedObjects.Add(this._relatedHideout);
				collectedObjects.Add(this._questProgressLogTest);
			}

			// Token: 0x06006158 RID: 24920 RVA: 0x001C4DDC File Offset: 0x001C2FDC
			internal static object AutoGeneratedGetMemberValue_totalPartyCount(object o)
			{
				return ((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest)o)._totalPartyCount;
			}

			// Token: 0x06006159 RID: 24921 RVA: 0x001C4DEE File Offset: 0x001C2FEE
			internal static object AutoGeneratedGetMemberValue_destroyedPartyCount(object o)
			{
				return ((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest)o)._destroyedPartyCount;
			}

			// Token: 0x0600615A RID: 24922 RVA: 0x001C4E00 File Offset: 0x001C3000
			internal static object AutoGeneratedGetMemberValue_recruitedPartyCount(object o)
			{
				return ((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest)o)._recruitedPartyCount;
			}

			// Token: 0x0600615B RID: 24923 RVA: 0x001C4E12 File Offset: 0x001C3012
			internal static object AutoGeneratedGetMemberValue_validPartiesList(object o)
			{
				return ((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest)o)._validPartiesList;
			}

			// Token: 0x0600615C RID: 24924 RVA: 0x001C4E1F File Offset: 0x001C301F
			internal static object AutoGeneratedGetMemberValue_relatedHideout(object o)
			{
				return ((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest)o)._relatedHideout;
			}

			// Token: 0x0600615D RID: 24925 RVA: 0x001C4E2C File Offset: 0x001C302C
			internal static object AutoGeneratedGetMemberValue_questProgressLogTest(object o)
			{
				return ((MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest)o)._questProgressLogTest;
			}

			// Token: 0x1700133B RID: 4923
			// (get) Token: 0x0600615E RID: 24926 RVA: 0x001C4E3C File Offset: 0x001C303C
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=PBGiIbEM}{ISSUE_GIVER.NAME} Needs Help With Brigands", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700133C RID: 4924
			// (get) Token: 0x0600615F RID: 24927 RVA: 0x001C4E6E File Offset: 0x001C306E
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700133D RID: 4925
			// (get) Token: 0x06006160 RID: 24928 RVA: 0x001C4E71 File Offset: 0x001C3071
			private int _questPartyProgress
			{
				get
				{
					return this._destroyedPartyCount + this._recruitedPartyCount;
				}
			}

			// Token: 0x1700133E RID: 4926
			// (get) Token: 0x06006161 RID: 24929 RVA: 0x001C4E80 File Offset: 0x001C3080
			private TextObject PlayerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=6iLxrDBa}You have accepted {QUEST_GIVER.LINK}'s request to find at least {TOTAL_COUNT} different parties of brigands around {QUEST_SETTLEMENT} and decided to hunt them down personally. {?QUEST_GIVER.GENDER}She{?}He{\\?} will reward you {AMOUNT}{GOLD_ICON} gold once you have dealt with them.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TOTAL_COUNT", this._totalPartyCount);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("AMOUNT", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x1700133F RID: 4927
			// (get) Token: 0x06006162 RID: 24930 RVA: 0x001C4F04 File Offset: 0x001C3104
			private TextObject SuccessQuestLogText1
			{
				get
				{
					TextObject textObject = new TextObject("{=cQ6CzXKM}You have defeated all the brigands as {QUEST_GIVER.LINK} has asked. {?QUEST_GIVER.GENDER}She{?}He{\\?} is grateful. And sends you the reward, {GOLD_AMOUNT}{GOLD_ICON} gold as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.Name);
					textObject.SetTextVariable("GOLD_AMOUNT", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x17001340 RID: 4928
			// (get) Token: 0x06006163 RID: 24931 RVA: 0x001C4F78 File Offset: 0x001C3178
			private TextObject SuccessQuestLogText2
			{
				get
				{
					TextObject textObject = new TextObject("{=dSHgU9gD}You have defeated some of the brigands and recruited the rest into your party. {QUEST_GIVER.LINK} is grateful and sends you the {GOLD_AMOUNT}{GOLD_ICON} as promised. ", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("GOLD_AMOUNT", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x17001341 RID: 4929
			// (get) Token: 0x06006164 RID: 24932 RVA: 0x001C4FD0 File Offset: 0x001C31D0
			private TextObject SuccessQuestLogText3
			{
				get
				{
					TextObject textObject = new TextObject("{=3V5udYJO}You have recruited the brigands into your party. {QUEST_GIVER.LINK} finds your solution acceptable and sends you the {GOLD_AMOUNT}{GOLD_ICON} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("GOLD_AMOUNT", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x17001342 RID: 4930
			// (get) Token: 0x06006165 RID: 24933 RVA: 0x001C5028 File Offset: 0x001C3228
			private TextObject TimeoutLog
			{
				get
				{
					TextObject textObject = new TextObject("{=Tcux6Sru}You have failed to defeat all {TOTAL_COUNT} outlaw parties in time as {QUEST_GIVER.LINK} asked. {?QUEST_GIVER.GENDER}She{?}He{\\?} is very disappointed.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TOTAL_COUNT", this._totalPartyCount);
					return textObject;
				}
			}

			// Token: 0x17001343 RID: 4931
			// (get) Token: 0x06006166 RID: 24934 RVA: 0x001C506C File Offset: 0x001C326C
			private TextObject QuestGiverVillageRaided
			{
				get
				{
					TextObject textObject = new TextObject("{=4rCIZ6e5}{QUEST_SETTLEMENT} was raided, Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17001344 RID: 4932
			// (get) Token: 0x06006167 RID: 24935 RVA: 0x001C50BC File Offset: 0x001C32BC
			private TextObject QuestCanceledWarDeclaredLog
			{
				get
				{
					TextObject textObject = new TextObject("{=vW6kBki9}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001345 RID: 4933
			// (get) Token: 0x06006168 RID: 24936 RVA: 0x001C50F0 File Offset: 0x001C32F0
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=DDur6mHb}Your actions have started a war with {ISSUE_GIVER.LINK}'s faction. Your agreement with {ISSUE_GIVER.LINK} is failed.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06006169 RID: 24937 RVA: 0x001C5124 File Offset: 0x001C3324
			public MerchantNeedsHelpWithOutlawsIssueQuest(string questId, Hero giverHero, CampaignTime duration, int rewardGold, int totalPartyCount, Hideout relatedHideout)
				: base(questId, giverHero, duration, rewardGold)
			{
				this._totalPartyCount = totalPartyCount;
				this._destroyedPartyCount = 0;
				this._recruitedPartyCount = 0;
				this._validPartiesList = new List<MobileParty>();
				this._relatedHideout = relatedHideout;
				this.AddHideoutPartiesToValidPartiesList();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x0600616A RID: 24938 RVA: 0x001C5178 File Offset: 0x001C3378
			protected override void SetDialogs()
			{
				TextObject textObject = new TextObject("{=PQIYPCDn}Very good. I will be waiting for the good news then. Once you return, I'm ready to offer a reward of {REWARD_GOLD}{GOLD_ICON} denars. Just make sure that you defeat at least {TROOP_COUNT} bands no more than a day's ride away from here.[ib:normal][if:convo_bemused]", null);
				textObject.SetTextVariable("REWARD_GOLD", this.RewardGold);
				textObject.SetTextVariable("TROOP_COUNT", this._totalPartyCount);
				textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(textObject, null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=jjTcNhKE}Have you been able to find any bandits yet?[if:convo_undecided_open]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=mU45Th70}We're off to hunt them now.", null), null, null, null)
					.NpcLine(new TextObject("{=u9vtceCV}You are a savior.[if:convo_astonished]", null), null, null, null, null)
					.CloseDialog()
					.Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.PlayerOption(new TextObject("{=QPv1b7f8}I haven't had the time yet.", null), null, null, null)
					.NpcLine(new TextObject("{=6ba4n9n6}We are waiting for your good news {?PLAYER.GENDER}my lady{?}sir{\\?}.[if:convo_focused_happy]", null), null, null, null, null)
					.CloseDialog()
					.Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x0600616B RID: 24939 RVA: 0x001C52CB File Offset: 0x001C34CB
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				this._questProgressLogTest = base.AddDiscreteLog(this.PlayerStartsQuestLogText, new TextObject("{=HzcLsnYn}Destroyed parties", null), this._destroyedPartyCount, this._totalPartyCount, null, true);
			}

			// Token: 0x0600616C RID: 24940 RVA: 0x001C5300 File Offset: 0x001C3500
			private void AddQuestStepLog()
			{
				this._questProgressLogTest.UpdateCurrentProgress(this._questPartyProgress);
				if (this._questPartyProgress >= this._totalPartyCount)
				{
					this.SuccessConsequences();
					return;
				}
				TextObject textObject = new TextObject("{=xbVCRbUu}You hunted {CURRENT_COUNT}/{TOTAL_COUNT} gang of brigands.", null);
				textObject.SetTextVariable("CURRENT_COUNT", this._questPartyProgress);
				textObject.SetTextVariable("TOTAL_COUNT", this._totalPartyCount);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}

			// Token: 0x0600616D RID: 24941 RVA: 0x001C536F File Offset: 0x001C356F
			protected override void HourlyTick()
			{
			}

			// Token: 0x0600616E RID: 24942 RVA: 0x001C5374 File Offset: 0x001C3574
			protected override void RegisterEvents()
			{
				CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.MobilePartyDestroyed));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.VillageBeingRaided.AddNonSerializedListener(this, new Action<Village>(this.OnVillageRaided));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.BanditPartyRecruited.AddNonSerializedListener(this, new Action<MobileParty>(this.OnBanditPartyRecruited));
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
				CampaignEvents.IsSettlementBusyEvent.AddNonSerializedListener(this, new ReferenceAction<Settlement, object, int>(this.IsSettlementBusy));
			}

			// Token: 0x0600616F RID: 24943 RVA: 0x001C5467 File Offset: 0x001C3667
			private void IsSettlementBusy(Settlement settlement, object asker, ref int priority)
			{
				if (asker != this && settlement == this._relatedHideout.Settlement)
				{
					priority = Math.Max(priority, 200);
				}
			}

			// Token: 0x06006170 RID: 24944 RVA: 0x001C548C File Offset: 0x001C368C
			private void OnGameLoadFinished()
			{
				if (this._relatedHideout == null && MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.9", 0))
				{
					Hideout hideout = SettlementHelper.FindNearestHideoutToMobileParty(MobileParty.MainParty, MobileParty.NavigationType.Default, (Settlement x) => x.Hideout.IsInfested);
					if (hideout != null && Campaign.Current.Models.MapDistanceModel.GetDistance(base.QuestGiver.CurrentSettlement, hideout.Settlement, false, false, MobileParty.NavigationType.Default) < Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default) * 1.25f)
					{
						this._relatedHideout = hideout;
					}
					if (this._relatedHideout != null)
					{
						this.AddHideoutPartiesToValidPartiesList();
					}
					else
					{
						base.CompleteQuestWithCancel(null);
					}
				}
				if (this._relatedHideout != null && base.IsOngoing && MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.9", 0) && this._relatedHideout.Settlement.IsSettlementBusy(this))
				{
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06006171 RID: 24945 RVA: 0x001C5598 File Offset: 0x001C3798
			private void AddHideoutPartiesToValidPartiesList()
			{
				foreach (MobileParty mobileParty in this._relatedHideout.Settlement.Parties)
				{
					if (mobileParty.IsBandit)
					{
						this._validPartiesList.Add(mobileParty);
					}
				}
			}

			// Token: 0x06006172 RID: 24946 RVA: 0x001C5604 File Offset: 0x001C3804
			private void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (this._validPartiesList.Contains(party) && settlement.IsHideout && settlement.Hideout == this._relatedHideout)
				{
					this._validPartiesList.Remove(party);
				}
			}

			// Token: 0x06006173 RID: 24947 RVA: 0x001C5637 File Offset: 0x001C3837
			private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
			{
				if (party != null && party.IsBandit && settlement.IsHideout && settlement.Hideout == this._relatedHideout)
				{
					this._validPartiesList.Add(party);
				}
			}

			// Token: 0x06006174 RID: 24948 RVA: 0x001C5666 File Offset: 0x001C3866
			private void OnBanditPartyRecruited(MobileParty banditParty)
			{
				if (this._validPartiesList.Contains(banditParty))
				{
					this._recruitedPartyCount++;
					this._validPartiesList.Remove(banditParty);
					this.AddQuestStepLog();
				}
			}

			// Token: 0x06006175 RID: 24949 RVA: 0x001C5697 File Offset: 0x001C3897
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06006176 RID: 24950 RVA: 0x001C56AA File Offset: 0x001C38AA
			private void OnVillageRaided(Village village)
			{
				if (village == base.QuestGiver.CurrentSettlement.Village)
				{
					base.AddLog(this.QuestGiverVillageRaided, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06006177 RID: 24951 RVA: 0x001C56D4 File Offset: 0x001C38D4
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclaredLog);
				}
			}

			// Token: 0x06006178 RID: 24952 RVA: 0x001C5703 File Offset: 0x001C3903
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestCanceledWarDeclaredLog, false);
			}

			// Token: 0x06006179 RID: 24953 RVA: 0x001C571B File Offset: 0x001C391B
			private void MobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
			{
				if (destroyerParty == PartyBase.MainParty && this._validPartiesList.Contains(mobileParty))
				{
					this._destroyedPartyCount++;
					this.AddQuestStepLog();
				}
			}

			// Token: 0x0600617A RID: 24954 RVA: 0x001C5748 File Offset: 0x001C3948
			protected override void HourlyTickParty(MobileParty mobileParty)
			{
				if (base.IsOngoing && mobileParty.IsBandit && mobileParty.MapEvent == null && mobileParty.MapFaction.IsBanditFaction && !mobileParty.IsCurrentlyUsedByAQuest && !mobileParty.IsCurrentlyAtSea)
				{
					float num;
					if (((mobileParty.CurrentSettlement != null) ? Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty.CurrentSettlement, base.QuestGiver.CurrentSettlement, false, false, MobileParty.NavigationType.Default, out num) : Campaign.Current.Models.MapDistanceModel.GetDistance(mobileParty, base.QuestGiver.CurrentSettlement, false, MobileParty.NavigationType.Default, out num)) <= MerchantNeedsHelpWithOutlawsIssueQuestBehavior.ValidBanditPartyDistance)
					{
						if (!this._validPartiesList.Contains(mobileParty))
						{
							if (!base.IsTracked(mobileParty))
							{
								base.AddTrackedObject(mobileParty);
							}
							this._validPartiesList.Add(mobileParty);
							if (mobileParty.CurrentSettlement == null && MBRandom.RandomFloat < 1f / (float)this._validPartiesList.Count)
							{
								SetPartyAiAction.GetActionForPatrollingAroundSettlement(mobileParty, base.QuestGiver.CurrentSettlement, MobileParty.NavigationType.Default, false, false);
								mobileParty.Ai.SetDoNotMakeNewDecisions(true);
								mobileParty.IgnoreForHours(500f);
								return;
							}
						}
						else if (MBRandom.RandomFloat < 0.05f)
						{
							mobileParty.Ai.SetDoNotMakeNewDecisions(false);
							return;
						}
					}
					else if (base.IsTracked(mobileParty))
					{
						base.RemoveTrackedObject(mobileParty);
						this._validPartiesList.Remove(mobileParty);
						mobileParty.Ai.SetDoNotMakeNewDecisions(false);
					}
				}
			}

			// Token: 0x0600617B RID: 24955 RVA: 0x001C58BC File Offset: 0x001C3ABC
			private void SuccessConsequences()
			{
				if (this._destroyedPartyCount == this._totalPartyCount)
				{
					base.AddLog(this.SuccessQuestLogText1, false);
				}
				else if (this._recruitedPartyCount != 0 && this._recruitedPartyCount < this._totalPartyCount)
				{
					base.AddLog(this.SuccessQuestLogText2, false);
				}
				else
				{
					base.AddLog(this.SuccessQuestLogText3, false);
				}
				this.RelationshipChangeWithQuestGiver = 3;
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
				if (base.QuestGiver.CurrentSettlement.IsVillage && base.QuestGiver.CurrentSettlement.Village.TradeBound != null)
				{
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security += 5f;
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity += 5f;
				}
				else if (base.QuestGiver.CurrentSettlement.IsTown)
				{
					base.QuestGiver.CurrentSettlement.Town.Security += 5f;
					base.QuestGiver.CurrentSettlement.Town.Prosperity += 5f;
				}
				Hero.MainHero.Clan.AddRenown(1f, true);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x0600617C RID: 24956 RVA: 0x001C5A24 File Offset: 0x001C3C24
			protected override void OnTimedOut()
			{
				this.RelationshipChangeWithQuestGiver = -5;
				if (base.QuestGiver.CurrentSettlement.IsVillage)
				{
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity -= 10f;
				}
				else if (base.QuestGiver.CurrentSettlement.IsTown)
				{
					base.QuestGiver.CurrentSettlement.Town.Prosperity -= 10f;
				}
				base.AddLog(this.TimeoutLog, false);
			}

			// Token: 0x0600617D RID: 24957 RVA: 0x001C5AB9 File Offset: 0x001C3CB9
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x0600617E RID: 24958 RVA: 0x001C5AC4 File Offset: 0x001C3CC4
			protected override void OnFinalize()
			{
				foreach (MobileParty mobileParty in this._validPartiesList)
				{
					mobileParty.Ai.SetDoNotMakeNewDecisions(false);
					mobileParty.IgnoreForHours(0f);
					if (base.IsTracked(mobileParty))
					{
						base.RemoveTrackedObject(mobileParty);
					}
				}
				this._validPartiesList.Clear();
			}

			// Token: 0x04001F0B RID: 7947
			[SaveableField(10)]
			private readonly int _totalPartyCount;

			// Token: 0x04001F0C RID: 7948
			[SaveableField(30)]
			private int _destroyedPartyCount;

			// Token: 0x04001F0D RID: 7949
			[SaveableField(50)]
			private int _recruitedPartyCount;

			// Token: 0x04001F0E RID: 7950
			[SaveableField(40)]
			private List<MobileParty> _validPartiesList;

			// Token: 0x04001F0F RID: 7951
			[SaveableField(70)]
			private Hideout _relatedHideout;

			// Token: 0x04001F10 RID: 7952
			private const float ValidBanditPartyEnableAiChance = 0.05f;

			// Token: 0x04001F11 RID: 7953
			[SaveableField(60)]
			private JournalLog _questProgressLogTest;
		}

		// Token: 0x02000778 RID: 1912
		public class MerchantNeedsHelpWithOutlawsIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06006183 RID: 24963 RVA: 0x001C5B80 File Offset: 0x001C3D80
			public MerchantNeedsHelpWithOutlawsIssueTypeDefiner()
				: base(590000)
			{
			}

			// Token: 0x06006184 RID: 24964 RVA: 0x001C5B8D File Offset: 0x001C3D8D
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssue), 1, null);
				base.AddClassDefinition(typeof(MerchantNeedsHelpWithOutlawsIssueQuestBehavior.MerchantNeedsHelpWithOutlawsIssueQuest), 2, null);
			}
		}
	}
}
