using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x02000029 RID: 41
	public class PartySortControllerVM : ViewModel
	{
		// Token: 0x06000344 RID: 836 RVA: 0x00016954 File Offset: 0x00014B54
		public PartySortControllerVM(PartyScreenLogic.PartyRosterSide rosterSide, Action<PartyScreenLogic.PartyRosterSide, PartyScreenLogic.TroopSortType, bool> onSort)
		{
			this._rosterSide = rosterSide;
			this.SortOptions = new SelectorVM<TroopSortSelectorItemVM>(-1, new Action<SelectorVM<TroopSortSelectorItemVM>>(this.OnSortSelected));
			this.SortOptions.AddItem(new TroopSortSelectorItemVM(new TextObject("{=zMMqgxb1}Type", null), PartyScreenLogic.TroopSortType.Type));
			this.SortOptions.AddItem(new TroopSortSelectorItemVM(new TextObject("{=PDdh1sBj}Name", null), PartyScreenLogic.TroopSortType.Name));
			this.SortOptions.AddItem(new TroopSortSelectorItemVM(new TextObject("{=zFDoDbNj}Count", null), PartyScreenLogic.TroopSortType.Count));
			this.SortOptions.AddItem(new TroopSortSelectorItemVM(new TextObject("{=cc1d7mkq}Tier", null), PartyScreenLogic.TroopSortType.Tier));
			this.SortOptions.AddItem(new TroopSortSelectorItemVM(new TextObject("{=jvOYgHOe}Custom", null), PartyScreenLogic.TroopSortType.Custom));
			this.SortOptions.SelectedIndex = this.SortOptions.ItemList.Count - 1;
			this.IsAscending = true;
			this._onSort = onSort;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00016A40 File Offset: 0x00014C40
		private void OnSortSelected(SelectorVM<TroopSortSelectorItemVM> selector)
		{
			this._sortType = selector.SelectedItem.SortType;
			this.IsCustomSort = this._sortType == PartyScreenLogic.TroopSortType.Custom;
			Action<PartyScreenLogic.PartyRosterSide, PartyScreenLogic.TroopSortType, bool> onSort = this._onSort;
			if (onSort == null)
			{
				return;
			}
			onSort(this._rosterSide, this._sortType, this.IsAscending);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00016A90 File Offset: 0x00014C90
		public void SelectSortType(PartyScreenLogic.TroopSortType sortType)
		{
			for (int i = 0; i < this.SortOptions.ItemList.Count; i++)
			{
				if (this.SortOptions.ItemList[i].SortType == sortType)
				{
					this.SortOptions.SelectedIndex = i;
				}
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00016ADD File Offset: 0x00014CDD
		public void SortWith(PartyScreenLogic.TroopSortType sortType, bool isAscending)
		{
			Action<PartyScreenLogic.PartyRosterSide, PartyScreenLogic.TroopSortType, bool> onSort = this._onSort;
			if (onSort == null)
			{
				return;
			}
			onSort(this._rosterSide, sortType, isAscending);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00016AF7 File Offset: 0x00014CF7
		public void ExecuteToggleOrder()
		{
			this.IsAscending = !this.IsAscending;
			Action<PartyScreenLogic.PartyRosterSide, PartyScreenLogic.TroopSortType, bool> onSort = this._onSort;
			if (onSort == null)
			{
				return;
			}
			onSort(this._rosterSide, this._sortType, this.IsAscending);
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00016B2A File Offset: 0x00014D2A
		// (set) Token: 0x0600034A RID: 842 RVA: 0x00016B32 File Offset: 0x00014D32
		[DataSourceProperty]
		public bool IsAscending
		{
			get
			{
				return this._isAscending;
			}
			set
			{
				if (value != this._isAscending)
				{
					this._isAscending = value;
					base.OnPropertyChangedWithValue(value, "IsAscending");
				}
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00016B50 File Offset: 0x00014D50
		// (set) Token: 0x0600034C RID: 844 RVA: 0x00016B58 File Offset: 0x00014D58
		[DataSourceProperty]
		public bool IsCustomSort
		{
			get
			{
				return this._isCustomSort;
			}
			set
			{
				if (value != this._isCustomSort)
				{
					this._isCustomSort = value;
					base.OnPropertyChangedWithValue(value, "IsCustomSort");
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00016B76 File Offset: 0x00014D76
		// (set) Token: 0x0600034E RID: 846 RVA: 0x00016B7E File Offset: 0x00014D7E
		[DataSourceProperty]
		public SelectorVM<TroopSortSelectorItemVM> SortOptions
		{
			get
			{
				return this._sortOptions;
			}
			set
			{
				if (value != this._sortOptions)
				{
					this._sortOptions = value;
					base.OnPropertyChangedWithValue<SelectorVM<TroopSortSelectorItemVM>>(value, "SortOptions");
				}
			}
		}

		// Token: 0x04000174 RID: 372
		private readonly PartyScreenLogic.PartyRosterSide _rosterSide;

		// Token: 0x04000175 RID: 373
		private readonly Action<PartyScreenLogic.PartyRosterSide, PartyScreenLogic.TroopSortType, bool> _onSort;

		// Token: 0x04000176 RID: 374
		private PartyScreenLogic.TroopSortType _sortType;

		// Token: 0x04000177 RID: 375
		private bool _isAscending;

		// Token: 0x04000178 RID: 376
		private bool _isCustomSort;

		// Token: 0x04000179 RID: 377
		private SelectorVM<TroopSortSelectorItemVM> _sortOptions;
	}
}
