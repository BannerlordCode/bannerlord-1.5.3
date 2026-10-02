using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Library;

namespace SandBox.GauntletUI.Encyclopedia
{
	// Token: 0x02000046 RID: 70
	public class EncyclopediaListViewDataController
	{
		// Token: 0x0600033D RID: 829 RVA: 0x000137C4 File Offset: 0x000119C4
		public EncyclopediaListViewDataController()
		{
			this._listData = new Dictionary<EncyclopediaPage, EncyclopediaListViewDataController.EncyclopediaListViewData>();
			foreach (EncyclopediaPage encyclopediaPage in Campaign.Current.EncyclopediaManager.GetEncyclopediaPages())
			{
				if (!this._listData.ContainsKey(encyclopediaPage))
				{
					this._listData.Add(encyclopediaPage, new EncyclopediaListViewDataController.EncyclopediaListViewData(new MBBindingList<EncyclopediaFilterGroupVM>(), 0, "", false));
				}
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00013850 File Offset: 0x00011A50
		public void SaveListData(EncyclopediaListVM list, string id)
		{
			if (list != null && this._listData.ContainsKey(list.Page))
			{
				EncyclopediaListSortControllerVM sortController = list.SortController;
				int? num;
				if (sortController == null)
				{
					num = null;
				}
				else
				{
					EncyclopediaListSelectorVM sortSelection = sortController.SortSelection;
					num = ((sortSelection != null) ? new int?(sortSelection.SelectedIndex) : null);
				}
				int num2 = num ?? 0;
				Dictionary<EncyclopediaPage, EncyclopediaListViewDataController.EncyclopediaListViewData> listData = this._listData;
				EncyclopediaPage page = list.Page;
				MBBindingList<EncyclopediaFilterGroupVM> filterGroups = list.FilterGroups;
				int num3 = num2;
				EncyclopediaListSortControllerVM sortController2 = list.SortController;
				listData[page] = new EncyclopediaListViewDataController.EncyclopediaListViewData(filterGroups, num3, id, sortController2 != null && sortController2.GetSortOrder());
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000138F0 File Offset: 0x00011AF0
		public void LoadListData(EncyclopediaListVM list)
		{
			if (list != null && this._listData.ContainsKey(list.Page))
			{
				EncyclopediaListViewDataController.EncyclopediaListViewData encyclopediaListViewData = this._listData[list.Page];
				EncyclopediaListSortControllerVM sortController = list.SortController;
				if (sortController != null)
				{
					sortController.SetSortSelection(encyclopediaListViewData.SelectedSortIndex);
				}
				EncyclopediaListSortControllerVM sortController2 = list.SortController;
				if (sortController2 != null)
				{
					sortController2.SetSortOrder(encyclopediaListViewData.IsAscending);
				}
				list.CopyFiltersFrom(encyclopediaListViewData.Filters);
				list.LastSelectedItemId = encyclopediaListViewData.LastSelectedItemId;
			}
		}

		// Token: 0x04000141 RID: 321
		private Dictionary<EncyclopediaPage, EncyclopediaListViewDataController.EncyclopediaListViewData> _listData;

		// Token: 0x02000088 RID: 136
		private readonly struct EncyclopediaListViewData
		{
			// Token: 0x06000474 RID: 1140 RVA: 0x00018DD8 File Offset: 0x00016FD8
			public EncyclopediaListViewData(MBBindingList<EncyclopediaFilterGroupVM> filters, int selectedSortIndex, string lastSelectedItemId, bool isAscending)
			{
				Dictionary<EncyclopediaFilterItem, bool> dictionary = new Dictionary<EncyclopediaFilterItem, bool>();
				foreach (EncyclopediaFilterGroupVM encyclopediaFilterGroupVM in filters)
				{
					foreach (EncyclopediaListFilterVM encyclopediaListFilterVM in encyclopediaFilterGroupVM.Filters)
					{
						if (!dictionary.ContainsKey(encyclopediaListFilterVM.Filter))
						{
							dictionary.Add(encyclopediaListFilterVM.Filter, encyclopediaListFilterVM.IsSelected);
						}
					}
				}
				this.Filters = dictionary;
				this.SelectedSortIndex = selectedSortIndex;
				this.LastSelectedItemId = lastSelectedItemId;
				this.IsAscending = isAscending;
			}

			// Token: 0x0400020A RID: 522
			public readonly Dictionary<EncyclopediaFilterItem, bool> Filters;

			// Token: 0x0400020B RID: 523
			public readonly int SelectedSortIndex;

			// Token: 0x0400020C RID: 524
			public readonly string LastSelectedItemId;

			// Token: 0x0400020D RID: 525
			public readonly bool IsAscending;
		}
	}
}
