using System;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E4 RID: 228
	public class EncyclopediaListSortControllerVM : ViewModel
	{
		// Token: 0x06001574 RID: 5492 RVA: 0x000550CC File Offset: 0x000532CC
		public EncyclopediaListSortControllerVM(EncyclopediaPage page, MBBindingList<EncyclopediaListItemVM> items)
		{
			this._page = page;
			this._items = items;
			this.UpdateSortItemsFromPage(page);
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0005511A File Offset: 0x0005331A
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent evnt)
		{
			this.IsHighlightEnabled = evnt.NewNotificationElementID == "EncyclopediaSortButton";
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x00055134 File Offset: 0x00053334
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NameLabel = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.SortByLabel = GameTexts.FindText("str_sort_by_label", null).ToString();
			this.SortedValueLabelText = this._sortedValueLabel.ToString();
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00055184 File Offset: 0x00053384
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x000551A7 File Offset: 0x000533A7
		public void SetSortSelection(int index)
		{
			this.SortSelection.SelectedIndex = index;
			this.OnSortSelectionChanged(this.SortSelection);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x000551C4 File Offset: 0x000533C4
		private void UpdateSortItemsFromPage(EncyclopediaPage page)
		{
			this.SortSelection = new EncyclopediaListSelectorVM(0, new Action<SelectorVM<EncyclopediaListSelectorItemVM>>(this.OnSortSelectionChanged), new Action(this.OnSortSelectionActivated));
			foreach (EncyclopediaSortController encyclopediaSortController in page.GetSortControllers())
			{
				EncyclopediaListItemComparer encyclopediaListItemComparer = new EncyclopediaListItemComparer(encyclopediaSortController);
				this.SortSelection.AddItem(new EncyclopediaListSelectorItemVM(encyclopediaListItemComparer));
			}
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00055244 File Offset: 0x00053444
		private void UpdateAlternativeSortState(EncyclopediaListItemComparerBase comparer)
		{
			CampaignUIHelper.SortState sortState = (comparer.IsAscending ? CampaignUIHelper.SortState.Ascending : CampaignUIHelper.SortState.Descending);
			this.AlternativeSortState = (int)sortState;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00055268 File Offset: 0x00053468
		private void OnSortSelectionChanged(SelectorVM<EncyclopediaListSelectorItemVM> s)
		{
			EncyclopediaListItemComparer comparer = s.SelectedItem.Comparer;
			comparer.SortController.Comparer.SetDefaultSortOrder();
			this._items.Sort(comparer);
			this._items.ApplyActionOnAllItems(delegate(EncyclopediaListItemVM x)
			{
				x.SetComparedValue(comparer.SortController.Comparer);
			});
			this._sortedValueLabel = comparer.SortController.Name;
			this.SortedValueLabelText = this._sortedValueLabel.ToString();
			this.IsAlternativeSortVisible = this.SortSelection.SelectedIndex != 0;
			this.UpdateAlternativeSortState(comparer.SortController.Comparer);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0005531C File Offset: 0x0005351C
		public void ExecuteSwitchSortOrder()
		{
			EncyclopediaListItemComparer comparer = this.SortSelection.SelectedItem.Comparer;
			comparer.SortController.Comparer.SwitchSortOrder();
			this._items.Sort(comparer);
			this.UpdateAlternativeSortState(comparer.SortController.Comparer);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00055368 File Offset: 0x00053568
		public void SetSortOrder(bool isAscending)
		{
			EncyclopediaListItemComparer comparer = this.SortSelection.SelectedItem.Comparer;
			if (comparer.SortController.Comparer.IsAscending != isAscending)
			{
				comparer.SortController.Comparer.SetSortOrder(isAscending);
				this._items.Sort(comparer);
				this.UpdateAlternativeSortState(comparer.SortController.Comparer);
			}
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x000553C7 File Offset: 0x000535C7
		public bool GetSortOrder()
		{
			return this.SortSelection.SelectedItem.Comparer.SortController.Comparer.IsAscending;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x000553E8 File Offset: 0x000535E8
		private void OnSortSelectionActivated()
		{
			Game.Current.EventManager.TriggerEvent<OnEncyclopediaListSortedEvent>(new OnEncyclopediaListSortedEvent());
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x000553FE File Offset: 0x000535FE
		// (set) Token: 0x06001581 RID: 5505 RVA: 0x00055406 File Offset: 0x00053606
		[DataSourceProperty]
		public EncyclopediaListSelectorVM SortSelection
		{
			get
			{
				return this._sortSelection;
			}
			set
			{
				if (value != this._sortSelection)
				{
					this._sortSelection = value;
					base.OnPropertyChangedWithValue<EncyclopediaListSelectorVM>(value, "SortSelection");
				}
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00055424 File Offset: 0x00053624
		// (set) Token: 0x06001583 RID: 5507 RVA: 0x0005542C File Offset: 0x0005362C
		[DataSourceProperty]
		public string NameLabel
		{
			get
			{
				return this._nameLabel;
			}
			set
			{
				if (value != this._nameLabel)
				{
					this._nameLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "NameLabel");
				}
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x0005544F File Offset: 0x0005364F
		// (set) Token: 0x06001585 RID: 5509 RVA: 0x00055457 File Offset: 0x00053657
		[DataSourceProperty]
		public string SortedValueLabelText
		{
			get
			{
				return this._sortedValueLabelText;
			}
			set
			{
				if (value != this._sortedValueLabelText)
				{
					this._sortedValueLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "SortedValueLabelText");
				}
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x0005547A File Offset: 0x0005367A
		// (set) Token: 0x06001587 RID: 5511 RVA: 0x00055482 File Offset: 0x00053682
		[DataSourceProperty]
		public string SortByLabel
		{
			get
			{
				return this._sortByLabel;
			}
			set
			{
				if (value != this._sortByLabel)
				{
					this._sortByLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "SortByLabel");
				}
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x000554A5 File Offset: 0x000536A5
		// (set) Token: 0x06001589 RID: 5513 RVA: 0x000554AD File Offset: 0x000536AD
		[DataSourceProperty]
		public int AlternativeSortState
		{
			get
			{
				return this._alternativeSortState;
			}
			set
			{
				if (value != this._alternativeSortState)
				{
					this._alternativeSortState = value;
					base.OnPropertyChangedWithValue(value, "AlternativeSortState");
				}
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x000554CB File Offset: 0x000536CB
		// (set) Token: 0x0600158B RID: 5515 RVA: 0x000554D3 File Offset: 0x000536D3
		[DataSourceProperty]
		public bool IsAlternativeSortVisible
		{
			get
			{
				return this._isAlternativeSortVisible;
			}
			set
			{
				if (value != this._isAlternativeSortVisible)
				{
					this._isAlternativeSortVisible = value;
					base.OnPropertyChangedWithValue(value, "IsAlternativeSortVisible");
				}
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x000554F1 File Offset: 0x000536F1
		// (set) Token: 0x0600158D RID: 5517 RVA: 0x000554F9 File Offset: 0x000536F9
		[DataSourceProperty]
		public bool IsHighlightEnabled
		{
			get
			{
				return this._isHighlightEnabled;
			}
			set
			{
				if (value != this._isHighlightEnabled)
				{
					this._isHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHighlightEnabled");
				}
			}
		}

		// Token: 0x040009BC RID: 2492
		private TextObject _sortedValueLabel = TextObject.GetEmpty();

		// Token: 0x040009BD RID: 2493
		private MBBindingList<EncyclopediaListItemVM> _items;

		// Token: 0x040009BE RID: 2494
		private EncyclopediaPage _page;

		// Token: 0x040009BF RID: 2495
		private EncyclopediaListSelectorVM _sortSelection;

		// Token: 0x040009C0 RID: 2496
		private string _nameLabel;

		// Token: 0x040009C1 RID: 2497
		private string _sortedValueLabelText;

		// Token: 0x040009C2 RID: 2498
		private string _sortByLabel;

		// Token: 0x040009C3 RID: 2499
		private int _alternativeSortState;

		// Token: 0x040009C4 RID: 2500
		private bool _isAlternativeSortVisible;

		// Token: 0x040009C5 RID: 2501
		private bool _isHighlightEnabled;
	}
}
