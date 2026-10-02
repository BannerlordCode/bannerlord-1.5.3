using System;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000076 RID: 118
	public class KingdomTruceItemVM : KingdomDiplomacyItemVM
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x00029C15 File Offset: 0x00027E15
		public KingdomTruceItemVM(IFaction faction1, IFaction faction2, Action<KingdomDiplomacyItemVM> onSelection)
			: base(faction1, faction2)
		{
			this._onSelection = onSelection;
			this.UpdateDiplomacyProperties();
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00029C2C File Offset: 0x00027E2C
		protected override void OnSelect()
		{
			if (base.IsSelected)
			{
				return;
			}
			this.UpdateDiplomacyProperties();
			this._onSelection(this);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00029C4C File Offset: 0x00027E4C
		protected override void UpdateDiplomacyProperties()
		{
			base.UpdateDiplomacyProperties();
			base.Stats.Add(new KingdomWarComparableStatVM((int)this.Faction1.CurrentTotalStrength, (int)this.Faction2.CurrentTotalStrength, GameTexts.FindText("str_total_strength", null), this._faction1Color, this._faction2Color, 10000, null, null));
			base.Stats.Add(new KingdomWarComparableStatVM(this._faction1Towns.Count, this._faction2Towns.Count, GameTexts.FindText("str_towns", null), this._faction1Color, this._faction2Color, 25, new BasicTooltipViewModel(() => CampaignUIHelper.GetTruceOwnedSettlementsTooltip(this._faction1Towns, this.Faction1.Name, true)), new BasicTooltipViewModel(() => CampaignUIHelper.GetTruceOwnedSettlementsTooltip(this._faction2Towns, this.Faction2.Name, true))));
			base.Stats.Add(new KingdomWarComparableStatVM(this._faction1Castles.Count, this._faction2Castles.Count, GameTexts.FindText("str_castles", null), this._faction1Color, this._faction2Color, 25, new BasicTooltipViewModel(() => CampaignUIHelper.GetTruceOwnedSettlementsTooltip(this._faction1Castles, this.Faction1.Name, false)), new BasicTooltipViewModel(() => CampaignUIHelper.GetTruceOwnedSettlementsTooltip(this._faction2Castles, this.Faction2.Name, false))));
			StanceLink stanceWith = this._playerKingdom.GetStanceWith(this.Faction2);
			this.TributePaid = stanceWith.GetDailyTributeToPay(this._playerKingdom);
			if (stanceWith.IsNeutral && this.TributePaid != 0)
			{
				base.Stats.Add(new KingdomWarComparableStatVM(MathF.Max(stanceWith.GetTotalTributePaid(this.Faction2), 0), MathF.Max(stanceWith.GetTotalTributePaid(this.Faction1), 0), GameTexts.FindText("str_comparison_tribute_received", null), this._faction1Color, this._faction2Color, 10000, null, null));
			}
			if (!this.Faction1.IsKingdomFaction || !this.Faction2.IsKingdomFaction)
			{
				this.HasTradeAgreement = false;
				this.HasAlliance = false;
				this.TradeAgreementEndTimeStr = null;
				this.AllianceEndTimeStr = null;
				return;
			}
			ITradeAgreementsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ITradeAgreementsCampaignBehavior>();
			TradeAgreementsCampaignBehavior.TradeAgreement tradeAgreement = default(TradeAgreementsCampaignBehavior.TradeAgreement);
			this.HasTradeAgreement = campaignBehavior != null && campaignBehavior.HasTradeAgreement(this.Faction1 as Kingdom, this.Faction2 as Kingdom, out tradeAgreement);
			this.HasAlliance = DiplomacyHelper.HasAllianceWithFaction(this.Faction1, this.Faction2);
			if (this.HasTradeAgreement)
			{
				int num = MathF.Ceiling(tradeAgreement.EndTime.RemainingDaysFromNow);
				this.TradeAgreementEndTimeStr = new TextObject("{=6ayEZQE1}Expires in {DAYS} {?DAYS > 1}days{?}day{\\?}.", null).SetTextVariable("DAYS", num.ToString()).ToString();
				int kingdom1GoldGainedTotal = tradeAgreement.Kingdom1GoldGainedTotal;
				int kingdom2GoldGainedTotal = tradeAgreement.Kingdom2GoldGainedTotal;
				if (kingdom1GoldGainedTotal > 0 || kingdom2GoldGainedTotal > 0)
				{
					base.Stats.Add(new KingdomWarComparableStatVM(MathF.Max((tradeAgreement.Kingdom1 == this.Faction1) ? kingdom1GoldGainedTotal : kingdom2GoldGainedTotal, 0), MathF.Max((tradeAgreement.Kingdom1 == this.Faction2) ? kingdom1GoldGainedTotal : kingdom2GoldGainedTotal, 0), GameTexts.FindText("str_comparison_trade_gold_gained", null), this._faction1Color, this._faction2Color, 10000, null, null));
				}
			}
			else
			{
				this.TradeAgreementEndTimeStr = null;
			}
			IAllianceCampaignBehavior campaignBehavior2 = Campaign.Current.GetCampaignBehavior<IAllianceCampaignBehavior>();
			if (this.HasAlliance && campaignBehavior2 != null)
			{
				int num2 = MathF.Ceiling(campaignBehavior2.GetAllianceEndDate(this.Faction1 as Kingdom, this.Faction2 as Kingdom).RemainingDaysFromNow);
				this.AllianceEndTimeStr = new TextObject("{=6ayEZQE1}Expires in {DAYS} {?DAYS > 1}days{?}day{\\?}.", null).SetTextVariable("DAYS", num2.ToString()).ToString();
				return;
			}
			this.AllianceEndTimeStr = null;
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x00029FBC File Offset: 0x000281BC
		// (set) Token: 0x0600095C RID: 2396 RVA: 0x00029FC4 File Offset: 0x000281C4
		[DataSourceProperty]
		public int TributePaid
		{
			get
			{
				return this._tributePaid;
			}
			set
			{
				if (value != this._tributePaid)
				{
					this._tributePaid = value;
					base.OnPropertyChangedWithValue(value, "TributePaid");
				}
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x00029FE2 File Offset: 0x000281E2
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x00029FEA File Offset: 0x000281EA
		[DataSourceProperty]
		public bool HasTradeAgreement
		{
			get
			{
				return this._hasTradeAgreement;
			}
			set
			{
				if (value != this._hasTradeAgreement)
				{
					this._hasTradeAgreement = value;
					base.OnPropertyChangedWithValue(value, "HasTradeAgreement");
				}
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0002A008 File Offset: 0x00028208
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x0002A010 File Offset: 0x00028210
		[DataSourceProperty]
		public bool HasAlliance
		{
			get
			{
				return this._hasAlliance;
			}
			set
			{
				if (value != this._hasAlliance)
				{
					this._hasAlliance = value;
					base.OnPropertyChangedWithValue(value, "HasAlliance");
				}
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0002A02E File Offset: 0x0002822E
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x0002A036 File Offset: 0x00028236
		[DataSourceProperty]
		public string AllianceEndTimeStr
		{
			get
			{
				return this._allianceEndTimeStr;
			}
			set
			{
				if (value != this._allianceEndTimeStr)
				{
					this._allianceEndTimeStr = value;
					base.OnPropertyChangedWithValue<string>(value, "AllianceEndTimeStr");
				}
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0002A059 File Offset: 0x00028259
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x0002A061 File Offset: 0x00028261
		[DataSourceProperty]
		public string TradeAgreementEndTimeStr
		{
			get
			{
				return this._tradeAgreementEndTimeStr;
			}
			set
			{
				if (value != this._tradeAgreementEndTimeStr)
				{
					this._tradeAgreementEndTimeStr = value;
					base.OnPropertyChangedWithValue<string>(value, "TradeAgreementEndTimeStr");
				}
			}
		}

		// Token: 0x0400040F RID: 1039
		private readonly Action<KingdomDiplomacyItemVM> _onSelection;

		// Token: 0x04000410 RID: 1040
		private int _tributePaid;

		// Token: 0x04000411 RID: 1041
		private bool _hasTradeAgreement;

		// Token: 0x04000412 RID: 1042
		private bool _hasAlliance;

		// Token: 0x04000413 RID: 1043
		private string _tradeAgreementEndTimeStr;

		// Token: 0x04000414 RID: 1044
		private string _allianceEndTimeStr;
	}
}
