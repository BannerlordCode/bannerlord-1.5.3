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
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace SandBox.Issues
{
	// Token: 0x020000B8 RID: 184
	public class NotableWantsDaughterFoundIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x060007A1 RID: 1953 RVA: 0x00033CB7 File Offset: 0x00031EB7
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00033CE8 File Offset: 0x00031EE8
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.2.8.31599", 0)))
			{
				foreach (Hero hero in Hero.DeadOrDisabledHeroes)
				{
					if (hero.IsDead && hero.CompanionOf == Clan.PlayerClan && hero.Father != null && hero.Father.IsNotable && hero.Father.CurrentSettlement.IsVillage)
					{
						RemoveCompanionAction.ApplyByDeath(Clan.PlayerClan, hero);
					}
				}
			}
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00033DA0 File Offset: 0x00031FA0
		public void OnCheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue), IssueBase.IssueFrequency.Rare, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00033E04 File Offset: 0x00032004
		private bool ConditionsHold(Hero issueGiver)
		{
			if (issueGiver.IsRuralNotable && issueGiver.CurrentSettlement.IsVillage && issueGiver.CurrentSettlement.Village.Bound != null && issueGiver.CurrentSettlement.Village.Bound.BoundVillages.Count > 2 && issueGiver.CanHaveCampaignIssues() && issueGiver.Age > (float)(Campaign.Current.Models.AgeModel.HeroComesOfAge * 2) && CharacterHelper.GetRandomCompanionTemplateWithPredicate((CharacterObject x) => x.IsFemale && x.Culture == issueGiver.CurrentSettlement.Culture) != null)
			{
				if (issueGiver.CurrentSettlement.Culture.NotableTemplates.Any<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.GangLeader && !x.IsFemale) && issueGiver.GetTraitLevel(DefaultTraits.Mercy) <= 0)
				{
					return issueGiver.GetTraitLevel(DefaultTraits.Generosity) <= 0;
				}
			}
			return false;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00033F2F File Offset: 0x0003212F
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			return new NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue(issueOwner);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00033F37 File Offset: 0x00032137
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04000413 RID: 1043
		private const IssueBase.IssueFrequency NotableWantsDaughterFoundIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x04000414 RID: 1044
		private const int IssueDuration = 30;

		// Token: 0x04000415 RID: 1045
		private const int QuestTimeLimit = 19;

		// Token: 0x04000416 RID: 1046
		private const int BaseRewardGold = 500;

		// Token: 0x020001CD RID: 461
		public class NotableWantsDaughterFoundIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600108A RID: 4234 RVA: 0x0006C4AE File Offset: 0x0006A6AE
			public NotableWantsDaughterFoundIssueTypeDefiner()
				: base(1088000)
			{
			}

			// Token: 0x0600108B RID: 4235 RVA: 0x0006C4BB File Offset: 0x0006A6BB
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue), 1, null);
				base.AddClassDefinition(typeof(NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest), 2, null);
			}
		}

		// Token: 0x020001CE RID: 462
		public class NotableWantsDaughterFoundIssue : IssueBase
		{
			// Token: 0x1700017B RID: 379
			// (get) Token: 0x0600108C RID: 4236 RVA: 0x0006C4E1 File Offset: 0x0006A6E1
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x1700017C RID: 380
			// (get) Token: 0x0600108D RID: 4237 RVA: 0x0006C4E4 File Offset: 0x0006A6E4
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700017D RID: 381
			// (get) Token: 0x0600108E RID: 4238 RVA: 0x0006C4E7 File Offset: 0x0006A6E7
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700017E RID: 382
			// (get) Token: 0x0600108F RID: 4239 RVA: 0x0006C4EA File Offset: 0x0006A6EA
			protected override int RewardGold
			{
				get
				{
					return 500 + MathF.Round(1200f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700017F RID: 383
			// (get) Token: 0x06001090 RID: 4240 RVA: 0x0006C503 File Offset: 0x0006A703
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 2 + MathF.Ceiling(4f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000180 RID: 384
			// (get) Token: 0x06001091 RID: 4241 RVA: 0x0006C518 File Offset: 0x0006A718
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 4 + MathF.Ceiling(5f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000181 RID: 385
			// (get) Token: 0x06001092 RID: 4242 RVA: 0x0006C52D File Offset: 0x0006A72D
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(500f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06001093 RID: 4243 RVA: 0x0006C542 File Offset: 0x0006A742
			public NotableWantsDaughterFoundIssue(Hero issueOwner)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
			}

			// Token: 0x06001094 RID: 4244 RVA: 0x0006C555 File Offset: 0x0006A755
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x17000182 RID: 386
			// (get) Token: 0x06001095 RID: 4245 RVA: 0x0006C56C File Offset: 0x0006A76C
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=x9VgLEzi}Yes... I've suffered a great misfortune. [ib:demure][if:convo_shocked]My daughter, a headstrong girl, has been bewitched by this never-do-well. I told her to stop seeing him but she wouldn't listen! Now she's missing - I'm sure she's been abducted by him! I'm offering a bounty of {BASE_REWARD_GOLD}{GOLD_ICON} to anyone who brings her back. Please {?PLAYER.GENDER}ma'am{?}sir{\\?}! Don't let a father's heart be broken.", null);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x06001096 RID: 4246 RVA: 0x0006C5BB File Offset: 0x0006A7BB
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=35w6g8gM}Tell me more. What's wrong with the man? ", null);
				}
			}

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x06001097 RID: 4247 RVA: 0x0006C5C8 File Offset: 0x0006A7C8
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=IY5b9vZV}Everything is wrong. [if:convo_annoyed]He is from a low family, the kind who is always involved in some land fraud scheme, or seen dealing with known bandits. Every village has a black sheep like that but I never imagined he would get his hooks into my daughter!", null);
				}
			}

			// Token: 0x17000185 RID: 389
			// (get) Token: 0x06001098 RID: 4248 RVA: 0x0006C5D5 File Offset: 0x0006A7D5
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=v0XsM7Zz}If you send your best tracker with a few men, I am sure they will find my girl [if:convo_pondering]and be back to you in no more than {ALTERNATIVE_SOLUTION_WAIT_DAYS} days.", null);
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_WAIT_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x06001099 RID: 4249 RVA: 0x0006C5F4 File Offset: 0x0006A7F4
			public override TextObject IssuePlayerResponseAfterAlternativeExplanation
			{
				get
				{
					return new TextObject("{=Ldp6ckgj}Don't worry, either I or one of my companions should be able to find her and see what's going on.", null);
				}
			}

			// Token: 0x17000187 RID: 391
			// (get) Token: 0x0600109A RID: 4250 RVA: 0x0006C601 File Offset: 0x0006A801
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=uYrxCtDa}I should be able to find her and see what's going on.", null);
				}
			}

			// Token: 0x17000188 RID: 392
			// (get) Token: 0x0600109B RID: 4251 RVA: 0x0006C60E File Offset: 0x0006A80E
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=WSrGHkal}I will have one of my trackers and {REQUIRED_TROOP_AMOUNT} of my men to find your daughter.", null);
					textObject.SetTextVariable("REQUIRED_TROOP_AMOUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					return textObject;
				}
			}

			// Token: 0x17000189 RID: 393
			// (get) Token: 0x0600109C RID: 4252 RVA: 0x0006C62D File Offset: 0x0006A82D
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=mBPcZddA}{?PLAYER.GENDER}Madam{?}Sir{\\?}, we are still waiting [ib:demure][if:convo_undecided_open]for your men to bring my daughter back. I pray for their success.", null);
				}
			}

			// Token: 0x1700018A RID: 394
			// (get) Token: 0x0600109D RID: 4253 RVA: 0x0006C63C File Offset: 0x0006A83C
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=Hhd3KaKu}Thank you, my {?PLAYER.GENDER}lady{?}lord{\\?}. If your men can find my girl and bring her back to me, I will be so grateful.[if:convo_happy] I will pay you {BASE_REWARD_GOLD}{GOLD_ICON} for your trouble.", null);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700018B RID: 395
			// (get) Token: 0x0600109E RID: 4254 RVA: 0x0006C68C File Offset: 0x0006A88C
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=6OmbzoBs}{ISSUE_GIVER.LINK}, a merchant from {ISSUE_GIVER_SETTLEMENT}, has told you that {?ISSUE_GIVER.GENDER}her{?}his{\\?} daughter has gone missing. You choose {COMPANION.LINK} and {REQUIRED_TROOP_AMOUNT} men to search for her and bring her back. You expect them to complete this task and return in {ALTERNATIVE_SOLUTION_DAYS} days.", null);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					textObject.SetTextVariable("ISSUE_GIVER_SETTLEMENT", base.IssueOwner.CurrentSettlement.Name);
					textObject.SetTextVariable("REQUIRED_TROOP_AMOUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x0600109F RID: 4255 RVA: 0x0006C754 File Offset: 0x0006A954
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.ApplySuccessRewards();
				float randomFloat = MBRandom.RandomFloat;
				SkillObject skillObject;
				if (randomFloat <= 0.33f)
				{
					skillObject = DefaultSkills.OneHanded;
				}
				else if (randomFloat <= 0.66f)
				{
					skillObject = DefaultSkills.TwoHanded;
				}
				else
				{
					skillObject = DefaultSkills.Polearm;
				}
				base.AlternativeSolutionHero.AddSkillXp(skillObject, (float)((int)(500f + 1000f * base.IssueDifficultyMultiplier)));
			}

			// Token: 0x060010A0 RID: 4256 RVA: 0x0006C7B8 File Offset: 0x0006A9B8
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -10;
				if (base.IssueOwner.CurrentSettlement.Village.Bound != null)
				{
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security -= 5f;
				}
			}

			// Token: 0x1700018C RID: 396
			// (get) Token: 0x060010A1 RID: 4257 RVA: 0x0006C83C File Offset: 0x0006AA3C
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=MaXA5HJi}Your companions report that the {ISSUE_GIVER.LINK}'s daughter returns to {?ISSUE_GIVER.GENDER}her{?}him{\\?} safe and sound. {?ISSUE_GIVER.GENDER}She{?}He{\\?} is happy and sends {?ISSUE_GIVER.GENDER}her{?}his{\\?} regards with a large pouch of {BASE_REWARD_GOLD}{GOLD_ICON}.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x060010A2 RID: 4258 RVA: 0x0006C894 File Offset: 0x0006AA94
			private void ApplySuccessRewards()
			{
				GainRenownAction.Apply(Hero.MainHero, 2f, false);
				base.IssueOwner.AddPower(10f);
				this.RelationshipChangeWithIssueOwner = 10;
				if (base.IssueOwner.CurrentSettlement.Village.Bound != null)
				{
					base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security += 10f;
				}
			}

			// Token: 0x1700018D RID: 397
			// (get) Token: 0x060010A3 RID: 4259 RVA: 0x0006C90C File Offset: 0x0006AB0C
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=kr68V5pm}{ISSUE_GIVER.NAME} Wants {?ISSUE_GIVER.GENDER}Her{?}His{\\?} Daughter Found", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700018E RID: 398
			// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0006C940 File Offset: 0x0006AB40
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=SkzM5eSv}{ISSUE_GIVER.LINK}'s daughter is missing. {?ISSUE_GIVER.GENDER}She{?}He{\\?} is offering a substantial reward to find the young woman and bring her back safely.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700018F RID: 399
			// (get) Token: 0x060010A5 RID: 4261 RVA: 0x0006C974 File Offset: 0x0006AB74
			public override TextObject IssueAsRumorInSettlement
			{
				get
				{
					TextObject textObject = new TextObject("{=7RyXSkEE}Wouldn't want to be the poor lovesick sap who ran off with {QUEST_GIVER.NAME}'s daughter.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x060010A6 RID: 4262 RVA: 0x0006C9A6 File Offset: 0x0006ABA6
			protected override void OnGameLoad()
			{
			}

			// Token: 0x060010A7 RID: 4263 RVA: 0x0006C9A8 File Offset: 0x0006ABA8
			protected override void HourlyTick()
			{
			}

			// Token: 0x060010A8 RID: 4264 RVA: 0x0006C9AA File Offset: 0x0006ABAA
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(19f), this.RewardGold, base.IssueDifficultyMultiplier);
			}

			// Token: 0x060010A9 RID: 4265 RVA: 0x0006C9CE File Offset: 0x0006ABCE
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x060010AA RID: 4266 RVA: 0x0006C9D1 File Offset: 0x0006ABD1
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Charm) >= hero.GetSkillValue(DefaultSkills.Scouting)) ? DefaultSkills.Charm : DefaultSkills.Scouting, 120);
			}

			// Token: 0x060010AB RID: 4267 RVA: 0x0006C9FE File Offset: 0x0006ABFE
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x060010AC RID: 4268 RVA: 0x0006CA18 File Offset: 0x0006AC18
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x060010AD RID: 4269 RVA: 0x0006CA29 File Offset: 0x0006AC29
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x060010AE RID: 4270 RVA: 0x0006CA38 File Offset: 0x0006AC38
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				bool flag2 = issueGiver.GetRelationWithPlayer() >= -10f && !issueGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction);
				flag = (flag2 ? IssueBase.PreconditionFlags.None : ((!issueGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction)) ? IssueBase.PreconditionFlags.Relation : IssueBase.PreconditionFlags.AtWar));
				relationHero = issueGiver;
				skill = null;
				requiredGold = 0;
				return flag2;
			}

			// Token: 0x060010AF RID: 4271 RVA: 0x0006CAA8 File Offset: 0x0006ACA8
			public override bool IssueStayAliveConditions()
			{
				return !base.IssueOwner.CurrentSettlement.IsRaided && !base.IssueOwner.CurrentSettlement.IsUnderRaid;
			}

			// Token: 0x060010B0 RID: 4272 RVA: 0x0006CAD1 File Offset: 0x0006ACD1
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x060010B1 RID: 4273 RVA: 0x0006CAD3 File Offset: 0x0006ACD3
			internal static void AutoGeneratedStaticCollectObjectsNotableWantsDaughterFoundIssue(object o, List<object> collectedObjects)
			{
				((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060010B2 RID: 4274 RVA: 0x0006CAE1 File Offset: 0x0006ACE1
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x04000841 RID: 2113
			private const int TroopTierForAlternativeSolution = 2;

			// Token: 0x04000842 RID: 2114
			private const int RequiredSkillLevelForAlternativeSolution = 120;
		}

		// Token: 0x020001CF RID: 463
		public class NotableWantsDaughterFoundIssueQuest : QuestBase
		{
			// Token: 0x17000190 RID: 400
			// (get) Token: 0x060010B3 RID: 4275 RVA: 0x0006CAEC File Offset: 0x0006ACEC
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=kr68V5pm}{ISSUE_GIVER.NAME} Wants {?ISSUE_GIVER.GENDER}Her{?}His{\\?} Daughter Found", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000191 RID: 401
			// (get) Token: 0x060010B4 RID: 4276 RVA: 0x0006CB1E File Offset: 0x0006AD1E
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000192 RID: 402
			// (get) Token: 0x060010B5 RID: 4277 RVA: 0x0006CB21 File Offset: 0x0006AD21
			private bool DoesMainPartyHasEnoughScoutingSkill
			{
				get
				{
					return (float)MobilePartyHelper.GetMainPartySkillCounsellor(DefaultSkills.Scouting).GetSkillValue(DefaultSkills.Scouting) >= 150f * this._questDifficultyMultiplier;
				}
			}

			// Token: 0x17000193 RID: 403
			// (get) Token: 0x060010B6 RID: 4278 RVA: 0x0006CB4C File Offset: 0x0006AD4C
			private TextObject PlayerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=1jExD58d}{QUEST_GIVER.LINK}, a merchant from {SETTLEMENT_NAME}, told you that {?QUEST_GIVER.GENDER}her{?}his{\\?} daughter {TARGET_HERO.NAME} has either been abducted or run off with a local rogue. You have agreed to search for her and bring her back to {SETTLEMENT_NAME}. If you cannot find their tracks when you exit settlement, you should visit the nearby villages of {SETTLEMENT_NAME} to look for clues and tracks of the kidnapper.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT_NAME", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("BASE_REWARD_GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x17000194 RID: 404
			// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0006CBD4 File Offset: 0x0006ADD4
			private TextObject SuccessQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=asVE53ac}Daughter returns to {QUEST_GIVER.LINK}. {?QUEST_GIVER.GENDER}She{?}He{\\?} is happy. Sends {?QUEST_GIVER.GENDER}her{?}his{\\?} regards with a large pouch of {BASE_REWARD}{GOLD_ICON}.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("BASE_REWARD", this.RewardGold);
					return textObject;
				}
			}

			// Token: 0x17000195 RID: 405
			// (get) Token: 0x060010B8 RID: 4280 RVA: 0x0006CC28 File Offset: 0x0006AE28
			private TextObject PlayerDefeatedByRogueLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=i1sth9Ls}You were defeated by the rogue. He and {TARGET_HERO.NAME} ran off while you were unconscious. You failed to bring the daughter back to her {?QUEST_GIVER.GENDER}mother{?}father{\\?} as promised to {QUEST_GIVER.LINK}. {?QUEST_GIVER.GENDER}She{?}He{\\?} is furious.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000196 RID: 406
			// (get) Token: 0x060010B9 RID: 4281 RVA: 0x0006CC72 File Offset: 0x0006AE72
			private TextObject FailQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=ak2EMWWR}You failed to bring the daughter back to her {?QUEST_GIVER.GENDER}mother{?}father{\\?} as promised to {QUEST_GIVER.LINK}. {QUEST_GIVER.LINK} is furious", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					return textObject;
				}
			}

			// Token: 0x17000197 RID: 407
			// (get) Token: 0x060010BA RID: 4282 RVA: 0x0006CC96 File Offset: 0x0006AE96
			private TextObject QuestCanceledWarDeclaredLog
			{
				get
				{
					TextObject textObject = new TextObject("{=vW6kBki9}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					return textObject;
				}
			}

			// Token: 0x17000198 RID: 408
			// (get) Token: 0x060010BB RID: 4283 RVA: 0x0006CCBA File Offset: 0x0006AEBA
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					return textObject;
				}
			}

			// Token: 0x17000199 RID: 409
			// (get) Token: 0x060010BC RID: 4284 RVA: 0x0006CCDE File Offset: 0x0006AEDE
			private TextObject VillageRaidedCancelQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=aN85Kfnq}{SETTLEMENT} was raided. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x060010BD RID: 4285 RVA: 0x0006CD20 File Offset: 0x0006AF20
			public NotableWantsDaughterFoundIssueQuest(string questId, Hero questGiver, CampaignTime duration, int baseReward, float issueDifficultyMultiplier)
				: base(questId, questGiver, duration, baseReward)
			{
				this._questDifficultyMultiplier = issueDifficultyMultiplier;
				this._targetVillage = questGiver.CurrentSettlement.Village.Bound.BoundVillages.GetRandomElementWithPredicate<Village>((Village x) => x != questGiver.CurrentSettlement.Village);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture = this._rogueCharacterBasedOnCulture;
				string text = "khuzait";
				Clan clan = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "steppe_bandits");
				rogueCharacterBasedOnCulture.Add(text, (clan != null) ? clan.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture2 = this._rogueCharacterBasedOnCulture;
				string text2 = "vlandia";
				Clan clan2 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
				rogueCharacterBasedOnCulture2.Add(text2, (clan2 != null) ? clan2.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture3 = this._rogueCharacterBasedOnCulture;
				string text3 = "aserai";
				Clan clan3 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "desert_bandits");
				rogueCharacterBasedOnCulture3.Add(text3, (clan3 != null) ? clan3.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture4 = this._rogueCharacterBasedOnCulture;
				string text4 = "battania";
				Clan clan4 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "forest_bandits");
				rogueCharacterBasedOnCulture4.Add(text4, (clan4 != null) ? clan4.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture5 = this._rogueCharacterBasedOnCulture;
				string text5 = "sturgia";
				Clan clan5 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "sea_raiders");
				rogueCharacterBasedOnCulture5.Add(text5, (clan5 != null) ? clan5.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture6 = this._rogueCharacterBasedOnCulture;
				string text6 = "empire_w";
				Clan clan6 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
				rogueCharacterBasedOnCulture6.Add(text6, (clan6 != null) ? clan6.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture7 = this._rogueCharacterBasedOnCulture;
				string text7 = "empire_s";
				Clan clan7 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
				rogueCharacterBasedOnCulture7.Add(text7, (clan7 != null) ? clan7.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture8 = this._rogueCharacterBasedOnCulture;
				string text8 = "empire";
				Clan clan8 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "mountain_bandits");
				rogueCharacterBasedOnCulture8.Add(text8, (clan8 != null) ? clan8.Culture.BanditBoss : null);
				Dictionary<string, CharacterObject> rogueCharacterBasedOnCulture9 = this._rogueCharacterBasedOnCulture;
				string text9 = "nord";
				Clan clan9 = Clan.BanditFactions.FirstOrDefault<Clan>((Clan x) => x.StringId == "sea_raiders");
				rogueCharacterBasedOnCulture9.Add(text9, (clan9 != null) ? clan9.Culture.BanditBoss : null);
				int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
				int num = MBRandom.RandomInt(heroComesOfAge, 25);
				int num2 = MBRandom.RandomInt(heroComesOfAge, 25);
				CharacterObject randomCompanionTemplateWithPredicate = CharacterHelper.GetRandomCompanionTemplateWithPredicate((CharacterObject x) => x.IsFemale && x.Culture == questGiver.CurrentSettlement.Culture);
				this._daughterHero = HeroCreator.CreateSpecialHero(randomCompanionTemplateWithPredicate, questGiver.HomeSettlement, questGiver.Clan, null, num);
				this._daughterHero.HiddenInEncyclopedia = true;
				this._daughterHero.Father = questGiver;
				this._rogueHero = HeroCreator.CreateSpecialHero(this.GetRogueCharacterBasedOnCulture(questGiver.Culture.StringId), questGiver.HomeSettlement, questGiver.Clan, null, num2);
				this._rogueHero.Culture = questGiver.Culture;
				this._rogueHero.HiddenInEncyclopedia = true;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x060010BE RID: 4286 RVA: 0x0006D11C File Offset: 0x0006B31C
			private CharacterObject GetRogueCharacterBasedOnCulture(string cultureStrId)
			{
				CharacterObject characterObject;
				if (this._rogueCharacterBasedOnCulture.ContainsKey(cultureStrId))
				{
					characterObject = this._rogueCharacterBasedOnCulture[cultureStrId];
				}
				else
				{
					characterObject = base.QuestGiver.CurrentSettlement.Culture.NotableTemplates.GetRandomElementWithPredicate<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.GangLeader && !x.IsFemale);
				}
				return characterObject;
			}

			// Token: 0x060010BF RID: 4287 RVA: 0x0006D184 File Offset: 0x0006B384
			protected override void SetDialogs()
			{
				TextObject textObject = new TextObject("{=PZq1EMcx}Thank you for your help. I am still very worried about my girl {TARGET_HERO.FIRSTNAME}. Please find her and bring her back to me as soon as you can.[if:convo_worried]", null);
				StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject, false);
				TextObject textObject2 = new TextObject("{=sglD6abb}Please! Bring my daughter back.", null);
				TextObject textObject3 = new TextObject("{=ddEu5IFQ}I hope so.", null);
				TextObject textObject4 = new TextObject("{=IdKG3IaS}Good to hear that.", null);
				TextObject textObject5 = new TextObject("{=0hXofVLx}Don't worry I'll bring her.", null);
				TextObject textObject6 = new TextObject("{=zpqP5LsC}I'll go right away.", null);
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(textObject, null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver && !this._didPlayerBeatRouge)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(textObject2, null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver && !this._didPlayerBeatRouge)
					.BeginPlayerOptions(null, false)
					.PlayerOption(textObject5, null, null, null)
					.NpcLine(textObject3, null, null, null, null)
					.CloseDialog()
					.PlayerOption(textObject6, null, null, null)
					.NpcLine(textObject4, null, null, null, null)
					.CloseDialog();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRougeDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDaughterAfterFightDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDaughterAfterAcceptDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDaughterAfterPersuadedDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDaughterDialogWhenVillageRaid(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRougeAfterAcceptDialog(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetRogueAfterPersuadedDialog(), this);
			}

			// Token: 0x060010C0 RID: 4288 RVA: 0x0006D336 File Offset: 0x0006B536
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
				if (this._daughterHero != null)
				{
					this._daughterHero.HiddenInEncyclopedia = true;
				}
				if (this._rogueHero != null)
				{
					this._rogueHero.HiddenInEncyclopedia = true;
				}
			}

			// Token: 0x060010C1 RID: 4289 RVA: 0x0006D366 File Offset: 0x0006B566
			protected override void HourlyTick()
			{
			}

			// Token: 0x060010C2 RID: 4290 RVA: 0x0006D368 File Offset: 0x0006B568
			private bool IsRougeHero(IAgent agent)
			{
				return agent.Character == this._rogueHero.CharacterObject;
			}

			// Token: 0x060010C3 RID: 4291 RVA: 0x0006D37D File Offset: 0x0006B57D
			private bool IsDaughterHero(IAgent agent)
			{
				return agent.Character == this._daughterHero.CharacterObject;
			}

			// Token: 0x060010C4 RID: 4292 RVA: 0x0006D392 File Offset: 0x0006B592
			private bool IsMainHero(IAgent agent)
			{
				return agent.Character == CharacterObject.PlayerCharacter;
			}

			// Token: 0x060010C5 RID: 4293 RVA: 0x0006D3A4 File Offset: 0x0006B5A4
			private bool multi_character_conversation_on_condition()
			{
				if (!this._villageIsRaidedTalkWithDaughter && !this._isDaughterPersuaded && !this._didPlayerBeatRouge && !this._acceptedDaughtersEscape && this._isQuestTargetMission && (CharacterObject.OneToOneConversationCharacter == this._daughterHero.CharacterObject || CharacterObject.OneToOneConversationCharacter == this._rogueHero.CharacterObject))
				{
					MBList<Agent> mblist = new MBList<Agent>();
					foreach (Agent agent in Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 100f, mblist))
					{
						if (agent.Character == this._daughterHero.CharacterObject)
						{
							this._daughterAgent = agent;
							if (Mission.Current.GetMissionBehavior<MissionConversationLogic>() != null && Hero.OneToOneConversationHero != this._daughterHero)
							{
								Campaign.Current.ConversationManager.AddConversationAgents(new List<Agent> { this._daughterAgent }, true);
							}
						}
						else if (agent.Character == this._rogueHero.CharacterObject)
						{
							this._rogueAgent = agent;
							if (Mission.Current.GetMissionBehavior<MissionConversationLogic>() != null && Hero.OneToOneConversationHero != this._rogueHero)
							{
								Campaign.Current.ConversationManager.AddConversationAgents(new List<Agent> { this._rogueAgent }, true);
							}
						}
					}
					return this._daughterAgent != null && this._rogueAgent != null && this._daughterAgent.Health > 10f && this._rogueAgent.Health > 10f;
				}
				return false;
			}

			// Token: 0x060010C6 RID: 4294 RVA: 0x0006D55C File Offset: 0x0006B75C
			private bool daughter_conversation_after_fight_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._daughterHero.CharacterObject && this._didPlayerBeatRouge;
			}

			// Token: 0x060010C7 RID: 4295 RVA: 0x0006D578 File Offset: 0x0006B778
			private void multi_agent_conversation_on_consequence()
			{
				this._task = this.GetPersuasionTask();
			}

			// Token: 0x060010C8 RID: 4296 RVA: 0x0006D588 File Offset: 0x0006B788
			private DialogFlow GetRougeDialogFlow()
			{
				TextObject textObject = new TextObject("{=ovFbMMTJ}Who are you? Are you one of the bounty hunters sent by {QUEST_GIVER.LINK} to track us? Like we're animals or something? Look friend, we have done nothing wrong. As you may have figured out already, this woman and I, we love each other. I didn't force her to do anything.[ib:closed][if:convo_innocent_smile]", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				TextObject textObject2 = new TextObject("{=D25oY3j1}Thank you {?PLAYER.GENDER}lady{?}sir{\\?}. For your kindness and understanding. We won't forget this.[ib:demure][if:convo_happy]", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject2, false);
				TextObject textObject3 = new TextObject("{=oL3amiu1}Come {DAUGHTER_NAME.NAME}, let's go before other hounds sniff our trail... I mean... No offense {?PLAYER.GENDER}madam{?}sir{\\?}.", null);
				StringHelpers.SetCharacterProperties("DAUGHTER_NAME", this._daughterHero.CharacterObject, textObject3, false);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject3, false);
				TextObject textObject4 = new TextObject("{=92sbq1YY}I'm no child, {?PLAYER.GENDER}lady{?}sir{\\?}! Draw your weapon! I challenge you to a duel![ib:warrior2][if:convo_excited]", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject4, false);
				TextObject textObject5 = new TextObject("{=jfzErupx}He is right! I ran away with him willingly. I love my {?QUEST_GIVER.GENDER}mother{?}father{\\?},[ib:closed][if:convo_grave] but {?QUEST_GIVER.GENDER}she{?}he{\\?} can be such a tyrant. Please {?PLAYER.GENDER}lady{?}sir{\\?}, if you believe in freedom and love, please leave us be.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject5, false);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject5, false);
				TextObject textObject6 = new TextObject("{=5NljlbLA}Thank you kind {?PLAYER.GENDER}lady{?}sir{\\?}, thank you.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject6, false);
				TextObject textObject7 = new TextObject("{=i5fNZrhh}Please, {?PLAYER.GENDER}lady{?}sir{\\?}. I love him truly and I wish to spend the rest of my life with him.[ib:demure][if:convo_worried] I beg of you, please don't stand in our way.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject7, false);
				TextObject textObject8 = new TextObject("{=0RCdPKj2}Yes {?QUEST_GIVER.GENDER}she{?}he{\\?} would probably be sad. But not because of what you think. See, {QUEST_GIVER.LINK} promised me to one of {?QUEST_GIVER.GENDER}her{?}his{\\?} allies' sons and this will devastate {?QUEST_GIVER.GENDER}her{?}his{\\?} plans. That is true.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject8, false);
				TextObject textObject9 = new TextObject("{=5W7Kxfq9}I understand. If that is the case, I will let you go.", null);
				TextObject textObject10 = new TextObject("{=3XimdHOn}How do I know he's not forcing you to say that?", null);
				TextObject textObject11 = new TextObject("{=zNqDEuAw}But I've promised to find you and return you to your {?QUEST_GIVER.GENDER}mother{?}father{\\?}. {?QUEST_GIVER.GENDER}She{?}He{\\?} would be devastated.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject11, false);
				TextObject textObject12 = new TextObject("{=tuaQ5uU3}I guess the only way to free you from this pretty boy's spell is to kill him.", null);
				TextObject textObject13 = new TextObject("{=HDCmeGhG}I'm sorry but I gave a promise. I don't break my promises.", null);
				TextObject textObject14 = new TextObject("{=VGrHWxzf}This will be a massacre, not a duel, but I'm fine with that.", null);
				TextObject textObject15 = new TextObject("{=sytYViXb}I accept your duel.", null);
				DialogFlow dialogFlow = DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.multi_character_conversation_on_condition))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.multi_agent_conversation_on_consequence))
					.NpcLine(textObject5, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption(textObject9, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject2, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.NpcLine(textObject3, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject6, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerAcceptedDaughtersEscape;
					})
					.CloseDialog()
					.PlayerOption(textObject10, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject7, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.PlayerLine(textObject11, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject8, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.GotoDialogState("start_daughter_persuade_to_come_persuasion")
					.GoBackToDialogState("daughter_persuade_to_come_persuasion_finished")
					.PlayerLine((Hero.MainHero.GetTraitLevel(DefaultTraits.Mercy) < 0) ? textObject12 : textObject13, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), null, null)
					.NpcLine(textObject4, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption(textObject14, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), null, null)
					.NpcLine(new TextObject("{=XWVW0oTB}You bastard![ib:aggressive][if:convo_furious]", null), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerRejectsDuelFight;
					})
					.CloseDialog()
					.PlayerOption(textObject15, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), null, null)
					.NpcLine(new TextObject("{=jqahxjWD}Heaven protect me![ib:aggressive][if:convo_astonished]", null), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsRougeHero), null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerAcceptsDuelFight;
					})
					.CloseDialog()
					.EndPlayerOptions()
					.EndPlayerOptions()
					.CloseDialog();
				this.AddPersuasionDialogs(dialogFlow);
				return dialogFlow;
			}

			// Token: 0x060010C9 RID: 4297 RVA: 0x0006D9C4 File Offset: 0x0006BBC4
			private DialogFlow GetDaughterAfterFightDialog()
			{
				TextObject textObject = new TextObject("{=MN2v1AZQ}I hate you! You killed him! I can't believe it! I will hate you with all my heart till my dying days.[if:convo_angry]", null);
				TextObject textObject2 = new TextObject("{=TTkVcObg}What choice do I have, you heartless bastard?![if:convo_furious]", null);
				TextObject textObject3 = new TextObject("{=XqsrsjiL}I did what I had to do. Pack up, you need to go.", null);
				TextObject textObject4 = new TextObject("{=KQ3aYvp3}Some day you'll see I did you a favor. Pack up, you need to go.", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.daughter_conversation_after_fight_on_condition))
					.PlayerLine((Hero.MainHero.GetTraitLevel(DefaultTraits.Mercy) < 0) ? textObject3 : textObject4, null, null, null)
					.NpcLine(textObject2, null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerWonTheFight;
					})
					.CloseDialog();
			}

			// Token: 0x060010CA RID: 4298 RVA: 0x0006DA68 File Offset: 0x0006BC68
			private DialogFlow GetDaughterAfterAcceptDialog()
			{
				TextObject textObject = new TextObject("{=0Wg00sfN}Thank you, {?PLAYER.GENDER}madam{?}sir{\\?}. We will be moving immediately.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				TextObject textObject2 = new TextObject("{=kUReBc04}Good.", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.daughter_conversation_after_accept_on_condition))
					.PlayerLine(textObject2, null, null, null)
					.CloseDialog();
			}

			// Token: 0x060010CB RID: 4299 RVA: 0x0006DAD4 File Offset: 0x0006BCD4
			private bool daughter_conversation_after_accept_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._daughterHero.CharacterObject && this._acceptedDaughtersEscape;
			}

			// Token: 0x060010CC RID: 4300 RVA: 0x0006DAF0 File Offset: 0x0006BCF0
			private DialogFlow GetDaughterAfterPersuadedDialog()
			{
				TextObject textObject = new TextObject("{=B8bHpJRP}You are right, {?PLAYER.GENDER}my lady{?}sir{\\?}. I should be moving immediately.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				TextObject textObject2 = new TextObject("{=kUReBc04}Good.", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.daughter_conversation_after_persuaded_on_condition))
					.PlayerLine(textObject2, null, null, null)
					.CloseDialog();
			}

			// Token: 0x060010CD RID: 4301 RVA: 0x0006DB5C File Offset: 0x0006BD5C
			private DialogFlow GetDaughterDialogWhenVillageRaid()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=w0HPC53e}Who are you? What do you want from me?[ib:nervous][if:convo_bared_teeth]", null), null, null, null, null).Condition(() => this._villageIsRaidedTalkWithDaughter)
					.PlayerLine(new TextObject("{=iRupMGI0}Calm down! Your father has sent me to find you.", null), null, null, null)
					.NpcLine(new TextObject("{=dwNquUNr}My father? Oh, thank god! I saw terrible things. [ib:nervous2][if:convo_shocked]They took my beloved one and slew many innocents without hesitation.", null), null, null, null, null)
					.PlayerLine("{=HtAr22re}Try to forget all about these and return to your father's house.", null, null, null)
					.NpcLine("{=FgSIsasF}Yes, you are right. I shall be on my way...", null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
						{
							this.ApplyDeliverySuccessConsequences();
							base.CompleteQuestWithSuccess();
							base.AddLog(this.SuccessQuestLogText, false);
							this._villageIsRaidedTalkWithDaughter = false;
						};
					})
					.CloseDialog();
			}

			// Token: 0x060010CE RID: 4302 RVA: 0x0006DBF2 File Offset: 0x0006BDF2
			private bool daughter_conversation_after_persuaded_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._daughterHero.CharacterObject && this._isDaughterPersuaded;
			}

			// Token: 0x060010CF RID: 4303 RVA: 0x0006DC10 File Offset: 0x0006BE10
			private DialogFlow GetRougeAfterAcceptDialog()
			{
				TextObject textObject = new TextObject("{=wlKtDR2z}Thank you, {?PLAYER.GENDER}my lady{?}sir{\\?}.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.rogue_conversation_after_accept_on_condition))
					.PlayerLine(new TextObject("{=0YJGvJ7o}You should leave now.", null), null, null, null)
					.NpcLine(new TextObject("{=6Q4cPOSG}Yes, we will.", null), null, null, null, null)
					.CloseDialog();
			}

			// Token: 0x060010D0 RID: 4304 RVA: 0x0006DC8E File Offset: 0x0006BE8E
			private bool rogue_conversation_after_accept_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._rogueHero.CharacterObject && this._acceptedDaughtersEscape;
			}

			// Token: 0x060010D1 RID: 4305 RVA: 0x0006DCAC File Offset: 0x0006BEAC
			private DialogFlow GetRogueAfterPersuadedDialog()
			{
				TextObject textObject = new TextObject("{=GFt9KiHP}You are right. Maybe we need to persuade {QUEST_GIVER.NAME}.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				TextObject textObject2 = new TextObject("{=btJkBTSF}I am sure you can solve it.", null);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.rogue_conversation_after_persuaded_on_condition))
					.PlayerLine(textObject2, null, null, null)
					.CloseDialog();
			}

			// Token: 0x060010D2 RID: 4306 RVA: 0x0006DD1E File Offset: 0x0006BF1E
			private bool rogue_conversation_after_persuaded_on_condition()
			{
				return CharacterObject.OneToOneConversationCharacter == this._rogueHero.CharacterObject && this._isDaughterPersuaded;
			}

			// Token: 0x060010D3 RID: 4307 RVA: 0x0006DD3C File Offset: 0x0006BF3C
			protected override void OnTimedOut()
			{
				this.OnFailed();
				TextObject textObject = new TextObject("{=KAvwytDK}You didn't bring {DAUGHTER.NAME} to {QUEST_GIVER.LINK}. {?QUEST_GIVER.GENDER}she{?}he{\\?} must be furious.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("DAUGHTER", this._daughterHero.CharacterObject, textObject, false);
				base.AddLog(textObject, false);
			}

			// Token: 0x060010D4 RID: 4308 RVA: 0x0006DD94 File Offset: 0x0006BF94
			private void PlayerAcceptedDaughtersEscape()
			{
				this._acceptedDaughtersEscape = true;
			}

			// Token: 0x060010D5 RID: 4309 RVA: 0x0006DD9D File Offset: 0x0006BF9D
			private void PlayerWonTheFight()
			{
				this._isDaughterCaptured = true;
				Mission.Current.SetMissionMode(MissionMode.StartUp, false);
			}

			// Token: 0x060010D6 RID: 4310 RVA: 0x0006DDB4 File Offset: 0x0006BFB4
			private void ApplyDeliverySuccessConsequences()
			{
				GainRenownAction.Apply(Hero.MainHero, 2f, false);
				base.QuestGiver.AddPower(10f);
				this.RelationshipChangeWithQuestGiver = 10;
				if (base.QuestGiver.CurrentSettlement.Village.Bound != null)
				{
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security += 10f;
				}
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
			}

			// Token: 0x060010D7 RID: 4311 RVA: 0x0006DE40 File Offset: 0x0006C040
			private void PlayerRejectsDuelFight()
			{
				this._rogueAgent = (Agent)MissionConversationLogic.Current.ConversationManager.ConversationAgents.First<IAgent>((IAgent x) => !x.Character.IsFemale);
				List<Agent> list = new List<Agent> { Agent.Main };
				List<Agent> list2 = new List<Agent> { this._rogueAgent };
				MBList<Agent> mblist = new MBList<Agent>();
				foreach (Agent agent in Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 30f, mblist))
				{
					foreach (Hero hero in Hero.MainHero.CompanionsInParty)
					{
						if (agent.Character == hero.CharacterObject)
						{
							list.Add(agent);
							break;
						}
					}
				}
				this._rogueAgent.Health = (float)(150 + list.Count * 20);
				this._rogueAgent.Defensiveness = 1f;
				Mission.Current.GetMissionBehavior<MissionFightHandler>().StartCustomFight(list, list2, false, false, new MissionFightHandler.OnFightEndDelegate(this.StartConversationAfterFight), float.Epsilon);
			}

			// Token: 0x060010D8 RID: 4312 RVA: 0x0006DFBC File Offset: 0x0006C1BC
			private void PlayerAcceptsDuelFight()
			{
				this._rogueAgent = (Agent)MissionConversationLogic.Current.ConversationManager.ConversationAgents.First<IAgent>((IAgent x) => !x.Character.IsFemale);
				List<Agent> list = new List<Agent> { Agent.Main };
				List<Agent> list2 = new List<Agent> { this._rogueAgent };
				MBList<Agent> mblist = new MBList<Agent>();
				foreach (Agent agent in Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 30f, mblist))
				{
					foreach (Hero hero in Hero.MainHero.CompanionsInParty)
					{
						if (agent.Character == hero.CharacterObject)
						{
							agent.SetTeam(Mission.Current.SpectatorTeam, false);
							DailyBehaviorGroup behaviorGroup = agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
							if (behaviorGroup.GetActiveBehavior() is FollowAgentBehavior)
							{
								behaviorGroup.GetBehavior<FollowAgentBehavior>().SetTargetAgent(null);
								break;
							}
							break;
						}
					}
				}
				this._rogueAgent.Health = 200f;
				Mission.Current.GetMissionBehavior<MissionFightHandler>().StartCustomFight(list, list2, false, false, new MissionFightHandler.OnFightEndDelegate(this.StartConversationAfterFight), float.Epsilon);
			}

			// Token: 0x060010D9 RID: 4313 RVA: 0x0006E158 File Offset: 0x0006C358
			private void StartConversationAfterFight(bool isPlayerSideWon)
			{
				if (isPlayerSideWon)
				{
					this._didPlayerBeatRouge = true;
					Campaign.Current.ConversationManager.SetupAndStartMissionConversation(this._daughterAgent, Mission.Current.MainAgent, false);
					TraitLevelingHelper.OnHostileAction(-50);
					return;
				}
				this._playerDefeatedByRogue = true;
			}

			// Token: 0x060010DA RID: 4314 RVA: 0x0006E194 File Offset: 0x0006C394
			private void AddPersuasionDialogs(DialogFlow dialog)
			{
				TextObject textObject = new TextObject("{=ob5SejgJ}I will not abandon my love, {?PLAYER.GENDER}lady{?}sir{\\?}!", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				TextObject textObject2 = new TextObject("{=cqe8FU8M}{?QUEST_GIVER.GENDER}She{?}He{\\?} cares nothing about me! Only about {?QUEST_GIVER.GENDER}her{?}his{\\?} reputation in our district.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject2, false);
				dialog.AddDialogLine("daughter_persuade_to_come_introduction", "start_daughter_persuade_to_come_persuasion", "daughter_persuade_to_come_start_reservation", textObject2.ToString(), null, new ConversationSentence.OnConsequenceDelegate(this.persuasion_start_with_daughter_on_consequence), this, 100, null, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero));
				dialog.AddDialogLine("daughter_persuade_to_come_rejected", "daughter_persuade_to_come_start_reservation", "daughter_persuade_to_come_persuasion_failed", "{=!}{FAILED_PERSUASION_LINE}", new ConversationSentence.OnConditionDelegate(this.daughter_persuade_to_come_persuasion_failed_on_condition), new ConversationSentence.OnConsequenceDelegate(this.daughter_persuade_to_come_persuasion_failed_on_consequence), this, 100, null, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero));
				dialog.AddDialogLine("daughter_persuade_to_come_failed", "daughter_persuade_to_come_persuasion_failed", "daughter_persuade_to_come_persuasion_finished", textObject.ToString(), null, null, this, 100, null, null, null);
				dialog.AddDialogLine("daughter_persuade_to_come_start", "daughter_persuade_to_come_start_reservation", "daughter_persuade_to_come_persuasion_select_option", "{=9b2BETct}I have already decided. Don't expect me to change my mind.", () => !this.daughter_persuade_to_come_persuasion_failed_on_condition(), null, this, 100, null, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero));
				dialog.AddDialogLine("daughter_persuade_to_come_success", "daughter_persuade_to_come_start_reservation", "close_window", "{=3tmXBpRH}You're right. I cannot do this. I will return to my family. ", new ConversationSentence.OnConditionDelegate(ConversationManager.GetPersuasionProgressSatisfied), new ConversationSentence.OnConsequenceDelegate(this.daughter_persuade_to_come_persuasion_success_on_consequence), this, int.MaxValue, null, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero));
				string text = "daughter_persuade_to_come_select_option_1";
				string text2 = "daughter_persuade_to_come_persuasion_select_option";
				string text3 = "daughter_persuade_to_come_persuasion_selected_option_response";
				string text4 = "{=!}{DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_1}";
				ConversationSentence.OnConditionDelegate onConditionDelegate = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_1_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_1_on_consequence);
				ConversationSentence.OnPersuasionOptionDelegate onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_1);
				ConversationSentence.OnClickableConditionDelegate onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_1_on_condition);
				dialog.AddPlayerLine(text, text2, text3, text4, onConditionDelegate, onConsequenceDelegate, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero));
				string text5 = "daughter_persuade_to_come_select_option_2";
				string text6 = "daughter_persuade_to_come_persuasion_select_option";
				string text7 = "daughter_persuade_to_come_persuasion_selected_option_response";
				string text8 = "{=!}{DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_2}";
				ConversationSentence.OnConditionDelegate onConditionDelegate2 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_2_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate2 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_2_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_2);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_2_on_condition);
				dialog.AddPlayerLine(text5, text6, text7, text8, onConditionDelegate2, onConsequenceDelegate2, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero));
				string text9 = "daughter_persuade_to_come_select_option_3";
				string text10 = "daughter_persuade_to_come_persuasion_select_option";
				string text11 = "daughter_persuade_to_come_persuasion_selected_option_response";
				string text12 = "{=!}{DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_3}";
				ConversationSentence.OnConditionDelegate onConditionDelegate3 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_3_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate3 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_3_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_3);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_3_on_condition);
				dialog.AddPlayerLine(text9, text10, text11, text12, onConditionDelegate3, onConsequenceDelegate3, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero));
				string text13 = "daughter_persuade_to_come_select_option_4";
				string text14 = "daughter_persuade_to_come_persuasion_select_option";
				string text15 = "daughter_persuade_to_come_persuasion_selected_option_response";
				string text16 = "{=!}{DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_4}";
				ConversationSentence.OnConditionDelegate onConditionDelegate4 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_4_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate4 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_4_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_4);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_4_on_condition);
				dialog.AddPlayerLine(text13, text14, text15, text16, onConditionDelegate4, onConsequenceDelegate4, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainHero), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsDaughterHero));
				dialog.AddDialogLine("daughter_persuade_to_come_select_option_reaction", "daughter_persuade_to_come_persuasion_selected_option_response", "daughter_persuade_to_come_start_reservation", "{=D0xDRqvm}{PERSUASION_REACTION}", new ConversationSentence.OnConditionDelegate(this.persuasion_selected_option_response_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_selected_option_response_on_consequence), this, 100, null, null, null);
			}

			// Token: 0x060010DB RID: 4315 RVA: 0x0006E514 File Offset: 0x0006C714
			private void persuasion_selected_option_response_on_consequence()
			{
				Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
				float difficulty = Campaign.Current.Models.PersuasionModel.GetDifficulty(PersuasionDifficulty.Hard);
				float num;
				float num2;
				Campaign.Current.Models.PersuasionModel.GetEffectChances(tuple.Item1, out num, out num2, difficulty);
				this._task.ApplyEffects(num, num2);
			}

			// Token: 0x060010DC RID: 4316 RVA: 0x0006E570 File Offset: 0x0006C770
			private bool persuasion_selected_option_response_on_condition()
			{
				PersuasionOptionResult item = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>().Item2;
				MBTextManager.SetTextVariable("PERSUASION_REACTION", PersuasionHelper.GetDefaultPersuasionOptionReaction(item), false);
				return true;
			}

			// Token: 0x060010DD RID: 4317 RVA: 0x0006E5A0 File Offset: 0x0006C7A0
			private bool persuasion_select_option_1_on_condition()
			{
				if (this._task.Options.Count > 0)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(0), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(0).Line);
					MBTextManager.SetTextVariable("DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_1", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060010DE RID: 4318 RVA: 0x0006E620 File Offset: 0x0006C820
			private bool persuasion_select_option_2_on_condition()
			{
				if (this._task.Options.Count > 1)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(1), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(1).Line);
					MBTextManager.SetTextVariable("DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_2", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060010DF RID: 4319 RVA: 0x0006E6A0 File Offset: 0x0006C8A0
			private bool persuasion_select_option_3_on_condition()
			{
				if (this._task.Options.Count > 2)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(2), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(2).Line);
					MBTextManager.SetTextVariable("DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_3", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060010E0 RID: 4320 RVA: 0x0006E720 File Offset: 0x0006C920
			private bool persuasion_select_option_4_on_condition()
			{
				if (this._task.Options.Count > 3)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(3), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(3).Line);
					MBTextManager.SetTextVariable("DAUGHTER_PERSUADE_TO_COME_PERSUADE_ATTEMPT_4", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x060010E1 RID: 4321 RVA: 0x0006E7A0 File Offset: 0x0006C9A0
			private void persuasion_select_option_1_on_consequence()
			{
				if (this._task.Options.Count > 0)
				{
					this._task.Options[0].BlockTheOption(true);
				}
			}

			// Token: 0x060010E2 RID: 4322 RVA: 0x0006E7CC File Offset: 0x0006C9CC
			private void persuasion_select_option_2_on_consequence()
			{
				if (this._task.Options.Count > 1)
				{
					this._task.Options[1].BlockTheOption(true);
				}
			}

			// Token: 0x060010E3 RID: 4323 RVA: 0x0006E7F8 File Offset: 0x0006C9F8
			private void persuasion_select_option_3_on_consequence()
			{
				if (this._task.Options.Count > 2)
				{
					this._task.Options[2].BlockTheOption(true);
				}
			}

			// Token: 0x060010E4 RID: 4324 RVA: 0x0006E824 File Offset: 0x0006CA24
			private void persuasion_select_option_4_on_consequence()
			{
				if (this._task.Options.Count > 3)
				{
					this._task.Options[3].BlockTheOption(true);
				}
			}

			// Token: 0x060010E5 RID: 4325 RVA: 0x0006E850 File Offset: 0x0006CA50
			private PersuasionOptionArgs persuasion_setup_option_1()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(0);
			}

			// Token: 0x060010E6 RID: 4326 RVA: 0x0006E863 File Offset: 0x0006CA63
			private PersuasionOptionArgs persuasion_setup_option_2()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(1);
			}

			// Token: 0x060010E7 RID: 4327 RVA: 0x0006E876 File Offset: 0x0006CA76
			private PersuasionOptionArgs persuasion_setup_option_3()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(2);
			}

			// Token: 0x060010E8 RID: 4328 RVA: 0x0006E889 File Offset: 0x0006CA89
			private PersuasionOptionArgs persuasion_setup_option_4()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(3);
			}

			// Token: 0x060010E9 RID: 4329 RVA: 0x0006E89C File Offset: 0x0006CA9C
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

			// Token: 0x060010EA RID: 4330 RVA: 0x0006E904 File Offset: 0x0006CB04
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

			// Token: 0x060010EB RID: 4331 RVA: 0x0006E96C File Offset: 0x0006CB6C
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

			// Token: 0x060010EC RID: 4332 RVA: 0x0006E9D4 File Offset: 0x0006CBD4
			private bool persuasion_clickable_option_4_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 3)
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(3).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(3).IsBlocked;
				}
				return false;
			}

			// Token: 0x060010ED RID: 4333 RVA: 0x0006EA3C File Offset: 0x0006CC3C
			private PersuasionTask GetPersuasionTask()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=5aDlmdmb}No... No. It does not make sense.", null);
				persuasionTask.TryLaterLine = TextObject.GetEmpty();
				persuasionTask.SpokenLine = new TextObject("{=6P1ruzsC}Maybe...", null);
				PersuasionOptionArgs persuasionOptionArgs = new PersuasionOptionArgs(DefaultSkills.Leadership, DefaultTraits.Honor, TraitEffect.Positive, PersuasionArgumentStrength.Hard, true, new TextObject("{=Nhfl6tcM}Maybe, but that is your duty to your family.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs);
				TextObject textObject = new TextObject("{=lustkZ7s}Perhaps {?QUEST_GIVER.GENDER}she{?}he{\\?} made those plans because {?QUEST_GIVER.GENDER}she{?}he{\\?} loves you.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				PersuasionOptionArgs persuasionOptionArgs2 = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Mercy, TraitEffect.Positive, PersuasionArgumentStrength.VeryEasy, false, textObject, null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs2);
				PersuasionOptionArgs persuasionOptionArgs3 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.VeryHard, false, new TextObject("{=Ns6Svjsn}Do you think this one will be faithful to you over many years? I know a rogue when I see one.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs3);
				PersuasionOptionArgs persuasionOptionArgs4 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Mercy, TraitEffect.Negative, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=2dL6j8Hp}You want to marry a corpse? Because I'm going to kill your lover if you don't listen.", null), null, true, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs4);
				return persuasionTask;
			}

			// Token: 0x060010EE RID: 4334 RVA: 0x0006EB3E File Offset: 0x0006CD3E
			private void persuasion_start_with_daughter_on_consequence()
			{
				ConversationManager.StartPersuasion(2f, 1f, 0f, 2f, 2f, 0f, PersuasionDifficulty.Hard);
			}

			// Token: 0x060010EF RID: 4335 RVA: 0x0006EB64 File Offset: 0x0006CD64
			private void daughter_persuade_to_come_persuasion_success_on_consequence()
			{
				ConversationManager.EndPersuasion();
				this._isDaughterPersuaded = true;
			}

			// Token: 0x060010F0 RID: 4336 RVA: 0x0006EB74 File Offset: 0x0006CD74
			private bool daughter_persuade_to_come_persuasion_failed_on_condition()
			{
				if (this._task.Options.All<PersuasionOptionArgs>((PersuasionOptionArgs x) => x.IsBlocked) && !ConversationManager.GetPersuasionProgressSatisfied())
				{
					MBTextManager.SetTextVariable("FAILED_PERSUASION_LINE", this._task.FinalFailLine, false);
					return true;
				}
				return false;
			}

			// Token: 0x060010F1 RID: 4337 RVA: 0x0006EBD2 File Offset: 0x0006CDD2
			private void daughter_persuade_to_come_persuasion_failed_on_consequence()
			{
				ConversationManager.EndPersuasion();
			}

			// Token: 0x060010F2 RID: 4338 RVA: 0x0006EBDC File Offset: 0x0006CDDC
			private void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (party.IsMainParty && settlement == base.QuestGiver.CurrentSettlement && this._exitedQuestSettlementForTheFirstTime)
				{
					if (this.DoesMainPartyHasEnoughScoutingSkill)
					{
						QuestHelper.AddMapArrowFromPointToTarget(new TextObject("{=YdwLnWa1}Direction of daughter and rogue", null), settlement.Position, this._targetVillage.Settlement.Position, 5f, 0.1f);
						MBInformationManager.AddQuickInformation(new TextObject("{=O15PyNUK}With the help of your scouting skill, you were able to trace their tracks.", null), 0, null, null, "");
						MBInformationManager.AddQuickInformation(new TextObject("{=gOWebWiK}Their direction is marked with an arrow in the campaign map.", null), 0, null, null, "");
						base.AddTrackedObject(this._targetVillage.Settlement);
					}
					else
					{
						foreach (Village village in base.QuestGiver.CurrentSettlement.Village.Bound.BoundVillages)
						{
							if (village != base.QuestGiver.CurrentSettlement.Village)
							{
								this._villagesAndAlreadyVisitedBooleans.Add(village, false);
								base.AddTrackedObject(village.Settlement);
							}
						}
					}
					TextObject textObject = new TextObject("{=FvtAJE2Q}In order to find {QUEST_GIVER.LINK}'s daughter, you have decided to visit nearby villages.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					base.AddLog(textObject, this.DoesMainPartyHasEnoughScoutingSkill);
					this._exitedQuestSettlementForTheFirstTime = false;
				}
				if (party.IsMainParty && settlement == this._targetVillage.Settlement)
				{
					this._isQuestTargetMission = false;
				}
			}

			// Token: 0x060010F3 RID: 4339 RVA: 0x0006ED60 File Offset: 0x0006CF60
			public void OnBeforeMissionOpened()
			{
				if (this._isQuestTargetMission)
				{
					Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("village_center");
					if (locationWithId != null)
					{
						this.HandleRogueEquipment();
						locationWithId.AddCharacter(this.CreateQuestLocationCharacter(this._daughterHero.CharacterObject, LocationCharacter.CharacterRelations.Neutral));
						locationWithId.AddCharacter(this.CreateQuestLocationCharacter(this._rogueHero.CharacterObject, LocationCharacter.CharacterRelations.Neutral));
					}
				}
			}

			// Token: 0x060010F4 RID: 4340 RVA: 0x0006EDC4 File Offset: 0x0006CFC4
			private void HandleRogueEquipment()
			{
				ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("short_sword_t3");
				this._rogueHero.CivilianEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(@object, null, null, false));
				for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
				{
					ItemObject item = this._rogueHero.BattleEquipment[equipmentIndex].Item;
					if (item != null && item.WeaponComponent.PrimaryWeapon.IsShield)
					{
						this._rogueHero.BattleEquipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, default(EquipmentElement));
					}
				}
			}

			// Token: 0x060010F5 RID: 4341 RVA: 0x0006EE50 File Offset: 0x0006D050
			private void OnMissionEnded(IMission mission)
			{
				if (this._isQuestTargetMission)
				{
					this._daughterAgent = null;
					this._rogueAgent = null;
					if (this._isDaughterPersuaded)
					{
						this.ApplyDeliverySuccessConsequences();
						base.CompleteQuestWithSuccess();
						base.AddLog(this.SuccessQuestLogText, false);
						this.RemoveQuestCharacters();
						return;
					}
					if (this._acceptedDaughtersEscape)
					{
						base.CompleteQuestWithFail(this.FailQuestLogText);
						this.RemoveQuestCharacters();
						return;
					}
					if (this._isDaughterCaptured)
					{
						this.ApplyDeliverySuccessConsequences();
						base.CompleteQuestWithSuccess();
						base.AddLog(this.SuccessQuestLogText, false);
						this.RemoveQuestCharacters();
						return;
					}
					if (this._playerDefeatedByRogue)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, this._daughterHero, -5, true);
						base.CompleteQuestWithFail(null);
						base.AddLog(this.PlayerDefeatedByRogueLogText, false);
						this.RemoveQuestCharacters();
					}
				}
			}

			// Token: 0x060010F6 RID: 4342 RVA: 0x0006EF1C File Offset: 0x0006D11C
			private LocationCharacter CreateQuestLocationCharacter(CharacterObject character, LocationCharacter.CharacterRelations relation)
			{
				Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(character.Race, "_settlement");
				Tuple<string, Monster> tuple = new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, character.IsFemale, "_villager"), monsterWithSuffix);
				return new LocationCharacter(new AgentData(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor))).Monster(tuple.Item2), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddCompanionBehaviors), "alley_2", true, relation, tuple.Item1, false, false, null, false, true, true, null, false);
			}

			// Token: 0x060010F7 RID: 4343 RVA: 0x0006EFA3 File Offset: 0x0006D1A3
			private void RemoveQuestCharacters()
			{
				Settlement.CurrentSettlement.LocationComplex.RemoveCharacterIfExists(this._daughterHero);
				Settlement.CurrentSettlement.LocationComplex.RemoveCharacterIfExists(this._rogueHero);
			}

			// Token: 0x060010F8 RID: 4344 RVA: 0x0006EFD0 File Offset: 0x0006D1D0
			private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
			{
				if (party != null && party.IsMainParty && settlement.IsVillage)
				{
					if (this._villagesAndAlreadyVisitedBooleans.ContainsKey(settlement.Village) && !this._villagesAndAlreadyVisitedBooleans[settlement.Village])
					{
						if (settlement.Village != this._targetVillage)
						{
							if (settlement.IsSettlementBusy(this))
							{
								TextObject textObject = (settlement.IsRaided ? new TextObject("{=YTaM6G1E}It seems the village has been raided a short while ago. You found nothing but smoke, fire and crying people.", null) : new TextObject("{=2P3UJ8be}You ask around the village if anyone saw {TARGET_HERO.NAME} or some suspicious characters with a young woman.{newline}{newline}Villagers say that they saw a young man and woman ride in early in the morning. They bought some supplies and trotted off towards {TARGET_VILLAGE}.", null));
								textObject.SetTextVariable("TARGET_VILLAGE", this._targetVillage.Name);
								StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject, false);
								InformationManager.ShowInquiry(new InquiryData(this.Title.ToString(), textObject.ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
							}
							if (!this._isTrackerLogAdded)
							{
								TextObject textObject2 = new TextObject("{=WGi3Zuv7}You asked the villagers around {CURRENT_SETTLEMENT} if they saw a young woman matching the description of {QUEST_GIVER.LINK}'s daughter, {TARGET_HERO.NAME}.{newline}{newline}They said a young woman and a young man dropped by early in the morning to buy some supplies and then rode off towards {TARGET_VILLAGE}.", null);
								textObject2.SetTextVariable("CURRENT_SETTLEMENT", Hero.MainHero.CurrentSettlement.Name);
								textObject2.SetTextVariable("TARGET_VILLAGE", this._targetVillage.Settlement.EncyclopediaLinkWithName);
								StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject2, false);
								StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject2, false);
								base.AddLog(textObject2, false);
								this._isTrackerLogAdded = true;
							}
						}
						else
						{
							InquiryData inquiryData = null;
							if (settlement.IsRaided)
							{
								TextObject textObject3 = new TextObject("{=edoXFdmg}You have found {QUEST_GIVER.NAME}'s daughter.", null);
								StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject3, false);
								TextObject textObject4 = new TextObject("{=aYMW8bWi}Talk to her", null);
								inquiryData = new InquiryData(this.Title.ToString(), textObject3.ToString(), true, false, textObject4.ToString(), null, new Action(this.TalkWithDaughterAfterRaid), null, "", 0f, null, null, null);
							}
							else if (settlement.IsSettlementBusy(this))
							{
								TextObject textObject5 = new TextObject("{=aj4DZYyX}You ask around the village if anyone saw {TARGET_HERO.NAME} or some suspicious characters with a young woman. Villagers say that there was a young man and woman who arrived here exhausted. The villagers allowed them to stay for a while. You should search the village to find her.", null);
								StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject5, false);
								base.AddLog(textObject5, false);
							}
							else
							{
								TextObject textObject6 = new TextObject("{=bbwNIIKI}You ask around the village if anyone saw {TARGET_HERO.NAME} or some suspicious characters with a young woman.{newline}{newline}Villagers say that there was a young man and woman who arrived here exhausted. The villagers allowed them to stay for a while.{newline}You can check the area, and see if they are still hiding here.", null);
								StringHelpers.SetCharacterProperties("TARGET_HERO", this._daughterHero.CharacterObject, textObject6, false);
								inquiryData = new InquiryData(this.Title.ToString(), textObject6.ToString(), true, true, new TextObject("{=bb6e8DoM}Search the village", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), new Action(this.SearchTheVillage), null, "", 0f, null, null, null);
							}
							if (inquiryData != null)
							{
								InformationManager.ShowInquiry(inquiryData, false, false);
							}
						}
						this._villagesAndAlreadyVisitedBooleans[settlement.Village] = true;
					}
					if (settlement == this._targetVillage.Settlement)
					{
						if (!base.IsTracked(this._daughterHero))
						{
							base.AddTrackedObject(this._daughterHero);
						}
						if (!base.IsTracked(this._rogueHero))
						{
							base.AddTrackedObject(this._rogueHero);
						}
						this._isQuestTargetMission = true;
					}
				}
			}

			// Token: 0x060010F9 RID: 4345 RVA: 0x0006F2FE File Offset: 0x0006D4FE
			private void SearchTheVillage()
			{
				VillageEncounter villageEncounter = PlayerEncounter.LocationEncounter as VillageEncounter;
				if (villageEncounter == null)
				{
					return;
				}
				villageEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("village_center"), null, null, null);
			}

			// Token: 0x060010FA RID: 4346 RVA: 0x0006F328 File Offset: 0x0006D528
			private void TalkWithDaughterAfterRaid()
			{
				this._villageIsRaidedTalkWithDaughter = true;
				CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(this._daughterHero.CharacterObject, null, false, false, false, false, false, false));
			}

			// Token: 0x060010FB RID: 4347 RVA: 0x0006F369 File Offset: 0x0006D569
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this.PlayerStartsQuestLogText, false);
			}

			// Token: 0x060010FC RID: 4348 RVA: 0x0006F37F File Offset: 0x0006D57F
			private void CanHeroDie(Hero victim, KillCharacterAction.KillCharacterActionDetail detail, ref bool result)
			{
				if (victim == Hero.MainHero && Settlement.CurrentSettlement == this._targetVillage.Settlement && Mission.Current != null)
				{
					result = false;
				}
			}

			// Token: 0x060010FD RID: 4349 RVA: 0x0006F3A8 File Offset: 0x0006D5A8
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
				CampaignEvents.BeforeMissionOpenedEvent.AddNonSerializedListener(this, new Action(this.OnBeforeMissionOpened));
				CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.CanHeroDieEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.CanHeroDie));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
			}

			// Token: 0x060010FE RID: 4350 RVA: 0x0006F484 File Offset: 0x0006D684
			private void OnRaidCompleted(BattleSideEnum side, RaidEventComponent raidEventComponent)
			{
				if (raidEventComponent.MapEventSettlement == base.QuestGiver.CurrentSettlement)
				{
					base.CompleteQuestWithCancel(this.VillageRaidedCancelQuestLogText);
				}
			}

			// Token: 0x060010FF RID: 4351 RVA: 0x0006F4A5 File Offset: 0x0006D6A5
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._rogueHero || hero == this._daughterHero)
				{
					result = false;
				}
			}

			// Token: 0x06001100 RID: 4352 RVA: 0x0006F4BC File Offset: 0x0006D6BC
			public override void OnHeroCanMoveToSettlementInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._rogueHero || hero == this._daughterHero)
				{
					result = false;
				}
			}

			// Token: 0x06001101 RID: 4353 RVA: 0x0006F4D3 File Offset: 0x0006D6D3
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06001102 RID: 4354 RVA: 0x0006F4E6 File Offset: 0x0006D6E6
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclaredLog);
				}
			}

			// Token: 0x06001103 RID: 4355 RVA: 0x0006F515 File Offset: 0x0006D715
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestCanceledWarDeclaredLog, false);
			}

			// Token: 0x06001104 RID: 4356 RVA: 0x0006F530 File Offset: 0x0006D730
			protected override void OnFinalize()
			{
				if (base.IsTracked(this._targetVillage.Settlement))
				{
					base.RemoveTrackedObject(this._targetVillage.Settlement);
				}
				if (!Hero.MainHero.IsPrisoner && !this.DoesMainPartyHasEnoughScoutingSkill)
				{
					foreach (Village village in base.QuestGiver.CurrentSettlement.BoundVillages)
					{
						if (base.IsTracked(village.Settlement))
						{
							base.RemoveTrackedObject(village.Settlement);
						}
					}
				}
				if (this._rogueHero != null && this._rogueHero.IsAlive)
				{
					KillCharacterAction.ApplyByRemove(this._rogueHero, false, true);
				}
				if (this._daughterHero != null && this._daughterHero.IsAlive)
				{
					KillCharacterAction.ApplyByRemove(this._daughterHero, false, true);
				}
			}

			// Token: 0x06001105 RID: 4357 RVA: 0x0006F61C File Offset: 0x0006D81C
			public override void OnFailed()
			{
				this.RelationshipChangeWithQuestGiver = -10;
				if (base.QuestGiver.CurrentSettlement.Village.Bound != null)
				{
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security -= 5f;
					base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
				}
			}

			// Token: 0x06001106 RID: 4358 RVA: 0x0006F69E File Offset: 0x0006D89E
			internal static void AutoGeneratedStaticCollectObjectsNotableWantsDaughterFoundIssueQuest(object o, List<object> collectedObjects)
			{
				((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06001107 RID: 4359 RVA: 0x0006F6AC File Offset: 0x0006D8AC
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._daughterHero);
				collectedObjects.Add(this._rogueHero);
				collectedObjects.Add(this._targetVillage);
				collectedObjects.Add(this._villagesAndAlreadyVisitedBooleans);
			}

			// Token: 0x06001108 RID: 4360 RVA: 0x0006F6E5 File Offset: 0x0006D8E5
			internal static object AutoGeneratedGetMemberValue_daughterHero(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._daughterHero;
			}

			// Token: 0x06001109 RID: 4361 RVA: 0x0006F6F2 File Offset: 0x0006D8F2
			internal static object AutoGeneratedGetMemberValue_rogueHero(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._rogueHero;
			}

			// Token: 0x0600110A RID: 4362 RVA: 0x0006F6FF File Offset: 0x0006D8FF
			internal static object AutoGeneratedGetMemberValue_isQuestTargetMission(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._isQuestTargetMission;
			}

			// Token: 0x0600110B RID: 4363 RVA: 0x0006F711 File Offset: 0x0006D911
			internal static object AutoGeneratedGetMemberValue_didPlayerBeatRouge(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._didPlayerBeatRouge;
			}

			// Token: 0x0600110C RID: 4364 RVA: 0x0006F723 File Offset: 0x0006D923
			internal static object AutoGeneratedGetMemberValue_exitedQuestSettlementForTheFirstTime(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._exitedQuestSettlementForTheFirstTime;
			}

			// Token: 0x0600110D RID: 4365 RVA: 0x0006F735 File Offset: 0x0006D935
			internal static object AutoGeneratedGetMemberValue_isTrackerLogAdded(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._isTrackerLogAdded;
			}

			// Token: 0x0600110E RID: 4366 RVA: 0x0006F747 File Offset: 0x0006D947
			internal static object AutoGeneratedGetMemberValue_isDaughterPersuaded(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._isDaughterPersuaded;
			}

			// Token: 0x0600110F RID: 4367 RVA: 0x0006F759 File Offset: 0x0006D959
			internal static object AutoGeneratedGetMemberValue_isDaughterCaptured(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._isDaughterCaptured;
			}

			// Token: 0x06001110 RID: 4368 RVA: 0x0006F76B File Offset: 0x0006D96B
			internal static object AutoGeneratedGetMemberValue_acceptedDaughtersEscape(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._acceptedDaughtersEscape;
			}

			// Token: 0x06001111 RID: 4369 RVA: 0x0006F77D File Offset: 0x0006D97D
			internal static object AutoGeneratedGetMemberValue_targetVillage(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._targetVillage;
			}

			// Token: 0x06001112 RID: 4370 RVA: 0x0006F78A File Offset: 0x0006D98A
			internal static object AutoGeneratedGetMemberValue_villageIsRaidedTalkWithDaughter(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._villageIsRaidedTalkWithDaughter;
			}

			// Token: 0x06001113 RID: 4371 RVA: 0x0006F79C File Offset: 0x0006D99C
			internal static object AutoGeneratedGetMemberValue_villagesAndAlreadyVisitedBooleans(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._villagesAndAlreadyVisitedBooleans;
			}

			// Token: 0x06001114 RID: 4372 RVA: 0x0006F7A9 File Offset: 0x0006D9A9
			internal static object AutoGeneratedGetMemberValue_questDifficultyMultiplier(object o)
			{
				return ((NotableWantsDaughterFoundIssueBehavior.NotableWantsDaughterFoundIssueQuest)o)._questDifficultyMultiplier;
			}

			// Token: 0x04000843 RID: 2115
			[SaveableField(10)]
			private readonly Hero _daughterHero;

			// Token: 0x04000844 RID: 2116
			[SaveableField(20)]
			private readonly Hero _rogueHero;

			// Token: 0x04000845 RID: 2117
			private Agent _daughterAgent;

			// Token: 0x04000846 RID: 2118
			private Agent _rogueAgent;

			// Token: 0x04000847 RID: 2119
			[SaveableField(50)]
			private bool _isQuestTargetMission;

			// Token: 0x04000848 RID: 2120
			[SaveableField(60)]
			private bool _didPlayerBeatRouge;

			// Token: 0x04000849 RID: 2121
			[SaveableField(70)]
			private bool _exitedQuestSettlementForTheFirstTime = true;

			// Token: 0x0400084A RID: 2122
			[SaveableField(80)]
			private bool _isTrackerLogAdded;

			// Token: 0x0400084B RID: 2123
			[SaveableField(90)]
			private bool _isDaughterPersuaded;

			// Token: 0x0400084C RID: 2124
			[SaveableField(91)]
			private bool _isDaughterCaptured;

			// Token: 0x0400084D RID: 2125
			[SaveableField(100)]
			private bool _acceptedDaughtersEscape;

			// Token: 0x0400084E RID: 2126
			[SaveableField(110)]
			private readonly Village _targetVillage;

			// Token: 0x0400084F RID: 2127
			[SaveableField(120)]
			private bool _villageIsRaidedTalkWithDaughter;

			// Token: 0x04000850 RID: 2128
			[SaveableField(140)]
			private Dictionary<Village, bool> _villagesAndAlreadyVisitedBooleans = new Dictionary<Village, bool>();

			// Token: 0x04000851 RID: 2129
			private Dictionary<string, CharacterObject> _rogueCharacterBasedOnCulture = new Dictionary<string, CharacterObject>();

			// Token: 0x04000852 RID: 2130
			private bool _playerDefeatedByRogue;

			// Token: 0x04000853 RID: 2131
			private PersuasionTask _task;

			// Token: 0x04000854 RID: 2132
			private const PersuasionDifficulty Difficulty = PersuasionDifficulty.Hard;

			// Token: 0x04000855 RID: 2133
			private const int MaxAgeForDaughterAndRogue = 25;

			// Token: 0x04000856 RID: 2134
			[SaveableField(130)]
			private readonly float _questDifficultyMultiplier;
		}
	}
}
