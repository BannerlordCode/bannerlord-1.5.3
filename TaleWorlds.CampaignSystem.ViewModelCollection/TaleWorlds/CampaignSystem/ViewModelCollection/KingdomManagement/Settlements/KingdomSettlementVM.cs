using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Settlements
{
	// Token: 0x0200006F RID: 111
	public class KingdomSettlementVM : KingdomCategoryVM
	{
		// Token: 0x06000853 RID: 2131 RVA: 0x0002624C File Offset: 0x0002444C
		public KingdomSettlementVM(Action<KingdomDecision> forceDecision, Action<Settlement> onGrantFief)
		{
			this._forceDecision = forceDecision;
			this._onGrantFief = onGrantFief;
			this._kingdom = Hero.MainHero.MapFaction as Kingdom;
			this.AnnexCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAnnexation(Clan.PlayerClan);
			this.AnnexHint = new HintViewModel();
			base.IsAcceptableItemSelected = false;
			this.Settlements = new MBBindingList<KingdomSettlementItemVM>();
			this.RefreshSettlementList();
			base.NotificationCount = 0;
			this.SettlementSortController = new KingdomSettlementSortControllerVM(this.Settlements);
			this.RefreshValues();
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x000262E2 File Offset: 0x000244E2
		protected virtual KingdomSettlementItemVM CreateSettlementItemVM(Settlement settlement, Action<KingdomSettlementItemVM> onSelect)
		{
			return new KingdomSettlementItemVM(settlement, onSelect);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000262EC File Offset: 0x000244EC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.OwnerText = GameTexts.FindText("str_owner", null).ToString();
			this.NameText = GameTexts.FindText("str_scoreboard_header", "name").ToString();
			this.TypeText = GameTexts.FindText("str_sort_by_type_label", null).ToString();
			this.ProsperityText = GameTexts.FindText("str_prosperity_abbr", null).ToString();
			this.FoodText = GameTexts.FindText("str_inventory_category_tooltip", "6").ToString();
			this.GarrisonText = GameTexts.FindText("str_map_tooltip_garrison", null).ToString();
			this.MilitiaText = GameTexts.FindText("str_militia", null).ToString();
			this.ClanText = GameTexts.FindText("str_clans", null).ToString();
			this.VillagesText = GameTexts.FindText("str_villages", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_settlement_selected", null).ToString();
			this.ProposeText = GameTexts.FindText("str_policy_propose", null).ToString();
			this.DefendersText = GameTexts.FindText("str_sort_by_defenders_label", null).ToString();
			base.CategoryNameText = new TextObject("{=qKUjgS6r}Settlement", null).ToString();
			this.Settlements.ApplyActionOnAllItems(delegate(KingdomSettlementItemVM x)
			{
				x.RefreshValues();
			});
			KingdomSettlementItemVM currentSelectedSettlement = this.CurrentSelectedSettlement;
			if (currentSelectedSettlement == null)
			{
				return;
			}
			currentSelectedSettlement.RefreshValues();
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00026460 File Offset: 0x00024660
		public void RefreshSettlementList()
		{
			this.Settlements.Clear();
			if (this._kingdom != null)
			{
				foreach (Settlement settlement in this._kingdom.Settlements.Where<Settlement>((Settlement S) => S.IsCastle || S.IsTown))
				{
					KingdomSettlementItemVM kingdomSettlementItemVM = this.CreateSettlementItemVM(settlement, new Action<KingdomSettlementItemVM>(this.OnSettlementSelection));
					this.Settlements.Add(kingdomSettlementItemVM);
				}
			}
			if (this.Settlements.Count > 0)
			{
				this.SetCurrentSelectedSettlement(this.Settlements.FirstOrDefault<KingdomSettlementItemVM>());
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00026524 File Offset: 0x00024724
		private void SetCurrentSelectedSettlement(KingdomSettlementItemVM settlementItem)
		{
			if (this.CurrentSelectedSettlement != settlementItem)
			{
				if (this.CurrentSelectedSettlement != null)
				{
					this.CurrentSelectedSettlement.IsSelected = false;
				}
				this.CurrentSelectedSettlement = settlementItem;
				this.CurrentSelectedSettlement.IsSelected = true;
				if (settlementItem != null)
				{
					this._currenItemsUnresolvedDecision = this.GetSettlementsAnyWaitingDecision(settlementItem.Settlement);
					if (this._currenItemsUnresolvedDecision != null)
					{
						base.IsAcceptableItemSelected = true;
						this.AnnexCost = 0;
						this.AnnexText = GameTexts.FindText("str_resolve", null).ToString();
						this.AnnexActionExplanationText = GameTexts.FindText("str_resolve_explanation", null).ToString();
						TextObject textObject;
						this.CanAnnexCurrentSettlement = this.GetCanAnnexSettlementWithReason(this.AnnexCost, out textObject);
						this.AnnexHint.HintText = (this.CanAnnexCurrentSettlement ? TextObject.GetEmpty() : textObject);
					}
					else if (settlementItem.Owner.Hero == Hero.MainHero)
					{
						if (Hero.MainHero.IsKingdomLeader)
						{
							this.AnnexActionExplanationText = new TextObject("{=G2h0V10w}Gift this settlement to a clan in your kingdom.", null).ToString();
							this.AnnexText = new TextObject("{=sffGeQ1g}Gift", null).ToString();
						}
						else
						{
							this.AnnexActionExplanationText = new TextObject("{=1UbocG5B}Denounce your rights and responsibilities from this fief by giving it back to the realm.", null).ToString();
							this.AnnexText = new TextObject("{=U3ksQXD3}Give Away", null).ToString();
						}
						if (Hero.MainHero.IsPrisoner)
						{
							this.CanAnnexCurrentSettlement = false;
							this.HasCost = true;
							this.AnnexHint.HintText = GameTexts.FindText("str_action_disabled_reason_prisoner", null);
						}
						else if (!Campaign.Current.Models.DiplomacyModel.CanSettlementBeGifted(this._currentSelectedSettlement.Settlement))
						{
							this.CanAnnexCurrentSettlement = false;
							this.HasCost = true;
							this.AnnexHint.HintText = GameTexts.FindText("str_cannot_annex_waiting_for_ruler_decision", null);
						}
						else if (PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement == null)
						{
							this.CanAnnexCurrentSettlement = false;
							this.HasCost = true;
							this.AnnexHint.HintText = GameTexts.FindText("str_action_disabled_reason_encounter", null);
						}
						else if (PlayerSiege.PlayerSiegeEvent != null)
						{
							this.CanAnnexCurrentSettlement = false;
							this.HasCost = true;
							this.AnnexHint.HintText = GameTexts.FindText("str_action_disabled_reason_siege", null);
						}
						else
						{
							this.CanAnnexCurrentSettlement = true;
							this.HasCost = false;
							this.AnnexHint.HintText = TextObject.GetEmpty();
						}
					}
					else
					{
						this.AnnexText = GameTexts.FindText("str_policy_propose", null).ToString();
						this.AnnexCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAnnexation(Clan.PlayerClan);
						this.AnnexActionExplanationText = GameTexts.FindText("str_annex_fief_action_explanation", null).SetTextVariable("SUPPORT", KingdomSettlementVM.CalculateLikelihood(settlementItem.Settlement)).ToString();
						TextObject textObject2;
						this.CanAnnexCurrentSettlement = this.GetCanAnnexSettlementWithReason(this.AnnexCost, out textObject2);
						this.AnnexHint.HintText = textObject2;
						this.HasCost = true;
					}
				}
				base.IsAcceptableItemSelected = this.CurrentSelectedSettlement != null;
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0002680C File Offset: 0x00024A0C
		private bool GetCanAnnexSettlementWithReason(int annexCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Hero.MainHero.Clan.Influence < (float)annexCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			if (this.CurrentSelectedSettlement.Settlement.OwnerClan == this._kingdom.RulingClan)
			{
				disabledReason = GameTexts.FindText("str_cannot_annex_ruling_clan_settlement", null);
				return false;
			}
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_cannot_annex_while_mercenary", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00026898 File Offset: 0x00024A98
		public void SelectSettlement(Settlement settlement)
		{
			foreach (KingdomSettlementItemVM kingdomSettlementItemVM in this.Settlements)
			{
				if (kingdomSettlementItemVM.Settlement == settlement)
				{
					this.OnSettlementSelection(kingdomSettlementItemVM);
					break;
				}
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000268F0 File Offset: 0x00024AF0
		private void OnSettlementSelection(KingdomSettlementItemVM settlement)
		{
			if (this._currentSelectedSettlement != settlement)
			{
				this.SetCurrentSelectedSettlement(settlement);
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00026904 File Offset: 0x00024B04
		private void ExecuteAnnex()
		{
			if (this._currentSelectedSettlement != null)
			{
				if (this._currenItemsUnresolvedDecision != null)
				{
					this._forceDecision(this._currenItemsUnresolvedDecision);
					return;
				}
				Settlement settlement = this._currentSelectedSettlement.Settlement;
				if (settlement.OwnerClan.Leader == Hero.MainHero)
				{
					this._onGrantFief(settlement);
					return;
				}
				if (Hero.MainHero.Clan.Influence >= (float)this.AnnexCost)
				{
					SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision = new SettlementClaimantPreliminaryDecision(Clan.PlayerClan, settlement);
					Clan.PlayerClan.Kingdom.AddDecision(settlementClaimantPreliminaryDecision, false);
					this._forceDecision(settlementClaimantPreliminaryDecision);
				}
			}
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000269A4 File Offset: 0x00024BA4
		private KingdomDecision GetSettlementsAnyWaitingDecision(Settlement settlement)
		{
			KingdomDecision kingdomDecision = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				SettlementClaimantDecision settlementClaimantDecision;
				return (settlementClaimantDecision = d as SettlementClaimantDecision) != null && settlementClaimantDecision.Settlement == settlement && !d.ShouldBeCancelled();
			});
			KingdomDecision kingdomDecision2 = Clan.PlayerClan.Kingdom.UnresolvedDecisions.FirstOrDefault<KingdomDecision>(delegate(KingdomDecision d)
			{
				SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision;
				return (settlementClaimantPreliminaryDecision = d as SettlementClaimantPreliminaryDecision) != null && settlementClaimantPreliminaryDecision.Settlement == settlement && !d.ShouldBeCancelled();
			});
			return kingdomDecision ?? kingdomDecision2;
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00026A04 File Offset: 0x00024C04
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x00026A0C File Offset: 0x00024C0C
		[DataSourceProperty]
		public KingdomSettlementItemVM CurrentSelectedSettlement
		{
			get
			{
				return this._currentSelectedSettlement;
			}
			set
			{
				if (value != this._currentSelectedSettlement)
				{
					this._currentSelectedSettlement = value;
					base.OnPropertyChangedWithValue<KingdomSettlementItemVM>(value, "CurrentSelectedSettlement");
				}
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00026A2A File Offset: 0x00024C2A
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x00026A32 File Offset: 0x00024C32
		[DataSourceProperty]
		public KingdomSettlementSortControllerVM SettlementSortController
		{
			get
			{
				return this._settlementSortController;
			}
			set
			{
				if (value != this._settlementSortController)
				{
					this._settlementSortController = value;
					base.OnPropertyChangedWithValue<KingdomSettlementSortControllerVM>(value, "SettlementSortController");
				}
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00026A50 File Offset: 0x00024C50
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x00026A58 File Offset: 0x00024C58
		[DataSourceProperty]
		public HintViewModel AnnexHint
		{
			get
			{
				return this._annexHint;
			}
			set
			{
				if (value != this._annexHint)
				{
					this._annexHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AnnexHint");
				}
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00026A76 File Offset: 0x00024C76
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x00026A7E File Offset: 0x00024C7E
		[DataSourceProperty]
		public string ProposeText
		{
			get
			{
				return this._proposeText;
			}
			set
			{
				if (value != this._proposeText)
				{
					this._proposeText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProposeText");
				}
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00026AA1 File Offset: 0x00024CA1
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x00026AA9 File Offset: 0x00024CA9
		[DataSourceProperty]
		public string AnnexActionExplanationText
		{
			get
			{
				return this._annexActionExplanationText;
			}
			set
			{
				if (value != this._annexActionExplanationText)
				{
					this._annexActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "AnnexActionExplanationText");
				}
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x00026ACC File Offset: 0x00024CCC
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x00026AD4 File Offset: 0x00024CD4
		[DataSourceProperty]
		public string ProsperityText
		{
			get
			{
				return this._prosperityText;
			}
			set
			{
				if (value != this._prosperityText)
				{
					this._prosperityText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProsperityText");
				}
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x00026AF7 File Offset: 0x00024CF7
		// (set) Token: 0x0600086A RID: 2154 RVA: 0x00026AFF File Offset: 0x00024CFF
		[DataSourceProperty]
		public string VillagesText
		{
			get
			{
				return this._villagesText;
			}
			set
			{
				if (value != this._villagesText)
				{
					this._villagesText = value;
					base.OnPropertyChangedWithValue<string>(value, "VillagesText");
				}
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x00026B22 File Offset: 0x00024D22
		// (set) Token: 0x0600086C RID: 2156 RVA: 0x00026B2A File Offset: 0x00024D2A
		[DataSourceProperty]
		public string OwnerText
		{
			get
			{
				return this._ownerText;
			}
			set
			{
				if (value != this._ownerText)
				{
					this._ownerText = value;
					base.OnPropertyChangedWithValue<string>(value, "OwnerText");
				}
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x00026B4D File Offset: 0x00024D4D
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x00026B55 File Offset: 0x00024D55
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x00026B78 File Offset: 0x00024D78
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x00026B80 File Offset: 0x00024D80
		[DataSourceProperty]
		public string ClanText
		{
			get
			{
				return this._clanText;
			}
			set
			{
				if (value != this._clanText)
				{
					this._clanText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanText");
				}
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x00026BA3 File Offset: 0x00024DA3
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x00026BAB File Offset: 0x00024DAB
		[DataSourceProperty]
		public string FoodText
		{
			get
			{
				return this._foodText;
			}
			set
			{
				if (value != this._foodText)
				{
					this._foodText = value;
					base.OnPropertyChangedWithValue<string>(value, "FoodText");
				}
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x00026BCE File Offset: 0x00024DCE
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x00026BD6 File Offset: 0x00024DD6
		[DataSourceProperty]
		public string GarrisonText
		{
			get
			{
				return this._garrisonText;
			}
			set
			{
				if (value != this._garrisonText)
				{
					this._garrisonText = value;
					base.OnPropertyChangedWithValue<string>(value, "GarrisonText");
				}
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00026BF9 File Offset: 0x00024DF9
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x00026C01 File Offset: 0x00024E01
		[DataSourceProperty]
		public string MilitiaText
		{
			get
			{
				return this._militiaText;
			}
			set
			{
				if (value != this._militiaText)
				{
					this._militiaText = value;
					base.OnPropertyChangedWithValue<string>(value, "MilitiaText");
				}
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x00026C24 File Offset: 0x00024E24
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x00026C2C File Offset: 0x00024E2C
		[DataSourceProperty]
		public string AnnexText
		{
			get
			{
				return this._annexText;
			}
			set
			{
				if (value != this._annexText)
				{
					this._annexText = value;
					base.OnPropertyChangedWithValue<string>(value, "AnnexText");
				}
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x00026C4F File Offset: 0x00024E4F
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x00026C57 File Offset: 0x00024E57
		[DataSourceProperty]
		public string TypeText
		{
			get
			{
				return this._typeText;
			}
			set
			{
				if (value != this._typeText)
				{
					this._typeText = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeText");
				}
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00026C7A File Offset: 0x00024E7A
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x00026C82 File Offset: 0x00024E82
		[DataSourceProperty]
		public int AnnexCost
		{
			get
			{
				return this._annexCost;
			}
			set
			{
				if (value != this._annexCost)
				{
					this._annexCost = value;
					base.OnPropertyChangedWithValue(value, "AnnexCost");
				}
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00026CA0 File Offset: 0x00024EA0
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x00026CA8 File Offset: 0x00024EA8
		[DataSourceProperty]
		public string DefendersText
		{
			get
			{
				return this._defendersText;
			}
			set
			{
				if (value != this._defendersText)
				{
					this._defendersText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefendersText");
				}
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00026CCB File Offset: 0x00024ECB
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x00026CD3 File Offset: 0x00024ED3
		[DataSourceProperty]
		public MBBindingList<KingdomSettlementItemVM> Settlements
		{
			get
			{
				return this._settlements;
			}
			set
			{
				if (value != this._settlements)
				{
					this._settlements = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomSettlementItemVM>>(value, "Settlements");
				}
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x00026CF1 File Offset: 0x00024EF1
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x00026CF9 File Offset: 0x00024EF9
		[DataSourceProperty]
		public bool CanAnnexCurrentSettlement
		{
			get
			{
				return this._canAnnexCurrentSettlement;
			}
			set
			{
				if (value != this._canAnnexCurrentSettlement)
				{
					this._canAnnexCurrentSettlement = value;
					base.OnPropertyChangedWithValue(value, "CanAnnexCurrentSettlement");
				}
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00026D17 File Offset: 0x00024F17
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x00026D1F File Offset: 0x00024F1F
		[DataSourceProperty]
		public bool HasCost
		{
			get
			{
				return this._hasCost;
			}
			set
			{
				if (value != this._hasCost)
				{
					this._hasCost = value;
					base.OnPropertyChangedWithValue(value, "HasCost");
				}
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00026D40 File Offset: 0x00024F40
		private static TextObject CalculateLikelihood(Settlement settlement)
		{
			SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision = new SettlementClaimantPreliminaryDecision(Clan.PlayerClan, settlement);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(settlementClaimantPreliminaryDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x04000393 RID: 915
		private readonly Action<KingdomDecision> _forceDecision;

		// Token: 0x04000394 RID: 916
		private readonly Action<Settlement> _onGrantFief;

		// Token: 0x04000395 RID: 917
		private readonly Kingdom _kingdom;

		// Token: 0x04000396 RID: 918
		private KingdomDecision _currenItemsUnresolvedDecision;

		// Token: 0x04000397 RID: 919
		private MBBindingList<KingdomSettlementItemVM> _settlements;

		// Token: 0x04000398 RID: 920
		private KingdomSettlementItemVM _currentSelectedSettlement;

		// Token: 0x04000399 RID: 921
		private HintViewModel _annexHint;

		// Token: 0x0400039A RID: 922
		private string _ownerText;

		// Token: 0x0400039B RID: 923
		private string _nameText;

		// Token: 0x0400039C RID: 924
		private string _typeText;

		// Token: 0x0400039D RID: 925
		private string _prosperityText;

		// Token: 0x0400039E RID: 926
		private string _foodText;

		// Token: 0x0400039F RID: 927
		private string _garrisonText;

		// Token: 0x040003A0 RID: 928
		private string _militiaText;

		// Token: 0x040003A1 RID: 929
		private string _annexText;

		// Token: 0x040003A2 RID: 930
		private string _clanText;

		// Token: 0x040003A3 RID: 931
		private string _villagesText;

		// Token: 0x040003A4 RID: 932
		private string _annexActionExplanationText;

		// Token: 0x040003A5 RID: 933
		private string _proposeText;

		// Token: 0x040003A6 RID: 934
		private string _defendersText;

		// Token: 0x040003A7 RID: 935
		private int _annexCost;

		// Token: 0x040003A8 RID: 936
		private bool _canAnnexCurrentSettlement;

		// Token: 0x040003A9 RID: 937
		private bool _hasCost;

		// Token: 0x040003AA RID: 938
		private KingdomSettlementSortControllerVM _settlementSortController;
	}
}
