using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F6 RID: 246
	public class DefaultAllianceModel : AllianceModel
	{
		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x00067D3A File Offset: 0x00065F3A
		private ITradeAgreementsCampaignBehavior TradeAgreementsCampaignBehavior
		{
			get
			{
				if (this._tradeAgreementsBehavior == null)
				{
					this._tradeAgreementsBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
				}
				return this._tradeAgreementsBehavior;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x060016A0 RID: 5792 RVA: 0x00067D5A File Offset: 0x00065F5A
		public override CampaignTime MaxDurationOfAlliance
		{
			get
			{
				return CampaignTime.Days(84f);
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060016A1 RID: 5793 RVA: 0x00067D66 File Offset: 0x00065F66
		public override CampaignTime MaxDurationOfWarParticipation
		{
			get
			{
				return CampaignTime.Days(42f);
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00067D72 File Offset: 0x00065F72
		public override int MaxNumberOfAlliances
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060016A3 RID: 5795 RVA: 0x00067D75 File Offset: 0x00065F75
		public override CampaignTime DurationForOffers
		{
			get
			{
				return CampaignTime.Hours(24f);
			}
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00067D84 File Offset: 0x00065F84
		public override int GetCallToWarCost(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			int callToWarCostForCalledKingdom = this.GetCallToWarCostForCalledKingdom(calledKingdom, kingdomToCallToWarAgainst);
			int callToWarBudgetOfCallingKingdom = this.GetCallToWarBudgetOfCallingKingdom(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
			if (callingKingdom == Clan.PlayerClan.Kingdom && callToWarBudgetOfCallingKingdom < 0)
			{
				return callToWarCostForCalledKingdom;
			}
			return MathF.Min((int)((double)callToWarCostForCalledKingdom * 1.5), (callToWarCostForCalledKingdom + callToWarBudgetOfCallingKingdom) / 2);
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00067DD0 File Offset: 0x00065FD0
		public override ExplainedNumber GetScoreOfStartingAlliance(Kingdom querierKingdom, Kingdom queriedKingdom, out TextObject explanationText, bool includeDescription = false)
		{
			explanationText = this._allianceNotFormedExplanationText;
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, includeDescription, null);
			List<ValueTuple<float, TextObject>> list = new List<ValueTuple<float, TextObject>>();
			if (this.AreKingdomsNeighbors(querierKingdom, queriedKingdom))
			{
				float num;
				float num2;
				ValueTuple<Kingdom, float> threateningNeighbor = this.GetThreateningNeighbor(querierKingdom, out num, out num2);
				Kingdom item = threateningNeighbor.Item1;
				float item2 = threateningNeighbor.Item2;
				if (item != null && item2 > 430f && item != queriedKingdom)
				{
					ValueTuple<Kingdom, float> threateningNeighbor2 = this.GetThreateningNeighbor(queriedKingdom, out num2, out num);
					Kingdom item3 = threateningNeighbor2.Item1;
					float item4 = threateningNeighbor2.Item2;
					if (item4 > 430f)
					{
						float alliancePenalty = this.GetAlliancePenalty(querierKingdom);
						TextObject alliancePenaltyText = this.GetAlliancePenaltyText(querierKingdom, includeDescription);
						explainedNumber.Add(alliancePenalty, alliancePenaltyText, null);
						float alliancePenalty2 = this.GetAlliancePenalty(queriedKingdom);
						TextObject alliancePenaltyText2 = this.GetAlliancePenaltyText(queriedKingdom, includeDescription);
						explainedNumber.Add(alliancePenalty2, alliancePenaltyText2, null);
						float threatEffect = this.GetThreatEffect(item4, item2);
						explainedNumber.Add(threatEffect, this._threatEffect, null);
						float relationshipEffect = this.GetRelationshipEffect(querierKingdom, queriedKingdom);
						explainedNumber.Add(relationshipEffect, this._relationshipText, null);
						float marriageEffect = this.GetMarriageEffect(querierKingdom, queriedKingdom);
						explainedNumber.Add(marriageEffect, this._marriageEffect, null);
						float atWarWithAllyEffect = this.GetAtWarWithAllyEffect(querierKingdom, queriedKingdom);
						explainedNumber.Add(atWarWithAllyEffect, this._atWarWithAllyText, null);
						float atWarWithEnemyEffect = this.GetAtWarWithEnemyEffect(querierKingdom, queriedKingdom);
						explainedNumber.Add(atWarWithEnemyEffect, this._atWarWithEnemyEffect, null);
						float atWarOrPeaceEffect = this.GetAtWarOrPeaceEffect(queriedKingdom);
						explainedNumber.Add(atWarOrPeaceEffect, this._atWarText, null);
						float fiefWithSameCultureEffect = this.GetFiefWithSameCultureEffect(querierKingdom, queriedKingdom);
						explainedNumber.Add(fiefWithSameCultureEffect, this._sameCultureFiefsText, null);
						float honorableKingEffect = this.GetHonorableKingEffect(querierKingdom, queriedKingdom);
						if (includeDescription)
						{
							this._lowHonorText.SetCharacterProperties("RULER", querierKingdom.Leader.CharacterObject, false);
						}
						explainedNumber.Add(honorableKingEffect, this._lowHonorText, null);
						float tradeAgreementEffect = this.GetTradeAgreementEffect(querierKingdom, queriedKingdom);
						explainedNumber.Add(tradeAgreementEffect, this._tradeAgreementEffect, null);
						float commonThreatEffect = this.GetCommonThreatEffect(item, item3);
						explainedNumber.Add(commonThreatEffect, this._commonThreatEffect, null);
						if (includeDescription)
						{
							if (alliancePenalty < 0f)
							{
								list.Add(new ValueTuple<float, TextObject>(alliancePenalty, alliancePenaltyText));
							}
							if (alliancePenalty2 < 0f)
							{
								list.Add(new ValueTuple<float, TextObject>(alliancePenalty2, alliancePenaltyText2));
							}
							if (relationshipEffect < 0f)
							{
								list.Add(new ValueTuple<float, TextObject>(relationshipEffect, this._relationshipText));
							}
							if (atWarWithAllyEffect < 0f)
							{
								list.Add(new ValueTuple<float, TextObject>(atWarWithAllyEffect, this._atWarWithAllyText));
							}
							if (atWarOrPeaceEffect < 0f)
							{
								list.Add(new ValueTuple<float, TextObject>(atWarOrPeaceEffect, this._atWarText));
							}
							if (atWarWithAllyEffect < 0f)
							{
								list.Add(new ValueTuple<float, TextObject>(fiefWithSameCultureEffect, this._sameCultureFiefsText));
							}
							if (honorableKingEffect < 0f)
							{
								list.Add(new ValueTuple<float, TextObject>(honorableKingEffect, this._lowHonorText));
							}
							if ((float)list.Count > 0f)
							{
								explanationText = this.BuildExplanationForAlliance(querierKingdom, list);
							}
							else
							{
								this._allianceScoreNotEnoughText.SetTextVariable("KINGDOM_NAME", querierKingdom.Name);
								explanationText.SetTextVariable("REASON", this._allianceScoreNotEnoughText);
							}
						}
					}
					else if (includeDescription)
					{
						this._allianceScoreNotEnoughText.SetTextVariable("KINGDOM_NAME", querierKingdom.Name);
						explanationText.SetTextVariable("REASON", this._allianceScoreNotEnoughText);
					}
				}
				else if (includeDescription)
				{
					this._kingdomsNotSeekingAllianceText.SetTextVariable("KINGDOM_NAME", querierKingdom.Name);
					explanationText.SetTextVariable("REASON", this._kingdomsNotSeekingAllianceText);
				}
			}
			else if (includeDescription)
			{
				explanationText.SetTextVariable("REASON", this._kingdomsNotNeighborsText);
			}
			return explainedNumber;
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00068148 File Offset: 0x00066348
		public override float GetSupportScoreOfStartingAllianceForClan(Kingdom querierKingdom, Kingdom queriedKingdom, Clan evaluatingClan, out TextObject explanationText, bool includeDescriptions = false)
		{
			explanationText = (includeDescriptions ? TextObject.GetEmpty() : null);
			int influenceCostOfProposingStartingAlliance = Campaign.Current.Models.AllianceModel.GetInfluenceCostOfProposingStartingAlliance(evaluatingClan);
			if (evaluatingClan == Clan.PlayerClan)
			{
				return (float)influenceCostOfProposingStartingAlliance;
			}
			if (evaluatingClan.Kingdom != Clan.PlayerClan.Kingdom)
			{
				return (float)influenceCostOfProposingStartingAlliance;
			}
			float num = Campaign.Current.Models.AllianceModel.GetScoreOfStartingAlliance(querierKingdom, queriedKingdom, out explanationText, includeDescriptions).ResultNumber;
			if (evaluatingClan.Leader != null)
			{
				num += (float)evaluatingClan.Leader.GetRelation(queriedKingdom.Leader) * 0.25f;
			}
			if (this.IsThereMarriageBetweenClans(evaluatingClan, queriedKingdom.RulingClan))
			{
				num += 25f;
			}
			else if (queriedKingdom.Clans.AnyQ<Clan>((Clan clan) => clan != queriedKingdom.RulingClan && this.IsThereMarriageBetweenClans(evaluatingClan, clan)))
			{
				num += 5f;
			}
			if (queriedKingdom.Leader != null)
			{
				num += (float)queriedKingdom.Leader.GetTraitLevel(DefaultTraits.Honor) * 6.25f;
			}
			if (evaluatingClan.Leader != null)
			{
				num += evaluatingClan.Leader.RandomFloatWithSeed((uint)CampaignTime.Now.ToDays) * 20f - 10f;
			}
			float num2;
			if (num > 0f)
			{
				num2 = MBMath.Map(num, 0f, 195f, 0f, (float)influenceCostOfProposingStartingAlliance);
			}
			else
			{
				num2 = MBMath.Map(num, -135f, 0f, (float)(-(float)influenceCostOfProposingStartingAlliance), 0f);
			}
			return num2;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x0006830C File Offset: 0x0006650C
		public override bool CanMakeAlliance(Kingdom kingdom, Kingdom targetKingdom, IFaction evaluatingFaction, out TextObject reason, bool includeReason = false)
		{
			reason = (includeReason ? this._allianceNotFormedExplanationText : null);
			if (targetKingdom.IsEliminated || kingdom.IsEliminated)
			{
				if (includeReason)
				{
					reason.SetTextVariable("REASON", new TextObject("{=a5EAl1aW}That realm has been eliminated.", null));
				}
				return false;
			}
			if (targetKingdom == kingdom)
			{
				if (includeReason)
				{
					reason.SetTextVariable("REASON", new TextObject("{=zPoS5fIu}You are referring to your own realm.", null));
				}
				return false;
			}
			if (targetKingdom.IsAtWarWith(kingdom))
			{
				if (includeReason)
				{
					TextObject textObject = new TextObject("{=lseJ70y0}Your realm is at war with the {KINGDOM_NAME}.", null);
					textObject.SetTextVariable("KINGDOM_NAME", targetKingdom.Name);
					reason.SetTextVariable("REASON", textObject);
				}
				return false;
			}
			if (kingdom.AlliedKingdoms.Count >= Campaign.Current.Models.AllianceModel.MaxNumberOfAlliances)
			{
				if (includeReason)
				{
					TextObject textObject2 = new TextObject("{=xOJtrGAR}Your realm's current number of allies: {NUMBER_OF_ALLIES}/{MAX_NUMBER_OF_ALLIES}", null);
					textObject2.SetTextVariable("NUMBER_OF_ALLIES", kingdom.AlliedKingdoms.Count);
					textObject2.SetTextVariable("MAX_NUMBER_OF_ALLIES", Campaign.Current.Models.AllianceModel.MaxNumberOfAlliances);
					reason.SetTextVariable("REASON", textObject2);
				}
				return false;
			}
			if (targetKingdom.AlliedKingdoms.Count >= Campaign.Current.Models.AllianceModel.MaxNumberOfAlliances)
			{
				if (includeReason)
				{
					TextObject textObject3 = new TextObject("{=rYssCdQb}{KINGDOM_NAME} cannot have any more allies.", null);
					textObject3.SetTextVariable("KINGDOM_NAME", targetKingdom.Name);
					reason.SetTextVariable("REASON", textObject3);
				}
				return false;
			}
			if (targetKingdom.IsAllyWith(kingdom))
			{
				if (includeReason)
				{
					TextObject textObject4 = new TextObject("{=zd9sawl9}You are already allied with the {KINGDOM_NAME}.", null);
					textObject4.SetTextVariable("KINGDOM_NAME", targetKingdom.Name);
					reason.SetTextVariable("REASON", textObject4);
				}
				return false;
			}
			if (kingdom == Clan.PlayerClan.Kingdom)
			{
				if (Campaign.Current.Models.AllianceModel.GetScoreOfStartingAlliance(targetKingdom, kingdom, out reason, includeReason).ResultNumber < 50f)
				{
					return false;
				}
				Clan clan;
				if (evaluatingFaction != Clan.PlayerClan && (clan = evaluatingFaction as Clan) != null && !this.CanMakeAllianceWithPlayerSupport(kingdom, targetKingdom, clan))
				{
					return false;
				}
			}
			else
			{
				if (Campaign.Current.Models.AllianceModel.GetScoreOfStartingAlliance(kingdom, targetKingdom, out reason, includeReason).ResultNumber < 50f)
				{
					return false;
				}
				Clan playerClan = Clan.PlayerClan;
				if (((playerClan != null) ? playerClan.Kingdom : null) != null && Clan.PlayerClan.Kingdom == targetKingdom)
				{
					if (!this.CanMakeAllianceWithPlayerSupport(targetKingdom, kingdom, Campaign.Current.Models.AllianceModel.GetProposerClanForAllianceDecision(targetKingdom, kingdom)))
					{
						return false;
					}
				}
				else if (Campaign.Current.Models.AllianceModel.GetScoreOfStartingAlliance(targetKingdom, kingdom, out reason, includeReason).ResultNumber < 50f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x000685BE File Offset: 0x000667BE
		public override int GetInfluenceCostOfProposingStartingAlliance(Clan proposingClan)
		{
			return 200;
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x000685C8 File Offset: 0x000667C8
		public override float GetScoreOfCallingToWar(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, IFaction evaluatingFaction, out TextObject reason)
		{
			float num = 60f;
			reason = TextObject.GetEmpty();
			int callToWarBudgetOfCallingKingdom = this.GetCallToWarBudgetOfCallingKingdom(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
			int callToWarCost = Campaign.Current.Models.AllianceModel.GetCallToWarCost(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
			Clan clan2;
			Clan clan = (((clan2 = evaluatingFaction as Clan) != null) ? clan2 : callingKingdom.RulingClan);
			TextObject textObject;
			float scoreOfDeclaringWar = Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(callingKingdom, kingdomToCallToWarAgainst, clan, out textObject, false);
			if (callToWarBudgetOfCallingKingdom < 0 || callingKingdom.CallToWarWallet < -100000 || (float)callToWarBudgetOfCallingKingdom * 1.5f < (float)callToWarCost || scoreOfDeclaringWar > 0f)
			{
				return -100f;
			}
			if (callToWarCost == 0)
			{
				return 100f;
			}
			float num2 = (float)callToWarBudgetOfCallingKingdom / (float)callToWarCost;
			num *= num2;
			return num + ((float)evaluatingFaction.Leader.GetTraitLevel(DefaultTraits.Calculating) * 2.5f - (float)evaluatingFaction.Leader.GetTraitLevel(DefaultTraits.Valor) * 2.5f);
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x000686B0 File Offset: 0x000668B0
		public override float GetScoreOfJoiningWar(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst, IFaction evaluatingFaction, out TextObject reason)
		{
			float num = 70f;
			reason = TextObject.GetEmpty();
			int callToWarCostForCalledKingdom = this.GetCallToWarCostForCalledKingdom(calledKingdom, kingdomToCallToWarAgainst);
			int callToWarCost = Campaign.Current.Models.AllianceModel.GetCallToWarCost(callingKingdom, calledKingdom, kingdomToCallToWarAgainst);
			if (callToWarCostForCalledKingdom == 0)
			{
				return 100f;
			}
			float num2 = (float)callToWarCost / (float)callToWarCostForCalledKingdom;
			num2 = MathF.Clamp(num2, 1E-05f, 2f);
			num *= num2;
			return num + ((float)evaluatingFaction.Leader.GetTraitLevel(DefaultTraits.Valor) * 2.5f + (float)evaluatingFaction.Leader.GetTraitLevel(DefaultTraits.Calculating) * 2.5f);
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00068745 File Offset: 0x00066945
		public override int GetInfluenceCostOfCallingToWar(Clan proposingClan)
		{
			return 200;
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0006874C File Offset: 0x0006694C
		public override float GetAllianceFactorForDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			if (factionDeclaresWar.IsKingdomFaction && factionDeclaredWar.IsKingdomFaction)
			{
				bool flag = false;
				float num = 1f;
				Kingdom kingdom = (Kingdom)factionDeclaresWar;
				Kingdom kingdom2 = (Kingdom)factionDeclaredWar;
				foreach (Kingdom kingdom3 in kingdom.AlliedKingdoms)
				{
					if (kingdom3 == kingdom2)
					{
						num *= 0.5f;
						break;
					}
					if (!flag && kingdom3.IsAtWarWith(kingdom2))
					{
						float num2;
						float num3;
						if (this.GetThreateningNeighbor(kingdom, out num2, out num3).Item1 == kingdom2)
						{
							num *= 1.5f;
						}
						else
						{
							num *= 1.3f;
						}
						flag = true;
					}
				}
				return num;
			}
			return 1f;
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x00068810 File Offset: 0x00066A10
		public override float GetAllianceFactorForDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace)
		{
			if (factionDeclaresPeace.IsKingdomFaction && factionDeclaredPeace.IsKingdomFaction)
			{
				float num = 1f;
				Kingdom kingdom = (Kingdom)factionDeclaresPeace;
				Kingdom kingdom2 = (Kingdom)factionDeclaredPeace;
				using (List<Kingdom>.Enumerator enumerator = kingdom.AlliedKingdoms.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsAtWarWith(kingdom2))
						{
							num *= 0.7f;
							break;
						}
					}
				}
				return num;
			}
			return 1f;
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00068898 File Offset: 0x00066A98
		public override Clan GetProposerClanForAllianceDecision(Kingdom proposerKingdom, Kingdom proposedKingdom)
		{
			TextObject textObject;
			Clan clan = ((proposerKingdom.RulingClan != Clan.PlayerClan && Campaign.Current.Models.AllianceModel.GetSupportScoreOfStartingAllianceForClan(proposerKingdom, proposedKingdom, proposerKingdom.RulingClan, out textObject, false) > 0f) ? proposerKingdom.RulingClan : null);
			if (clan == null)
			{
				List<ValueTuple<Clan, float>> list = new List<ValueTuple<Clan, float>>(proposerKingdom.Clans.Count);
				foreach (Clan clan2 in proposerKingdom.Clans)
				{
					if (!clan2.IsUnderMercenaryService && clan2 != Clan.PlayerClan)
					{
						float supportScoreOfStartingAllianceForClan = Campaign.Current.Models.AllianceModel.GetSupportScoreOfStartingAllianceForClan(proposerKingdom, proposedKingdom, clan2, out textObject, false);
						if (supportScoreOfStartingAllianceForClan > 0f)
						{
							list.Add(new ValueTuple<Clan, float>(clan2, supportScoreOfStartingAllianceForClan));
						}
					}
				}
				if (list.Count > 0)
				{
					clan = list.MaxBy<ValueTuple<Clan, float>, float>(([TupleElementNames(new string[] { "Clan", "Score" })] ValueTuple<Clan, float> x) => x.Item2).Item1;
				}
				else
				{
					clan = ((proposerKingdom == Clan.PlayerClan.Kingdom) ? Clan.PlayerClan : proposerKingdom.RulingClan);
				}
			}
			return clan;
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x000689D4 File Offset: 0x00066BD4
		[return: TupleElementNames(new string[] { "threateningKingdom", "threatScore" })]
		private ValueTuple<Kingdom, float> GetThreateningNeighbor(Kingdom querierKingdom, out float exposureScore, out float powerRatio)
		{
			HashSet<Settlement> hashSet = new HashSet<Settlement>();
			Dictionary<Kingdom, float> dictionary = new Dictionary<Kingdom, float>();
			float num = 0f;
			foreach (Town town in querierKingdom.Fiefs)
			{
				foreach (Settlement settlement in town.GetNeighborFortifications(MobileParty.NavigationType.All))
				{
					if (settlement.MapFaction != querierKingdom && settlement.MapFaction.IsKingdomFaction && !hashSet.Contains(settlement))
					{
						Kingdom kingdom = (Kingdom)settlement.MapFaction;
						if (dictionary.ContainsKey(kingdom))
						{
							Dictionary<Kingdom, float> dictionary2 = dictionary;
							Kingdom kingdom2 = kingdom;
							dictionary2[kingdom2] += 1f;
						}
						else
						{
							dictionary.Add(kingdom, 1f);
						}
						num += 1f;
						hashSet.Add(settlement);
					}
				}
			}
			float num2 = 0f;
			Kingdom kingdom3 = null;
			exposureScore = 0f;
			powerRatio = 0f;
			foreach (KeyValuePair<Kingdom, float> keyValuePair in dictionary)
			{
				float num4;
				float num5;
				float num3 = this.CalculateThreatScore(keyValuePair.Value, num, this.CalculateKingdomStrength(keyValuePair.Key), this.CalculateKingdomStrength(querierKingdom), out num4, out num5);
				if (num2 < num3)
				{
					kingdom3 = keyValuePair.Key;
					num2 = num3;
					exposureScore = num4;
					powerRatio = num5;
				}
			}
			return new ValueTuple<Kingdom, float>(kingdom3, num2);
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x00068B8C File Offset: 0x00066D8C
		private bool IsThereMarriageBetweenClans(Clan clan1, Clan clan2)
		{
			return clan1.AliveLords.AnyQ<Hero>(delegate(Hero x)
			{
				Hero spouse = x.Spouse;
				Clan clan3;
				if (spouse == null)
				{
					clan3 = null;
				}
				else
				{
					Hero father = spouse.Father;
					clan3 = ((father != null) ? father.Clan : null);
				}
				return clan3 == clan2;
			}) || clan2.AliveLords.AnyQ<Hero>(delegate(Hero x)
			{
				Hero spouse2 = x.Spouse;
				Clan clan4;
				if (spouse2 == null)
				{
					clan4 = null;
				}
				else
				{
					Hero father2 = spouse2.Father;
					clan4 = ((father2 != null) ? father2.Clan : null);
				}
				return clan4 == clan1;
			});
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00068BEC File Offset: 0x00066DEC
		private TextObject BuildExplanationForAlliance(Kingdom other, List<ValueTuple<float, TextObject>> explanationList)
		{
			TextObject textObject = null;
			textObject = this._kingdomNotConsederingAllianceText;
			List<TextObject> list = new List<TextObject>();
			foreach (ValueTuple<float, TextObject> valueTuple in explanationList.OrderBy<ValueTuple<float, TextObject>, float>((ValueTuple<float, TextObject> x) => x.Item1))
			{
				TextObject item = valueTuple.Item2;
				list.Add(item);
				if (list.Count >= 3)
				{
					break;
				}
			}
			TextObject textObject2 = GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(list, new TextObject("{=!}{newline}", null), null);
			textObject.SetTextVariable("REASONS_BY_LINE", textObject2);
			textObject.SetTextVariable("KINGDOM", other.Name);
			return textObject;
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00068CAC File Offset: 0x00066EAC
		private int GetCallToWarCostForCalledKingdom(Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			TextObject textObject;
			float scoreOfDeclaringWar = Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringWar(calledKingdom, kingdomToCallToWarAgainst, calledKingdom.RulingClan, out textObject, false);
			float num = Campaign.Current.Models.DiplomacyModel.GetDecisionMakingThreshold(kingdomToCallToWarAgainst) - scoreOfDeclaringWar;
			if (num <= 0f)
			{
				return 0;
			}
			float valueOfSettlementsForFaction = Campaign.Current.Models.DiplomacyModel.GetValueOfSettlementsForFaction(calledKingdom);
			double num2 = (double)(num / (valueOfSettlementsForFaction + 1f));
			double num3 = (double)calledKingdom.Fiefs.SumQ<Town>((Town x) => x.Prosperity) * 0.35;
			return (int)(num2 * num3 * Campaign.Current.Models.AllianceModel.MaxDurationOfWarParticipation.ToDays);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00068D74 File Offset: 0x00066F74
		private int GetCallToWarBudgetOfCallingKingdom(Kingdom callingKingdom, Kingdom calledKingdom, Kingdom kingdomToCallToWarAgainst)
		{
			float num = this.CalculateKingdomStrength(callingKingdom);
			float num2 = this.CalculateKingdomStrength(calledKingdom);
			float num3 = this.CalculateKingdomStrength(kingdomToCallToWarAgainst);
			double num4 = (double)callingKingdom.Fiefs.SumQ<Town>((Town x) => x.Prosperity) * 0.35;
			float num5 = num - num3;
			if (num5.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return int.MinValue;
			}
			return (int)((double)MathF.Clamp(-(num2 / num5), float.MinValue, 1f) * num4 * Campaign.Current.Models.AllianceModel.MaxDurationOfWarParticipation.ToDays);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00068E20 File Offset: 0x00067020
		private bool AreKingdomsNeighbors(Kingdom kingdom1, Kingdom kingdom2)
		{
			if (kingdom1 == kingdom2 || kingdom1.Fiefs.Count == 0 || kingdom2.Fiefs.Count == 0)
			{
				return false;
			}
			foreach (Town town in kingdom1.Fiefs)
			{
				using (List<Settlement>.Enumerator enumerator2 = town.GetNeighborFortifications(MobileParty.NavigationType.All).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.MapFaction == kingdom2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x00068ED4 File Offset: 0x000670D4
		private float CalculateThreatScore(float neighborScore, float totalNeighborScore, float powerOfThreat, float powerOfQuerier, out float exposureScore, out float powerRatio)
		{
			if (powerOfQuerier <= 0f || totalNeighborScore <= 0f)
			{
				exposureScore = 0f;
				powerRatio = 0f;
				return 0f;
			}
			exposureScore = MBMath.Map(neighborScore / totalNeighborScore, 0f, 1f, 1f, 2f);
			powerRatio = MathF.Clamp(powerOfThreat / powerOfQuerier, 0f, 3f);
			return (MathF.Min(exposureScore, 1.7f) + 0.4f + powerRatio) * 130f;
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x00068F5A File Offset: 0x0006715A
		private float GetThreatEffect(float threatScoreForQuerier, float threatScoreForQueried)
		{
			return 0.08f * (threatScoreForQueried + threatScoreForQuerier) * 0.66f;
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x00068F6C File Offset: 0x0006716C
		private float GetRelationshipEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			if (querierKingdom.Leader != null && queriedKingdom.Leader != null)
			{
				int relation = querierKingdom.Leader.GetRelation(queriedKingdom.Leader);
				int traitLevel = querierKingdom.Leader.GetTraitLevel(DefaultTraits.Calculating);
				if (relation > 0 || traitLevel <= 0)
				{
					return MathF.Clamp((float)relation, -100f, 100f) * 0.08f;
				}
			}
			return 0f;
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00068FD4 File Offset: 0x000671D4
		private float GetMarriageEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			if (this.IsThereMarriageBetweenClans(querierKingdom.RulingClan, queriedKingdom.RulingClan))
			{
				return 8f;
			}
			if (querierKingdom.Leader != null && queriedKingdom.RulingClan.AliveLords.AnyQ<Hero>(delegate(Hero x)
			{
				Hero spouse = x.Spouse;
				Kingdom kingdom;
				if (spouse == null)
				{
					kingdom = null;
				}
				else
				{
					Hero father = spouse.Father;
					kingdom = ((father != null) ? father.MapFaction : null);
				}
				return kingdom == queriedKingdom;
			}))
			{
				return 4f;
			}
			return 0f;
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x00069044 File Offset: 0x00067244
		private float GetAtWarWithAllyEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			return (queriedKingdom.FactionsAtWarWith.AnyQ<IFaction>((IFaction x) => x.IsKingdomFaction && querierKingdom.IsAllyWith((Kingdom)x)) ? (-100f) : 0f) * 0.08f;
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x0006908C File Offset: 0x0006728C
		private float GetAtWarWithEnemyEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			return (queriedKingdom.FactionsAtWarWith.AnyQ<IFaction>((IFaction x) => x.IsKingdomFaction && querierKingdom.IsAtWarWith((Kingdom)x)) ? 100f : 0f) * 0.08f;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x000690D1 File Offset: 0x000672D1
		private float GetAtWarOrPeaceEffect(Kingdom queriedKingdom)
		{
			return (queriedKingdom.FactionsAtWarWith.AnyQ<IFaction>((IFaction x) => x.IsKingdomFaction) ? (-5f) : 25f) * 0.08f;
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00069114 File Offset: 0x00067314
		private float GetFiefWithSameCultureEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			float num = (float)queriedKingdom.Fiefs.Count;
			if (num > 0f)
			{
				return MathF.Clamp((float)queriedKingdom.Fiefs.Count<Town>((Town fief) => fief.Culture == querierKingdom.Culture) / num * -200f, -200f, 0f) * 0.08f;
			}
			return 0f;
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00069180 File Offset: 0x00067380
		private float GetHonorableKingEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			if (querierKingdom.Leader != null)
			{
				int traitLevel = querierKingdom.Leader.GetTraitLevel(DefaultTraits.Honor);
				int traitLevel2 = queriedKingdom.Leader.GetTraitLevel(DefaultTraits.Honor);
				if (traitLevel > 0)
				{
					return 4f;
				}
				if (traitLevel < 0 && traitLevel2 > 0)
				{
					return -4f;
				}
			}
			return 0f;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000691D4 File Offset: 0x000673D4
		private float GetTradeAgreementEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			ITradeAgreementsCampaignBehavior tradeAgreementsCampaignBehavior = this.TradeAgreementsCampaignBehavior;
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
			if (tradeAgreementsCampaignBehavior != null && tradeAgreementsCampaignBehavior.HasTradeAgreement(querierKingdom, queriedKingdom, out tradeAgreement))
			{
				return 4f;
			}
			return 0f;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x00069204 File Offset: 0x00067404
		private float GetCommonThreatEffect(Kingdom threateningKingdomForQuerier, Kingdom threateningKingdomForQueried)
		{
			if (threateningKingdomForQuerier != null && threateningKingdomForQueried != null && threateningKingdomForQuerier == threateningKingdomForQueried)
			{
				return 8f;
			}
			return 0f;
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0006921C File Offset: 0x0006741C
		private float GetAlliancePenalty(Kingdom kingdom)
		{
			if (kingdom.AlliedKingdoms.Count > 0)
			{
				float num = -48f;
				if (kingdom == Clan.PlayerClan.Kingdom)
				{
					num *= 0.5f;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0006925C File Offset: 0x0006745C
		private TextObject GetAlliancePenaltyText(Kingdom kingdom, bool includeDescription)
		{
			TextObject textObject = ((kingdom == Clan.PlayerClan.Kingdom) ? this._tooManyAlliancePlayerPenaltyText : this._tooManyAllianceAIPenaltyText);
			if (includeDescription)
			{
				textObject.SetTextVariable("NUMBER_OF_ALLIES", kingdom.AlliedKingdoms.Count);
				textObject.SetTextVariable("KINGDOM_NAME", kingdom.Name);
				textObject.SetTextVariable("MAX_NUMBER_OF_ALLIES", Campaign.Current.Models.AllianceModel.MaxNumberOfAlliances);
			}
			return textObject;
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x000692D4 File Offset: 0x000674D4
		private bool CanMakeAllianceWithPlayerSupport(Kingdom proposingKingdom, Kingdom proposedKingdom, Clan evaluatingClan)
		{
			if (proposingKingdom == Clan.PlayerClan.Kingdom && evaluatingClan == Clan.PlayerClan)
			{
				return true;
			}
			KingdomElection kingdomElection = new KingdomElection(new StartAllianceDecision(evaluatingClan, proposedKingdom));
			DecisionOutcome decisionOutcome = kingdomElection.PossibleOutcomes.FirstOrDefault<DecisionOutcome>(delegate(DecisionOutcome x)
			{
				StartAllianceDecision.StartAllianceDecisionOutcome startAllianceDecisionOutcome;
				return (startAllianceDecisionOutcome = x as StartAllianceDecision.StartAllianceDecisionOutcome) != null && startAllianceDecisionOutcome.ShouldAllianceBeStarted;
			});
			kingdomElection.SetupResultWithoutPlayerSupport();
			return kingdomElection.GetWinChanceWithPlayerSupport(decisionOutcome, Supporter.SupportWeights.FullyPush) > 0.5f;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00069344 File Offset: 0x00067544
		private float CalculateKingdomStrength(Kingdom kingdom)
		{
			float num = 0f;
			foreach (Clan clan in kingdom.Clans)
			{
				if (!clan.IsUnderMercenaryService)
				{
					num += clan.CurrentTotalStrength;
				}
			}
			return num;
		}

		// Token: 0x04000775 RID: 1909
		private const int ThresholdForCallToWarWallet = 100000;

		// Token: 0x04000776 RID: 1910
		private const float FirstDegreeNeighborScore = 1f;

		// Token: 0x04000777 RID: 1911
		private const int ThreatScoreCoefficient = 130;

		// Token: 0x04000778 RID: 1912
		private const float AllianceScoreNormalizationFactor = 0.08f;

		// Token: 0x04000779 RID: 1913
		private const float MarriageEffect = 50f;

		// Token: 0x0400077A RID: 1914
		private const float AtWarEffect = -5f;

		// Token: 0x0400077B RID: 1915
		private const float AtPeaceEffect = 25f;

		// Token: 0x0400077C RID: 1916
		private const float AtWarWithAllyEffect = -100f;

		// Token: 0x0400077D RID: 1917
		private const float AtWarWithEnemyEffect = 100f;

		// Token: 0x0400077E RID: 1918
		private const float HonorableRulerEffect = 50f;

		// Token: 0x0400077F RID: 1919
		private const float DishonorableRulerEffect = -50f;

		// Token: 0x04000780 RID: 1920
		private const float TradeAgreementEffect = 50f;

		// Token: 0x04000781 RID: 1921
		private const float CommonThreatEffect = 100f;

		// Token: 0x04000782 RID: 1922
		private const float ThresholdForThreatScoreForQuerier = 430f;

		// Token: 0x04000783 RID: 1923
		private const float ThresholdForThreatScoreForQueried = 430f;

		// Token: 0x04000784 RID: 1924
		private const float SecondAlliancePenaltyForAllianceScore = -600f;

		// Token: 0x04000785 RID: 1925
		private const float WarDeclarationScorePenaltyAgainstAllies = 0.5f;

		// Token: 0x04000786 RID: 1926
		private const float WarDeclarationScoreBonusAgainstEnemiesOfAllies = 0.3f;

		// Token: 0x04000787 RID: 1927
		private const float WarDeclarationScoreBonusAgainstBiggestThreat = 0.5f;

		// Token: 0x04000788 RID: 1928
		private const float PeaceDeclarationScorePenaltyAgainstEnemiesOfAllies = 0.7f;

		// Token: 0x04000789 RID: 1929
		private const float AllianceScoreThreshold = 50f;

		// Token: 0x0400078A RID: 1930
		private readonly TextObject _relationshipText = new TextObject("{=3YVDMg5X}Low relations between rulers.", null);

		// Token: 0x0400078B RID: 1931
		private readonly TextObject _kingdomsNotNeighborsText = new TextObject("{=Bu6YdMme}Kingdoms aren't neighbors.", null);

		// Token: 0x0400078C RID: 1932
		private readonly TextObject _kingdomsNotSeekingAllianceText = new TextObject("{=ml9bhOka}{KINGDOM_NAME} is not seeking alliances at the moment.", null);

		// Token: 0x0400078D RID: 1933
		private readonly TextObject _atWarWithAllyText = new TextObject("{=tT91z3AL}Your realm is at war with their ally.", null);

		// Token: 0x0400078E RID: 1934
		private readonly TextObject _sameCultureFiefsText = new TextObject("{=S5lz4aHk}Your realm is occupying fiefs belonging to their culture.", null);

		// Token: 0x0400078F RID: 1935
		private readonly TextObject _atWarText = new TextObject("{=aUStIIWw}Your realm is participating in a war.", null);

		// Token: 0x04000790 RID: 1936
		private readonly TextObject _lowHonorText = new TextObject("{=VIkVcmaE}{RULER.NAME} has low honor.", null);

		// Token: 0x04000791 RID: 1937
		private readonly TextObject _kingdomNotConsederingAllianceText = new TextObject("{=tbH04aAX}{KINGDOM} is not considering an alliance with your realm due to:{newline}{newline}{REASONS_BY_LINE}", null);

		// Token: 0x04000792 RID: 1938
		private readonly TextObject _allianceNotFormedExplanationText = new TextObject("{=Y20TbMLR}An alliance cannot be formed due to:{newline}{newline}{REASON}", null);

		// Token: 0x04000793 RID: 1939
		private readonly TextObject _tooManyAlliancePlayerPenaltyText = new TextObject("{=2RiYKRM8}Number of alliances your realm's already in: {NUMBER_OF_ALLIES}/{MAX_NUMBER_OF_ALLIES}", null);

		// Token: 0x04000794 RID: 1940
		private readonly TextObject _tooManyAllianceAIPenaltyText = new TextObject("{=cFYMdTcr}Number of alliances {KINGDOM_NAME} is already in: {NUMBER_OF_ALLIES}/{MAX_NUMBER_OF_ALLIES}", null);

		// Token: 0x04000795 RID: 1941
		private readonly TextObject _allianceScoreNotEnoughText = new TextObject("{=XnwKgWab}{KINGDOM_NAME} currently does not consider your realm to be a possible ally.", null);

		// Token: 0x04000796 RID: 1942
		private readonly TextObject _threatEffect = new TextObject("{=!}Threat Effect", null);

		// Token: 0x04000797 RID: 1943
		private readonly TextObject _marriageEffect = new TextObject("{=!}Marriage Effect", null);

		// Token: 0x04000798 RID: 1944
		private readonly TextObject _atWarWithEnemyEffect = new TextObject("{=!}At war with enemy Effect", null);

		// Token: 0x04000799 RID: 1945
		private readonly TextObject _tradeAgreementEffect = new TextObject("{=!}Trade Agreement Effect", null);

		// Token: 0x0400079A RID: 1946
		private readonly TextObject _commonThreatEffect = new TextObject("{=!}Common Threat Effect", null);

		// Token: 0x0400079B RID: 1947
		private const int MaxReasonsInExplanation = 3;

		// Token: 0x0400079C RID: 1948
		private ITradeAgreementsCampaignBehavior _tradeAgreementsBehavior;
	}
}
