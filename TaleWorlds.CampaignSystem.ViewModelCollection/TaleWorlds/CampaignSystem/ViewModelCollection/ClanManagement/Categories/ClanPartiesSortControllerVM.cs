using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x02000143 RID: 323
	public class ClanPartiesSortControllerVM : ViewModel
	{
		// Token: 0x06001EF4 RID: 7924 RVA: 0x0006FB57 File Offset: 0x0006DD57
		public ClanPartiesSortControllerVM(MBBindingList<MBBindingList<ClanPartyItemVM>> listsToControl)
		{
			this._listsToControl = listsToControl;
			this._nameComparer = new ClanPartiesSortControllerVM.ItemNameComparer();
			this._locationComparer = new ClanPartiesSortControllerVM.ItemLocationComparer();
			this._sizeComparer = new ClanPartiesSortControllerVM.ItemSizeComparer();
			this._shipCountComparer = new ClanPartiesSortControllerVM.ItemShipCountComparer();
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0006FB94 File Offset: 0x0006DD94
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			this.SizeText = GameTexts.FindText("str_clan_party_size", null).ToString();
			this.ShipCountText = new TextObject("{=7Q8ufo5X}Ships", null).ToString();
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0006FC00 File Offset: 0x0006DE00
		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				int nameState2 = this.NameState;
				this.NameState = nameState2 + 1;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			foreach (MBBindingList<ClanPartyItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._nameComparer);
			}
			this.IsNameSelected = true;
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0006FC9C File Offset: 0x0006DE9C
		public void ExecuteSortByLocation()
		{
			int locationState = this.LocationState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.LocationState = (locationState + 1) % 3;
			if (this.LocationState == 0)
			{
				int locationState2 = this.LocationState;
				this.LocationState = locationState2 + 1;
			}
			this._locationComparer.SetSortMode(this.LocationState == 1);
			foreach (MBBindingList<ClanPartyItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._locationComparer);
			}
			this.IsLocationSelected = true;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0006FD38 File Offset: 0x0006DF38
		public void ExecuteSortBySize()
		{
			int sizeState = this.SizeState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.SizeState = (sizeState + 1) % 3;
			if (this.SizeState == 0)
			{
				int sizeState2 = this.SizeState;
				this.SizeState = sizeState2 + 1;
			}
			this._sizeComparer.SetSortMode(this.SizeState == 1);
			foreach (MBBindingList<ClanPartyItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._sizeComparer);
			}
			this.IsSizeSelected = true;
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0006FDD4 File Offset: 0x0006DFD4
		public void ExecuteSortByShipCount()
		{
			int shipCountState = this.ShipCountState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ShipCountState = (shipCountState + 1) % 3;
			if (this.ShipCountState == 0)
			{
				int shipCountState2 = this.ShipCountState;
				this.ShipCountState = shipCountState2 + 1;
			}
			this._shipCountComparer.SetSortMode(this.ShipCountState == 1);
			foreach (MBBindingList<ClanPartyItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._shipCountComparer);
			}
			this.IsShipCountSelected = true;
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0006FE70 File Offset: 0x0006E070
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.LocationState = (int)state;
			this.SizeState = (int)state;
			this.ShipCountState = (int)state;
			this.IsNameSelected = false;
			this.IsLocationSelected = false;
			this.IsSizeSelected = false;
			this.IsShipCountSelected = false;
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0006FEAA File Offset: 0x0006E0AA
		public void ResetAllStates()
		{
			this.SetAllStates(CampaignUIHelper.SortState.Default);
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x0006FEB3 File Offset: 0x0006E0B3
		// (set) Token: 0x06001EFD RID: 7933 RVA: 0x0006FEBB File Offset: 0x0006E0BB
		[DataSourceProperty]
		public int NameState
		{
			get
			{
				return this._nameState;
			}
			set
			{
				if (value != this._nameState)
				{
					this._nameState = value;
					base.OnPropertyChangedWithValue(value, "NameState");
				}
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x0006FED9 File Offset: 0x0006E0D9
		// (set) Token: 0x06001EFF RID: 7935 RVA: 0x0006FEE1 File Offset: 0x0006E0E1
		[DataSourceProperty]
		public int LocationState
		{
			get
			{
				return this._locationState;
			}
			set
			{
				if (value != this._locationState)
				{
					this._locationState = value;
					base.OnPropertyChangedWithValue(value, "LocationState");
				}
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x0006FEFF File Offset: 0x0006E0FF
		// (set) Token: 0x06001F01 RID: 7937 RVA: 0x0006FF07 File Offset: 0x0006E107
		[DataSourceProperty]
		public int SizeState
		{
			get
			{
				return this._sizeState;
			}
			set
			{
				if (value != this._sizeState)
				{
					this._sizeState = value;
					base.OnPropertyChangedWithValue(value, "SizeState");
				}
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0006FF25 File Offset: 0x0006E125
		// (set) Token: 0x06001F03 RID: 7939 RVA: 0x0006FF2D File Offset: 0x0006E12D
		[DataSourceProperty]
		public int ShipCountState
		{
			get
			{
				return this._shipCountState;
			}
			set
			{
				if (value != this._shipCountState)
				{
					this._shipCountState = value;
					base.OnPropertyChangedWithValue(value, "ShipCountState");
				}
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0006FF4B File Offset: 0x0006E14B
		// (set) Token: 0x06001F05 RID: 7941 RVA: 0x0006FF53 File Offset: 0x0006E153
		[DataSourceProperty]
		public bool IsNameSelected
		{
			get
			{
				return this._isNameSelected;
			}
			set
			{
				if (value != this._isNameSelected)
				{
					this._isNameSelected = value;
					base.OnPropertyChangedWithValue(value, "IsNameSelected");
				}
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x0006FF71 File Offset: 0x0006E171
		// (set) Token: 0x06001F07 RID: 7943 RVA: 0x0006FF79 File Offset: 0x0006E179
		[DataSourceProperty]
		public bool IsLocationSelected
		{
			get
			{
				return this._isLocationSelected;
			}
			set
			{
				if (value != this._isLocationSelected)
				{
					this._isLocationSelected = value;
					base.OnPropertyChangedWithValue(value, "IsLocationSelected");
				}
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x0006FF97 File Offset: 0x0006E197
		// (set) Token: 0x06001F09 RID: 7945 RVA: 0x0006FF9F File Offset: 0x0006E19F
		[DataSourceProperty]
		public bool IsSizeSelected
		{
			get
			{
				return this._isSizeSelected;
			}
			set
			{
				if (value != this._isSizeSelected)
				{
					this._isSizeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSizeSelected");
				}
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x0006FFBD File Offset: 0x0006E1BD
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x0006FFC5 File Offset: 0x0006E1C5
		[DataSourceProperty]
		public bool IsShipCountSelected
		{
			get
			{
				return this._isShipCountSelected;
			}
			set
			{
				if (value != this._isShipCountSelected)
				{
					this._isShipCountSelected = value;
					base.OnPropertyChangedWithValue(value, "IsShipCountSelected");
				}
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x0006FFE3 File Offset: 0x0006E1E3
		// (set) Token: 0x06001F0D RID: 7949 RVA: 0x0006FFEB File Offset: 0x0006E1EB
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

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x0007000E File Offset: 0x0006E20E
		// (set) Token: 0x06001F0F RID: 7951 RVA: 0x00070016 File Offset: 0x0006E216
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x00070039 File Offset: 0x0006E239
		// (set) Token: 0x06001F11 RID: 7953 RVA: 0x00070041 File Offset: 0x0006E241
		[DataSourceProperty]
		public string SizeText
		{
			get
			{
				return this._sizeText;
			}
			set
			{
				if (value != this._sizeText)
				{
					this._sizeText = value;
					base.OnPropertyChangedWithValue<string>(value, "SizeText");
				}
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x00070064 File Offset: 0x0006E264
		// (set) Token: 0x06001F13 RID: 7955 RVA: 0x0007006C File Offset: 0x0006E26C
		[DataSourceProperty]
		public string ShipCountText
		{
			get
			{
				return this._shipCountText;
			}
			set
			{
				if (value != this._shipCountText)
				{
					this._shipCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShipCountText");
				}
			}
		}

		// Token: 0x04000E2F RID: 3631
		private readonly MBBindingList<MBBindingList<ClanPartyItemVM>> _listsToControl;

		// Token: 0x04000E30 RID: 3632
		private readonly ClanPartiesSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000E31 RID: 3633
		private readonly ClanPartiesSortControllerVM.ItemLocationComparer _locationComparer;

		// Token: 0x04000E32 RID: 3634
		private readonly ClanPartiesSortControllerVM.ItemSizeComparer _sizeComparer;

		// Token: 0x04000E33 RID: 3635
		private readonly ClanPartiesSortControllerVM.ItemShipCountComparer _shipCountComparer;

		// Token: 0x04000E34 RID: 3636
		private int _nameState;

		// Token: 0x04000E35 RID: 3637
		private int _locationState;

		// Token: 0x04000E36 RID: 3638
		private int _sizeState;

		// Token: 0x04000E37 RID: 3639
		private int _shipCountState;

		// Token: 0x04000E38 RID: 3640
		private bool _isNameSelected;

		// Token: 0x04000E39 RID: 3641
		private bool _isLocationSelected;

		// Token: 0x04000E3A RID: 3642
		private bool _isSizeSelected;

		// Token: 0x04000E3B RID: 3643
		private bool _isShipCountSelected;

		// Token: 0x04000E3C RID: 3644
		private string _nameText;

		// Token: 0x04000E3D RID: 3645
		private string _locationText;

		// Token: 0x04000E3E RID: 3646
		private string _sizeText;

		// Token: 0x04000E3F RID: 3647
		private string _shipCountText;

		// Token: 0x020002BF RID: 703
		public abstract class ItemComparerBase : IComparer<ClanPartyItemVM>
		{
			// Token: 0x060027B0 RID: 10160 RVA: 0x00085726 File Offset: 0x00083926
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x060027B1 RID: 10161
			public abstract int Compare(ClanPartyItemVM x, ClanPartyItemVM y);

			// Token: 0x040013A4 RID: 5028
			protected bool _isAcending;
		}

		// Token: 0x020002C0 RID: 704
		public class ItemNameComparer : ClanPartiesSortControllerVM.ItemComparerBase
		{
			// Token: 0x060027B3 RID: 10163 RVA: 0x00085737 File Offset: 0x00083937
			public override int Compare(ClanPartyItemVM x, ClanPartyItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002C1 RID: 705
		public class ItemLocationComparer : ClanPartiesSortControllerVM.ItemComparerBase
		{
			// Token: 0x060027B5 RID: 10165 RVA: 0x00085770 File Offset: 0x00083970
			public override int Compare(ClanPartyItemVM x, ClanPartyItemVM y)
			{
				int num = this.GetDistanceToMainParty(y).CompareTo(this.GetDistanceToMainParty(x));
				if (this._isAcending)
				{
					return num * -1;
				}
				return num;
			}

			// Token: 0x060027B6 RID: 10166 RVA: 0x000857A4 File Offset: 0x000839A4
			private float GetDistanceToMainParty(ClanPartyItemVM item)
			{
				if (!item.Position.IsValid())
				{
					return float.MaxValue;
				}
				return item.Position.Distance(Hero.MainHero.GetCampaignPosition());
			}
		}

		// Token: 0x020002C2 RID: 706
		public class ItemSizeComparer : ClanPartiesSortControllerVM.ItemComparerBase
		{
			// Token: 0x060027B8 RID: 10168 RVA: 0x000857E8 File Offset: 0x000839E8
			public override int Compare(ClanPartyItemVM x, ClanPartyItemVM y)
			{
				PartyBase party = x.Party;
				int num = ((party != null) ? party.MobileParty.MemberRoster.TotalManCount : 0);
				PartyBase party2 = y.Party;
				int num2 = ((party2 != null) ? party2.MobileParty.MemberRoster.TotalManCount : 0);
				if (this._isAcending)
				{
					return num2.CompareTo(num) * -1;
				}
				return num2.CompareTo(num);
			}
		}

		// Token: 0x020002C3 RID: 707
		public class ItemShipCountComparer : ClanPartiesSortControllerVM.ItemComparerBase
		{
			// Token: 0x060027BA RID: 10170 RVA: 0x00085854 File Offset: 0x00083A54
			public override int Compare(ClanPartyItemVM x, ClanPartyItemVM y)
			{
				PartyBase party = x.Party;
				int num = ((party != null) ? party.Ships.Count : 0);
				PartyBase party2 = y.Party;
				int num2 = ((party2 != null) ? party2.Ships.Count : 0);
				if (this._isAcending)
				{
					return num2.CompareTo(num) * -1;
				}
				return num2.CompareTo(num);
			}
		}
	}
}
