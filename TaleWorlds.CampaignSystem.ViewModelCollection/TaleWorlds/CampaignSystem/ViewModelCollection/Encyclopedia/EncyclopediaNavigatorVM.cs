using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia
{
	// Token: 0x020000D1 RID: 209
	public class EncyclopediaNavigatorVM : ViewModel
	{
		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0004EAEB File Offset: 0x0004CCEB
		public Tuple<string, object> LastActivePage
		{
			get
			{
				if (!this.History.IsEmpty<Tuple<string, object>>())
				{
					return this.History[this.HistoryIndex];
				}
				return null;
			}
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0004EB10 File Offset: 0x0004CD10
		public EncyclopediaNavigatorVM(Func<string, object, bool, EncyclopediaPageVM> goToLink, Action closeEncyclopedia)
		{
			this._closeEncyclopedia = closeEncyclopedia;
			this.History = new List<Tuple<string, object>>();
			this.HistoryIndex = 0;
			this.MinCharAmountToShowResults = 3;
			this.SearchResults = new MBBindingList<EncyclopediaSearchResultVM>();
			Campaign.Current.EncyclopediaManager.SetLinkCallback(new Action<string, object>(this.ExecuteLink));
			this._goToLink = goToLink;
			this._searchResultComparer = new EncyclopediaNavigatorVM.SearchResultComparer(string.Empty);
			this.AddHistory("Home", null);
			this.RefreshValues();
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0004EBB8 File Offset: 0x0004CDB8
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent evnt)
		{
			this.IsHighlightEnabled = evnt.NewNotificationElementID == "EncyclopediaSearchButton";
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0004EBD0 File Offset: 0x0004CDD0
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM previousPageInputKey = this.PreviousPageInputKey;
			if (previousPageInputKey != null)
			{
				previousPageInputKey.OnFinalize();
			}
			InputKeyItemVM nextPageInputKey = this.NextPageInputKey;
			if (nextPageInputKey == null)
			{
				return;
			}
			nextPageInputKey.OnFinalize();
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0004EBF9 File Offset: 0x0004CDF9
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.LeaderText = GameTexts.FindText("str_done", null).ToString();
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0004EC2D File Offset: 0x0004CE2D
		public void ExecuteHome()
		{
			Campaign.Current.EncyclopediaManager.GoToLink("Home", "-1");
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0004EC48 File Offset: 0x0004CE48
		public void ExecuteBarLink(string targetID)
		{
			if (targetID.Contains("Home"))
			{
				this.ExecuteHome();
				return;
			}
			if (targetID.Contains("ListPage"))
			{
				string text = targetID.Split(new char[] { '-' })[1];
				if (text == "Clans")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Faction");
					return;
				}
				if (text == "Kingdoms")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Kingdom");
					return;
				}
				if (text == "Heroes")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Hero");
					return;
				}
				if (text == "Settlements")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Settlement");
					return;
				}
				if (text == "Units")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "NPCCharacter");
					return;
				}
				if (text == "Concept")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "Concept");
					return;
				}
				if (text == "Ships")
				{
					Campaign.Current.EncyclopediaManager.GoToLink("ListPage", "ShipHull");
				}
			}
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0004ED9D File Offset: 0x0004CF9D
		public void ExecuteCloseEncyclopedia()
		{
			this._closeEncyclopedia();
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0004EDAC File Offset: 0x0004CFAC
		private void ExecuteLink(string pageId, object target)
		{
			if (pageId != "LastPage" && target != this.LastActivePage.Item2)
			{
				if (!(pageId != "Home"))
				{
					pageId != this.LastActivePage.Item1;
				}
				this.AddHistory(pageId, target);
			}
			this._goToLink(pageId, target, true);
			this.PageName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
			this.ResetSearch();
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0004EE31 File Offset: 0x0004D031
		public void ResetHistory()
		{
			this.HistoryIndex = 0;
			this.History.Clear();
			this.AddHistory("Home", null);
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0004EE54 File Offset: 0x0004D054
		public void ExecuteBack()
		{
			if (this.HistoryIndex == 0)
			{
				return;
			}
			int num = this.HistoryIndex - 1;
			Tuple<string, object> tuple = this.History[num];
			if (tuple.Item1 != "LastPage" && (tuple.Item1 != this.LastActivePage.Item1 || tuple.Item2 != this.LastActivePage.Item2))
			{
				if (!(tuple.Item1 != "Home"))
				{
					tuple.Item1 != this.LastActivePage.Item1;
				}
			}
			this.UpdateHistoryIndex(num);
			this._goToLink(tuple.Item1, tuple.Item2, true);
			this.PageName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0004EF24 File Offset: 0x0004D124
		public void ExecuteForward()
		{
			if (this.HistoryIndex == this.History.Count - 1)
			{
				return;
			}
			int num = this.HistoryIndex + 1;
			Tuple<string, object> tuple = this.History[num];
			if (tuple.Item1 != "LastPage" && (tuple.Item1 != this.LastActivePage.Item1 || tuple.Item2 != this.LastActivePage.Item2))
			{
				if (!(tuple.Item1 != "Home"))
				{
					tuple.Item1 != this.LastActivePage.Item1;
				}
			}
			this.UpdateHistoryIndex(num);
			this._goToLink(tuple.Item1, tuple.Item2, true);
			this.PageName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0004EFFF File Offset: 0x0004D1FF
		public Tuple<string, object> GetLastPage()
		{
			return this.History[this.HistoryIndex];
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0004F014 File Offset: 0x0004D214
		public void AddHistory(string pageId, object obj)
		{
			if (this.HistoryIndex < this.History.Count - 1)
			{
				Tuple<string, object> tuple = this.History[this.HistoryIndex];
				if (tuple.Item1 == pageId && tuple.Item2 == obj)
				{
					return;
				}
				this.History.RemoveRange(this.HistoryIndex + 1, this.History.Count - this.HistoryIndex - 1);
			}
			this.History.Add(new Tuple<string, object>(pageId, obj));
			this.UpdateHistoryIndex(this.History.Count - 1);
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0004F0AC File Offset: 0x0004D2AC
		private void UpdateHistoryIndex(int newIndex)
		{
			this.HistoryIndex = newIndex;
			this.IsBackEnabled = newIndex > 0;
			this.IsForwardEnabled = newIndex < this.History.Count - 1;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0004F0D5 File Offset: 0x0004D2D5
		public void UpdatePageName(string value)
		{
			this.PageName = GameTexts.FindText("str_encyclopedia_name", null).ToString();
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0004F0F0 File Offset: 0x0004D2F0
		private void RefreshSearch(bool isAppending, bool isPasted)
		{
			int firstAsianCharIndex = EncyclopediaNavigatorVM.GetFirstAsianCharIndex(this.SearchText);
			this.MinCharAmountToShowResults = ((firstAsianCharIndex > -1 && firstAsianCharIndex < 3) ? (firstAsianCharIndex + 1) : 3);
			if (this.SearchText.Length < this.MinCharAmountToShowResults)
			{
				this.SearchResults.Clear();
				return;
			}
			string text = StringHelpers.RemoveDiacritics(this._searchText);
			if (!isAppending || this.SearchText.Length == this.MinCharAmountToShowResults || isPasted)
			{
				this.SearchResults.Clear();
				foreach (EncyclopediaPage encyclopediaPage in Campaign.Current.EncyclopediaManager.GetEncyclopediaPages())
				{
					foreach (EncyclopediaListItem encyclopediaListItem in encyclopediaPage.GetListItems())
					{
						int num = StringHelpers.RemoveDiacritics(encyclopediaListItem.Name).IndexOf(text, StringComparison.InvariantCultureIgnoreCase);
						if (num >= 0)
						{
							this.SearchResults.Add(new EncyclopediaSearchResultVM(encyclopediaListItem, text, num));
						}
					}
				}
				this._searchResultComparer.SearchText = text;
				this.SearchResults.Sort(this._searchResultComparer);
				return;
			}
			if (isAppending)
			{
				foreach (EncyclopediaSearchResultVM encyclopediaSearchResultVM in this.SearchResults.ToList<EncyclopediaSearchResultVM>())
				{
					if (StringHelpers.RemoveDiacritics(encyclopediaSearchResultVM.OrgNameText).IndexOf(text, StringComparison.InvariantCultureIgnoreCase) == -1)
					{
						this.SearchResults.Remove(encyclopediaSearchResultVM);
					}
					else
					{
						encyclopediaSearchResultVM.UpdateSearchedText(text);
					}
				}
			}
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0004F2AC File Offset: 0x0004D4AC
		private static int GetFirstAsianCharIndex(string searchText)
		{
			for (int i = 0; i < searchText.Length; i++)
			{
				if (Common.IsCharAsian(searchText[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0004F2DB File Offset: 0x0004D4DB
		public void ResetSearch()
		{
			this.SearchText = string.Empty;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0004F2E8 File Offset: 0x0004D4E8
		public void ExecuteOnSearchActivated()
		{
			Game.Current.EventManager.TriggerEvent<OnEncyclopediaSearchActivatedEvent>(new OnEncyclopediaSearchActivatedEvent());
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x0004F2FE File Offset: 0x0004D4FE
		// (set) Token: 0x06001388 RID: 5000 RVA: 0x0004F306 File Offset: 0x0004D506
		[DataSourceProperty]
		public bool CanSwitchTabs
		{
			get
			{
				return this._canSwitchTabs;
			}
			set
			{
				if (value != this._canSwitchTabs)
				{
					this._canSwitchTabs = value;
					base.OnPropertyChangedWithValue(value, "CanSwitchTabs");
				}
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x0004F324 File Offset: 0x0004D524
		// (set) Token: 0x0600138A RID: 5002 RVA: 0x0004F32C File Offset: 0x0004D52C
		[DataSourceProperty]
		public bool IsBackEnabled
		{
			get
			{
				return this._isBackEnabled;
			}
			set
			{
				if (value != this._isBackEnabled)
				{
					this._isBackEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBackEnabled");
				}
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x0004F34A File Offset: 0x0004D54A
		// (set) Token: 0x0600138C RID: 5004 RVA: 0x0004F352 File Offset: 0x0004D552
		[DataSourceProperty]
		public bool IsForwardEnabled
		{
			get
			{
				return this._isForwardEnabled;
			}
			set
			{
				if (value != this._isForwardEnabled)
				{
					this._isForwardEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsForwardEnabled");
				}
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x0004F370 File Offset: 0x0004D570
		// (set) Token: 0x0600138E RID: 5006 RVA: 0x0004F378 File Offset: 0x0004D578
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

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x0004F396 File Offset: 0x0004D596
		// (set) Token: 0x06001390 RID: 5008 RVA: 0x0004F39E File Offset: 0x0004D59E
		[DataSourceProperty]
		public bool IsSearchResultsShown
		{
			get
			{
				return this._isSearchResultsShown;
			}
			set
			{
				if (value != this._isSearchResultsShown)
				{
					this._isSearchResultsShown = value;
					base.OnPropertyChangedWithValue(value, "IsSearchResultsShown");
				}
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x0004F3BC File Offset: 0x0004D5BC
		// (set) Token: 0x06001392 RID: 5010 RVA: 0x0004F3C4 File Offset: 0x0004D5C4
		[DataSourceProperty]
		public string NavBarString
		{
			get
			{
				return this._navBarString;
			}
			set
			{
				if (value != this._navBarString)
				{
					this._navBarString = value;
					base.OnPropertyChangedWithValue<string>(value, "NavBarString");
				}
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x0004F3E7 File Offset: 0x0004D5E7
		// (set) Token: 0x06001394 RID: 5012 RVA: 0x0004F3EF File Offset: 0x0004D5EF
		[DataSourceProperty]
		public string PageName
		{
			get
			{
				return this._pageName;
			}
			set
			{
				if (value != this._pageName)
				{
					this._pageName = value;
					base.OnPropertyChangedWithValue<string>(value, "PageName");
				}
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x0004F412 File Offset: 0x0004D612
		// (set) Token: 0x06001396 RID: 5014 RVA: 0x0004F41A File Offset: 0x0004D61A
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001397 RID: 5015 RVA: 0x0004F43D File Offset: 0x0004D63D
		// (set) Token: 0x06001398 RID: 5016 RVA: 0x0004F445 File Offset: 0x0004D645
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._leaderText;
			}
			set
			{
				if (value != this._leaderText)
				{
					this._leaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderText");
				}
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x0004F468 File Offset: 0x0004D668
		// (set) Token: 0x0600139A RID: 5018 RVA: 0x0004F470 File Offset: 0x0004D670
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSearchResultVM> SearchResults
		{
			get
			{
				return this._searchResults;
			}
			set
			{
				if (value != this._searchResults)
				{
					this._searchResults = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSearchResultVM>>(value, "SearchResults");
				}
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600139B RID: 5019 RVA: 0x0004F48E File Offset: 0x0004D68E
		// (set) Token: 0x0600139C RID: 5020 RVA: 0x0004F498 File Offset: 0x0004D698
		[DataSourceProperty]
		public string SearchText
		{
			get
			{
				return this._searchText;
			}
			set
			{
				if (value != this._searchText)
				{
					bool flag = value.ToLower().Contains(this._searchText);
					bool flag2 = string.IsNullOrEmpty(this._searchText) && !string.IsNullOrEmpty(value);
					this._searchText = value.ToLower();
					Debug.Print("isAppending: " + flag.ToString() + " isPasted: " + flag2.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
					this.RefreshSearch(flag, flag2);
					base.OnPropertyChangedWithValue<string>(value, "SearchText");
				}
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600139D RID: 5021 RVA: 0x0004F52D File Offset: 0x0004D72D
		// (set) Token: 0x0600139E RID: 5022 RVA: 0x0004F535 File Offset: 0x0004D735
		[DataSourceProperty]
		public int MinCharAmountToShowResults
		{
			get
			{
				return this._minCharAmountToShowResults;
			}
			set
			{
				if (value != this._minCharAmountToShowResults)
				{
					this._minCharAmountToShowResults = value;
					base.OnPropertyChangedWithValue(value, "MinCharAmountToShowResults");
				}
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x0004F553 File Offset: 0x0004D753
		// (set) Token: 0x060013A0 RID: 5024 RVA: 0x0004F55B File Offset: 0x0004D75B
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

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x0004F579 File Offset: 0x0004D779
		// (set) Token: 0x060013A2 RID: 5026 RVA: 0x0004F581 File Offset: 0x0004D781
		[DataSourceProperty]
		public InputKeyItemVM PreviousPageInputKey
		{
			get
			{
				return this._previousPageInputKey;
			}
			set
			{
				if (value != this._previousPageInputKey)
				{
					this._previousPageInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousPageInputKey");
				}
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x0004F59F File Offset: 0x0004D79F
		// (set) Token: 0x060013A4 RID: 5028 RVA: 0x0004F5A7 File Offset: 0x0004D7A7
		[DataSourceProperty]
		public InputKeyItemVM NextPageInputKey
		{
			get
			{
				return this._nextPageInputKey;
			}
			set
			{
				if (value != this._nextPageInputKey)
				{
					this._nextPageInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextPageInputKey");
				}
			}
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0004F5C5 File Offset: 0x0004D7C5
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0004F5D4 File Offset: 0x0004D7D4
		public void SetPreviousPageInputKey(HotKey hotkey)
		{
			this.PreviousPageInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0004F5E3 File Offset: 0x0004D7E3
		public void SetNextPageInputKey(HotKey hotkey)
		{
			this.NextPageInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x040008DB RID: 2267
		private List<Tuple<string, object>> History;

		// Token: 0x040008DC RID: 2268
		private int HistoryIndex;

		// Token: 0x040008DD RID: 2269
		private readonly Func<string, object, bool, EncyclopediaPageVM> _goToLink;

		// Token: 0x040008DE RID: 2270
		private readonly Action _closeEncyclopedia;

		// Token: 0x040008DF RID: 2271
		private EncyclopediaNavigatorVM.SearchResultComparer _searchResultComparer;

		// Token: 0x040008E0 RID: 2272
		private MBBindingList<EncyclopediaSearchResultVM> _searchResults;

		// Token: 0x040008E1 RID: 2273
		private string _searchText = "";

		// Token: 0x040008E2 RID: 2274
		private string _pageName;

		// Token: 0x040008E3 RID: 2275
		private string _doneText;

		// Token: 0x040008E4 RID: 2276
		private string _leaderText;

		// Token: 0x040008E5 RID: 2277
		private bool _canSwitchTabs;

		// Token: 0x040008E6 RID: 2278
		private bool _isBackEnabled;

		// Token: 0x040008E7 RID: 2279
		private bool _isForwardEnabled;

		// Token: 0x040008E8 RID: 2280
		private bool _isHighlightEnabled;

		// Token: 0x040008E9 RID: 2281
		private bool _isSearchResultsShown;

		// Token: 0x040008EA RID: 2282
		private string _navBarString;

		// Token: 0x040008EB RID: 2283
		private int _minCharAmountToShowResults;

		// Token: 0x040008EC RID: 2284
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040008ED RID: 2285
		private InputKeyItemVM _previousPageInputKey;

		// Token: 0x040008EE RID: 2286
		private InputKeyItemVM _nextPageInputKey;

		// Token: 0x02000246 RID: 582
		private class SearchResultComparer : IComparer<EncyclopediaSearchResultVM>
		{
			// Token: 0x17000C2F RID: 3119
			// (get) Token: 0x06002624 RID: 9764 RVA: 0x00082ECB File Offset: 0x000810CB
			// (set) Token: 0x06002625 RID: 9765 RVA: 0x00082ED3 File Offset: 0x000810D3
			public string SearchText
			{
				get
				{
					return this._searchText;
				}
				set
				{
					if (value != this._searchText)
					{
						this._searchText = value;
					}
				}
			}

			// Token: 0x06002626 RID: 9766 RVA: 0x00082EEA File Offset: 0x000810EA
			public SearchResultComparer(string searchText)
			{
				this.SearchText = searchText;
			}

			// Token: 0x06002627 RID: 9767 RVA: 0x00082EFC File Offset: 0x000810FC
			private int CompareBasedOnCapitalization(EncyclopediaSearchResultVM x, EncyclopediaSearchResultVM y)
			{
				int num = ((x.NameText.Length > 0 && char.IsUpper(x.NameText[0])) ? 1 : (-1));
				int num2 = ((y.NameText.Length > 0 && char.IsUpper(y.NameText[0])) ? 1 : (-1));
				return num.CompareTo(num2);
			}

			// Token: 0x06002628 RID: 9768 RVA: 0x00082F60 File Offset: 0x00081160
			public int Compare(EncyclopediaSearchResultVM x, EncyclopediaSearchResultVM y)
			{
				if (x.MatchStartIndex != y.MatchStartIndex)
				{
					return y.MatchStartIndex.CompareTo(x.MatchStartIndex);
				}
				int num = this.CompareBasedOnCapitalization(x, y);
				if (num == 0)
				{
					return y.NameText.Length.CompareTo(x.NameText.Length);
				}
				return num;
			}

			// Token: 0x0400128C RID: 4748
			private string _searchText;
		}
	}
}
