using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200042B RID: 1067
	public class IssuesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600437C RID: 17276 RVA: 0x0013DC6C File Offset: 0x0013BE6C
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.OnIssueUpdatedEvent.AddNonSerializedListener(this, new Action<IssueBase, IssueBase.IssueUpdateDetails, Hero>(this.OnIssueUpdated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnSettlementDailyTick));
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x0013DD1C File Offset: 0x0013BF1C
		private void OnSettlementDailyTick(Settlement settlement)
		{
			float num = 0f;
			for (int i = 0; i < settlement.HeroesWithoutParty.Count; i++)
			{
				if (settlement.HeroesWithoutParty[i].Issue != null)
				{
					num += 1f;
				}
			}
			int num2 = (settlement.IsTown ? 1 : 1);
			int num3 = (settlement.IsTown ? 3 : 2);
			if (num < (float)num3 && (num < (float)num2 || MBRandom.RandomFloat < this.GetIssueGenerationChance(num, num3)))
			{
				int num4 = 0;
				foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
				{
					if (!keyValuePair.Value.IsTriedToSolveBefore)
					{
						num4++;
					}
				}
				this.CreateAnIssueForSettlementNotables(settlement, num4 + 1);
			}
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x0013DE00 File Offset: 0x0013C000
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			Settlement[] array = Village.All.Select<Village, Settlement>((Village x) => x.Settlement).ToArray<Settlement>();
			int num = MathF.Ceiling(0.7f * (float)array.Length);
			Settlement[] array2 = Town.AllTowns.Select<Town, Settlement>((Town x) => x.Settlement).ToArray<Settlement>();
			int num2 = MathF.Ceiling(0.8f * (float)array2.Length);
			int num3 = Hero.AllAliveHeroes.Count<Hero>((Hero x) => x.IsLord && x.Clan != null && !x.Clan.IsBanditFaction && !x.IsChild);
			int num4 = MathF.Ceiling(0.120000005f * (float)num3);
			int num5 = num + num2 + num4;
			Campaign.Current.ConversationManager.DisableSentenceSort();
			this._additionalFrequencyScore = -0.4f;
			array.Shuffle<Settlement>();
			this.CreateRandomSettlementIssues(array, 2, num, num5);
			array2.Shuffle<Settlement>();
			this.CreateRandomSettlementIssues(array2, 3, num2, num5);
			Clan[] array3 = Clan.NonBanditFactions.Where<Clan>((Clan x) => x.Heroes.Count != 0).ToArray<Clan>();
			array3.Shuffle<Clan>();
			this.CreateRandomClanIssues(array3, num4, num5);
			this._additionalFrequencyScore = 0.2f;
			Campaign.Current.ConversationManager.EnableSentenceSort();
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0013DF68 File Offset: 0x0013C168
		private void DailyTickClan(Clan clan)
		{
			if (this.IsClanSuitableForIssueCreation(clan))
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < clan.Heroes.Count; i++)
				{
					Hero hero = clan.Heroes[i];
					if (hero.Issue != null)
					{
						num++;
					}
					if (hero.IsAlive && !hero.IsChild && hero.IsLord)
					{
						num2++;
					}
				}
				int num3 = MathF.Ceiling((float)num2 * 0.1f);
				int num4 = MathF.Floor((float)num2 * 0.2f);
				if (num4 > 0 && num < num4 && (num < num3 || MBRandom.RandomFloat < this.GetIssueGenerationChance((float)num, num4)))
				{
					int num5 = 0;
					foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
					{
						if (!keyValuePair.Value.IsTriedToSolveBefore)
						{
							num5++;
						}
					}
					this.CreateAnIssueForClanNobles(clan, num5 + 1);
				}
			}
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x0013E080 File Offset: 0x0013C280
		private bool IsClanSuitableForIssueCreation(Clan clan)
		{
			return clan.Heroes.Count > 0 && !clan.IsBanditFaction;
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0013E09C File Offset: 0x0013C29C
		private void OnGameLoaded(CampaignGameStarter obj)
		{
			this._additionalFrequencyScore = 0.2f;
			List<IssueBase> list = new List<IssueBase>();
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
			{
				if (keyValuePair.Key.IsNotable && keyValuePair.Key.CurrentSettlement == null)
				{
					list.Add(keyValuePair.Value);
				}
			}
			foreach (IssueBase issueBase in list)
			{
				issueBase.CompleteIssueWithCancel(null);
			}
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x0013E168 File Offset: 0x0013C368
		private float GetIssueGenerationChance(float currentIssueCount, int maxIssueCount)
		{
			float num = 1f - currentIssueCount / (float)maxIssueCount;
			return 0.3f * num * num;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0013E18C File Offset: 0x0013C38C
		private void CreateRandomSettlementIssues(Settlement[] shuffledSettlementArray, int maxIssueCountPerSettlement, int desiredIssueCount, int totalDesiredIssueCount)
		{
			int num = shuffledSettlementArray.Length;
			int[] array = new int[num];
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			while (num2 < num && num4 < desiredIssueCount)
			{
				int num6 = (num4 + num2 + num3) % num;
				if (array[num6] < num5)
				{
					num3++;
				}
				else if (array[num6] < maxIssueCountPerSettlement && this.CreateAnIssueForSettlementNotables(shuffledSettlementArray[num6], totalDesiredIssueCount))
				{
					num4++;
					array[num6]++;
				}
				else
				{
					num2++;
				}
			}
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x0013E200 File Offset: 0x0013C400
		private void CreateRandomClanIssues(Clan[] shuffledClanArray, int desiredIssueCount, int totalDesiredIssueCount)
		{
			int num = shuffledClanArray.Length;
			int num2 = 0;
			int num3 = 0;
			while (num2 < num && num3 < desiredIssueCount)
			{
				if (this.CreateAnIssueForClanNobles(shuffledClanArray[(num3 + num2) % num], totalDesiredIssueCount))
				{
					num3++;
				}
				else
				{
					num2++;
				}
			}
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x0013E23C File Offset: 0x0013C43C
		private bool CreateAnIssueForSettlementNotables(Settlement settlement, int totalDesiredIssueCount)
		{
			IssueManager issueManager = Campaign.Current.IssueManager;
			foreach (Hero hero in settlement.Notables)
			{
				if (hero.Issue == null && hero.CanHaveCampaignIssues())
				{
					List<PotentialIssueData> list = Campaign.Current.IssueManager.CheckForIssues(hero);
					int num = list.SumQ<PotentialIssueData>((PotentialIssueData x) => this.GetFrequencyScore(x.Frequency));
					foreach (PotentialIssueData potentialIssueData in list)
					{
						if (potentialIssueData.IsValid)
						{
							float num2 = this.CalculateIssueScoreForNotable(in potentialIssueData, settlement, totalDesiredIssueCount, num);
							if (num2 > 0f && !issueManager.HasIssueCoolDown(potentialIssueData.IssueType, hero))
							{
								this._cachedIssueDataList.Add(new IssuesCampaignBehavior.IssueData(potentialIssueData, hero, num2));
							}
						}
					}
				}
			}
			if (this._cachedIssueDataList.Count > 0)
			{
				List<ValueTuple<IssuesCampaignBehavior.IssueData, float>> list2 = new List<ValueTuple<IssuesCampaignBehavior.IssueData, float>>();
				foreach (IssuesCampaignBehavior.IssueData issueData in this._cachedIssueDataList)
				{
					list2.Add(new ValueTuple<IssuesCampaignBehavior.IssueData, float>(issueData, issueData.Score));
				}
				IssuesCampaignBehavior.IssueData issueData2 = MBRandom.ChooseWeighted<IssuesCampaignBehavior.IssueData>(list2);
				Campaign.Current.IssueManager.CreateNewIssue(in issueData2.PotentialIssueData, issueData2.Hero);
				this._cachedIssueDataList.Clear();
				return true;
			}
			this._cachedIssueDataList.Clear();
			return false;
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x0013E3F8 File Offset: 0x0013C5F8
		private bool CreateAnIssueForClanNobles(Clan clan, int totalDesiredIssueCount)
		{
			IssuesCampaignBehavior.IssueData? issueData = null;
			float num = 0f;
			IssueManager issueManager = Campaign.Current.IssueManager;
			foreach (Hero hero in clan.AliveLords)
			{
				if (hero.Clan != Clan.PlayerClan && hero.CanHaveCampaignIssues() && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && (hero.IsActive || hero.IsPrisoner) && hero.Issue == null)
				{
					List<PotentialIssueData> list = Campaign.Current.IssueManager.CheckForIssues(hero);
					int num2 = list.SumQ<PotentialIssueData>((PotentialIssueData x) => this.GetFrequencyScore(x.Frequency));
					foreach (PotentialIssueData potentialIssueData in list)
					{
						if (potentialIssueData.IsValid)
						{
							float num3 = this.CalculateIssueScoreForClan(in potentialIssueData, clan, totalDesiredIssueCount, num2);
							if (num3 > num && !issueManager.HasIssueCoolDown(potentialIssueData.IssueType, hero))
							{
								issueData = new IssuesCampaignBehavior.IssueData?(new IssuesCampaignBehavior.IssueData(potentialIssueData, hero, num3));
								num = num3;
							}
						}
					}
				}
			}
			if (issueData != null)
			{
				Campaign.Current.IssueManager.CreateNewIssue(in issueData.Value.PotentialIssueData, issueData.Value.Hero);
				return true;
			}
			return false;
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x0013E5B0 File Offset: 0x0013C7B0
		private float CalculateIssueScoreForClan(in PotentialIssueData pid, Clan clan, int totalDesiredIssueCount, int totalFrequencyScore)
		{
			foreach (Hero hero in clan.Heroes)
			{
				if (hero.Issue != null)
				{
					Type type = hero.Issue.GetType();
					PotentialIssueData potentialIssueData = pid;
					if (type == potentialIssueData.IssueType)
					{
						return 0f;
					}
				}
			}
			return this.CalculateIssueScoreInternal(in pid, totalDesiredIssueCount, totalFrequencyScore);
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x0013E638 File Offset: 0x0013C838
		private float CalculateIssueScoreForNotable(in PotentialIssueData pid, Settlement settlement, int totalDesiredIssueCount, int totalFrequencyScore)
		{
			foreach (Hero hero in settlement.Notables)
			{
				if (hero.Issue != null)
				{
					Type type = hero.Issue.GetType();
					PotentialIssueData potentialIssueData = pid;
					if (type == potentialIssueData.IssueType)
					{
						return 0f;
					}
				}
			}
			return this.CalculateIssueScoreInternal(in pid, totalDesiredIssueCount, totalFrequencyScore);
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x0013E6C0 File Offset: 0x0013C8C0
		private float CalculateIssueScoreInternal(in PotentialIssueData pid, int totalDesiredIssueCount, int totalFrequencyScore)
		{
			PotentialIssueData potentialIssueData = pid;
			float num = (float)this.GetFrequencyScore(potentialIssueData.Frequency) / (float)totalFrequencyScore;
			float num2;
			if (totalDesiredIssueCount == 0)
			{
				num2 = 1f;
			}
			else
			{
				int num3 = 0;
				foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
				{
					Type type = keyValuePair.Value.GetType();
					potentialIssueData = pid;
					if (type == potentialIssueData.IssueType)
					{
						num3++;
					}
				}
				num2 = (float)num3 / (float)totalDesiredIssueCount;
			}
			float num4 = 1f + this._additionalFrequencyScore - num2 / num;
			if (num4 < 0f)
			{
				num4 = 0f;
			}
			else if (num4 < this._additionalFrequencyScore)
			{
				num4 *= 0.01f;
			}
			else if (num4 < this._additionalFrequencyScore + 0.4f)
			{
				num4 *= 0.1f;
			}
			return num * num4;
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x0013E7BC File Offset: 0x0013C9BC
		private int GetFrequencyScore(IssueBase.IssueFrequency frequency)
		{
			int num = 0;
			switch (frequency)
			{
			case IssueBase.IssueFrequency.VeryCommon:
				num = 6;
				break;
			case IssueBase.IssueFrequency.Common:
				num = 3;
				break;
			case IssueBase.IssueFrequency.Rare:
				num = 1;
				break;
			}
			return num;
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x0013E7EC File Offset: 0x0013C9EC
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CharacterObject characterObject;
			if (party == null)
			{
				characterObject = hero.CharacterObject;
			}
			else
			{
				Hero leaderHero = party.LeaderHero;
				characterObject = ((leaderHero != null) ? leaderHero.CharacterObject : null);
			}
			CharacterObject characterObject2 = characterObject;
			if (characterObject2 != null && !characterObject2.IsPlayerCharacter && ((party != null) ? party.Army : null) == null && Campaign.Current.GameStarted)
			{
				MBList<IssueBase> mblist = IssueManager.GetIssuesInSettlement(settlement, true).ToMBList<IssueBase>();
				float num = ((settlement.OwnerClan == characterObject2.HeroObject.Clan) ? 0.05f : 0.01f);
				if (mblist.Count > 0 && MBRandom.RandomFloat < num)
				{
					IssueBase randomElement = mblist.GetRandomElement<IssueBase>();
					if (randomElement.CanBeCompletedByAI() && randomElement.IsOngoingWithoutQuest)
					{
						randomElement.CompleteIssueWithAiLord(characterObject2.HeroObject);
					}
				}
			}
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0013E8A0 File Offset: 0x0013CAA0
		private void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver = null)
		{
			if (details == IssueBase.IssueUpdateDetails.IssueFinishedWithSuccess && issueSolver != null && issueSolver.GetPerkValue(DefaultPerks.Charm.Oratory))
			{
				GainRenownAction.Apply(issueSolver, (float)MathF.Round(DefaultPerks.Charm.Oratory.PrimaryBonus), false);
				GainKingdomInfluenceAction.ApplyForDefault(issueSolver, (float)MathF.Round(DefaultPerks.Charm.Oratory.PrimaryBonus));
			}
			if ((details == IssueBase.IssueUpdateDetails.IssueFail || details == IssueBase.IssueUpdateDetails.IssueFinishedWithSuccess || details == IssueBase.IssueUpdateDetails.IssueFinishedWithBetrayal || details == IssueBase.IssueUpdateDetails.IssueTimedOut || details == IssueBase.IssueUpdateDetails.SentTroopsFinishedQuest || details == IssueBase.IssueUpdateDetails.SentTroopsFailedQuest) && issueSolver != null && issue.IssueOwner != null)
			{
				int num = (issue.IsSolvingWithQuest ? issue.IssueQuest.RelationshipChangeWithQuestGiver : issue.RelationshipChangeWithIssueOwner);
				if (num > 0)
				{
					if (issueSolver.GetPerkValue(DefaultPerks.Trade.DistributedGoods) && issue.IssueOwner.IsArtisan)
					{
						num *= (int)DefaultPerks.Trade.DistributedGoods.PrimaryBonus;
					}
					if (issueSolver.GetPerkValue(DefaultPerks.Trade.LocalConnection) && issue.IssueOwner.IsMerchant)
					{
						num *= (int)DefaultPerks.Trade.LocalConnection.PrimaryBonus;
					}
					ChangeRelationAction.ApplyPlayerRelation(issue.IsSolvingWithQuest ? issue.IssueQuest.QuestGiver : issue.IssueOwner, num, true, true);
				}
				else if (num < 0)
				{
					ChangeRelationAction.ApplyPlayerRelation(issue.IsSolvingWithQuest ? issue.IssueQuest.QuestGiver : issue.IssueOwner, num, true, true);
				}
			}
			if (details == IssueBase.IssueUpdateDetails.IssueCancel || details == IssueBase.IssueUpdateDetails.IssueFail || details == IssueBase.IssueUpdateDetails.IssueFinishedWithSuccess || details == IssueBase.IssueUpdateDetails.IssueFinishedWithBetrayal || details == IssueBase.IssueUpdateDetails.IssueTimedOut || details == IssueBase.IssueUpdateDetails.SentTroopsFinishedQuest || details == IssueBase.IssueUpdateDetails.SentTroopsFailedQuest || details == IssueBase.IssueUpdateDetails.IssueFinishedByAILord)
			{
				Campaign.Current.IssueManager.AddIssueCoolDownData(issue.GetType(), new HeroRelatedIssueCoolDownData(issue.IssueOwner, CampaignTime.DaysFromNow((float)Campaign.Current.Models.IssueModel.IssueOwnerCoolDownInDays)));
			}
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0013EA33 File Offset: 0x0013CC33
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x0013EA38 File Offset: 0x0013CC38
		private void OnSessionLaunched(CampaignGameStarter starter)
		{
			List<Settlement> list = Settlement.All.Where<Settlement>((Settlement x) => x.IsTown || x.IsVillage).ToList<Settlement>();
			this.DeterministicShuffle(list);
			this.AddDialogues(starter);
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0013EA84 File Offset: 0x0013CC84
		private void DeterministicShuffle(List<Settlement> settlements)
		{
			Random random = new Random(53);
			for (int i = 0; i < settlements.Count; i++)
			{
				int num = random.Next() % settlements.Count;
				Settlement settlement = settlements[i];
				settlements[i] = settlements[num];
				settlements[num] = settlement;
			}
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0013EAD8 File Offset: 0x0013CCD8
		private void AddDialogues(CampaignGameStarter starter)
		{
			starter.AddDialogLine("issue_not_offered", "issue_offer", "hero_main_options", "{=!}{ISSUE_NOT_OFFERED_EXPLANATION}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_not_offered_condition), new ConversationSentence.OnConsequenceDelegate(this.leave_on_conversation_end_consequence), 100, null);
			starter.AddDialogLine("issue_explanation", "issue_offer", "issue_explanation_player_response", "{=!}{IssueBriefByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offered_begin_condition), new ConversationSentence.OnConsequenceDelegate(this.leave_on_conversation_end_consequence), 100, null);
			starter.AddPlayerLine("issue_explanation_player_response_pre_lord_solution", "issue_explanation_player_response", "issue_lord_solution_brief", "{=!}{IssueAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(this.issue_explanation_player_response_pre_lord_solution_condition), null, 100, null, null);
			starter.AddPlayerLine("issue_explanation_player_response_pre_quest_solution", "issue_explanation_player_response", "issue_quest_solution_brief", "{=!}{IssueAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(this.issue_explanation_player_response_pre_quest_solution_condition), null, 100, null, null);
			starter.AddDialogLine("issue_lord_solution_brief", "issue_lord_solution_brief", "issue_lord_solution_player_response", "{=!}{IssueLordSolutionExplanationByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_lord_solution_brief_condition), null, 100, null);
			starter.AddPlayerLine("issue_lord_solution_player_response", "issue_lord_solution_player_response", "issue_quest_solution_brief", "{=!}{IssuePlayerResponseAfterLordExplanationText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_lord_solution_player_response_condition), null, 100, null, null);
			starter.AddDialogLine("issue_quest_solution_brief_pre_alternative_solution", "issue_quest_solution_brief", "issue_alternative_solution_player_response", "{=!}{IssueQuestSolutionExplanationByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_quest_solution_brief_pre_alternative_solution_condition), null, 100, null);
			starter.AddDialogLine("issue_quest_solution_brief_pre_player_response", "issue_quest_solution_brief", "issue_offer_player_response", "{=!}{IssueQuestSolutionExplanationByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_quest_solution_brief_pre_player_response_condition), null, 100, null);
			starter.AddPlayerLine("issue_alternative_solution_player_response", "issue_alternative_solution_player_response", "issue_alternative_solution_brief", "{=!}{IssuePlayerResponseAfterAlternativeExplanationText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_alternative_solution_player_response_condition), null, 100, null, null);
			starter.AddDialogLine("issue_alternative_solution_brief", "issue_alternative_solution_brief", "issue_offer_player_response", "{=!}{IssueAlternativeSolutionExplanationByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_alternative_solution_brief_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_2_consequence), 100, null);
			starter.AddPlayerLine("issue_offer_player_accept_quest", "issue_offer_player_response", "issue_classic_quest_start", "{=!}{IssueQuestSolutionAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_quest_condition), delegate
			{
				Campaign.Current.IssueManager.StartIssueQuest(Hero.OneToOneConversationHero);
			}, 100, null, null);
			starter.AddPlayerLine("issue_offer_player_accept_alternative", "issue_offer_player_response", "issue_offer_player_accept_alternative_2", "{=!}{IssueAlternativeSolutionAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_condition), null, 100, new ConversationSentence.OnClickableConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_clickable_condition), null);
			starter.AddPlayerLine("issue_offer_player_accept_lord", "issue_offer_player_response", "issue_offer_player_accept_lord_2", "{=!}{IssueLordSolutionAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(this.issue_offer_player_accept_lord_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_lord_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_lord_clickable_condition), null);
			starter.AddPlayerLine("issue_offer_player_response_reject", "issue_offer_player_response", "issue_offer_hero_response_reject", "{=l549ODcw}Sorry. I can't do that right now.", null, null, 100, null, null);
			starter.AddDialogLine("issue_offer_player_accept_alternative_2", "issue_offer_player_accept_alternative_2", "issue_offer_player_accept_alternative_3", "{=X4ITSQOl}Which of your people can help us?", null, null, 100, null);
			starter.AddRepeatablePlayerLine("issue_offer_player_accept_alternative_3", "issue_offer_player_accept_alternative_3", "issue_offer_player_accept_alternative_4", "{=C2ZGNwwh}{COMPANION.NAME} {COMPANION_SCALED_PARAMETERS}", "{=nomZx5Nw}I am thinking of a different companion.", "issue_offer_player_accept_alternative_2", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_3_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_3_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_3_clickable_condition));
			starter.AddPlayerLine("issue_offer_player_accept_go_back", "issue_offer_player_accept_alternative_3", "issue_offer_hero_response_reject", "{=OymJQD7M}Actually, I don't have any available men right now...", null, null, 100, null, null);
			starter.AddDialogLine("issue_offer_player_accept_alternative_4", "issue_offer_player_accept_alternative_4", "issue_offer_player_accept_alternative_5", "{=!}Party screen goes here", null, new ConversationSentence.OnConsequenceDelegate(this.issue_offer_player_accept_alternative_4_consequence), 100, null);
			starter.AddDialogLine("issue_offer_player_accept_alternative_5_a", "issue_offer_player_accept_alternative_5", "close_window", "{=!}{IssueAlternativeSolutionResponseByIssueGiverText}", new ConversationSentence.OnConditionDelegate(this.issue_offer_player_accept_alternative_5_a_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_5_a_consequence), 100, null);
			starter.AddDialogLine("issue_offer_player_accept_alternative_5_b", "issue_offer_player_accept_alternative_5", "issue_offer_player_response", "{=!}{IssueGiverResponseToRejection}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_hero_response_reject_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_5_b_consequence), 100, null);
			starter.AddPlayerLine("issue_offer_player_back", "issue_offer_player_accept_alternative_5", "issue_offer_player_response", GameTexts.FindText("str_back", null).ToString(), null, null, 100, null, null);
			starter.AddDialogLine("issue_offer_player_accept_lord_2", "issue_offer_player_accept_lord_2", "hero_main_options", "{=!}{IssueLordSolutionResponseByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_lord_2_condition), null, 100, null);
			starter.AddDialogLine("issue_offer_hero_response_reject", "issue_offer_hero_response_reject", "hero_main_options", "{=!}{IssueGiverResponseToRejection}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_hero_response_reject_condition), null, 100, null);
			starter.AddDialogLine("issue_counter_offer_1", "start", "issue_counter_offer_2", "{=!}{IssueLordSolutionCounterOfferBriefByOtherNpcText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_start_condition), null, int.MaxValue, null);
			starter.AddDialogLine("issue_counter_offer_2", "issue_counter_offer_2", "issue_counter_offer_player_response", "{=!}{IssueLordSolutionCounterOfferExplanationByOtherNpcText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_2_condition), null, 100, null);
			starter.AddPlayerLine("issue_counter_offer_player_accept", "issue_counter_offer_player_response", "issue_counter_offer_accepted", "{=!}{IssueLordSolutionCounterOfferAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_player_accept_condition), null, 100, null, null);
			starter.AddDialogLine("issue_counter_offer_accepted", "issue_counter_offer_accepted", "close_window", "{=!}{IssueLordSolutionCounterOfferAcceptResponseByOtherNpcText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_accepted_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_counter_offer_accepted_consequence), 100, null);
			starter.AddPlayerLine("issue_counter_offer_player_reject", "issue_counter_offer_player_response", "issue_counter_offer_reject", "{=!}{IssueLordSolutionCounterOfferDeclineByPlayerText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_player_reject_condition), null, 100, null, null);
			starter.AddDialogLine("issue_counter_offer_reject", "issue_counter_offer_reject", "close_window", "{=!}{IssueLordSolutionCounterOfferDeclineResponseByOtherNpcText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_reject_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_counter_offer_reject_consequence), 100, null);
			starter.AddDialogLine("issue_alternative_solution_discuss", "issue_discuss_alternative_solution", "close_window", "{=!}{IssueDiscussAlternativeSolution}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_alternative_solution_discussion_condition), new ConversationSentence.OnConsequenceDelegate(this.issue_alternative_solution_discussion_consequence), int.MaxValue, null);
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0013F098 File Offset: 0x0013D298
		private static bool issue_alternative_solution_discussion_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if (issueOwnersIssue != null && issueOwnersIssue.IsThereAlternativeSolution && issueOwnersIssue.IsSolvingWithAlternative)
			{
				MBTextManager.SetTextVariable("IssueDiscussAlternativeSolution", issueOwnersIssue.IssueDiscussAlternativeSolution, false);
				return true;
			}
			return false;
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0013F0D2 File Offset: 0x0013D2D2
		private void issue_alternative_solution_discussion_consequence()
		{
			if (PlayerEncounter.Current != null && Campaign.Current.ConversationManager.ConversationParty == PlayerEncounter.EncounteredMobileParty)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0013F0F8 File Offset: 0x0013D2F8
		private static void issue_counter_offer_reject_consequence()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			Campaign.Current.ConversationManager.ConversationEndOneShot += counterOfferersIssue.CompleteIssueWithLordSolutionWithRefuseCounterOffer;
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0013F128 File Offset: 0x0013D328
		private static bool issue_counter_offer_reject_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferDeclineResponseByOtherNpcText", counterOfferersIssue.IssueLordSolutionCounterOfferDeclineResponseByOtherNpc, false);
			return true;
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0013F150 File Offset: 0x0013D350
		private static bool issue_counter_offer_player_reject_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferDeclineByPlayerText", counterOfferersIssue.IssueLordSolutionCounterOfferDeclineByPlayer, false);
			return true;
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0013F178 File Offset: 0x0013D378
		private static void issue_counter_offer_accepted_consequence()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			Campaign.Current.ConversationManager.ConversationEndOneShot += counterOfferersIssue.CompleteIssueWithLordSolutionWithAcceptCounterOffer;
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0013F1A8 File Offset: 0x0013D3A8
		private static bool issue_counter_offer_accepted_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferAcceptResponseByOtherNpcText", counterOfferersIssue.IssueLordSolutionCounterOfferAcceptResponseByOtherNpc, false);
			return true;
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x0013F1D0 File Offset: 0x0013D3D0
		private static bool issue_counter_offer_player_accept_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferAcceptByPlayerText", counterOfferersIssue.IssueLordSolutionCounterOfferAcceptByPlayer, false);
			return true;
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x0013F1F8 File Offset: 0x0013D3F8
		private static bool issue_counter_offer_2_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferExplanationByOtherNpcText", counterOfferersIssue.IssueLordSolutionCounterOfferExplanationByOtherNpc, false);
			return true;
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x0013F220 File Offset: 0x0013D420
		private static bool issue_counter_offer_start_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			if (counterOfferersIssue != null)
			{
				MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferBriefByOtherNpcText", counterOfferersIssue.IssueLordSolutionCounterOfferBriefByOtherNpc, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0013F24C File Offset: 0x0013D44C
		private static bool issue_offer_player_accept_lord_2_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionResponseByIssueGiverText", issueOwnersIssue.IssueLordSolutionResponseByIssueGiver, false);
			return true;
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x0013F274 File Offset: 0x0013D474
		private void issue_offer_player_accept_alternative_4_consequence()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			int totalAlternativeSolutionNeededMenCount = issueOwnersIssue.GetTotalAlternativeSolutionNeededMenCount();
			if (totalAlternativeSolutionNeededMenCount > 1)
			{
				PartyScreenHelper.OpenScreenAsQuest(issueOwnersIssue.AlternativeSolutionSentTroops, new TextObject("{=FbLOFO88}Select troops for mission", null), totalAlternativeSolutionNeededMenCount + 1, issueOwnersIssue.GetTotalAlternativeSolutionDurationInDays(), new PartyPresentationDoneButtonConditionDelegate(this.PartyScreenDoneCondition), new PartyScreenClosedDelegate(IssuesCampaignBehavior.PartyScreenDoneClicked), new IsTroopTransferableDelegate(IssuesCampaignBehavior.TroopTransferableDelegate), null);
				return;
			}
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x0013F2E8 File Offset: 0x0013D4E8
		private static void issue_offer_player_accept_alternative_5_b_consequence()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MobileParty.MainParty.MemberRoster.Add(issueOwnersIssue.AlternativeSolutionSentTroops);
			issueOwnersIssue.AlternativeSolutionSentTroops.Clear();
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0013F31B File Offset: 0x0013D51B
		private static void issue_offer_player_accept_alternative_5_a_consequence()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			issueOwnersIssue.AlternativeSolutionStartConsequence();
			issueOwnersIssue.StartIssueWithAlternativeSolution();
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0013F330 File Offset: 0x0013D530
		private bool issue_offer_player_accept_alternative_5_a_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueAlternativeSolutionResponseByIssueGiverText", issueOwnersIssue.IssueAlternativeSolutionResponseByIssueGiver, false);
			TextObject textObject;
			return IssuesCampaignBehavior.DoTroopsSatisfyAlternativeSolutionInternal(issueOwnersIssue.AlternativeSolutionSentTroops, out textObject);
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0013F364 File Offset: 0x0013D564
		private static bool issue_offer_player_accept_alternative_3_clickable_condition(out TextObject explanation)
		{
			bool flag = true;
			Hero hero = ConversationSentence.CurrentProcessedRepeatObject as Hero;
			if (hero == null || hero.PartyBelongedTo != MobileParty.MainParty)
			{
				explanation = null;
				flag = false;
			}
			else if (!hero.CanHaveCampaignIssues())
			{
				explanation = new TextObject("{=DBabgrcC}This hero is not available right now.", null);
				flag = false;
			}
			else if (hero.IsWounded)
			{
				explanation = new TextObject("{=CyrOuz4h}This hero is wounded.", null);
				flag = false;
			}
			else if (hero.IsPregnant)
			{
				explanation = new TextObject("{=BaKOWJb6}This hero is pregnant.", null);
				flag = false;
			}
			else
			{
				explanation = null;
			}
			return flag;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0013F3E4 File Offset: 0x0013D5E4
		private static void issue_offer_player_accept_alternative_3_consequence()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			Hero hero = ConversationSentence.SelectedRepeatObject as Hero;
			if (hero != null)
			{
				MobileParty.MainParty.MemberRoster.AddToCounts(hero.CharacterObject, -1, false, 0, 0, true, -1);
				issueOwnersIssue.AlternativeSolutionSentTroops.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
				CampaignEventDispatcher.Instance.OnHeroGetsBusy(hero, HeroGetsBusyReasons.SolvesIssue);
			}
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x0013F448 File Offset: 0x0013D648
		private static bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase leftOwnerParty)
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			return !character.IsHero && !character.IsNotTransferableInPartyScreen && type != PartyScreenLogic.TroopType.Prisoner && issueOwnersIssue.IsTroopTypeNeededByAlternativeSolution(character);
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x0013F478 File Offset: 0x0013D678
		private static void PartyScreenDoneClicked(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x0013F48C File Offset: 0x0013D68C
		private Tuple<bool, TextObject> PartyScreenDoneCondition(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
		{
			TextObject textObject;
			return new Tuple<bool, TextObject>(IssuesCampaignBehavior.DoTroopsSatisfyAlternativeSolutionInternal(leftMemberRoster, out textObject), textObject);
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x0013F4A8 File Offset: 0x0013D6A8
		private static bool DoTroopsSatisfyAlternativeSolutionInternal(TroopRoster troopRoster, out TextObject explanation)
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			int totalAlternativeSolutionNeededMenCount = issueOwnersIssue.GetTotalAlternativeSolutionNeededMenCount();
			if (troopRoster.TotalRegulars >= totalAlternativeSolutionNeededMenCount && troopRoster.TotalRegulars - troopRoster.TotalWoundedRegulars < totalAlternativeSolutionNeededMenCount)
			{
				explanation = new TextObject("{=fjmGXcLW}You have to send healthy troops to this quest.", null);
				return false;
			}
			return issueOwnersIssue.DoTroopsSatisfyAlternativeSolution(troopRoster, out explanation);
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x0013F4F4 File Offset: 0x0013D6F4
		private static bool issue_offer_player_accept_alternative_3_condition()
		{
			Hero hero = ConversationSentence.CurrentProcessedRepeatObject as Hero;
			if (hero != null)
			{
				StringHelpers.SetRepeatableCharacterProperties("COMPANION", hero.CharacterObject, false);
			}
			List<TextObject> list = new List<TextObject>();
			IssueModel issueModel = Campaign.Current.Models.IssueModel;
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			bool flag = false;
			if (issueOwnersIssue.AlternativeSolutionHasCasualties)
			{
				ValueTuple<int, int> causalityForHero = issueModel.GetCausalityForHero(hero, issueOwnersIssue);
				if (causalityForHero.Item2 > 0)
				{
					TextObject textObject;
					if (causalityForHero.Item1 == causalityForHero.Item2)
					{
						textObject = new TextObject("{=zPlFvCRm}{NUMBER_OF_TROOPS} troop loss", null);
						textObject.SetTextVariable("NUMBER_OF_TROOPS", causalityForHero.Item1);
					}
					else
					{
						textObject = new TextObject("{=bdlomGZ1}{MIN_NUMBER_OF_TROOPS} - {MAX_NUMBER_OF_TROOPS_LOST} troop loss", null);
						textObject.SetTextVariable("MIN_NUMBER_OF_TROOPS", causalityForHero.Item1);
						textObject.SetTextVariable("MAX_NUMBER_OF_TROOPS_LOST", causalityForHero.Item2);
					}
					flag = true;
					list.Add(textObject);
				}
			}
			if (issueOwnersIssue.AlternativeSolutionHasFailureRisk)
			{
				float num = issueModel.GetFailureRiskForHero(hero, issueOwnersIssue);
				if (num > 0f)
				{
					num = (float)((int)(num * 100f));
					TextObject textObject2 = new TextObject("{=9tLYXGGc}{FAILURE_RISK}% risk of failure", null);
					textObject2.SetTextVariable("FAILURE_RISK", num, 2);
					list.Add(textObject2);
					flag = true;
				}
				else
				{
					TextObject textObject3 = new TextObject("{=way8jWK8}no risk of failure", null);
					list.Add(textObject3);
				}
			}
			if (issueOwnersIssue.AlternativeSolutionHasScaledRequiredTroops)
			{
				int troopsRequiredForHero = issueModel.GetTroopsRequiredForHero(hero, issueOwnersIssue);
				if (troopsRequiredForHero > 0)
				{
					TextObject textObject4 = new TextObject("{=b3bJXMt2}{NUMBER_OF_TROOPS} required troops", null);
					textObject4.SetTextVariable("NUMBER_OF_TROOPS", troopsRequiredForHero);
					list.Add(textObject4);
					flag = true;
				}
			}
			if (issueOwnersIssue.AlternativeSolutionHasScaledDuration)
			{
				CampaignTime durationOfResolutionForHero = issueModel.GetDurationOfResolutionForHero(hero, issueOwnersIssue);
				if (durationOfResolutionForHero > CampaignTime.Days(0f))
				{
					TextObject textObject5 = new TextObject("{=ImatoO4Y}{DURATION_IN_DAYS} required days to complete", null);
					textObject5.SetTextVariable("DURATION_IN_DAYS", (float)durationOfResolutionForHero.ToDays, 2);
					list.Add(textObject5);
					flag = true;
				}
			}
			if (flag)
			{
				ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = issueModel.GetIssueAlternativeSolutionSkill(hero, issueOwnersIssue);
				if (issueAlternativeSolutionSkill.Item1 != null)
				{
					TextObject textObject6 = new TextObject("{=!}{SKILL}: {NUMBER}", null);
					textObject6.SetTextVariable("SKILL", issueAlternativeSolutionSkill.Item1.Name);
					textObject6.SetTextVariable("NUMBER", hero.GetSkillValue(issueAlternativeSolutionSkill.Item1));
					list.Add(textObject6);
				}
			}
			if (list.IsEmpty<TextObject>())
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("COMPANION_SCALED_PARAMETERS", TextObject.GetEmpty());
			}
			else
			{
				TextObject textObject7 = GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, false);
				TextObject textObject8 = GameTexts.FindText("str_STR_in_parentheses", null);
				textObject8.SetTextVariable("STR", textObject7);
				ConversationSentence.SelectedRepeatLine.SetTextVariable("COMPANION_SCALED_PARAMETERS", textObject8);
			}
			return true;
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0013F780 File Offset: 0x0013D980
		private static void issue_offer_player_accept_alternative_2_consequence()
		{
			List<Hero> list = new List<Hero>();
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsHero && !troopRosterElement.Character.IsPlayerCharacter && troopRosterElement.Character.HeroObject.CanHaveCampaignIssues())
				{
					list.Add(troopRosterElement.Character.HeroObject);
				}
			}
			ConversationSentence.SetObjectsToRepeatOver(list, 5);
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x0013F820 File Offset: 0x0013DA20
		private static bool issue_offer_hero_response_reject_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.GetPersona() == DefaultTraits.PersonaCurt)
			{
				MBTextManager.SetTextVariable("IssueGiverResponseToRejection", new TextObject("{=h2Wle7ZI}Well. That's a pity.", null), false);
			}
			else if (CharacterObject.OneToOneConversationCharacter.GetPersona() == DefaultTraits.PersonaIronic)
			{
				MBTextManager.SetTextVariable("IssueGiverResponseToRejection", new TextObject("{=wbLnJrJA}Ah, well. I can look elsewhere for help, I suppose.", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("IssueGiverResponseToRejection", new TextObject("{=Uoy2tTZJ}Very well. But perhaps you will reconsider later.", null), false);
			}
			return true;
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x0013F898 File Offset: 0x0013DA98
		private static bool issue_offer_player_accept_lord_clickable_condition(out TextObject explanation)
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if (!issueOwnersIssue.LordSolutionCondition(out explanation))
			{
				return false;
			}
			if (Clan.PlayerClan.Influence < (float)issueOwnersIssue.NeededInfluenceForLordSolution)
			{
				explanation = new TextObject("{=hRdhfSs0}You don't have enough influence for this solution. ({NEEDED_INFLUENCE}{INFLUENCE_ICON})", null);
				explanation.SetTextVariable("NEEDED_INFLUENCE", issueOwnersIssue.NeededInfluenceForLordSolution);
				explanation.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
				return false;
			}
			explanation = new TextObject("{=xbvgc8Sp}This solution will cost {INFLUENCE} influence.", null);
			explanation.SetTextVariable("INFLUENCE", issueOwnersIssue.NeededInfluenceForLordSolution);
			return true;
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x0013F91E File Offset: 0x0013DB1E
		private static void issue_offer_player_accept_lord_consequence()
		{
			Hero.OneToOneConversationHero.Issue.StartIssueWithLordSolution();
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0013F930 File Offset: 0x0013DB30
		private bool issue_offer_player_accept_lord_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if (issueOwnersIssue.IsThereLordSolution)
			{
				MBTextManager.SetTextVariable("IssueLordSolutionAcceptByPlayerText", issueOwnersIssue.IssueLordSolutionAcceptByPlayer, false);
				return IssuesCampaignBehavior.IssueLordSolutionCondition();
			}
			return false;
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0013F964 File Offset: 0x0013DB64
		private static bool issue_offer_player_accept_alternative_clickable_condition(out TextObject explanation)
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if ((from m in MobileParty.MainParty.MemberRoster.GetTroopRoster()
				where m.Character.IsHero && !m.Character.IsPlayerCharacter && m.Character.HeroObject.CanHaveCampaignIssues()
				select m).IsEmpty<TroopRosterElement>())
			{
				if (MobileParty.MainParty.IsCurrentlyAtSea)
				{
					explanation = new TextObject("{=3V2BTAfB}You cannot do this action when you are at sea.", null);
				}
				else
				{
					explanation = new TextObject("{=qjpNREwg}You don't have any companions or family members.", null);
				}
				return false;
			}
			if (!issueOwnersIssue.AlternativeSolutionCondition(out explanation))
			{
				return false;
			}
			explanation = null;
			return true;
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0013F9EC File Offset: 0x0013DBEC
		private static bool issue_offer_player_accept_alternative_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if (issueOwnersIssue.IsThereAlternativeSolution)
			{
				MBTextManager.SetTextVariable("IssueAlternativeSolutionAcceptByPlayerText", issueOwnersIssue.IssueAlternativeSolutionAcceptByPlayer, false);
				return true;
			}
			return false;
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0013FA1C File Offset: 0x0013DC1C
		private static bool issue_offer_player_accept_quest_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueQuestSolutionAcceptByPlayerText", issueOwnersIssue.IssueQuestSolutionAcceptByPlayer, false);
			return true;
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x0013FA44 File Offset: 0x0013DC44
		private static bool issue_alternative_solution_brief_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueAlternativeSolutionExplanationByIssueGiverText", issueOwnersIssue.IssueAlternativeSolutionExplanationByIssueGiver, false);
			return true;
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x0013FA6C File Offset: 0x0013DC6C
		private static bool issue_alternative_solution_player_response_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssuePlayerResponseAfterAlternativeExplanationText", issueOwnersIssue.IssuePlayerResponseAfterAlternativeExplanation, false);
			return issueOwnersIssue.IsThereAlternativeSolution;
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x0013FA98 File Offset: 0x0013DC98
		private static bool issue_quest_solution_brief_pre_player_response_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueQuestSolutionExplanationByIssueGiverText", issueOwnersIssue.IssueQuestSolutionExplanationByIssueGiver, false);
			return !issueOwnersIssue.IsThereAlternativeSolution;
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x0013FAC8 File Offset: 0x0013DCC8
		private static bool issue_quest_solution_brief_pre_alternative_solution_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueQuestSolutionExplanationByIssueGiverText", issueOwnersIssue.IssueQuestSolutionExplanationByIssueGiver, false);
			return issueOwnersIssue.IsThereAlternativeSolution;
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0013FAF4 File Offset: 0x0013DCF4
		private static bool issue_lord_solution_player_response_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssuePlayerResponseAfterLordExplanationText", issueOwnersIssue.IssuePlayerResponseAfterLordExplanation, false);
			return true;
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0013FB1C File Offset: 0x0013DD1C
		private static bool issue_lord_solution_brief_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionExplanationByIssueGiverText", issueOwnersIssue.IssueLordSolutionExplanationByIssueGiver, false);
			return true;
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0013FB44 File Offset: 0x0013DD44
		private bool issue_explanation_player_response_pre_quest_solution_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueAcceptByPlayerText", issueOwnersIssue.IssueAcceptByPlayer, false);
			return !issueOwnersIssue.IsThereLordSolution || !IssuesCampaignBehavior.IssueLordSolutionCondition();
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x0013FB7C File Offset: 0x0013DD7C
		private bool issue_explanation_player_response_pre_lord_solution_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueAcceptByPlayerText", issueOwnersIssue.IssueAcceptByPlayer, false);
			return issueOwnersIssue.IsThereLordSolution && IssuesCampaignBehavior.IssueLordSolutionCondition();
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0013FBB0 File Offset: 0x0013DDB0
		private static bool IssueLordSolutionCondition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			return issueOwnersIssue.IssueOwner.CurrentSettlement != null && issueOwnersIssue.IssueOwner.CurrentSettlement.OwnerClan == Clan.PlayerClan;
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0013FBEC File Offset: 0x0013DDEC
		private static bool issue_offered_begin_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			TextObject textObject;
			if (issueOwnersIssue != null && issueOwnersIssue.CheckPreconditions(Hero.OneToOneConversationHero, out textObject))
			{
				MBTextManager.SetTextVariable("IssueBriefByIssueGiverText", issueOwnersIssue.IssueBriefByIssueGiver, false);
				return true;
			}
			return false;
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0013FC28 File Offset: 0x0013DE28
		private static bool issue_not_offered_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			TextObject textObject;
			if (issueOwnersIssue != null && !issueOwnersIssue.CheckPreconditions(Hero.OneToOneConversationHero, out textObject))
			{
				MBTextManager.SetTextVariable("ISSUE_NOT_OFFERED_EXPLANATION", textObject, false);
				return true;
			}
			return false;
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0013FC5C File Offset: 0x0013DE5C
		private void leave_on_conversation_end_consequence()
		{
			Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x0013FC79 File Offset: 0x0013DE79
		private static IssueBase GetIssueOwnersIssue()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			if (oneToOneConversationHero == null)
			{
				return null;
			}
			return oneToOneConversationHero.Issue;
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x0013FC8C File Offset: 0x0013DE8C
		private static IssueBase GetCounterOfferersIssue()
		{
			if (Hero.OneToOneConversationHero != null)
			{
				foreach (IssueBase issueBase in Campaign.Current.IssueManager.Issues.Values)
				{
					if (issueBase.CounterOfferHero == Hero.OneToOneConversationHero && issueBase.IsSolvingWithLordSolution)
					{
						return issueBase;
					}
				}
			}
			return null;
		}

		// Token: 0x040013ED RID: 5101
		private const int MinNotableIssueCountForTowns = 1;

		// Token: 0x040013EE RID: 5102
		private const int MaxNotableIssueCountForTowns = 3;

		// Token: 0x040013EF RID: 5103
		private const int MinNotableIssueCountForVillages = 1;

		// Token: 0x040013F0 RID: 5104
		private const int MaxNotableIssueCountForVillages = 2;

		// Token: 0x040013F1 RID: 5105
		private const float MinIssuePercentageForClanHeroes = 0.1f;

		// Token: 0x040013F2 RID: 5106
		private const float MaxIssuePercentageForClanHeroes = 0.2f;

		// Token: 0x040013F3 RID: 5107
		private float _additionalFrequencyScore;

		// Token: 0x040013F4 RID: 5108
		private List<IssuesCampaignBehavior.IssueData> _cachedIssueDataList = new List<IssuesCampaignBehavior.IssueData>();

		// Token: 0x0200085F RID: 2143
		private struct IssueData
		{
			// Token: 0x06006A2C RID: 27180 RVA: 0x001D9648 File Offset: 0x001D7848
			public IssueData(PotentialIssueData issueData, Hero hero, float score)
			{
				this.PotentialIssueData = issueData;
				this.Hero = hero;
				this.Score = score;
			}

			// Token: 0x04002414 RID: 9236
			public readonly PotentialIssueData PotentialIssueData;

			// Token: 0x04002415 RID: 9237
			public readonly Hero Hero;

			// Token: 0x04002416 RID: 9238
			public readonly float Score;
		}
	}
}
