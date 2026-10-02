using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000075 RID: 117
	public class KingdomDiplomacyVM : KingdomCategoryVM
	{
		// Token: 0x0600090E RID: 2318 RVA: 0x00028708 File Offset: 0x00026908
		public KingdomDiplomacyVM(Action<KingdomDecision> forceDecision)
		{
			this._forceDecision = forceDecision;
			this._playerKingdom = Hero.MainHero.MapFaction as Kingdom;
			this.PlayerWars = new MBBindingList<KingdomWarItemVM>();
			this.PlayerTruces = new MBBindingList<KingdomTruceItemVM>();
			this.WarsSortController = new KingdomWarSortControllerVM(ref this._playerWars);
			this.Actions = new MBBindingList<KingdomDiplomacyProposalActionItemVM>();
			this.ExecuteShowStatComparisons();
			this.RefreshValues();
			this.SetDefaultSelectedItem();
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0002877C File Offset: 0x0002697C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.BehaviorSelection = new SelectorVM<SelectorItemVM>(0, new Action<SelectorVM<SelectorItemVM>>(this.OnBehaviorSelectionChanged));
			this.BehaviorSelection.AddItem(new SelectorItemVM(GameTexts.FindText("str_kingdom_war_strategy_balanced", null), GameTexts.FindText("str_kingdom_war_strategy_balanced_desc", null)));
			this.BehaviorSelection.AddItem(new SelectorItemVM(GameTexts.FindText("str_kingdom_war_strategy_defensive", null), GameTexts.FindText("str_kingdom_war_strategy_defensive_desc", null)));
			this.BehaviorSelection.AddItem(new SelectorItemVM(GameTexts.FindText("str_kingdom_war_strategy_offensive", null), GameTexts.FindText("str_kingdom_war_strategy_offensive_desc", null)));
			this.RefreshDiplomacyList();
			this.BehaviorSelectionTitle = GameTexts.FindText("str_kingdom_war_strategy", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_war_selected", null).ToString();
			this.PlayerWarsText = GameTexts.FindText("str_kingdom_at_war", null).ToString();
			this.PlayerTrucesText = GameTexts.FindText("str_kingdom_at_peace", null).ToString();
			this.WarsText = GameTexts.FindText("str_diplomatic_group", null).ToString();
			this.ShowStatBarsHint = new HintViewModel(GameTexts.FindText("str_kingdom_war_show_comparison_bars", null), null);
			this.ShowWarLogsHint = new HintViewModel(GameTexts.FindText("str_kingdom_war_show_war_logs", null), null);
			this.WarHint = new HintViewModel(GameTexts.FindText("str_kingdom_at_war", null), null);
			this.PeaceHint = new HintViewModel(GameTexts.FindText("str_kingdom_at_peace", null), null);
			this.TradeAgreementHint = new HintViewModel(GameTexts.FindText("str_kingdom_trade_agreement", null), null);
			this.AllianceHint = new HintViewModel(GameTexts.FindText("str_kingdom_alliance", null), null);
			this.PayingTributeHint = new HintViewModel(new TextObject("{=Jq8h4XAg}Paying Tribute to {PLAYER_KINGDOM}", null).SetTextVariable("PLAYER_KINGDOM", this._playerKingdom.Name), null);
			this.ReceivingTributeHint = new HintViewModel(new TextObject("{=UPpRGWae}Receiving Tribute from {PLAYER_KINGDOM}", null).SetTextVariable("PLAYER_KINGDOM", this._playerKingdom.Name), null);
			this.PlayerWars.ApplyActionOnAllItems(delegate(KingdomWarItemVM x)
			{
				x.RefreshValues();
			});
			this.PlayerTruces.ApplyActionOnAllItems(delegate(KingdomTruceItemVM x)
			{
				x.RefreshValues();
			});
			KingdomDiplomacyItemVM currentSelectedDiplomacyItem = this.CurrentSelectedDiplomacyItem;
			if (currentSelectedDiplomacyItem != null)
			{
				currentSelectedDiplomacyItem.RefreshValues();
			}
			this.Actions.ApplyActionOnAllItems(delegate(KingdomDiplomacyProposalActionItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00028A00 File Offset: 0x00026C00
		public void RefreshDiplomacyList()
		{
			Kingdom kingdom = Clan.PlayerClan.Kingdom;
			int num;
			if (kingdom == null)
			{
				num = 0;
			}
			else
			{
				num = kingdom.UnresolvedDecisions.Count<KingdomDecision>((KingdomDecision d) => !d.ShouldBeCancelled());
			}
			base.NotificationCount = num;
			this.PlayerWars.Clear();
			this.PlayerTruces.Clear();
			foreach (StanceLink stanceLink in from x in this._playerKingdom.FactionsAtWarWith
				select this._playerKingdom.GetStanceWith(x) into w
				orderby w.Faction1.Name.ToString() + w.Faction2.Name.ToString()
				select w)
			{
				if (stanceLink.Faction1.IsKingdomFaction && stanceLink.Faction2.IsKingdomFaction)
				{
					this.PlayerWars.Add(new KingdomWarItemVM(stanceLink, new Action<KingdomWarItemVM>(this.OnDiplomacyItemSelection)));
				}
			}
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (kingdom2 != this._playerKingdom && !kingdom2.IsEliminated && (DiplomacyHelper.IsSameFactionAndNotEliminated(kingdom2, this._playerKingdom) || FactionManager.IsNeutralWithFaction(kingdom2, this._playerKingdom)))
				{
					this.PlayerTruces.Add(new KingdomTruceItemVM(this._playerKingdom, kingdom2, new Action<KingdomDiplomacyItemVM>(this.OnDiplomacyItemSelection)));
				}
			}
			GameTexts.SetVariable("STR", this.PlayerWars.Count);
			this.NumOfPlayerWarsText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			GameTexts.SetVariable("STR", this.PlayerTruces.Count);
			this.NumOfPlayerTrucesText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			this.SetDefaultSelectedItem();
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00028BF4 File Offset: 0x00026DF4
		public void SelectKingdom(Kingdom kingdom)
		{
			bool flag = false;
			foreach (KingdomWarItemVM kingdomWarItemVM in this.PlayerWars)
			{
				if (kingdomWarItemVM.Faction1 == kingdom || kingdomWarItemVM.Faction2 == kingdom)
				{
					this.OnSetCurrentDiplomacyItem(kingdomWarItemVM);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (KingdomTruceItemVM kingdomTruceItemVM in this.PlayerTruces)
				{
					if (kingdomTruceItemVM.Faction1 == kingdom || kingdomTruceItemVM.Faction2 == kingdom)
					{
						this.OnSetCurrentDiplomacyItem(kingdomTruceItemVM);
						flag = true;
						break;
					}
				}
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00028CB4 File Offset: 0x00026EB4
		private void OnSetCurrentDiplomacyItem(KingdomDiplomacyItemVM item)
		{
			this.Actions.Clear();
			if (item is KingdomWarItemVM)
			{
				this.OnSetWarItem(item as KingdomWarItemVM);
			}
			else if (item is KingdomTruceItemVM)
			{
				this.OnSetPeaceItem(item as KingdomTruceItemVM);
			}
			this.RefreshCurrentWarVisuals(item);
			this.UpdateBehaviorSelection();
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00028D04 File Offset: 0x00026F04
		private void OnSetWarItem(KingdomWarItemVM item)
		{
			KingdomDiplomacyVM.<>c__DisplayClass9_0 CS$<>8__locals1 = new KingdomDiplomacyVM.<>c__DisplayClass9_0();
			CS$<>8__locals1.item = item;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.unresolvedPeaceDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				MakePeaceKingdomDecision makePeaceKingdomDecision;
				return (makePeaceKingdomDecision = d as MakePeaceKingdomDecision) != null && makePeaceKingdomDecision.FactionToMakePeaceWith == CS$<>8__locals1.item.Faction2 && !d.ShouldBeCancelled();
			});
			if (CS$<>8__locals1.unresolvedPeaceDecision != null)
			{
				TextObject textObject;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM(GameTexts.FindText("str_resolve", null), GameTexts.FindText("str_resolve_explanation", null), 0, this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.Peace, 0f, out textObject), textObject, delegate
				{
					CS$<>8__locals1.<>4__this._forceDecision(CS$<>8__locals1.unresolvedPeaceDecision);
				}));
				return;
			}
			int durationInDays;
			int dailyPeaceTributeToPay = Campaign.Current.Models.DiplomacyModel.GetDailyTributeToPay(Clan.PlayerClan, CS$<>8__locals1.item.Faction2.Leader.Clan, out durationInDays);
			dailyPeaceTributeToPay = 10 * (dailyPeaceTributeToPay / 10);
			TextObject textObject2 = ((dailyPeaceTributeToPay == 0) ? GameTexts.FindText("str_propose_peace_explanation", null) : ((dailyPeaceTributeToPay > 0) ? GameTexts.FindText("str_propose_peace_explanation_pay_tribute", null) : GameTexts.FindText("str_propose_peace_explanation_get_tribute", null)));
			textObject2.SetTextVariable("SUPPORT", this.CalculatePeaceSupport(CS$<>8__locals1.item.Faction2, dailyPeaceTributeToPay, durationInDays)).SetTextVariable("TRIBUTE_AMOUNT", MathF.Abs(dailyPeaceTributeToPay)).SetTextVariable("TRIBUTE_DURATION", durationInDays);
			int influenceCostOfProposingPeace = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingPeace(Clan.PlayerClan);
			TextObject textObject3;
			this.Actions.Add(new KingdomDiplomacyProposalActionItemVM((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null) : GameTexts.FindText("str_policy_enact", null), textObject2, influenceCostOfProposingPeace, this.GetIsProposingPeaceEnabledWithReason(CS$<>8__locals1.item, (float)influenceCostOfProposingPeace, out textObject3), textObject3, delegate
			{
				CS$<>8__locals1.<>4__this.OnDeclarePeace(CS$<>8__locals1.item, dailyPeaceTributeToPay, durationInDays);
			}));
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00028EFC File Offset: 0x000270FC
		private void OnSetPeaceItem(KingdomTruceItemVM item)
		{
			KingdomDecision unresolvedAllianceDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				StartAllianceDecision startAllianceDecision;
				return (startAllianceDecision = d as StartAllianceDecision) != null && startAllianceDecision.KingdomToStartAllianceWith == item.Faction2 && !d.ShouldBeCancelled();
			});
			if (unresolvedAllianceDecision != null)
			{
				TextObject textObject;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM(GameTexts.FindText("str_resolve", null), GameTexts.FindText("str_resolve_explanation", null), 0, this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.Alliance, 0f, out textObject), textObject, delegate
				{
					this._forceDecision(unresolvedAllianceDecision);
				}));
			}
			else if (!DiplomacyHelper.HasAllianceWithFaction(item.Faction1, item.Faction2))
			{
				int influenceCostOfProposingStartingAlliance = Campaign.Current.Models.AllianceModel.GetInfluenceCostOfProposingStartingAlliance(Clan.PlayerClan);
				TextObject textObject2;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null) : GameTexts.FindText("str_policy_enact", null), GameTexts.FindText("str_propose_alliance_explanation", null).SetTextVariable("SUPPORT", this.CalculateAllianceSupport(item.Faction2)), influenceCostOfProposingStartingAlliance, this.GetIsProposingAllianceEnabledWithReason(item, (float)influenceCostOfProposingStartingAlliance, out textObject2), textObject2, delegate
				{
					this.OnStartAlliance(item);
				}));
			}
			KingdomDecision unresolvedWarDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				DeclareWarDecision declareWarDecision;
				return (declareWarDecision = d as DeclareWarDecision) != null && declareWarDecision.FactionToDeclareWarOn == item.Faction2 && !d.ShouldBeCancelled();
			});
			if (unresolvedWarDecision != null)
			{
				TextObject textObject3;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM(GameTexts.FindText("str_resolve", null), GameTexts.FindText("str_resolve_explanation", null), 0, this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.War, 0f, out textObject3), textObject3, delegate
				{
					this._forceDecision(unresolvedWarDecision);
				}));
			}
			else
			{
				int influenceCostOfProposingWar = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingWar(Clan.PlayerClan);
				TextObject textObject4;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null) : GameTexts.FindText("str_policy_enact", null), GameTexts.FindText("str_propose_war_explanation", null).SetTextVariable("SUPPORT", this.CalculateWarSupport(item.Faction2)), influenceCostOfProposingWar, this.GetIsProposingWarEnabledWithReason(item, (float)influenceCostOfProposingWar, out textObject4), textObject4, delegate
				{
					this.OnDeclareWar(item);
				}));
			}
			KingdomDecision unresolvedTradeAgreementDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				TradeAgreementDecision tradeAgreementDecision;
				return (tradeAgreementDecision = d as TradeAgreementDecision) != null && tradeAgreementDecision.TargetKingdom == item.Faction2 && !d.ShouldBeCancelled();
			});
			if (unresolvedTradeAgreementDecision != null)
			{
				TextObject textObject5;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM(GameTexts.FindText("str_resolve", null), GameTexts.FindText("str_resolve_explanation", null), 0, this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.TradeAgreement, 0f, out textObject5), textObject5, delegate
				{
					this._forceDecision(unresolvedTradeAgreementDecision);
				}));
				return;
			}
			ITradeAgreementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement;
			if (campaignBehavior != null && !campaignBehavior.HasTradeAgreement(item.Faction1 as Kingdom, item.Faction2 as Kingdom, out tradeAgreement))
			{
				int influenceCostOfProposingTradeAgreement = Campaign.Current.Models.TradeAgreementModel.GetInfluenceCostOfProposingTradeAgreement(Clan.PlayerClan);
				TextObject textObject6;
				this.Actions.Add(new KingdomDiplomacyProposalActionItemVM((this._playerKingdom.Clans.Count > 1) ? GameTexts.FindText("str_policy_propose", null) : GameTexts.FindText("str_policy_enact", null), GameTexts.FindText("str_propose_trade_agreement_explanation", null).SetTextVariable("SUPPORT", this.CalculateTradeAgreementSupport(item.Faction2)), influenceCostOfProposingTradeAgreement, this.GetIsProposingTradeAgreementEnabledWithReason(item, (float)influenceCostOfProposingTradeAgreement, out textObject6), textObject6, delegate
				{
					this.OnStartTradeAgreement(item);
				}));
			}
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x000292B0 File Offset: 0x000274B0
		private bool GetIsProposingWarEnabledWithReason(KingdomTruceItemVM item, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.War, actionInfluenceCost, out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			TextObject textObject2;
			if (!Campaign.Current.Models.KingdomDecisionPermissionModel.IsWarDecisionAllowedBetweenKingdoms(item.Faction1 as Kingdom, item.Faction2 as Kingdom, out textObject2))
			{
				disabledReason = textObject2;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002930C File Offset: 0x0002750C
		private bool GetIsProposingPeaceEnabledWithReason(KingdomWarItemVM item, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.Peace, actionInfluenceCost, out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			TextObject textObject2;
			if (!Campaign.Current.Models.KingdomDecisionPermissionModel.IsPeaceDecisionAllowedBetweenKingdoms(item.Faction1 as Kingdom, item.Faction2 as Kingdom, out textObject2))
			{
				disabledReason = textObject2;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00029368 File Offset: 0x00027568
		private bool GetIsProposingAllianceEnabledWithReason(KingdomTruceItemVM item, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.Alliance, actionInfluenceCost, out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			TextObject textObject2;
			if (!new StartAllianceDecision(Clan.PlayerClan, item.Faction2 as Kingdom).CanMakeDecision(out textObject2, true))
			{
				disabledReason = textObject2;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000293B4 File Offset: 0x000275B4
		private bool GetIsProposingTradeAgreementEnabledWithReason(KingdomTruceItemVM item, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!this.GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType.TradeAgreement, actionInfluenceCost, out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			TextObject textObject2;
			if (!new TradeAgreementDecision(Clan.PlayerClan, item.Faction2 as Kingdom).CanMakeDecision(out textObject2, true))
			{
				disabledReason = textObject2;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00029400 File Offset: 0x00027600
		private bool GetAreProposalActionsEnabledWithReason(KingdomDiplomacyVM.DiplomacyItemType diplomacyItemType, float actionInfluenceCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				switch (diplomacyItemType)
				{
				case KingdomDiplomacyVM.DiplomacyItemType.War:
				case KingdomDiplomacyVM.DiplomacyItemType.Peace:
					disabledReason = GameTexts.FindText("str_cannot_propose_war_truce_while_mercenary", null);
					return false;
				case KingdomDiplomacyVM.DiplomacyItemType.Alliance:
					disabledReason = GameTexts.FindText("str_cannot_propose_alliance_while_mercenary", null);
					return false;
				case KingdomDiplomacyVM.DiplomacyItemType.TradeAgreement:
					disabledReason = GameTexts.FindText("str_cannot_propose_trade_agreement_while_mercenary", null);
					return false;
				}
				disabledReason = TextObject.GetEmpty();
				return false;
			}
			if (actionInfluenceCost > 0f && Clan.PlayerClan.Influence < actionInfluenceCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x000294A5 File Offset: 0x000276A5
		private void RefreshCurrentWarVisuals(KingdomDiplomacyItemVM item)
		{
			if (item != null)
			{
				if (this.CurrentSelectedDiplomacyItem != null)
				{
					this.CurrentSelectedDiplomacyItem.IsSelected = false;
				}
				this.CurrentSelectedDiplomacyItem = item;
				if (this.CurrentSelectedDiplomacyItem != null)
				{
					this.CurrentSelectedDiplomacyItem.IsSelected = true;
				}
			}
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x000294D9 File Offset: 0x000276D9
		private void OnDiplomacyItemSelection(KingdomDiplomacyItemVM item)
		{
			if (this.CurrentSelectedDiplomacyItem != item)
			{
				if (this.CurrentSelectedDiplomacyItem != null)
				{
					this.CurrentSelectedDiplomacyItem.IsSelected = false;
				}
				this.CurrentSelectedDiplomacyItem = item;
				base.IsAcceptableItemSelected = item != null;
				this.OnSetCurrentDiplomacyItem(item);
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00029510 File Offset: 0x00027710
		private void OnDeclareWar(KingdomTruceItemVM item)
		{
			DeclareWarDecision declareWarDecision = new DeclareWarDecision(Clan.PlayerClan, item.Faction2);
			Clan.PlayerClan.Kingdom.AddDecision(declareWarDecision, false);
			this._forceDecision(declareWarDecision);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0002954C File Offset: 0x0002774C
		private void OnDeclarePeace(KingdomWarItemVM item, int tributeToPay, int tributeDurationInDays)
		{
			MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(Clan.PlayerClan, item.Faction2 as Kingdom, tributeToPay, tributeDurationInDays, true, false);
			Clan.PlayerClan.Kingdom.AddDecision(makePeaceKingdomDecision, false);
			this._forceDecision(makePeaceKingdomDecision);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00029590 File Offset: 0x00027790
		private void OnStartAlliance(KingdomTruceItemVM item)
		{
			if (item.Faction2.IsKingdomFaction)
			{
				StartAllianceDecision startAllianceDecision = new StartAllianceDecision(Clan.PlayerClan, (Kingdom)item.Faction2);
				Clan.PlayerClan.Kingdom.AddDecision(startAllianceDecision, false);
				this._forceDecision(startAllianceDecision);
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x000295E0 File Offset: 0x000277E0
		private void OnStartTradeAgreement(KingdomTruceItemVM item)
		{
			if (item.Faction2.IsKingdomFaction)
			{
				TradeAgreementDecision tradeAgreementDecision = new TradeAgreementDecision(Clan.PlayerClan, (Kingdom)item.Faction2);
				Clan.PlayerClan.Kingdom.AddDecision(tradeAgreementDecision, false);
				this._forceDecision(tradeAgreementDecision);
			}
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0002962D File Offset: 0x0002782D
		private void ExecuteShowWarLogs()
		{
			this.IsDisplayingWarLogs = true;
			this.IsDisplayingStatComparisons = false;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0002963D File Offset: 0x0002783D
		private void ExecuteShowStatComparisons()
		{
			this.IsDisplayingWarLogs = false;
			this.IsDisplayingStatComparisons = true;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00029650 File Offset: 0x00027850
		private void SetDefaultSelectedItem()
		{
			KingdomDiplomacyItemVM kingdomDiplomacyItemVM = this.PlayerWars.FirstOrDefault<KingdomWarItemVM>();
			KingdomDiplomacyItemVM kingdomDiplomacyItemVM2 = this.PlayerTruces.FirstOrDefault<KingdomTruceItemVM>();
			this.OnDiplomacyItemSelection(kingdomDiplomacyItemVM ?? kingdomDiplomacyItemVM2);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00029684 File Offset: 0x00027884
		private void UpdateBehaviorSelection()
		{
			if (Hero.MainHero.MapFaction.IsKingdomFaction && Hero.MainHero.MapFaction.Leader == Hero.MainHero && this.CurrentSelectedDiplomacyItem != null)
			{
				StanceLink stanceWith = Hero.MainHero.MapFaction.GetStanceWith(this.CurrentSelectedDiplomacyItem.Faction2);
				this.BehaviorSelection.SelectedIndex = stanceWith.BehaviorPriority;
			}
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x000296EC File Offset: 0x000278EC
		private void OnBehaviorSelectionChanged(SelectorVM<SelectorItemVM> s)
		{
			if (!this._isChangingDiplomacyItem && Hero.MainHero.MapFaction.IsKingdomFaction && Hero.MainHero.MapFaction.Leader == Hero.MainHero && this.CurrentSelectedDiplomacyItem != null)
			{
				Hero.MainHero.MapFaction.GetStanceWith(this.CurrentSelectedDiplomacyItem.Faction2).BehaviorPriority = s.SelectedIndex;
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00029758 File Offset: 0x00027958
		private TextObject CalculateWarSupport(IFaction faction)
		{
			DeclareWarDecision declareWarDecision = new DeclareWarDecision(Clan.PlayerClan, faction);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(declareWarDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00029794 File Offset: 0x00027994
		private TextObject CalculateAllianceSupport(IFaction faction)
		{
			StartAllianceDecision startAllianceDecision = new StartAllianceDecision(Clan.PlayerClan, faction as Kingdom);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(startAllianceDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x000297D8 File Offset: 0x000279D8
		private TextObject CalculatePeaceSupport(IFaction faction, int dailyTributeToBePaid, int durationInDays)
		{
			MakePeaceKingdomDecision makePeaceKingdomDecision = new MakePeaceKingdomDecision(Clan.PlayerClan, faction, dailyTributeToBePaid, durationInDays, true, false);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(makePeaceKingdomDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00029818 File Offset: 0x00027A18
		private TextObject CalculateTradeAgreementSupport(IFaction faction)
		{
			TradeAgreementDecision tradeAgreementDecision = new TradeAgreementDecision(Clan.PlayerClan, faction as Kingdom);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(tradeAgreementDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x00029859 File Offset: 0x00027A59
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x00029861 File Offset: 0x00027A61
		[DataSourceProperty]
		public MBBindingList<KingdomWarItemVM> PlayerWars
		{
			get
			{
				return this._playerWars;
			}
			set
			{
				if (value != this._playerWars)
				{
					this._playerWars = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomWarItemVM>>(value, "PlayerWars");
				}
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0002987F File Offset: 0x00027A7F
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x00029887 File Offset: 0x00027A87
		[DataSourceProperty]
		public bool IsDisplayingWarLogs
		{
			get
			{
				return this._isDisplayingWarLogs;
			}
			set
			{
				if (value != this._isDisplayingWarLogs)
				{
					this._isDisplayingWarLogs = value;
					base.OnPropertyChangedWithValue(value, "IsDisplayingWarLogs");
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x000298A5 File Offset: 0x00027AA5
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x000298AD File Offset: 0x00027AAD
		[DataSourceProperty]
		public bool IsDisplayingStatComparisons
		{
			get
			{
				return this._isDisplayingStatComparisons;
			}
			set
			{
				if (value != this._isDisplayingStatComparisons)
				{
					this._isDisplayingStatComparisons = value;
					base.OnPropertyChangedWithValue(value, "IsDisplayingStatComparisons");
				}
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x000298CB File Offset: 0x00027ACB
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x000298D3 File Offset: 0x00027AD3
		[DataSourceProperty]
		public bool IsWar
		{
			get
			{
				return this._isWar;
			}
			set
			{
				if (value != this._isWar)
				{
					this._isWar = value;
					if (!value)
					{
						this.ExecuteShowStatComparisons();
					}
					base.OnPropertyChangedWithValue(value, "IsWar");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x000298FA File Offset: 0x00027AFA
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x00029902 File Offset: 0x00027B02
		[DataSourceProperty]
		public string BehaviorSelectionTitle
		{
			get
			{
				return this._behaviorSelectionTitle;
			}
			set
			{
				if (value != this._behaviorSelectionTitle)
				{
					this._behaviorSelectionTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "BehaviorSelectionTitle");
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x00029925 File Offset: 0x00027B25
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x0002992D File Offset: 0x00027B2D
		[DataSourceProperty]
		public MBBindingList<KingdomTruceItemVM> PlayerTruces
		{
			get
			{
				return this._playerTruces;
			}
			set
			{
				if (value != this._playerTruces)
				{
					this._playerTruces = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomTruceItemVM>>(value, "PlayerTruces");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0002994B File Offset: 0x00027B4B
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x00029953 File Offset: 0x00027B53
		[DataSourceProperty]
		public KingdomDiplomacyItemVM CurrentSelectedDiplomacyItem
		{
			get
			{
				return this._currentSelectedItem;
			}
			set
			{
				if (value != this._currentSelectedItem)
				{
					this._isChangingDiplomacyItem = true;
					this._currentSelectedItem = value;
					this.IsWar = value is KingdomWarItemVM;
					base.OnPropertyChangedWithValue<KingdomDiplomacyItemVM>(value, "CurrentSelectedDiplomacyItem");
					this._isChangingDiplomacyItem = false;
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x0002998E File Offset: 0x00027B8E
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x00029996 File Offset: 0x00027B96
		[DataSourceProperty]
		public KingdomWarSortControllerVM WarsSortController
		{
			get
			{
				return this._warsSortController;
			}
			set
			{
				if (value != this._warsSortController)
				{
					this._warsSortController = value;
					base.OnPropertyChangedWithValue<KingdomWarSortControllerVM>(value, "WarsSortController");
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x000299B4 File Offset: 0x00027BB4
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x000299BC File Offset: 0x00027BBC
		[DataSourceProperty]
		public string PlayerWarsText
		{
			get
			{
				return this._playerWarsText;
			}
			set
			{
				if (value != this._playerWarsText)
				{
					this._playerWarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerWarsText");
				}
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x000299DF File Offset: 0x00027BDF
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x000299E7 File Offset: 0x00027BE7
		[DataSourceProperty]
		public string WarsText
		{
			get
			{
				return this._warsText;
			}
			set
			{
				if (value != this._warsText)
				{
					this._warsText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarsText");
				}
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x00029A0A File Offset: 0x00027C0A
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x00029A12 File Offset: 0x00027C12
		[DataSourceProperty]
		public string NumOfPlayerWarsText
		{
			get
			{
				return this._numOfPlayerWarsText;
			}
			set
			{
				if (value != this._numOfPlayerWarsText)
				{
					this._numOfPlayerWarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "NumOfPlayerWarsText");
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00029A35 File Offset: 0x00027C35
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x00029A3D File Offset: 0x00027C3D
		[DataSourceProperty]
		public string PlayerTrucesText
		{
			get
			{
				return this._otherWarsText;
			}
			set
			{
				if (value != this._otherWarsText)
				{
					this._otherWarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerTrucesText");
				}
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00029A60 File Offset: 0x00027C60
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x00029A68 File Offset: 0x00027C68
		[DataSourceProperty]
		public string NumOfPlayerTrucesText
		{
			get
			{
				return this._numOfOtherWarsText;
			}
			set
			{
				if (value != this._numOfOtherWarsText)
				{
					this._numOfOtherWarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "NumOfPlayerTrucesText");
				}
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00029A8B File Offset: 0x00027C8B
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x00029A93 File Offset: 0x00027C93
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> BehaviorSelection
		{
			get
			{
				return this._behaviorSelection;
			}
			set
			{
				if (value != this._behaviorSelection)
				{
					this._behaviorSelection = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "BehaviorSelection");
				}
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00029AB1 File Offset: 0x00027CB1
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x00029AB9 File Offset: 0x00027CB9
		[DataSourceProperty]
		public HintViewModel ShowStatBarsHint
		{
			get
			{
				return this._showStatBarsHint;
			}
			set
			{
				if (value != this._showStatBarsHint)
				{
					this._showStatBarsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShowStatBarsHint");
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x00029AD7 File Offset: 0x00027CD7
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x00029ADF File Offset: 0x00027CDF
		[DataSourceProperty]
		public HintViewModel ShowWarLogsHint
		{
			get
			{
				return this._showWarLogsHint;
			}
			set
			{
				if (value != this._showWarLogsHint)
				{
					this._showWarLogsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ShowWarLogsHint");
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00029AFD File Offset: 0x00027CFD
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x00029B05 File Offset: 0x00027D05
		[DataSourceProperty]
		public HintViewModel WarHint
		{
			get
			{
				return this._warHint;
			}
			set
			{
				if (value != this._warHint)
				{
					this._warHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "WarHint");
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00029B23 File Offset: 0x00027D23
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x00029B2B File Offset: 0x00027D2B
		[DataSourceProperty]
		public HintViewModel PeaceHint
		{
			get
			{
				return this._peaceHint;
			}
			set
			{
				if (value != this._peaceHint)
				{
					this._peaceHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PeaceHint");
				}
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00029B49 File Offset: 0x00027D49
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x00029B51 File Offset: 0x00027D51
		[DataSourceProperty]
		public HintViewModel TradeAgreementHint
		{
			get
			{
				return this._tradeAgreementHint;
			}
			set
			{
				if (value != this._tradeAgreementHint)
				{
					this._tradeAgreementHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TradeAgreementHint");
				}
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00029B6F File Offset: 0x00027D6F
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x00029B77 File Offset: 0x00027D77
		[DataSourceProperty]
		public HintViewModel AllianceHint
		{
			get
			{
				return this._allianceHint;
			}
			set
			{
				if (value != this._allianceHint)
				{
					this._allianceHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AllianceHint");
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00029B95 File Offset: 0x00027D95
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x00029B9D File Offset: 0x00027D9D
		[DataSourceProperty]
		public HintViewModel PayingTributeHint
		{
			get
			{
				return this._payingTributeHint;
			}
			set
			{
				if (value != this._payingTributeHint)
				{
					this._payingTributeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PayingTributeHint");
				}
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00029BBB File Offset: 0x00027DBB
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x00029BC3 File Offset: 0x00027DC3
		[DataSourceProperty]
		public HintViewModel ReceivingTributeHint
		{
			get
			{
				return this._receivingTributeHint;
			}
			set
			{
				if (value != this._receivingTributeHint)
				{
					this._receivingTributeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ReceivingTributeHint");
				}
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x00029BE1 File Offset: 0x00027DE1
		// (set) Token: 0x06000956 RID: 2390 RVA: 0x00029BE9 File Offset: 0x00027DE9
		[DataSourceProperty]
		public MBBindingList<KingdomDiplomacyProposalActionItemVM> Actions
		{
			get
			{
				return this._actions;
			}
			set
			{
				if (value != this._actions)
				{
					this._actions = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomDiplomacyProposalActionItemVM>>(value, "Actions");
				}
			}
		}

		// Token: 0x040003F5 RID: 1013
		private readonly Action<KingdomDecision> _forceDecision;

		// Token: 0x040003F6 RID: 1014
		private readonly Kingdom _playerKingdom;

		// Token: 0x040003F7 RID: 1015
		private bool _isChangingDiplomacyItem;

		// Token: 0x040003F8 RID: 1016
		private MBBindingList<KingdomWarItemVM> _playerWars;

		// Token: 0x040003F9 RID: 1017
		private MBBindingList<KingdomTruceItemVM> _playerTruces;

		// Token: 0x040003FA RID: 1018
		private KingdomWarSortControllerVM _warsSortController;

		// Token: 0x040003FB RID: 1019
		private KingdomDiplomacyItemVM _currentSelectedItem;

		// Token: 0x040003FC RID: 1020
		private SelectorVM<SelectorItemVM> _behaviorSelection;

		// Token: 0x040003FD RID: 1021
		private HintViewModel _showStatBarsHint;

		// Token: 0x040003FE RID: 1022
		private HintViewModel _showWarLogsHint;

		// Token: 0x040003FF RID: 1023
		private HintViewModel _warHint;

		// Token: 0x04000400 RID: 1024
		private HintViewModel _peaceHint;

		// Token: 0x04000401 RID: 1025
		private HintViewModel _tradeAgreementHint;

		// Token: 0x04000402 RID: 1026
		private HintViewModel _allianceHint;

		// Token: 0x04000403 RID: 1027
		private HintViewModel _payingTributeHint;

		// Token: 0x04000404 RID: 1028
		private HintViewModel _receivingTributeHint;

		// Token: 0x04000405 RID: 1029
		private string _playerWarsText;

		// Token: 0x04000406 RID: 1030
		private string _numOfPlayerWarsText;

		// Token: 0x04000407 RID: 1031
		private string _otherWarsText;

		// Token: 0x04000408 RID: 1032
		private string _numOfOtherWarsText;

		// Token: 0x04000409 RID: 1033
		private string _warsText;

		// Token: 0x0400040A RID: 1034
		private string _behaviorSelectionTitle;

		// Token: 0x0400040B RID: 1035
		private bool _isDisplayingWarLogs;

		// Token: 0x0400040C RID: 1036
		private bool _isDisplayingStatComparisons;

		// Token: 0x0400040D RID: 1037
		private bool _isWar;

		// Token: 0x0400040E RID: 1038
		private MBBindingList<KingdomDiplomacyProposalActionItemVM> _actions;

		// Token: 0x020001D8 RID: 472
		private enum DiplomacyItemType
		{
			// Token: 0x0400114F RID: 4431
			None,
			// Token: 0x04001150 RID: 4432
			War,
			// Token: 0x04001151 RID: 4433
			Peace,
			// Token: 0x04001152 RID: 4434
			Alliance,
			// Token: 0x04001153 RID: 4435
			TradeAgreement
		}
	}
}
