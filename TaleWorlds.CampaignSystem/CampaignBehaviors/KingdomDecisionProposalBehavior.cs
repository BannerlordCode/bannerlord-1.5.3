using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000434 RID: 1076
	public class KingdomDecisionProposalBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x0600441C RID: 17436 RVA: 0x00140801 File Offset: 0x0013EA01
		public ITradeAgreementsCampaignBehavior TradeAgreementsCampaignBehavior
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

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x0600441D RID: 17437 RVA: 0x00140821 File Offset: 0x0013EA21
		private IAllianceCampaignBehavior AllianceCampaignBehavior
		{
			get
			{
				if (this._allianceCampaignBehavior == null)
				{
					this._allianceCampaignBehavior = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
				}
				return this._allianceCampaignBehavior;
			}
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x00140844 File Offset: 0x0013EA44
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceMade));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.KingdomDecisionAdded.AddNonSerializedListener(this, new Action<KingdomDecision, bool>(this.OnKingdomDecisionAdded));
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x00140909 File Offset: 0x0013EB09
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			this.UpdateKingdomDecisions(kingdom);
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x00140914 File Offset: 0x0013EB14
		private void DailyTickClan(Clan clan)
		{
			if ((float)((int)Campaign.Current.Models.CampaignTimeModel.CampaignStartTime.ElapsedDaysUntilNow) < 5f)
			{
				return;
			}
			if (clan.IsEliminated)
			{
				return;
			}
			if (clan == Clan.PlayerClan || clan.CurrentTotalStrength <= 0f)
			{
				return;
			}
			if (clan.IsBanditFaction)
			{
				return;
			}
			if (clan.Kingdom == null)
			{
				return;
			}
			if (clan.Influence < 100f)
			{
				return;
			}
			KingdomDecision kingdomDecision = null;
			float randomFloat = MBRandom.RandomFloat;
			int num = ((Kingdom)clan.MapFaction).Clans.Count<Clan>((Clan x) => x.Influence > 100f);
			float num2 = MathF.Min(0.33f, 1f / ((float)num + 2f));
			num2 *= ((clan.Kingdom == Hero.MainHero.MapFaction && !Hero.MainHero.Clan.IsUnderMercenaryService) ? ((clan.Kingdom.Leader == Hero.MainHero) ? 0.5f : 0.75f) : 1f);
			DiplomacyModel diplomacyModel = Campaign.Current.Models.DiplomacyModel;
			AllianceModel allianceModel = Campaign.Current.Models.AllianceModel;
			if (randomFloat < num2 && clan.Influence > (float)diplomacyModel.GetInfluenceCostOfProposingPeace(clan))
			{
				kingdomDecision = this.GetRandomPeaceDecision(clan);
			}
			else if (randomFloat < num2 * 2f && clan.Influence > (float)diplomacyModel.GetInfluenceCostOfProposingWar(clan))
			{
				kingdomDecision = this.GetRandomWarDecision(clan);
			}
			else if (randomFloat < num2 * 2.5f)
			{
				kingdomDecision = ((MBRandom.RandomFloat < 0.5f) ? this.GetRandomTradeAgreementDecision(clan) : this.GetRandomStartingAllianceDecision(clan));
			}
			else if (randomFloat < num2 * 2.75f && clan.Influence > (float)(diplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan) * 4))
			{
				kingdomDecision = this.GetRandomPolicyDecision(clan);
			}
			else if (randomFloat < num2 * 3f && clan.Influence > 700f)
			{
				kingdomDecision = this.GetRandomAnnexationDecision(clan);
			}
			if (kingdomDecision != null)
			{
				bool flag = false;
				if (kingdomDecision is MakePeaceKingdomDecision && ((MakePeaceKingdomDecision)kingdomDecision).FactionToMakePeaceWith == Hero.MainHero.MapFaction)
				{
					foreach (KingdomDecision kingdomDecision2 in this._kingdomDecisionsList)
					{
						if (kingdomDecision2 is MakePeaceKingdomDecision && kingdomDecision2.Kingdom == Hero.MainHero.MapFaction && ((MakePeaceKingdomDecision)kingdomDecision2).FactionToMakePeaceWith == clan.Kingdom && kingdomDecision2.TriggerTime.IsFuture)
						{
							flag = true;
							break;
						}
						if (kingdomDecision2 is MakePeaceKingdomDecision && kingdomDecision2.Kingdom == clan.Kingdom && ((MakePeaceKingdomDecision)kingdomDecision2).FactionToMakePeaceWith == Hero.MainHero.MapFaction && kingdomDecision2.TriggerTime.IsFuture)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					bool flag2 = false;
					foreach (KingdomDecision kingdomDecision3 in this._kingdomDecisionsList)
					{
						DeclareWarDecision declareWarDecision;
						DeclareWarDecision declareWarDecision2;
						if ((declareWarDecision = kingdomDecision3 as DeclareWarDecision) != null && (declareWarDecision2 = kingdomDecision as DeclareWarDecision) != null && declareWarDecision.FactionToDeclareWarOn == declareWarDecision2.FactionToDeclareWarOn && declareWarDecision.ProposerClan.MapFaction == declareWarDecision2.ProposerClan.MapFaction)
						{
							flag2 = true;
							break;
						}
						MakePeaceKingdomDecision makePeaceKingdomDecision;
						MakePeaceKingdomDecision makePeaceKingdomDecision2;
						if ((makePeaceKingdomDecision = kingdomDecision3 as MakePeaceKingdomDecision) != null && (makePeaceKingdomDecision2 = kingdomDecision as MakePeaceKingdomDecision) != null && makePeaceKingdomDecision.FactionToMakePeaceWith == makePeaceKingdomDecision2.FactionToMakePeaceWith && makePeaceKingdomDecision.ProposerClan.MapFaction == makePeaceKingdomDecision2.ProposerClan.MapFaction)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						clan.Kingdom.AddDecision(kingdomDecision, false);
						return;
					}
				}
			}
			else
			{
				this.UpdateKingdomDecisions(clan.Kingdom);
			}
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x00140D00 File Offset: 0x0013EF00
		private void HourlyTick()
		{
			if (Clan.PlayerClan.Kingdom != null)
			{
				this.UpdateKingdomDecisions(Clan.PlayerClan.Kingdom);
			}
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x00140D20 File Offset: 0x0013EF20
		private void DailyTick()
		{
			for (int i = this._kingdomDecisionsList.Count - 1; i >= 0; i--)
			{
				if (this._kingdomDecisionsList[i].TriggerTime.ElapsedDaysUntilNow > 5f)
				{
					this._kingdomDecisionsList.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x00140D74 File Offset: 0x0013EF74
		public void UpdateKingdomDecisions(Kingdom kingdom)
		{
			List<KingdomDecision> list = new List<KingdomDecision>();
			List<KingdomDecision> list2 = new List<KingdomDecision>();
			foreach (KingdomDecision kingdomDecision in kingdom.UnresolvedDecisions)
			{
				if (kingdomDecision.ShouldBeCancelled())
				{
					list.Add(kingdomDecision);
				}
				else if (!kingdomDecision.IsPlayerParticipant || (kingdomDecision.TriggerTime.IsPast && !kingdomDecision.NeedsPlayerResolution))
				{
					list2.Add(kingdomDecision);
				}
			}
			foreach (KingdomDecision kingdomDecision2 in list)
			{
				kingdom.RemoveDecision(kingdomDecision2);
				bool flag;
				if (!kingdomDecision2.DetermineChooser().Leader.IsHumanPlayerCharacter)
				{
					flag = kingdomDecision2.DetermineSupporters().Any<Supporter>((Supporter x) => x.IsPlayer);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				CampaignEventDispatcher.Instance.OnKingdomDecisionCancelled(kingdomDecision2, flag2);
			}
			foreach (KingdomDecision kingdomDecision3 in list2)
			{
				new KingdomElection(kingdomDecision3).StartElectionWithoutPlayer();
			}
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x00140ED4 File Offset: 0x0013F0D4
		private void OnPeaceMade(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			this.HandleDiplomaticChangeBetweenFactions(side1Faction, side2Faction);
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00140EDE File Offset: 0x0013F0DE
		private void OnWarDeclared(IFaction side1Faction, IFaction side2Faction, DeclareWarAction.DeclareWarDetail detail)
		{
			this.HandleDiplomaticChangeBetweenFactions(side1Faction, side2Faction);
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x00140EE8 File Offset: 0x0013F0E8
		private void HandleDiplomaticChangeBetweenFactions(IFaction side1Faction, IFaction side2Faction)
		{
			if (side1Faction.IsKingdomFaction && side2Faction.IsKingdomFaction)
			{
				this.UpdateKingdomDecisions((Kingdom)side1Faction);
				this.UpdateKingdomDecisions((Kingdom)side2Faction);
			}
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x00140F14 File Offset: 0x0013F114
		private KingdomDecision GetRandomStartingAllianceDecision(Clan clan)
		{
			Kingdom kingdom = clan.Kingdom;
			KingdomDecision kingdomDecision = null;
			if (kingdom.UnresolvedDecisions.AnyQ<KingdomDecision>((KingdomDecision x) => x is StartAllianceDecision) || clan.Influence < (float)Campaign.Current.Models.AllianceModel.GetInfluenceCostOfProposingStartingAlliance(clan))
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => !x.IsEliminated && x != kingdom);
			if (randomElementWithPredicate != null)
			{
				kingdomDecision = new StartAllianceDecision(clan, randomElementWithPredicate);
				TextObject textObject;
				if (!kingdomDecision.CanMakeDecision(out textObject, false))
				{
					kingdomDecision = null;
				}
			}
			return kingdomDecision;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x00140FB8 File Offset: 0x0013F1B8
		private KingdomDecision GetRandomWarDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is DeclareWarDecision) != null)
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => !x.IsEliminated && x != kingdom && !x.IsAtWarWith(kingdom) && x.GetStanceWith(kingdom).PeaceDeclarationDate.ElapsedDaysUntilNow > 20f);
			if (randomElementWithPredicate != null)
			{
				if ((float)new DeclareWarBarterable(kingdom, randomElementWithPredicate).GetValueForFaction(clan) < Campaign.Current.Models.DiplomacyModel.GetDecisionMakingThreshold(randomElementWithPredicate))
				{
					return null;
				}
				if (this.ConsiderWar(clan, kingdom, randomElementWithPredicate))
				{
					kingdomDecision = new DeclareWarDecision(clan, randomElementWithPredicate);
				}
			}
			return kingdomDecision;
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x0014106C File Offset: 0x0013F26C
		private KingdomDecision GetRandomPeaceDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is MakePeaceKingdomDecision) != null)
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>(delegate(Kingdom x)
			{
				if (x.IsAtWarWith(kingdom) && !x.IsAtConstantWarWith(kingdom))
				{
					IAllianceCampaignBehavior allianceCampaignBehavior = this.AllianceCampaignBehavior;
					Kingdom kingdom2;
					if (allianceCampaignBehavior == null || !allianceCampaignBehavior.IsAtWarByCallToWarAgreement(kingdom, x, out kingdom2))
					{
						IAllianceCampaignBehavior allianceCampaignBehavior2 = this.AllianceCampaignBehavior;
						return allianceCampaignBehavior2 == null || !allianceCampaignBehavior2.IsAtWarByCallToWarAgreement(x, kingdom, out kingdom2);
					}
				}
				return false;
			});
			MakePeaceKingdomDecision makePeaceKingdomDecision;
			if (randomElementWithPredicate != null && KingdomDecisionProposalBehavior.ConsiderPeace(clan, randomElementWithPredicate.RulingClan, randomElementWithPredicate, out makePeaceKingdomDecision))
			{
				kingdomDecision = makePeaceKingdomDecision;
			}
			return kingdomDecision;
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x001410F8 File Offset: 0x0013F2F8
		private bool ConsiderWar(Clan clan, Kingdom kingdom, IFaction otherFaction)
		{
			int num = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingWar(clan) / 2;
			if (clan.Influence < (float)num)
			{
				return false;
			}
			DeclareWarDecision declareWarDecision = new DeclareWarDecision(clan, otherFaction);
			if (declareWarDecision.CalculateSupport(clan) > 50f)
			{
				KingdomElection kingdomElection = new KingdomElection(declareWarDecision);
				float num2 = 0f;
				using (List<DecisionOutcome>.Enumerator enumerator = kingdomElection.PossibleOutcomes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DeclareWarDecision.DeclareWarDecisionOutcome declareWarDecisionOutcome;
						if ((declareWarDecisionOutcome = enumerator.Current as DeclareWarDecision.DeclareWarDecisionOutcome) != null && declareWarDecisionOutcome.ShouldWarBeDeclared)
						{
							num2 = declareWarDecisionOutcome.Likelihood;
							break;
						}
					}
				}
				if (MBRandom.RandomFloat < 1.4f * num2 - 0.55f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x001411BC File Offset: 0x0013F3BC
		private static bool ConsiderPeace(Clan clan, Clan otherClan, IFaction otherFaction, out MakePeaceKingdomDecision decision)
		{
			if (!Campaign.Current.Models.DiplomacyModel.IsPeaceSuitable(clan.MapFaction, otherFaction))
			{
				decision = null;
				return false;
			}
			if (Campaign.Current.Models.DiplomacyModel.GetScoreOfDeclaringPeace(clan.MapFaction, otherFaction) < Campaign.Current.Models.DiplomacyModel.GetDecisionMakingThreshold(clan.Kingdom))
			{
				decision = null;
				return false;
			}
			int num;
			int dailyTributeToPay = Campaign.Current.Models.DiplomacyModel.GetDailyTributeToPay(clan, otherClan, out num);
			if (dailyTributeToPay < 0)
			{
				decision = null;
				return false;
			}
			MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(clan, otherFaction, dailyTributeToPay, num, true, false);
			DecisionOutcome decisionOutcome = makePeaceKingdomDecision.DetermineInitialCandidates().First<DecisionOutcome>(delegate(DecisionOutcome x)
			{
				MakePeaceKingdomDecision.MakePeaceDecisionOutcome makePeaceDecisionOutcome;
				return (makePeaceDecisionOutcome = x as MakePeaceKingdomDecision.MakePeaceDecisionOutcome) != null && makePeaceDecisionOutcome.ShouldPeaceBeDeclared;
			});
			if (makePeaceKingdomDecision.DetermineSupport(clan, decisionOutcome) <= 0f)
			{
				decision = null;
				return false;
			}
			decision = makePeaceKingdomDecision;
			return true;
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x00141298 File Offset: 0x0013F498
		private KingdomDecision GetRandomPolicyDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is KingdomPolicyDecision) != null)
			{
				return null;
			}
			if (clan.Influence < 200f)
			{
				return null;
			}
			PolicyObject randomElement = PolicyObject.All.GetRandomElement<PolicyObject>();
			bool flag = kingdom.ActivePolicies.Contains(randomElement);
			if (this.ConsiderPolicy(clan, kingdom, randomElement, flag))
			{
				kingdomDecision = new KingdomPolicyDecision(clan, randomElement, flag);
			}
			return kingdomDecision;
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0014131C File Offset: 0x0013F51C
		private bool ConsiderPolicy(Clan clan, Kingdom kingdom, PolicyObject policy, bool invert)
		{
			int influenceCostOfPolicyProposalAndDisavowal = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan);
			if (clan.Influence < (float)influenceCostOfPolicyProposalAndDisavowal)
			{
				return false;
			}
			KingdomPolicyDecision kingdomPolicyDecision = new KingdomPolicyDecision(clan, policy, invert);
			if (kingdomPolicyDecision.CalculateSupport(clan) > 50f)
			{
				KingdomElection kingdomElection = new KingdomElection(kingdomPolicyDecision);
				float num = 0f;
				using (List<DecisionOutcome>.Enumerator enumerator = kingdomElection.PossibleOutcomes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KingdomPolicyDecision.PolicyDecisionOutcome policyDecisionOutcome;
						if ((policyDecisionOutcome = enumerator.Current as KingdomPolicyDecision.PolicyDecisionOutcome) != null && policyDecisionOutcome.ShouldDecisionBeEnforced)
						{
							num = policyDecisionOutcome.Likelihood;
							break;
						}
					}
				}
				if ((double)MBRandom.RandomFloat < (double)num - 0.55)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x001413E0 File Offset: 0x0013F5E0
		private float GetKingdomSupportForPolicy(Clan clan, Kingdom kingdom, PolicyObject policy, bool invert)
		{
			Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan);
			return new KingdomElection(new KingdomPolicyDecision(clan, policy, invert)).GetLikelihoodForSponsor(clan);
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0014140C File Offset: 0x0013F60C
		private KingdomDecision GetRandomAnnexationDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is KingdomPolicyDecision) != null)
			{
				return null;
			}
			if (clan.Influence < 300f)
			{
				return null;
			}
			Clan randomElement = kingdom.Clans.GetRandomElement<Clan>();
			if (randomElement != null && randomElement != clan && randomElement.GetRelationWithClan(clan) < -25)
			{
				if (randomElement.Fiefs.Count == 0)
				{
					return null;
				}
				Town randomElement2 = randomElement.Fiefs.GetRandomElement<Town>();
				if (this.ConsiderAnnex(clan, randomElement2))
				{
					kingdomDecision = new SettlementClaimantPreliminaryDecision(clan, randomElement2.Settlement);
				}
			}
			return kingdomDecision;
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x001414B0 File Offset: 0x0013F6B0
		private bool ConsiderAnnex(Clan clan, Town targetSettlement)
		{
			int influenceCostOfAnnexation = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAnnexation(clan);
			if (clan.Influence < (float)influenceCostOfAnnexation)
			{
				return false;
			}
			SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision = new SettlementClaimantPreliminaryDecision(clan, targetSettlement.Settlement);
			if (settlementClaimantPreliminaryDecision.CalculateSupport(clan) > 50f)
			{
				float num = 0f;
				using (List<DecisionOutcome>.Enumerator enumerator = new KingdomElection(settlementClaimantPreliminaryDecision).PossibleOutcomes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SettlementClaimantPreliminaryDecision.SettlementClaimantPreliminaryOutcome settlementClaimantPreliminaryOutcome;
						if ((settlementClaimantPreliminaryOutcome = enumerator.Current as SettlementClaimantPreliminaryDecision.SettlementClaimantPreliminaryOutcome) != null && settlementClaimantPreliminaryOutcome.ShouldSettlementOwnerChange)
						{
							num = settlementClaimantPreliminaryOutcome.Likelihood;
							break;
						}
					}
				}
				if ((double)MBRandom.RandomFloat < (double)num - 0.6)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x00141578 File Offset: 0x0013F778
		private KingdomDecision GetRandomTradeAgreementDecision(Clan clan)
		{
			KingdomDecision kingdomDecision = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>((KingdomDecision x) => x is TradeAgreementDecision) != null || clan.Influence < (float)Campaign.Current.Models.TradeAgreementModel.GetInfluenceCostOfProposingTradeAgreement(clan))
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate<Kingdom>((Kingdom x) => x != kingdom);
			if (randomElementWithPredicate != null && this.ConsiderTradeAgreement(clan, kingdom, randomElementWithPredicate))
			{
				kingdomDecision = new TradeAgreementDecision(clan, randomElementWithPredicate);
			}
			return kingdomDecision;
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x00141620 File Offset: 0x0013F820
		private bool ConsiderTradeAgreement(Clan clan, Kingdom kingdom, Kingdom otherKingdom)
		{
			TextObject textObject;
			if (!Campaign.Current.Models.TradeAgreementModel.CanMakeTradeAgreement(kingdom, otherKingdom, true, out textObject, false))
			{
				return false;
			}
			TradeAgreementDecision tradeAgreementDecision = new TradeAgreementDecision(clan, otherKingdom);
			if (kingdom != Clan.PlayerClan.Kingdom)
			{
				return tradeAgreementDecision.CalculateSupport(clan, out textObject) > 50f;
			}
			KingdomElection kingdomElection = new KingdomElection(tradeAgreementDecision);
			kingdomElection.SetupResultWithoutPlayerSupport();
			DecisionOutcome decisionOutcome = kingdomElection.PossibleOutcomes.FirstOrDefault<DecisionOutcome>(delegate(DecisionOutcome x)
			{
				TradeAgreementDecision.TradeAgreementDecisionOutcome tradeAgreementDecisionOutcome;
				return (tradeAgreementDecisionOutcome = x as TradeAgreementDecision.TradeAgreementDecisionOutcome) != null && tradeAgreementDecisionOutcome.ShouldTradeAgreementStart;
			});
			return kingdomElection.GetWinChanceWithPlayerSupport(decisionOutcome, Supporter.SupportWeights.FullyPush) > 0.5f;
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x001416B8 File Offset: 0x0013F8B8
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<KingdomDecision>>("_kingdomDecisionsList", ref this._kingdomDecisionsList);
			if (dataStore.IsLoading && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0)) && this._kingdomDecisionsList == null)
			{
				this._kingdomDecisionsList = new List<KingdomDecision>();
			}
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x0014170C File Offset: 0x0013F90C
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan && oldKingdom != null && detail != ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction)
			{
				this.UpdateKingdomDecisions(oldKingdom);
			}
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x00141725 File Offset: 0x0013F925
		private void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
			this._kingdomDecisionsList.Add(decision);
		}

		// Token: 0x040013F5 RID: 5109
		private const float DaysBetweenSameProposal = 5f;

		// Token: 0x040013F6 RID: 5110
		private List<KingdomDecision> _kingdomDecisionsList = new List<KingdomDecision>();

		// Token: 0x040013F7 RID: 5111
		private ITradeAgreementsCampaignBehavior _tradeAgreementsBehavior;

		// Token: 0x040013F8 RID: 5112
		private IAllianceCampaignBehavior _allianceCampaignBehavior;

		// Token: 0x02000863 RID: 2147
		// (Invoke) Token: 0x06006A3B RID: 27195
		private delegate KingdomDecision KingdomDecisionCreatorDelegate(Clan sponsorClan);
	}
}
