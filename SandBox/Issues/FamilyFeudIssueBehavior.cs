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
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace SandBox.Issues
{
	// Token: 0x020000B7 RID: 183
	public class FamilyFeudIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600079B RID: 1947 RVA: 0x00033ADF File Offset: 0x00031CDF
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00033AF8 File Offset: 0x00031CF8
		public void OnCheckForIssue(Hero hero)
		{
			Settlement settlement;
			Hero hero2;
			if (this.ConditionsHold(hero, out settlement, out hero2))
			{
				KeyValuePair<Hero, Settlement> keyValuePair = new KeyValuePair<Hero, Settlement>(hero2, settlement);
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(FamilyFeudIssueBehavior.FamilyFeudIssue), IssueBase.IssueFrequency.Rare, keyValuePair));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(FamilyFeudIssueBehavior.FamilyFeudIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00033B70 File Offset: 0x00031D70
		private bool ConditionsHold(Hero issueGiver, out Settlement otherVillage, out Hero otherNotable)
		{
			otherVillage = null;
			otherNotable = null;
			if (!issueGiver.IsNotable)
			{
				return false;
			}
			if (issueGiver.IsRuralNotable && issueGiver.CurrentSettlement.IsVillage)
			{
				Settlement bound = issueGiver.CurrentSettlement.Village.Bound;
				if (bound.IsTown)
				{
					foreach (Village village in bound.BoundVillages.WhereQ<Village>((Village x) => x != issueGiver.CurrentSettlement.Village))
					{
						Hero hero = village.Settlement.Notables.FirstOrDefaultQ<Hero>((Hero y) => y.IsRuralNotable && y.CanHaveCampaignIssues() && y.GetTraitLevel(DefaultTraits.Mercy) <= 0);
						if (hero != null)
						{
							otherVillage = village.Settlement;
							otherNotable = hero;
						}
					}
					return otherVillage != null;
				}
			}
			return false;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00033C78 File Offset: 0x00031E78
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			KeyValuePair<Hero, Settlement> keyValuePair = (KeyValuePair<Hero, Settlement>)potentialIssueData.RelatedObject;
			return new FamilyFeudIssueBehavior.FamilyFeudIssue(issueOwner, keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00033CAD File Offset: 0x00031EAD
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04000412 RID: 1042
		private const IssueBase.IssueFrequency FamilyFeudIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x020001C7 RID: 455
		public class FamilyFeudIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06000FA7 RID: 4007 RVA: 0x00068C82 File Offset: 0x00066E82
			public FamilyFeudIssueTypeDefiner()
				: base(1087000)
			{
			}

			// Token: 0x06000FA8 RID: 4008 RVA: 0x00068C8F File Offset: 0x00066E8F
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(FamilyFeudIssueBehavior.FamilyFeudIssue), 1, null);
				base.AddClassDefinition(typeof(FamilyFeudIssueBehavior.FamilyFeudIssueQuest), 2, null);
			}
		}

		// Token: 0x020001C8 RID: 456
		public class FamilyFeudIssueMissionBehavior : MissionLogic
		{
			// Token: 0x06000FA9 RID: 4009 RVA: 0x00068CB5 File Offset: 0x00066EB5
			public FamilyFeudIssueMissionBehavior(Action<Agent, Agent, int> agentHitAction)
			{
				this.OnAgentHitAction = agentHitAction;
			}

			// Token: 0x06000FAA RID: 4010 RVA: 0x00068CC4 File Offset: 0x00066EC4
			public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
			{
				Action<Agent, Agent, int> onAgentHitAction = this.OnAgentHitAction;
				if (onAgentHitAction == null)
				{
					return;
				}
				onAgentHitAction(affectedAgent, affectorAgent, blow.InflictedDamage);
			}

			// Token: 0x04000821 RID: 2081
			private Action<Agent, Agent, int> OnAgentHitAction;
		}

		// Token: 0x020001C9 RID: 457
		public class FamilyFeudIssue : IssueBase
		{
			// Token: 0x17000149 RID: 329
			// (get) Token: 0x06000FAB RID: 4011 RVA: 0x00068CDF File Offset: 0x00066EDF
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x1700014A RID: 330
			// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00068CE2 File Offset: 0x00066EE2
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 3 + MathF.Ceiling(5f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700014B RID: 331
			// (get) Token: 0x06000FAD RID: 4013 RVA: 0x00068CF7 File Offset: 0x00066EF7
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 3 + MathF.Ceiling(7f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700014C RID: 332
			// (get) Token: 0x06000FAE RID: 4014 RVA: 0x00068D0C File Offset: 0x00066F0C
			protected override int RewardGold
			{
				get
				{
					return (int)(350f + 1500f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x1700014D RID: 333
			// (get) Token: 0x06000FAF RID: 4015 RVA: 0x00068D21 File Offset: 0x00066F21
			// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x00068D29 File Offset: 0x00066F29
			[SaveableProperty(30)]
			public override Hero CounterOfferHero { get; protected set; }

			// Token: 0x1700014E RID: 334
			// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00068D32 File Offset: 0x00066F32
			public override int NeededInfluenceForLordSolution
			{
				get
				{
					return 20;
				}
			}

			// Token: 0x1700014F RID: 335
			// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x00068D36 File Offset: 0x00066F36
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(500f + 700f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17000150 RID: 336
			// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x00068D4C File Offset: 0x00066F4C
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=zRJ1bQFO}{ISSUE_GIVER.LINK}, a landowner from {ISSUE_GIVER_SETTLEMENT}, told you about an incident that is about to turn into an ugly feud. One of the youngsters killed another in an accident and the victim's family refused blood money as compensation and wants blood. You decided to leave {COMPANION.LINK} with some men for {RETURN_DAYS} days to let things cool down. They should return with the reward of {REWARD_GOLD}{GOLD_ICON} denars as promised by {ISSUE_GIVER.LINK} after {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("ISSUE_GIVER_SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					textObject.SetTextVariable("REWARD_GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x17000151 RID: 337
			// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x00068DE7 File Offset: 0x00066FE7
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000152 RID: 338
			// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00068DEA File Offset: 0x00066FEA
			public override bool IsThereLordSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000153 RID: 339
			// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x00068DED File Offset: 0x00066FED
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=7qPda0SA}Yes... We do have a problem. One of my relatives fell victim to his temper during a quarrel and killed a man from {TARGET_VILLAGE}.[ib:normal2][if:convo_dismayed] We offered to pay blood money but the family of the deceased have stubbornly refused it. As it turns out, the deceased is kin to {TARGET_NOTABLE}, an elder of this region and now the men of {TARGET_VILLAGE} have sworn to kill my relative.", null);
					textObject.SetTextVariable("TARGET_VILLAGE", this._targetVillage.Name);
					textObject.SetTextVariable("TARGET_NOTABLE", this._targetNotable.Name);
					return textObject;
				}
			}

			// Token: 0x17000154 RID: 340
			// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x00068E28 File Offset: 0x00067028
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=XX3sWsVX}This sounds pretty serious. Go on.", null);
				}
			}

			// Token: 0x17000155 RID: 341
			// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x00068E38 File Offset: 0x00067038
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=mgUoXwZt}My family is concerned for the boy's life. He has gone hiding around the village commons. We need someone who can protect him until [ib:normal][if:convo_normal]{TARGET_NOTABLE.LINK} sees reason, accepts the blood money and ends the feud. We would be eternally grateful, if you can help my relative and take him with you for a while maybe.", null);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_VILLAGE", this._targetVillage.Name);
					return textObject;
				}
			}

			// Token: 0x17000156 RID: 342
			// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00068E81 File Offset: 0x00067081
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=cDYz49kZ}You can keep my relative under your protection for a time until the calls for vengeance die down.[ib:closed][if:convo_pondering] Maybe you can leave one of your warrior companions and {ALTERNATIVE_TROOP_COUNT} men with him to protect him.", null);
					textObject.SetTextVariable("ALTERNATIVE_TROOP_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					return textObject;
				}
			}

			// Token: 0x17000157 RID: 343
			// (get) Token: 0x06000FBA RID: 4026 RVA: 0x00068EA0 File Offset: 0x000670A0
			protected override TextObject LordSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=oJt4bemH}{QUEST_GIVER.LINK}, a landowner from {QUEST_SETTLEMENT}, told you about an incident that is about to turn into an ugly feud. One young man killed another in an quarrel and the victim's family refused blood money compensation, demanding vengeance instead.", null);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17000158 RID: 344
			// (get) Token: 0x06000FBB RID: 4027 RVA: 0x00068F08 File Offset: 0x00067108
			protected override TextObject LordSolutionCounterOfferRefuseLog
			{
				get
				{
					TextObject textObject = new TextObject("{=JqN5BSjN}As the dispenser of justice in the district, you decided to allow {TARGET_NOTABLE.LINK} to take vengeance for {?TARGET_NOTABLE.GENDER}her{?}his{\\?} kinsman. You failed to protect the culprit as you promised. {QUEST_GIVER.LINK} is furious.", null);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000159 RID: 345
			// (get) Token: 0x06000FBC RID: 4028 RVA: 0x00068F54 File Offset: 0x00067154
			protected override TextObject LordSolutionCounterOfferAcceptLog
			{
				get
				{
					TextObject textObject = new TextObject("{=UxrXNSW7}As the ruler, you have let {TARGET_NOTABLE.LINK} to take {?TARGET_NOTABLE.GENDER}her{?}him{\\?} kinsman's vengeance and failed to protect the boy as you have promised to {QUEST_GIVER.LINK}. {QUEST_GIVER.LINK} is furious.", null);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700015A RID: 346
			// (get) Token: 0x06000FBD RID: 4029 RVA: 0x00068FB0 File Offset: 0x000671B0
			public override TextObject IssueLordSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=tsjwrZCZ}I am sure that, as {?PLAYER.GENDER}lady{?}lord{\\?} of this district, you will not let these unlawful threats go unpunished. As the lord of the region, you can talk to {TARGET_NOTABLE.LINK} and force him to accept the blood money.", null);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700015B RID: 347
			// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00068FE2 File Offset: 0x000671E2
			public override TextObject IssuePlayerResponseAfterLordExplanation
			{
				get
				{
					return new TextObject("{=A3GfCPUb}I'm not sure about using my authority in this way. Is there any other way to solve this?", null);
				}
			}

			// Token: 0x1700015C RID: 348
			// (get) Token: 0x06000FBF RID: 4031 RVA: 0x00068FEF File Offset: 0x000671EF
			public override TextObject IssuePlayerResponseAfterAlternativeExplanation
			{
				get
				{
					return new TextObject("{=8EaCJ2uw}What else can I do?", null);
				}
			}

			// Token: 0x1700015D RID: 349
			// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00068FFC File Offset: 0x000671FC
			public override TextObject IssueLordSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=Du31GKSb}As the magistrate of this district, I hereby order that blood money shall be accepted. This is a crime of passion, not malice. Tell {TARGET_NOTABLE.LINK} to take the silver or face my wrath!", null);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700015E RID: 350
			// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0006902E File Offset: 0x0006722E
			public override TextObject IssueLordSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=xNyLPMnx}Thank you, my {?PLAYER.GENDER}lady{?}lord{\\?}, thank you.", null);
				}
			}

			// Token: 0x1700015F RID: 351
			// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0006903C File Offset: 0x0006723C
			public override TextObject IssueLordSolutionCounterOfferExplanationByOtherNpc
			{
				get
				{
					TextObject textObject = new TextObject("{=vjk2q3OT}{?PLAYER.GENDER}Madam{?}Sir{\\?}, {TARGET_NOTABLE.LINK}'s nephew murdered one of my kinsman, [ib:aggressive][if:convo_bared_teeth]and it is our right to take vengeance on the murderer. Custom gives us the right of vengeance. Everyone must know that we are willing to avenge our sons, or others will think little of killing them. Does it do us good to be a clan of old men and women, drowning in silver, if all our sons are slain? Please sir, allow us to take vengeance. We promise we won't let this turn into a senseless blood feud.", null);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000160 RID: 352
			// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x00069080 File Offset: 0x00067280
			public override TextObject IssueLordSolutionCounterOfferBriefByOtherNpc
			{
				get
				{
					return new TextObject("{=JhbbB2dp}My {?PLAYER.GENDER}lady{?}lord{\\?}, may I have a word please?", null);
				}
			}

			// Token: 0x17000161 RID: 353
			// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0006908D File Offset: 0x0006728D
			public override TextObject IssueLordSolutionCounterOfferAcceptByPlayer
			{
				get
				{
					return new TextObject("{=TIVHLAjy}You may have a point. I hereby revoke my previous decision.", null);
				}
			}

			// Token: 0x17000162 RID: 354
			// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0006909A File Offset: 0x0006729A
			public override TextObject IssueLordSolutionCounterOfferAcceptResponseByOtherNpc
			{
				get
				{
					return new TextObject("{=A9uSikTY}Thank you my {?PLAYER.GENDER}lady{?}lord{\\?}.", null);
				}
			}

			// Token: 0x17000163 RID: 355
			// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x000690A7 File Offset: 0x000672A7
			public override TextObject IssueLordSolutionCounterOfferDeclineByPlayer
			{
				get
				{
					return new TextObject("{=Vs9DfZmJ}No. My word is final. You will have to take the blood money.", null);
				}
			}

			// Token: 0x17000164 RID: 356
			// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x000690B4 File Offset: 0x000672B4
			public override TextObject IssueLordSolutionCounterOfferDeclineResponseByOtherNpc
			{
				get
				{
					return new TextObject("{=3oaVUNdr}I hope you won't be [if:convo_disbelief]regret with your decision, my {?PLAYER.GENDER}lady{?}lord{\\?}.", null);
				}
			}

			// Token: 0x17000165 RID: 357
			// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x000690C1 File Offset: 0x000672C1
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=VcfZdKcp}Don't worry, I will protect your relative.", null);
				}
			}

			// Token: 0x17000166 RID: 358
			// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x000690CE File Offset: 0x000672CE
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=ZpDQxmzJ}Family Feud", null);
				}
			}

			// Token: 0x17000167 RID: 359
			// (get) Token: 0x06000FCA RID: 4042 RVA: 0x000690DC File Offset: 0x000672DC
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=aSZvZRYC}A relative of {QUEST_GIVER.NAME} kills a relative of {TARGET_NOTABLE.NAME}. {QUEST_GIVER.NAME} offers to pay blood money for the crime but {TARGET_NOTABLE.NAME} wants revenge.", null);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000168 RID: 360
			// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00069126 File Offset: 0x00067326
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=9ZngZ6W7}I will have one of my companions and {REQUIRED_TROOP_AMOUNT} of my men protect your kinsman for {RETURN_DAYS} days. ", null);
					textObject.SetTextVariable("REQUIRED_TROOP_AMOUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17000169 RID: 361
			// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00069157 File Offset: 0x00067357
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					TextObject textObject = new TextObject("{=n9QRnxbC}I have no doubt that {TARGET_NOTABLE.LINK} will have to accept[ib:closed][if:convo_grateful] the offer after seeing the boy with that many armed men behind him. Thank you, {?PLAYER.GENDER}madam{?}sir{\\?}, for helping to ending this without more blood.", null);
					textObject.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, false);
					return textObject;
				}
			}

			// Token: 0x1700016A RID: 362
			// (get) Token: 0x06000FCD RID: 4045 RVA: 0x0006917C File Offset: 0x0006737C
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=MaGPKGHA}Thank you my {?PLAYER.GENDER}lady{?}lord{\\?}. [if:convo_pondering]I am sure your men will protect the boy and {TARGET_NOTABLE.LINK} will have nothing to do but to accept the blood money. I have to add, I'm ready to pay you {REWARD_GOLD}{GOLD_ICON} denars for your trouble.", null);
					StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD_GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x1700016B RID: 363
			// (get) Token: 0x06000FCE RID: 4046 RVA: 0x000691E4 File Offset: 0x000673E4
			public override TextObject IssueAsRumorInSettlement
			{
				get
				{
					TextObject textObject = new TextObject("{=lmVCRD4Q}I hope {QUEST_GIVER.LINK} [if:convo_disbelief]can work out that trouble with {?QUEST_GIVER.GENDER}her{?}his{\\?} kinsman.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700016C RID: 364
			// (get) Token: 0x06000FCF RID: 4047 RVA: 0x00069218 File Offset: 0x00067418
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=vS6oZJPA}Your companion {COMPANION.LINK} and your men returns with the news of their success. Apparently {TARGET_NOTABLE.LINK} and {?TARGET_NOTABLE.GENDER}her{?}his{\\?} thugs finds the culprit and tries to murder him but your men manages to drive them away. {COMPANION.LINK} tells you that they bloodied their noses so badly that they wouldn’t dare to try again. {QUEST_GIVER.LINK} is grateful and sends {?QUEST_GIVER.GENDER}her{?}his{\\?} regards with a purse full of {REWARD}{GOLD_ICON} denars.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x06000FD0 RID: 4048 RVA: 0x0006929D File Offset: 0x0006749D
			public FamilyFeudIssue(Hero issueOwner, Hero targetNotable, Settlement targetVillage)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._targetNotable = targetNotable;
				this._targetVillage = targetVillage;
			}

			// Token: 0x06000FD1 RID: 4049 RVA: 0x000692BE File Offset: 0x000674BE
			public override void OnHeroCanBeSelectedInInventoryInfoIsRequested(Hero hero, ref bool result)
			{
				this.CommonResrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x06000FD2 RID: 4050 RVA: 0x000692C8 File Offset: 0x000674C8
			public override void OnHeroCanHavePartyRoleOrBeGovernorInfoIsRequested(Hero hero, ref bool result)
			{
				this.CommonResrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x06000FD3 RID: 4051 RVA: 0x000692D2 File Offset: 0x000674D2
			public override void OnHeroCanLeadPartyInfoIsRequested(Hero hero, ref bool result)
			{
				this.CommonResrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x06000FD4 RID: 4052 RVA: 0x000692DC File Offset: 0x000674DC
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				this.CommonResrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x06000FD5 RID: 4053 RVA: 0x000692E6 File Offset: 0x000674E6
			private void CommonResrictionInfoIsRequested(Hero hero, ref bool result)
			{
				if (this._targetNotable == hero)
				{
					result = false;
				}
			}

			// Token: 0x06000FD6 RID: 4054 RVA: 0x000692F4 File Offset: 0x000674F4
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -1f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x06000FD7 RID: 4055 RVA: 0x00069317 File Offset: 0x00067517
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Athletics) >= hero.GetSkillValue(DefaultSkills.Charm)) ? DefaultSkills.Athletics : DefaultSkills.Charm, 120);
			}

			// Token: 0x06000FD8 RID: 4056 RVA: 0x00069344 File Offset: 0x00067544
			protected override void LordSolutionConsequenceWithAcceptCounterOffer()
			{
				TraitLevelingHelper.OnIssueSolvedThroughBetrayal(base.IssueOwner, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -50)
				});
				this.RelationshipChangeWithIssueOwner = -10;
				ChangeRelationAction.ApplyPlayerRelation(this._targetNotable, 5, true, true);
				base.IssueOwner.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
				base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security -= 5f;
			}

			// Token: 0x06000FD9 RID: 4057 RVA: 0x000693DD File Offset: 0x000675DD
			protected override void LordSolutionConsequenceWithRefuseCounterOffer()
			{
				this.ApplySuccessRewards();
			}

			// Token: 0x06000FDA RID: 4058 RVA: 0x000693E5 File Offset: 0x000675E5
			public override bool LordSolutionCondition(out TextObject explanation)
			{
				if (base.IssueOwner.CurrentSettlement.OwnerClan == Clan.PlayerClan)
				{
					explanation = null;
					return true;
				}
				explanation = new TextObject("{=9y0zpKUF}You need to be the owner of this settlement!", null);
				return false;
			}

			// Token: 0x06000FDB RID: 4059 RVA: 0x00069411 File Offset: 0x00067611
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x06000FDC RID: 4060 RVA: 0x0006942B File Offset: 0x0006762B
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), out explanation, 2, false);
			}

			// Token: 0x06000FDD RID: 4061 RVA: 0x0006943C File Offset: 0x0006763C
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x06000FDE RID: 4062 RVA: 0x0006944C File Offset: 0x0006764C
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
				base.AlternativeSolutionHero.AddSkillXp(skillObject, (float)((int)(500f + 700f * base.IssueDifficultyMultiplier)));
			}

			// Token: 0x06000FDF RID: 4063 RVA: 0x000694B0 File Offset: 0x000676B0
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -10;
				ChangeRelationAction.ApplyPlayerRelation(this._targetNotable, 5, true, true);
				base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security -= 5f;
				base.IssueOwner.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
			}

			// Token: 0x06000FE0 RID: 4064 RVA: 0x0006952C File Offset: 0x0006772C
			private void ApplySuccessRewards()
			{
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				this.RelationshipChangeWithIssueOwner = 10;
				ChangeRelationAction.ApplyPlayerRelation(this._targetNotable, -5, true, true);
				base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security += 10f;
			}

			// Token: 0x06000FE1 RID: 4065 RVA: 0x0006958B File Offset: 0x0006778B
			protected override void AfterIssueCreation()
			{
				this.CounterOfferHero = base.IssueOwner.CurrentSettlement.Notables.FirstOrDefault<Hero>((Hero x) => x.CharacterObject.IsHero && x.CharacterObject.HeroObject != base.IssueOwner);
			}

			// Token: 0x06000FE2 RID: 4066 RVA: 0x000695B4 File Offset: 0x000677B4
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06000FE3 RID: 4067 RVA: 0x000695B6 File Offset: 0x000677B6
			protected override void HourlyTick()
			{
			}

			// Token: 0x06000FE4 RID: 4068 RVA: 0x000695B8 File Offset: 0x000677B8
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new FamilyFeudIssueBehavior.FamilyFeudIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(20f), this._targetVillage, this._targetNotable, this.RewardGold);
			}

			// Token: 0x06000FE5 RID: 4069 RVA: 0x000695E2 File Offset: 0x000677E2
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x06000FE6 RID: 4070 RVA: 0x000695E8 File Offset: 0x000677E8
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
				if (Clan.PlayerClan.Companions.Count >= Clan.PlayerClan.CompanionLimit)
				{
					flag |= IssueBase.PreconditionFlags.CompanionLimitReached;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06000FE7 RID: 4071 RVA: 0x00069668 File Offset: 0x00067868
			public override bool IssueStayAliveConditions()
			{
				return this._targetNotable != null && this._targetNotable.IsActive && (this.CounterOfferHero == null || (this.CounterOfferHero.IsActive && this.CounterOfferHero.CurrentSettlement == base.IssueSettlement));
			}

			// Token: 0x06000FE8 RID: 4072 RVA: 0x000696B8 File Offset: 0x000678B8
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x06000FE9 RID: 4073 RVA: 0x000696BA File Offset: 0x000678BA
			internal static void AutoGeneratedStaticCollectObjectsFamilyFeudIssue(object o, List<object> collectedObjects)
			{
				((FamilyFeudIssueBehavior.FamilyFeudIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06000FEA RID: 4074 RVA: 0x000696C8 File Offset: 0x000678C8
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetVillage);
				collectedObjects.Add(this._targetNotable);
				collectedObjects.Add(this.CounterOfferHero);
			}

			// Token: 0x06000FEB RID: 4075 RVA: 0x000696F5 File Offset: 0x000678F5
			internal static object AutoGeneratedGetMemberValueCounterOfferHero(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssue)o).CounterOfferHero;
			}

			// Token: 0x06000FEC RID: 4076 RVA: 0x00069702 File Offset: 0x00067902
			internal static object AutoGeneratedGetMemberValue_targetVillage(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssue)o)._targetVillage;
			}

			// Token: 0x06000FED RID: 4077 RVA: 0x0006970F File Offset: 0x0006790F
			internal static object AutoGeneratedGetMemberValue_targetNotable(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssue)o)._targetNotable;
			}

			// Token: 0x04000822 RID: 2082
			private const int CompanionRequiredSkillLevel = 120;

			// Token: 0x04000823 RID: 2083
			private const int QuestTimeLimit = 20;

			// Token: 0x04000824 RID: 2084
			private const int IssueDuration = 30;

			// Token: 0x04000825 RID: 2085
			private const int TroopTierForAlternativeSolution = 2;

			// Token: 0x04000826 RID: 2086
			[SaveableField(10)]
			private Settlement _targetVillage;

			// Token: 0x04000827 RID: 2087
			[SaveableField(20)]
			private Hero _targetNotable;
		}

		// Token: 0x020001CA RID: 458
		public class FamilyFeudIssueQuest : QuestBase
		{
			// Token: 0x1700016D RID: 365
			// (get) Token: 0x06000FEF RID: 4079 RVA: 0x00069743 File Offset: 0x00067943
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700016E RID: 366
			// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x00069746 File Offset: 0x00067946
			private bool FightEnded
			{
				get
				{
					return this._isCulpritDiedInMissionFight || this._isNotableKnockedDownInMissionFight || this._persuationInDoneAndSuccessfull;
				}
			}

			// Token: 0x1700016F RID: 367
			// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x00069760 File Offset: 0x00067960
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=ZpDQxmzJ}Family Feud", null);
				}
			}

			// Token: 0x17000170 RID: 368
			// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00069770 File Offset: 0x00067970
			private TextObject PlayerStartsQuestLogText1
			{
				get
				{
					TextObject textObject = new TextObject("{=rjHQpVDZ}{QUEST_GIVER.LINK} a landowner from {QUEST_GIVER_SETTLEMENT}, told you about an incident that is about to turn into an ugly feud. One of the youngsters killed another during a quarrel and the victim's family refuses the blood money as compensation and wants blood.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_GIVER_SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17000171 RID: 369
			// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x000697C0 File Offset: 0x000679C0
			private TextObject PlayerStartsQuestLogText2
			{
				get
				{
					TextObject textObject = new TextObject("{=fgRq7kF2}You agreed to talk to {CULPRIT.LINK} in {QUEST_GIVER_SETTLEMENT} first and convince him to go to {TARGET_NOTABLE.LINK} with you in {TARGET_SETTLEMENT} and mediate the issue between them peacefully and end unnecessary bloodshed. {QUEST_GIVER.LINK} said {?QUEST_GIVER.GENDER}she{?}he{\\?} will pay you {REWARD_GOLD} once the boy is safe again.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("CULPRIT", this._culprit.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_GIVER_SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("REWARD_GOLD", this._rewardGold);
					return textObject;
				}
			}

			// Token: 0x17000172 RID: 370
			// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x00069868 File Offset: 0x00067A68
			private TextObject SuccessQuestSolutionLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=KJ61SXEU}You have successfully protected {CULPRIT.LINK} from harm as you have promised. {QUEST_GIVER.LINK} is grateful for your service and sends his regards with a purse full of {REWARD_GOLD}{GOLD_ICON} denars for your trouble.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("CULPRIT", this._culprit.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD_GOLD", this._rewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					return textObject;
				}
			}

			// Token: 0x17000173 RID: 371
			// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x000698D8 File Offset: 0x00067AD8
			private TextObject CulpritJoinedPlayerPartyLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=s5fXZf2f}You have convinced {CULPRIT.LINK} to go to {TARGET_SETTLEMENT} to face {TARGET_NOTABLE.LINK} to try to solve this issue peacefully. He agreed on the condition that you protect him from his victim's angry relatives.", null);
					StringHelpers.SetCharacterProperties("CULPRIT", this._culprit.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17000174 RID: 372
			// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x0006993C File Offset: 0x00067B3C
			private TextObject QuestGiverVillageRaidedBeforeTalkingToCulpritCancel
			{
				get
				{
					TextObject textObject = new TextObject("{=gJG0xmAq}{QUEST_GIVER.LINK}'s village {QUEST_SETTLEMENT} was raided. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17000175 RID: 373
			// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x0006998C File Offset: 0x00067B8C
			private TextObject TargetVillageRaidedBeforeTalkingToCulpritCancel
			{
				get
				{
					TextObject textObject = new TextObject("{=WqY4nvHc}{TARGET_NOTABLE.LINK}'s village {TARGET_SETTLEMENT} was raided. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this._targetSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x000699F0 File Offset: 0x00067BF0
			private TextObject CulpritDiedQuestFail
			{
				get
				{
					TextObject textObject = new TextObject("{=6zcG8eng}You tried to defend {CULPRIT.LINK} but you were overcome. {NOTABLE.LINK} took {?NOTABLE.GENDER}her{?}his{\\?} revenge. You failed to protect {CULPRIT.LINK} as promised to {QUEST_GIVER.LINK}. {?QUEST_GIVER.GENDER}she{?}he{\\?} is furious.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("CULPRIT", this._culprit.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x00069A54 File Offset: 0x00067C54
			private TextObject PlayerDiedInNotableBattle
			{
				get
				{
					TextObject textObject = new TextObject("{=kG92fjCY}You fell unconscious while defending {CULPRIT.LINK}. {TARGET_NOTABLE.LINK} has taken revenge. You failed to protect {CULPRIT.LINK} as you promised {QUEST_GIVER.LINK}. {?QUEST_GIVER.GENDER}She{?}He{\\?} is furious.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("CULPRIT", this._culprit.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00069AB8 File Offset: 0x00067CB8
			private TextObject FailQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=LWjIbTBi}You failed to protect {CULPRIT.LINK} as you promised {QUEST_GIVER.LINK}. {QUEST_GIVER.LINK} is furious.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("CULPRIT", this._culprit.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x06000FFB RID: 4091 RVA: 0x00069B04 File Offset: 0x00067D04
			private TextObject CulpritNoLongerAClanMember
			{
				get
				{
					TextObject textObject = new TextObject("{=wWrEvkuj}{CULPRIT.LINK} is no longer a member of your clan. Your agreement with {QUEST_GIVER.LINK} was terminated.", null);
					StringHelpers.SetCharacterProperties("CULPRIT", this._culprit.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x06000FFC RID: 4092 RVA: 0x00069B4E File Offset: 0x00067D4E
			private TextObject CompanionLimitReachedQuestLogText
			{
				get
				{
					return new TextObject("{=rkQ7D36f}The quest was canceled because your party had more companions than you could manage.", null);
				}
			}

			// Token: 0x06000FFD RID: 4093 RVA: 0x00069B5C File Offset: 0x00067D5C
			public FamilyFeudIssueQuest(string questId, Hero questGiver, CampaignTime duration, Settlement targetSettlement, Hero targetHero, int rewardGold)
				: base(questId, questGiver, duration, rewardGold)
			{
				this._targetSettlement = targetSettlement;
				this._targetNotable = targetHero;
				this._culpritJoinedPlayerParty = false;
				this._checkForMissionEvents = false;
				this._culprit = HeroCreator.CreateSpecialHero(MBObjectManager.Instance.GetObject<CharacterObject>("townsman_" + targetSettlement.Culture.StringId), targetSettlement, null, null, -1);
				this._culprit.SetNewOccupation(Occupation.Wanderer);
				ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("pugio");
				this._culprit.CivilianEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(@object, null, null, false));
				this._culprit.BattleEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(@object, null, null, false));
				this._notableGangsterCharacterObject = questGiver.CurrentSettlement.MapFaction.Culture.GangleaderBodyguard;
				this._rewardGold = rewardGold;
				this.InitializeQuestDialogs();
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06000FFE RID: 4094 RVA: 0x00069C48 File Offset: 0x00067E48
			private void InitializeQuestDialogs()
			{
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCulpritDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetNotableThugDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetNotableDialogFlowBeforeTalkingToCulprit(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetNotableDialogFlowAfterTalkingToCulprit(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetNotableDialogFlowAfterKillingCulprit(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetNotableDialogFlowAfterPlayerBetrayCulprit(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCulpritDialogFlowAfterCulpritJoin(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetNotableDialogFlowAfterNotableKnowdown(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetNotableDialogFlowAfterQuestEnd(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCulpritDialogFlowAfterQuestEnd(), this);
			}

			// Token: 0x06000FFF RID: 4095 RVA: 0x00069D31 File Offset: 0x00067F31
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
				this.InitializeQuestDialogs();
				this._notableGangsterCharacterObject = MBObjectManager.Instance.GetObject<CharacterObject>("gangster_1");
			}

			// Token: 0x06001000 RID: 4096 RVA: 0x00069D54 File Offset: 0x00067F54
			protected override void HourlyTick()
			{
			}

			// Token: 0x06001001 RID: 4097 RVA: 0x00069D58 File Offset: 0x00067F58
			private DialogFlow GetNotableDialogFlowBeforeTalkingToCulprit()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=dpTHWqwv}Are you the {?PLAYER.GENDER}woman{?}man{\\?} who thinks our blood is cheap, that we will accept silver for the life of one of our own?", null), null, null, null, null).Condition(new ConversationSentence.OnConditionDelegate(this.notable_culprit_is_not_near_on_condition))
					.NpcLine(new TextObject("{=Vd22iVGE}Well {?PLAYER.GENDER}lady{?}sir{\\?}, sorry to disappoint you, but our people have some self-respect.", null), null, null, null, null)
					.PlayerLine(new TextObject("{=a3AFjfsU}We will see. ", null), null, null, null)
					.NpcLine(new TextObject("{=AeJqCMJc}Yes, you will see. Good day to you. ", null), null, null, null, null)
					.CloseDialog();
			}

			// Token: 0x06001002 RID: 4098 RVA: 0x00069DD8 File Offset: 0x00067FD8
			private DialogFlow GetNotableDialogFlowAfterKillingCulprit()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=108Dchvt}Stop! We don't need to fight any longer. We have no quarrel with you as justice has been served.", null), null, null, null, null).Condition(() => this._isCulpritDiedInMissionFight && Hero.OneToOneConversationHero == this._targetNotable && !this._playerBetrayedCulprit)
					.NpcLine(new TextObject("{=NMrzr7Me}Now, leave peacefully...", null), null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.CulpritDiedInNotableFightFail;
					})
					.CloseDialog();
			}

			// Token: 0x06001003 RID: 4099 RVA: 0x00069E40 File Offset: 0x00068040
			private DialogFlow GetNotableDialogFlowAfterPlayerBetrayCulprit()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=4aiabOd4}I knew you are a reasonable {?PLAYER.GENDER}woman{?}man{\\?}.", null), null, null, null, null).Condition(() => this._isCulpritDiedInMissionFight && this._playerBetrayedCulprit && Hero.OneToOneConversationHero == this._targetNotable)
					.NpcLine(new TextObject("{=NMrzr7Me}Now, leave peacefully...", null), null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.CulpritDiedInNotableFightFail;
					})
					.CloseDialog();
			}

			// Token: 0x06001004 RID: 4100 RVA: 0x00069EA8 File Offset: 0x000680A8
			private DialogFlow GetCulpritDialogFlowAfterCulpritJoin()
			{
				TextObject textObject = new TextObject("{=56ynu2bW}Yes, {?PLAYER.GENDER}milady{?}sir{\\?}.", null);
				TextObject textObject2 = new TextObject("{=c452Kevh}Well I'm anxious, but I am in your hands now. I trust you will protect me {?PLAYER.GENDER}milady{?}sir{\\?}.", null);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject, false);
				StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, textObject2, false);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(() => !this.FightEnded && this._culpritJoinedPlayerParty && Hero.OneToOneConversationHero == this._culprit)
					.PlayerLine(new TextObject("{=p1ETQbzg}Just checking on you.", null), null, null, null)
					.NpcLine(textObject2, null, null, null, null)
					.CloseDialog();
			}

			// Token: 0x06001005 RID: 4101 RVA: 0x00069F3C File Offset: 0x0006813C
			private DialogFlow GetNotableDialogFlowAfterQuestEnd()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=UBFS1JLj}I have no problem with the boy anymore,[ib:closed][if:convo_annoyed] okay? Just leave me alone.", null), null, null, null, null).Condition(() => this.FightEnded && !this._persuationInDoneAndSuccessfull && Hero.OneToOneConversationHero == this._targetNotable && !this._playerBetrayedCulprit)
					.CloseDialog()
					.NpcLine(new TextObject("{=adbQR9j0}I got my gold, you got your boy.[if:convo_bored2] Now leave me alone...", null), null, null, null, null)
					.Condition(() => this.FightEnded && this._persuationInDoneAndSuccessfull && Hero.OneToOneConversationHero == this._targetNotable && !this._playerBetrayedCulprit)
					.CloseDialog();
			}

			// Token: 0x06001006 RID: 4102 RVA: 0x00069FA9 File Offset: 0x000681A9
			private DialogFlow GetCulpritDialogFlowAfterQuestEnd()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=OybG76Kf}Thank you for saving me, sir.[ib:normal][if:convo_astonished] I won't forget what you did here today.", null), null, null, null, null).Condition(() => this.FightEnded && Hero.OneToOneConversationHero == this._culprit)
					.CloseDialog();
			}

			// Token: 0x06001007 RID: 4103 RVA: 0x00069FE4 File Offset: 0x000681E4
			private DialogFlow GetNotableDialogFlowAfterNotableKnowdown()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=c6GbRQlg}Stop. We don’t need to fight any longer. [ib:closed][if:convo_insulted]You have made your point. We will accept the blood money.", null), (IAgent agent) => this.IsTargetNotable(agent), (IAgent agent) => this.IsMainAgent(agent), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.multi_character_conversation_condition_after_fight))
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.multi_character_conversation_consequence_after_fight))
					.NpcLine(new TextObject("{=pS0bBRjt}You! Go to your family and tell [if:convo_angry]them to send us the blood money.", null), (IAgent agent) => this.IsTargetNotable(agent), (IAgent agent) => this.IsCulprit(agent), null, null)
					.NpcLine(new TextObject("{=nxs2U0Yk}Leave now and never come back! [if:convo_furious]If we ever see you here we will kill you.", null), (IAgent agent) => this.IsTargetNotable(agent), (IAgent agent) => this.IsCulprit(agent), null, null)
					.NpcLine("{=udD7Y7mO}Thank you, my {?PLAYER.GENDER}lady{?}sir{\\?}, for protecting me. I will go and tell {ISSUE_GIVER.LINK} of your success here.", (IAgent agent) => this.IsCulprit(agent), (IAgent agent) => this.IsMainAgent(agent), null, null)
					.Condition(new ConversationSentence.OnConditionDelegate(this.AfterNotableKnowdownEndingCondition))
					.PlayerLine(new TextObject("{=g8qb3Ame}Thank you.", null), (IAgent agent) => this.IsCulprit(agent), null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerAndCulpritKnockedDownNotableQuestSuccess;
					})
					.CloseDialog();
			}

			// Token: 0x06001008 RID: 4104 RVA: 0x0006A106 File Offset: 0x00068306
			private bool AfterNotableKnowdownEndingCondition()
			{
				StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.QuestGiver.CharacterObject, null, false);
				return true;
			}

			// Token: 0x06001009 RID: 4105 RVA: 0x0006A121 File Offset: 0x00068321
			private void PlayerAndCulpritKnockedDownNotableQuestSuccess()
			{
				this._conversationAfterFightIsDone = true;
				this.HandleAgentBehaviorAfterQuestConversations();
			}

			// Token: 0x0600100A RID: 4106 RVA: 0x0006A130 File Offset: 0x00068330
			private void HandleAgentBehaviorAfterQuestConversations()
			{
				foreach (AccompanyingCharacter accompanyingCharacter in PlayerEncounter.LocationEncounter.CharactersAccompanyingPlayer)
				{
					if (accompanyingCharacter.LocationCharacter.Character == this._culprit.CharacterObject && this._culpritAgent.IsActive())
					{
						accompanyingCharacter.LocationCharacter.SpecialTargetTag = "npc_common";
						accompanyingCharacter.LocationCharacter.CharacterRelation = LocationCharacter.CharacterRelations.Neutral;
						this._culpritAgent.SetMortalityState(Agent.MortalityState.Invulnerable);
						this._culpritAgent.SetTeam(Team.Invalid, false);
						DailyBehaviorGroup behaviorGroup = this._culpritAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
						behaviorGroup.AddBehavior<WalkingBehavior>();
						behaviorGroup.RemoveBehavior<FollowAgentBehavior>();
						this._culpritAgent.ResetEnemyCaches();
						this._culpritAgent.InvalidateTargetAgent();
						this._culpritAgent.InvalidateAIWeaponSelections();
						this._culpritAgent.SetWatchState(Agent.WatchState.Patrolling);
						if (this._notableAgent != null)
						{
							this._notableAgent.ResetEnemyCaches();
							this._notableAgent.InvalidateTargetAgent();
							this._notableAgent.InvalidateAIWeaponSelections();
							this._notableAgent.SetWatchState(Agent.WatchState.Patrolling);
						}
						this._culpritAgent.TryToSheathWeaponInHand(Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.WithAnimationUninterruptible);
						this._culpritAgent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimationUninterruptible);
					}
				}
				Mission.Current.SetMissionMode(MissionMode.StartUp, false);
			}

			// Token: 0x0600100B RID: 4107 RVA: 0x0006A2A0 File Offset: 0x000684A0
			private void ApplySuccessConsequences()
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				this.RelationshipChangeWithQuestGiver = 10;
				ChangeRelationAction.ApplyPlayerRelation(this._targetNotable, -5, true, true);
				base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security += 10f;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x0600100C RID: 4108 RVA: 0x0006A317 File Offset: 0x00068517
			private bool multi_character_conversation_condition_after_fight()
			{
				return !this._conversationAfterFightIsDone && Hero.OneToOneConversationHero == this._targetNotable && this._isNotableKnockedDownInMissionFight;
			}

			// Token: 0x0600100D RID: 4109 RVA: 0x0006A336 File Offset: 0x00068536
			private void multi_character_conversation_consequence_after_fight()
			{
				if (Mission.Current.GetMissionBehavior<MissionConversationLogic>() != null)
				{
					Campaign.Current.ConversationManager.AddConversationAgents(new List<Agent> { this._culpritAgent }, true);
				}
				this._conversationAfterFightIsDone = true;
			}

			// Token: 0x0600100E RID: 4110 RVA: 0x0006A36C File Offset: 0x0006856C
			private DialogFlow GetNotableDialogFlowAfterTalkingToCulprit()
			{
				DialogFlow dialogFlow = DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=nh7a3Nog}Well well. Who did you bring to see us? [ib:confident][if:convo_irritable]Did he bring his funeral shroud with him? I hope so. He's not leaving here alive.", null), (IAgent agent) => this.IsTargetNotable(agent), (IAgent agent) => this.IsCulprit(agent), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.multi_character_conversation_on_condition))
					.NpcLine(new TextObject("{=RsOmvdmU}We have come to talk! Just listen to us please![if:convo_shocked]", null), (IAgent agent) => this.IsCulprit(agent), (IAgent agent) => this.IsTargetNotable(agent), null, null)
					.NpcLine("{=JUjvu4XL}I knew we'd find you eventually. Now you will face justice![if:convo_evil_smile]", (IAgent agent) => this.IsTargetNotable(agent), (IAgent agent) => this.IsCulprit(agent), null, null)
					.PlayerLine("{=UQyCoQCY}Wait! This lad is now under my protection. We have come to talk in peace..", (IAgent agent) => this.IsTargetNotable(agent), null, null)
					.NpcLine("{=7AiP4BwY}What there is to talk about? [if:convo_confused_annoyed]This bastard murdered one of my kinsman, and it is our right to take vengeance on him!", (IAgent agent) => this.IsTargetNotable(agent), (IAgent agent) => this.IsMainAgent(agent), null, null)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=2iVytG2y}I am not convinced. I will protect the accused until you see reason.", null), null, null, null)
					.NpcLine(new TextObject("{=4HokUcma}You will regret pushing [if:convo_very_stern]your nose into issues that do not concern you!", null), null, null, null, null)
					.NpcLine(new TextObject("{=vjOkDM6C}If you defend a murderer [ib:warrior][if:convo_furious]then you die like a murderer. Boys, kill them all!", null), null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
						{
							this.StartFightWithNotableGang(false);
						};
					})
					.CloseDialog()
					.PlayerOption(new TextObject("{=boAcQxVV}You're breaking the law.", null), null, null, null)
					.Condition(delegate
					{
						if (this._task != null)
						{
							return !this._task.Options.All<PersuasionOptionArgs>((PersuasionOptionArgs x) => x.IsBlocked);
						}
						return true;
					})
					.GotoDialogState("start_notable_family_feud_persuasion")
					.PlayerOption(new TextObject("{=J5cQPqGQ}You are right. You are free to deliver justice as you see fit.", null), null, null, null)
					.NpcLine(new TextObject("{=aRPLW15x}Thank you. I knew you are a reasonable[ib:aggressive][if:convo_evil_smile] {?PLAYER.GENDER}woman{?}man{\\?}.", null), null, null, null, null)
					.NpcLine(new TextObject("{=k5R4qGtL}What? Are you just going [ib:nervous][if:convo_nervous2]to leave me here to be killed? My kin will never forget this!", null), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsCulprit), new ConversationSentence.OnMultipleConversationConsequenceDelegate(this.IsMainAgent), null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
						{
							this._playerBetrayedCulprit = true;
							this.StartFightWithNotableGang(this._playerBetrayedCulprit);
						};
					})
					.CloseDialog();
				this.AddPersuasionDialogs(dialogFlow);
				return dialogFlow;
			}

			// Token: 0x0600100F RID: 4111 RVA: 0x0006A540 File Offset: 0x00068740
			private bool IsMainAgent(IAgent agent)
			{
				return agent == Mission.Current.MainAgent;
			}

			// Token: 0x06001010 RID: 4112 RVA: 0x0006A54F File Offset: 0x0006874F
			private bool IsTargetNotable(IAgent agent)
			{
				return agent.Character == this._targetNotable.CharacterObject;
			}

			// Token: 0x06001011 RID: 4113 RVA: 0x0006A564 File Offset: 0x00068764
			private bool IsCulprit(IAgent agent)
			{
				return agent.Character == this._culprit.CharacterObject;
			}

			// Token: 0x06001012 RID: 4114 RVA: 0x0006A57C File Offset: 0x0006877C
			private bool notable_culprit_is_not_near_on_condition()
			{
				return Hero.OneToOneConversationHero == this._targetNotable && Mission.Current != null && !this.FightEnded && Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 10f, new MBList<Agent>()).All<Agent>((Agent a) => a.Character != this._culprit.CharacterObject);
			}

			// Token: 0x06001013 RID: 4115 RVA: 0x0006A5E0 File Offset: 0x000687E0
			private bool multi_character_conversation_on_condition()
			{
				if (Hero.OneToOneConversationHero != this._targetNotable || Mission.Current == null || this.FightEnded)
				{
					return false;
				}
				MBList<Agent> nearbyAgents = Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 10f, new MBList<Agent>());
				if (nearbyAgents.IsEmpty<Agent>() || nearbyAgents.All<Agent>((Agent a) => a.Character != this._culprit.CharacterObject))
				{
					return false;
				}
				foreach (Agent agent in nearbyAgents)
				{
					if (agent.Character == this._culprit.CharacterObject)
					{
						this._culpritAgent = agent;
						if (Mission.Current.GetMissionBehavior<MissionConversationLogic>() != null)
						{
							Campaign.Current.ConversationManager.AddConversationAgents(new List<Agent> { this._culpritAgent }, true);
							break;
						}
						break;
					}
				}
				return true;
			}

			// Token: 0x06001014 RID: 4116 RVA: 0x0006A6D4 File Offset: 0x000688D4
			private void AddPersuasionDialogs(DialogFlow dialog)
			{
				dialog.AddDialogLine("family_feud_notable_persuasion_check_accepted", "start_notable_family_feud_persuasion", "family_feud_notable_persuasion_start_reservation", "{=6P1ruzsC}Maybe...", null, new ConversationSentence.OnConsequenceDelegate(this.persuasion_start_with_notable_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("family_feud_notable_persuasion_failed", "family_feud_notable_persuasion_start_reservation", "persuation_failed", "{=!}{FAILED_PERSUASION_LINE}", new ConversationSentence.OnConditionDelegate(this.persuasion_failed_with_family_feud_notable_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_failed_with_notable_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("family_feud_notable_persuasion_rejected", "persuation_failed", "close_window", "{=vjOkDM6C}If you defend a murderer [ib:warrior][if:convo_furious]then you die like a murderer. Boys, kill them all!", null, new ConversationSentence.OnConsequenceDelegate(this.persuasion_failed_with_notable_start_fight_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("family_feud_notable_persuasion_attempt", "family_feud_notable_persuasion_start_reservation", "family_feud_notable_persuasion_select_option", "{CONTINUE_PERSUASION_LINE}", () => !this.persuasion_failed_with_family_feud_notable_on_condition(), null, this, 100, null, null, null);
				dialog.AddDialogLine("family_feud_notable_persuasion_success", "family_feud_notable_persuasion_start_reservation", "close_window", "{=qIQbIjVS}All right! I spare the boy's life. Now get out of my sight[ib:closed][if:convo_nonchalant]", new ConversationSentence.OnConditionDelegate(ConversationManager.GetPersuasionProgressSatisfied), new ConversationSentence.OnConsequenceDelegate(this.persuasion_complete_with_notable_on_consequence), this, int.MaxValue, null, null, null);
				string text = "family_feud_notable_persuasion_select_option_1";
				string text2 = "family_feud_notable_persuasion_select_option";
				string text3 = "family_feud_notable_persuasion_selected_option_response";
				string text4 = "{=!}{FAMILY_FEUD_PERSUADE_ATTEMPT_1}";
				ConversationSentence.OnConditionDelegate onConditionDelegate = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_1_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_1_on_consequence);
				ConversationSentence.OnPersuasionOptionDelegate onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_1);
				ConversationSentence.OnClickableConditionDelegate onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_1_on_condition);
				dialog.AddPlayerLine(text, text2, text3, text4, onConditionDelegate, onConsequenceDelegate, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text5 = "family_feud_notable_persuasion_select_option_2";
				string text6 = "family_feud_notable_persuasion_select_option";
				string text7 = "family_feud_notable_persuasion_selected_option_response";
				string text8 = "{=!}{FAMILY_FEUD_PERSUADE_ATTEMPT_2}";
				ConversationSentence.OnConditionDelegate onConditionDelegate2 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_2_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate2 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_2_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_2);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_2_on_condition);
				dialog.AddPlayerLine(text5, text6, text7, text8, onConditionDelegate2, onConsequenceDelegate2, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				string text9 = "family_feud_notable_persuasion_select_option_3";
				string text10 = "family_feud_notable_persuasion_select_option";
				string text11 = "family_feud_notable_persuasion_selected_option_response";
				string text12 = "{=!}{FAMILY_FEUD_PERSUADE_ATTEMPT_3}";
				ConversationSentence.OnConditionDelegate onConditionDelegate3 = new ConversationSentence.OnConditionDelegate(this.persuasion_select_option_3_on_condition);
				ConversationSentence.OnConsequenceDelegate onConsequenceDelegate3 = new ConversationSentence.OnConsequenceDelegate(this.persuasion_select_option_3_on_consequence);
				onPersuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.persuasion_setup_option_3);
				onClickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.persuasion_clickable_option_3_on_condition);
				dialog.AddPlayerLine(text9, text10, text11, text12, onConditionDelegate3, onConsequenceDelegate3, this, 100, onClickableConditionDelegate, onPersuasionOptionDelegate, null, null);
				dialog.AddDialogLine("family_feud_notable_persuasion_select_option_reaction", "family_feud_notable_persuasion_selected_option_response", "family_feud_notable_persuasion_start_reservation", "{=D0xDRqvm}{PERSUASION_REACTION}", new ConversationSentence.OnConditionDelegate(this.persuasion_selected_option_response_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_selected_option_response_on_consequence), this, 100, null, null, null);
			}

			// Token: 0x06001015 RID: 4117 RVA: 0x0006A915 File Offset: 0x00068B15
			private void persuasion_complete_with_notable_on_consequence()
			{
				ConversationManager.EndPersuasion();
				this._persuationInDoneAndSuccessfull = true;
				this.HandleAgentBehaviorAfterQuestConversations();
			}

			// Token: 0x06001016 RID: 4118 RVA: 0x0006A929 File Offset: 0x00068B29
			private void persuasion_failed_with_notable_on_consequence()
			{
				ConversationManager.EndPersuasion();
			}

			// Token: 0x06001017 RID: 4119 RVA: 0x0006A930 File Offset: 0x00068B30
			private void persuasion_failed_with_notable_start_fight_on_consequence()
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += delegate
				{
					this.StartFightWithNotableGang(false);
				};
			}

			// Token: 0x06001018 RID: 4120 RVA: 0x0006A950 File Offset: 0x00068B50
			private bool persuasion_failed_with_family_feud_notable_on_condition()
			{
				MBTextManager.SetTextVariable("CONTINUE_PERSUASION_LINE", "{=7B7BhVhV}Let's see what you will come up with...[if:convo_confused_annoyed]", false);
				if (this._task.Options.Any<PersuasionOptionArgs>((PersuasionOptionArgs x) => x.IsBlocked))
				{
					MBTextManager.SetTextVariable("CONTINUE_PERSUASION_LINE", "{=wvbiyZfp}What else do you have to say?[if:convo_confused_annoyed]", false);
				}
				if (this._task.Options.All<PersuasionOptionArgs>((PersuasionOptionArgs x) => x.IsBlocked) && !ConversationManager.GetPersuasionProgressSatisfied())
				{
					MBTextManager.SetTextVariable("FAILED_PERSUASION_LINE", this._task.FinalFailLine, false);
					return true;
				}
				return false;
			}

			// Token: 0x06001019 RID: 4121 RVA: 0x0006AA00 File Offset: 0x00068C00
			private void persuasion_selected_option_response_on_consequence()
			{
				Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
				float difficulty = Campaign.Current.Models.PersuasionModel.GetDifficulty(PersuasionDifficulty.MediumHard);
				float num;
				float num2;
				Campaign.Current.Models.PersuasionModel.GetEffectChances(tuple.Item1, out num, out num2, difficulty);
				this._task.ApplyEffects(num, num2);
			}

			// Token: 0x0600101A RID: 4122 RVA: 0x0006AA5C File Offset: 0x00068C5C
			private bool persuasion_selected_option_response_on_condition()
			{
				PersuasionOptionResult item = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>().Item2;
				MBTextManager.SetTextVariable("PERSUASION_REACTION", PersuasionHelper.GetDefaultPersuasionOptionReaction(item), false);
				return true;
			}

			// Token: 0x0600101B RID: 4123 RVA: 0x0006AA8B File Offset: 0x00068C8B
			private void persuasion_start_with_notable_on_consequence()
			{
				this._task = this.GetPersuasionTask();
				ConversationManager.StartPersuasion(2f, 1f, 0f, 2f, 2f, 0f, PersuasionDifficulty.MediumHard);
			}

			// Token: 0x0600101C RID: 4124 RVA: 0x0006AAC0 File Offset: 0x00068CC0
			private bool persuasion_select_option_1_on_condition()
			{
				if (this._task.Options.Count > 0)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(0), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(0).Line);
					MBTextManager.SetTextVariable("FAMILY_FEUD_PERSUADE_ATTEMPT_1", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x0600101D RID: 4125 RVA: 0x0006AB40 File Offset: 0x00068D40
			private bool persuasion_select_option_2_on_condition()
			{
				if (this._task.Options.Count > 1)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(1), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(1).Line);
					MBTextManager.SetTextVariable("FAMILY_FEUD_PERSUADE_ATTEMPT_2", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x0600101E RID: 4126 RVA: 0x0006ABC0 File Offset: 0x00068DC0
			private bool persuasion_select_option_3_on_condition()
			{
				if (this._task.Options.Count > 2)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt<PersuasionOptionArgs>(2), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt<PersuasionOptionArgs>(2).Line);
					MBTextManager.SetTextVariable("FAMILY_FEUD_PERSUADE_ATTEMPT_3", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x0600101F RID: 4127 RVA: 0x0006AC40 File Offset: 0x00068E40
			private void persuasion_select_option_1_on_consequence()
			{
				if (this._task.Options.Count > 0)
				{
					this._task.Options[0].BlockTheOption(true);
				}
			}

			// Token: 0x06001020 RID: 4128 RVA: 0x0006AC6C File Offset: 0x00068E6C
			private void persuasion_select_option_2_on_consequence()
			{
				if (this._task.Options.Count > 1)
				{
					this._task.Options[1].BlockTheOption(true);
				}
			}

			// Token: 0x06001021 RID: 4129 RVA: 0x0006AC98 File Offset: 0x00068E98
			private void persuasion_select_option_3_on_consequence()
			{
				if (this._task.Options.Count > 2)
				{
					this._task.Options[2].BlockTheOption(true);
				}
			}

			// Token: 0x06001022 RID: 4130 RVA: 0x0006ACC4 File Offset: 0x00068EC4
			private PersuasionOptionArgs persuasion_setup_option_1()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(0);
			}

			// Token: 0x06001023 RID: 4131 RVA: 0x0006ACD7 File Offset: 0x00068ED7
			private PersuasionOptionArgs persuasion_setup_option_2()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(1);
			}

			// Token: 0x06001024 RID: 4132 RVA: 0x0006ACEA File Offset: 0x00068EEA
			private PersuasionOptionArgs persuasion_setup_option_3()
			{
				return this._task.Options.ElementAt<PersuasionOptionArgs>(2);
			}

			// Token: 0x06001025 RID: 4133 RVA: 0x0006AD00 File Offset: 0x00068F00
			private bool persuasion_clickable_option_1_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Any<PersuasionOptionArgs>())
				{
					hintText = (this._task.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked ? hintText : null);
					return !this._task.Options.ElementAt<PersuasionOptionArgs>(0).IsBlocked;
				}
				return false;
			}

			// Token: 0x06001026 RID: 4134 RVA: 0x0006AD68 File Offset: 0x00068F68
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

			// Token: 0x06001027 RID: 4135 RVA: 0x0006ADD0 File Offset: 0x00068FD0
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

			// Token: 0x06001028 RID: 4136 RVA: 0x0006AE38 File Offset: 0x00069038
			private PersuasionTask GetPersuasionTask()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=rzGqa5oD}Revenge will be taken. Save your breath for the fight...", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}IF YOU SEE THIS. CALL CAMPAIGN TEAM.", null);
				persuasionTask.SpokenLine = new TextObject("{=6P1ruzsC}Maybe...", null);
				PersuasionOptionArgs persuasionOptionArgs = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Easy, false, new TextObject("{=K9i5SaDc}Blood money is appropriate for a crime of passion. But you kill this boy in cold blood, you will be a real murderer in the eyes of the law, and will no doubt die.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs);
				PersuasionOptionArgs persuasionOptionArgs2 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Valor, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=FUL8TcYa}I promised to protect the boy at the cost of my life. If you try to harm him, you will bleed for it.", null), null, true, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs2);
				PersuasionOptionArgs persuasionOptionArgs3 = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Mercy, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=Ytws5O9S}Some day you may wish to save the life of one of your sons through blood money. If you refuse mercy, mercy may be refused you.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(persuasionOptionArgs3);
				return persuasionTask;
			}

			// Token: 0x06001029 RID: 4137 RVA: 0x0006AEFC File Offset: 0x000690FC
			private void StartFightWithNotableGang(bool playerBetrayedCulprit)
			{
				this._notableAgent = (Agent)MissionConversationLogic.Current.ConversationManager.ConversationAgents[0];
				List<Agent> list = new List<Agent> { this._culpritAgent };
				List<Agent> list2 = new List<Agent> { this._notableAgent };
				MBList<Agent> mblist = new MBList<Agent>();
				foreach (Agent agent in Mission.Current.GetNearbyAgents(Agent.Main.Position.AsVec2, 30f, mblist))
				{
					if ((CharacterObject)agent.Character == this._notableGangsterCharacterObject)
					{
						list2.Add(agent);
					}
				}
				if (playerBetrayedCulprit)
				{
					Agent.Main.SetTeam(Mission.Current.SpectatorTeam, false);
				}
				else
				{
					list.Add(Agent.Main);
					foreach (Agent agent2 in list2)
					{
						agent2.Defensiveness = 2f;
					}
					this._culpritAgent.Health = 350f;
					this._culpritAgent.BaseHealthLimit = 350f;
					this._culpritAgent.HealthLimit = 350f;
				}
				this._notableAgent.Health = 350f;
				this._notableAgent.BaseHealthLimit = 350f;
				this._notableAgent.HealthLimit = 350f;
				Mission.Current.GetMissionBehavior<MissionFightHandler>().StartCustomFight(list, list2, false, false, delegate(bool isPlayerSideWon)
				{
					if (this._isNotableKnockedDownInMissionFight)
					{
						if (Agent.Main != null && this._notableAgent.Position.DistanceSquared(Agent.Main.Position) < 49f)
						{
							MissionConversationLogic.Current.StartConversation(this._notableAgent, false, false);
							return;
						}
						this.PlayerAndCulpritKnockedDownNotableQuestSuccess();
						return;
					}
					else
					{
						if (Agent.Main != null && this._notableAgent.Position.DistanceSquared(Agent.Main.Position) < 49f)
						{
							MissionConversationLogic.Current.StartConversation(this._notableAgent, false, false);
							return;
						}
						this.CulpritDiedInNotableFightFail();
						return;
					}
				}, float.Epsilon);
			}

			// Token: 0x0600102A RID: 4138 RVA: 0x0006B0B4 File Offset: 0x000692B4
			private void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage)
			{
				if (base.IsOngoing && !this._persuationInDoneAndSuccessfull && affectedAgent.Health <= (float)damage && Agent.Main != null)
				{
					if (affectedAgent == this._notableAgent && !this._isNotableKnockedDownInMissionFight)
					{
						affectedAgent.Health = 50f;
						this._isNotableKnockedDownInMissionFight = true;
						Mission.Current.GetMissionBehavior<MissionFightHandler>().EndFight(false);
					}
					if (affectedAgent == this._culpritAgent && !this._isCulpritDiedInMissionFight)
					{
						Blow blow = new Blow
						{
							DamageCalculated = true,
							BaseMagnitude = (float)damage,
							InflictedDamage = damage,
							DamagedPercentage = 1f,
							OwnerId = ((affectorAgent != null) ? affectorAgent.Index : (-1))
						};
						affectedAgent.Die(blow, Agent.KillInfo.Invalid);
						this._isCulpritDiedInMissionFight = true;
					}
				}
			}

			// Token: 0x0600102B RID: 4139 RVA: 0x0006B184 File Offset: 0x00069384
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=JjXETjYb}Thank you.[ib:demure][if:convo_thinking] I have to add, I'm ready to pay you {REWARD_GOLD}{GOLD_ICON} denars for your trouble. He is hiding somewhere nearby. Go talk to him, and tell him that you're here to sort things out.", null), null, null, null, null).Condition(delegate
				{
					MBTextManager.SetTextVariable("REWARD_GOLD", this._rewardGold);
					MBTextManager.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">", false);
					return Hero.OneToOneConversationHero == base.QuestGiver;
				})
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=ndDpjT8s}Have you been able to talk with my boy yet?[if:convo_innocent_smile]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=ETiAbgHa}I will talk with them right away", null), null, null, null)
					.NpcLine(new TextObject("{=qmqTLZ9R}Thank you {?PLAYER.GENDER}madam{?}sir{\\?}. You are a savior.", null), null, null, null, null)
					.CloseDialog()
					.PlayerOption(new TextObject("{=18NtjryL}Not yet, but I will soon.", null), null, null, null)
					.NpcLine(new TextObject("{=HeIIW3EH}We are waiting for your good news {?PLAYER.GENDER}milady{?}sir{\\?}.", null), null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x0600102C RID: 4140 RVA: 0x0006B280 File Offset: 0x00069480
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this.PlayerStartsQuestLogText1, false);
				base.AddLog(this.PlayerStartsQuestLogText2, false);
				base.AddTrackedObject(this._targetNotable);
				base.AddTrackedObject(this._culprit);
				Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("village_center");
				Settlement.CurrentSettlement.LocationComplex.ChangeLocation(this.CreateCulpritLocationCharacter(Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral), null, locationWithId);
			}

			// Token: 0x0600102D RID: 4141 RVA: 0x0006B300 File Offset: 0x00069500
			private DialogFlow GetCulpritDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=w0HPC53e}Who are you? What do you want from me?[ib:nervous][if:convo_bared_teeth]", null), null, null, null, null).Condition(() => !this._culpritJoinedPlayerParty && Hero.OneToOneConversationHero == this._culprit)
					.PlayerLine(new TextObject("{=UGTCe2qP}Relax. I've talked with your relative, {QUEST_GIVER.NAME}. I know all about your situation. I'm here to help.", null), null, null, null)
					.Condition(delegate
					{
						StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
						return Hero.OneToOneConversationHero == this._culprit;
					})
					.Consequence(delegate
					{
						this._culprit.SetHasMet();
					})
					.NpcLine(new TextObject("{=45llLiYG}How will you help? Will you protect me?[ib:normal][if:convo_astonished]", null), null, null, null, null)
					.PlayerLine(new TextObject("{=4mwSvCgG}Yes I will. Come now, I will take you with me to {TARGET_NOTABLE.NAME} to resolve this issue peacefully.", null), null, null, null)
					.Condition(delegate
					{
						StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, null, false);
						return Hero.OneToOneConversationHero == this._culprit;
					})
					.NpcLine(new TextObject("{=bHRZhYzd}No! I won't go anywhere near them! They'll kill me![ib:closed2][if:convo_stern]", null), null, null, null, null)
					.PlayerLine(new TextObject("{=sakSp6H8}You can't hide in the shadows forever. I pledge on my honor to protect you if things turn ugly.", null), null, null, null)
					.NpcLine(new TextObject("{=4CFOH0kB}I'm still not sure about all this, but I suppose you're right that I don't have much choice. Let's go get this over.[ib:closed][if:convo_pondering]", null), null, null, null, null)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += this.CulpritJoinedPlayersArmy;
					})
					.CloseDialog();
			}

			// Token: 0x0600102E RID: 4142 RVA: 0x0006B3FC File Offset: 0x000695FC
			private DialogFlow GetNotableThugDialogFlow()
			{
				TextObject textObject = new TextObject("{=QMaYa25R}If you dare to even breathe a word against {TARGET_NOTABLE.LINK},[ib:aggressive2][if:convo_furious] it will be your last. You got it scum?", null);
				StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject, false);
				TextObject textObject2 = new TextObject("{=vGnY4KBO}I care very little for your threats. My business is with {TARGET_NOTABLE.LINK}.", null);
				StringHelpers.SetCharacterProperties("TARGET_NOTABLE", this._targetNotable.CharacterObject, textObject2, false);
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(textObject, null, null, null, null).Condition(delegate
				{
					if (this._notableThugs != null)
					{
						return this._notableThugs.Exists((LocationCharacter x) => x.AgentOrigin == Campaign.Current.ConversationManager.ConversationAgents[0].Origin);
					}
					return false;
				})
					.PlayerLine(textObject2, null, null, null)
					.CloseDialog();
			}

			// Token: 0x0600102F RID: 4143 RVA: 0x0006B488 File Offset: 0x00069688
			private void CulpritJoinedPlayersArmy()
			{
				this._culprit.ChangeState(Hero.CharacterStates.Active);
				AddCompanionAction.Apply(Clan.PlayerClan, this._culprit);
				AddHeroToPartyAction.Apply(this._culprit, MobileParty.MainParty, true);
				base.AddLog(this.CulpritJoinedPlayerPartyLogText, false);
				if (Mission.Current != null)
				{
					DailyBehaviorGroup behaviorGroup = ((Agent)MissionConversationLogic.Current.ConversationManager.ConversationAgents[0]).GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
					FollowAgentBehavior followAgentBehavior = behaviorGroup.AddBehavior<FollowAgentBehavior>();
					behaviorGroup.SetScriptedBehavior<FollowAgentBehavior>();
					followAgentBehavior.SetTargetAgent(Agent.Main);
				}
				this._culpritJoinedPlayerParty = true;
			}

			// Token: 0x06001030 RID: 4144 RVA: 0x0006B520 File Offset: 0x00069720
			protected override void RegisterEvents()
			{
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.VillageBeingRaided.AddNonSerializedListener(this, new Action<Village>(this.OnVillageRaid));
				CampaignEvents.BeforeMissionOpenedEvent.AddNonSerializedListener(this, new Action(this.OnBeforeMissionOpened));
				CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
				CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnd));
				CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionStarted));
				CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
				CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
				CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnPrisonerTaken));
				CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.CanMoveToSettlementEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.CanMoveToSettlement));
				CampaignEvents.CanHeroDieEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.CanHeroDie));
				CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
				CampaignEvents.PerkResetEvent.AddNonSerializedListener(this, new Action<Hero, PerkObject>(this.OnPerksReset));
				CampaignEvents.NewCompanionAdded.AddNonSerializedListener(this, new Action<Hero>(this.OnNewCompanionAdded));
				CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			}

			// Token: 0x06001031 RID: 4145 RVA: 0x0006B6B4 File Offset: 0x000698B4
			private void OnGameLoadFinished()
			{
				this.CheckCompanionLimit();
			}

			// Token: 0x06001032 RID: 4146 RVA: 0x0006B6BC File Offset: 0x000698BC
			private void OnNewCompanionAdded(Hero hero)
			{
				this.CheckCompanionLimit();
			}

			// Token: 0x06001033 RID: 4147 RVA: 0x0006B6C4 File Offset: 0x000698C4
			private void OnPerksReset(Hero hero, PerkObject perk)
			{
				if (hero == Hero.MainHero)
				{
					this.CheckCompanionLimit();
				}
			}

			// Token: 0x06001034 RID: 4148 RVA: 0x0006B6D4 File Offset: 0x000698D4
			private void CheckCompanionLimit()
			{
				if (Clan.PlayerClan.Companions.Count > Clan.PlayerClan.CompanionLimit)
				{
					base.AddLog(this.CompanionLimitReachedQuestLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06001035 RID: 4149 RVA: 0x0006B708 File Offset: 0x00069908
			private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
			{
				if (!this._culpritJoinedPlayerParty && Settlement.CurrentSettlement == base.QuestGiver.CurrentSettlement)
				{
					Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("village_center").AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateCulpritLocationCharacter), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
				}
			}

			// Token: 0x06001036 RID: 4150 RVA: 0x0006B760 File Offset: 0x00069960
			private void CanMoveToSettlement(Hero hero, ref bool result)
			{
				this.CommonRestrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x06001037 RID: 4151 RVA: 0x0006B76A File Offset: 0x0006996A
			public override void OnHeroCanBeSelectedInInventoryInfoIsRequested(Hero hero, ref bool result)
			{
				this.CommonRestrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x06001038 RID: 4152 RVA: 0x0006B774 File Offset: 0x00069974
			public override void OnHeroCanHavePartyRoleOrBeGovernorInfoIsRequested(Hero hero, ref bool result)
			{
				this.CommonRestrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x06001039 RID: 4153 RVA: 0x0006B77E File Offset: 0x0006997E
			public override void OnHeroCanLeadPartyInfoIsRequested(Hero hero, ref bool result)
			{
				this.CommonRestrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x0600103A RID: 4154 RVA: 0x0006B788 File Offset: 0x00069988
			public override void OnHeroCanHaveCampaignIssuesInfoIsRequested(Hero hero, ref bool result)
			{
				this.CommonRestrictionInfoIsRequested(hero, ref result);
			}

			// Token: 0x0600103B RID: 4155 RVA: 0x0006B792 File Offset: 0x00069992
			private void CommonRestrictionInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == this._culprit || this._targetNotable == hero)
				{
					result = false;
				}
			}

			// Token: 0x0600103C RID: 4156 RVA: 0x0006B7A9 File Offset: 0x000699A9
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x0600103D RID: 4157 RVA: 0x0006B7BC File Offset: 0x000699BC
			private void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
			{
				if (hero == this._targetNotable)
				{
					result = false;
					return;
				}
				if (hero == Hero.MainHero && Settlement.CurrentSettlement == this._targetSettlement && Mission.Current != null)
				{
					result = false;
				}
			}

			// Token: 0x0600103E RID: 4158 RVA: 0x0006B7EC File Offset: 0x000699EC
			private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
			{
				if (victim == this._targetNotable)
				{
					TextObject textObject = ((detail == KillCharacterAction.KillCharacterActionDetail.Lost) ? this.TargetHeroDisappearedLogText : this.TargetHeroDiedLogText);
					StringHelpers.SetCharacterProperties("QUEST_TARGET", this._targetNotable.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					base.AddLog(textObject, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x0600103F RID: 4159 RVA: 0x0006B855 File Offset: 0x00069A55
			private void OnPrisonerTaken(PartyBase capturer, Hero prisoner)
			{
				if (prisoner == this._culprit)
				{
					base.AddLog(this.FailQuestLogText, false);
					this.TiemoutFailConsequences();
					base.CompleteQuestWithFail(null);
				}
			}

			// Token: 0x06001040 RID: 4160 RVA: 0x0006B87C File Offset: 0x00069A7C
			private void OnVillageRaid(Village village)
			{
				if (village == this._targetSettlement.Village)
				{
					base.AddLog(this.TargetVillageRaidedBeforeTalkingToCulpritCancel, false);
					base.CompleteQuestWithCancel(null);
					return;
				}
				if (village == base.QuestGiver.CurrentSettlement.Village && !this._culpritJoinedPlayerParty)
				{
					base.AddLog(this.QuestGiverVillageRaidedBeforeTalkingToCulpritCancel, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06001041 RID: 4161 RVA: 0x0006B8DD File Offset: 0x00069ADD
			private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
			{
				if (base.IsOngoing && !this._isCulpritDiedInMissionFight && !this._isPlayerKnockedOutMissionFight && companion == this._culprit)
				{
					base.AddLog(this.CulpritNoLongerAClanMember, false);
					this.TiemoutFailConsequences();
					base.CompleteQuestWithFail(null);
				}
			}

			// Token: 0x06001042 RID: 4162 RVA: 0x0006B91C File Offset: 0x00069B1C
			public void OnMissionStarted(IMission iMission)
			{
				if (this._checkForMissionEvents)
				{
					if (PlayerEncounter.LocationEncounter.CharactersAccompanyingPlayer.All<AccompanyingCharacter>((AccompanyingCharacter x) => x.LocationCharacter.Character != this._culprit.CharacterObject))
					{
						LocationCharacter locationCharacterOfHero = LocationComplex.Current.GetLocationCharacterOfHero(this._culprit);
						if (locationCharacterOfHero != null)
						{
							PlayerEncounter.LocationEncounter.AddAccompanyingCharacter(locationCharacterOfHero, true);
						}
					}
					FamilyFeudIssueBehavior.FamilyFeudIssueMissionBehavior familyFeudIssueMissionBehavior = new FamilyFeudIssueBehavior.FamilyFeudIssueMissionBehavior(new Action<Agent, Agent, int>(this.OnAgentHit));
					Mission.Current.AddMissionBehavior(familyFeudIssueMissionBehavior);
					Mission.Current.GetMissionBehavior<MissionConversationLogic>().SetSpawnArea("alley_2");
				}
			}

			// Token: 0x06001043 RID: 4163 RVA: 0x0006B9A0 File Offset: 0x00069BA0
			private void OnMissionEnd(IMission mission)
			{
				if (this._checkForMissionEvents)
				{
					this._notableAgent = null;
					this._culpritAgent = null;
					if (Agent.Main == null)
					{
						base.AddLog(this.PlayerDiedInNotableBattle, false);
						this.RelationshipChangeWithQuestGiver = -10;
						base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
						base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security -= 5f;
						this._isPlayerKnockedOutMissionFight = true;
						base.CompleteQuestWithFail(null);
						return;
					}
					if (this._isCulpritDiedInMissionFight)
					{
						if (this._playerBetrayedCulprit)
						{
							base.AddLog(this.FailQuestLogText, false);
							TraitLevelingHelper.OnIssueSolvedThroughBetrayal(Hero.MainHero, new Tuple<TraitObject, int>[]
							{
								new Tuple<TraitObject, int>(DefaultTraits.Honor, -50)
							});
							ChangeRelationAction.ApplyPlayerRelation(this._targetNotable, 5, true, true);
						}
						else
						{
							base.AddLog(this.CulpritDiedQuestFail, false);
						}
						this.RelationshipChangeWithQuestGiver = -10;
						base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
						base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security -= 5f;
						base.CompleteQuestWithFail(null);
						return;
					}
					if (this._persuationInDoneAndSuccessfull)
					{
						base.AddLog(this.SuccessQuestSolutionLogText, false);
						this.ApplySuccessConsequences();
						return;
					}
					if (this._isNotableKnockedDownInMissionFight)
					{
						base.AddLog(this.SuccessQuestSolutionLogText, false);
						this.ApplySuccessConsequences();
					}
				}
			}

			// Token: 0x06001044 RID: 4164 RVA: 0x0006BB45 File Offset: 0x00069D45
			private void OnGameMenuOpened(MenuCallbackArgs args)
			{
				if (this._culpritJoinedPlayerParty && Hero.MainHero.CurrentSettlement == this._targetSettlement)
				{
					this._checkForMissionEvents = args.MenuContext.GameMenu.StringId == "village";
				}
			}

			// Token: 0x06001045 RID: 4165 RVA: 0x0006BB84 File Offset: 0x00069D84
			public void OnSettlementLeft(MobileParty party, Settlement settlement)
			{
				if (party == MobileParty.MainParty)
				{
					if (settlement == this._targetSettlement)
					{
						this._checkForMissionEvents = false;
					}
					if (settlement == base.QuestGiver.CurrentSettlement && this._culpritJoinedPlayerParty && !base.IsTracked(this._targetSettlement))
					{
						base.AddTrackedObject(this._targetSettlement);
					}
				}
			}

			// Token: 0x06001046 RID: 4166 RVA: 0x0006BBDC File Offset: 0x00069DDC
			public void OnBeforeMissionOpened()
			{
				if (this._checkForMissionEvents)
				{
					Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("village_center");
					if (locationWithId != null)
					{
						locationWithId.GetLocationCharacter(this._targetNotable).SpecialTargetTag = "alley_2";
						if (this._notableThugs == null)
						{
							this._notableThugs = new List<LocationCharacter>();
						}
						else
						{
							this._notableThugs.Clear();
						}
						locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateNotablesThugs), Settlement.CurrentSettlement.Culture, LocationCharacter.CharacterRelations.Neutral, MathF.Ceiling(Campaign.Current.Models.IssueModel.GetIssueDifficultyMultiplier() * 3f));
					}
				}
			}

			// Token: 0x06001047 RID: 4167 RVA: 0x0006BC80 File Offset: 0x00069E80
			private LocationCharacter CreateCulpritLocationCharacter(CultureObject culture, LocationCharacter.CharacterRelations relation)
			{
				Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(this._culprit.CharacterObject.Race, "_settlement");
				Tuple<string, Monster> tuple = new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, this._culprit.CharacterObject.IsFemale, "_villager"), monsterWithSuffix);
				return new LocationCharacter(new AgentData(new SimpleAgentOrigin(this._culprit.CharacterObject, -1, null, default(UniqueTroopDescriptor))).Monster(tuple.Item2), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFirstCompanionBehavior), "alley_2", true, relation, tuple.Item1, true, false, null, false, false, true, null, false);
			}

			// Token: 0x06001048 RID: 4168 RVA: 0x0006BD28 File Offset: 0x00069F28
			private LocationCharacter CreateNotablesThugs(CultureObject culture, LocationCharacter.CharacterRelations relation)
			{
				Monster monsterWithSuffix = TaleWorlds.Core.FaceGen.GetMonsterWithSuffix(this._notableGangsterCharacterObject.Race, "_settlement");
				Tuple<string, Monster> tuple = new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, this._notableGangsterCharacterObject.IsFemale, "_villain"), monsterWithSuffix);
				LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new SimpleAgentOrigin(this._notableGangsterCharacterObject, -1, null, default(UniqueTroopDescriptor))).Monster(tuple.Item2), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "alley_2", true, relation, tuple.Item1, true, false, null, false, false, true, null, false);
				this._notableThugs.Add(locationCharacter);
				return locationCharacter;
			}

			// Token: 0x06001049 RID: 4169 RVA: 0x0006BDCC File Offset: 0x00069FCC
			private void OnMapEventEnded(MapEvent mapEvent)
			{
				if (mapEvent.IsPlayerMapEvent && this._culpritJoinedPlayerParty && !MobileParty.MainParty.MemberRoster.GetTroopRoster().Exists((TroopRosterElement x) => x.Character == this._culprit.CharacterObject))
				{
					base.AddLog(this.FailQuestLogText, false);
					this.TiemoutFailConsequences();
					base.CompleteQuestWithFail(null);
				}
			}

			// Token: 0x0600104A RID: 4170 RVA: 0x0006BE26 File Offset: 0x0006A026
			private void CulpritDiedInNotableFightFail()
			{
				this._conversationAfterFightIsDone = true;
				this.HandleAgentBehaviorAfterQuestConversations();
			}

			// Token: 0x0600104B RID: 4171 RVA: 0x0006BE38 File Offset: 0x0006A038
			protected override void OnFinalize()
			{
				if (this._culprit.IsPlayerCompanion)
				{
					if (this._culprit.IsPrisoner)
					{
						EndCaptivityAction.ApplyByEscape(this._culprit, null, false);
					}
					RemoveCompanionAction.ApplyAfterQuest(Clan.PlayerClan, this._culprit);
				}
				if (this._culprit.IsAlive)
				{
					this._culprit.Clan = null;
					KillCharacterAction.ApplyByRemove(this._culprit, false, true);
				}
			}

			// Token: 0x0600104C RID: 4172 RVA: 0x0006BEA2 File Offset: 0x0006A0A2
			protected override void OnTimedOut()
			{
				base.AddLog(this.FailQuestLogText, false);
				this.TiemoutFailConsequences();
			}

			// Token: 0x0600104D RID: 4173 RVA: 0x0006BEB8 File Offset: 0x0006A0B8
			private void TiemoutFailConsequences()
			{
				TraitLevelingHelper.OnIssueSolvedThroughBetrayal(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -50)
				});
				this.RelationshipChangeWithQuestGiver = -10;
				base.QuestGiver.CurrentSettlement.Village.Bound.Town.Prosperity -= 5f;
				base.QuestGiver.CurrentSettlement.Village.Bound.Town.Security -= 5f;
			}

			// Token: 0x0600104E RID: 4174 RVA: 0x0006BF43 File Offset: 0x0006A143
			internal static void AutoGeneratedStaticCollectObjectsFamilyFeudIssueQuest(object o, List<object> collectedObjects)
			{
				((FamilyFeudIssueBehavior.FamilyFeudIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600104F RID: 4175 RVA: 0x0006BF51 File Offset: 0x0006A151
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetSettlement);
				collectedObjects.Add(this._targetNotable);
				collectedObjects.Add(this._culprit);
			}

			// Token: 0x06001050 RID: 4176 RVA: 0x0006BF7E File Offset: 0x0006A17E
			internal static object AutoGeneratedGetMemberValue_targetSettlement(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssueQuest)o)._targetSettlement;
			}

			// Token: 0x06001051 RID: 4177 RVA: 0x0006BF8B File Offset: 0x0006A18B
			internal static object AutoGeneratedGetMemberValue_targetNotable(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssueQuest)o)._targetNotable;
			}

			// Token: 0x06001052 RID: 4178 RVA: 0x0006BF98 File Offset: 0x0006A198
			internal static object AutoGeneratedGetMemberValue_culprit(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssueQuest)o)._culprit;
			}

			// Token: 0x06001053 RID: 4179 RVA: 0x0006BFA5 File Offset: 0x0006A1A5
			internal static object AutoGeneratedGetMemberValue_culpritJoinedPlayerParty(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssueQuest)o)._culpritJoinedPlayerParty;
			}

			// Token: 0x06001054 RID: 4180 RVA: 0x0006BFB7 File Offset: 0x0006A1B7
			internal static object AutoGeneratedGetMemberValue_checkForMissionEvents(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssueQuest)o)._checkForMissionEvents;
			}

			// Token: 0x06001055 RID: 4181 RVA: 0x0006BFC9 File Offset: 0x0006A1C9
			internal static object AutoGeneratedGetMemberValue_rewardGold(object o)
			{
				return ((FamilyFeudIssueBehavior.FamilyFeudIssueQuest)o)._rewardGold;
			}

			// Token: 0x04000829 RID: 2089
			private const int CustomCulpritAgentHealth = 350;

			// Token: 0x0400082A RID: 2090
			private const int CustomTargetNotableAgentHealth = 350;

			// Token: 0x0400082B RID: 2091
			public const string CommonAreaTag = "alley_2";

			// Token: 0x0400082C RID: 2092
			[SaveableField(10)]
			private readonly Settlement _targetSettlement;

			// Token: 0x0400082D RID: 2093
			[SaveableField(20)]
			private Hero _targetNotable;

			// Token: 0x0400082E RID: 2094
			[SaveableField(30)]
			private Hero _culprit;

			// Token: 0x0400082F RID: 2095
			[SaveableField(40)]
			private bool _culpritJoinedPlayerParty;

			// Token: 0x04000830 RID: 2096
			[SaveableField(50)]
			private bool _checkForMissionEvents;

			// Token: 0x04000831 RID: 2097
			[SaveableField(70)]
			private int _rewardGold;

			// Token: 0x04000832 RID: 2098
			private bool _isCulpritDiedInMissionFight;

			// Token: 0x04000833 RID: 2099
			private bool _isPlayerKnockedOutMissionFight;

			// Token: 0x04000834 RID: 2100
			private bool _isNotableKnockedDownInMissionFight;

			// Token: 0x04000835 RID: 2101
			private bool _conversationAfterFightIsDone;

			// Token: 0x04000836 RID: 2102
			private bool _persuationInDoneAndSuccessfull;

			// Token: 0x04000837 RID: 2103
			private bool _playerBetrayedCulprit;

			// Token: 0x04000838 RID: 2104
			private Agent _notableAgent;

			// Token: 0x04000839 RID: 2105
			private Agent _culpritAgent;

			// Token: 0x0400083A RID: 2106
			private CharacterObject _notableGangsterCharacterObject;

			// Token: 0x0400083B RID: 2107
			private List<LocationCharacter> _notableThugs;

			// Token: 0x0400083C RID: 2108
			private PersuasionTask _task;

			// Token: 0x0400083D RID: 2109
			private const PersuasionDifficulty Difficulty = PersuasionDifficulty.MediumHard;
		}
	}
}
