using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.Menu
{
	// Token: 0x0200003D RID: 61
	public class MenuViewContext : IMenuContextHandler
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0001315D File Offset: 0x0001135D
		internal GameMenu CurGameMenu
		{
			get
			{
				return this._menuContext.GameMenu;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0001316A File Offset: 0x0001136A
		public MenuContext MenuContext
		{
			get
			{
				return this._menuContext;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00013172 File Offset: 0x00011372
		// (set) Token: 0x060001CC RID: 460 RVA: 0x0001317A File Offset: 0x0001137A
		public List<MenuView> MenuViews { get; private set; }

		// Token: 0x060001CD RID: 461 RVA: 0x00013184 File Offset: 0x00011384
		public MenuViewContext(ScreenBase screen, MenuContext menuContext)
		{
			this._screen = screen;
			this._menuContext = menuContext;
			this.MenuViews = new List<MenuView>();
			this._menuContext.Handler = this;
			if (Campaign.Current.GameMode != CampaignGameMode.Tutorial && this.CurGameMenu.StringId != "siege_test_menu")
			{
				((IMenuContextHandler)this).OnMenuCreate();
				((IMenuContextHandler)this).OnMenuActivate();
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000131EC File Offset: 0x000113EC
		public void UpdateMenuContext(MenuContext menuContext)
		{
			this._menuContext = menuContext;
			this._menuContext.Handler = this;
			this.MenuViews.ForEach(delegate(MenuView m)
			{
				m.MenuContext = menuContext;
			});
			this.MenuViews.ForEach(delegate(MenuView m)
			{
				m.OnMenuContextUpdated(menuContext);
			});
			this.CheckAndInitializeOverlay();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00013252 File Offset: 0x00011452
		public void AddLayer(ScreenLayer layer)
		{
			this._screen.AddLayer(layer);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00013260 File Offset: 0x00011460
		public void RemoveLayer(ScreenLayer layer)
		{
			this._screen.RemoveLayer(layer);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0001326E File Offset: 0x0001146E
		public T FindLayer<T>() where T : ScreenLayer
		{
			return this._screen.FindLayer<T>();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0001327B File Offset: 0x0001147B
		public T FindLayer<T>(string name) where T : ScreenLayer
		{
			return this._screen.FindLayer<T>(name);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0001328C File Offset: 0x0001148C
		public void OnFrameTick(float dt)
		{
			if (this._isBackgroundMeshDirty)
			{
				foreach (MenuView menuView in this.MenuViews)
				{
					menuView.OnBackgroundMeshNameSet(this._menuContext.CurrentBackgroundMeshName);
				}
				this._isBackgroundMeshDirty = false;
			}
			if (this._isAmbientSoundDirty)
			{
				if (!string.IsNullOrEmpty(this._menuContext.CurrentAmbientSoundID))
				{
					this.PlayAmbientSound(this._menuContext.CurrentAmbientSoundID);
				}
				else
				{
					SoundEvent ambientSound = this._ambientSound;
					if (ambientSound != null)
					{
						ambientSound.Release();
					}
					this._ambientSound = null;
				}
				this._isAmbientSoundDirty = false;
			}
			if (this._isPanelSoundDirty)
			{
				if (!string.IsNullOrEmpty(this._menuContext.CurrentPanelSoundID))
				{
					this.PlayPanelSound(this._menuContext.CurrentPanelSoundID);
				}
				else
				{
					SoundEvent panelSound = this._panelSound;
					if (panelSound != null)
					{
						panelSound.Release();
					}
					this._panelSound = null;
				}
				this._isPanelSoundDirty = false;
			}
			for (int i = 0; i < this.MenuViews.Count; i++)
			{
				MenuView menuView2 = this.MenuViews[i];
				menuView2.OnFrameTick(dt);
				if (menuView2.Removed)
				{
					i--;
				}
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000133C4 File Offset: 0x000115C4
		public void OnResume()
		{
			this._isActive = true;
			for (int i = 0; i < this.MenuViews.Count; i++)
			{
				this.MenuViews[i].OnResume();
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00013400 File Offset: 0x00011600
		public void OnHourlyTick()
		{
			for (int i = 0; i < this.MenuViews.Count; i++)
			{
				this.MenuViews[i].OnHourlyTick();
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00013434 File Offset: 0x00011634
		public void OnActivate()
		{
			this._isActive = true;
			for (int i = 0; i < this.MenuViews.Count; i++)
			{
				this.MenuViews[i].OnActivate();
			}
			this._isPanelSoundDirty = true;
			this._isAmbientSoundDirty = true;
			this._isBackgroundMeshDirty = true;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00013484 File Offset: 0x00011684
		public void OnDeactivate()
		{
			this._isActive = false;
			for (int i = 0; i < this.MenuViews.Count; i++)
			{
				this.MenuViews[i].OnDeactivate();
			}
			this.StopAllSounds();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000134C5 File Offset: 0x000116C5
		public void OnInitialize()
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000134C7 File Offset: 0x000116C7
		public void OnFinalize()
		{
			this.ClearMenuViews();
			MBInformationManager.HideInformations();
			this._menuContext = null;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000134DC File Offset: 0x000116DC
		private void ClearMenuViews()
		{
			foreach (MenuView menuView in this.MenuViews.ToArray())
			{
				this.RemoveMenuView(menuView);
			}
			this._menuCharacterDeveloper = null;
			this._menuOverlayBase = null;
			this._menuRecruitVolunteers = null;
			this._menuTownManagement = null;
			this._menuTroopSelection = null;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00013531 File Offset: 0x00011731
		public void StopAllSounds()
		{
			SoundEvent ambientSound = this._ambientSound;
			if (ambientSound != null)
			{
				ambientSound.Release();
			}
			this._ambientSound = null;
			SoundEvent panelSound = this._panelSound;
			if (panelSound != null)
			{
				panelSound.Release();
			}
			this._panelSound = null;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00013563 File Offset: 0x00011763
		private void PlayAmbientSound(string ambientSoundID)
		{
			if (this._isActive)
			{
				SoundEvent ambientSound = this._ambientSound;
				if (ambientSound != null)
				{
					ambientSound.Release();
				}
				this._ambientSound = SoundEvent.CreateEventFromString(ambientSoundID, null);
				this._ambientSound.Play();
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00013597 File Offset: 0x00011797
		private void PlayPanelSound(string panelSoundID)
		{
			if (this._isActive)
			{
				SoundEvent panelSound = this._panelSound;
				if (panelSound != null)
				{
					panelSound.Release();
				}
				this._panelSound = SoundEvent.CreateEventFromString(panelSoundID, null);
				this._panelSound.Play();
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000135CC File Offset: 0x000117CC
		public void OnMapConversationActivated()
		{
			for (int i = 0; i < this.MenuViews.Count; i++)
			{
				MenuView menuView = this.MenuViews[i];
				menuView.OnMapConversationActivated();
				if (menuView.Removed)
				{
					i--;
				}
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0001360C File Offset: 0x0001180C
		public void OnMapConversationDeactivated()
		{
			for (int i = 0; i < this.MenuViews.Count; i++)
			{
				MenuView menuView = this.MenuViews[i];
				menuView.OnMapConversationDeactivated();
				if (menuView.Removed)
				{
					i--;
				}
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0001364C File Offset: 0x0001184C
		public void OnGameStateDeactivate()
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0001364E File Offset: 0x0001184E
		public void OnGameStateInitialize()
		{
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00013650 File Offset: 0x00011850
		public void OnGameStateFinalize()
		{
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00013654 File Offset: 0x00011854
		private void CheckAndInitializeOverlay()
		{
			GameMenu.MenuOverlayType menuOverlayType = Campaign.Current.GameMenuManager.GetMenuOverlayType(this._menuContext);
			if (menuOverlayType != GameMenu.MenuOverlayType.None)
			{
				if (menuOverlayType != this._currentOverlayType)
				{
					if (this._menuOverlayBase != null && ((this._currentOverlayType != GameMenu.MenuOverlayType.Encounter && menuOverlayType == GameMenu.MenuOverlayType.Encounter) || (this._currentOverlayType == GameMenu.MenuOverlayType.Encounter && (menuOverlayType == GameMenu.MenuOverlayType.SettlementWithBoth || menuOverlayType == GameMenu.MenuOverlayType.SettlementWithCharacters || menuOverlayType == GameMenu.MenuOverlayType.SettlementWithParties))))
					{
						this.RemoveMenuView(this._menuOverlayBase);
						this._menuOverlayBase = null;
					}
					if (this._menuOverlayBase == null)
					{
						this._menuOverlayBase = this.AddMenuView<MenuOverlayBaseView>(Array.Empty<object>());
					}
					else
					{
						this._menuOverlayBase.OnOverlayTypeChange(menuOverlayType);
					}
				}
				else
				{
					MenuView menuOverlayBase = this._menuOverlayBase;
					if (menuOverlayBase != null)
					{
						menuOverlayBase.OnOverlayTypeChange(menuOverlayType);
					}
				}
			}
			else
			{
				if (this._menuOverlayBase != null)
				{
					this.RemoveMenuView(this._menuOverlayBase);
					this._menuOverlayBase = null;
				}
				if (this._currentMenuBackground != null)
				{
					this.RemoveMenuView(this._currentMenuBackground);
					this._currentMenuBackground = null;
				}
			}
			this._currentOverlayType = menuOverlayType;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00013740 File Offset: 0x00011940
		public void CloseCharacterDeveloper()
		{
			this.RemoveMenuView(this._menuCharacterDeveloper);
			this._menuCharacterDeveloper = null;
			foreach (MenuView menuView in this.MenuViews)
			{
				menuView.OnCharacterDeveloperClosed();
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000137A4 File Offset: 0x000119A4
		public MenuView AddMenuView<T>(params object[] parameters) where T : MenuView, new()
		{
			MenuView menuView = SandBoxViewCreator.CreateMenuView<T>(parameters);
			menuView.MenuViewContext = this;
			menuView.MenuContext = this._menuContext;
			this.MenuViews.Add(menuView);
			menuView.OnInitialize();
			return menuView;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000137E0 File Offset: 0x000119E0
		public T GetMenuView<T>() where T : MenuView
		{
			foreach (MenuView menuView in this.MenuViews)
			{
				T t = menuView as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00013850 File Offset: 0x00011A50
		public void RemoveMenuView(MenuView menuView)
		{
			menuView.OnFinalize();
			menuView.Removed = true;
			this.MenuViews.Remove(menuView);
			if (menuView.ShouldUpdateMenuAfterRemoved)
			{
				this.MenuViews.ForEach(delegate(MenuView m)
				{
					m.OnMenuContextUpdated(this._menuContext);
				});
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0001388B File Offset: 0x00011A8B
		public void CloseTownManagement()
		{
			this.RemoveMenuView(this._menuTownManagement);
			this._menuTownManagement = null;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000138A0 File Offset: 0x00011AA0
		public void CloseRecruitVolunteers()
		{
			this.RemoveMenuView(this._menuRecruitVolunteers);
			this._menuRecruitVolunteers = null;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000138B5 File Offset: 0x00011AB5
		public void CloseTournamentLeaderboard()
		{
			this.RemoveMenuView(this._menuTournamentLeaderboard);
			this._menuTournamentLeaderboard = null;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000138CA File Offset: 0x00011ACA
		public void CloseTroopSelection()
		{
			this.RemoveMenuView(this._menuTroopSelection);
			this._menuTroopSelection = null;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000138DF File Offset: 0x00011ADF
		protected virtual MenuView CreateTroopSelectionView(TroopRoster fullRoster, TroopRoster initialSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster> onDone, int maxSelectableTroopCount, int minSelectableTroopCount)
		{
			return this.AddMenuView<MenuTroopSelectionView>(new object[] { fullRoster, initialSelections, canChangeStatusOfTroop, onDone, maxSelectableTroopCount, minSelectableTroopCount });
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00013912 File Offset: 0x00011B12
		protected virtual MenuView CreateNavalTroopSelectionView(TroopRoster fullRoster, TroopRoster initialTroopSelections, List<Ship> eligibleShips, List<Ship> initialShipSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster, List<Ship>> onDone, int minSelectableTroopCount, int minSelectableShipCount, int maxSelectableShipCount, bool anyOtherPartiesOnPlayerSide)
		{
			return null;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00013915 File Offset: 0x00011B15
		void IMenuContextHandler.OnAmbientSoundIDSet(string ambientSoundID)
		{
			this._isAmbientSoundDirty = true;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0001391E File Offset: 0x00011B1E
		void IMenuContextHandler.OnPanelSoundIDSet(string panelSoundID)
		{
			this._isPanelSoundDirty = true;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00013928 File Offset: 0x00011B28
		void IMenuContextHandler.OnMenuCreate()
		{
			bool flag = Campaign.Current.GameMode == CampaignGameMode.Tutorial || this.CurGameMenu.StringId == "siege_test_menu";
			if (flag && this._currentMenuBackground == null)
			{
				this._currentMenuBackground = this.AddMenuView<MenuBackgroundView>(Array.Empty<object>());
			}
			if (this._currentMenuBase == null)
			{
				this._currentMenuBase = this.AddMenuView<MenuBaseView>(Array.Empty<object>());
			}
			if (!flag)
			{
				this.CheckAndInitializeOverlay();
			}
			this.StopAllSounds();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000139A0 File Offset: 0x00011BA0
		void IMenuContextHandler.OnMenuActivate()
		{
			foreach (MenuView menuView in this.MenuViews)
			{
				menuView.OnActivate();
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000139F0 File Offset: 0x00011BF0
		void IMenuContextHandler.OnBackgroundMeshNameSet(string name)
		{
			this._isBackgroundMeshDirty = true;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000139F9 File Offset: 0x00011BF9
		void IMenuContextHandler.OnOpenTownManagement()
		{
			if (this._menuTownManagement == null)
			{
				this._menuTownManagement = this.AddMenuView<MenuTownManagementView>(Array.Empty<object>());
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00013A14 File Offset: 0x00011C14
		void IMenuContextHandler.OnOpenRecruitVolunteers()
		{
			if (this._menuRecruitVolunteers == null)
			{
				this._menuRecruitVolunteers = this.AddMenuView<MenuRecruitVolunteersView>(Array.Empty<object>());
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00013A2F File Offset: 0x00011C2F
		void IMenuContextHandler.OnOpenTournamentLeaderboard()
		{
			if (this._menuTournamentLeaderboard == null)
			{
				this._menuTournamentLeaderboard = this.AddMenuView<MenuTournamentLeaderboardView>(Array.Empty<object>());
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00013A4A File Offset: 0x00011C4A
		void IMenuContextHandler.OnOpenTroopSelection(TroopRoster fullRoster, TroopRoster initialSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster> onDone, int maxSelectableTroopCount, int minSelectableTroopCount)
		{
			if (this._menuTroopSelection == null)
			{
				this._menuTroopSelection = this.CreateTroopSelectionView(fullRoster, initialSelections, canChangeStatusOfTroop, onDone, maxSelectableTroopCount, minSelectableTroopCount);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00013A6C File Offset: 0x00011C6C
		void IMenuContextHandler.OnOpenNavalTroopSelection(TroopRoster fullRoster, TroopRoster initialTroopSelections, List<Ship> eligibleShips, List<Ship> initialShipSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster, List<Ship>> onDone, int minSelectableTroopCount, int minSelectableShipCount, int maxSelectableShipCount, bool anyOtherPartiesOnPlayerSide)
		{
			if (this._menuTroopSelection == null)
			{
				this._menuTroopSelection = this.CreateNavalTroopSelectionView(fullRoster, initialTroopSelections, eligibleShips, initialShipSelections, canChangeStatusOfTroop, onDone, minSelectableTroopCount, minSelectableShipCount, maxSelectableShipCount, anyOtherPartiesOnPlayerSide);
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00013AA0 File Offset: 0x00011CA0
		void IMenuContextHandler.OnMenuRefresh()
		{
			foreach (MenuView menuView in this.MenuViews)
			{
				menuView.OnMenuContextRefreshed();
			}
		}

		// Token: 0x040000FD RID: 253
		private MenuContext _menuContext;

		// Token: 0x040000FE RID: 254
		private MenuView _currentMenuBase;

		// Token: 0x040000FF RID: 255
		private MenuView _currentMenuBackground;

		// Token: 0x04000100 RID: 256
		private MenuView _menuCharacterDeveloper;

		// Token: 0x04000101 RID: 257
		private MenuView _menuOverlayBase;

		// Token: 0x04000102 RID: 258
		private MenuView _menuRecruitVolunteers;

		// Token: 0x04000103 RID: 259
		private MenuView _menuTournamentLeaderboard;

		// Token: 0x04000104 RID: 260
		private MenuView _menuTroopSelection;

		// Token: 0x04000105 RID: 261
		private MenuView _menuTownManagement;

		// Token: 0x04000106 RID: 262
		private SoundEvent _panelSound;

		// Token: 0x04000107 RID: 263
		private SoundEvent _ambientSound;

		// Token: 0x04000108 RID: 264
		private GameMenu.MenuOverlayType _currentOverlayType;

		// Token: 0x0400010A RID: 266
		private ScreenBase _screen;

		// Token: 0x0400010B RID: 267
		private bool _isActive;

		// Token: 0x0400010C RID: 268
		private bool _isAmbientSoundDirty;

		// Token: 0x0400010D RID: 269
		private bool _isPanelSoundDirty;

		// Token: 0x0400010E RID: 270
		private bool _isBackgroundMeshDirty;
	}
}
