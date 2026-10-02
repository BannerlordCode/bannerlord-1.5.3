using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000391 RID: 913
	public class ScoutEnemyGarrisonsIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600360C RID: 13836 RVA: 0x000DD8D9 File Offset: 0x000DBAD9
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x000DD8F4 File Offset: 0x000DBAF4
		public void OnCheckForIssue(Hero hero)
		{
			List<Settlement> list;
			if (this.ConditionsHold(hero, out list))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), IssueBase.IssueFrequency.VeryCommon, list));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), IssueBase.IssueFrequency.VeryCommon));
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000DD95C File Offset: 0x000DBB5C
		private bool ConditionsHold(Hero issueGiver, out List<Settlement> settlements)
		{
			settlements = new List<Settlement>();
			if (issueGiver.MapFaction.IsKingdomFaction && issueGiver.IsFactionLeader && !issueGiver.IsMinorFactionHero && !issueGiver.IsPrisoner && !issueGiver.IsFugitive)
			{
				Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => x.IsAtWarWith(issueGiver.MapFaction));
				MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
				IMapPoint mapPoint = issueGiver.GetMapPoint();
				float num = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) * 5f;
				if (randomElementWithPredicate != null && mapPoint != null)
				{
					ValueTuple<Settlement, float>[] array = new ValueTuple<Settlement, float>[3];
					foreach (Settlement settlement in randomElementWithPredicate.Settlements)
					{
						if (ScoutEnemyGarrisonsIssueBehavior.SuitableSettlementCondition(settlement, issueGiver))
						{
							float num2 = float.MaxValue;
							if (issueGiver.CurrentSettlement != null)
							{
								num2 = mapDistanceModel.GetDistance(issueGiver.CurrentSettlement, settlement, false, false, MobileParty.NavigationType.All);
							}
							else if (issueGiver.PartyBelongedTo != null)
							{
								float num3;
								num2 = mapDistanceModel.GetDistance(issueGiver.PartyBelongedTo, settlement, false, MobileParty.NavigationType.All, out num3);
							}
							else if (issueGiver.PartyBelongedToAsPrisoner != null)
							{
								float num3;
								num2 = mapDistanceModel.GetDistance(issueGiver.PartyBelongedToAsPrisoner.MobileParty, settlement, false, MobileParty.NavigationType.All, out num3);
							}
							if (num2 <= num)
							{
								if (array[2].Item1 == null || array[2].Item2 > num2)
								{
									array[2] = new ValueTuple<Settlement, float>(settlement, num2);
								}
								int num4 = array.Length - 1;
								while (num4 > 0 && (array[num4 - 1].Item1 == null || array[num4].Item2 < array[num4 - 1].Item2))
								{
									ValueTuple<Settlement, float> valueTuple = array[num4 - 1];
									array[num4 - 1] = array[num4];
									array[num4] = valueTuple;
									num4--;
								}
							}
						}
					}
					if (array[2].Item1 != null)
					{
						settlements.Add(array[2].Item1);
						settlements.Add(array[1].Item1);
						settlements.Add(array[0].Item1);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000DDC10 File Offset: 0x000DBE10
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue(issueOwner, potentialIssueData.RelatedObject as List<Settlement>);
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000DDC36 File Offset: 0x000DBE36
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000DDC38 File Offset: 0x000DBE38
		private static bool SuitableSettlementCondition(Settlement settlement, Hero issueGiver)
		{
			return settlement.IsFortification && settlement.MapFaction.IsAtWarWith(issueGiver.MapFaction) && (!settlement.IsUnderSiege || settlement.SiegeEvent.BesiegerCamp.MapFaction != Hero.MainHero.MapFaction);
		}

		// Token: 0x04000F29 RID: 3881
		private const IssueBase.IssueFrequency ScoutEnemyGarrisonsIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x04000F2A RID: 3882
		private const int QuestDurationInDays = 30;

		// Token: 0x02000787 RID: 1927
		public class ScoutEnemyGarrisonsIssue : IssueBase
		{
			// Token: 0x06006287 RID: 25223 RVA: 0x001C8B67 File Offset: 0x001C6D67
			internal static void AutoGeneratedStaticCollectObjectsScoutEnemyGarrisonsIssue(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006288 RID: 25224 RVA: 0x001C8B75 File Offset: 0x001C6D75
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._settlement1);
				collectedObjects.Add(this._settlement2);
				collectedObjects.Add(this._settlement3);
			}

			// Token: 0x06006289 RID: 25225 RVA: 0x001C8BA2 File Offset: 0x001C6DA2
			internal static object AutoGeneratedGetMemberValue_settlement1(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement1;
			}

			// Token: 0x0600628A RID: 25226 RVA: 0x001C8BAF File Offset: 0x001C6DAF
			internal static object AutoGeneratedGetMemberValue_settlement2(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement2;
			}

			// Token: 0x0600628B RID: 25227 RVA: 0x001C8BBC File Offset: 0x001C6DBC
			internal static object AutoGeneratedGetMemberValue_settlement3(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue)o)._settlement3;
			}

			// Token: 0x1700138D RID: 5005
			// (get) Token: 0x0600628C RID: 25228 RVA: 0x001C8BC9 File Offset: 0x001C6DC9
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700138E RID: 5006
			// (get) Token: 0x0600628D RID: 25229 RVA: 0x001C8BCC File Offset: 0x001C6DCC
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700138F RID: 5007
			// (get) Token: 0x0600628E RID: 25230 RVA: 0x001C8BCF File Offset: 0x001C6DCF
			protected override int RewardGold
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17001390 RID: 5008
			// (get) Token: 0x0600628F RID: 25231 RVA: 0x001C8BD2 File Offset: 0x001C6DD2
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=rrCkJgtd}We don't know enough about the enemy, [ib:closed][if:convo_thinking]where they are strong and where they are weak. I don't want to lead a huge army through their territory on a wild goose hunt. We need someone to ride through there swiftly, scouting out their garrisons. Can you do this?", null);
				}
			}

			// Token: 0x17001391 RID: 5009
			// (get) Token: 0x06006290 RID: 25232 RVA: 0x001C8BE0 File Offset: 0x001C6DE0
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=dGakGflE}Yes, your {?QUEST_GIVER.GENDER}ladyship{?}lordship{\\?}, I'll gladly do it.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001392 RID: 5010
			// (get) Token: 0x06006291 RID: 25233 RVA: 0x001C8C14 File Offset: 0x001C6E14
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=seEyGLMz}Go deep into {ENEMY} territory, to {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}. [ib:hip][if:convo_normal]I want to know every detail about them, what sort of fortifications they have, whether the walls are well-manned or undergarrisoned, and any other enemy forces in the vicinity.", null);
					textObject.SetTextVariable("ENEMY", this._settlement1.MapFaction.Name);
					textObject.SetTextVariable("SETTLEMENT_1", this._settlement1.Name);
					textObject.SetTextVariable("SETTLEMENT_2", this._settlement2.Name);
					textObject.SetTextVariable("SETTLEMENT_3", this._settlement3.Name);
					return textObject;
				}
			}

			// Token: 0x17001393 RID: 5011
			// (get) Token: 0x06006292 RID: 25234 RVA: 0x001C8C8D File Offset: 0x001C6E8D
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=g6P6nKIf}Consider it done, commander.", null);
				}
			}

			// Token: 0x17001394 RID: 5012
			// (get) Token: 0x06006293 RID: 25235 RVA: 0x001C8C9A File Offset: 0x001C6E9A
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=G79IzJsZ}Scout Enemy Garrisons", null);
				}
			}

			// Token: 0x17001395 RID: 5013
			// (get) Token: 0x06006294 RID: 25236 RVA: 0x001C8CA8 File Offset: 0x001C6EA8
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=AdoaDR26}{QUEST_GIVER.LINK} asks you to scout {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT_1", this._settlement1.Name);
					textObject.SetTextVariable("SETTLEMENT_2", this._settlement2.Name);
					textObject.SetTextVariable("SETTLEMENT_3", this._settlement3.Name);
					return textObject;
				}
			}

			// Token: 0x06006295 RID: 25237 RVA: 0x001C8D1F File Offset: 0x001C6F1F
			public ScoutEnemyGarrisonsIssue(Hero issueOwner, List<Settlement> settlements)
				: base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._settlement1 = settlements[0];
				this._settlement2 = settlements[1];
				this._settlement3 = settlements[2];
			}

			// Token: 0x06006296 RID: 25238 RVA: 0x001C8D59 File Offset: 0x001C6F59
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06006297 RID: 25239 RVA: 0x001C8D5B File Offset: 0x001C6F5B
			protected override void HourlyTick()
			{
			}

			// Token: 0x06006298 RID: 25240 RVA: 0x001C8D5D File Offset: 0x001C6F5D
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest(questId, base.IssueOwner, this._settlement1, this._settlement2, this._settlement3);
			}

			// Token: 0x06006299 RID: 25241 RVA: 0x001C8D7D File Offset: 0x001C6F7D
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x0600629A RID: 25242 RVA: 0x001C8D80 File Offset: 0x001C6F80
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill, out int requiredGold)
			{
				relationHero = null;
				skill = null;
				requiredGold = 0;
				flag = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (Hero.MainHero.IsKingdomLeader)
				{
					flag |= IssueBase.PreconditionFlags.MainHeroIsKingdomLeader;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				if (Clan.PlayerClan.Tier < 2)
				{
					flag |= IssueBase.PreconditionFlags.ClanTier;
				}
				if (Hero.MainHero.GetSkillValue(DefaultSkills.Scouting) < 30)
				{
					flag |= IssueBase.PreconditionFlags.Skill;
					skill = DefaultSkills.Scouting;
				}
				if (Hero.MainHero.MapFaction != base.IssueOwner.MapFaction)
				{
					flag |= IssueBase.PreconditionFlags.NotInSameFaction;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x0600629B RID: 25243 RVA: 0x001C8E44 File Offset: 0x001C7044
			public override bool IssueStayAliveConditions()
			{
				bool flag = this._settlement1.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction) && this._settlement2.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction) && this._settlement3.MapFaction.IsAtWarWith(base.IssueOwner.MapFaction);
				if (!flag)
				{
					flag = this.TryToUpdateSettlements();
				}
				return flag && base.IssueOwner.MapFaction.IsKingdomFaction;
			}

			// Token: 0x0600629C RID: 25244 RVA: 0x001C8EC9 File Offset: 0x001C70C9
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.ClanInfluence)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x0600629D RID: 25245 RVA: 0x001C8EE0 File Offset: 0x001C70E0
			private bool TryToUpdateSettlements()
			{
				Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => x.IsAtWarWith(base.IssueOwner.MapFaction));
				MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
				IMapPoint mapPoint = base.IssueOwner.GetMapPoint();
				float num = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) * 5f;
				if (randomElementWithPredicate != null && mapPoint != null)
				{
					ValueTuple<Settlement, float>[] array = new ValueTuple<Settlement, float>[3];
					foreach (Settlement settlement in randomElementWithPredicate.Settlements)
					{
						if (ScoutEnemyGarrisonsIssueBehavior.SuitableSettlementCondition(settlement, base.IssueOwner))
						{
							float num2 = float.MaxValue;
							if (base.IssueOwner.CurrentSettlement != null)
							{
								num2 = mapDistanceModel.GetDistance(base.IssueOwner.CurrentSettlement, settlement, false, false, MobileParty.NavigationType.All);
							}
							else if (base.IssueOwner.PartyBelongedTo != null)
							{
								float num3;
								num2 = mapDistanceModel.GetDistance(base.IssueOwner.PartyBelongedTo, settlement, false, MobileParty.NavigationType.All, out num3);
							}
							else if (base.IssueOwner.PartyBelongedToAsPrisoner != null)
							{
								float num3;
								num2 = mapDistanceModel.GetDistance(base.IssueOwner.PartyBelongedToAsPrisoner.MobileParty, settlement, false, MobileParty.NavigationType.All, out num3);
							}
							if (num2 <= num)
							{
								if (array[2].Item1 == null || array[2].Item2 > num2)
								{
									array[2] = new ValueTuple<Settlement, float>(settlement, num2);
								}
								int num4 = array.Length - 1;
								while (num4 > 0 && (array[num4 - 1].Item1 == null || array[num4].Item2 < array[num4 - 1].Item2))
								{
									ValueTuple<Settlement, float> valueTuple = array[num4 - 1];
									array[num4 - 1] = array[num4];
									array[num4] = valueTuple;
									num4--;
								}
							}
						}
					}
					if (array[2].Item1 != null)
					{
						this._settlement1 = array[2].Item1;
						this._settlement2 = array[1].Item1;
						this._settlement3 = array[0].Item1;
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600629E RID: 25246 RVA: 0x001C9124 File Offset: 0x001C7324
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001F6B RID: 8043
			private const int MinimumRelationToTakeQuest = -10;

			// Token: 0x04001F6C RID: 8044
			[SaveableField(10)]
			private Settlement _settlement1;

			// Token: 0x04001F6D RID: 8045
			[SaveableField(20)]
			private Settlement _settlement2;

			// Token: 0x04001F6E RID: 8046
			[SaveableField(30)]
			private Settlement _settlement3;
		}

		// Token: 0x02000788 RID: 1928
		public class ScoutEnemyGarrisonsQuest : QuestBase
		{
			// Token: 0x060062A0 RID: 25248 RVA: 0x001C9139 File Offset: 0x001C7339
			internal static void AutoGeneratedStaticCollectObjectsScoutEnemyGarrisonsQuest(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060062A1 RID: 25249 RVA: 0x001C9147 File Offset: 0x001C7347
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._questSettlement1);
				collectedObjects.Add(this._questSettlement2);
				collectedObjects.Add(this._questSettlement3);
				collectedObjects.Add(this._startQuestLog);
			}

			// Token: 0x060062A2 RID: 25250 RVA: 0x001C9180 File Offset: 0x001C7380
			internal static object AutoGeneratedGetMemberValue_questSettlement1(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement1;
			}

			// Token: 0x060062A3 RID: 25251 RVA: 0x001C918D File Offset: 0x001C738D
			internal static object AutoGeneratedGetMemberValue_questSettlement2(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement2;
			}

			// Token: 0x060062A4 RID: 25252 RVA: 0x001C919A File Offset: 0x001C739A
			internal static object AutoGeneratedGetMemberValue_questSettlement3(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._questSettlement3;
			}

			// Token: 0x060062A5 RID: 25253 RVA: 0x001C91A7 File Offset: 0x001C73A7
			internal static object AutoGeneratedGetMemberValue_scoutedSettlementCount(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._scoutedSettlementCount;
			}

			// Token: 0x060062A6 RID: 25254 RVA: 0x001C91B9 File Offset: 0x001C73B9
			internal static object AutoGeneratedGetMemberValue_startQuestLog(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest)o)._startQuestLog;
			}

			// Token: 0x17001396 RID: 5014
			// (get) Token: 0x060062A7 RID: 25255 RVA: 0x001C91C6 File Offset: 0x001C73C6
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001397 RID: 5015
			// (get) Token: 0x060062A8 RID: 25256 RVA: 0x001C91C9 File Offset: 0x001C73C9
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=G79IzJsZ}Scout Enemy Garrisons", null);
				}
			}

			// Token: 0x17001398 RID: 5016
			// (get) Token: 0x060062A9 RID: 25257 RVA: 0x001C91D8 File Offset: 0x001C73D8
			private TextObject PlayerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=8avwit9N}{QUEST_GIVER.LINK}, the army commander of {FACTION} has told you that they need detailed information about enemy fortifications and troop numbers of the enemy. {?QUEST_GIVER.GENDER}She{?}He{\\?} wanted you to scout {SETTLEMENT_1}, {SETTLEMENT_2} and {SETTLEMENT_3}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("FACTION", base.QuestGiver.MapFaction.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_1", this._questSettlement1.Settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_2", this._questSettlement2.Settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("SETTLEMENT_3", this._questSettlement3.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17001399 RID: 5017
			// (get) Token: 0x060062AA RID: 25258 RVA: 0x001C927A File Offset: 0x001C747A
			private TextObject SettlementBecomeNeutralLogText
			{
				get
				{
					return new TextObject("{=wgX2nL5Z}{SETTLEMENT} is no longer in control of enemy. There is no need to scout that settlement.", null);
				}
			}

			// Token: 0x1700139A RID: 5018
			// (get) Token: 0x060062AB RID: 25259 RVA: 0x001C9287 File Offset: 0x001C7487
			private TextObject ArmyDisbandedQuestCancelLogText
			{
				get
				{
					return new TextObject("{=JiHaL6IV}Army has disbanded and your mission has been canceled.", null);
				}
			}

			// Token: 0x1700139B RID: 5019
			// (get) Token: 0x060062AC RID: 25260 RVA: 0x001C9294 File Offset: 0x001C7494
			private TextObject NoLongerAllyQuestCancelLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=vTnSa9rr}You are no longer allied with {QUEST_GIVER.LINK}'s faction. Your agreement with {QUEST_GIVER.LINK} was terminated.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700139C RID: 5020
			// (get) Token: 0x060062AD RID: 25261 RVA: 0x001C92C6 File Offset: 0x001C74C6
			private TextObject AllTargetsAreNeutral
			{
				get
				{
					return new TextObject("{=LC2F84GR}None of the target settlements are in control of the enemy. Army Commander has canceled the mission.", null);
				}
			}

			// Token: 0x1700139D RID: 5021
			// (get) Token: 0x060062AE RID: 25262 RVA: 0x001C92D3 File Offset: 0x001C74D3
			private TextObject ScoutFinishedForSettlementWallLevel1LogText
			{
				get
				{
					return new TextObject("{=5kxDhBWk}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are not high enough but can be useful with sufficient garrison support.", null);
				}
			}

			// Token: 0x1700139E RID: 5022
			// (get) Token: 0x060062AF RID: 25263 RVA: 0x001C92E0 File Offset: 0x001C74E0
			private TextObject ScoutFinishedForSettlementWallLevel2LogText
			{
				get
				{
					return new TextObject("{=GUqjL6xk}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are high enough to defend against invaders.", null);
				}
			}

			// Token: 0x1700139F RID: 5023
			// (get) Token: 0x060062B0 RID: 25264 RVA: 0x001C92ED File Offset: 0x001C74ED
			private TextObject ScoutFinishedForSettlementWallLevel3LogText
			{
				get
				{
					return new TextObject("{=YErURO5l}Your scouts have returned from {SETTLEMENT}. According to their report {SETTLEMENT}'s garrison has {GARRISON_SIZE} men and walls are too high and hard to breach.", null);
				}
			}

			// Token: 0x170013A0 RID: 5024
			// (get) Token: 0x060062B1 RID: 25265 RVA: 0x001C92FC File Offset: 0x001C74FC
			private TextObject QuestSuccess
			{
				get
				{
					TextObject textObject = new TextObject("{=Qy7Zmmvk}You have successfully scouted the target settlements and sent the report to {QUEST_GIVER.LINK}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170013A1 RID: 5025
			// (get) Token: 0x060062B2 RID: 25266 RVA: 0x001C932E File Offset: 0x001C752E
			private TextObject QuestTimedOut
			{
				get
				{
					return new TextObject("{=GzodT3vS}You have failed to scout the enemy settlements in time.", null);
				}
			}

			// Token: 0x060062B3 RID: 25267 RVA: 0x001C933C File Offset: 0x001C753C
			public ScoutEnemyGarrisonsQuest(string questId, Hero questGiver, Settlement settlement1, Settlement settlement2, Settlement settlement3)
				: base(questId, questGiver, CampaignTime.DaysFromNow(30f), 0)
			{
				this._questSettlement1 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement1, 0);
				this._questSettlement2 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement2, 0);
				this._questSettlement3 = new ScoutEnemyGarrisonsIssueBehavior.QuestSettlement(settlement3, 0);
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x060062B4 RID: 25268 RVA: 0x001C9391 File Offset: 0x001C7591
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x060062B5 RID: 25269 RVA: 0x001C939C File Offset: 0x001C759C
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=lyGvyZK4}Very well. When you reach one of their fortresses, spend some time observing. Don't move on to the next one at once. You don't need to find me to report back the details, just send your messengers.", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences))
					.CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=x3TO0gkN}Is there any progress on the task I gave you?[ib:closed][if:convo_normal]", null), null, null, null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver)
					.Consequence(delegate
					{
						Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
					})
					.BeginPlayerOptions(null, false)
					.PlayerOption(new TextObject("{=W5ab31gQ}Soon, commander. We are still working on it.", null), null, null, null)
					.NpcLine(new TextObject("{=U3LR7dyK}Good. I'll be waiting for your messengers.[if:convo_thinking]", null), null, null, null, null)
					.CloseDialog()
					.PlayerOption(new TextObject("{=v75k1FoT}Not yet. We need to make more preparations.", null), null, null, null)
					.NpcLine(new TextObject("{=zYKeYZAo}All right. Don't rush this but also don't wait too long.", null), null, null, null, null)
					.CloseDialog()
					.EndPlayerOptions()
					.CloseDialog();
			}

			// Token: 0x060062B6 RID: 25270 RVA: 0x001C94BC File Offset: 0x001C76BC
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddTrackedObject(this._questSettlement1.Settlement);
				base.AddTrackedObject(this._questSettlement2.Settlement);
				base.AddTrackedObject(this._questSettlement3.Settlement);
				this._scoutedSettlementCount = 0;
				this._startQuestLog = base.AddDiscreteLog(this.PlayerStartsQuestLogText, new TextObject("{=jpBpwgAs}Settlements", null), this._scoutedSettlementCount, 3, null, false);
			}

			// Token: 0x060062B7 RID: 25271 RVA: 0x001C9530 File Offset: 0x001C7730
			protected override void RegisterEvents()
			{
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			}

			// Token: 0x060062B8 RID: 25272 RVA: 0x001C9584 File Offset: 0x001C7784
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> list = new List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> { this._questSettlement1, this._questSettlement2, this._questSettlement3 };
					if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => !x.Settlement.MapFaction.IsAtWarWith(base.QuestGiver.MapFaction)))
					{
						base.AddLog(this.AllTargetsAreNeutral, false);
						base.CompleteQuestWithCancel(null);
						return;
					}
					foreach (ScoutEnemyGarrisonsIssueBehavior.QuestSettlement questSettlement in list)
					{
						if (!questSettlement.IsScoutingCompleted())
						{
							if (DistanceHelper.FindClosestDistanceFromMobilePartyToSettlement(MobileParty.MainParty, questSettlement.Settlement, MobileParty.NavigationType.Default) <= MobileParty.MainParty.SeeingRange)
							{
								questSettlement.CurrentScoutProgress++;
								if (questSettlement.CurrentScoutProgress == 1)
								{
									TextObject textObject = new TextObject("{=qfjRGjM4}Your scouts started to gather information about {SETTLEMENT}.", null);
									textObject.SetTextVariable("SETTLEMENT", questSettlement.Settlement.Name);
									MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
								}
								else if (questSettlement.IsScoutingCompleted())
								{
									JournalLog startQuestLog = this._startQuestLog;
									int num = this._scoutedSettlementCount + 1;
									this._scoutedSettlementCount = num;
									startQuestLog.UpdateCurrentProgress(num);
									base.RemoveTrackedObject(questSettlement.Settlement);
									TextObject textObject2;
									if (questSettlement.Settlement.Town.GetWallLevel() == 1)
									{
										textObject2 = this.ScoutFinishedForSettlementWallLevel1LogText;
									}
									else if (questSettlement.Settlement.Town.GetWallLevel() == 2)
									{
										textObject2 = this.ScoutFinishedForSettlementWallLevel2LogText;
									}
									else
									{
										textObject2 = this.ScoutFinishedForSettlementWallLevel3LogText;
									}
									textObject2.SetTextVariable("SETTLEMENT", questSettlement.Settlement.EncyclopediaLinkWithName);
									MobileParty garrisonParty = questSettlement.Settlement.Town.GarrisonParty;
									int num2 = ((garrisonParty != null) ? garrisonParty.MemberRoster.TotalHealthyCount : 0);
									int num3 = (int)questSettlement.Settlement.Militia;
									textObject2.SetTextVariable("GARRISON_SIZE", num2 + num3);
									base.AddLog(textObject2, false);
								}
							}
							else
							{
								questSettlement.ResetCurrentProgress();
							}
						}
					}
					if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => x.IsScoutingCompleted()))
					{
						this.AllScoutingDone();
					}
				}
			}

			// Token: 0x060062B9 RID: 25273 RVA: 0x001C97C0 File Offset: 0x001C79C0
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> list = new List<ScoutEnemyGarrisonsIssueBehavior.QuestSettlement> { this._questSettlement1, this._questSettlement2, this._questSettlement3 };
				foreach (ScoutEnemyGarrisonsIssueBehavior.QuestSettlement questSettlement in list)
				{
					if (settlement == questSettlement.Settlement && !questSettlement.IsScoutingCompleted() && (newOwner.MapFaction == base.QuestGiver.MapFaction || !newOwner.MapFaction.IsAtWarWith(base.QuestGiver.MapFaction)))
					{
						questSettlement.IsCompletedThroughBeingNeutral = true;
						questSettlement.SetScoutingCompleted();
						JournalLog startQuestLog = this._startQuestLog;
						int num = this._scoutedSettlementCount + 1;
						this._scoutedSettlementCount = num;
						startQuestLog.UpdateCurrentProgress(num);
						if (base.IsTracked(questSettlement.Settlement))
						{
							base.RemoveTrackedObject(questSettlement.Settlement);
						}
						TextObject settlementBecomeNeutralLogText = this.SettlementBecomeNeutralLogText;
						settlementBecomeNeutralLogText.SetTextVariable("SETTLEMENT", questSettlement.Settlement.EncyclopediaLinkWithName);
						base.AddLog(settlementBecomeNeutralLogText, false);
						if (list.TrueForAll((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement x) => x.IsCompletedThroughBeingNeutral))
						{
							base.AddLog(this.AllTargetsAreNeutral, false);
							base.CompleteQuestWithCancel(null);
							break;
						}
						break;
					}
				}
			}

			// Token: 0x060062BA RID: 25274 RVA: 0x001C9934 File Offset: 0x001C7B34
			private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
			{
				if (army.ArmyOwner == base.QuestGiver)
				{
					base.AddLog(this.ArmyDisbandedQuestCancelLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x060062BB RID: 25275 RVA: 0x001C9959 File Offset: 0x001C7B59
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (clan == Clan.PlayerClan && oldKingdom == base.QuestGiver.MapFaction)
				{
					base.AddLog(this.NoLongerAllyQuestCancelLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x060062BC RID: 25276 RVA: 0x001C9986 File Offset: 0x001C7B86
			private void AllScoutingDone()
			{
				base.AddLog(this.QuestSuccess, false);
				GainRenownAction.Apply(Hero.MainHero, 3f, false);
				GainKingdomInfluenceAction.ApplyForDefault(Hero.MainHero, 10f);
				this.RelationshipChangeWithQuestGiver = 3;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x060062BD RID: 25277 RVA: 0x001C99C2 File Offset: 0x001C7BC2
			protected override void OnTimedOut()
			{
				base.AddLog(this.QuestTimedOut, false);
				this.RelationshipChangeWithQuestGiver = -2;
			}

			// Token: 0x04001F6F RID: 8047
			[SaveableField(10)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement1;

			// Token: 0x04001F70 RID: 8048
			[SaveableField(20)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement2;

			// Token: 0x04001F71 RID: 8049
			[SaveableField(30)]
			private ScoutEnemyGarrisonsIssueBehavior.QuestSettlement _questSettlement3;

			// Token: 0x04001F72 RID: 8050
			[SaveableField(40)]
			private int _scoutedSettlementCount;

			// Token: 0x04001F73 RID: 8051
			[SaveableField(50)]
			private JournalLog _startQuestLog;
		}

		// Token: 0x02000789 RID: 1929
		public class QuestSettlement
		{
			// Token: 0x060062C1 RID: 25281 RVA: 0x001C9A18 File Offset: 0x001C7C18
			internal static void AutoGeneratedStaticCollectObjectsQuestSettlement(object o, List<object> collectedObjects)
			{
				((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060062C2 RID: 25282 RVA: 0x001C9A26 File Offset: 0x001C7C26
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Settlement);
			}

			// Token: 0x060062C3 RID: 25283 RVA: 0x001C9A34 File Offset: 0x001C7C34
			internal static object AutoGeneratedGetMemberValueSettlement(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).Settlement;
			}

			// Token: 0x060062C4 RID: 25284 RVA: 0x001C9A41 File Offset: 0x001C7C41
			internal static object AutoGeneratedGetMemberValueCurrentScoutProgress(object o)
			{
				return ((ScoutEnemyGarrisonsIssueBehavior.QuestSettlement)o).CurrentScoutProgress;
			}

			// Token: 0x060062C5 RID: 25285 RVA: 0x001C9A53 File Offset: 0x001C7C53
			public QuestSettlement(Settlement settlement, int currentScoutProgress)
			{
				this.Settlement = settlement;
				this.CurrentScoutProgress = currentScoutProgress;
				this.IsCompletedThroughBeingNeutral = false;
			}

			// Token: 0x060062C6 RID: 25286 RVA: 0x001C9A70 File Offset: 0x001C7C70
			public bool IsScoutingCompleted()
			{
				return this.CurrentScoutProgress >= 8;
			}

			// Token: 0x060062C7 RID: 25287 RVA: 0x001C9A7E File Offset: 0x001C7C7E
			public void SetScoutingCompleted()
			{
				this.CurrentScoutProgress = 8;
			}

			// Token: 0x060062C8 RID: 25288 RVA: 0x001C9A87 File Offset: 0x001C7C87
			public void ResetCurrentProgress()
			{
				this.CurrentScoutProgress = 0;
			}

			// Token: 0x04001F74 RID: 8052
			private const int CompleteScoutAfterHours = 8;

			// Token: 0x04001F75 RID: 8053
			[SaveableField(10)]
			public Settlement Settlement;

			// Token: 0x04001F76 RID: 8054
			[SaveableField(20)]
			public int CurrentScoutProgress;

			// Token: 0x04001F77 RID: 8055
			public bool IsCompletedThroughBeingNeutral;
		}

		// Token: 0x0200078A RID: 1930
		public class ScoutEnemyGarrisonsIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060062C9 RID: 25289 RVA: 0x001C9A90 File Offset: 0x001C7C90
			public ScoutEnemyGarrisonsIssueTypeDefiner()
				: base(97600)
			{
			}

			// Token: 0x060062CA RID: 25290 RVA: 0x001C9A9D File Offset: 0x001C7C9D
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsIssue), 1, null);
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.ScoutEnemyGarrisonsQuest), 2, null);
				base.AddClassDefinition(typeof(ScoutEnemyGarrisonsIssueBehavior.QuestSettlement), 3, null);
			}
		}
	}
}
