using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x02000064 RID: 100
	public class MapNavigationVM : ViewModel
	{
		// Token: 0x060006E3 RID: 1763 RVA: 0x00022690 File Offset: 0x00020890
		public MapNavigationVM(INavigationHandler navigationHandler, Func<MapBarShortcuts> getMapBarShortcuts)
		{
			this._navigationHandler = navigationHandler;
			this._getMapBarShortcuts = getMapBarShortcuts;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			this.NavigationItems = new MBBindingList<MapNavigationItemVM>();
			INavigationElement[] elements = navigationHandler.GetElements();
			for (int i = 0; i < elements.Length; i++)
			{
				this.NavigationItems.Add(new MapNavigationItemVM(elements[i]));
			}
			this.RefreshValues();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000226FC File Offset: 0x000208FC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._shortcuts = this._getMapBarShortcuts();
			this.EncyclopediaHint = new HintViewModel(GameTexts.FindText("str_encyclopedia", null), null);
			this.CampHint = new HintViewModel(GameTexts.FindText("str_camp", null), null);
			this.FinanceHint = new HintViewModel(GameTexts.FindText("str_finance", null), null);
			this.CenterCameraHint = new HintViewModel(GameTexts.FindText("str_return_to_hero", null), null);
			this.Refresh();
			this.NavigationItems.ApplyActionOnAllItems(delegate(MapNavigationItemVM n)
			{
				n.RefreshValues();
			});
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x000227AC File Offset: 0x000209AC
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._navigationHandler = null;
			this._getMapBarShortcuts = null;
			this.NavigationItems.ApplyActionOnAllItems(delegate(MapNavigationItemVM n)
			{
				n.OnFinalize();
			});
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000227EC File Offset: 0x000209EC
		public void Refresh()
		{
			this.RefreshStates();
			this._viewDataTracker.UpdatePartyNotification();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000227FF File Offset: 0x000209FF
		public void Tick()
		{
			this.RefreshStates();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00022807 File Offset: 0x00020A07
		protected virtual void RefreshStates()
		{
			this.NavigationItems.ApplyActionOnAllItems(delegate(MapNavigationItemVM n)
			{
				n.RefreshStates(false);
			});
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00022833 File Offset: 0x00020A33
		public void ExecuteOpenQuests()
		{
			this._navigationHandler.OpenQuests();
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00022840 File Offset: 0x00020A40
		public void ExecuteOpenInventory()
		{
			this._navigationHandler.OpenInventory();
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002284D File Offset: 0x00020A4D
		public void ExecuteOpenParty()
		{
			this._navigationHandler.OpenParty();
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002285A File Offset: 0x00020A5A
		public void ExecuteOpenCharacterDeveloper()
		{
			this._navigationHandler.OpenCharacterDeveloper();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00022867 File Offset: 0x00020A67
		public void ExecuteOpenKingdom()
		{
			this._navigationHandler.OpenKingdom();
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00022874 File Offset: 0x00020A74
		public void ExecuteOpenClan()
		{
			this._navigationHandler.OpenClan();
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00022881 File Offset: 0x00020A81
		public void ExecuteOpenEscapeMenu()
		{
			this._navigationHandler.OpenEscapeMenu();
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002288E File Offset: 0x00020A8E
		public void ExecuteOpenMainHeroKingdomEncyclopedia()
		{
			if (Hero.MainHero.MapFaction != null)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.MapFaction.EncyclopediaLink);
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x000228BA File Offset: 0x00020ABA
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x000228C2 File Offset: 0x00020AC2
		[DataSourceProperty]
		public MBBindingList<MapNavigationItemVM> NavigationItems
		{
			get
			{
				return this._navigationItems;
			}
			set
			{
				if (value != this._navigationItems)
				{
					this._navigationItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<MapNavigationItemVM>>(value, "NavigationItems");
				}
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x000228E0 File Offset: 0x00020AE0
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x000228E8 File Offset: 0x00020AE8
		[DataSourceProperty]
		public HintViewModel FinanceHint
		{
			get
			{
				return this._financeHint;
			}
			set
			{
				if (value != this._financeHint)
				{
					this._financeHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FinanceHint");
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00022906 File Offset: 0x00020B06
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0002290E File Offset: 0x00020B0E
		[DataSourceProperty]
		public HintViewModel EncyclopediaHint
		{
			get
			{
				return this._encyclopediaHint;
			}
			set
			{
				if (value != this._encyclopediaHint)
				{
					this._encyclopediaHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EncyclopediaHint");
				}
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0002292C File Offset: 0x00020B2C
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x00022934 File Offset: 0x00020B34
		[DataSourceProperty]
		public HintViewModel CenterCameraHint
		{
			get
			{
				return this._centerCameraHint;
			}
			set
			{
				if (value != this._centerCameraHint)
				{
					this._centerCameraHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CenterCameraHint");
				}
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00022952 File Offset: 0x00020B52
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x0002295A File Offset: 0x00020B5A
		[DataSourceProperty]
		public HintViewModel CampHint
		{
			get
			{
				return this._campHint;
			}
			set
			{
				if (value != this._campHint)
				{
					this._campHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CampHint");
				}
			}
		}

		// Token: 0x040002F4 RID: 756
		protected INavigationHandler _navigationHandler;

		// Token: 0x040002F5 RID: 757
		protected Func<MapBarShortcuts> _getMapBarShortcuts;

		// Token: 0x040002F6 RID: 758
		protected MapBarShortcuts _shortcuts;

		// Token: 0x040002F7 RID: 759
		protected readonly IViewDataTracker _viewDataTracker;

		// Token: 0x040002F8 RID: 760
		private MBBindingList<MapNavigationItemVM> _navigationItems;

		// Token: 0x040002F9 RID: 761
		private HintViewModel _encyclopediaHint;

		// Token: 0x040002FA RID: 762
		private HintViewModel _financeHint;

		// Token: 0x040002FB RID: 763
		private HintViewModel _centerCameraHint;

		// Token: 0x040002FC RID: 764
		private HintViewModel _campHint;
	}
}
