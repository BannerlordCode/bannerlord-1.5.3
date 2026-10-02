using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages
{
	// Token: 0x020000DC RID: 220
	public class EncyclopediaPageVM : ViewModel
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x00052CD0 File Offset: 0x00050ED0
		public object Obj
		{
			get
			{
				return this._args.Obj;
			}
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x00052CDD File Offset: 0x00050EDD
		public virtual string GetName()
		{
			return "";
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x00052CE4 File Offset: 0x00050EE4
		public virtual string GetNavigationBarURL()
		{
			return "";
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x00052CEB File Offset: 0x00050EEB
		public virtual void Refresh()
		{
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00052CED File Offset: 0x00050EED
		public EncyclopediaPageVM(EncyclopediaPageArgs args)
		{
			this._args = args;
			this.BookmarkHint = new HintViewModel();
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00052D07 File Offset: 0x00050F07
		public virtual void OnTick()
		{
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00052D09 File Offset: 0x00050F09
		public virtual void ExecuteSwitchBookmarkedState()
		{
			this.IsBookmarked = !this.IsBookmarked;
			this.UpdateBookmarkHintText();
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00052D20 File Offset: 0x00050F20
		protected void UpdateBookmarkHintText()
		{
			if (this.IsBookmarked)
			{
				this.BookmarkHint.HintText = new TextObject("{=BV5exuPf}Remove From Bookmarks", null);
				return;
			}
			this.BookmarkHint.HintText = new TextObject("{=d8jrv3nA}Add To Bookmarks", null);
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x00052D57 File Offset: 0x00050F57
		// (set) Token: 0x060014BE RID: 5310 RVA: 0x00052D5F File Offset: 0x00050F5F
		[DataSourceProperty]
		public bool IsLoadingOver
		{
			get
			{
				return this._isLoadingOver;
			}
			set
			{
				if (value != this._isLoadingOver)
				{
					this._isLoadingOver = value;
					base.OnPropertyChangedWithValue(value, "IsLoadingOver");
				}
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x00052D7D File Offset: 0x00050F7D
		// (set) Token: 0x060014C0 RID: 5312 RVA: 0x00052D85 File Offset: 0x00050F85
		[DataSourceProperty]
		public bool IsBookmarked
		{
			get
			{
				return this._isBookmarked;
			}
			set
			{
				if (value != this._isBookmarked)
				{
					this._isBookmarked = value;
					base.OnPropertyChanged("IsBookmarked");
				}
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x00052DA2 File Offset: 0x00050FA2
		// (set) Token: 0x060014C2 RID: 5314 RVA: 0x00052DAA File Offset: 0x00050FAA
		[DataSourceProperty]
		public HintViewModel BookmarkHint
		{
			get
			{
				return this._bookmarkHint;
			}
			set
			{
				if (value != this._bookmarkHint)
				{
					this._bookmarkHint = value;
					base.OnPropertyChanged("BookmarkHint");
				}
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x00052DC7 File Offset: 0x00050FC7
		// (set) Token: 0x060014C4 RID: 5316 RVA: 0x00052DCA File Offset: 0x00050FCA
		[DataSourceProperty]
		public virtual MBBindingList<EncyclopediaListItemVM> Items
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060014C5 RID: 5317 RVA: 0x00052DCC File Offset: 0x00050FCC
		// (set) Token: 0x060014C6 RID: 5318 RVA: 0x00052DCF File Offset: 0x00050FCF
		[DataSourceProperty]
		public virtual MBBindingList<EncyclopediaFilterGroupVM> FilterGroups
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x060014C7 RID: 5319 RVA: 0x00052DD1 File Offset: 0x00050FD1
		// (set) Token: 0x060014C8 RID: 5320 RVA: 0x00052DD4 File Offset: 0x00050FD4
		[DataSourceProperty]
		public virtual EncyclopediaListSortControllerVM SortController
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x04000973 RID: 2419
		private EncyclopediaPageArgs _args;

		// Token: 0x04000974 RID: 2420
		private bool _isLoadingOver;

		// Token: 0x04000975 RID: 2421
		private bool _isBookmarked;

		// Token: 0x04000976 RID: 2422
		private HintViewModel _bookmarkHint;
	}
}
