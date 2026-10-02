using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x02000141 RID: 321
	public class ClanMembersSortControllerVM : ViewModel
	{
		// Token: 0x06001EBF RID: 7871 RVA: 0x0006EF2C File Offset: 0x0006D12C
		public ClanMembersSortControllerVM(MBBindingList<MBBindingList<ClanLordItemVM>> listsToControl)
		{
			this._listsToControl = listsToControl;
			this._nameComparer = new ClanMembersSortControllerVM.ItemNameComparer();
			this._locationComparer = new ClanMembersSortControllerVM.ItemLocationComparer();
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0006EF51 File Offset: 0x0006D151
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0006EF88 File Offset: 0x0006D188
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
			foreach (MBBindingList<ClanLordItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._nameComparer);
			}
			this.IsNameSelected = true;
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0006F024 File Offset: 0x0006D224
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
			foreach (MBBindingList<ClanLordItemVM> mbbindingList in this._listsToControl)
			{
				mbbindingList.Sort(this._locationComparer);
			}
			this.IsLocationSelected = true;
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0006F0C0 File Offset: 0x0006D2C0
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.LocationState = (int)state;
			this.IsNameSelected = false;
			this.IsLocationSelected = false;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0006F0DE File Offset: 0x0006D2DE
		public void ResetAllStates()
		{
			this.SetAllStates(CampaignUIHelper.SortState.Default);
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x0006F0E7 File Offset: 0x0006D2E7
		// (set) Token: 0x06001EC6 RID: 7878 RVA: 0x0006F0EF File Offset: 0x0006D2EF
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

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x0006F10D File Offset: 0x0006D30D
		// (set) Token: 0x06001EC8 RID: 7880 RVA: 0x0006F115 File Offset: 0x0006D315
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

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x0006F133 File Offset: 0x0006D333
		// (set) Token: 0x06001ECA RID: 7882 RVA: 0x0006F13B File Offset: 0x0006D33B
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

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x0006F159 File Offset: 0x0006D359
		// (set) Token: 0x06001ECC RID: 7884 RVA: 0x0006F161 File Offset: 0x0006D361
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

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x0006F17F File Offset: 0x0006D37F
		// (set) Token: 0x06001ECE RID: 7886 RVA: 0x0006F187 File Offset: 0x0006D387
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

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x0006F1AA File Offset: 0x0006D3AA
		// (set) Token: 0x06001ED0 RID: 7888 RVA: 0x0006F1B2 File Offset: 0x0006D3B2
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

		// Token: 0x04000E16 RID: 3606
		private readonly MBBindingList<MBBindingList<ClanLordItemVM>> _listsToControl;

		// Token: 0x04000E17 RID: 3607
		private readonly ClanMembersSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000E18 RID: 3608
		private readonly ClanMembersSortControllerVM.ItemLocationComparer _locationComparer;

		// Token: 0x04000E19 RID: 3609
		private int _nameState;

		// Token: 0x04000E1A RID: 3610
		private int _locationState;

		// Token: 0x04000E1B RID: 3611
		private bool _isNameSelected;

		// Token: 0x04000E1C RID: 3612
		private bool _isLocationSelected;

		// Token: 0x04000E1D RID: 3613
		private string _nameText;

		// Token: 0x04000E1E RID: 3614
		private string _locationText;

		// Token: 0x020002BB RID: 699
		public abstract class ItemComparerBase : IComparer<ClanLordItemVM>
		{
			// Token: 0x060027A1 RID: 10145 RVA: 0x0008563C File Offset: 0x0008383C
			public void SetSortMode(bool isAcending)
			{
				this._isAcending = isAcending;
			}

			// Token: 0x060027A2 RID: 10146
			public abstract int Compare(ClanLordItemVM x, ClanLordItemVM y);

			// Token: 0x0400139D RID: 5021
			protected bool _isAcending;
		}

		// Token: 0x020002BC RID: 700
		public class ItemNameComparer : ClanMembersSortControllerVM.ItemComparerBase
		{
			// Token: 0x060027A4 RID: 10148 RVA: 0x0008564D File Offset: 0x0008384D
			public override int Compare(ClanLordItemVM x, ClanLordItemVM y)
			{
				if (this._isAcending)
				{
					return y.Name.CompareTo(x.Name) * -1;
				}
				return y.Name.CompareTo(x.Name);
			}
		}

		// Token: 0x020002BD RID: 701
		public class ItemLocationComparer : ClanMembersSortControllerVM.ItemComparerBase
		{
			// Token: 0x060027A6 RID: 10150 RVA: 0x00085684 File Offset: 0x00083884
			public override int Compare(ClanLordItemVM x, ClanLordItemVM y)
			{
				int num = this.GetDistanceToMainHero(y).CompareTo(this.GetDistanceToMainHero(x));
				if (this._isAcending)
				{
					return num * -1;
				}
				return num;
			}

			// Token: 0x060027A7 RID: 10151 RVA: 0x000856B8 File Offset: 0x000838B8
			private float GetDistanceToMainHero(ClanLordItemVM item)
			{
				return item.GetHero().GetCampaignPosition().Distance(Hero.MainHero.GetCampaignPosition());
			}
		}
	}
}
