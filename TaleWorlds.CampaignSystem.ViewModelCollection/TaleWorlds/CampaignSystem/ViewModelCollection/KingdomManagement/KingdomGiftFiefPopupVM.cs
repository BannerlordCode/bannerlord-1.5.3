using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x02000069 RID: 105
	public class KingdomGiftFiefPopupVM : ViewModel
	{
		// Token: 0x06000782 RID: 1922 RVA: 0x00023DA6 File Offset: 0x00021FA6
		public KingdomGiftFiefPopupVM(Action onSettlementGranted)
		{
			this._clans = new MBBindingList<KingdomClanItemVM>();
			this._onSettlementGranted = onSettlementGranted;
			this.ClanSortController = new KingdomClanSortControllerVM(ref this._clans);
			this.RefreshValues();
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00023DD8 File Offset: 0x00021FD8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=rOKAvjtT}Gift Settlement", null).ToString();
			this.GiftText = GameTexts.FindText("str_gift", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.NameText = GameTexts.FindText("str_scoreboard_header", "name").ToString();
			this.InfluenceText = GameTexts.FindText("str_influence", null).ToString();
			this.FiefsText = GameTexts.FindText("str_fiefs", null).ToString();
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.BannerText = GameTexts.FindText("str_banner", null).ToString();
			this.TypeText = GameTexts.FindText("str_sort_by_type_label", null).ToString();
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00023EB5 File Offset: 0x000220B5
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
				this.IsAnyClanSelected = true;
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00023EF0 File Offset: 0x000220F0
		private void RefreshClanList()
		{
			this.Clans.Clear();
			foreach (Clan clan in Clan.PlayerClan.Kingdom.Clans)
			{
				if (FactionHelper.CanClanBeGrantedFief(clan))
				{
					this.Clans.Add(new KingdomClanItemVM(clan, new Action<KingdomClanItemVM>(this.SetCurrentSelectedClan)));
				}
			}
			if (this.Clans.Count > 0)
			{
				this.SetCurrentSelectedClan(this.Clans[0]);
			}
			if (this.ClanSortController != null)
			{
				this.ClanSortController.SortByCurrentState();
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00023FA8 File Offset: 0x000221A8
		public void OpenWith(Settlement settlement)
		{
			this._settlementToGive = settlement;
			this.RefreshClanList();
			this.IsOpen = true;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00023FC0 File Offset: 0x000221C0
		public void ExecuteGiftSettlement()
		{
			if (this._settlementToGive != null && this.CurrentSelectedClan != null)
			{
				Campaign.Current.KingdomManager.GiftSettlementOwnership(this._settlementToGive, this.CurrentSelectedClan.Clan);
				this.ExecuteClose();
				this._onSettlementGranted();
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002400E File Offset: 0x0002220E
		public void ExecuteClose()
		{
			this._settlementToGive = null;
			this.IsOpen = false;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002401E File Offset: 0x0002221E
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00024047 File Offset: 0x00022247
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00024056 File Offset: 0x00022256
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x00024065 File Offset: 0x00022265
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x0002406D File Offset: 0x0002226D
		[DataSourceProperty]
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x0002408B File Offset: 0x0002228B
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x00024093 File Offset: 0x00022293
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x000240B1 File Offset: 0x000222B1
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x000240B9 File Offset: 0x000222B9
		[DataSourceProperty]
		public bool IsAnyClanSelected
		{
			get
			{
				return this._isAnyClanSelected;
			}
			set
			{
				if (value != this._isAnyClanSelected)
				{
					this._isAnyClanSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyClanSelected");
				}
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x000240D7 File Offset: 0x000222D7
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x000240DF File Offset: 0x000222DF
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

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x000240FD File Offset: 0x000222FD
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x00024105 File Offset: 0x00022305
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00024123 File Offset: 0x00022323
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x0002412B File Offset: 0x0002232B
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

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00024149 File Offset: 0x00022349
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x00024151 File Offset: 0x00022351
		[DataSourceProperty]
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
			set
			{
				if (value != this._isOpen)
				{
					this._isOpen = value;
					base.OnPropertyChangedWithValue(value, "IsOpen");
				}
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0002416F File Offset: 0x0002236F
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x00024177 File Offset: 0x00022377
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0002419A File Offset: 0x0002239A
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x000241A2 File Offset: 0x000223A2
		[DataSourceProperty]
		public string GiftText
		{
			get
			{
				return this._giftText;
			}
			set
			{
				if (value != this._giftText)
				{
					this._giftText = value;
					base.OnPropertyChangedWithValue<string>(value, "GiftText");
				}
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000241C5 File Offset: 0x000223C5
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x000241CD File Offset: 0x000223CD
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000241F0 File Offset: 0x000223F0
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x000241F8 File Offset: 0x000223F8
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

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0002421B File Offset: 0x0002241B
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x00024223 File Offset: 0x00022423
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

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00024246 File Offset: 0x00022446
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0002424E File Offset: 0x0002244E
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

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00024271 File Offset: 0x00022471
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x00024279 File Offset: 0x00022479
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

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0002429C File Offset: 0x0002249C
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x000242A4 File Offset: 0x000224A4
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

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x000242C7 File Offset: 0x000224C7
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x000242CF File Offset: 0x000224CF
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

		// Token: 0x04000339 RID: 825
		private Settlement _settlementToGive;

		// Token: 0x0400033A RID: 826
		private Action _onSettlementGranted;

		// Token: 0x0400033B RID: 827
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400033C RID: 828
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400033D RID: 829
		private bool _isAnyClanSelected;

		// Token: 0x0400033E RID: 830
		private MBBindingList<KingdomClanItemVM> _clans;

		// Token: 0x0400033F RID: 831
		private KingdomClanItemVM _currentSelectedClan;

		// Token: 0x04000340 RID: 832
		private KingdomClanSortControllerVM _clanSortController;

		// Token: 0x04000341 RID: 833
		private bool _isOpen;

		// Token: 0x04000342 RID: 834
		private string _titleText;

		// Token: 0x04000343 RID: 835
		private string _giftText;

		// Token: 0x04000344 RID: 836
		private string _cancelText;

		// Token: 0x04000345 RID: 837
		private string _bannerText;

		// Token: 0x04000346 RID: 838
		private string _nameText;

		// Token: 0x04000347 RID: 839
		private string _influenceText;

		// Token: 0x04000348 RID: 840
		private string _membersText;

		// Token: 0x04000349 RID: 841
		private string _fiefsText;

		// Token: 0x0400034A RID: 842
		private string _typeText;
	}
}
