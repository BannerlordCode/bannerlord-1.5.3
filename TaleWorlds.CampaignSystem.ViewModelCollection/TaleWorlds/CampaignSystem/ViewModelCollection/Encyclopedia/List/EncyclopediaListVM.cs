using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List
{
	// Token: 0x020000E8 RID: 232
	public class EncyclopediaListVM : EncyclopediaPageVM
	{
		// Token: 0x06001595 RID: 5525 RVA: 0x000555D0 File Offset: 0x000537D0
		public EncyclopediaListVM(EncyclopediaPageArgs args)
			: base(args)
		{
			this.Page = base.Obj as EncyclopediaPage;
			this.Items = new MBBindingList<EncyclopediaListItemVM>();
			this.FilterGroups = new MBBindingList<EncyclopediaFilterGroupVM>();
			this.SortController = new EncyclopediaListSortControllerVM(this.Page, this.Items);
			this.IsInitializationOver = true;
			foreach (EncyclopediaFilterGroup encyclopediaFilterGroup in this.Page.GetFilterItems())
			{
				this.FilterGroups.Add(new EncyclopediaFilterGroupVM(encyclopediaFilterGroup, new Action<EncyclopediaListFilterVM>(this.UpdateFilters)));
			}
			this.IsInitializationOver = false;
			this.Items.Clear();
			foreach (EncyclopediaListItem encyclopediaListItem in this.Page.GetListItems())
			{
				EncyclopediaListItemVM encyclopediaListItemVM = new EncyclopediaListItemVM(encyclopediaListItem);
				encyclopediaListItemVM.IsFiltered = this.Page.IsFiltered(encyclopediaListItemVM.Object);
				this.Items.Add(encyclopediaListItemVM);
			}
			this.RefreshValues();
			this.IsInitializationOver = true;
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00055720 File Offset: 0x00053920
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent evnt)
		{
			this.IsFilterHighlightEnabled = evnt.NewNotificationElementID == "EncyclopediaFiltersContainer";
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00055738 File Offset: 0x00053938
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SortController.RefreshValues();
			this.EmptyListText = GameTexts.FindText("str_encyclopedia_empty_list_error", null).ToString();
			this.Items.ApplyActionOnAllItems(delegate(EncyclopediaListItemVM x)
			{
				x.RefreshValues();
			});
			this.FilterGroups.ApplyActionOnAllItems(delegate(EncyclopediaFilterGroupVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x000557C0 File Offset: 0x000539C0
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			EncyclopediaListSortControllerVM sortController = this.SortController;
			if (sortController != null)
			{
				sortController.OnFinalize();
			}
			this.SortController = null;
			this.FilterGroups.ApplyActionOnAllItems(delegate(EncyclopediaFilterGroupVM x)
			{
				x.OnFinalize();
			});
			this.FilterGroups.Clear();
			this.Items.ApplyActionOnAllItems(delegate(EncyclopediaListItemVM x)
			{
				x.OnFinalize();
			});
			this.Items.Clear();
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00055870 File Offset: 0x00053A70
		public override string GetName()
		{
			return this.Page.GetName().ToString();
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00055884 File Offset: 0x00053A84
		public override string GetNavigationBarURL()
		{
			string text = HyperlinkTexts.GetGenericHyperlinkText("Home", GameTexts.FindText("str_encyclopedia_home", null).ToString()) + " \\ ";
			if (this.Page.HasIdentifierType(typeof(Kingdom)))
			{
				text += GameTexts.FindText("str_encyclopedia_kingdoms", null).ToString();
			}
			else if (this.Page.HasIdentifierType(typeof(Clan)))
			{
				text += GameTexts.FindText("str_encyclopedia_clans", null).ToString();
			}
			else if (this.Page.HasIdentifierType(typeof(Hero)))
			{
				text += GameTexts.FindText("str_encyclopedia_heroes", null).ToString();
			}
			else if (this.Page.HasIdentifierType(typeof(Settlement)))
			{
				text += GameTexts.FindText("str_encyclopedia_settlements", null).ToString();
			}
			else if (this.Page.HasIdentifierType(typeof(CharacterObject)))
			{
				text += GameTexts.FindText("str_encyclopedia_troops", null).ToString();
			}
			else if (this.Page.HasIdentifierType(typeof(Concept)))
			{
				text += GameTexts.FindText("str_encyclopedia_concepts", null).ToString();
			}
			else if (this.Page.HasIdentifierType(typeof(ShipHull)))
			{
				text += GameTexts.FindText("str_encyclopedia_ships", null).ToString();
			}
			return text;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00055A14 File Offset: 0x00053C14
		private void ExecuteResetFilters()
		{
			foreach (EncyclopediaFilterGroupVM encyclopediaFilterGroupVM in this.FilterGroups)
			{
				foreach (EncyclopediaListFilterVM encyclopediaListFilterVM in encyclopediaFilterGroupVM.Filters)
				{
					encyclopediaListFilterVM.IsSelected = false;
				}
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00055A94 File Offset: 0x00053C94
		public void CopyFiltersFrom(Dictionary<EncyclopediaFilterItem, bool> filters)
		{
			this.FilterGroups.ApplyActionOnAllItems(delegate(EncyclopediaFilterGroupVM x)
			{
				x.CopyFiltersFrom(filters);
			});
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00055AC8 File Offset: 0x00053CC8
		public override void Refresh()
		{
			base.Refresh();
			foreach (EncyclopediaListItemVM encyclopediaListItemVM in this.Items)
			{
				Hero hero;
				Clan clan;
				Concept concept;
				Kingdom kingdom;
				Settlement settlement;
				CharacterObject characterObject;
				ShipHull shipHull;
				if ((hero = encyclopediaListItemVM.Object as Hero) != null)
				{
					encyclopediaListItemVM.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(hero);
				}
				else if ((clan = encyclopediaListItemVM.Object as Clan) != null)
				{
					encyclopediaListItemVM.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(clan);
				}
				else if ((concept = encyclopediaListItemVM.Object as Concept) != null)
				{
					encyclopediaListItemVM.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(concept);
				}
				else if ((kingdom = encyclopediaListItemVM.Object as Kingdom) != null)
				{
					encyclopediaListItemVM.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(kingdom);
				}
				else if ((settlement = encyclopediaListItemVM.Object as Settlement) != null)
				{
					encyclopediaListItemVM.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(settlement);
				}
				else if ((characterObject = encyclopediaListItemVM.Object as CharacterObject) != null)
				{
					encyclopediaListItemVM.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(characterObject);
				}
				else if ((shipHull = encyclopediaListItemVM.Object as ShipHull) != null)
				{
					encyclopediaListItemVM.IsBookmarked = Campaign.Current.EncyclopediaManager.ViewDataTracker.IsEncyclopediaBookmarked(shipHull);
				}
			}
			this._isInitializationOver = false;
			this.IsInitializationOver = true;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x00055C7C File Offset: 0x00053E7C
		private void UpdateFilters(EncyclopediaListFilterVM filterVM)
		{
			this.IsInitializationOver = false;
			foreach (EncyclopediaListItemVM encyclopediaListItemVM in this.Items)
			{
				encyclopediaListItemVM.IsFiltered = this.Page.IsFiltered(encyclopediaListItemVM.Object);
			}
			this.IsInitializationOver = true;
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x0600159F RID: 5535 RVA: 0x00055CE8 File Offset: 0x00053EE8
		// (set) Token: 0x060015A0 RID: 5536 RVA: 0x00055CF0 File Offset: 0x00053EF0
		[DataSourceProperty]
		public string EmptyListText
		{
			get
			{
				return this._emptyListText;
			}
			set
			{
				if (value != this._emptyListText)
				{
					this._emptyListText = value;
					base.OnPropertyChangedWithValue<string>(value, "EmptyListText");
				}
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060015A1 RID: 5537 RVA: 0x00055D13 File Offset: 0x00053F13
		// (set) Token: 0x060015A2 RID: 5538 RVA: 0x00055D1B File Offset: 0x00053F1B
		[DataSourceProperty]
		public string LastSelectedItemId
		{
			get
			{
				return this._lastSelectedItemId;
			}
			set
			{
				if (value != this._lastSelectedItemId)
				{
					this._lastSelectedItemId = value;
					base.OnPropertyChangedWithValue<string>(value, "LastSelectedItemId");
				}
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x00055D3E File Offset: 0x00053F3E
		// (set) Token: 0x060015A4 RID: 5540 RVA: 0x00055D46 File Offset: 0x00053F46
		[DataSourceProperty]
		public override MBBindingList<EncyclopediaListItemVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaListItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x00055D64 File Offset: 0x00053F64
		// (set) Token: 0x060015A6 RID: 5542 RVA: 0x00055D6C File Offset: 0x00053F6C
		[DataSourceProperty]
		public override EncyclopediaListSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<EncyclopediaListSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x00055D8A File Offset: 0x00053F8A
		// (set) Token: 0x060015A8 RID: 5544 RVA: 0x00055D92 File Offset: 0x00053F92
		[DataSourceProperty]
		public bool IsInitializationOver
		{
			get
			{
				return this._isInitializationOver;
			}
			set
			{
				if (value != this._isInitializationOver)
				{
					this._isInitializationOver = value;
					base.OnPropertyChangedWithValue(value, "IsInitializationOver");
				}
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x00055DB0 File Offset: 0x00053FB0
		// (set) Token: 0x060015AA RID: 5546 RVA: 0x00055DB8 File Offset: 0x00053FB8
		[DataSourceProperty]
		public bool IsFilterHighlightEnabled
		{
			get
			{
				return this._isFilterHighlightEnabled;
			}
			set
			{
				if (value != this._isFilterHighlightEnabled)
				{
					this._isFilterHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsFilterHighlightEnabled");
				}
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x00055DD6 File Offset: 0x00053FD6
		// (set) Token: 0x060015AC RID: 5548 RVA: 0x00055DDE File Offset: 0x00053FDE
		[DataSourceProperty]
		public override MBBindingList<EncyclopediaFilterGroupVM> FilterGroups
		{
			get
			{
				return this._filterGroups;
			}
			set
			{
				if (value != this._filterGroups)
				{
					this._filterGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaFilterGroupVM>>(value, "FilterGroups");
				}
			}
		}

		// Token: 0x040009C9 RID: 2505
		public readonly EncyclopediaPage Page;

		// Token: 0x040009CA RID: 2506
		private MBBindingList<EncyclopediaFilterGroupVM> _filterGroups;

		// Token: 0x040009CB RID: 2507
		private MBBindingList<EncyclopediaListItemVM> _items;

		// Token: 0x040009CC RID: 2508
		private EncyclopediaListSortControllerVM _sortController;

		// Token: 0x040009CD RID: 2509
		private bool _isInitializationOver;

		// Token: 0x040009CE RID: 2510
		private bool _isFilterHighlightEnabled;

		// Token: 0x040009CF RID: 2511
		private string _emptyListText;

		// Token: 0x040009D0 RID: 2512
		private string _lastSelectedItemId;
	}
}
