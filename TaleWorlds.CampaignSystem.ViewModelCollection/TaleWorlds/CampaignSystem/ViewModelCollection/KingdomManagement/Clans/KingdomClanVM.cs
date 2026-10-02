using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans
{
	// Token: 0x0200008E RID: 142
	public class KingdomClanVM : KingdomCategoryVM
	{
		// Token: 0x06000B97 RID: 2967 RVA: 0x00030DD0 File Offset: 0x0002EFD0
		public KingdomClanVM(Action<KingdomDecision> forceDecide)
		{
			this._forceDecide = forceDecide;
			this.SupportHint = new HintViewModel();
			this.ExpelHint = new HintViewModel();
			this._clans = new MBBindingList<KingdomClanItemVM>();
			base.IsAcceptableItemSelected = false;
			this.RefreshClanList();
			base.NotificationCount = 0;
			this.SupportCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfSupportingClan();
			this.ExpelCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfExpellingClan(Clan.PlayerClan);
			TextObject textObject;
			this.CanSupportCurrentClan = this.GetCanSupportCurrentClanWithReason(this.SupportCost, out textObject);
			this.SupportHint.HintText = textObject;
			TextObject textObject2;
			this.CanExpelCurrentClan = this.GetCanExpelCurrentClanWithReason(this._isThereAPendingDecisionToExpelThisClan, this.ExpelCost, out textObject2);
			this.ExpelHint.HintText = textObject2;
			this.ClanSortController = new KingdomClanSortControllerVM(ref this._clans);
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			this.RefreshValues();
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00030ECC File Offset: 0x0002F0CC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SupportText = new TextObject("{=N63XYX2r}Support", null).ToString();
			this.NameText = GameTexts.FindText("str_scoreboard_header", "name").ToString();
			this.InfluenceText = GameTexts.FindText("str_influence", null).ToString();
			this.FiefsText = GameTexts.FindText("str_fiefs", null).ToString();
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.BannerText = GameTexts.FindText("str_banner", null).ToString();
			this.TypeText = GameTexts.FindText("str_sort_by_type_label", null).ToString();
			base.CategoryNameText = new TextObject("{=j4F7tTzy}Clan", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_clan_selected", null).ToString();
			this.SupportActionExplanationText = GameTexts.FindText("str_support_clan_action_explanation", null).ToString();
			this.ExpelActionExplanationText = GameTexts.FindText("str_expel_clan_action_explanation", null).SetTextVariable("SUPPORT", GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.ElectionOutcomeSupport.LowSupport.ToString())).ToString();
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00030FF8 File Offset: 0x0002F1F8
		private void SetCurrentSelectedClan(KingdomClanItemVM clan)
		{
			if (clan != this.CurrentSelectedClan)
			{
				if (this.CurrentSelectedClan != null)
				{
					this.CurrentSelectedClan.IsSelected = false;
				}
				this.CurrentSelectedClan = clan;
				this.CurrentSelectedClan.IsSelected = true;
				this.SupportCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfSupportingClan();
				this._isThereAPendingDecisionToExpelThisClan = Clan.PlayerClan.Kingdom.UnresolvedDecisions.Any<KingdomDecision>(delegate(KingdomDecision x)
				{
					ExpelClanFromKingdomDecision expelClanFromKingdomDecision;
					return (expelClanFromKingdomDecision = x as ExpelClanFromKingdomDecision) != null && expelClanFromKingdomDecision.ClanToExpel == this.CurrentSelectedClan.Clan && !x.ShouldBeCancelled();
				});
				TextObject textObject;
				this.CanExpelCurrentClan = this.GetCanExpelCurrentClanWithReason(this._isThereAPendingDecisionToExpelThisClan, this.ExpelCost, out textObject);
				this.ExpelHint.HintText = textObject;
				if (this._isThereAPendingDecisionToExpelThisClan)
				{
					this.ExpelActionText = GameTexts.FindText("str_resolve", null).ToString();
					this.ExpelActionExplanationText = GameTexts.FindText("str_resolve_explanation", null).ToString();
					this.ExpelCost = 0;
					return;
				}
				this.ExpelActionText = GameTexts.FindText("str_policy_propose", null).ToString();
				this.ExpelCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfExpellingClan(Clan.PlayerClan);
				TextObject textObject2;
				this.CanSupportCurrentClan = this.GetCanSupportCurrentClanWithReason(this.SupportCost, out textObject2);
				this.SupportHint.HintText = textObject2;
				this.ExpelActionExplanationText = GameTexts.FindText("str_expel_clan_action_explanation", null).SetTextVariable("SUPPORT", this.GetExpelLikelihoodText(this.CurrentSelectedClan)).ToString();
				base.IsAcceptableItemSelected = this.CurrentSelectedClan != null;
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0003116C File Offset: 0x0002F36C
		private TextObject GetExpelLikelihoodText(KingdomClanItemVM clan)
		{
			ExpelClanFromKingdomDecision expelClanFromKingdomDecision = new ExpelClanFromKingdomDecision(Clan.PlayerClan, clan.Clan);
			return GameTexts.FindText("str_decision_outcome_support_status", KingdomElection.GetElectionOutcomeSupport(expelClanFromKingdomDecision, Clan.PlayerClan).ToString());
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000311B0 File Offset: 0x0002F3B0
		private bool GetCanSupportCurrentClanWithReason(int supportCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Hero.MainHero.Clan.Influence < (float)supportCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			if (this.CurrentSelectedClan.Clan == Clan.PlayerClan)
			{
				disabledReason = GameTexts.FindText("str_cannot_support_your_clan", null);
				return false;
			}
			if (Hero.MainHero.Clan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_mercenaries_cannot_support_clans", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00031238 File Offset: 0x0002F438
		private bool GetCanExpelCurrentClanWithReason(bool isThereAPendingDecision, int expelCost, out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (Hero.MainHero.Clan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_mercenaries_cannot_expel_clans", null);
				return false;
			}
			if (!isThereAPendingDecision)
			{
				if (Hero.MainHero.Clan.Influence < (float)expelCost)
				{
					disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
					return false;
				}
				if (this.CurrentSelectedClan.Clan == Clan.PlayerClan)
				{
					disabledReason = GameTexts.FindText("str_cannot_expel_your_clan", null);
					return false;
				}
				Clan clan = this.CurrentSelectedClan.Clan;
				Kingdom kingdom = this.CurrentSelectedClan.Clan.Kingdom;
				if (clan == ((kingdom != null) ? kingdom.RulingClan : null))
				{
					disabledReason = GameTexts.FindText("str_cannot_expel_ruling_clan", null);
					return false;
				}
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000312FC File Offset: 0x0002F4FC
		public void RefreshClan()
		{
			this.RefreshClanList();
			foreach (KingdomClanItemVM kingdomClanItemVM in this.Clans)
			{
				kingdomClanItemVM.Refresh();
			}
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0003134C File Offset: 0x0002F54C
		public void SelectClan(Clan clan)
		{
			foreach (KingdomClanItemVM kingdomClanItemVM in this.Clans)
			{
				if (kingdomClanItemVM.Clan == clan)
				{
					this.OnClanSelection(kingdomClanItemVM);
					break;
				}
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x000313A4 File Offset: 0x0002F5A4
		private void OnClanSelection(KingdomClanItemVM clan)
		{
			if (this._currentSelectedClan != clan)
			{
				this.SetCurrentSelectedClan(clan);
			}
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x000313B8 File Offset: 0x0002F5B8
		private void ExecuteExpelCurrentClan()
		{
			if (Hero.MainHero.Clan.Influence >= (float)this.ExpelCost)
			{
				KingdomDecision kingdomDecision = new ExpelClanFromKingdomDecision(Clan.PlayerClan, this._currentSelectedClan.Clan);
				Clan.PlayerClan.Kingdom.AddDecision(kingdomDecision, false);
				this._forceDecide(kingdomDecision);
			}
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00031410 File Offset: 0x0002F610
		private void ExecuteSupport()
		{
			if (Hero.MainHero.Clan.Influence >= (float)this.SupportCost)
			{
				this._currentSelectedClan.Clan.OnSupportedByClan(Hero.MainHero.Clan);
				Clan clan = this._currentSelectedClan.Clan;
				this.RefreshClan();
				this.SelectClan(clan);
			}
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00031468 File Offset: 0x0002F668
		private int CalculateExpelLikelihood(KingdomClanItemVM clan)
		{
			return MathF.Round(new KingdomElection(new ExpelClanFromKingdomDecision(Clan.PlayerClan, clan.Clan)).GetLikelihoodForSponsor(Clan.PlayerClan) * 100f);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00031494 File Offset: 0x0002F694
		private void RefreshClanList()
		{
			this.Clans.Clear();
			if (Clan.PlayerClan.Kingdom != null)
			{
				foreach (Clan clan in Clan.PlayerClan.Kingdom.Clans)
				{
					this.Clans.Add(new KingdomClanItemVM(clan, new Action<KingdomClanItemVM>(this.OnClanSelection)));
				}
			}
			if (this.Clans.Count > 0)
			{
				this.SetCurrentSelectedClan(this.Clans.FirstOrDefault<KingdomClanItemVM>());
			}
			if (this.ClanSortController != null)
			{
				this.ClanSortController.SortByCurrentState();
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00031550 File Offset: 0x0002F750
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.OnClanChangedKingdomEvent.ClearListeners(this);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00031563 File Offset: 0x0002F763
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			if (clan != Clan.PlayerClan && (oldKingdom == Clan.PlayerClan.Kingdom || newKingdom == Clan.PlayerClan.Kingdom))
			{
				this.RefreshClanList();
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0003158D File Offset: 0x0002F78D
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x00031595 File Offset: 0x0002F795
		[DataSourceProperty]
		public KingdomClanSortControllerVM ClanSortController
		{
			get
			{
				return this._clanSortController;
			}
			set
			{
				if (value != this._clanSortController)
				{
					this._clanSortController = value;
					base.OnPropertyChangedWithValue<KingdomClanSortControllerVM>(value, "ClanSortController");
				}
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x000315B3 File Offset: 0x0002F7B3
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x000315BB File Offset: 0x0002F7BB
		[DataSourceProperty]
		public KingdomClanItemVM CurrentSelectedClan
		{
			get
			{
				return this._currentSelectedClan;
			}
			set
			{
				if (value != this._currentSelectedClan)
				{
					this._currentSelectedClan = value;
					base.OnPropertyChangedWithValue<KingdomClanItemVM>(value, "CurrentSelectedClan");
				}
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x000315D9 File Offset: 0x0002F7D9
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x000315E1 File Offset: 0x0002F7E1
		[DataSourceProperty]
		public string ExpelActionExplanationText
		{
			get
			{
				return this._expelActionExplanationText;
			}
			set
			{
				if (value != this._expelActionExplanationText)
				{
					this._expelActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExpelActionExplanationText");
				}
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00031604 File Offset: 0x0002F804
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x0003160C File Offset: 0x0002F80C
		[DataSourceProperty]
		public string SupportActionExplanationText
		{
			get
			{
				return this._supportActionExplanationText;
			}
			set
			{
				if (value != this._supportActionExplanationText)
				{
					this._supportActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportActionExplanationText");
				}
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0003162F File Offset: 0x0002F82F
		// (set) Token: 0x06000BAF RID: 2991 RVA: 0x00031637 File Offset: 0x0002F837
		[DataSourceProperty]
		public string BannerText
		{
			get
			{
				return this._bannerText;
			}
			set
			{
				if (value != this._bannerText)
				{
					this._bannerText = value;
					base.OnPropertyChangedWithValue<string>(value, "BannerText");
				}
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0003165A File Offset: 0x0002F85A
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x00031662 File Offset: 0x0002F862
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

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x00031685 File Offset: 0x0002F885
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x0003168D File Offset: 0x0002F88D
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

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x000316B0 File Offset: 0x0002F8B0
		// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x000316B8 File Offset: 0x0002F8B8
		[DataSourceProperty]
		public string InfluenceText
		{
			get
			{
				return this._influenceText;
			}
			set
			{
				if (value != this._influenceText)
				{
					this._influenceText = value;
					base.OnPropertyChangedWithValue<string>(value, "InfluenceText");
				}
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x000316DB File Offset: 0x0002F8DB
		// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x000316E3 File Offset: 0x0002F8E3
		[DataSourceProperty]
		public string FiefsText
		{
			get
			{
				return this._fiefsText;
			}
			set
			{
				if (value != this._fiefsText)
				{
					this._fiefsText = value;
					base.OnPropertyChangedWithValue<string>(value, "FiefsText");
				}
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x00031706 File Offset: 0x0002F906
		// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x0003170E File Offset: 0x0002F90E
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != this._membersText)
				{
					this._membersText = value;
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00031731 File Offset: 0x0002F931
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x00031739 File Offset: 0x0002F939
		[DataSourceProperty]
		public MBBindingList<KingdomClanItemVM> Clans
		{
			get
			{
				return this._clans;
			}
			set
			{
				if (value != this._clans)
				{
					this._clans = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomClanItemVM>>(value, "Clans");
				}
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00031757 File Offset: 0x0002F957
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0003175F File Offset: 0x0002F95F
		[DataSourceProperty]
		public bool CanSupportCurrentClan
		{
			get
			{
				return this._canSupportCurrentClan;
			}
			set
			{
				if (value != this._canSupportCurrentClan)
				{
					this._canSupportCurrentClan = value;
					base.OnPropertyChangedWithValue(value, "CanSupportCurrentClan");
				}
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0003177D File Offset: 0x0002F97D
		// (set) Token: 0x06000BBF RID: 3007 RVA: 0x00031785 File Offset: 0x0002F985
		[DataSourceProperty]
		public bool CanExpelCurrentClan
		{
			get
			{
				return this._canExpelCurrentClan;
			}
			set
			{
				if (value != this._canExpelCurrentClan)
				{
					this._canExpelCurrentClan = value;
					base.OnPropertyChangedWithValue(value, "CanExpelCurrentClan");
				}
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x000317A3 File Offset: 0x0002F9A3
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x000317AB File Offset: 0x0002F9AB
		[DataSourceProperty]
		public string SupportText
		{
			get
			{
				return this._supportText;
			}
			set
			{
				if (value != this._supportText)
				{
					this._supportText = value;
					base.OnPropertyChangedWithValue<string>(value, "SupportText");
				}
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x000317CE File Offset: 0x0002F9CE
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x000317D6 File Offset: 0x0002F9D6
		[DataSourceProperty]
		public string ExpelActionText
		{
			get
			{
				return this._expelActionText;
			}
			set
			{
				if (value != this._expelActionText)
				{
					this._expelActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExpelActionText");
				}
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x000317F9 File Offset: 0x0002F9F9
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x00031801 File Offset: 0x0002FA01
		[DataSourceProperty]
		public int SupportCost
		{
			get
			{
				return this._supportCost;
			}
			set
			{
				if (value != this._supportCost)
				{
					this._supportCost = value;
					base.OnPropertyChangedWithValue(value, "SupportCost");
				}
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0003181F File Offset: 0x0002FA1F
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x00031827 File Offset: 0x0002FA27
		[DataSourceProperty]
		public int ExpelCost
		{
			get
			{
				return this._expelCost;
			}
			set
			{
				if (value != this._expelCost)
				{
					this._expelCost = value;
					base.OnPropertyChangedWithValue(value, "ExpelCost");
				}
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00031845 File Offset: 0x0002FA45
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x0003184D File Offset: 0x0002FA4D
		[DataSourceProperty]
		public HintViewModel ExpelHint
		{
			get
			{
				return this._expelHint;
			}
			set
			{
				if (value != this._expelHint)
				{
					this._expelHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ExpelHint");
				}
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0003186B File Offset: 0x0002FA6B
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x00031873 File Offset: 0x0002FA73
		[DataSourceProperty]
		public HintViewModel SupportHint
		{
			get
			{
				return this._supportHint;
			}
			set
			{
				if (value != this._supportHint)
				{
					this._supportHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SupportHint");
				}
			}
		}

		// Token: 0x04000522 RID: 1314
		private Action<KingdomDecision> _forceDecide;

		// Token: 0x04000523 RID: 1315
		private bool _isThereAPendingDecisionToExpelThisClan;

		// Token: 0x04000524 RID: 1316
		private MBBindingList<KingdomClanItemVM> _clans;

		// Token: 0x04000525 RID: 1317
		private HintViewModel _expelHint;

		// Token: 0x04000526 RID: 1318
		private HintViewModel _supportHint;

		// Token: 0x04000527 RID: 1319
		private string _bannerText;

		// Token: 0x04000528 RID: 1320
		private string _nameText;

		// Token: 0x04000529 RID: 1321
		private string _influenceText;

		// Token: 0x0400052A RID: 1322
		private string _membersText;

		// Token: 0x0400052B RID: 1323
		private string _fiefsText;

		// Token: 0x0400052C RID: 1324
		private string _typeText;

		// Token: 0x0400052D RID: 1325
		private string _expelActionText;

		// Token: 0x0400052E RID: 1326
		private string _expelActionExplanationText;

		// Token: 0x0400052F RID: 1327
		private string _supportActionExplanationText;

		// Token: 0x04000530 RID: 1328
		private int _expelCost;

		// Token: 0x04000531 RID: 1329
		private string _supportText;

		// Token: 0x04000532 RID: 1330
		private int _supportCost;

		// Token: 0x04000533 RID: 1331
		private bool _canSupportCurrentClan;

		// Token: 0x04000534 RID: 1332
		private bool _canExpelCurrentClan;

		// Token: 0x04000535 RID: 1333
		private KingdomClanItemVM _currentSelectedClan;

		// Token: 0x04000536 RID: 1334
		private KingdomClanSortControllerVM _clanSortController;
	}
}
