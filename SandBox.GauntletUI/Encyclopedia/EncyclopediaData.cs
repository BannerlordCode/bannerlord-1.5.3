using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Encyclopedia.Pages;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.List;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Encyclopedia
{
	// Token: 0x02000045 RID: 69
	public class EncyclopediaData
	{
		// Token: 0x06000331 RID: 817 RVA: 0x00012C28 File Offset: 0x00010E28
		public EncyclopediaData(GauntletMapEncyclopediaView manager, ScreenBase screen, EncyclopediaHomeVM homeDatasource, EncyclopediaNavigatorVM navigatorDatasource)
		{
			this._manager = manager;
			this._screen = screen;
			this._pages = new Dictionary<string, EncyclopediaPage>();
			foreach (EncyclopediaPage encyclopediaPage in Campaign.Current.EncyclopediaManager.GetEncyclopediaPages())
			{
				foreach (string text in encyclopediaPage.GetIdentifierNames())
				{
					if (!this._pages.ContainsKey(text))
					{
						this._pages.Add(text, encyclopediaPage);
					}
				}
			}
			this._homeDatasource = homeDatasource;
			this._lists = new Dictionary<EncyclopediaPage, EncyclopediaListVM>();
			foreach (EncyclopediaPage encyclopediaPage2 in Campaign.Current.EncyclopediaManager.GetEncyclopediaPages())
			{
				if (!this._lists.ContainsKey(encyclopediaPage2))
				{
					EncyclopediaListVM encyclopediaListVM = new EncyclopediaListVM(new EncyclopediaPageArgs(encyclopediaPage2));
					this._manager.ListViewDataController.LoadListData(encyclopediaListVM);
					this._lists.Add(encyclopediaPage2, encyclopediaListVM);
				}
			}
			this._navigatorDatasource = navigatorDatasource;
			this._navigatorDatasource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._navigatorDatasource.SetPreviousPageInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToPreviousTab"));
			this._navigatorDatasource.SetNextPageInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SwitchToNextTab"));
			Game.Current.EventManager.RegisterEvent<TutorialContextChangedEvent>(new Action<TutorialContextChangedEvent>(this.OnTutorialContextChanged));
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00012DDC File Offset: 0x00010FDC
		private void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			if (obj.NewContext != TutorialContexts.EncyclopediaWindow)
			{
				this._prevContext = obj.NewContext;
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00012DF4 File Offset: 0x00010FF4
		internal void OnTick()
		{
			this._navigatorDatasource.CanSwitchTabs = !Input.IsGamepadActive || !InformationManager.GetIsAnyTooltipActiveAndExtended();
			if (this._activeGauntletLayer.Input.IsHotKeyReleased("Exit") || (this._activeGauntletLayer.Input.IsGameKeyReleased(39) && !this._activeGauntletLayer.IsFocusedOnInput()))
			{
				if (this._navigatorDatasource.IsSearchResultsShown)
				{
					this._navigatorDatasource.SearchText = string.Empty;
				}
				else
				{
					this._manager.CloseEncyclopedia();
					UISoundsHelper.PlayUISound("event:/ui/default");
				}
			}
			else if (!this._activeGauntletLayer.IsFocusedOnInput() && this._navigatorDatasource.CanSwitchTabs)
			{
				if ((Input.IsKeyPressed(InputKey.BackSpace) && this._navigatorDatasource.IsBackEnabled) || this._activeGauntletLayer.Input.IsHotKeyReleased("SwitchToPreviousTab"))
				{
					this._navigatorDatasource.ExecuteBack();
				}
				else if (this._activeGauntletLayer.Input.IsHotKeyReleased("SwitchToNextTab"))
				{
					this._navigatorDatasource.ExecuteForward();
				}
			}
			if (this._activeGauntletLayer != null)
			{
				object initialState = this._initialState;
				Game game = Game.Current;
				object obj;
				if (game == null)
				{
					obj = null;
				}
				else
				{
					GameStateManager gameStateManager = game.GameStateManager;
					obj = ((gameStateManager != null) ? gameStateManager.ActiveState : null);
				}
				if (initialState != obj)
				{
					this._manager.CloseEncyclopedia();
				}
			}
			EncyclopediaPageVM activeDatasource = this._activeDatasource;
			if (activeDatasource == null)
			{
				return;
			}
			activeDatasource.OnTick();
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00012F50 File Offset: 0x00011150
		private void SetEncyclopediaPage(string pageId, object obj)
		{
			GauntletLayer activeGauntletLayer = this._activeGauntletLayer;
			if (this._activeGauntletLayer != null && this._activeGauntletMovie != null)
			{
				this._activeGauntletLayer.ReleaseMovie(this._activeGauntletMovie);
			}
			EncyclopediaListVM encyclopediaListVM;
			if ((encyclopediaListVM = this._activeDatasource as EncyclopediaListVM) != null)
			{
				EncyclopediaListItemVM encyclopediaListItemVM = encyclopediaListVM.Items.FirstOrDefault<EncyclopediaListItemVM>((EncyclopediaListItemVM x) => x.Object == obj);
				this._manager.ListViewDataController.SaveListData(encyclopediaListVM, (encyclopediaListItemVM != null) ? encyclopediaListItemVM.Id : encyclopediaListVM.LastSelectedItemId);
			}
			if (this._activeGauntletLayer == null)
			{
				this._activeGauntletLayer = new GauntletLayer("EncyclopediaBar", 310, false);
				this._navigatorActiveGauntletMovie = this._activeGauntletLayer.LoadMovie("EncyclopediaBar", this._navigatorDatasource);
				this._navigatorDatasource.PageName = this._homeDatasource.GetName();
				this._activeGauntletLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this._activeGauntletLayer);
				this._activeGauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
				this._activeGauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
				Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
				Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.EncyclopediaWindow));
				this._initialState = Game.Current.GameStateManager.ActiveState;
			}
			if (pageId == "Home")
			{
				this._activeGauntletMovie = this._activeGauntletLayer.LoadMovie("EncyclopediaHome", this._homeDatasource);
				this._homeGauntletMovie = this._activeGauntletMovie;
				this._activeDatasource = this._homeDatasource;
				this._activeDatasource.Refresh();
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Home, false));
			}
			else if (pageId == "ListPage")
			{
				EncyclopediaPage encyclopediaPage = obj as EncyclopediaPage;
				this._activeDatasource = this._lists[encyclopediaPage];
				this._activeGauntletMovie = this._activeGauntletLayer.LoadMovie("EncyclopediaItemList", this._activeDatasource);
				this._activeDatasource.Refresh();
				this._manager.ListViewDataController.LoadListData(this._activeDatasource as EncyclopediaListVM);
				this.SetTutorialListPageContext(encyclopediaPage);
			}
			else
			{
				EncyclopediaPage encyclopediaPage2 = this._pages[pageId];
				this._activeDatasource = this.GetEncyclopediaPageInstance(encyclopediaPage2, obj);
				EncyclopediaContentPageVM encyclopediaContentPageVM = this._activeDatasource as EncyclopediaContentPageVM;
				if (encyclopediaContentPageVM != null)
				{
					encyclopediaContentPageVM.InitializeQuickNavigation(this._lists[encyclopediaPage2]);
				}
				this._activeGauntletMovie = this._activeGauntletLayer.LoadMovie(this._pages[pageId].GetViewFullyQualifiedName(), this._activeDatasource);
				this.SetTutorialPageContext(this._activeDatasource);
			}
			this._navigatorDatasource.NavBarString = this._activeDatasource.GetNavigationBarURL();
			if (activeGauntletLayer != null && activeGauntletLayer != this._activeGauntletLayer)
			{
				this._screen.RemoveLayer(activeGauntletLayer);
				this._screen.AddLayer(this._activeGauntletLayer);
			}
			else if (activeGauntletLayer == null && this._activeGauntletLayer != null)
			{
				this._screen.AddLayer(this._activeGauntletLayer);
			}
			this._activeGauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._previousPageID = pageId;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00013283 File Offset: 0x00011483
		internal EncyclopediaPageVM ExecuteLink(string pageId, object obj, bool needsRefresh)
		{
			this.SetEncyclopediaPage(pageId, obj);
			return this._activeDatasource;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00013294 File Offset: 0x00011494
		private EncyclopediaPageVM GetEncyclopediaPageInstance(EncyclopediaPage page, object o)
		{
			Type type = null;
			EncyclopediaPageArgs encyclopediaPageArgs = new EncyclopediaPageArgs(o);
			Assembly assembly = typeof(EncyclopediaHomeVM).Assembly;
			List<Type> typesSafe = assembly.GetTypesSafe(null);
			for (int i = 0; i < typesSafe.Count; i++)
			{
				Type type2 = typesSafe[i];
				if (EncyclopediaData.IsEncyclopediaPageType(page, type2))
				{
					type = type2;
				}
			}
			Assembly[] activeReferencingGameAssembliesSafe = assembly.GetActiveReferencingGameAssembliesSafe();
			for (int j = 0; j < activeReferencingGameAssembliesSafe.Length; j++)
			{
				List<Type> typesSafe2 = activeReferencingGameAssembliesSafe[j].GetTypesSafe(null);
				for (int k = 0; k < typesSafe2.Count; k++)
				{
					Type type3 = typesSafe2[k];
					if (EncyclopediaData.IsEncyclopediaPageType(page, type3))
					{
						type = type3;
					}
				}
			}
			if (type != null)
			{
				return Activator.CreateInstance(type, new object[] { encyclopediaPageArgs }) as EncyclopediaPageVM;
			}
			return null;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0001336C File Offset: 0x0001156C
		private static bool IsEncyclopediaPageType(EncyclopediaPage page, Type type)
		{
			if (typeof(EncyclopediaPageVM).IsAssignableFrom(type))
			{
				object[] customAttributesSafe = type.GetCustomAttributesSafe(typeof(EncyclopediaViewModel), false);
				for (int i = 0; i < customAttributesSafe.Length; i++)
				{
					EncyclopediaViewModel encyclopediaViewModel;
					if ((encyclopediaViewModel = customAttributesSafe[i] as EncyclopediaViewModel) != null && page.HasIdentifierType(encyclopediaViewModel.PageTargetType))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000133C8 File Offset: 0x000115C8
		public void OnFinalize()
		{
			Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
			this._pages = null;
			this._homeDatasource = null;
			foreach (KeyValuePair<EncyclopediaPage, EncyclopediaListVM> keyValuePair in this._lists)
			{
				EncyclopediaListVM value = keyValuePair.Value;
				if (value != null)
				{
					value.OnFinalize();
				}
			}
			this._lists = null;
			this._activeGauntletMovie = null;
			this._activeDatasource = null;
			this._activeGauntletLayer = null;
			this._navigatorActiveGauntletMovie = null;
			this._navigatorDatasource = null;
			this._initialState = null;
			Game.Current.EventManager.UnregisterEvent<TutorialContextChangedEvent>(new Action<TutorialContextChangedEvent>(this.OnTutorialContextChanged));
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00013490 File Offset: 0x00011690
		public void CloseEncyclopedia()
		{
			EncyclopediaListVM encyclopediaListVM;
			if ((encyclopediaListVM = this._activeDatasource as EncyclopediaListVM) != null)
			{
				this._manager.ListViewDataController.SaveListData(encyclopediaListVM, encyclopediaListVM.LastSelectedItemId);
			}
			this.ResetPageFilters();
			this._activeGauntletLayer.ReleaseMovie(this._activeGauntletMovie);
			this._screen.RemoveLayer(this._activeGauntletLayer);
			this._activeGauntletLayer.InputRestrictions.ResetInputRestrictions();
			this.OnFinalize();
			Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.None, false));
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(this._prevContext));
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00013534 File Offset: 0x00011734
		private void ResetPageFilters()
		{
			foreach (EncyclopediaListVM encyclopediaListVM in this._lists.Values)
			{
				foreach (EncyclopediaFilterGroupVM encyclopediaFilterGroupVM in encyclopediaListVM.FilterGroups)
				{
					foreach (EncyclopediaListFilterVM encyclopediaListFilterVM in encyclopediaFilterGroupVM.Filters)
					{
						encyclopediaListFilterVM.IsSelected = false;
					}
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000135F0 File Offset: 0x000117F0
		private void SetTutorialPageContext(EncyclopediaPageVM _page)
		{
			if (_page is EncyclopediaClanPageVM)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Clan, false));
				return;
			}
			if (_page is EncyclopediaConceptPageVM)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Concept, false));
				return;
			}
			if (_page is EncyclopediaFactionPageVM)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Kingdom, false));
				return;
			}
			if (_page is EncyclopediaUnitPageVM)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Unit, false));
				return;
			}
			if (_page is EncyclopediaHeroPageVM)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Hero, false));
				return;
			}
			if (_page is EncyclopediaShipPageVM)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Ship, false));
				return;
			}
			if (_page is EncyclopediaSettlementPageVM)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.Settlement, false));
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000136DC File Offset: 0x000118DC
		private void SetTutorialListPageContext(EncyclopediaPage _page)
		{
			if (_page is DefaultEncyclopediaClanPage)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.ListClans, false));
				return;
			}
			if (_page is DefaultEncyclopediaConceptPage)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.ListConcepts, false));
				return;
			}
			if (_page is DefaultEncyclopediaFactionPage)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.ListKingdoms, false));
				return;
			}
			if (_page is DefaultEncyclopediaUnitPage)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.ListUnits, false));
				return;
			}
			if (_page is DefaultEncyclopediaHeroPage)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.ListHeroes, false));
				return;
			}
			if (_page is DefaultEncyclopediaShipPage)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.ListShips, false));
				return;
			}
			if (_page is DefaultEncyclopediaSettlementPage)
			{
				Game.Current.EventManager.TriggerEvent<EncyclopediaPageChangedEvent>(new EncyclopediaPageChangedEvent(EncyclopediaPages.ListSettlements, false));
			}
		}

		// Token: 0x04000133 RID: 307
		private Dictionary<string, EncyclopediaPage> _pages;

		// Token: 0x04000134 RID: 308
		private string _previousPageID;

		// Token: 0x04000135 RID: 309
		private EncyclopediaHomeVM _homeDatasource;

		// Token: 0x04000136 RID: 310
		private GauntletMovieIdentifier _homeGauntletMovie;

		// Token: 0x04000137 RID: 311
		private Dictionary<EncyclopediaPage, EncyclopediaListVM> _lists;

		// Token: 0x04000138 RID: 312
		private EncyclopediaPageVM _activeDatasource;

		// Token: 0x04000139 RID: 313
		private GauntletLayer _activeGauntletLayer;

		// Token: 0x0400013A RID: 314
		private GauntletMovieIdentifier _activeGauntletMovie;

		// Token: 0x0400013B RID: 315
		private EncyclopediaNavigatorVM _navigatorDatasource;

		// Token: 0x0400013C RID: 316
		private GauntletMovieIdentifier _navigatorActiveGauntletMovie;

		// Token: 0x0400013D RID: 317
		private readonly ScreenBase _screen;

		// Token: 0x0400013E RID: 318
		private TutorialContexts _prevContext;

		// Token: 0x0400013F RID: 319
		private readonly GauntletMapEncyclopediaView _manager;

		// Token: 0x04000140 RID: 320
		private object _initialState;
	}
}
