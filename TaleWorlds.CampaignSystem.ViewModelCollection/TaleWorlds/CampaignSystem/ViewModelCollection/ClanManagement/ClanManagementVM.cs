using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Library.Information;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x0200012E RID: 302
	public class ClanManagementVM : ViewModel
	{
		// Token: 0x06001B2E RID: 6958 RVA: 0x00065D7C File Offset: 0x00063F7C
		public ClanManagementVM(Action onClose, Action<Hero> showHeroOnMap, Action<Hero> openPartyAsManage, Action openBannerEditor)
		{
			this._onClose = onClose;
			this._openPartyAsManage = openPartyAsManage;
			this._openBannerEditor = openBannerEditor;
			this._showHeroOnMap = showHeroOnMap;
			this._clan = Hero.MainHero.Clan;
			this.CardSelectionPopup = new ClanCardSelectionPopupVM();
			this.ClanMembers = new ClanMembersVM(new Action(this.RefreshCategoryValues), this._showHeroOnMap);
			this.ClanFiefs = this.CreateFiefsDataSource(new Action(this.RefreshCategoryValues), new Action<ClanCardSelectionInfo>(this.CardSelectionPopup.Open));
			this.ClanParties = new ClanPartiesVM(new Action(this.OnAnyExpenseChange), this._openPartyAsManage, new Action(this.RefreshCategoryValues), new Action<ClanCardSelectionInfo>(this.CardSelectionPopup.Open));
			this.ClanIncome = new ClanIncomeVM(new Action(this.RefreshCategoryValues), new Action<ClanCardSelectionInfo>(this.CardSelectionPopup.Open));
			this._categoryCount = 4;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			int num = this._viewDataTracker.GetLastOpenedClanTabIndex();
			if (this._categoryCount <= num)
			{
				Debug.FailedAssert("Tab index is out of bounds", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\ClanManagement\\ClanManagementVM.cs", ".ctor", 54);
				num = 0;
			}
			this.SetSelectedCategory(num);
			this.Leader = new HeroVM(this._clan.Leader, false);
			this.CurrentRenown = (int)Clan.PlayerClan.Renown;
			this.CurrentTier = Clan.PlayerClan.Tier;
			TextObject textObject;
			if (Campaign.Current.Models.ClanTierModel.HasUpcomingTier(Clan.PlayerClan, out textObject, false).Item2)
			{
				this.NextTierRenown = Clan.PlayerClan.RenownRequirementForNextTier;
				this.MinRenownForCurrentTier = Campaign.Current.Models.ClanTierModel.GetRequiredRenownForTier(this.CurrentTier);
				this.NextTier = Clan.PlayerClan.Tier + 1;
				this.IsRenownProgressComplete = false;
			}
			else
			{
				this.NextTierRenown = 1;
				this.MinRenownForCurrentTier = 1;
				this.NextTier = 0;
				this.IsRenownProgressComplete = true;
			}
			this.CurrentRenownOverPreviousTier = this.CurrentRenown - this.MinRenownForCurrentTier;
			this.CurrentTierRenownRange = this.NextTierRenown - this.MinRenownForCurrentTier;
			this.RenownHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetClanRenownTooltip(Clan.PlayerClan));
			this.GoldChangeTooltip = CampaignUIHelper.GetDenarTooltip();
			this.RefreshDailyValues();
			this.CanChooseBanner = true;
			TextObject textObject2;
			this.PlayerCanChangeClanName = this.GetPlayerCanChangeClanNameWithReason(out textObject2);
			this.ChangeClanNameHint = new HintViewModel(textObject2, null);
			this.TutorialNotification = new ElementNotificationVM();
			this.UpdateKingdomRelatedProperties();
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x00066032 File Offset: 0x00064232
		protected virtual ClanFiefsVM CreateFiefsDataSource(Action onRefresh, Action<ClanCardSelectionInfo> openCardSelectionPopup)
		{
			return new ClanFiefsVM(onRefresh, openCardSelectionPopup);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0006603C File Offset: 0x0006423C
		private bool GetPlayerCanChangeClanNameWithReason(out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (this._clan.Leader != Hero.MainHero)
			{
				disabledReason = new TextObject("{=GCaYjA5W}You need to be the leader of the clan to change it's name.", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x00066080 File Offset: 0x00064280
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = Hero.MainHero.Clan.Name.ToString();
			this.CurrentGoldText = GameTexts.FindText("str_clan_finance_current_gold", null).ToString();
			this.TotalExpensesText = GameTexts.FindText("str_clan_finance_total_expenses", null).ToString();
			this.TotalIncomeText = GameTexts.FindText("str_clan_finance_total_income", null).ToString();
			this.DailyChangeText = GameTexts.FindText("str_clan_finance_daily_change", null).ToString();
			this.ExpectedGoldText = GameTexts.FindText("str_clan_finance_expected", null).ToString();
			this.ExpenseText = GameTexts.FindText("str_clan_expenses", null).ToString();
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.PartiesText = GameTexts.FindText("str_parties", null).ToString();
			this.IncomeText = GameTexts.FindText("str_other", null).ToString();
			this.FiefsText = GameTexts.FindText("str_fiefs", null).ToString();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			this.LeaderText = GameTexts.FindText("str_sort_by_leader_name_label", null).ToString();
			this.FinanceText = GameTexts.FindText("str_finance", null).ToString();
			GameTexts.SetVariable("TIER", Clan.PlayerClan.Tier);
			this.CurrentRenownText = GameTexts.FindText("str_clan_tier", null).ToString();
			ElementNotificationVM tutorialNotification = this.TutorialNotification;
			if (tutorialNotification != null)
			{
				tutorialNotification.RefreshValues();
			}
			ClanMembersVM clanMembers = this._clanMembers;
			if (clanMembers != null)
			{
				clanMembers.RefreshValues();
			}
			ClanPartiesVM clanParties = this._clanParties;
			if (clanParties != null)
			{
				clanParties.RefreshValues();
			}
			ClanFiefsVM clanFiefs = this._clanFiefs;
			if (clanFiefs != null)
			{
				clanFiefs.RefreshValues();
			}
			ClanIncomeVM clanIncome = this._clanIncome;
			if (clanIncome != null)
			{
				clanIncome.RefreshValues();
			}
			HeroVM leader = this._leader;
			if (leader == null)
			{
				return;
			}
			leader.RefreshValues();
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0006625A File Offset: 0x0006445A
		public void SelectHero(Hero hero)
		{
			this.SetSelectedCategory(0);
			this.ClanMembers.SelectMember(hero);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x0006626F File Offset: 0x0006446F
		public void SelectParty(PartyBase party)
		{
			this.SetSelectedCategory(1);
			this.ClanParties.SelectParty(party);
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x00066284 File Offset: 0x00064484
		public void SelectSettlement(Settlement settlement)
		{
			this.SetSelectedCategory(2);
			this.ClanFiefs.SelectFief(settlement);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x00066299 File Offset: 0x00064499
		public void SelectWorkshop(Workshop workshop)
		{
			this.SetSelectedCategory(3);
			this.ClanIncome.SelectWorkshop(workshop);
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000662AE File Offset: 0x000644AE
		public void SelectAlley(Alley alley)
		{
			this.SetSelectedCategory(3);
			this.ClanIncome.SelectAlley(alley);
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000662C4 File Offset: 0x000644C4
		public void SelectPreviousCategory()
		{
			int num = ((this._currentCategory == 0) ? (this._categoryCount - 1) : (this._currentCategory - 1));
			this.SetSelectedCategory(num);
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000662F4 File Offset: 0x000644F4
		public void SelectNextCategory()
		{
			int num = (this._currentCategory + 1) % this._categoryCount;
			this.SetSelectedCategory(num);
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00066318 File Offset: 0x00064518
		public void ExecuteOpenBannerEditor()
		{
			this._openBannerEditor();
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x00066325 File Offset: 0x00064525
		public void UpdateBannerVisuals()
		{
			this.ClanBanner = new BannerImageIdentifierVM(this._clan.Banner, true);
			this.ClanBannerHint = new HintViewModel(new TextObject("{=Nkue5MX8}Click to edit your clan's banner", null), null);
			this.RefreshValues();
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0006635C File Offset: 0x0006455C
		public void SetSelectedCategory(int index)
		{
			this.ClanMembers.IsSelected = false;
			this.ClanParties.IsSelected = false;
			this.ClanFiefs.IsSelected = false;
			this.ClanIncome.IsSelected = false;
			this._currentCategory = index;
			if (index == 0)
			{
				this.ClanMembers.IsSelected = true;
			}
			else if (index == 1)
			{
				this.ClanParties.IsSelected = true;
			}
			else if (index == 2)
			{
				this.ClanFiefs.IsSelected = true;
			}
			else
			{
				this._currentCategory = 3;
				this.ClanIncome.IsSelected = true;
			}
			this.IsMembersSelected = this.ClanMembers.IsSelected;
			this.IsPartiesSelected = this.ClanParties.IsSelected;
			this.IsFiefsSelected = this.ClanFiefs.IsSelected;
			this.IsIncomeSelected = this.ClanIncome.IsSelected;
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0006642C File Offset: 0x0006462C
		private void UpdateKingdomRelatedProperties()
		{
			this.ClanIsInAKingdom = this._clan.Kingdom != null;
			if (this.ClanIsInAKingdom)
			{
				if (this._clan.Kingdom.RulingClan == this._clan)
				{
					this.IsKingdomActionEnabled = false;
					this.KingdomActionDisabledReasonHint = new BasicTooltipViewModel(() => new TextObject("{=vIPrZCZ1}You can abdicate leadership from the kingdom screen.", null).ToString());
					this.KingdomActionText = GameTexts.FindText("str_abdicate_leadership", null).ToString();
				}
				else
				{
					this.IsKingdomActionEnabled = MobileParty.MainParty.Army == null;
					this.KingdomActionText = GameTexts.FindText("str_leave_kingdom", null).ToString();
					this.KingdomActionDisabledReasonHint = new BasicTooltipViewModel();
				}
			}
			else
			{
				List<TextObject> kingdomCreationDisabledReasons;
				this.IsKingdomActionEnabled = Campaign.Current.Models.KingdomCreationModel.IsPlayerKingdomCreationPossible(out kingdomCreationDisabledReasons);
				this.KingdomActionText = GameTexts.FindText("str_create_kingdom", null).ToString();
				this.KingdomActionDisabledReasonHint = new BasicTooltipViewModel(() => CampaignUIHelper.MergeTextObjectsWithNewline(kingdomCreationDisabledReasons));
			}
			this.UpdateBannerVisuals();
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00066550 File Offset: 0x00064750
		public void RefreshDailyValues()
		{
			if (this.ClanIncome != null)
			{
				this.CurrentGold = Hero.MainHero.Gold;
				this.TotalIncome = (int)Campaign.Current.Models.ClanFinanceModel.CalculateClanIncome(this._clan, false, false, false).ResultNumber;
				this.TotalExpenses = (int)Campaign.Current.Models.ClanFinanceModel.CalculateClanExpenses(this._clan, false, false, false).ResultNumber;
				this.DailyChange = MathF.Abs(this.TotalIncome) - MathF.Abs(this.TotalExpenses);
				this.ExpectedGold = this.CurrentGold + this.DailyChange;
				if (this.TotalIncome == 0)
				{
					this.TotalIncomeValueText = GameTexts.FindText("str_clan_finance_value_zero", null).ToString();
				}
				else
				{
					GameTexts.SetVariable("IS_POSITIVE", (this.TotalIncome > 0) ? 1 : 0);
					GameTexts.SetVariable("NUMBER", MathF.Abs(this.TotalIncome));
					this.TotalIncomeValueText = GameTexts.FindText("str_clan_finance_value", null).ToString();
				}
				if (this.TotalExpenses == 0)
				{
					this.TotalExpensesValueText = GameTexts.FindText("str_clan_finance_value_zero", null).ToString();
				}
				else
				{
					GameTexts.SetVariable("IS_POSITIVE", (this.TotalExpenses > 0) ? 1 : 0);
					GameTexts.SetVariable("NUMBER", MathF.Abs(this.TotalExpenses));
					this.TotalExpensesValueText = GameTexts.FindText("str_clan_finance_value", null).ToString();
				}
				if (this.DailyChange == 0)
				{
					this.DailyChangeValueText = GameTexts.FindText("str_clan_finance_value_zero", null).ToString();
					return;
				}
				GameTexts.SetVariable("IS_POSITIVE", (this.DailyChange > 0) ? 1 : 0);
				GameTexts.SetVariable("NUMBER", MathF.Abs(this.DailyChange));
				this.DailyChangeValueText = GameTexts.FindText("str_clan_finance_value", null).ToString();
			}
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x00066725 File Offset: 0x00064925
		public void RefreshCategoryValues()
		{
			this.ClanFiefs.RefreshAllLists();
			this.ClanMembers.RefreshMembersList();
			this.ClanParties.RefreshPartiesList();
			this.ClanIncome.RefreshList();
			this.RefreshDailyValues();
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x0006675C File Offset: 0x0006495C
		public void ExecuteChangeClanName()
		{
			GameTexts.SetVariable("MAX_LETTER_COUNT", 50);
			GameTexts.SetVariable("MIN_LETTER_COUNT", 1);
			InformationManager.ShowTextInquiry(new TextInquiryData(GameTexts.FindText("str_change_clan_name", null).ToString(), string.Empty, true, true, GameTexts.FindText("str_done", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action<string>(this.OnChangeClanNameDone), null, false, new Func<string, Tuple<bool, string>>(FactionHelper.IsClanNameApplicable), "", ""), false, false);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x000667E8 File Offset: 0x000649E8
		private void OnChangeClanNameDone(string newClanName)
		{
			TextObject textObject = GameTexts.FindText("str_generic_clan_name", null);
			textObject.SetTextVariable("CLAN_NAME", new TextObject(newClanName, null));
			this._clan.ChangeClanName(textObject, textObject);
			this.RefreshCategoryValues();
			this.RefreshValues();
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0006682D File Offset: 0x00064A2D
		private void OnAnyExpenseChange()
		{
			this.RefreshDailyValues();
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00066835 File Offset: 0x00064A35
		public void ExecuteClose()
		{
			this._onClose();
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00066844 File Offset: 0x00064A44
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._viewDataTracker.SetLastOpenedClanTabIndex(this._currentCategory);
			this.ClanFiefs.OnFinalize();
			this.DoneInputKey.OnFinalize();
			this.PreviousTabInputKey.OnFinalize();
			this.NextTabInputKey.OnFinalize();
			this.CardSelectionPopup.OnFinalize();
			this.ClanMembers.OnFinalize();
			this.ClanParties.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x000668D0 File Offset: 0x00064AD0
		// (set) Token: 0x06001B45 RID: 6981 RVA: 0x000668D8 File Offset: 0x00064AD8
		[DataSourceProperty]
		public HeroVM Leader
		{
			get
			{
				return this._leader;
			}
			set
			{
				if (value != this._leader)
				{
					this._leader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Leader");
				}
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06001B46 RID: 6982 RVA: 0x000668F6 File Offset: 0x00064AF6
		// (set) Token: 0x06001B47 RID: 6983 RVA: 0x000668FE File Offset: 0x00064AFE
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06001B48 RID: 6984 RVA: 0x0006691C File Offset: 0x00064B1C
		// (set) Token: 0x06001B49 RID: 6985 RVA: 0x00066924 File Offset: 0x00064B24
		[DataSourceProperty]
		public ClanCardSelectionPopupVM CardSelectionPopup
		{
			get
			{
				return this._cardSelectionPopup;
			}
			set
			{
				if (value != this._cardSelectionPopup)
				{
					this._cardSelectionPopup = value;
					base.OnPropertyChangedWithValue<ClanCardSelectionPopupVM>(value, "CardSelectionPopup");
				}
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06001B4A RID: 6986 RVA: 0x00066942 File Offset: 0x00064B42
		// (set) Token: 0x06001B4B RID: 6987 RVA: 0x0006694A File Offset: 0x00064B4A
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06001B4C RID: 6988 RVA: 0x0006696D File Offset: 0x00064B6D
		// (set) Token: 0x06001B4D RID: 6989 RVA: 0x00066975 File Offset: 0x00064B75
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._leaderText;
			}
			set
			{
				if (value != this._leaderText)
				{
					this._leaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderText");
				}
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06001B4E RID: 6990 RVA: 0x00066998 File Offset: 0x00064B98
		// (set) Token: 0x06001B4F RID: 6991 RVA: 0x000669A0 File Offset: 0x00064BA0
		[DataSourceProperty]
		public ClanMembersVM ClanMembers
		{
			get
			{
				return this._clanMembers;
			}
			set
			{
				if (value != this._clanMembers)
				{
					this._clanMembers = value;
					base.OnPropertyChangedWithValue<ClanMembersVM>(value, "ClanMembers");
				}
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06001B50 RID: 6992 RVA: 0x000669BE File Offset: 0x00064BBE
		// (set) Token: 0x06001B51 RID: 6993 RVA: 0x000669C6 File Offset: 0x00064BC6
		[DataSourceProperty]
		public ClanPartiesVM ClanParties
		{
			get
			{
				return this._clanParties;
			}
			set
			{
				if (value != this._clanParties)
				{
					this._clanParties = value;
					base.OnPropertyChangedWithValue<ClanPartiesVM>(value, "ClanParties");
				}
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06001B52 RID: 6994 RVA: 0x000669E4 File Offset: 0x00064BE4
		// (set) Token: 0x06001B53 RID: 6995 RVA: 0x000669EC File Offset: 0x00064BEC
		[DataSourceProperty]
		public ClanFiefsVM ClanFiefs
		{
			get
			{
				return this._clanFiefs;
			}
			set
			{
				if (value != this._clanFiefs)
				{
					this._clanFiefs = value;
					base.OnPropertyChangedWithValue<ClanFiefsVM>(value, "ClanFiefs");
				}
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06001B54 RID: 6996 RVA: 0x00066A0A File Offset: 0x00064C0A
		// (set) Token: 0x06001B55 RID: 6997 RVA: 0x00066A12 File Offset: 0x00064C12
		[DataSourceProperty]
		public ClanIncomeVM ClanIncome
		{
			get
			{
				return this._clanIncome;
			}
			set
			{
				if (value != this._clanIncome)
				{
					this._clanIncome = value;
					base.OnPropertyChangedWithValue<ClanIncomeVM>(value, "ClanIncome");
				}
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06001B56 RID: 6998 RVA: 0x00066A30 File Offset: 0x00064C30
		// (set) Token: 0x06001B57 RID: 6999 RVA: 0x00066A38 File Offset: 0x00064C38
		[DataSourceProperty]
		public bool IsMembersSelected
		{
			get
			{
				return this._isMembersSelected;
			}
			set
			{
				if (value != this._isMembersSelected)
				{
					this._isMembersSelected = value;
					base.OnPropertyChangedWithValue(value, "IsMembersSelected");
				}
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06001B58 RID: 7000 RVA: 0x00066A56 File Offset: 0x00064C56
		// (set) Token: 0x06001B59 RID: 7001 RVA: 0x00066A5E File Offset: 0x00064C5E
		[DataSourceProperty]
		public bool IsPartiesSelected
		{
			get
			{
				return this._isPartiesSelected;
			}
			set
			{
				if (value != this._isPartiesSelected)
				{
					this._isPartiesSelected = value;
					base.OnPropertyChangedWithValue(value, "IsPartiesSelected");
				}
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06001B5A RID: 7002 RVA: 0x00066A7C File Offset: 0x00064C7C
		// (set) Token: 0x06001B5B RID: 7003 RVA: 0x00066A84 File Offset: 0x00064C84
		[DataSourceProperty]
		public bool CanSwitchTabs
		{
			get
			{
				return this._canSwitchTabs;
			}
			set
			{
				if (value != this._canSwitchTabs)
				{
					this._canSwitchTabs = value;
					base.OnPropertyChangedWithValue(value, "CanSwitchTabs");
				}
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x00066AA2 File Offset: 0x00064CA2
		// (set) Token: 0x06001B5D RID: 7005 RVA: 0x00066AAA File Offset: 0x00064CAA
		[DataSourceProperty]
		public bool IsFiefsSelected
		{
			get
			{
				return this._isFiefsSelected;
			}
			set
			{
				if (value != this._isFiefsSelected)
				{
					this._isFiefsSelected = value;
					base.OnPropertyChangedWithValue(value, "IsFiefsSelected");
				}
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06001B5E RID: 7006 RVA: 0x00066AC8 File Offset: 0x00064CC8
		// (set) Token: 0x06001B5F RID: 7007 RVA: 0x00066AD0 File Offset: 0x00064CD0
		[DataSourceProperty]
		public bool IsIncomeSelected
		{
			get
			{
				return this._isIncomeSelected;
			}
			set
			{
				if (value != this._isIncomeSelected)
				{
					this._isIncomeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsIncomeSelected");
				}
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06001B60 RID: 7008 RVA: 0x00066AEE File Offset: 0x00064CEE
		// (set) Token: 0x06001B61 RID: 7009 RVA: 0x00066AF6 File Offset: 0x00064CF6
		[DataSourceProperty]
		public bool ClanIsInAKingdom
		{
			get
			{
				return this._clanIsInAKingdom;
			}
			set
			{
				if (value != this._clanIsInAKingdom)
				{
					this._clanIsInAKingdom = value;
					base.OnPropertyChangedWithValue(value, "ClanIsInAKingdom");
				}
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06001B62 RID: 7010 RVA: 0x00066B14 File Offset: 0x00064D14
		// (set) Token: 0x06001B63 RID: 7011 RVA: 0x00066B1C File Offset: 0x00064D1C
		[DataSourceProperty]
		public bool IsKingdomActionEnabled
		{
			get
			{
				return this._isKingdomActionEnabled;
			}
			set
			{
				if (value != this._isKingdomActionEnabled)
				{
					this._isKingdomActionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsKingdomActionEnabled");
				}
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06001B64 RID: 7012 RVA: 0x00066B3A File Offset: 0x00064D3A
		// (set) Token: 0x06001B65 RID: 7013 RVA: 0x00066B42 File Offset: 0x00064D42
		[DataSourceProperty]
		public bool PlayerCanChangeClanName
		{
			get
			{
				return this._playerCanChangeClanName;
			}
			set
			{
				if (value != this._playerCanChangeClanName)
				{
					this._playerCanChangeClanName = value;
					base.OnPropertyChangedWithValue(value, "PlayerCanChangeClanName");
				}
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06001B66 RID: 7014 RVA: 0x00066B60 File Offset: 0x00064D60
		// (set) Token: 0x06001B67 RID: 7015 RVA: 0x00066B68 File Offset: 0x00064D68
		[DataSourceProperty]
		public bool CanChooseBanner
		{
			get
			{
				return this._canChooseBanner;
			}
			set
			{
				if (value != this._canChooseBanner)
				{
					this._canChooseBanner = value;
					base.OnPropertyChangedWithValue(value, "CanChooseBanner");
				}
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06001B68 RID: 7016 RVA: 0x00066B86 File Offset: 0x00064D86
		// (set) Token: 0x06001B69 RID: 7017 RVA: 0x00066B8E File Offset: 0x00064D8E
		[DataSourceProperty]
		public bool IsRenownProgressComplete
		{
			get
			{
				return this._isRenownProgressComplete;
			}
			set
			{
				if (value != this._isRenownProgressComplete)
				{
					this._isRenownProgressComplete = value;
					base.OnPropertyChangedWithValue(value, "IsRenownProgressComplete");
				}
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06001B6A RID: 7018 RVA: 0x00066BAC File Offset: 0x00064DAC
		// (set) Token: 0x06001B6B RID: 7019 RVA: 0x00066BB4 File Offset: 0x00064DB4
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x00066BD7 File Offset: 0x00064DD7
		// (set) Token: 0x06001B6D RID: 7021 RVA: 0x00066BDF File Offset: 0x00064DDF
		[DataSourceProperty]
		public string CurrentRenownText
		{
			get
			{
				return this._currentRenownText;
			}
			set
			{
				if (value != this._currentRenownText)
				{
					this._currentRenownText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentRenownText");
				}
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06001B6E RID: 7022 RVA: 0x00066C02 File Offset: 0x00064E02
		// (set) Token: 0x06001B6F RID: 7023 RVA: 0x00066C0A File Offset: 0x00064E0A
		[DataSourceProperty]
		public string KingdomActionText
		{
			get
			{
				return this._kingdomActionText;
			}
			set
			{
				if (value != this._kingdomActionText)
				{
					this._kingdomActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "KingdomActionText");
				}
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06001B70 RID: 7024 RVA: 0x00066C2D File Offset: 0x00064E2D
		// (set) Token: 0x06001B71 RID: 7025 RVA: 0x00066C35 File Offset: 0x00064E35
		[DataSourceProperty]
		public int NextTierRenown
		{
			get
			{
				return this._nextTierRenown;
			}
			set
			{
				if (value != this._nextTierRenown)
				{
					this._nextTierRenown = value;
					base.OnPropertyChangedWithValue(value, "NextTierRenown");
				}
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06001B72 RID: 7026 RVA: 0x00066C53 File Offset: 0x00064E53
		// (set) Token: 0x06001B73 RID: 7027 RVA: 0x00066C5B File Offset: 0x00064E5B
		[DataSourceProperty]
		public int CurrentTier
		{
			get
			{
				return this._currentTier;
			}
			set
			{
				if (value != this._currentTier)
				{
					this._currentTier = value;
					base.OnPropertyChangedWithValue(value, "CurrentTier");
				}
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06001B74 RID: 7028 RVA: 0x00066C79 File Offset: 0x00064E79
		// (set) Token: 0x06001B75 RID: 7029 RVA: 0x00066C81 File Offset: 0x00064E81
		[DataSourceProperty]
		public int MinRenownForCurrentTier
		{
			get
			{
				return this._minRenownForCurrentTier;
			}
			set
			{
				if (value != this._minRenownForCurrentTier)
				{
					this._minRenownForCurrentTier = value;
					base.OnPropertyChangedWithValue(value, "MinRenownForCurrentTier");
				}
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06001B76 RID: 7030 RVA: 0x00066C9F File Offset: 0x00064E9F
		// (set) Token: 0x06001B77 RID: 7031 RVA: 0x00066CA7 File Offset: 0x00064EA7
		[DataSourceProperty]
		public int NextTier
		{
			get
			{
				return this._nextTier;
			}
			set
			{
				if (value != this._nextTier)
				{
					this._nextTier = value;
					base.OnPropertyChangedWithValue(value, "NextTier");
				}
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x00066CC5 File Offset: 0x00064EC5
		// (set) Token: 0x06001B79 RID: 7033 RVA: 0x00066CCD File Offset: 0x00064ECD
		[DataSourceProperty]
		public int CurrentRenown
		{
			get
			{
				return this._currentRenown;
			}
			set
			{
				if (value != this._currentRenown)
				{
					this._currentRenown = value;
					base.OnPropertyChangedWithValue(value, "CurrentRenown");
				}
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06001B7A RID: 7034 RVA: 0x00066CEB File Offset: 0x00064EEB
		// (set) Token: 0x06001B7B RID: 7035 RVA: 0x00066CF3 File Offset: 0x00064EF3
		[DataSourceProperty]
		public int CurrentTierRenownRange
		{
			get
			{
				return this._currentTierRenownRange;
			}
			set
			{
				if (value != this._currentTierRenownRange)
				{
					this._currentTierRenownRange = value;
					base.OnPropertyChangedWithValue(value, "CurrentTierRenownRange");
				}
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06001B7C RID: 7036 RVA: 0x00066D11 File Offset: 0x00064F11
		// (set) Token: 0x06001B7D RID: 7037 RVA: 0x00066D19 File Offset: 0x00064F19
		[DataSourceProperty]
		public int CurrentRenownOverPreviousTier
		{
			get
			{
				return this._currentRenownOverPreviousTier;
			}
			set
			{
				if (value != this._currentRenownOverPreviousTier)
				{
					this._currentRenownOverPreviousTier = value;
					base.OnPropertyChangedWithValue(value, "CurrentRenownOverPreviousTier");
				}
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06001B7E RID: 7038 RVA: 0x00066D37 File Offset: 0x00064F37
		// (set) Token: 0x06001B7F RID: 7039 RVA: 0x00066D3F File Offset: 0x00064F3F
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

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x00066D62 File Offset: 0x00064F62
		// (set) Token: 0x06001B81 RID: 7041 RVA: 0x00066D6A File Offset: 0x00064F6A
		[DataSourceProperty]
		public string PartiesText
		{
			get
			{
				return this._partiesText;
			}
			set
			{
				if (value != this._partiesText)
				{
					this._partiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartiesText");
				}
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x00066D8D File Offset: 0x00064F8D
		// (set) Token: 0x06001B83 RID: 7043 RVA: 0x00066D95 File Offset: 0x00064F95
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

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06001B84 RID: 7044 RVA: 0x00066DB8 File Offset: 0x00064FB8
		// (set) Token: 0x06001B85 RID: 7045 RVA: 0x00066DC0 File Offset: 0x00064FC0
		[DataSourceProperty]
		public string IncomeText
		{
			get
			{
				return this._incomeText;
			}
			set
			{
				if (value != this._incomeText)
				{
					this._incomeText = value;
					base.OnPropertyChanged("OtherText");
				}
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06001B86 RID: 7046 RVA: 0x00066DE2 File Offset: 0x00064FE2
		// (set) Token: 0x06001B87 RID: 7047 RVA: 0x00066DEA File Offset: 0x00064FEA
		[DataSourceProperty]
		public BasicTooltipViewModel RenownHint
		{
			get
			{
				return this._renownHint;
			}
			set
			{
				if (value != this._renownHint)
				{
					this._renownHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "RenownHint");
				}
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x00066E08 File Offset: 0x00065008
		// (set) Token: 0x06001B89 RID: 7049 RVA: 0x00066E10 File Offset: 0x00065010
		[DataSourceProperty]
		public HintViewModel ClanBannerHint
		{
			get
			{
				return this._clanBannerHint;
			}
			set
			{
				if (value != this._clanBannerHint)
				{
					this._clanBannerHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ClanBannerHint");
				}
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x00066E2E File Offset: 0x0006502E
		// (set) Token: 0x06001B8B RID: 7051 RVA: 0x00066E36 File Offset: 0x00065036
		[DataSourceProperty]
		public HintViewModel ChangeClanNameHint
		{
			get
			{
				return this._changeClanNameHint;
			}
			set
			{
				if (value != this._changeClanNameHint)
				{
					this._changeClanNameHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ChangeClanNameHint");
				}
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06001B8C RID: 7052 RVA: 0x00066E54 File Offset: 0x00065054
		// (set) Token: 0x06001B8D RID: 7053 RVA: 0x00066E5C File Offset: 0x0006505C
		[DataSourceProperty]
		public BasicTooltipViewModel KingdomActionDisabledReasonHint
		{
			get
			{
				return this._kingdomActionDisabledReasonHint;
			}
			set
			{
				if (value != this._kingdomActionDisabledReasonHint)
				{
					this._kingdomActionDisabledReasonHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "KingdomActionDisabledReasonHint");
				}
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06001B8E RID: 7054 RVA: 0x00066E7A File Offset: 0x0006507A
		// (set) Token: 0x06001B8F RID: 7055 RVA: 0x00066E82 File Offset: 0x00065082
		[DataSourceProperty]
		public TooltipTriggerVM GoldChangeTooltip
		{
			get
			{
				return this._goldChangeTooltip;
			}
			set
			{
				if (value != this._goldChangeTooltip)
				{
					this._goldChangeTooltip = value;
					base.OnPropertyChangedWithValue<TooltipTriggerVM>(value, "GoldChangeTooltip");
				}
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06001B90 RID: 7056 RVA: 0x00066EA0 File Offset: 0x000650A0
		// (set) Token: 0x06001B91 RID: 7057 RVA: 0x00066EA8 File Offset: 0x000650A8
		[DataSourceProperty]
		public string CurrentGoldText
		{
			get
			{
				return this._currentGoldText;
			}
			set
			{
				if (value != this._currentGoldText)
				{
					this._currentGoldText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentGoldText");
				}
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x00066ECB File Offset: 0x000650CB
		// (set) Token: 0x06001B93 RID: 7059 RVA: 0x00066ED3 File Offset: 0x000650D3
		[DataSourceProperty]
		public int CurrentGold
		{
			get
			{
				return this._currentGold;
			}
			set
			{
				if (value != this._currentGold)
				{
					this._currentGold = value;
					base.OnPropertyChangedWithValue(value, "CurrentGold");
				}
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06001B94 RID: 7060 RVA: 0x00066EF1 File Offset: 0x000650F1
		// (set) Token: 0x06001B95 RID: 7061 RVA: 0x00066EF9 File Offset: 0x000650F9
		[DataSourceProperty]
		public string ExpenseText
		{
			get
			{
				return this._expenseText;
			}
			set
			{
				if (value != this._expenseText)
				{
					this._expenseText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExpenseText");
				}
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x00066F1C File Offset: 0x0006511C
		// (set) Token: 0x06001B97 RID: 7063 RVA: 0x00066F24 File Offset: 0x00065124
		[DataSourceProperty]
		public string TotalIncomeText
		{
			get
			{
				return this._totalIncomeText;
			}
			set
			{
				if (value != this._totalIncomeText)
				{
					this._totalIncomeText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalIncomeText");
				}
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06001B98 RID: 7064 RVA: 0x00066F47 File Offset: 0x00065147
		// (set) Token: 0x06001B99 RID: 7065 RVA: 0x00066F4F File Offset: 0x0006514F
		[DataSourceProperty]
		public string FinanceText
		{
			get
			{
				return this._financeText;
			}
			set
			{
				if (value != this._financeText)
				{
					this._financeText = value;
					base.OnPropertyChangedWithValue<string>(value, "FinanceText");
				}
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x00066F72 File Offset: 0x00065172
		// (set) Token: 0x06001B9B RID: 7067 RVA: 0x00066F7A File Offset: 0x0006517A
		[DataSourceProperty]
		public int TotalIncome
		{
			get
			{
				return this._totalIncome;
			}
			set
			{
				if (value != this._totalIncome)
				{
					this._totalIncome = value;
					base.OnPropertyChangedWithValue(value, "TotalIncome");
				}
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x00066F98 File Offset: 0x00065198
		// (set) Token: 0x06001B9D RID: 7069 RVA: 0x00066FA0 File Offset: 0x000651A0
		[DataSourceProperty]
		public string TotalExpensesText
		{
			get
			{
				return this._totalExpensesText;
			}
			set
			{
				if (value != this._totalExpensesText)
				{
					this._totalExpensesText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalExpensesText");
				}
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06001B9E RID: 7070 RVA: 0x00066FC3 File Offset: 0x000651C3
		// (set) Token: 0x06001B9F RID: 7071 RVA: 0x00066FCB File Offset: 0x000651CB
		[DataSourceProperty]
		public int TotalExpenses
		{
			get
			{
				return this._totalExpenses;
			}
			set
			{
				if (value != this._totalExpenses)
				{
					this._totalExpenses = value;
					base.OnPropertyChangedWithValue(value, "TotalExpenses");
				}
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06001BA0 RID: 7072 RVA: 0x00066FE9 File Offset: 0x000651E9
		// (set) Token: 0x06001BA1 RID: 7073 RVA: 0x00066FF1 File Offset: 0x000651F1
		[DataSourceProperty]
		public string DailyChangeText
		{
			get
			{
				return this._dailyChangeText;
			}
			set
			{
				if (value != this._dailyChangeText)
				{
					this._dailyChangeText = value;
					base.OnPropertyChangedWithValue<string>(value, "DailyChangeText");
				}
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06001BA2 RID: 7074 RVA: 0x00067014 File Offset: 0x00065214
		// (set) Token: 0x06001BA3 RID: 7075 RVA: 0x0006701C File Offset: 0x0006521C
		[DataSourceProperty]
		public int DailyChange
		{
			get
			{
				return this._dailyChange;
			}
			set
			{
				if (value != this._dailyChange)
				{
					this._dailyChange = value;
					base.OnPropertyChangedWithValue(value, "DailyChange");
				}
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06001BA4 RID: 7076 RVA: 0x0006703A File Offset: 0x0006523A
		// (set) Token: 0x06001BA5 RID: 7077 RVA: 0x00067042 File Offset: 0x00065242
		[DataSourceProperty]
		public string ExpectedGoldText
		{
			get
			{
				return this._expectedGoldText;
			}
			set
			{
				if (value != this._expectedGoldText)
				{
					this._expectedGoldText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExpectedGoldText");
				}
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x00067065 File Offset: 0x00065265
		// (set) Token: 0x06001BA7 RID: 7079 RVA: 0x0006706D File Offset: 0x0006526D
		[DataSourceProperty]
		public int ExpectedGold
		{
			get
			{
				return this._expectedGold;
			}
			set
			{
				if (value != this._expectedGold)
				{
					this._expectedGold = value;
					base.OnPropertyChangedWithValue(value, "ExpectedGold");
				}
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x0006708B File Offset: 0x0006528B
		// (set) Token: 0x06001BA9 RID: 7081 RVA: 0x00067093 File Offset: 0x00065293
		[DataSourceProperty]
		public string DailyChangeValueText
		{
			get
			{
				return this._dailyChangeValueText;
			}
			set
			{
				if (value != this._dailyChangeValueText)
				{
					this._dailyChangeValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "DailyChangeValueText");
				}
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x000670B6 File Offset: 0x000652B6
		// (set) Token: 0x06001BAB RID: 7083 RVA: 0x000670BE File Offset: 0x000652BE
		[DataSourceProperty]
		public string TotalExpensesValueText
		{
			get
			{
				return this._totalExpensesValueText;
			}
			set
			{
				if (value != this._totalExpensesValueText)
				{
					this._totalExpensesValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalExpensesValueText");
				}
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06001BAC RID: 7084 RVA: 0x000670E1 File Offset: 0x000652E1
		// (set) Token: 0x06001BAD RID: 7085 RVA: 0x000670E9 File Offset: 0x000652E9
		[DataSourceProperty]
		public string TotalIncomeValueText
		{
			get
			{
				return this._totalIncomeValueText;
			}
			set
			{
				if (value != this._totalIncomeValueText)
				{
					this._totalIncomeValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalIncomeValueText");
				}
			}
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0006710C File Offset: 0x0006530C
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
			this.CardSelectionPopup.SetDoneInputKey(hotkey);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x00067127 File Offset: 0x00065327
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CardSelectionPopup.SetCancelInputKey(hotkey);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x00067135 File Offset: 0x00065335
		public void SetPreviousTabInputKey(HotKey hotkey)
		{
			this.PreviousTabInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x00067144 File Offset: 0x00065344
		public void SetNextTabInputKey(HotKey hotkey)
		{
			this.NextTabInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x00067153 File Offset: 0x00065353
		// (set) Token: 0x06001BB3 RID: 7091 RVA: 0x0006715B File Offset: 0x0006535B
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x00067179 File Offset: 0x00065379
		// (set) Token: 0x06001BB5 RID: 7093 RVA: 0x00067181 File Offset: 0x00065381
		public InputKeyItemVM PreviousTabInputKey
		{
			get
			{
				return this._previousTabInputKey;
			}
			set
			{
				if (value != this._previousTabInputKey)
				{
					this._previousTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousTabInputKey");
				}
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x0006719F File Offset: 0x0006539F
		// (set) Token: 0x06001BB7 RID: 7095 RVA: 0x000671A7 File Offset: 0x000653A7
		public InputKeyItemVM NextTabInputKey
		{
			get
			{
				return this._nextTabInputKey;
			}
			set
			{
				if (value != this._nextTabInputKey)
				{
					this._nextTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextTabInputKey");
				}
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x000671C5 File Offset: 0x000653C5
		// (set) Token: 0x06001BB9 RID: 7097 RVA: 0x000671CD File Offset: 0x000653CD
		[DataSourceProperty]
		public ElementNotificationVM TutorialNotification
		{
			get
			{
				return this._tutorialNotification;
			}
			set
			{
				if (value != this._tutorialNotification)
				{
					this._tutorialNotification = value;
					base.OnPropertyChangedWithValue<ElementNotificationVM>(value, "TutorialNotification");
				}
			}
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x000671EC File Offset: 0x000653EC
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = string.Empty;
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = this._latestTutorialElementID;
					if (this._latestTutorialElementID == "RoleAssignmentWidget")
					{
						this.SetSelectedCategory(1);
					}
				}
			}
		}

		// Token: 0x04000CA1 RID: 3233
		private readonly Action _onClose;

		// Token: 0x04000CA2 RID: 3234
		private readonly Action _openBannerEditor;

		// Token: 0x04000CA3 RID: 3235
		private readonly Action<Hero> _openPartyAsManage;

		// Token: 0x04000CA4 RID: 3236
		private readonly Action<Hero> _showHeroOnMap;

		// Token: 0x04000CA5 RID: 3237
		private readonly Clan _clan;

		// Token: 0x04000CA6 RID: 3238
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x04000CA7 RID: 3239
		private readonly int _categoryCount;

		// Token: 0x04000CA8 RID: 3240
		private int _currentCategory;

		// Token: 0x04000CA9 RID: 3241
		private ClanMembersVM _clanMembers;

		// Token: 0x04000CAA RID: 3242
		private ClanPartiesVM _clanParties;

		// Token: 0x04000CAB RID: 3243
		private ClanFiefsVM _clanFiefs;

		// Token: 0x04000CAC RID: 3244
		private ClanIncomeVM _clanIncome;

		// Token: 0x04000CAD RID: 3245
		private HeroVM _leader;

		// Token: 0x04000CAE RID: 3246
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000CAF RID: 3247
		private ClanCardSelectionPopupVM _cardSelectionPopup;

		// Token: 0x04000CB0 RID: 3248
		private bool _canSwitchTabs;

		// Token: 0x04000CB1 RID: 3249
		private bool _isPartiesSelected;

		// Token: 0x04000CB2 RID: 3250
		private bool _isMembersSelected;

		// Token: 0x04000CB3 RID: 3251
		private bool _isFiefsSelected;

		// Token: 0x04000CB4 RID: 3252
		private bool _isIncomeSelected;

		// Token: 0x04000CB5 RID: 3253
		private bool _canChooseBanner;

		// Token: 0x04000CB6 RID: 3254
		private bool _isRenownProgressComplete;

		// Token: 0x04000CB7 RID: 3255
		private bool _playerCanChangeClanName;

		// Token: 0x04000CB8 RID: 3256
		private bool _clanIsInAKingdom;

		// Token: 0x04000CB9 RID: 3257
		private string _doneLbl;

		// Token: 0x04000CBA RID: 3258
		private bool _isKingdomActionEnabled;

		// Token: 0x04000CBB RID: 3259
		private string _name;

		// Token: 0x04000CBC RID: 3260
		private string _kingdomActionText;

		// Token: 0x04000CBD RID: 3261
		private string _leaderText;

		// Token: 0x04000CBE RID: 3262
		private int _minRenownForCurrentTier;

		// Token: 0x04000CBF RID: 3263
		private int _currentRenown;

		// Token: 0x04000CC0 RID: 3264
		private int _currentTier = -1;

		// Token: 0x04000CC1 RID: 3265
		private int _nextTierRenown;

		// Token: 0x04000CC2 RID: 3266
		private int _nextTier;

		// Token: 0x04000CC3 RID: 3267
		private int _currentTierRenownRange;

		// Token: 0x04000CC4 RID: 3268
		private int _currentRenownOverPreviousTier;

		// Token: 0x04000CC5 RID: 3269
		private string _currentRenownText;

		// Token: 0x04000CC6 RID: 3270
		private string _membersText;

		// Token: 0x04000CC7 RID: 3271
		private string _partiesText;

		// Token: 0x04000CC8 RID: 3272
		private string _fiefsText;

		// Token: 0x04000CC9 RID: 3273
		private string _incomeText;

		// Token: 0x04000CCA RID: 3274
		private BasicTooltipViewModel _renownHint;

		// Token: 0x04000CCB RID: 3275
		private BasicTooltipViewModel _kingdomActionDisabledReasonHint;

		// Token: 0x04000CCC RID: 3276
		private HintViewModel _clanBannerHint;

		// Token: 0x04000CCD RID: 3277
		private HintViewModel _changeClanNameHint;

		// Token: 0x04000CCE RID: 3278
		private string _financeText;

		// Token: 0x04000CCF RID: 3279
		private string _currentGoldText;

		// Token: 0x04000CD0 RID: 3280
		private int _currentGold;

		// Token: 0x04000CD1 RID: 3281
		private string _totalIncomeText;

		// Token: 0x04000CD2 RID: 3282
		private int _totalIncome;

		// Token: 0x04000CD3 RID: 3283
		private string _totalIncomeValueText;

		// Token: 0x04000CD4 RID: 3284
		private string _totalExpensesText;

		// Token: 0x04000CD5 RID: 3285
		private int _totalExpenses;

		// Token: 0x04000CD6 RID: 3286
		private string _totalExpensesValueText;

		// Token: 0x04000CD7 RID: 3287
		private string _dailyChangeText;

		// Token: 0x04000CD8 RID: 3288
		private int _dailyChange;

		// Token: 0x04000CD9 RID: 3289
		private string _dailyChangeValueText;

		// Token: 0x04000CDA RID: 3290
		private string _expectedGoldText;

		// Token: 0x04000CDB RID: 3291
		private int _expectedGold;

		// Token: 0x04000CDC RID: 3292
		private string _expenseText;

		// Token: 0x04000CDD RID: 3293
		private TooltipTriggerVM _goldChangeTooltip;

		// Token: 0x04000CDE RID: 3294
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000CDF RID: 3295
		private InputKeyItemVM _previousTabInputKey;

		// Token: 0x04000CE0 RID: 3296
		private InputKeyItemVM _nextTabInputKey;

		// Token: 0x04000CE1 RID: 3297
		private ElementNotificationVM _tutorialNotification;

		// Token: 0x04000CE2 RID: 3298
		private string _latestTutorialElementID;
	}
}
