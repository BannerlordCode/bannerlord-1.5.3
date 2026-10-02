using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x0200007A RID: 122
	public class KingdomWarSortControllerVM : ViewModel
	{
		// Token: 0x0600099A RID: 2458 RVA: 0x0002AA3E File Offset: 0x00028C3E
		public KingdomWarSortControllerVM(ref MBBindingList<KingdomWarItemVM> listToControl)
		{
			this._listToControl = listToControl;
			this._scoreComparer = new KingdomWarSortControllerVM.ItemScoreComparer();
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002AA5C File Offset: 0x00028C5C
		private void ExecuteSortByScore()
		{
			int scoreState = this.ScoreState;
			this.SetAllStates(CampaignUIHelper.SortState.Default);
			this.ScoreState = (scoreState + 1) % 3;
			if (this.ScoreState == 0)
			{
				int scoreState2 = this.ScoreState;
				this.ScoreState = scoreState2 + 1;
			}
			this._scoreComparer.SetSortMode(this.ScoreState == 1);
			this._listToControl.Sort(this._scoreComparer);
			this.IsScoreSelected = true;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002AAC6 File Offset: 0x00028CC6
		private void SetAllStates(CampaignUIHelper.SortState state)
		{
			this.ScoreState = (int)state;
			this.IsScoreSelected = false;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0002AAD6 File Offset: 0x00028CD6
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x0002AADE File Offset: 0x00028CDE
		[DataSourceProperty]
		public int ScoreState
		{
			get
			{
				return this._scoreState;
			}
			set
			{
				if (value != this._scoreState)
				{
					this._scoreState = value;
					base.OnPropertyChangedWithValue(value, "ScoreState");
				}
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0002AAFC File Offset: 0x00028CFC
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x0002AB04 File Offset: 0x00028D04
		[DataSourceProperty]
		public bool IsScoreSelected
		{
			get
			{
				return this._isScoreSelected;
			}
			set
			{
				if (value != this._isScoreSelected)
				{
					this._isScoreSelected = value;
					base.OnPropertyChangedWithValue(value, "IsScoreSelected");
				}
			}
		}

		// Token: 0x04000435 RID: 1077
		private readonly MBBindingList<KingdomWarItemVM> _listToControl;

		// Token: 0x04000436 RID: 1078
		private readonly KingdomWarSortControllerVM.ItemScoreComparer _scoreComparer;

		// Token: 0x04000437 RID: 1079
		private int _scoreState;

		// Token: 0x04000438 RID: 1080
		private bool _isScoreSelected;

		// Token: 0x020001DD RID: 477
		public abstract class ItemComparerBase : IComparer<KingdomWarItemVM>
		{
			// Token: 0x060024D0 RID: 9424 RVA: 0x00080E90 File Offset: 0x0007F090
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x060024D1 RID: 9425
			public abstract int Compare(KingdomWarItemVM x, KingdomWarItemVM y);

			// Token: 0x04001165 RID: 4453
			protected bool _isAscending;
		}

		// Token: 0x020001DE RID: 478
		public class ItemScoreComparer : KingdomWarSortControllerVM.ItemComparerBase
		{
			// Token: 0x060024D3 RID: 9427 RVA: 0x00080EA4 File Offset: 0x0007F0A4
			public override int Compare(KingdomWarItemVM x, KingdomWarItemVM y)
			{
				if (this._isAscending)
				{
					return x.Score.CompareTo(y.Score);
				}
				return x.Score.CompareTo(y.Score) * -1;
			}
		}
	}
}
