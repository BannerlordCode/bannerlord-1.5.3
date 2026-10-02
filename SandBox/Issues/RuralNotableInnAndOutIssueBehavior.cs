using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using SandBox.BoardGames.MissionLogics;
using SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;

namespace SandBox.Issues
{
	// Token: 0x020000BB RID: 187
	public class RuralNotableInnAndOutIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x060007C1 RID: 1985 RVA: 0x00034610 File Offset: 0x00032810
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00034629 File Offset: 0x00032829
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0003462C File Offset: 0x0003282C
		private bool ConditionsHold(Hero issueGiver)
		{
			return (issueGiver.IsRuralNotable || issueGiver.IsHeadman) && issueGiver.CurrentSettlement.Village != null && issueGiver.CurrentSettlement.Village.Bound.IsTown && issueGiver.GetTraitLevel(DefaultTraits.Mercy) + issueGiver.GetTraitLevel(DefaultTraits.Honor) < 0 && Campaign.Current.GetCampaignBehavior<BoardGameCampaignBehavior>() != null && issueGiver.CurrentSettlement.Village.Bound.Culture.BoardGame != CultureObject.BoardGameType.None;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000346B8 File Offset: 0x000328B8
		public void OnCheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssue), IssueBase.IssueFrequency.Common, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssue), IssueBase.IssueFrequency.Common));
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0003471C File Offset: 0x0003291C
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			return new RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssue(issueOwner);
		}

		// Token: 0x0400041D RID: 1053
		private const IssueBase.IssueFrequency RuralNotableInnAndOutIssueFrequency = IssueBase.IssueFrequency.Common;

		// Token: 0x0400041E RID: 1054
		private const float IssueDuration = 30f;

		// Token: 0x0400041F RID: 1055
		private const float QuestDuration = 14f;

		// Token: 0x020001DB RID: 475
		public class RuralNotableInnAndOutIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600122B RID: 4651 RVA: 0x00073E79 File Offset: 0x00072079
			public RuralNotableInnAndOutIssueTypeDefiner()
				: base(585900)
			{
			}

			// Token: 0x0600122C RID: 4652 RVA: 0x00073E86 File Offset: 0x00072086
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssue), 1, null);
				base.AddClassDefinition(typeof(RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssueQuest), 2, null);
			}
		}

		// Token: 0x020001DC RID: 476
		public class RuralNotableInnAndOutIssue : IssueBase
		{
			// Token: 0x170001E0 RID: 480
			// (get) Token: 0x0600122D RID: 4653 RVA: 0x00073EAC File Offset: 0x000720AC
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x170001E1 RID: 481
			// (get) Token: 0x0600122E RID: 4654 RVA: 0x00073EAF File Offset: 0x000720AF
			protected override bool IssueQuestCanBeDuplicated
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170001E2 RID: 482
			// (get) Token: 0x0600122F RID: 4655 RVA: 0x00073EB2 File Offset: 0x000720B2
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 1 + MathF.Ceiling(3f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001E3 RID: 483
			// (get) Token: 0x06001230 RID: 4656 RVA: 0x00073EC7 File Offset: 0x000720C7
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 1 + MathF.Ceiling(3f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170001E4 RID: 484
			// (get) Token: 0x06001231 RID: 4657 RVA: 0x00073EDC File Offset: 0x000720DC
			protected override int RewardGold
			{
				get
				{
					return 1000;
				}
			}

			// Token: 0x170001E5 RID: 485
			// (get) Token: 0x06001232 RID: 4658 RVA: 0x00073EE3 File Offset: 0x000720E3
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=uUhtKnfA}Inn and Out", null);
				}
			}

			// Token: 0x170001E6 RID: 486
			// (get) Token: 0x06001233 RID: 4659 RVA: 0x00073EF0 File Offset: 0x000720F0
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=swamqBRq}{ISSUE_OWNER.NAME} wants you to beat the game host", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001E7 RID: 487
			// (get) Token: 0x06001234 RID: 4660 RVA: 0x00073F22 File Offset: 0x00072122
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=T0zupcGB}Ah yes... It is a bit embarrassing to mention, [ib:nervous][if:convo_nervous]but... Well, when I am in town, I often have a drink at the inn and perhaps play a round of {GAME_TYPE} or two. Normally I play for low stakes but let's just say that last time the wine went to my head, and I lost something I couldn't afford to lose.", null);
					textObject.SetTextVariable("GAME_TYPE", GameTexts.FindText("str_boardgame_name", this._boardGameType.ToString()));
					return textObject;
				}
			}

			// Token: 0x170001E8 RID: 488
			// (get) Token: 0x06001235 RID: 4661 RVA: 0x00073F56 File Offset: 0x00072156
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=h2tMadtI}I've heard that story before. What did you lose?", null);
				}
			}

			// Token: 0x170001E9 RID: 489
			// (get) Token: 0x06001236 RID: 4662 RVA: 0x00073F64 File Offset: 0x00072164
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=LD4tGYCA}It's a deed to a plot of farmland. Not a big or valuable plot,[ib:normal][if:convo_disbelief] mind you, but I'd rather not have to explain to my men why they won't be sowing it this year. You can find the man who took it from me at the tavern in {TARGET_SETTLEMENT}. They call him the \"Game Host\". Just be straight about what you're doing. He's in no position to work the land. I don't imagine that he'll turn down a chance to make more money off of it. Bring it back and {REWARD}{GOLD_ICON} is yours.", null);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170001EA RID: 490
			// (get) Token: 0x06001237 RID: 4663 RVA: 0x00073FB6 File Offset: 0x000721B6
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=urCXu9Fc}Well, I could try and buy it from him, but I would not really prefer that.[if:convo_innocent_smile] I would be the joke of the tavern for months to come... If you choose to do that, I can only offer {REWARD}{GOLD_ICON} to compensate for your payment. If you have a man with a knack for such games he might do the trick.", null);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x170001EB RID: 491
			// (get) Token: 0x06001238 RID: 4664 RVA: 0x00073FE6 File Offset: 0x000721E6
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=KMThnMbt}I'll go to the tavern and win it back the same way you lost it.", null);
				}
			}

			// Token: 0x170001EC RID: 492
			// (get) Token: 0x06001239 RID: 4665 RVA: 0x00073FF4 File Offset: 0x000721F4
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=QdKWaabR}Worry not {ISSUE_OWNER.NAME}, my men will be back with your deed in no time.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001ED RID: 493
			// (get) Token: 0x0600123A RID: 4666 RVA: 0x00074026 File Offset: 0x00072226
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=1yEyUHJe}I really hope your men can get my deed back. [if:convo_excited]On my father's name, I will never gamble again.", null);
				}
			}

			// Token: 0x170001EE RID: 494
			// (get) Token: 0x0600123B RID: 4667 RVA: 0x00074034 File Offset: 0x00072234
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=kiaN39yb}Thank you, {PLAYER.NAME}. I'm sure your companion will be persuasive.[if:convo_relaxed_happy]", null);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001EF RID: 495
			// (get) Token: 0x0600123C RID: 4668 RVA: 0x00074060 File Offset: 0x00072260
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170001F0 RID: 496
			// (get) Token: 0x0600123D RID: 4669 RVA: 0x00074063 File Offset: 0x00072263
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170001F1 RID: 497
			// (get) Token: 0x0600123E RID: 4670 RVA: 0x00074068 File Offset: 0x00072268
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=MIxzaqzi}{QUEST_GIVER.LINK} told you that he lost a land deed in a wager in {TARGET_CITY}. He needs to buy it back, and he wants your companions to intimidate the seller into offering a reasonable price. You asked {COMPANION.LINK} to take {TROOP_COUNT} of your men to go and take care of it. They should report back to you in {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_CITY", this._targetSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					textObject.SetTextVariable("TROOP_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					return textObject;
				}
			}

			// Token: 0x0600123F RID: 4671 RVA: 0x000740F4 File Offset: 0x000722F4
			public RuralNotableInnAndOutIssue(Hero issueOwner)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this.InitializeQuestVariables();
			}

			// Token: 0x06001240 RID: 4672 RVA: 0x0007410D File Offset: 0x0007230D
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementProsperity)
				{
					return -0.1f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x06001241 RID: 4673 RVA: 0x00074130 File Offset: 0x00072330
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Charm) >= hero.GetSkillValue(DefaultSkills.Tactics)) ? DefaultSkills.Charm : DefaultSkills.Tactics, 120);
			}

			// Token: 0x06001242 RID: 4674 RVA: 0x0007415D File Offset: 0x0007235D
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 0, false) && QuestHelper.CheckGoldForAlternativeSolution(1000, out explanation);
			}

			// Token: 0x170001F2 RID: 498
			// (get) Token: 0x06001243 RID: 4675 RVA: 0x00074186 File Offset: 0x00072386
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(500f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06001244 RID: 4676 RVA: 0x0007419C File Offset: 0x0007239C
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.RelationshipChangeWithIssueOwner = 5;
				GainRenownAction.Apply(Hero.MainHero, 5f, false);
				base.IssueOwner.CurrentSettlement.Village.Bound.Town.Loyalty += 5f;
			}

			// Token: 0x06001245 RID: 4677 RVA: 0x000741EB File Offset: 0x000723EB
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner -= 5;
				base.IssueOwner.CurrentSettlement.Village.Bound.Town.Loyalty -= 5f;
			}

			// Token: 0x06001246 RID: 4678 RVA: 0x00074226 File Offset: 0x00072426
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 0, false);
			}

			// Token: 0x06001247 RID: 4679 RVA: 0x00074237 File Offset: 0x00072437
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Common;
			}

			// Token: 0x06001248 RID: 4680 RVA: 0x0007423C File Offset: 0x0007243C
			public override bool IssueStayAliveConditions()
			{
				BoardGameCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<BoardGameCampaignBehavior>();
				return campaignBehavior != null && !campaignBehavior.WonBoardGamesInOneWeekInSettlement.Contains(this._targetSettlement) && !base.IssueOwner.CurrentSettlement.IsRaided && !base.IssueOwner.CurrentSettlement.IsUnderRaid;
			}

			// Token: 0x06001249 RID: 4681 RVA: 0x00074291 File Offset: 0x00072491
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x0600124A RID: 4682 RVA: 0x00074293 File Offset: 0x00072493
			private void InitializeQuestVariables()
			{
				this._targetSettlement = base.IssueOwner.CurrentSettlement.Village.Bound;
				this._boardGameType = this._targetSettlement.Culture.BoardGame;
			}

			// Token: 0x0600124B RID: 4683 RVA: 0x000742C6 File Offset: 0x000724C6
			protected override void OnGameLoad()
			{
				this.InitializeQuestVariables();
			}

			// Token: 0x0600124C RID: 4684 RVA: 0x000742CE File Offset: 0x000724CE
			protected override void HourlyTick()
			{
			}

			// Token: 0x0600124D RID: 4685 RVA: 0x000742D0 File Offset: 0x000724D0
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(14f), this.RewardGold);
			}

			// Token: 0x0600124E RID: 4686 RVA: 0x000742F0 File Offset: 0x000724F0
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				skill = null;
				relationHero = null;
				requiredGold = 0;
				flag = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (FactionManager.IsAtWarAgainstFaction(issueGiver.CurrentSettlement.MapFaction, Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				if (Hero.MainHero.Gold < 2000)
				{
					requiredGold = 2000;
					flag |= IssueBase.PreconditionFlags.Money;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x0600124F RID: 4687 RVA: 0x00074368 File Offset: 0x00072568
			internal static void AutoGeneratedStaticCollectObjectsRuralNotableInnAndOutIssue(object o, List<object> collectedObjects)
			{
				((RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06001250 RID: 4688 RVA: 0x00074376 File Offset: 0x00072576
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x040008B5 RID: 2229
			private const int CompanionSkillLimit = 120;

			// Token: 0x040008B6 RID: 2230
			private const int QuestMoneyLimit = 2000;

			// Token: 0x040008B7 RID: 2231
			private const int AlternativeSolutionGoldCost = 1000;

			// Token: 0x040008B8 RID: 2232
			private CultureObject.BoardGameType _boardGameType;

			// Token: 0x040008B9 RID: 2233
			private Settlement _targetSettlement;
		}

		// Token: 0x020001DD RID: 477
		public class RuralNotableInnAndOutIssueQuest : QuestBase
		{
			// Token: 0x170001F3 RID: 499
			// (get) Token: 0x06001251 RID: 4689 RVA: 0x00074380 File Offset: 0x00072580
			private TextObject QuestStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=tirG1BB2}{QUEST_GIVER.LINK} told you that he lost a land deed while playing games in a tavern in {TARGET_SETTLEMENT}. He wants you to go find the game host and win it back for him. You told him that you will take care of the situation yourself.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001F4 RID: 500
			// (get) Token: 0x06001252 RID: 4690 RVA: 0x000743CC File Offset: 0x000725CC
			private TextObject SuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=bvhWLb4C}You defeated the Game Host and got the deed back. {QUEST_GIVER.LINK}.{newline}\"Thank you for resolving this issue so neatly. Please accept these {GOLD}{GOLD_ICON} denars with our gratitude.\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x170001F5 RID: 501
			// (get) Token: 0x06001253 RID: 4691 RVA: 0x00074424 File Offset: 0x00072624
			private TextObject SuccessWithPayingLog
			{
				get
				{
					TextObject textObject = new TextObject("{=TIPxWsYW}You have bought the deed from the game host. {QUEST_GIVER.LINK}.{newline}\"I am happy that I got my land back. I'm not so happy that everyone knows I had to pay for it, but... Anyway, please accept these {GOLD}{GOLD_ICON} denars with my gratitude.\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("GOLD", 800);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x170001F6 RID: 502
			// (get) Token: 0x06001254 RID: 4692 RVA: 0x00074478 File Offset: 0x00072678
			private TextObject LostLog
			{
				get
				{
					TextObject textObject = new TextObject("{=ye4oqBFB}You lost the board game and failed to help {QUEST_GIVER.LINK}. \"Thank you for trying, {PLAYER.NAME}, but I guess I chose the wrong person for the job.\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x06001255 RID: 4693 RVA: 0x000744BC File Offset: 0x000726BC
			private TextObject QuestCanceledTargetVillageRaided
			{
				get
				{
					TextObject textObject = new TextObject("{=DLesz9jI}{QUEST_GIVER.LINK}’s village is raided. {?QUEST_GIVER.GENDER}She{?}He{\\?} flees to the countryside, and your agreement with {?QUEST_GIVER.GENDER}her{?}him{\\?} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001F8 RID: 504
			// (get) Token: 0x06001256 RID: 4694 RVA: 0x00074505 File Offset: 0x00072705
			private TextObject QuestCanceledWarDeclared
			{
				get
				{
					TextObject textObject = new TextObject("{=cKz1cyuM}Your clan is now at war with {QUEST_GIVER_SETTLEMENT_FACTION}. Quest is canceled.", null);
					textObject.SetTextVariable("QUEST_GIVER_SETTLEMENT_FACTION", base.QuestGiver.CurrentSettlement.MapFaction.Name);
					return textObject;
				}
			}

			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x06001257 RID: 4695 RVA: 0x00074534 File Offset: 0x00072734
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001FA RID: 506
			// (get) Token: 0x06001258 RID: 4696 RVA: 0x00074568 File Offset: 0x00072768
			private TextObject QuestCanceledSettlementIsUnderSiege
			{
				get
				{
					TextObject textObject = new TextObject("{=b5LdBYpF}{SETTLEMENT} is under siege. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170001FB RID: 507
			// (get) Token: 0x06001259 RID: 4697 RVA: 0x000745B4 File Offset: 0x000727B4
			private TextObject TimeoutLog
			{
				get
				{
					TextObject textObject = new TextObject("{=XLy8anVr}You received a message from {QUEST_GIVER.LINK}. \"This may not have seemed like an important task, but I placed my trust in you. I guess I was wrong to do so.\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170001FC RID: 508
			// (get) Token: 0x0600125A RID: 4698 RVA: 0x000745E6 File Offset: 0x000727E6
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=uUhtKnfA}Inn and Out", null);
				}
			}

			// Token: 0x170001FD RID: 509
			// (get) Token: 0x0600125B RID: 4699 RVA: 0x000745F3 File Offset: 0x000727F3
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600125C RID: 4700 RVA: 0x000745F6 File Offset: 0x000727F6
			public RuralNotableInnAndOutIssueQuest(string questId, Hero giverHero, CampaignTime duration, int rewardGold)
				: base(questId, giverHero, duration, rewardGold)
			{
				this.InitializeQuestVariables();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x0600125D RID: 4701 RVA: 0x00074615 File Offset: 0x00072815
			private void InitializeQuestVariables()
			{
				this._targetSettlement = base.QuestGiver.CurrentSettlement.Village.Bound;
				this._boardGameType = this._targetSettlement.Culture.BoardGame;
			}

			// Token: 0x0600125E RID: 4702 RVA: 0x00074648 File Offset: 0x00072848
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this.QuestStartLog, false);
				base.AddTrackedObject(this._targetSettlement);
			}

			// Token: 0x0600125F RID: 4703 RVA: 0x0007466A File Offset: 0x0007286A
			protected override void InitializeQuestOnGameLoad()
			{
				this.InitializeQuestVariables();
				this.SetDialogs();
				if (Campaign.Current.GetCampaignBehavior<BoardGameCampaignBehavior>() == null)
				{
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06001260 RID: 4704 RVA: 0x0007468B File Offset: 0x0007288B
			protected override void HourlyTick()
			{
			}

			// Token: 0x06001261 RID: 4705 RVA: 0x00074690 File Offset: 0x00072890
			protected override void RegisterEvents()
			{
				CampaignEvents.OnPlayerBoardGameOverEvent.AddNonSerializedListener(this, new Action<Hero, BoardGameHelper.BoardGameState>(this.OnBoardGameEnd));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeStarted));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.VillageBeingRaided.AddNonSerializedListener(this, new Action<Village>(this.OnVillageBeingRaided));
				CampaignEvents.LocationCharactersSimulatedEvent.AddNonSerializedListener(this, new Action(this.OnLocationCharactersSimulated));
			}

			// Token: 0x06001262 RID: 4706 RVA: 0x00074740 File Offset: 0x00072940
			private void OnLocationCharactersSimulated()
			{
				if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == this._targetSettlement && Campaign.Current.GameMenuManager.MenuLocations.Count > 0 && Campaign.Current.GameMenuManager.MenuLocations[0].StringId == "tavern")
				{
					foreach (Agent agent in Mission.Current.Agents)
					{
						LocationCharacter locationCharacter = LocationComplex.Current.GetLocationWithId("tavern").GetLocationCharacter(agent.Origin);
						if (locationCharacter != null && locationCharacter.Character.Occupation == Occupation.TavernGameHost)
						{
							locationCharacter.IsVisualTracked = true;
						}
					}
				}
			}

			// Token: 0x06001263 RID: 4707 RVA: 0x00074820 File Offset: 0x00072A20
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06001264 RID: 4708 RVA: 0x00074833 File Offset: 0x00072A33
			private void OnVillageBeingRaided(Village village)
			{
				if (village == base.QuestGiver.CurrentSettlement.Village)
				{
					base.CompleteQuestWithCancel(this.QuestCanceledTargetVillageRaided);
				}
			}

			// Token: 0x06001265 RID: 4709 RVA: 0x00074854 File Offset: 0x00072A54
			private void OnBoardGameEnd(Hero opposingHero, BoardGameHelper.BoardGameState state)
			{
				if (this._checkForBoardGameEnd)
				{
					this._playerWonTheGame = state == BoardGameHelper.BoardGameState.Win;
				}
			}

			// Token: 0x06001266 RID: 4710 RVA: 0x00074868 File Offset: 0x00072A68
			private void OnSiegeStarted(SiegeEvent siegeEvent)
			{
				if (siegeEvent.BesiegedSettlement == this._targetSettlement)
				{
					base.CompleteQuestWithCancel(this.QuestCanceledSettlementIsUnderSiege);
				}
			}

			// Token: 0x06001267 RID: 4711 RVA: 0x00074884 File Offset: 0x00072A84
			protected override void SetDialogs()
			{
				TextObject textObject = new TextObject("{=I6amLvVE}Good, good. That's the best way to do these things. [if:convo_normal]Go to {TARGET_SETTLEMENT}, find this game host and wipe the smirk off of his face.", null);
				textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.Name);
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(textObject, null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=HGRWs0zE}Have you met the man who took my deed? Did you get it back?[if:convo_astonished]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=uJPAYUU7}I will be on my way soon enough.", null), null, null, null)
					.NpcLine(new TextObject("{=MOmePlJQ}Could you hurry this along? I don't want him to find another buyer.[if:convo_pondering] Thank you.", null), null, null, null, null)
					.CloseDialog()
					.PlayerOption(new TextObject("{=azVhRGik}I am waiting for the right moment.", null), null, null, null)
					.NpcLine(new TextObject("{=bRMLn0jj}Well, if he wanders off to another town, or gets his throat slit,[if:convo_pondering] or loses the deed, that would be the wrong moment, now wouldn't it?", null), null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetGameHostDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetGameHostDialogueAfterFirstGame(), this);
			}

			// Token: 0x06001268 RID: 4712 RVA: 0x000749C0 File Offset: 0x00072BC0
			private DialogFlow GetGameHostDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=dzWioKRa}Hello there, are you looking for a friendly match? A wager perhaps?[if:convo_mocking_aristocratic]", null, null, null, null).Condition(() => this.TavernHostDialogCondition(true))
					.PlayerLine(new TextObject("{=eOle8pYT}You won a deed of land from my associate. I'm here to win it back.", null), null, null, null)
					.NpcLine("{=bEipgE5E}Ah, yes, these are the most interesting kinds of games, aren't they? [if:convo_excited]I won't deny myself the pleasure but clearly that deed is worth more to him than just the value of the land. I'll wager the deed, but you need to put up 1000 denars.", null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=XvkSbY6N}I see your wager. Let's play.", null, null, null)
					.Condition(() => Hero.MainHero.Gold >= 1000)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.StartBoardGame))
					.CloseDialog()
					.PlayerOption("{=89b5ao7P}As of now, I do not have 1000 denars to afford on gambling. I may get back to you once I get the required amount.", null, null, null)
					.Condition(() => Hero.MainHero.Gold < 1000)
					.NpcLine(new TextObject("{=ppi6eVos}As you wish.", null), null, null, null, null)
					.CloseDialog()
					.PlayerOption("{=WrnvRayQ}Let's just save ourselves some trouble, and I'll just pay you that amount.", null, null, null)
					.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.CheckPlayerHasEnoughDenarsClickableCondition))
					.NpcLine("{=pa3RY39w}Sure. I'm happy to turn paper into silver... 1000 denars it is.[if:convo_evil_smile]", null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.PlayerPaid1000QuestSuccess))
					.CloseDialog()
					.PlayerOption("{=BSeplVwe}That's too much. I will be back later.", null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x06001269 RID: 4713 RVA: 0x00074B10 File Offset: 0x00072D10
			private DialogFlow GetGameHostDialogueAfterFirstGame()
			{
				return DialogFlow.CreateDialogFlow("start", 125).BeginNpcOptions(null, false).NpcOption(new TextObject("{=dyhZUHao}Well, I thought you were here to be sheared, [if:convo_shocked]but it looks like the sheep bites back. Very well, nicely played, here's your man's land back.", null), () => this._playerWonTheGame && this.TavernHostDialogCondition(false), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.PlayerWonTheBoardGame))
					.CloseDialog()
					.NpcOption("{=TdnD29Ax}Ah! You almost had me! Maybe you just weren't paying attention. [if:convo_mocking_teasing]Care to put another 1000 denars on the table and have another go?", () => !this._playerWonTheGame && this._tryCount < 2 && this.TavernHostDialogCondition(false), null, null, null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption("{=fiMZ696A}Yes, I'll play again.", null, null, null)
					.ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.CheckPlayerHasEnoughDenarsClickableCondition))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.StartBoardGame))
					.CloseDialog()
					.PlayerOption("{=zlFSIvD5}No, no. I know a trap when I see one. You win. Good-bye.", null, null, null)
					.NpcLine(new TextObject("{=ppi6eVos}As you wish.", null), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.PlayerFailAfterBoardGame))
					.CloseDialog()
					.EndPlayerOptions()
					.NpcOption("{=hkNrC5d3}That was fun, but I've learned not to inflict too great a humiliation on those who carry a sword.[if:convo_merry] I'll take my winnings and enjoy them now. Good-bye to you!", () => this._tryCount >= 2 && this.TavernHostDialogCondition(false), null, null, null, null)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.PlayerFailAfterBoardGame))
					.CloseDialog()
					.EndNpcOptions();
			}

			// Token: 0x0600126A RID: 4714 RVA: 0x00074C2C File Offset: 0x00072E2C
			private bool CheckPlayerHasEnoughDenarsClickableCondition(out TextObject explanation)
			{
				if (Hero.MainHero.Gold >= 1000)
				{
					explanation = null;
					return true;
				}
				explanation = new TextObject("{=AMlaYbJv}You don't have 1000 denars.", null);
				return false;
			}

			// Token: 0x0600126B RID: 4715 RVA: 0x00074C54 File Offset: 0x00072E54
			private bool TavernHostDialogCondition(bool isInitialDialogue = false)
			{
				if ((!this._checkForBoardGameEnd || !isInitialDialogue) && Settlement.CurrentSettlement == this._targetSettlement && CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernGameHost)
				{
					LocationComplex locationComplex = LocationComplex.Current;
					if (((locationComplex != null) ? locationComplex.GetLocationWithId("tavern") : null) != null)
					{
						Mission.Current.GetMissionBehavior<MissionBoardGameLogic>().DetectOpposingAgent();
						return Mission.Current.GetMissionBehavior<MissionBoardGameLogic>().CheckIfBothSidesAreSitting();
					}
				}
				return false;
			}

			// Token: 0x0600126C RID: 4716 RVA: 0x00074CBF File Offset: 0x00072EBF
			private void PlayerPaid1000QuestSuccess()
			{
				base.AddLog(this.SuccessWithPayingLog, false);
				this._applyLesserReward = true;
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, 1000, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x0600126D RID: 4717 RVA: 0x00074CF0 File Offset: 0x00072EF0
			protected override void OnFinalize()
			{
				if (Mission.Current != null)
				{
					foreach (Agent agent in Mission.Current.Agents)
					{
						Location locationWithId = LocationComplex.Current.GetLocationWithId("tavern");
						if (locationWithId != null)
						{
							LocationCharacter locationCharacter = locationWithId.GetLocationCharacter(agent.Origin);
							if (locationCharacter != null && locationCharacter.Character.Occupation == Occupation.TavernGameHost)
							{
								locationCharacter.IsVisualTracked = false;
							}
						}
					}
				}
			}

			// Token: 0x0600126E RID: 4718 RVA: 0x00074D80 File Offset: 0x00072F80
			private void ApplySuccessRewards()
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._applyLesserReward ? 800 : this.RewardGold, false);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, 5, true, true);
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				base.QuestGiver.CurrentSettlement.Village.Bound.Town.Loyalty += 5f;
			}

			// Token: 0x0600126F RID: 4719 RVA: 0x00074DF7 File Offset: 0x00072FF7
			protected override void OnCompleteWithSuccess()
			{
				this.ApplySuccessRewards();
			}

			// Token: 0x06001270 RID: 4720 RVA: 0x00074E00 File Offset: 0x00073000
			private void StartBoardGame()
			{
				MissionBoardGameLogic missionBehavior = Mission.Current.GetMissionBehavior<MissionBoardGameLogic>();
				Campaign.Current.GetCampaignBehavior<BoardGameCampaignBehavior>().SetBetAmount(1000);
				missionBehavior.DetectOpposingAgent();
				missionBehavior.SetCurrentDifficulty(BoardGameHelper.AIDifficulty.Normal);
				missionBehavior.SetBoardGame(this._boardGameType);
				missionBehavior.StartBoardGame();
				this._checkForBoardGameEnd = true;
				this._tryCount++;
			}

			// Token: 0x06001271 RID: 4721 RVA: 0x00074E5E File Offset: 0x0007305E
			private void PlayerWonTheBoardGame()
			{
				base.AddLog(this.SuccessLog, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06001272 RID: 4722 RVA: 0x00074E74 File Offset: 0x00073074
			private void PlayerFailAfterBoardGame()
			{
				base.AddLog(this.LostLog, false);
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.CurrentSettlement.Village.Bound.Town.Loyalty -= 5f;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06001273 RID: 4723 RVA: 0x00074EC9 File Offset: 0x000730C9
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclared);
				}
			}

			// Token: 0x06001274 RID: 4724 RVA: 0x00074EF8 File Offset: 0x000730F8
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestCanceledWarDeclared, false);
			}

			// Token: 0x06001275 RID: 4725 RVA: 0x00074F10 File Offset: 0x00073110
			public override GameMenuOption.IssueQuestFlags IsLocationTrackedByQuest(Location location)
			{
				if (PlayerEncounter.LocationEncounter.Settlement == this._targetSettlement && location.StringId == "tavern")
				{
					return GameMenuOption.IssueQuestFlags.ActiveIssue;
				}
				return GameMenuOption.IssueQuestFlags.None;
			}

			// Token: 0x06001276 RID: 4726 RVA: 0x00074F3C File Offset: 0x0007313C
			protected override void OnTimedOut()
			{
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.CurrentSettlement.Village.Bound.Town.Loyalty -= 5f;
				base.AddLog(this.TimeoutLog, false);
			}

			// Token: 0x06001277 RID: 4727 RVA: 0x00074F8A File Offset: 0x0007318A
			internal static void AutoGeneratedStaticCollectObjectsRuralNotableInnAndOutIssueQuest(object o, List<object> collectedObjects)
			{
				((RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06001278 RID: 4728 RVA: 0x00074F98 File Offset: 0x00073198
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06001279 RID: 4729 RVA: 0x00074FA1 File Offset: 0x000731A1
			internal static object AutoGeneratedGetMemberValue_tryCount(object o)
			{
				return ((RuralNotableInnAndOutIssueBehavior.RuralNotableInnAndOutIssueQuest)o)._tryCount;
			}

			// Token: 0x040008BA RID: 2234
			public const int LesserReward = 800;

			// Token: 0x040008BB RID: 2235
			private CultureObject.BoardGameType _boardGameType;

			// Token: 0x040008BC RID: 2236
			private Settlement _targetSettlement;

			// Token: 0x040008BD RID: 2237
			private bool _checkForBoardGameEnd;

			// Token: 0x040008BE RID: 2238
			private bool _playerWonTheGame;

			// Token: 0x040008BF RID: 2239
			private bool _applyLesserReward;

			// Token: 0x040008C0 RID: 2240
			[SaveableField(1)]
			private int _tryCount;
		}
	}
}
