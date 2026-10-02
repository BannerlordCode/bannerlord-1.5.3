using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions.AgentBehaviors;
using SandBox.Missions.MissionLogics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace SandBox.Issues
{
	// Token: 0x020000B9 RID: 185
	public class ProdigalSonIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00033F41 File Offset: 0x00032141
		private float MaxDistanceForSettlementSelection
		{
			get
			{
				return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default) * 2.18f;
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00033F54 File Offset: 0x00032154
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.CheckForIssue));
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00033F6D File Offset: 0x0003216D
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00033F70 File Offset: 0x00032170
		public void CheckForIssue(Hero hero)
		{
			Hero hero2;
			Hero hero3;
			if (this.ConditionsHold(hero, out hero2, out hero3))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(ProdigalSonIssueBehavior.ProdigalSonIssue), IssueBase.IssueFrequency.Rare, new Tuple<Hero, Hero>(hero2, hero3)));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(ProdigalSonIssueBehavior.ProdigalSonIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00033FE0 File Offset: 0x000321E0
		private bool ConditionsHoldForSettlement(Settlement settlement, Hero issueGiver)
		{
			if (settlement.IsTown && settlement.MapFaction == issueGiver.MapFaction && settlement != issueGiver.CurrentSettlement && settlement.OwnerClan != issueGiver.Clan && settlement.OwnerClan != Clan.PlayerClan)
			{
				if (settlement.HeroesWithoutParty.FirstOrDefault<Hero>((Hero x) => x.CanHaveCampaignIssues() && x.IsGangLeader) != null)
				{
					return settlement.LocationComplex.GetListOfLocations().AnyQ<Location>((Location x) => x.CanBeReserved && !x.IsReserved);
				}
			}
			return false;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0003408C File Offset: 0x0003228C
		private bool ConditionsHold(Hero issueGiver, out Hero selectedHero, out Hero targetHero)
		{
			selectedHero = null;
			targetHero = null;
			if (issueGiver.IsLord && !issueGiver.IsPrisoner && issueGiver.Clan != Clan.PlayerClan && issueGiver.Age > 30f && issueGiver.GetTraitLevel(DefaultTraits.Mercy) <= 0 && (issueGiver.CurrentSettlement != null || issueGiver.PartyBelongedTo != null))
			{
				selectedHero = issueGiver.Clan.AliveLords.GetRandomElementWithPredicate<Hero>((Hero x) => x.IsActive && !x.IsFemale && x.Age < 35f && (int)x.Age + 10 <= (int)issueGiver.Age && !x.IsPrisoner && x.CanHaveCampaignIssues() && x.PartyBelongedTo == null && x.CurrentSettlement != null && x.GovernorOf == null && x.GetTraitLevel(DefaultTraits.Honor) + x.GetTraitLevel(DefaultTraits.Calculating) < 0);
				if (selectedHero != null)
				{
					Settlement settlement = SettlementHelper.FindRandomSettlement((Settlement x) => this.ConditionsHoldForSettlement(x, issueGiver) && x.HeroesWithoutParty.FirstOrDefault<Hero>((Hero y) => y.CanHaveCampaignIssues() && y.IsGangLeader && Campaign.Current.Models.MapDistanceModel.GetDistance(issueGiver.CurrentSettlement, x, false, false, MobileParty.NavigationType.Default) < this.MaxDistanceForSettlementSelection) != null);
					Hero hero;
					if (settlement == null)
					{
						hero = null;
					}
					else
					{
						hero = settlement.HeroesWithoutParty.FirstOrDefault<Hero>((Hero y) => y.CanHaveCampaignIssues() && y.IsGangLeader);
					}
					targetHero = hero;
				}
			}
			return selectedHero != null && targetHero != null;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x000341A4 File Offset: 0x000323A4
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			Tuple<Hero, Hero> tuple = potentialIssueData.RelatedObject as Tuple<Hero, Hero>;
			return new ProdigalSonIssueBehavior.ProdigalSonIssue(issueOwner, tuple.Item1, tuple.Item2);
		}

		// Token: 0x04000417 RID: 1047
		private const IssueBase.IssueFrequency ProdigalSonIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x04000418 RID: 1048
		private const int AgeLimitForSon = 35;

		// Token: 0x04000419 RID: 1049
		private const int AgeLimitForIssueOwner = 30;

		// Token: 0x0400041A RID: 1050
		private const int MinimumAgeDifference = 10;

		// Token: 0x020001D2 RID: 466
		public class ProdigalSonIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06001124 RID: 4388 RVA: 0x0006F90D File Offset: 0x0006DB0D
			public ProdigalSonIssueTypeDefiner()
				: base(345000)
			{
			}

			// Token: 0x06001125 RID: 4389 RVA: 0x0006F91A File Offset: 0x0006DB1A
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(ProdigalSonIssueBehavior.ProdigalSonIssue), 1, null);
				base.AddClassDefinition(typeof(ProdigalSonIssueBehavior.ProdigalSonIssueQuest), 2, null);
			}
		}

		// Token: 0x020001D3 RID: 467
		public class ProdigalSonIssue : IssueBase
		{
			// Token: 0x1700019A RID: 410
			// (get) Token: 0x06001126 RID: 4390 RVA: 0x0006F940 File Offset: 0x0006DB40
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x1700019B RID: 411
			// (get) Token: 0x06001127 RID: 4391 RVA: 0x0006F943 File Offset: 0x0006DB43
			private Clan Clan
			{
				get
				{
					return base.IssueOwner.Clan;
				}
			}

			// Token: 0x1700019C RID: 412
			// (get) Token: 0x06001128 RID: 4392 RVA: 0x0006F950 File Offset: 0x0006DB50
			protected override int RewardGold
			{
				get
				{
					return 1200 + (int)(3000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700019D RID: 413
			// (get) Token: 0x06001129 RID: 4393 RVA: 0x0006F968 File Offset: 0x0006DB68
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=5a6KlSXt}I have a problem. [ib:normal2][if:convo_pondering]My young kinsman {PRODIGAL_SON.LINK} has gone to town to have fun, drinking, wenching and gambling. Many young men do that, but it seems he was a bit reckless. Now he sends news that he owes a large sum of money to {TARGET_HERO.LINK}, one of the local gang bosses in the city of {SETTLEMENT_LINK}. These ruffians are holding him as a “guest” in their house until someone pays his debt.", null);
					StringHelpers.SetCharacterProperties("PRODIGAL_SON", this._prodigalSon.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._targetHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT_LINK", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700019E RID: 414
			// (get) Token: 0x0600112A RID: 4394 RVA: 0x0006F9C9 File Offset: 0x0006DBC9
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=YtS3cgto}What are you planning to do?", null);
				}
			}

			// Token: 0x1700019F RID: 415
			// (get) Token: 0x0600112B RID: 4395 RVA: 0x0006F9D6 File Offset: 0x0006DBD6
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=ZC1slXw1}I'm not inclined to pay the debt. [ib:closed][if:convo_worried]I'm not going to reward this kind of lawlessness, when even the best families aren't safe. I've sent word to the lord of {SETTLEMENT_NAME} but I can't say I expect to hear back, what with the wars and all. I want someone to go there and free the lad. You could pay, I suppose, but I'd prefer it if you taught those bastards a lesson. I'll pay you either way but obviously you get to keep more if you use force.", null);
					textObject.SetTextVariable("SETTLEMENT_NAME", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001A0 RID: 416
			// (get) Token: 0x0600112C RID: 4396 RVA: 0x0006F9FA File Offset: 0x0006DBFA
			public override TextObject IssuePlayerResponseAfterAlternativeExplanation
			{
				get
				{
					return new TextObject("{=4zf1lg6L}I could go myself, or send a companion.", null);
				}
			}

			// Token: 0x170001A1 RID: 417
			// (get) Token: 0x0600112D RID: 4397 RVA: 0x0006FA07 File Offset: 0x0006DC07
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=CWbAoGRu}Yes, I don't care how you solve it. [if:convo_normal]Just solve it any way you like. I reckon {NEEDED_MEN_COUNT} led by someone who knows how to handle thugs could solve this in about {ALTERNATIVE_SOLUTION_DURATION} days. I'd send my own men but it could cause complications for us to go marching in wearing our clan colors in another lord's territory.", null);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_DURATION", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170001A2 RID: 418
			// (get) Token: 0x0600112E RID: 4398 RVA: 0x0006FA38 File Offset: 0x0006DC38
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=aKbyJsho}I will free your kinsman myself.", null);
				}
			}

			// Token: 0x170001A3 RID: 419
			// (get) Token: 0x0600112F RID: 4399 RVA: 0x0006FA45 File Offset: 0x0006DC45
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=PuuVGOyM}I will send {NEEDED_MEN_COUNT} of my men with one of my lieutenants for {ALTERNATIVE_SOLUTION_DURATION} days to help you.", null);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_DURATION", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170001A4 RID: 420
			// (get) Token: 0x06001130 RID: 4400 RVA: 0x0006FA76 File Offset: 0x0006DC76
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=qxhMagyZ}I'm glad someone's on it.[if:convo_relaxed_happy] Just see that they do it quickly.", null);
				}
			}

			// Token: 0x170001A5 RID: 421
			// (get) Token: 0x06001131 RID: 4401 RVA: 0x0006FA83 File Offset: 0x0006DC83
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=mDXzDXKY}Very good. [if:convo_relaxed_happy]I'm sure you'll chose competent men to bring our boy back.", null);
				}
			}

			// Token: 0x170001A6 RID: 422
			// (get) Token: 0x06001132 RID: 4402 RVA: 0x0006FA90 File Offset: 0x0006DC90
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=Z9sp21rl}{QUEST_GIVER.LINK}, a lord from the {QUEST_GIVER_CLAN} clan, asked you to free {?QUEST_GIVER.GENDER}her{?}his{\\?} relative. The young man is currently held by {TARGET_HERO.LINK} a local gang leader because of his debts. {?QUEST_GIVER.GENDER}Lady{?}Lord{\\?} {QUEST_GIVER.LINK} has given you enough gold to settle {?QUEST_GIVER.GENDER}her{?}his{\\?} debts but {?QUEST_GIVER.GENDER}she{?}he{\\?} encourages you to keep the money to yourself and make an example of these criminals so no one would dare to hold a nobleman again. You have sent {COMPANION.LINK} and {NEEDED_MEN_COUNT} men to take care of the situation for you. They should be back in {ALTERNATIVE_SOLUTION_DURATION} days.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._targetHero.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_GIVER_CLAN", base.IssueOwner.Clan.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_DURATION", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170001A7 RID: 423
			// (get) Token: 0x06001133 RID: 4403 RVA: 0x0006FB4C File Offset: 0x0006DD4C
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=IXnvQ8kG}{COMPANION.LINK} and the men you sent with {?COMPANION.GENDER}her{?}him{\\?} safely return with the news of success. {QUEST_GIVER.LINK} is happy and sends you {?QUEST_GIVER.GENDER}her{?}his{\\?} regards with {REWARD}{GOLD_ICON} the money he promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x170001A8 RID: 424
			// (get) Token: 0x06001134 RID: 4404 RVA: 0x0006FBA8 File Offset: 0x0006DDA8
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170001A9 RID: 425
			// (get) Token: 0x06001135 RID: 4405 RVA: 0x0006FBAB File Offset: 0x0006DDAB
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 1 + MathF.Ceiling(3f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001AA RID: 426
			// (get) Token: 0x06001136 RID: 4406 RVA: 0x0006FBC0 File Offset: 0x0006DDC0
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 7 + MathF.Ceiling(7f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001AB RID: 427
			// (get) Token: 0x06001137 RID: 4407 RVA: 0x0006FBD5 File Offset: 0x0006DDD5
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(700f + 900f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001AC RID: 428
			// (get) Token: 0x06001138 RID: 4408 RVA: 0x0006FBEA File Offset: 0x0006DDEA
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170001AD RID: 429
			// (get) Token: 0x06001139 RID: 4409 RVA: 0x0006FBED File Offset: 0x0006DDED
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=Mr2rt8g8}Prodigal Son of {CLAN_NAME}", null);
					textObject.SetTextVariable("CLAN_NAME", this.Clan.Name);
					return textObject;
				}
			}

			// Token: 0x170001AE RID: 430
			// (get) Token: 0x0600113A RID: 4410 RVA: 0x0006FC14 File Offset: 0x0006DE14
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=5puy0Jle}{ISSUE_OWNER.NAME} asks the player to aid a young clan member. He is supposed to have huge gambling debts so the gang leaders holds him as a hostage. You are asked to retrieve him any way possible.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x0600113B RID: 4411 RVA: 0x0006FC48 File Offset: 0x0006DE48
			public ProdigalSonIssue(Hero issueOwner, Hero prodigalSon, Hero targetGangHero)
				: base(issueOwner, CampaignTime.DaysFromNow(50f))
			{
				this._prodigalSon = prodigalSon;
				this._targetHero = targetGangHero;
				this._targetSettlement = this._targetHero.CurrentSettlement;
				this._targetHouse = this._targetSettlement.LocationComplex.GetListOfLocations().FirstOrDefault<Location>((Location x) => x.CanBeReserved && !x.IsReserved);
				TextObject textObject = new TextObject("{=EZ19JOGj}{MENTOR.NAME}'s House", null);
				StringHelpers.SetCharacterProperties("MENTOR", this._targetHero.CharacterObject, textObject, false);
				this._targetHouse.ReserveLocation(textObject, textObject);
				DisableHeroAction.Apply(this._prodigalSon);
			}

			// Token: 0x0600113C RID: 4412 RVA: 0x0006FCFB File Offset: 0x0006DEFB
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._targetHero || hero == this._prodigalSon)
				{
					result = false;
				}
			}

			// Token: 0x0600113D RID: 4413 RVA: 0x0006FD12 File Offset: 0x0006DF12
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x0600113E RID: 4414 RVA: 0x0006FD27 File Offset: 0x0006DF27
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Charm) >= hero.GetSkillValue(DefaultSkills.Roguery)) ? DefaultSkills.Charm : DefaultSkills.Roguery, 120);
			}

			// Token: 0x0600113F RID: 4415 RVA: 0x0006FD54 File Offset: 0x0006DF54
			protected override void OnGameLoad()
			{
				Town town = Town.AllTowns.FirstOrDefault<Town>((Town x) => x.Settlement.LocationComplex.GetListOfLocations().Contains(this._targetHouse));
				if (town != null)
				{
					this._targetSettlement = town.Settlement;
				}
			}

			// Token: 0x06001140 RID: 4416 RVA: 0x0006FD87 File Offset: 0x0006DF87
			protected override void HourlyTick()
			{
			}

			// Token: 0x06001141 RID: 4417 RVA: 0x0006FD89 File Offset: 0x0006DF89
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new ProdigalSonIssueBehavior.ProdigalSonIssueQuest(questId, base.IssueOwner, this._targetHero, this._prodigalSon, this._targetHouse, base.IssueDifficultyMultiplier, CampaignTime.DaysFromNow(24f), this.RewardGold);
			}

			// Token: 0x06001142 RID: 4418 RVA: 0x0006FDBF File Offset: 0x0006DFBF
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x06001143 RID: 4419 RVA: 0x0006FDC4 File Offset: 0x0006DFC4
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				bool flag2 = issueGiver.GetRelationWithPlayer() >= -10f && !issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction) && Clan.PlayerClan.Tier >= 1;
				flag = (flag2 ? IssueBase.PreconditionFlags.None : ((!issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction)) ? ((Clan.PlayerClan.Tier >= 1) ? IssueBase.PreconditionFlags.Relation : IssueBase.PreconditionFlags.ClanTier) : IssueBase.PreconditionFlags.AtWar));
				relationHero = issueGiver;
				skill = null;
				requiredGold = 0;
				return flag2;
			}

			// Token: 0x06001144 RID: 4420 RVA: 0x0006FE4D File Offset: 0x0006E04D
			public override bool IssueStayAliveConditions()
			{
				return this._targetHero.IsActive;
			}

			// Token: 0x06001145 RID: 4421 RVA: 0x0006FE5A File Offset: 0x0006E05A
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x06001146 RID: 4422 RVA: 0x0006FE5C File Offset: 0x0006E05C
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x06001147 RID: 4423 RVA: 0x0006FE6D File Offset: 0x0006E06D
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x06001148 RID: 4424 RVA: 0x0006FE7B File Offset: 0x0006E07B
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x06001149 RID: 4425 RVA: 0x0006FE95 File Offset: 0x0006E095
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				base.AlternativeSolutionHero.AddSkillXp(DefaultSkills.Charm, (float)((int)(700f + 900f * base.IssueDifficultyMultiplier)));
				this.RelationshipChangeWithIssueOwner = 5;
				GainRenownAction.Apply(Hero.MainHero, 3f, false);
			}

			// Token: 0x0600114A RID: 4426 RVA: 0x0006FED2 File Offset: 0x0006E0D2
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -5;
			}

			// Token: 0x0600114B RID: 4427 RVA: 0x0006FEDC File Offset: 0x0006E0DC
			protected override void OnIssueFinalized()
			{
				if (this._prodigalSon.HeroState == Hero.CharacterStates.Disabled)
				{
					this._prodigalSon.ChangeState(Hero.CharacterStates.Released);
				}
			}

			// Token: 0x0600114C RID: 4428 RVA: 0x0006FEF8 File Offset: 0x0006E0F8
			internal static void AutoGeneratedStaticCollectObjectsProdigalSonIssue(object o, List<object> collectedObjects)
			{
				((ProdigalSonIssueBehavior.ProdigalSonIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600114D RID: 4429 RVA: 0x0006FF06 File Offset: 0x0006E106
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._prodigalSon);
				collectedObjects.Add(this._targetHero);
				collectedObjects.Add(this._targetHouse);
			}

			// Token: 0x0600114E RID: 4430 RVA: 0x0006FF33 File Offset: 0x0006E133
			internal static object AutoGeneratedGetMemberValue_prodigalSon(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssue)o)._prodigalSon;
			}

			// Token: 0x0600114F RID: 4431 RVA: 0x0006FF40 File Offset: 0x0006E140
			internal static object AutoGeneratedGetMemberValue_targetHero(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssue)o)._targetHero;
			}

			// Token: 0x06001150 RID: 4432 RVA: 0x0006FF4D File Offset: 0x0006E14D
			internal static object AutoGeneratedGetMemberValue_targetHouse(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssue)o)._targetHouse;
			}

			// Token: 0x0400085A RID: 2138
			private const int IssueDurationInDays = 50;

			// Token: 0x0400085B RID: 2139
			private const int QuestDurationInDays = 24;

			// Token: 0x0400085C RID: 2140
			private const int TroopTierForAlternativeSolution = 2;

			// Token: 0x0400085D RID: 2141
			private const int RequiredSkillValueForAlternativeSolution = 120;

			// Token: 0x0400085E RID: 2142
			[SaveableField(10)]
			private readonly Hero _prodigalSon;

			// Token: 0x0400085F RID: 2143
			[SaveableField(20)]
			private readonly Hero _targetHero;

			// Token: 0x04000860 RID: 2144
			[SaveableField(30)]
			private readonly Location _targetHouse;

			// Token: 0x04000861 RID: 2145
			private Settlement _targetSettlement;
		}

		// Token: 0x020001D4 RID: 468
		public class ProdigalSonIssueQuest : QuestBase
		{
			// Token: 0x170001AF RID: 431
			// (get) Token: 0x06001152 RID: 4434 RVA: 0x0006FF77 File Offset: 0x0006E177
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=Mr2rt8g8}Prodigal Son of {CLAN_NAME}", null);
					textObject.SetTextVariable("CLAN_NAME", base.QuestGiver.Clan.Name);
					return textObject;
				}
			}

			// Token: 0x170001B0 RID: 432
			// (get) Token: 0x06001153 RID: 4435 RVA: 0x0006FFA0 File Offset: 0x0006E1A0
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170001B1 RID: 433
			// (get) Token: 0x06001154 RID: 4436 RVA: 0x0006FFA3 File Offset: 0x0006E1A3
			private Settlement Settlement
			{
				get
				{
					return this._targetHero.CurrentSettlement;
				}
			}

			// Token: 0x170001B2 RID: 434
			// (get) Token: 0x06001155 RID: 4437 RVA: 0x0006FFB0 File Offset: 0x0006E1B0
			private int DebtWithInterest
			{
				get
				{
					return (int)((float)this.RewardGold * 1.1f);
				}
			}

			// Token: 0x170001B3 RID: 435
			// (get) Token: 0x06001156 RID: 4438 RVA: 0x0006FFC0 File Offset: 0x0006E1C0
			private TextObject QuestStartedLog
			{
				get
				{
					TextObject textObject = new TextObject("{=CXw9a1i5}{QUEST_GIVER.LINK}, a {?QUEST_GIVER.GENDER}lady{?}lord{\\?} from the {QUEST_GIVER_CLAN} clan, asked you to go to {SETTLEMENT} to free {?QUEST_GIVER.GENDER}her{?}his{\\?} relative. The young man is currently held by {TARGET_HERO.LINK}, a local gang leader, because of his debts. {QUEST_GIVER.LINK} has suggested that you make an example of the gang so no one would dare to hold a nobleman again. {?QUEST_GIVER.GENDER}She{?}He{\\?} said you can easily find the house in which the young nobleman is held in the town square.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._targetHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_GIVER_CLAN", base.QuestGiver.Clan.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT", this.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001B4 RID: 436
			// (get) Token: 0x06001157 RID: 4439 RVA: 0x00070040 File Offset: 0x0006E240
			private TextObject PlayerDefeatsThugsQuestSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=axLR9bQo}You have defeated the thugs that held {PRODIGAL_SON.LINK} as {QUEST_GIVER.LINK} has asked you to. {?QUEST_GIVER.GENDER}Lady{?}Lord{\\?} {QUEST_GIVER.LINK} soon sends {?QUEST_GIVER.GENDER}her{?}his{\\?} best regards and a sum of {REWARD}{GOLD_ICON} as a reward.", null);
					StringHelpers.SetCharacterProperties("PRODIGAL_SON", this._prodigalSon.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x170001B5 RID: 437
			// (get) Token: 0x06001158 RID: 4440 RVA: 0x0007009C File Offset: 0x0006E29C
			private TextObject PlayerPaysTheDebtQuestSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=skMoB7c6}You have paid the debt that {PRODIGAL_SON.LINK} owes. True to {?TARGET_HERO.GENDER}her{?}his{\\?} word {TARGET_HERO.LINK} releases the boy immediately. Soon after, {?QUEST_GIVER.GENDER}Lady{?}Lord{\\?} {QUEST_GIVER.LINK} sends {?QUEST_GIVER.GENDER}her{?}his{\\?} best regards and a sum of {REWARD}{GOLD_ICON} as a reward.", null);
					StringHelpers.SetCharacterProperties("PRODIGAL_SON", this._prodigalSon.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._targetHero.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x170001B6 RID: 438
			// (get) Token: 0x06001159 RID: 4441 RVA: 0x00070110 File Offset: 0x0006E310
			private TextObject QuestTimeOutFailLog
			{
				get
				{
					TextObject textObject = new TextObject("{=dmijPqWn}You have failed to extract {QUEST_GIVER.LINK}'s relative captive in time. They have moved the boy to a more secure place. Its impossible to find him now. {QUEST_GIVER.LINK} will have to deal with {TARGET_HERO.LINK} himself now. {?QUEST_GIVER.GENDER}She{?}He{\\?} won't be happy to hear this.", null);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._targetHero.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001B7 RID: 439
			// (get) Token: 0x0600115A RID: 4442 RVA: 0x0007015C File Offset: 0x0006E35C
			private TextObject PlayerHasDefeatedQuestFailLog
			{
				get
				{
					TextObject textObject = new TextObject("{=d5a8xQos}You have failed to defeat the thugs that keep {QUEST_GIVER.LINK}'s relative captive. After your assault you learn that they move the boy to a more secure place. Now its impossible to find him. {QUEST_GIVER.LINK} will have to deal with {TARGET_HERO.LINK} himself now. {?QUEST_GIVER.GENDER}She{?}He{\\?} won't be happy to hear this.", null);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._targetHero.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001B8 RID: 440
			// (get) Token: 0x0600115B RID: 4443 RVA: 0x000701A8 File Offset: 0x0006E3A8
			private TextObject PlayerConvincesGangLeaderQuestSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=Rb7g1U2s}You have convinced {TARGET_HERO.LINK} to release {PRODIGAL_SON.LINK}. Soon after, {?QUEST_GIVER.GENDER}Lady{?}Lord{\\?} {QUEST_GIVER.LINK} sends {?QUEST_GIVER.GENDER}her{?}his{\\?} best regards and a sum of {REWARD}{GOLD_ICON} as a reward.", null);
					StringHelpers.SetCharacterProperties("PRODIGAL_SON", this._prodigalSon.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._targetHero.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x170001B9 RID: 441
			// (get) Token: 0x0600115C RID: 4444 RVA: 0x0007021C File Offset: 0x0006E41C
			private TextObject WarDeclaredQuestCancelLog
			{
				get
				{
					TextObject textObject = new TextObject("{=VuqZuSe2}Your clan is now at war with the {QUEST_GIVER.LINK}'s faction. Your agreement has been canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001BA RID: 442
			// (get) Token: 0x0600115D RID: 4445 RVA: 0x00070250 File Offset: 0x0006E450
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001BB RID: 443
			// (get) Token: 0x0600115E RID: 4446 RVA: 0x00070282 File Offset: 0x0006E482
			private TextObject CrimeRatingCancelLog
			{
				get
				{
					TextObject textObject = new TextObject("{=oulvvl52}You are accused in {SETTLEMENT} of a crime, and {QUEST_GIVER.LINK} no longer trusts you in this matter.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT", this.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x0600115F RID: 4447 RVA: 0x000702BD File Offset: 0x0006E4BD
			public ProdigalSonIssueQuest(string questId, Hero questGiver, Hero targetHero, Hero prodigalSon, Location targetHouse, float questDifficulty, CampaignTime duration, int rewardGold)
				: base(questId, questGiver, duration, rewardGold)
			{
				this._targetHero = targetHero;
				this._prodigalSon = prodigalSon;
				this._targetHouse = targetHouse;
				this._questDifficulty = questDifficulty;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06001160 RID: 4448 RVA: 0x000702F8 File Offset: 0x0006E4F8
			protected override void SetDialogs()
			{
				TextObject textObject = new TextObject("{=bQnVtegC}Good, even better. [ib:confident][if:convo_astonished]You can find the house easily when you go to {SETTLEMENT} and walk around the town square. Or you could just speak to this gang leader, {TARGET_HERO.LINK}, and make {?TARGET_HERO.GENDER}her{?}him{\\?} understand and get my boy released. Good luck. I await good news.", null);
				StringHelpers.SetCharacterProperties("TARGET_HERO", this._targetHero.CharacterObject, textObject, false);
				Settlement settlement = ((this._targetHero.CurrentSettlement != null) ? this._targetHero.CurrentSettlement : this._targetHero.PartyBelongedTo.HomeSettlement);
				textObject.SetTextVariable("SETTLEMENT", settlement.EncyclopediaLinkWithName);
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.is_talking_to_quest_giver))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=TkYk5yxn}Yes? Go already. Get our boy back.[if:convo_excited]", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.is_talking_to_quest_giver))
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=kqXxvtwQ}Don't worry I'll free him.", null), null, null, null)
					.NpcLine(new TextObject("{=ddEu5IFQ}I hope so.", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(MapEventHelper.OnConversationEnd))
					.CloseDialog()
					.PlayerOption(new TextObject("{=Jss9UqZC}I'll go right away", null), null, null, null)
					.NpcLine(new TextObject("{=IdKG3IaS}Good to hear that.", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(MapEventHelper.OnConversationEnd))
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetTargetHeroDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetProdigalSonDialogFlow(), this);
			}

			// Token: 0x06001161 RID: 4449 RVA: 0x00070497 File Offset: 0x0006E697
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x06001162 RID: 4450 RVA: 0x0007049F File Offset: 0x0006E69F
			protected override void HourlyTick()
			{
			}

			// Token: 0x06001163 RID: 4451 RVA: 0x000704A4 File Offset: 0x0006E6A4
			protected override void RegisterEvents()
			{
				CampaignEvents.BeforeMissionOpenedEvent.AddNonSerializedListener(this, new Action(this.BeforeMissionOpened));
				CampaignEvents.MissionTickEvent.AddNonSerializedListener(this, new Action<float>(this.OnMissionTick));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionStarted));
			}

			// Token: 0x06001164 RID: 4452 RVA: 0x00070552 File Offset: 0x0006E752
			private void OnMissionStarted(IMission mission)
			{
				ICampaignMission campaignMission = CampaignMission.Current;
				if (((campaignMission != null) ? campaignMission.Location : null) == this._targetHouse)
				{
					this._isFirstMissionTick = true;
				}
			}

			// Token: 0x06001165 RID: 4453 RVA: 0x00070574 File Offset: 0x0006E774
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._prodigalSon || hero == this._targetHero)
				{
					result = false;
				}
			}

			// Token: 0x06001166 RID: 4454 RVA: 0x0007058B File Offset: 0x0006E78B
			public override void OnHeroCanMoveToSettlementInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._prodigalSon)
				{
					result = false;
				}
			}

			// Token: 0x06001167 RID: 4455 RVA: 0x00070599 File Offset: 0x0006E799
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06001168 RID: 4456 RVA: 0x000705AC File Offset: 0x0006E7AC
			private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
			{
				if (victim == this._targetHero || victim == this._prodigalSon)
				{
					TextObject textObject = ((detail == KillCharacterAction.KillCharacterActionDetail.Lost) ? this.TargetHeroDisappearedLogText : this.TargetHeroDiedLogText);
					StringHelpers.SetCharacterProperties("QUEST_TARGET", victim.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					base.AddLog(textObject, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06001169 RID: 4457 RVA: 0x00070619 File Offset: 0x0006E819
			protected override void OnTimedOut()
			{
				this.FinishQuestFail1();
			}

			// Token: 0x0600116A RID: 4458 RVA: 0x00070621 File Offset: 0x0006E821
			protected override void OnFinalize()
			{
				this._targetHouse.RemoveReservation();
			}

			// Token: 0x0600116B RID: 4459 RVA: 0x00070630 File Offset: 0x0006E830
			private void BeforeMissionOpened()
			{
				if (Settlement.CurrentSettlement == this.Settlement && LocationComplex.Current != null)
				{
					if (LocationComplex.Current.GetLocationOfCharacter(this._prodigalSon) == null)
					{
						this.SpawnProdigalSonInHouse();
						if (!this._isHouseFightFinished)
						{
							this.SpawnThugsInHouse();
							this._isMissionFightInitialized = false;
						}
					}
					using (List<AccompanyingCharacter>.Enumerator enumerator = PlayerEncounter.LocationEncounter.CharactersAccompanyingPlayer.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AccompanyingCharacter character = enumerator.Current;
							if (!character.CanEnterLocation(this._targetHouse))
							{
								character.AllowEntranceToLocations((Location x) => character.CanEnterLocation(x) || x == this._targetHouse);
							}
						}
					}
				}
			}

			// Token: 0x0600116C RID: 4460 RVA: 0x00070704 File Offset: 0x0006E904
			private void OnMissionTick(float dt)
			{
				if (CampaignMission.Current.Location == this._targetHouse)
				{
					Mission mission = Mission.Current;
					if (this._isFirstMissionTick)
					{
						Mission.Current.Agents.First<Agent>((Agent x) => x.Character == this._prodigalSon.CharacterObject).GetComponent<CampaignAgentComponent>().AgentNavigator.RemoveBehaviorGroup<AlarmedBehaviorGroup>();
						this._isFirstMissionTick = false;
					}
					if (!this._isMissionFightInitialized && !this._isHouseFightFinished && mission.Agents.Count > 0)
					{
						this._isMissionFightInitialized = true;
						MissionFightHandler missionBehavior = mission.GetMissionBehavior<MissionFightHandler>();
						List<Agent> list = new List<Agent>();
						List<Agent> list2 = new List<Agent>();
						foreach (Agent agent in mission.Agents)
						{
							if (agent.IsEnemyOf(Agent.Main))
							{
								list.Add(agent);
							}
							else if (agent.Team == Agent.Main.Team)
							{
								list2.Add(agent);
							}
						}
						missionBehavior.StartCustomFight(list2, list, false, false, new MissionFightHandler.OnFightEndDelegate(this.HouseFightFinished), float.Epsilon);
						foreach (Agent agent2 in list)
						{
							agent2.Defensiveness = 2f;
						}
					}
				}
			}

			// Token: 0x0600116D RID: 4461 RVA: 0x00070878 File Offset: 0x0006EA78
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				if (base.QuestGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					if (detail == DeclareWarAction.DeclareWarDetail.CausedByCrimeRatingChange)
					{
						this.RelationshipChangeWithQuestGiver = -5;
						Tuple<TraitObject, int>[] array = new Tuple<TraitObject, int>[]
						{
							new Tuple<TraitObject, int>(DefaultTraits.Honor, -50)
						};
						TraitLevelingHelper.OnIssueSolvedThroughQuest(Hero.MainHero, array);
					}
					if (DiplomacyHelper.IsWarCausedByPlayer(faction1, faction2, detail))
					{
						base.CompleteQuestWithFail(this.PlayerDeclaredWarQuestLogText);
						return;
					}
					base.CompleteQuestWithCancel((detail == DeclareWarAction.DeclareWarDetail.CausedByCrimeRatingChange) ? this.CrimeRatingCancelLog : this.WarDeclaredQuestCancelLog);
				}
			}

			// Token: 0x0600116E RID: 4462 RVA: 0x00070900 File Offset: 0x0006EB00
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && ((newKingdom != null && newKingdom.IsAtWarWith(base.QuestGiver.MapFaction)) || (newKingdom == null && clan.IsAtWarWith(base.QuestGiver.MapFaction))))
				{
					base.CompleteQuestWithCancel(this.WarDeclaredQuestCancelLog);
				}
			}

			// Token: 0x0600116F RID: 4463 RVA: 0x00070950 File Offset: 0x0006EB50
			private void HouseFightFinished(bool isPlayerSideWon)
			{
				if (isPlayerSideWon)
				{
					Agent agent = Mission.Current.Agents.FirstOrDefaultQ<Agent>((Agent x) => x.Character == this._prodigalSon.CharacterObject);
					if (agent == null)
					{
						Debug.Print("Prodigal son id: " + this._prodigalSon.CharacterObject.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
						Debug.Print("Mission agent count: " + Mission.Current.Agents.Count, 0, Debug.DebugColor.White, 17592186044416UL);
						foreach (Agent agent2 in Mission.Current.Agents)
						{
							Debug.Print(string.Concat(new object[]
							{
								"Agent: ",
								agent2.Character.Name,
								", id: ",
								agent2.Character.StringId,
								", team: ",
								agent2.Team
							}), 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
					if (agent.Position.Distance(Agent.Main.Position) > agent.GetInteractionDistanceToUsable(Agent.Main))
					{
						ScriptBehavior.AddTargetWithDelegate(agent, new ScriptBehavior.SelectTargetDelegate(this.SelectPlayerAsTarget), null, new ScriptBehavior.OnTargetReachedDelegate(this.OnTargetReached), 0f);
					}
					else
					{
						Agent agent3 = null;
						UsableMachine usableMachine = null;
						WorldFrame invalid = WorldFrame.Invalid;
						this.OnTargetReached(agent, ref agent3, ref usableMachine, ref invalid);
					}
				}
				else
				{
					this.FinishQuestFail2();
				}
				this._isHouseFightFinished = true;
			}

			// Token: 0x06001170 RID: 4464 RVA: 0x00070AF0 File Offset: 0x0006ECF0
			private bool OnTargetReached(Agent agent, ref Agent targetAgent, ref UsableMachine targetUsableMachine, ref WorldFrame targetFrame)
			{
				Mission.Current.GetMissionBehavior<MissionConversationLogic>().StartConversation(agent, false, false);
				targetAgent = null;
				return false;
			}

			// Token: 0x06001171 RID: 4465 RVA: 0x00070B08 File Offset: 0x0006ED08
			private bool SelectPlayerAsTarget(Agent agent, ref Agent targetAgent, ref UsableMachine targetUsableMachine, ref WorldFrame targetFrame, ref float customTargetReachedRangeThreshold, ref float customTargetReachedRotationThreshold)
			{
				targetAgent = null;
				if (agent.Position.Distance(Agent.Main.Position) > agent.GetInteractionDistanceToUsable(Agent.Main))
				{
					targetAgent = Agent.Main;
				}
				return targetAgent != null;
			}

			// Token: 0x06001172 RID: 4466 RVA: 0x00070B4C File Offset: 0x0006ED4C
			private void SpawnProdigalSonInHouse()
			{
				Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(this._prodigalSon.CharacterObject.Race, "_settlement");
				LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new SimpleAgentOrigin(this._prodigalSon.CharacterObject, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common", true, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true, null, false);
				this._targetHouse.AddCharacter(locationCharacter);
			}

			// Token: 0x06001173 RID: 4467 RVA: 0x00070BD4 File Offset: 0x0006EDD4
			private void SpawnThugsInHouse()
			{
				CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("gangster_1");
				CharacterObject object2 = MBObjectManager.Instance.GetObject<CharacterObject>("gangster_2");
				CharacterObject object3 = MBObjectManager.Instance.GetObject<CharacterObject>("gangster_3");
				List<CharacterObject> list = new List<CharacterObject>();
				if (this._questDifficulty < 0.4f)
				{
					list.Add(@object);
					list.Add(@object);
					if (this._questDifficulty >= 0.2f)
					{
						list.Add(object2);
					}
				}
				else if (this._questDifficulty < 0.6f)
				{
					list.Add(@object);
					list.Add(object2);
					list.Add(object2);
				}
				else
				{
					list.Add(object2);
					list.Add(object3);
					list.Add(object3);
				}
				foreach (CharacterObject characterObject in list)
				{
					Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(characterObject.Race, "_settlement");
					LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new SimpleAgentOrigin(characterObject, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "npc_common", true, LocationCharacter.CharacterRelations.Enemy, null, true, false, null, false, false, true, null, false);
					this._targetHouse.AddCharacter(locationCharacter);
				}
			}

			// Token: 0x06001174 RID: 4468 RVA: 0x00070D24 File Offset: 0x0006EF24
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddTrackedObject(this.Settlement);
				base.AddTrackedObject(this._targetHero);
				base.AddLog(this.QuestStartedLog, false);
			}

			// Token: 0x06001175 RID: 4469 RVA: 0x00070D54 File Offset: 0x0006EF54
			private DialogFlow GetProdigalSonDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=DYq30shK}Thank you, {?PLAYER.GENDER}milady{?}sir{\\?}.", null, null, null, null).Condition(() => Hero.OneToOneConversationHero == this._prodigalSon)
					.NpcLine("{=K8TSoRSD}Did {?QUEST_GIVER.GENDER}Lady{?}Lord{\\?} {QUEST_GIVER.LINK} send you to rescue me?", null, null, null, null)
					.Condition(delegate
					{
						StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
						return true;
					})
					.PlayerLine("{=ln3bGyIO}Yes, I'm here to take you back.", null, null, null)
					.NpcLine("{=evIohG6b}Thank you, but there's no need. Once we are out of here I can manage to return on my own.[if:convo_happy] I appreciate your efforts. I'll tell everyone in my clan of your heroism.", null, null, null, null)
					.NpcLine("{=qsJxhNGZ}Safe travels {?PLAYER.GENDER}milady{?}sir{\\?}.", null, null, null, null)
					.Consequence(delegate
					{
						Mission.Current.Agents.First<Agent>((Agent x) => x.Character == this._prodigalSon.CharacterObject).GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>().DisableScriptedBehavior();
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.OnEndHouseMissionDialog;
					})
					.CloseDialog();
			}

			// Token: 0x06001176 RID: 4470 RVA: 0x00070DEC File Offset: 0x0006EFEC
			private DialogFlow GetTargetHeroDialogFlow()
			{
				DialogFlow dialogFlow = DialogFlow.CreateDialogFlow("start", 125).BeginNpcOptions(null, false).NpcOption(new TextObject("{=M0vxXQGB}Yes? Do you have something to say?[ib:closed][if:convo_nonchalant]", null), () => Hero.OneToOneConversationHero == this._targetHero && !this._playerTalkedToTargetHero, null, null, null, null)
					.Consequence(delegate
					{
						StringHelpers.SetCharacterProperties("PRODIGAL_SON", this._prodigalSon.CharacterObject, null, false);
						this._playerTalkedToTargetHero = true;
					})
					.PlayerLine("{=K5DgDU2a}I am here for the boy. {PRODIGAL_SON.LINK}. You know who I mean.", null, null, null)
					.GotoDialogState("start")
					.NpcOption(new TextObject("{=I979VDEn}Yes, did you bring {GOLD_AMOUNT}{GOLD_ICON}? [ib:hip][if:convo_stern]That's what he owes... With an interest of course.", null), delegate
					{
						bool flag = Hero.OneToOneConversationHero == this._targetHero && this._playerTalkedToTargetHero;
						if (flag)
						{
							MBTextManager.SetTextVariable("GOLD_AMOUNT", this.DebtWithInterest);
						}
						return flag;
					}, null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=IboStvbL}Here is the money, now release him!", null, null, null)
					.ClickableCondition(delegate(out TextObject explanation)
					{
						bool flag2 = false;
						if (Hero.MainHero.Gold >= this.DebtWithInterest)
						{
							explanation = null;
							flag2 = true;
						}
						else
						{
							explanation = new TextObject("{=YuLLsAUb}You don't have {GOLD_AMOUNT}{GOLD_ICON}.", null);
							explanation.SetTextVariable("GOLD_AMOUNT", this.DebtWithInterest);
						}
						return flag2;
					})
					.NpcLine("{=7k03GxZ1}It's great doing business with you. I'll order my men to release him immediately.[if:convo_mocking_teasing]", null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.FinishQuestSuccess4))
					.CloseDialog()
					.PlayerOption("{=9pTkQ5o2}It would be in your interest to let this young nobleman go...", null, null, null)
					.Condition(() => !this._playerTriedToPersuade)
					.Consequence(delegate
					{
						this._playerTriedToPersuade = true;
						this._task = this.GetPersuasionTask();
						this.persuasion_start_on_consequence();
					})
					.GotoDialogState("persuade_gang_start_reservation")
					.PlayerOption("{=AwZhx2tT}I will be back.", null, null, null)
					.NpcLine("{=0fp67gxl}Have a good day.", null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.EndNpcOptions();
				this.AddPersuasionDialogs(dialogFlow);
				return dialogFlow;
			}

			// Token: 0x06001177 RID: 4471 RVA: 0x00070F2C File Offset: 0x0006F12C
			private void AddPersuasionDialogs(DialogFlow dialog)
			{
				dialog.AddDialogLine("persuade_gang_introduction", "persuade_gang_start_reservation", "persuade_gang_player_option", "{=EIsQnfLP}Tell me how it's in my interest...[ib:closed][if:convo_nonchalant]", new ConversationSentence.OnConditionDelegate(this.persuasion_start_on_condition), null, this, 100, null, null, null);
				dialog.AddDialogLine("persuade_gang_success", "persuade_gang_start_reservation", "close_window", "{=alruamIW}Hmm... You may be right. It's not worth it. I'll release the boy immediately.[ib:hip][if:convo_pondering]", new ConversationSentence.OnConditionDelegate(ConversationManager.GetPersuasionProgressSatisfied), new ConversationSentence.OnConsequenceDelegate(this.persuasion_success_on_consequence), this, int.MaxValue, null, null, null);
				dialog.AddDialogLine("persuade_gang_failed", "persuade_gang_start_reservation", "start", "{=1YGgXOB7}Meh... Do you think ruling the streets of a city is easy? You underestimate us. Now, about the money.[ib:closed2][if:convo_nonchalant]", null, new ConversationSentence.OnConsequenceDelegate(ConversationManager.EndPersuasion), this, 100, null, null, null);
				string text = "persuade_gang_player_option_1";
				string text2 = "persuade_gang_player_option";
				string text3 = "persuade_gang_player_option_response";
				string text4 = "{=!}{PERSUADE_GANG_ATTEMPT_1}";
				ConversationSentence.OnConditionDelegate onConditionDelegate = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_1_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_1_on_consequence);
				ConversationSentence.OnPersuasionOptionDelegate onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_1);
				ConversationSentence.OnClickableConditionDelegate onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_1_on_condition);
				dialog.AddPlayerLine(text, text2, text3, text4, onConditionDelegate, onConsequenceDelegate, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text5 = "persuade_gang_player_option_2";
				string text6 = "persuade_gang_player_option";
				string text7 = "persuade_gang_player_option_response";
				string text8 = "{=!}{PERSUADE_GANG_ATTEMPT_2}";
				ConversationSentence.OnConditionDelegate onConditionDelegate2 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_2_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate2 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_2_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_2);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_2_on_condition);
				dialog.AddPlayerLine(text5, text6, text7, text8, onConditionDelegate2, onConsequenceDelegate2, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text9 = "persuade_gang_player_option_3";
				string text10 = "persuade_gang_player_option";
				string text11 = "persuade_gang_player_option_response";
				string text12 = "{=!}{PERSUADE_GANG_ATTEMPT_3}";
				ConversationSentence.OnConditionDelegate onConditionDelegate3 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_3_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate3 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_3_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_3);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_3_on_condition);
				dialog.AddPlayerLine(text9, text10, text11, text12, onConditionDelegate3, onConsequenceDelegate3, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				dialog.AddDialogLine("persuade_gang_option_reaction", "persuade_gang_player_option_response", "persuade_gang_start_reservation", "{=!}{PERSUASION_REACTION}", new ConversationSentence.OnConditionDelegate(this.persuasion_selected_option_response_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_selected_option_response_on_consequence), this, 100, null, null, null);
			}

			// Token: 0x06001178 RID: 4472 RVA: 0x00071106 File Offset: 0x0006F306
			private bool is_talking_to_quest_giver()
			{
				return Hero.OneToOneConversationHero == base.QuestGiver;
			}

			// Token: 0x06001179 RID: 4473 RVA: 0x00071118 File Offset: 0x0006F318
			private bool persuasion_start_on_condition()
			{
				if (Hero.OneToOneConversationHero == this._targetHero && !ConversationManager.GetPersuasionIsFailure())
				{
					return this._task.Options.Any<PersuasionOptionArgs>((PersuasionOptionArgs x) => !x.IsBlocked);
				}
				return false;
			}

			// Token: 0x0600117A RID: 4474 RVA: 0x0007116C File Offset: 0x0006F36C
			private void persuasion_selected_option_response_on_consequence()
			{
				Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
				float difficulty = Campaign.Current.Models.PersuasionModel.GetDifficulty(PersuasionDifficulty.Hard);
				float num;
				float num2;
				Campaign.Current.Models.PersuasionModel.GetEffectChances(tuple.Item1, out num, out num2, difficulty);
				this._task.ApplyEffects(num, num2);
			}

			// Token: 0x0600117B RID: 4475 RVA: 0x000711C8 File Offset: 0x0006F3C8
			private bool persuasion_selected_option_response_on_condition()
			{
				PersuasionOptionResult item = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>().Item2;
				MBTextManager.SetTextVariable("PERSUASION_REACTION", PersuasionHelper.GetDefaultPersuasionOptionReaction(item), false);
				if (item == PersuasionOptionResult.CriticalFailure)
				{
					this._task.BlockAllOptions();
				}
				return true;
			}

			// Token: 0x0600117C RID: 4476 RVA: 0x00071208 File Offset: 0x0006F408
			private bool persuasion_select_option_1_on_condition()
			{
				if (this._task.Options.Count > 0)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(0), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(0).Line);
					MBTextManager.SetTextVariable("PERSUADE_GANG_ATTEMPT_1", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x0600117D RID: 4477 RVA: 0x00071288 File Offset: 0x0006F488
			private bool persuasion_select_option_2_on_condition()
			{
				if (this._task.Options.Count > 1)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(1), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(1).Line);
					MBTextManager.SetTextVariable("PERSUADE_GANG_ATTEMPT_2", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x0600117E RID: 4478 RVA: 0x00071308 File Offset: 0x0006F508
			private bool persuasion_select_option_3_on_condition()
			{
				if (this._task.Options.Count > 2)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(2), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(2).Line);
					MBTextManager.SetTextVariable("PERSUADE_GANG_ATTEMPT_3", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x0600117F RID: 4479 RVA: 0x00071388 File Offset: 0x0006F588
			private void persuasion_select_option_1_on_consequence()
			{
				if (this._task.Options.Count > 0)
				{
					this._task.Options[0].BlockTheOption(true);
				}
			}

			// Token: 0x06001180 RID: 4480 RVA: 0x000713B4 File Offset: 0x0006F5B4
			private void persuasion_select_option_2_on_consequence()
			{
				if (this._task.Options.Count > 1)
				{
					this._task.Options[1].BlockTheOption(true);
				}
			}

			// Token: 0x06001181 RID: 4481 RVA: 0x000713E0 File Offset: 0x0006F5E0
			private void persuasion_select_option_3_on_consequence()
			{
				if (this._task.Options.Count > 2)
				{
					this._task.Options[2].BlockTheOption(true);
				}
			}

			// Token: 0x06001182 RID: 4482 RVA: 0x0007140C File Offset: 0x0006F60C
			private PersuasionOptionArgs persuasion_setup_option_1()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(0);
			}

			// Token: 0x06001183 RID: 4483 RVA: 0x0007141F File Offset: 0x0006F61F
			private PersuasionOptionArgs persuasion_setup_option_2()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(1);
			}

			// Token: 0x06001184 RID: 4484 RVA: 0x00071432 File Offset: 0x0006F632
			private PersuasionOptionArgs persuasion_setup_option_3()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(2);
			}

			// Token: 0x06001185 RID: 4485 RVA: 0x00071448 File Offset: 0x0006F648
			private bool persuasion_clickable_option_1_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 0)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked;
				}
				return false;
			}

			// Token: 0x06001186 RID: 4486 RVA: 0x000714B0 File Offset: 0x0006F6B0
			private bool persuasion_clickable_option_2_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 1)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(1).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(1).IsBlocked;
				}
				return false;
			}

			// Token: 0x06001187 RID: 4487 RVA: 0x00071518 File Offset: 0x0006F718
			private bool persuasion_clickable_option_3_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 2)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(2).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(2).IsBlocked;
				}
				return false;
			}

			// Token: 0x06001188 RID: 4488 RVA: 0x0007157F File Offset: 0x0006F77F
			private void persuasion_success_on_consequence()
			{
				ConversationManager.EndPersuasion();
				this.FinishQuestSuccess3();
			}

			// Token: 0x06001189 RID: 4489 RVA: 0x0007158C File Offset: 0x0006F78C
			private void OnEndHouseMissionDialog()
			{
				Campaign.Current.GameMenuManager.NextLocation = LocationComplex.Current.GetLocationWithId("center");
				Campaign.Current.GameMenuManager.PreviousLocation = CampaignMission.Current.Location;
				Mission.Current.EndMission();
				this.FinishQuestSuccess1();
			}

			// Token: 0x0600118A RID: 4490 RVA: 0x000715E0 File Offset: 0x0006F7E0
			private PersuasionTask GetPersuasionTask()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = TextObject.GetEmpty();
				persuasionTask.TryLaterLine = TextObject.GetEmpty();
				persuasionTask.SpokenLine = new TextObject("{=6P1ruzsC}Maybe...", null);
				PersuasionOptionArgs persuasionOptionArgs = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=Lol4clzR}Look, it was a good try, but they're not going to pay. Releasing the kid is the only move that makes sense.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs);
				PersuasionOptionArgs persuasionOptionArgs2 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Mercy, TraitEffect.Negative, PersuasionArgumentStrength.Hard, false, new TextObject("{=wJCVlVF7}These nobles aren't like you and me. They've kept their wealth by crushing people like you for generations. Don't mess with them.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs2);
				PersuasionOptionArgs persuasionOptionArgs3 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Generosity, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=o1KOn4WZ}If you let this boy go, his family will remember you did them a favor. That's a better deal for you than a fight you can't hope to win.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs3);
				return persuasionTask;
			}

			// Token: 0x0600118B RID: 4491 RVA: 0x00071696 File Offset: 0x0006F896
			private void persuasion_start_on_consequence()
			{
				ConversationManager.StartPersuasion(2f, 1f, 1f, 2f, 2f, 0f, PersuasionDifficulty.Hard);
			}

			// Token: 0x0600118C RID: 4492 RVA: 0x000716BC File Offset: 0x0006F8BC
			private void FinishQuestSuccess1()
			{
				base.CompleteQuestWithSuccess();
				base.AddLog(this.PlayerDefeatsThugsQuestSuccessLog, false);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, 5, true, true);
				GainRenownAction.Apply(Hero.MainHero, 3f, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
			}

			// Token: 0x0600118D RID: 4493 RVA: 0x00071710 File Offset: 0x0006F910
			private void FinishQuestSuccess3()
			{
				base.CompleteQuestWithSuccess();
				base.AddLog(this.PlayerConvincesGangLeaderQuestSuccessLog, false);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, 5, true, true);
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
			}

			// Token: 0x0600118E RID: 4494 RVA: 0x00071764 File Offset: 0x0006F964
			private void FinishQuestSuccess4()
			{
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, this._targetHero, this.DebtWithInterest, false);
				base.CompleteQuestWithSuccess();
				base.AddLog(this.PlayerPaysTheDebtQuestSuccessLog, false);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, 5, true, true);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
			}

			// Token: 0x0600118F RID: 4495 RVA: 0x000717CC File Offset: 0x0006F9CC
			private void FinishQuestFail1()
			{
				base.AddLog(this.QuestTimeOutFailLog, false);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -5, true, true);
			}

			// Token: 0x06001190 RID: 4496 RVA: 0x000717EB File Offset: 0x0006F9EB
			private void FinishQuestFail2()
			{
				base.CompleteQuestWithFail(null);
				base.AddLog(this.PlayerHasDefeatedQuestFailLog, false);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -5, true, true);
			}

			// Token: 0x06001191 RID: 4497 RVA: 0x00071811 File Offset: 0x0006FA11
			internal static void AutoGeneratedStaticCollectObjectsProdigalSonIssueQuest(object o, List<object> collectedObjects)
			{
				((ProdigalSonIssueBehavior.ProdigalSonIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06001192 RID: 4498 RVA: 0x0007181F File Offset: 0x0006FA1F
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetHero);
				collectedObjects.Add(this._prodigalSon);
				collectedObjects.Add(this._targetHouse);
			}

			// Token: 0x06001193 RID: 4499 RVA: 0x0007184C File Offset: 0x0006FA4C
			internal static object AutoGeneratedGetMemberValue_targetHero(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssueQuest)o)._targetHero;
			}

			// Token: 0x06001194 RID: 4500 RVA: 0x00071859 File Offset: 0x0006FA59
			internal static object AutoGeneratedGetMemberValue_prodigalSon(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssueQuest)o)._prodigalSon;
			}

			// Token: 0x06001195 RID: 4501 RVA: 0x00071866 File Offset: 0x0006FA66
			internal static object AutoGeneratedGetMemberValue_playerTalkedToTargetHero(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssueQuest)o)._playerTalkedToTargetHero;
			}

			// Token: 0x06001196 RID: 4502 RVA: 0x00071878 File Offset: 0x0006FA78
			internal static object AutoGeneratedGetMemberValue_targetHouse(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssueQuest)o)._targetHouse;
			}

			// Token: 0x06001197 RID: 4503 RVA: 0x00071885 File Offset: 0x0006FA85
			internal static object AutoGeneratedGetMemberValue_questDifficulty(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssueQuest)o)._questDifficulty;
			}

			// Token: 0x06001198 RID: 4504 RVA: 0x00071897 File Offset: 0x0006FA97
			internal static object AutoGeneratedGetMemberValue_isHouseFightFinished(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssueQuest)o)._isHouseFightFinished;
			}

			// Token: 0x06001199 RID: 4505 RVA: 0x000718A9 File Offset: 0x0006FAA9
			internal static object AutoGeneratedGetMemberValue_playerTriedToPersuade(object o)
			{
				return ((ProdigalSonIssueBehavior.ProdigalSonIssueQuest)o)._playerTriedToPersuade;
			}

			// Token: 0x04000862 RID: 2146
			private const PersuasionDifficulty Difficulty = PersuasionDifficulty.Hard;

			// Token: 0x04000863 RID: 2147
			private const int DistanceSquaredToStartConversation = 4;

			// Token: 0x04000864 RID: 2148
			private const int CrimeRatingCancelRelationshipPenalty = -5;

			// Token: 0x04000865 RID: 2149
			private const int CrimeRatingCancelHonorXpPenalty = -50;

			// Token: 0x04000866 RID: 2150
			[SaveableField(10)]
			private readonly Hero _targetHero;

			// Token: 0x04000867 RID: 2151
			[SaveableField(20)]
			private readonly Hero _prodigalSon;

			// Token: 0x04000868 RID: 2152
			[SaveableField(30)]
			private bool _playerTalkedToTargetHero;

			// Token: 0x04000869 RID: 2153
			[SaveableField(40)]
			private readonly Location _targetHouse;

			// Token: 0x0400086A RID: 2154
			[SaveableField(50)]
			private readonly float _questDifficulty;

			// Token: 0x0400086B RID: 2155
			[SaveableField(60)]
			private bool _isHouseFightFinished;

			// Token: 0x0400086C RID: 2156
			[SaveableField(70)]
			private bool _playerTriedToPersuade;

			// Token: 0x0400086D RID: 2157
			private PersuasionTask _task;

			// Token: 0x0400086E RID: 2158
			private bool _isMissionFightInitialized;

			// Token: 0x0400086F RID: 2159
			private bool _isFirstMissionTick;
		}
	}
}
