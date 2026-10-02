using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x02000092 RID: 146
	public class KingdomArmyVM : KingdomCategoryVM
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x00032368 File Offset: 0x00030568
		public KingdomArmyVM(Action onManageArmy, Action refreshDecision, Action<Army> showArmyOnMap)
		{
			this._onManageArmy = onManageArmy;
			this._refreshDecision = refreshDecision;
			this._showArmyOnMap = showArmyOnMap;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this._armies = new MBBindingList<KingdomArmyItemVM>();
			this.PlayerHasArmy = MobileParty.MainParty.Army != null;
			this.ChangeLeaderCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfChangingLeaderOfArmy();
			this.DisbandCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfDisbandingArmy();
			this.CreateArmyHint = new HintViewModel();
			this.DisbandHint = new HintViewModel();
			this.ManageArmyHint = new HintViewModel();
			base.IsAcceptableItemSelected = false;
			this.RefreshArmyList();
			this.ArmySortController = new KingdomArmySortControllerVM(ref this._armies);
			this.RefreshValues();
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00032438 File Offset: 0x00030638
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ArmyNameText = GameTexts.FindText("str_sort_by_army_name_label", null).ToString();
			this.LeaderText = GameTexts.FindText("str_sort_by_leader_name_label", null).ToString();
			this.StrengthText = GameTexts.FindText("str_men", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			base.NoItemSelectedText = GameTexts.FindText("str_kingdom_no_army_selected", null).ToString();
			this.DisbandActionExplanationText = GameTexts.FindText("str_kingdom_disband_army_explanation", null).ToString();
			this.ManageActionExplanationText = GameTexts.FindText("str_kingdom_manage_army_explanation", null).ToString();
			this.ManageText = GameTexts.FindText("str_manage", null).ToString();
			this.CreateArmyText = (this.PlayerHasArmy ? new TextObject("{=DAmdTxuC}Army Manage", null).ToString() : new TextObject("{=lc9s4rLZ}Create Army", null).ToString());
			base.CategoryNameText = new TextObject("{=j12VrGKz}Army", null).ToString();
			this.ChangeLeaderText = new TextObject("{=NcYbdiyT}Change Leader", null).ToString();
			this.PartiesText = new TextObject("{=t3tq0eoW}Parties", null).ToString();
			this.DisbandText = new TextObject("{=xXSFaGW8}Disband", null).ToString();
			this.ShowOnMapText = GameTexts.FindText("str_show_on_map", null).ToString();
			this.CreateArmyText = new TextObject("{=lc9s4rLZ}Create Army", null).ToString();
			this.Armies.ApplyActionOnAllItems(delegate(KingdomArmyItemVM x)
			{
				x.RefreshValues();
			});
			KingdomArmyItemVM currentSelectedArmy = this.CurrentSelectedArmy;
			if (currentSelectedArmy == null)
			{
				return;
			}
			currentSelectedArmy.RefreshValues();
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000325EC File Offset: 0x000307EC
		public void RefreshArmyList()
		{
			base.NotificationCount = this._viewDataTracker.NumOfKingdomArmyNotifications;
			this._kingdom = Hero.MainHero.MapFaction as Kingdom;
			if (this._kingdom != null)
			{
				this.Armies.Clear();
				using (List<Army>.Enumerator enumerator = this._kingdom.Armies.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Army army = enumerator.Current;
						this.Armies.Add(new KingdomArmyItemVM(army, new Action<KingdomArmyItemVM>(this.OnSelection)));
					}
					goto IL_00A0;
				}
			}
			Debug.FailedAssert("Kingdom screen can't open if you're not in kingdom", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\KingdomManagement\\Armies\\KingdomArmyVM.cs", "RefreshArmyList", 81);
			IL_00A0:
			this.RefreshCanManageArmy();
			if (this.Armies.Count == 0 && this.CurrentSelectedArmy != null)
			{
				this.OnSelection(null);
				return;
			}
			if (this.Armies.Count > 0)
			{
				this.OnSelection(this.Armies[0]);
				this.CurrentSelectedArmy.IsSelected = true;
			}
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000326F8 File Offset: 0x000308F8
		private void ExecuteManageArmy()
		{
			this._onManageArmy();
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00032705 File Offset: 0x00030905
		private void ExecuteShowOnMap()
		{
			if (this.CurrentSelectedArmy != null)
			{
				this._showArmyOnMap(this.CurrentSelectedArmy.Army);
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00032728 File Offset: 0x00030928
		private void RefreshCurrentArmyVisuals(KingdomArmyItemVM item)
		{
			if (item != null)
			{
				if (this.CurrentSelectedArmy != null)
				{
					this.CurrentSelectedArmy.IsSelected = false;
				}
				this.CanManageCurrentArmy = false;
				this.CurrentSelectedArmy = item;
				base.NotificationCount = this._viewDataTracker.NumOfKingdomArmyNotifications;
				this.DisbandCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfDisbandingArmy();
				this.ChangeLeaderCost = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfChangingLeaderOfArmy();
				TextObject textObject;
				this.CanDisbandCurrentArmy = this.GetCanDisbandCurrentArmyWithReason(item, this.DisbandCost, out textObject);
				this.DisbandHint.HintText = textObject;
				this.DisbandActionExplanationText = GameTexts.FindText("str_kingdom_disband_army_explanation", null).ToString();
				if (this.CurrentSelectedArmy != null)
				{
					this.CanShowLocationOfCurrentArmy = this.CurrentSelectedArmy.Army.AiBehaviorObject is Settlement || this.CurrentSelectedArmy.Army.AiBehaviorObject is MobileParty;
					TextObject textObject2;
					this.CanManageCurrentArmy = this.GetCanManageCurrentArmyWithReason(out textObject2);
					this.ManageArmyHint.HintText = textObject2;
				}
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00032833 File Offset: 0x00030A33
		private bool GetCanManageCurrentArmyWithReason(out TextObject disabledReason)
		{
			KingdomArmyItemVM currentSelectedArmy = this.CurrentSelectedArmy;
			if (currentSelectedArmy == null || !currentSelectedArmy.IsMainArmy)
			{
				disabledReason = TextObject.GetEmpty();
				return false;
			}
			return CampaignUIHelper.GetCanManageCurrentArmyWithReason(out disabledReason);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0003285C File Offset: 0x00030A5C
		private bool GetCanDisbandCurrentArmyWithReason(KingdomArmyItemVM armyItem, int disbandCost, out TextObject disabledReason)
		{
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				disabledReason = GameTexts.FindText("str_cannot_disband_army_while_mercenary", null);
				return false;
			}
			if (Clan.PlayerClan.Influence < (float)disbandCost)
			{
				disabledReason = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null);
				return false;
			}
			if (armyItem.Army.LeaderParty.MapEvent != null)
			{
				disabledReason = GameTexts.FindText("str_cannot_disband_army_while_in_event", null);
				return false;
			}
			if (armyItem.Army.Parties.Contains(MobileParty.MainParty))
			{
				disabledReason = GameTexts.FindText("str_cannot_disband_army_while_in_that_army", null);
				return false;
			}
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00032900 File Offset: 0x00030B00
		public void SelectArmy(Army army)
		{
			foreach (KingdomArmyItemVM kingdomArmyItemVM in this.Armies)
			{
				if (kingdomArmyItemVM.Army == army)
				{
					this.OnSelection(kingdomArmyItemVM);
					break;
				}
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00032958 File Offset: 0x00030B58
		private void OnSelection(KingdomArmyItemVM item)
		{
			if (this.CurrentSelectedArmy != item)
			{
				this.RefreshCurrentArmyVisuals(item);
				this.CurrentSelectedArmy = item;
				base.IsAcceptableItemSelected = item != null;
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0003297C File Offset: 0x00030B7C
		private void ExecuteDisbandCurrentArmy()
		{
			if (this.CurrentSelectedArmy != null && Hero.MainHero.Clan.Influence >= (float)this.DisbandCost)
			{
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_disband_army", null).ToString(), new TextObject("{=zrhr4rDA}Are you sure you want to disband this army? This will result in relation loss.", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.DisbandCurrentArmy), null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00032A14 File Offset: 0x00030C14
		private void DisbandCurrentArmy()
		{
			if (this.CurrentSelectedArmy != null && Hero.MainHero.Clan.Influence >= (float)this.DisbandCost)
			{
				DisbandArmyAction.ApplyByReleasedByPlayerAfterBattle(this.CurrentSelectedArmy.Army);
				this.RefreshArmyList();
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00032A4C File Offset: 0x00030C4C
		private void RefreshCanManageArmy()
		{
			this.PlayerHasArmy = MobileParty.MainParty.Army != null;
			TextObject textObject;
			this.CanCreateArmy = Campaign.Current.Models.ArmyManagementCalculationModel.CanPlayerCreateArmy(out textObject);
			this.CreateArmyHint.HintText = textObject;
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00032A94 File Offset: 0x00030C94
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x00032A9C File Offset: 0x00030C9C
		[DataSourceProperty]
		public KingdomArmySortControllerVM ArmySortController
		{
			get
			{
				return this._armySortController;
			}
			set
			{
				if (value != this._armySortController)
				{
					this._armySortController = value;
					base.OnPropertyChangedWithValue<KingdomArmySortControllerVM>(value, "ArmySortController");
				}
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00032ABA File Offset: 0x00030CBA
		// (set) Token: 0x06000C24 RID: 3108 RVA: 0x00032AC2 File Offset: 0x00030CC2
		[DataSourceProperty]
		public string CreateArmyText
		{
			get
			{
				return this._createArmyText;
			}
			set
			{
				if (value != this._createArmyText)
				{
					this._createArmyText = value;
					base.OnPropertyChangedWithValue<string>(value, "CreateArmyText");
				}
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00032AE5 File Offset: 0x00030CE5
		// (set) Token: 0x06000C26 RID: 3110 RVA: 0x00032AED File Offset: 0x00030CED
		[DataSourceProperty]
		public string DisbandActionExplanationText
		{
			get
			{
				return this._disbandActionExplanationText;
			}
			set
			{
				if (value != this._disbandActionExplanationText)
				{
					this._disbandActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisbandActionExplanationText");
				}
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00032B10 File Offset: 0x00030D10
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x00032B18 File Offset: 0x00030D18
		[DataSourceProperty]
		public string ManageActionExplanationText
		{
			get
			{
				return this._manageActionExplanationText;
			}
			set
			{
				if (value != this._manageActionExplanationText)
				{
					this._manageActionExplanationText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManageActionExplanationText");
				}
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00032B3B File Offset: 0x00030D3B
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x00032B43 File Offset: 0x00030D43
		[DataSourceProperty]
		public KingdomArmyItemVM CurrentSelectedArmy
		{
			get
			{
				return this._currentSelectedArmy;
			}
			set
			{
				if (value != this._currentSelectedArmy)
				{
					this._currentSelectedArmy = value;
					base.OnPropertyChangedWithValue<KingdomArmyItemVM>(value, "CurrentSelectedArmy");
				}
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00032B61 File Offset: 0x00030D61
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x00032B69 File Offset: 0x00030D69
		[DataSourceProperty]
		public HintViewModel CreateArmyHint
		{
			get
			{
				return this._createArmyHint;
			}
			set
			{
				if (value != this._createArmyHint)
				{
					this._createArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CreateArmyHint");
				}
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x00032B87 File Offset: 0x00030D87
		// (set) Token: 0x06000C2E RID: 3118 RVA: 0x00032B8F File Offset: 0x00030D8F
		[DataSourceProperty]
		public HintViewModel ManageArmyHint
		{
			get
			{
				return this._manageArmyHint;
			}
			set
			{
				if (value != this._manageArmyHint)
				{
					this._manageArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ManageArmyHint");
				}
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00032BAD File Offset: 0x00030DAD
		// (set) Token: 0x06000C30 RID: 3120 RVA: 0x00032BB5 File Offset: 0x00030DB5
		[DataSourceProperty]
		public bool PlayerHasArmy
		{
			get
			{
				return this._playerHasArmy;
			}
			set
			{
				if (value != this._playerHasArmy)
				{
					this._playerHasArmy = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasArmy");
				}
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00032BD3 File Offset: 0x00030DD3
		// (set) Token: 0x06000C32 RID: 3122 RVA: 0x00032BDB File Offset: 0x00030DDB
		[DataSourceProperty]
		public bool CanCreateArmy
		{
			get
			{
				return this._canCreateArmy;
			}
			set
			{
				if (value != this._canCreateArmy)
				{
					this._canCreateArmy = value;
					base.OnPropertyChangedWithValue(value, "CanCreateArmy");
				}
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00032BF9 File Offset: 0x00030DF9
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x00032C01 File Offset: 0x00030E01
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._categoryLeaderName;
			}
			set
			{
				if (value != this._categoryLeaderName)
				{
					this._categoryLeaderName = value;
					base.OnPropertyChanged("CategoryLeaderName");
				}
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00032C23 File Offset: 0x00030E23
		// (set) Token: 0x06000C36 RID: 3126 RVA: 0x00032C2B File Offset: 0x00030E2B
		[DataSourceProperty]
		public string ShowOnMapText
		{
			get
			{
				return this._showOnMapText;
			}
			set
			{
				if (value != this._showOnMapText)
				{
					this._showOnMapText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShowOnMapText");
				}
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00032C4E File Offset: 0x00030E4E
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x00032C56 File Offset: 0x00030E56
		[DataSourceProperty]
		public string ArmyNameText
		{
			get
			{
				return this._categoryLordCount;
			}
			set
			{
				if (value != this._categoryLordCount)
				{
					this._categoryLordCount = value;
					base.OnPropertyChanged("CategoryLordCount");
				}
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x00032C78 File Offset: 0x00030E78
		// (set) Token: 0x06000C3A RID: 3130 RVA: 0x00032C80 File Offset: 0x00030E80
		[DataSourceProperty]
		public string StrengthText
		{
			get
			{
				return this._categoryStrength;
			}
			set
			{
				if (value != this._categoryStrength)
				{
					this._categoryStrength = value;
					base.OnPropertyChanged("CategoryStrength");
				}
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00032CA2 File Offset: 0x00030EA2
		// (set) Token: 0x06000C3C RID: 3132 RVA: 0x00032CAA File Offset: 0x00030EAA
		[DataSourceProperty]
		public string PartiesText
		{
			get
			{
				return this._categoryParties;
			}
			set
			{
				if (value != this._categoryParties)
				{
					this._categoryParties = value;
					base.OnPropertyChangedWithValue<string>(value, "PartiesText");
				}
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00032CCD File Offset: 0x00030ECD
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x00032CD5 File Offset: 0x00030ED5
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._categoryObjective;
			}
			set
			{
				if (value != this._categoryObjective)
				{
					this._categoryObjective = value;
					base.OnPropertyChanged("CategoryObjective");
				}
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00032CF7 File Offset: 0x00030EF7
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x00032CFF File Offset: 0x00030EFF
		[DataSourceProperty]
		public MBBindingList<KingdomArmyItemVM> Armies
		{
			get
			{
				return this._armies;
			}
			set
			{
				if (value != this._armies)
				{
					this._armies = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomArmyItemVM>>(value, "Armies");
				}
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00032D1D File Offset: 0x00030F1D
		// (set) Token: 0x06000C42 RID: 3138 RVA: 0x00032D25 File Offset: 0x00030F25
		[DataSourceProperty]
		public bool CanDisbandCurrentArmy
		{
			get
			{
				return this._canDisbandCurrentArmy;
			}
			set
			{
				if (value != this._canDisbandCurrentArmy)
				{
					this._canDisbandCurrentArmy = value;
					base.OnPropertyChangedWithValue(value, "CanDisbandCurrentArmy");
				}
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00032D43 File Offset: 0x00030F43
		// (set) Token: 0x06000C44 RID: 3140 RVA: 0x00032D4B File Offset: 0x00030F4B
		[DataSourceProperty]
		public bool CanManageCurrentArmy
		{
			get
			{
				return this._canManageCurrentArmy;
			}
			set
			{
				if (value != this._canManageCurrentArmy)
				{
					this._canManageCurrentArmy = value;
					base.OnPropertyChangedWithValue(value, "CanManageCurrentArmy");
				}
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00032D69 File Offset: 0x00030F69
		// (set) Token: 0x06000C46 RID: 3142 RVA: 0x00032D71 File Offset: 0x00030F71
		[DataSourceProperty]
		public bool CanChangeLeaderOfCurrentArmy
		{
			get
			{
				return this._canChangeLeaderOfCurrentArmy;
			}
			set
			{
				if (value != this._canChangeLeaderOfCurrentArmy)
				{
					this._canChangeLeaderOfCurrentArmy = value;
					base.OnPropertyChangedWithValue(value, "CanChangeLeaderOfCurrentArmy");
				}
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00032D8F File Offset: 0x00030F8F
		// (set) Token: 0x06000C48 RID: 3144 RVA: 0x00032D97 File Offset: 0x00030F97
		[DataSourceProperty]
		public bool CanShowLocationOfCurrentArmy
		{
			get
			{
				return this._canShowLocationOfCurrentArmy;
			}
			set
			{
				if (value != this._canShowLocationOfCurrentArmy)
				{
					this._canShowLocationOfCurrentArmy = value;
					base.OnPropertyChangedWithValue(value, "CanShowLocationOfCurrentArmy");
				}
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00032DB5 File Offset: 0x00030FB5
		// (set) Token: 0x06000C4A RID: 3146 RVA: 0x00032DBD File Offset: 0x00030FBD
		[DataSourceProperty]
		public string DisbandText
		{
			get
			{
				return this._disbandText;
			}
			set
			{
				if (value != this._disbandText)
				{
					this._disbandText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisbandText");
				}
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x00032DE0 File Offset: 0x00030FE0
		// (set) Token: 0x06000C4C RID: 3148 RVA: 0x00032DE8 File Offset: 0x00030FE8
		[DataSourceProperty]
		public string ManageText
		{
			get
			{
				return this._manageText;
			}
			set
			{
				if (value != this._manageText)
				{
					this._manageText = value;
					base.OnPropertyChangedWithValue<string>(value, "ManageText");
				}
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00032E0B File Offset: 0x0003100B
		// (set) Token: 0x06000C4E RID: 3150 RVA: 0x00032E13 File Offset: 0x00031013
		[DataSourceProperty]
		public int DisbandCost
		{
			get
			{
				return this._disbandCost;
			}
			set
			{
				if (value != this._disbandCost)
				{
					this._disbandCost = value;
					base.OnPropertyChangedWithValue(value, "DisbandCost");
				}
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00032E31 File Offset: 0x00031031
		// (set) Token: 0x06000C50 RID: 3152 RVA: 0x00032E39 File Offset: 0x00031039
		[DataSourceProperty]
		public string ChangeLeaderText
		{
			get
			{
				return this._changeLeaderText;
			}
			set
			{
				if (value != this._changeLeaderText)
				{
					this._changeLeaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChangeLeaderText");
				}
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x00032E5C File Offset: 0x0003105C
		// (set) Token: 0x06000C52 RID: 3154 RVA: 0x00032E64 File Offset: 0x00031064
		[DataSourceProperty]
		public int ChangeLeaderCost
		{
			get
			{
				return this._changeLeaderCost;
			}
			set
			{
				if (value != this._changeLeaderCost)
				{
					this._changeLeaderCost = value;
					base.OnPropertyChangedWithValue(value, "ChangeLeaderCost");
				}
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x00032E82 File Offset: 0x00031082
		// (set) Token: 0x06000C54 RID: 3156 RVA: 0x00032E8A File Offset: 0x0003108A
		[DataSourceProperty]
		public HintViewModel DisbandHint
		{
			get
			{
				return this._disbandHint;
			}
			set
			{
				if (value != this._disbandHint)
				{
					this._disbandHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisbandHint");
				}
			}
		}

		// Token: 0x0400055B RID: 1371
		private readonly Action _onManageArmy;

		// Token: 0x0400055C RID: 1372
		private readonly Action _refreshDecision;

		// Token: 0x0400055D RID: 1373
		private readonly Action<Army> _showArmyOnMap;

		// Token: 0x0400055E RID: 1374
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x0400055F RID: 1375
		private Kingdom _kingdom;

		// Token: 0x04000560 RID: 1376
		private MBBindingList<KingdomArmyItemVM> _armies;

		// Token: 0x04000561 RID: 1377
		private KingdomArmyItemVM _currentSelectedArmy;

		// Token: 0x04000562 RID: 1378
		private HintViewModel _disbandHint;

		// Token: 0x04000563 RID: 1379
		private string _categoryLeaderName;

		// Token: 0x04000564 RID: 1380
		private string _categoryLordCount;

		// Token: 0x04000565 RID: 1381
		private string _categoryStrength;

		// Token: 0x04000566 RID: 1382
		private string _categoryObjective;

		// Token: 0x04000567 RID: 1383
		private string _categoryParties;

		// Token: 0x04000568 RID: 1384
		private string _createArmyText;

		// Token: 0x04000569 RID: 1385
		private string _disbandText;

		// Token: 0x0400056A RID: 1386
		private string _manageText;

		// Token: 0x0400056B RID: 1387
		private string _changeLeaderText;

		// Token: 0x0400056C RID: 1388
		private string _showOnMapText;

		// Token: 0x0400056D RID: 1389
		private string _disbandActionExplanationText;

		// Token: 0x0400056E RID: 1390
		private string _manageActionExplanationText;

		// Token: 0x0400056F RID: 1391
		private bool _canCreateArmy;

		// Token: 0x04000570 RID: 1392
		private bool _playerHasArmy;

		// Token: 0x04000571 RID: 1393
		private HintViewModel _createArmyHint;

		// Token: 0x04000572 RID: 1394
		private HintViewModel _manageArmyHint;

		// Token: 0x04000573 RID: 1395
		private bool _canChangeLeaderOfCurrentArmy;

		// Token: 0x04000574 RID: 1396
		private bool _canDisbandCurrentArmy;

		// Token: 0x04000575 RID: 1397
		private bool _canShowLocationOfCurrentArmy;

		// Token: 0x04000576 RID: 1398
		private bool _canManageCurrentArmy;

		// Token: 0x04000577 RID: 1399
		private int _disbandCost;

		// Token: 0x04000578 RID: 1400
		private int _changeLeaderCost;

		// Token: 0x04000579 RID: 1401
		private KingdomArmySortControllerVM _armySortController;
	}
}
