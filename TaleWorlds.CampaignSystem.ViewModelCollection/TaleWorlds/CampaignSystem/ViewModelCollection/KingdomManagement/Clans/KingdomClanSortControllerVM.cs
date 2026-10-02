using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans
{
	// Token: 0x0200008D RID: 141
	public class KingdomClanSortControllerVM : ViewModel
	{
		// Token: 0x06000B7B RID: 2939 RVA: 0x00030914 File Offset: 0x0002EB14
		public KingdomClanSortControllerVM(ref MBBindingList<KingdomClanItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._influenceComparer = new KingdomClanSortControllerVM.ItemInfluenceComparer();
			this._membersComparer = new KingdomClanSortControllerVM.ItemMembersComparer();
			this._nameComparer = new KingdomClanSortControllerVM.ItemNameComparer();
			this._fiefsComparer = new KingdomClanSortControllerVM.ItemFiefsComparer();
			this._typeComparer = new KingdomClanSortControllerVM.ItemTypeComparer();
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00030968 File Offset: 0x0002EB68
		public void SortByCurrentState()
		{
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				return;
			}
			if (this.IsTypeSelected)
			{
				this._listToControl.Sort(this._typeComparer);
				return;
			}
			if (this.IsInfluenceSelected)
			{
				this._listToControl.Sort(this._influenceComparer);
				return;
			}
			if (this.IsMembersSelected)
			{
				this._listToControl.Sort(this._membersComparer);
				return;
			}
			if (this.IsFiefsSelected)
			{
				this._listToControl.Sort(this._fiefsComparer);
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x000309F8 File Offset: 0x0002EBF8
		private void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				this.NameState++;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00030A60 File Offset: 0x0002EC60
		private void ExecuteSortByType()
		{
			int typeState = this.TypeState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.TypeState = (typeState + 1) % 3;
			if (this.TypeState == 0)
			{
				this.TypeState++;
			}
			this._typeComparer.SetSortMode(this.TypeState == 1);
			this._listToControl.Sort(this._typeComparer);
			this.IsTypeSelected = true;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00030AC8 File Offset: 0x0002ECC8
		private void ExecuteSortByInfluence()
		{
			int influenceState = this.InfluenceState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.InfluenceState = (influenceState + 1) % 3;
			if (this.InfluenceState == 0)
			{
				this.InfluenceState++;
			}
			this._influenceComparer.SetSortMode(this.InfluenceState == 1);
			this._listToControl.Sort(this._influenceComparer);
			this.IsInfluenceSelected = true;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00030B30 File Offset: 0x0002ED30
		private void ExecuteSortByMembers()
		{
			int membersState = this.MembersState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.MembersState = (membersState + 1) % 3;
			if (this.MembersState == 0)
			{
				this.MembersState++;
			}
			this._membersComparer.SetSortMode(this.MembersState == 1);
			this._listToControl.Sort(this._membersComparer);
			this.IsMembersSelected = true;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00030B98 File Offset: 0x0002ED98
		private void ExecuteSortByFiefs()
		{
			int fiefsState = this.FiefsState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.FiefsState = (fiefsState + 1) % 3;
			if (this.FiefsState == 0)
			{
				this.FiefsState++;
			}
			this._fiefsComparer.SetSortMode(this.FiefsState == 1);
			this._listToControl.Sort(this._fiefsComparer);
			this.IsFiefsSelected = true;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00030C00 File Offset: 0x0002EE00
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.InfluenceState = (int)state;
			this.FiefsState = (int)state;
			this.MembersState = (int)state;
			this.NameState = (int)state;
			this.TypeState = (int)state;
			this.IsInfluenceSelected = false;
			this.IsFiefsSelected = false;
			this.IsNameSelected = false;
			this.IsMembersSelected = false;
			this.IsTypeSelected = false;
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00030C53 File Offset: 0x0002EE53
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x00030C5B File Offset: 0x0002EE5B
		[DataSourceProperty]
		public int InfluenceState
		{
			get
			{
				return this._influenceState;
			}
			set
			{
				if (value != this._influenceState)
				{
					this._influenceState = value;
					base.OnPropertyChangedWithValue(value, "InfluenceState");
				}
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x00030C79 File Offset: 0x0002EE79
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x00030C81 File Offset: 0x0002EE81
		[DataSourceProperty]
		public int FiefsState
		{
			get
			{
				return this._fiefsState;
			}
			set
			{
				if (value != this._fiefsState)
				{
					this._fiefsState = value;
					base.OnPropertyChangedWithValue(value, "FiefsState");
				}
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00030C9F File Offset: 0x0002EE9F
		// (set) Token: 0x06000B88 RID: 2952 RVA: 0x00030CA7 File Offset: 0x0002EEA7
		[DataSourceProperty]
		public int MembersState
		{
			get
			{
				return this._membersState;
			}
			set
			{
				if (value != this._membersState)
				{
					this._membersState = value;
					base.OnPropertyChangedWithValue(value, "MembersState");
				}
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00030CC5 File Offset: 0x0002EEC5
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x00030CCD File Offset: 0x0002EECD
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

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00030CEB File Offset: 0x0002EEEB
		// (set) Token: 0x06000B8C RID: 2956 RVA: 0x00030CF3 File Offset: 0x0002EEF3
		[DataSourceProperty]
		public int TypeState
		{
			get
			{
				return this._typeState;
			}
			set
			{
				if (value != this._typeState)
				{
					this._typeState = value;
					base.OnPropertyChangedWithValue(value, "TypeState");
				}
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00030D11 File Offset: 0x0002EF11
		// (set) Token: 0x06000B8E RID: 2958 RVA: 0x00030D19 File Offset: 0x0002EF19
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

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x00030D37 File Offset: 0x0002EF37
		// (set) Token: 0x06000B90 RID: 2960 RVA: 0x00030D3F File Offset: 0x0002EF3F
		[DataSourceProperty]
		public bool IsTypeSelected
		{
			get
			{
				return this._isTypeSelected;
			}
			set
			{
				if (value != this._isTypeSelected)
				{
					this._isTypeSelected = value;
					base.OnPropertyChangedWithValue(value, "IsTypeSelected");
				}
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x00030D5D File Offset: 0x0002EF5D
		// (set) Token: 0x06000B92 RID: 2962 RVA: 0x00030D65 File Offset: 0x0002EF65
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

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00030D83 File Offset: 0x0002EF83
		// (set) Token: 0x06000B94 RID: 2964 RVA: 0x00030D8B File Offset: 0x0002EF8B
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

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00030DA9 File Offset: 0x0002EFA9
		// (set) Token: 0x06000B96 RID: 2966 RVA: 0x00030DB1 File Offset: 0x0002EFB1
		[DataSourceProperty]
		public bool IsInfluenceSelected
		{
			get
			{
				return this._isDistanceSelected;
			}
			set
			{
				if (value != this._isDistanceSelected)
				{
					this._isDistanceSelected = value;
					base.OnPropertyChangedWithValue(value, "IsInfluenceSelected");
				}
			}
		}

		// Token: 0x04000512 RID: 1298
		private readonly MBBindingList<KingdomClanItemVM> _listToControl;

		// Token: 0x04000513 RID: 1299
		private readonly KingdomClanSortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x04000514 RID: 1300
		private readonly KingdomClanSortControllerVM.ItemTypeComparer _typeComparer;

		// Token: 0x04000515 RID: 1301
		private readonly KingdomClanSortControllerVM.ItemInfluenceComparer _influenceComparer;

		// Token: 0x04000516 RID: 1302
		private readonly KingdomClanSortControllerVM.ItemMembersComparer _membersComparer;

		// Token: 0x04000517 RID: 1303
		private readonly KingdomClanSortControllerVM.ItemFiefsComparer _fiefsComparer;

		// Token: 0x04000518 RID: 1304
		private int _influenceState;

		// Token: 0x04000519 RID: 1305
		private int _fiefsState;

		// Token: 0x0400051A RID: 1306
		private int _membersState;

		// Token: 0x0400051B RID: 1307
		private int _nameState;

		// Token: 0x0400051C RID: 1308
		private int _typeState;

		// Token: 0x0400051D RID: 1309
		private bool _isNameSelected;

		// Token: 0x0400051E RID: 1310
		private bool _isTypeSelected;

		// Token: 0x0400051F RID: 1311
		private bool _isFiefsSelected;

		// Token: 0x04000520 RID: 1312
		private bool _isMembersSelected;

		// Token: 0x04000521 RID: 1313
		private bool _isDistanceSelected;

		// Token: 0x020001EE RID: 494
		public abstract class ItemComparerBase : IComparer<KingdomClanItemVM>
		{
			// Token: 0x0600250B RID: 9483 RVA: 0x00081180 File Offset: 0x0007F380
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x0600250C RID: 9484
			public abstract int Compare(KingdomClanItemVM x, KingdomClanItemVM y);

			// Token: 0x0600250D RID: 9485 RVA: 0x00081189 File Offset: 0x0007F389
			protected int ResolveEquality(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				return x.Clan.Name.ToString().CompareTo(y.Clan.Name.ToString());
			}

			// Token: 0x040011A1 RID: 4513
			protected bool _isAscending;
		}

		// Token: 0x020001EF RID: 495
		public class ItemNameComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x0600250F RID: 9487 RVA: 0x000811B8 File Offset: 0x0007F3B8
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				if (this._isAscending)
				{
					return y.Clan.Name.ToString().CompareTo(x.Clan.Name.ToString()) * -1;
				}
				return y.Clan.Name.ToString().CompareTo(x.Clan.Name.ToString());
			}
		}

		// Token: 0x020001F0 RID: 496
		public class ItemTypeComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002511 RID: 9489 RVA: 0x00081224 File Offset: 0x0007F424
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				int num = y.ClanType.CompareTo(x.ClanType);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001F1 RID: 497
		public class ItemInfluenceComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002513 RID: 9491 RVA: 0x00081268 File Offset: 0x0007F468
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				int num = y.Influence.CompareTo(x.Influence);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001F2 RID: 498
		public class ItemMembersComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002515 RID: 9493 RVA: 0x000812AC File Offset: 0x0007F4AC
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				int num = y.Members.Count.CompareTo(x.Members.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001F3 RID: 499
		public class ItemFiefsComparer : KingdomClanSortControllerVM.ItemComparerBase
		{
			// Token: 0x06002517 RID: 9495 RVA: 0x000812FC File Offset: 0x0007F4FC
			public override int Compare(KingdomClanItemVM x, KingdomClanItemVM y)
			{
				int num = y.Fiefs.Count.CompareTo(x.Fiefs.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
