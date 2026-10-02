using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies
{
	// Token: 0x02000091 RID: 145
	public class KingdomArmySortControllerVM : ViewModel
	{
		// Token: 0x06000BF9 RID: 3065 RVA: 0x00031F3C File Offset: 0x0003013C
		public KingdomArmySortControllerVM(ref MBBindingList<KingdomArmyItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._ownerComparer = new KingdomArmySortControllerVM.ItemOwnerComparer();
			this._strengthComparer = new KingdomArmySortControllerVM.ItemStrengthComparer();
			this._nameComparer = new KingdomArmySortControllerVM.ItemNameComparer();
			this._partiesComparer = new KingdomArmySortControllerVM.ItemPartiesComparer();
			this._distanceComparer = new KingdomArmySortControllerVM.ItemDistanceComparer();
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00031F90 File Offset: 0x00030190
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

		// Token: 0x06000BFB RID: 3067 RVA: 0x00031FF8 File Offset: 0x000301F8
		private void ExecuteSortByOwner()
		{
			int ownerState = this.OwnerState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.OwnerState = (ownerState + 1) % 3;
			if (this.OwnerState == 0)
			{
				this.OwnerState++;
			}
			this._ownerComparer.SetSortMode(this.OwnerState == 1);
			this._listToControl.Sort(this._ownerComparer);
			this.IsOwnerSelected = true;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00032060 File Offset: 0x00030260
		private void ExecuteSortByStrength()
		{
			int strengthState = this.StrengthState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.StrengthState = (strengthState + 1) % 3;
			if (this.StrengthState == 0)
			{
				this.StrengthState++;
			}
			this._strengthComparer.SetSortMode(this.StrengthState == 1);
			this._listToControl.Sort(this._strengthComparer);
			this.IsStrengthSelected = true;
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000320C8 File Offset: 0x000302C8
		private void ExecuteSortByParties()
		{
			int partiesState = this.PartiesState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.PartiesState = (partiesState + 1) % 3;
			if (this.PartiesState == 0)
			{
				this.PartiesState++;
			}
			this._partiesComparer.SetSortMode(this.PartiesState == 1);
			this._listToControl.Sort(this._partiesComparer);
			this.IsPartiesSelected = true;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00032130 File Offset: 0x00030330
		private void ExecuteSortByDistance()
		{
			int distanceState = this.DistanceState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.DistanceState = (distanceState + 1) % 3;
			if (this.DistanceState == 0)
			{
				this.DistanceState++;
			}
			this._distanceComparer.SetSortMode(this.DistanceState == 1);
			this._listToControl.Sort(this._distanceComparer);
			this.IsDistanceSelected = true;
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00032198 File Offset: 0x00030398
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.NameState = (int)state;
			this.OwnerState = (int)state;
			this.StrengthState = (int)state;
			this.PartiesState = (int)state;
			this.DistanceState = (int)state;
			this.IsNameSelected = false;
			this.IsOwnerSelected = false;
			this.IsStrengthSelected = false;
			this.IsPartiesSelected = false;
			this.IsDistanceSelected = false;
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x000321EB File Offset: 0x000303EB
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x000321F3 File Offset: 0x000303F3
		[DataSourceProperty]
		public int OwnerState
		{
			get
			{
				return this._ownerState;
			}
			set
			{
				if (value != this._ownerState)
				{
					this._ownerState = value;
					base.OnPropertyChangedWithValue(value, "OwnerState");
				}
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x00032211 File Offset: 0x00030411
		// (set) Token: 0x06000C03 RID: 3075 RVA: 0x00032219 File Offset: 0x00030419
		[DataSourceProperty]
		public int PartiesState
		{
			get
			{
				return this._partiesState;
			}
			set
			{
				if (value != this._partiesState)
				{
					this._partiesState = value;
					base.OnPropertyChangedWithValue(value, "PartiesState");
				}
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00032237 File Offset: 0x00030437
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x0003223F File Offset: 0x0003043F
		[DataSourceProperty]
		public int StrengthState
		{
			get
			{
				return this._strengthState;
			}
			set
			{
				if (value != this._strengthState)
				{
					this._strengthState = value;
					base.OnPropertyChangedWithValue(value, "StrengthState");
				}
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0003225D File Offset: 0x0003045D
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x00032265 File Offset: 0x00030465
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

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00032283 File Offset: 0x00030483
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x0003228B File Offset: 0x0003048B
		[DataSourceProperty]
		public int DistanceState
		{
			get
			{
				return this._distanceState;
			}
			set
			{
				if (value != this._distanceState)
				{
					this._distanceState = value;
					base.OnPropertyChangedWithValue(value, "DistanceState");
				}
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x000322A9 File Offset: 0x000304A9
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x000322B1 File Offset: 0x000304B1
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

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x000322CF File Offset: 0x000304CF
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x000322D7 File Offset: 0x000304D7
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

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x000322F5 File Offset: 0x000304F5
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x000322FD File Offset: 0x000304FD
		[DataSourceProperty]
		public bool IsStrengthSelected
		{
			get
			{
				return this._isStrengthSelected;
			}
			set
			{
				if (value != this._isStrengthSelected)
				{
					this._isStrengthSelected = value;
					base.OnPropertyChangedWithValue(value, "IsStrengthSelected");
				}
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x0003231B File Offset: 0x0003051B
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x00032323 File Offset: 0x00030523
		[DataSourceProperty]
		public bool IsOwnerSelected
		{
			get
			{
				return this._isOwnerSelected;
			}
			set
			{
				if (value != this._isOwnerSelected)
				{
					this._isOwnerSelected = value;
					base.OnPropertyChangedWithValue(value, "IsOwnerSelected");
				}
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00032341 File Offset: 0x00030541
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x00032349 File Offset: 0x00030549
		[DataSourceProperty]
		public bool IsDistanceSelected
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
					base.OnPropertyChangedWithValue(value, "IsDistanceSelected");
				}
			}
		}

		// Token: 0x0400054B RID: 1355
		private readonly MBBindingList<KingdomArmyItemVM> _listToControl;

		// Token: 0x0400054C RID: 1356
		private readonly KingdomArmySortControllerVM.ItemNameComparer _nameComparer;

		// Token: 0x0400054D RID: 1357
		private readonly KingdomArmySortControllerVM.ItemOwnerComparer _ownerComparer;

		// Token: 0x0400054E RID: 1358
		private readonly KingdomArmySortControllerVM.ItemStrengthComparer _strengthComparer;

		// Token: 0x0400054F RID: 1359
		private readonly KingdomArmySortControllerVM.ItemPartiesComparer _partiesComparer;

		// Token: 0x04000550 RID: 1360
		private readonly KingdomArmySortControllerVM.ItemDistanceComparer _distanceComparer;

		// Token: 0x04000551 RID: 1361
		private int _nameState;

		// Token: 0x04000552 RID: 1362
		private int _ownerState;

		// Token: 0x04000553 RID: 1363
		private int _strengthState;

		// Token: 0x04000554 RID: 1364
		private int _partiesState;

		// Token: 0x04000555 RID: 1365
		private int _distanceState;

		// Token: 0x04000556 RID: 1366
		private bool _isNameSelected;

		// Token: 0x04000557 RID: 1367
		private bool _isOwnerSelected;

		// Token: 0x04000558 RID: 1368
		private bool _isStrengthSelected;

		// Token: 0x04000559 RID: 1369
		private bool _isPartiesSelected;

		// Token: 0x0400055A RID: 1370
		private bool _isDistanceSelected;

		// Token: 0x020001F5 RID: 501
		public abstract class ItemComparerBase : IComparer<KingdomArmyItemVM>
		{
			// Token: 0x0600251E RID: 9502 RVA: 0x00081386 File Offset: 0x0007F586
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x0600251F RID: 9503
			public abstract int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y);

			// Token: 0x06002520 RID: 9504 RVA: 0x0008138F File Offset: 0x0007F58F
			protected int ResolveEquality(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				return x.ArmyName.CompareTo(y.ArmyName);
			}

			// Token: 0x040011A6 RID: 4518
			protected bool _isAscending;
		}

		// Token: 0x020001F6 RID: 502
		public class ItemNameComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x06002522 RID: 9506 RVA: 0x000813AA File Offset: 0x0007F5AA
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				if (this._isAscending)
				{
					return y.ArmyName.CompareTo(x.ArmyName) * -1;
				}
				return y.ArmyName.CompareTo(x.ArmyName);
			}
		}

		// Token: 0x020001F7 RID: 503
		public class ItemOwnerComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x06002524 RID: 9508 RVA: 0x000813E4 File Offset: 0x0007F5E4
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				int num = y.Leader.NameText.ToString().CompareTo(x.Leader.NameText.ToString());
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001F8 RID: 504
		public class ItemStrengthComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x06002526 RID: 9510 RVA: 0x0008143C File Offset: 0x0007F63C
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				int num = y.Strength.CompareTo(x.Strength);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001F9 RID: 505
		public class ItemPartiesComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x06002528 RID: 9512 RVA: 0x00081480 File Offset: 0x0007F680
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				int num = y.Parties.Count.CompareTo(x.Parties.Count);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		// Token: 0x020001FA RID: 506
		public class ItemDistanceComparer : KingdomArmySortControllerVM.ItemComparerBase
		{
			// Token: 0x0600252A RID: 9514 RVA: 0x000814D0 File Offset: 0x0007F6D0
			public override int Compare(KingdomArmyItemVM x, KingdomArmyItemVM y)
			{
				int num = y.DistanceToMainParty.CompareTo(x.DistanceToMainParty);
				if (num != 0)
				{
					return num * (this._isAscending ? (-1) : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
