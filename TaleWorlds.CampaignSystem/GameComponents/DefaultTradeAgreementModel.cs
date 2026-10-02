using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000166 RID: 358
	public class DefaultTradeAgreementModel : TradeAgreementModel
	{
		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001B70 RID: 7024 RVA: 0x0008D343 File Offset: 0x0008B543
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

		// Token: 0x06001B71 RID: 7025 RVA: 0x0008D363 File Offset: 0x0008B563
		public override int GetInfluenceCostOfProposingTradeAgreement(Clan proposerClan)
		{
			return 200;
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0008D36A File Offset: 0x0008B56A
		public override int GetMaximumTradeAgreementCount(Kingdom kingdom)
		{
			return 2;
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0008D370 File Offset: 0x0008B570
		public override bool CanMakeTradeAgreement(Kingdom querierKingdom, Kingdom queriedKingdom, bool checkOtherSideSupport, out TextObject reason, bool includeReason = false)
		{
			reason = (includeReason ? TextObject.GetEmpty() : null);
			if (querierKingdom.IsAtWarWith(queriedKingdom))
			{
				reason = DefaultTradeAgreementModel._kingdomsAtWarText;
				return false;
			}
			if (queriedKingdom.IsEliminated)
			{
				reason = DefaultTradeAgreementModel._eliminatedKingdomText;
				return false;
			}
			ITradeAgreementsCampaignBehavior tradeAgreementsCampaignBehavior = this.TradeAgreementsCampaignBehavior;
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
			if (tradeAgreementsCampaignBehavior != null && tradeAgreementsCampaignBehavior.HasTradeAgreement(querierKingdom, queriedKingdom, out tradeAgreement))
			{
				reason = DefaultTradeAgreementModel._existingTradeAgreementText;
				return false;
			}
			if (Kingdom.All.Count<Kingdom>(delegate(Kingdom x)
			{
				if (x != querierKingdom && !x.IsEliminated)
				{
					ITradeAgreementsCampaignBehavior tradeAgreementsCampaignBehavior2 = this.TradeAgreementsCampaignBehavior;
					TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement2;
					return tradeAgreementsCampaignBehavior2 != null && tradeAgreementsCampaignBehavior2.HasTradeAgreement(querierKingdom, x, out tradeAgreement2);
				}
				return false;
			}) >= Campaign.Current.Models.TradeAgreementModel.GetMaximumTradeAgreementCount(querierKingdom))
			{
				reason = DefaultTradeAgreementModel._maximumNumberOfTradeAgreementsText;
				return false;
			}
			if (Kingdom.All.Count<Kingdom>(delegate(Kingdom x)
			{
				if (x != queriedKingdom && !x.IsEliminated)
				{
					ITradeAgreementsCampaignBehavior tradeAgreementsCampaignBehavior3 = this.TradeAgreementsCampaignBehavior;
					TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement3;
					return tradeAgreementsCampaignBehavior3 != null && tradeAgreementsCampaignBehavior3.HasTradeAgreement(queriedKingdom, x, out tradeAgreement3);
				}
				return false;
			}) >= Campaign.Current.Models.TradeAgreementModel.GetMaximumTradeAgreementCount(querierKingdom))
			{
				if (includeReason)
				{
					reason = new TextObject("{=O6zpuLGa}{OTHER_KINGDOM} already has maximum number of trade agreements.", null);
					reason.SetTextVariable("OTHER_KINGDOM", queriedKingdom.Name);
				}
				return false;
			}
			if (querierKingdom.Towns.Count == 0)
			{
				reason = DefaultTradeAgreementModel._noTownText;
				return false;
			}
			if (queriedKingdom.Towns.Count == 0)
			{
				if (includeReason)
				{
					reason = new TextObject("{=XkbKOO3v}{OTHER_KINGDOM} does not own any towns.", null);
					reason.SetTextVariable("OTHER_KINGDOM", queriedKingdom.Name);
				}
				return false;
			}
			Func<Settlement, bool> <>9__7;
			if (!querierKingdom.Fiefs.Any<Town>(delegate(Town x)
			{
				IEnumerable<Settlement> neighborFortifications = x.GetNeighborFortifications(MobileParty.NavigationType.All);
				Func<Settlement, bool> func;
				if ((func = <>9__7) == null)
				{
					func = (<>9__7 = (Settlement y) => y.MapFaction == queriedKingdom);
				}
				return neighborFortifications.Any<Settlement>(func);
			}))
			{
				reason = DefaultTradeAgreementModel._kingdomsNotNeighborsText;
				return false;
			}
			if (querierKingdom.Towns.All<Town>((Town x) => x.Settlement.HasPort))
			{
				if (queriedKingdom.Towns.All<Town>((Town x) => !x.Settlement.HasPort))
				{
					goto IL_026C;
				}
			}
			if (querierKingdom.Towns.All<Town>((Town x) => !x.Settlement.HasPort))
			{
				if (queriedKingdom.Towns.All<Town>((Town x) => x.Settlement.HasPort))
				{
					goto IL_026C;
				}
			}
			if (checkOtherSideSupport)
			{
				TradeAgreementDecision tradeAgreementDecision = new TradeAgreementDecision(queriedKingdom.RulingClan, querierKingdom);
				KingdomElection kingdomElection = new KingdomElection(tradeAgreementDecision);
				kingdomElection.SetupResultWithoutPlayerSupport();
				if (queriedKingdom == Clan.PlayerClan.Kingdom)
				{
					DecisionOutcome decisionOutcome = kingdomElection.PossibleOutcomes.FirstOrDefault<DecisionOutcome>(delegate(DecisionOutcome x)
					{
						TradeAgreementDecision.TradeAgreementDecisionOutcome tradeAgreementDecisionOutcome;
						return (tradeAgreementDecisionOutcome = x as TradeAgreementDecision.TradeAgreementDecisionOutcome) != null && tradeAgreementDecisionOutcome.ShouldTradeAgreementStart;
					});
					return kingdomElection.GetWinChanceWithPlayerSupport(decisionOutcome, Supporter.SupportWeights.FullyPush) > 0.5f;
				}
				if (kingdomElection.GetWinChanceForSponsor(queriedKingdom.RulingClan) < 0.5f)
				{
					tradeAgreementDecision.CalculateSupport(queriedKingdom.RulingClan, out reason);
					return false;
				}
			}
			return true;
			IL_026C:
			reason = DefaultTradeAgreementModel._landlockedText;
			return false;
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0008D69C File Offset: 0x0008B89C
		public override float GetScoreOfStartingTradeAgreement(Kingdom querierKingdom, Kingdom queriedKingdom, Clan clan, out TextObject detailedBreakdownTooltip, bool includeExplanation = false)
		{
			detailedBreakdownTooltip = null;
			float securityEffect = this.GetSecurityEffect(queriedKingdom);
			float relationEffectBetweenRulers = this.GetRelationEffectBetweenRulers(querierKingdom, queriedKingdom);
			float allianceEffect = this.GetAllianceEffect(querierKingdom, queriedKingdom);
			bool flag;
			bool flag2;
			float diplomacyEffect = this.GetDiplomacyEffect(querierKingdom, queriedKingdom, out flag, out flag2);
			float marriageEffect = this.GetMarriageEffect(querierKingdom, queriedKingdom);
			float num;
			float num2;
			float prosperityEffect = this.GetProsperityEffect(querierKingdom, queriedKingdom, out num, out num2);
			float exposureEffect = this.GetExposureEffect(querierKingdom, queriedKingdom);
			float num3 = 0f;
			if (querierKingdom == Clan.PlayerClan.Kingdom && !Clan.PlayerClan.IsUnderMercenaryService)
			{
				num3 += this.GetSubjectiveEffect(queriedKingdom, clan);
			}
			float num4 = securityEffect + prosperityEffect + relationEffectBetweenRulers + allianceEffect + diplomacyEffect + exposureEffect + num3 + marriageEffect;
			if (includeExplanation)
			{
				List<ValueTuple<float, TextObject>> list = new List<ValueTuple<float, TextObject>>();
				if (securityEffect < 0f)
				{
					list.Add(new ValueTuple<float, TextObject>(MathF.Abs(securityEffect), DefaultTradeAgreementModel._lowSecurityText));
				}
				if (relationEffectBetweenRulers < 0f)
				{
					list.Add(new ValueTuple<float, TextObject>((float)Campaign.Current.Models.DiplomacyModel.MaxRelationLimit * 0.1f - relationEffectBetweenRulers, DefaultTradeAgreementModel._relationsText));
				}
				if (flag)
				{
					list.Add(new ValueTuple<float, TextObject>(MathF.Abs(-15f), DefaultTradeAgreementModel._warWithAlliedText));
				}
				else if (flag2)
				{
					list.Add(new ValueTuple<float, TextObject>(MathF.Abs(-1.5f), DefaultTradeAgreementModel._warText));
				}
				if (list.Sum<ValueTuple<float, TextObject>>((ValueTuple<float, TextObject> x) => x.Item1).ApproximatelyEqualsTo(0f, 1E-05f))
				{
					if (num > num2)
					{
						list.Add(new ValueTuple<float, TextObject>(1f, DefaultTradeAgreementModel._higherQuerierProsperityText));
					}
					else
					{
						list.Add(new ValueTuple<float, TextObject>(1f, DefaultTradeAgreementModel._higherQueriedProsperityText));
					}
					if (exposureEffect < 30f)
					{
						list.Add(new ValueTuple<float, TextObject>(1f, DefaultTradeAgreementModel._limitedSharedBordersText));
					}
				}
				list = list.OrderByDescending<ValueTuple<float, TextObject>, float>((ValueTuple<float, TextObject> x) => x.Item1).ToList<ValueTuple<float, TextObject>>();
				List<TextObject> list2 = new List<TextObject>();
				foreach (ValueTuple<float, TextObject> valueTuple in list)
				{
					list2.Add(valueTuple.Item2);
					if (list2.Count >= 3)
					{
						break;
					}
				}
				TextObject textObject = GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(list2, new TextObject("{=!}{newline}", null), null);
				detailedBreakdownTooltip = new TextObject("{=jXcb9oHi}{KINGDOM} is not considering a trade agreement with your realm due to:{newline}{newline}{REASONS_BY_LINE}", null);
				detailedBreakdownTooltip.SetTextVariable("KINGDOM", querierKingdom.Name);
				detailedBreakdownTooltip.SetTextVariable("REASONS_BY_LINE", textObject);
			}
			return MBMath.ClampFloat(num4, 0f, 100f);
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0008D948 File Offset: 0x0008BB48
		private float GetMarriageEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			if (this.IsThereMarriageBetweenClans(queriedKingdom.RulingClan, querierKingdom.RulingClan))
			{
				return 10f;
			}
			if (queriedKingdom.Clans.AnyQ<Clan>((Clan clan) => clan != queriedKingdom.RulingClan && this.IsThereMarriageBetweenClans(querierKingdom.RulingClan, clan)))
			{
				return 5f;
			}
			return 0f;
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0008D9C0 File Offset: 0x0008BBC0
		private bool IsThereMarriageBetweenClans(Clan clan1, Clan clan2)
		{
			return clan1.AliveLords.Any<Hero>((Hero x) => x.OriginClan == clan2) || clan2.AliveLords.Any<Hero>((Hero x) => x.OriginClan == clan1);
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0008DA20 File Offset: 0x0008BC20
		private float GetSubjectiveEffect(Kingdom queriedKingdom, Clan clan)
		{
			return (float)clan.Leader.GetRelation(queriedKingdom.Leader) * 0.25f + clan.Leader.RandomFloatWithSeed((uint)CampaignTime.Now.ToWeeks, -10f, 10f);
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0008DA69 File Offset: 0x0008BC69
		private float GetAllianceEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			if (queriedKingdom.IsAllyWith(querierKingdom))
			{
				return 5f;
			}
			return 0f;
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0008DA80 File Offset: 0x0008BC80
		private float GetDiplomacyEffect(Kingdom querierKingdom, Kingdom queriedKingdom, out bool isAtWarWithAlliedKingdom, out bool isAtWarWithAnyKingdom)
		{
			isAtWarWithAlliedKingdom = querierKingdom.AlliedKingdoms.Any<Kingdom>((Kingdom x) => x.IsAtWarWith(queriedKingdom));
			isAtWarWithAnyKingdom = isAtWarWithAlliedKingdom || Kingdom.All.Any<Kingdom>((Kingdom x) => !x.IsEliminated && x != queriedKingdom && x != querierKingdom && x.IsAtWarWith(queriedKingdom));
			return 2.5f;
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0008DAE4 File Offset: 0x0008BCE4
		private float GetRelationEffectBetweenRulers(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			return (float)querierKingdom.Leader.GetRelation(queriedKingdom.Leader) * 0.1f;
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x0008DB00 File Offset: 0x0008BD00
		private float GetProsperityEffect(Kingdom querierKingdom, Kingdom queriedKingdom, out float querierKingdomAverageProsperity, out float queriedKingdomAverageProsperity)
		{
			querierKingdomAverageProsperity = this.GetAverageProsperityOfTownsInKingdom(querierKingdom);
			queriedKingdomAverageProsperity = this.GetAverageProsperityOfTownsInKingdom(queriedKingdom);
			return (2500f - MathF.Clamp(MathF.Abs(queriedKingdomAverageProsperity - querierKingdomAverageProsperity), 0.1f, 2500f)) / 2500f * 45f;
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x0008DB4C File Offset: 0x0008BD4C
		private float GetAverageProsperityOfTownsInKingdom(Kingdom kingdom)
		{
			if (kingdom.Towns.Count > 0)
			{
				float num = 0f;
				for (int i = 0; i < kingdom.Towns.Count; i++)
				{
					num += kingdom.Towns[i].Prosperity;
				}
				return num / (float)kingdom.Towns.Count;
			}
			return 0f;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x0008DBAC File Offset: 0x0008BDAC
		private float GetSecurityEffect(Kingdom queriedKingdom)
		{
			if (queriedKingdom.Towns.Count > 0)
			{
				float num = 0f;
				for (int i = 0; i < queriedKingdom.Towns.Count; i++)
				{
					num += queriedKingdom.Towns[i].Security;
				}
				return MathF.Clamp((num / (float)queriedKingdom.Towns.Count - 85f) * 0.4f, -5f, 0f);
			}
			return 0f;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0008DC26 File Offset: 0x0008BE26
		public override CampaignTime GetTradeAgreementDurationInYears(Kingdom iniatatingKingdom, Kingdom otherKingdom)
		{
			return CampaignTime.Years(1f);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0008DC34 File Offset: 0x0008BE34
		private float GetExposureEffect(Kingdom querierKingdom, Kingdom queriedKingdom)
		{
			float num = 0f;
			if (queriedKingdom.Fiefs.Count > 0 && querierKingdom.Fiefs.Count > 0)
			{
				HashSet<Settlement> hashSet = new HashSet<Settlement>();
				int num2 = 0;
				int num3 = 0;
				foreach (Town town in querierKingdom.Fiefs)
				{
					foreach (Settlement settlement in town.GetNeighborFortifications(MobileParty.NavigationType.All))
					{
						if (settlement.IsFortification && !hashSet.Contains(settlement) && settlement.MapFaction != querierKingdom)
						{
							if (settlement.MapFaction == queriedKingdom)
							{
								num3++;
							}
							num2++;
							hashSet.Add(settlement);
						}
					}
				}
				num = Math.Min((float)num3 / (float)num2 * 50f, 30f);
			}
			return num;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0008DD40 File Offset: 0x0008BF40
		public override int GetProfitPerCaravanVisit(MobileParty mobileParty)
		{
			return 500;
		}

		// Token: 0x04000918 RID: 2328
		private const float MaxExposureEffect = 30f;

		// Token: 0x04000919 RID: 2329
		private const float MarriageBetweenRulingClansBonus = 10f;

		// Token: 0x0400091A RID: 2330
		private const float MarriageBetweenKingdomsBonus = 5f;

		// Token: 0x0400091B RID: 2331
		private const float AlliedScoreBonus = 5f;

		// Token: 0x0400091C RID: 2332
		private const float WarWithAllyPenalty = -15f;

		// Token: 0x0400091D RID: 2333
		private const float WarWithAnyKingdomPenalty = -1.5f;

		// Token: 0x0400091E RID: 2334
		private const float NoWarBonus = 2.5f;

		// Token: 0x0400091F RID: 2335
		private const float ProsperityCheckRange = 2500f;

		// Token: 0x04000920 RID: 2336
		private static readonly TextObject _kingdomsAtWarText = new TextObject("{=vo7kAlkR}The kingdoms are at war.", null);

		// Token: 0x04000921 RID: 2337
		private static readonly TextObject _eliminatedKingdomText = new TextObject("{=ZeNt57yM}The kingdom is eliminated.", null);

		// Token: 0x04000922 RID: 2338
		private static readonly TextObject _existingTradeAgreementText = new TextObject("{=8HXcla1b}These kingdoms already have a trade agreement.", null);

		// Token: 0x04000923 RID: 2339
		private static readonly TextObject _maximumNumberOfTradeAgreementsText = new TextObject("{=DJ51OJWj}You already have maximum number of trade agreements.", null);

		// Token: 0x04000924 RID: 2340
		private static readonly TextObject _noTownText = new TextObject("{=QQ4bi6Zr}You don't own any towns.", null);

		// Token: 0x04000925 RID: 2341
		private static readonly TextObject _landlockedText = new TextObject("{=Ig8l75Rg}One of the kingdoms is landlocked.", null);

		// Token: 0x04000926 RID: 2342
		private static readonly TextObject _kingdomsNotNeighborsText = new TextObject("{=Bu6YdMme}Kingdoms aren't neighbors.", null);

		// Token: 0x04000927 RID: 2343
		private static readonly TextObject _limitedSharedBordersText = new TextObject("{=EapZFDGF}Limited shared borders", null);

		// Token: 0x04000928 RID: 2344
		private static readonly TextObject _relationsText = new TextObject("{=3YVDMg5X}Low relations between rulers", null);

		// Token: 0x04000929 RID: 2345
		private static readonly TextObject _warWithAlliedText = new TextObject("{=tT91z3AL}Your realm is at war with their ally.", null);

		// Token: 0x0400092A RID: 2346
		private static readonly TextObject _warText = new TextObject("{=FaEOnF8q}Your realm is a participant in a war.", null);

		// Token: 0x0400092B RID: 2347
		private static readonly TextObject _lowSecurityText = new TextObject("{=aTuRql06}Target faction is concerned by security of towns in your realm.", null);

		// Token: 0x0400092C RID: 2348
		private static readonly TextObject _higherQuerierProsperityText = new TextObject("{=ji8oPOXU}Your realm is not open to negotiation as target faction’s prosperity is too low.", null);

		// Token: 0x0400092D RID: 2349
		private static readonly TextObject _higherQueriedProsperityText = new TextObject("{=lYaumdUj}Your realm has lower prosperity then target faction.", null);

		// Token: 0x0400092E RID: 2350
		private static readonly TextObject _recentWarText = new TextObject("{=lDIz0nEY}Recent war", null);

		// Token: 0x0400092F RID: 2351
		private const int MaxReasonsInExplanation = 3;

		// Token: 0x04000930 RID: 2352
		private ITradeAgreementsCampaignBehavior _tradeAgreementsBehavior;
	}
}
