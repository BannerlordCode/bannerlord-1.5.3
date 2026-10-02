using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x02000060 RID: 96
	public class MapBarVM : ViewModel
	{
		// Token: 0x0600068B RID: 1675 RVA: 0x000214A9 File Offset: 0x0001F6A9
		protected virtual MapInfoVM CreateInfoVM()
		{
			return new MapInfoVM();
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000214B0 File Offset: 0x0001F6B0
		public void Initialize(INavigationHandler navigationHandler, IMapStateHandler mapStateHandler, Func<MapBarShortcuts> getMapBarShortcuts, Action openArmyManagement)
		{
			this._navigationHandler = navigationHandler;
			this._refreshTimeSpan = ((Campaign.Current.GetSimplifiedTimeControlMode() == CampaignTimeControlMode.UnstoppableFastForward) ? 0.1f : 2f);
			this._openArmyManagement = openArmyManagement;
			this._mapStateHandler = mapStateHandler;
			this.TutorialNotification = new ElementNotificationVM();
			this.MapInfo = this.CreateInfoVM();
			this.MapTimeControl = new MapTimeControlVM(getMapBarShortcuts, new Action(this.OnTimeControlChange), delegate
			{
				mapStateHandler.ResetCamera(false, false);
			});
			this.MapNavigation = new MapNavigationVM(navigationHandler, getMapBarShortcuts);
			this.GatherArmyHint = new HintViewModel();
			this.OnRefresh();
			this.IsEnabled = true;
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002157F File Offset: 0x0001F77F
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.MapInfo.RefreshValues();
			this.MapTimeControl.RefreshValues();
			this.MapNavigation.RefreshValues();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000215A8 File Offset: 0x0001F7A8
		public void OnRefresh()
		{
			this.MapInfo.Refresh();
			this.MapTimeControl.Refresh();
			this.MapNavigation.Refresh();
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x000215CC File Offset: 0x0001F7CC
		public void Tick(float dt)
		{
			int simplifiedTimeControlMode = (int)Campaign.Current.GetSimplifiedTimeControlMode();
			this._refreshTimeSpan -= dt;
			if (this._refreshTimeSpan < 0f)
			{
				this.OnRefresh();
				this._refreshTimeSpan = ((simplifiedTimeControlMode == 2) ? 0.1f : 0.2f);
			}
			this.MapInfo.Tick();
			this.MapTimeControl.Tick();
			this.MapNavigation.Tick();
			if (this._mapStateHandler != null)
			{
				this.IsCameraCentered = this._mapStateHandler.IsCameraLockedToPlayerParty();
			}
			this.IsGatherArmyVisible = this.GetIsGatherArmyVisible();
			if (this.IsGatherArmyVisible)
			{
				this.UpdateCanGatherArmyAndReason();
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00021670 File Offset: 0x0001F870
		private void UpdateCanGatherArmyAndReason()
		{
			TextObject textObject;
			this.CanGatherArmy = Campaign.Current.Models.ArmyManagementCalculationModel.CanPlayerCreateArmy(out textObject);
			this.GatherArmyHint.HintText = textObject;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x000216A8 File Offset: 0x0001F8A8
		private bool GetIsGatherArmyVisible()
		{
			if (this.MapTimeControl.IsInMap)
			{
				MobileParty mainParty = MobileParty.MainParty;
				if (((mainParty != null) ? mainParty.Army : null) == null && !Hero.MainHero.IsPrisoner && Hero.MainHero.PartyBelongedTo != null && MobileParty.MainParty.MapEvent == null)
				{
					return this.MapTimeControl.IsCenterPanelEnabled;
				}
			}
			return false;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00021706 File Offset: 0x0001F906
		private void OnTimeControlChange()
		{
			this._refreshTimeSpan = ((Campaign.Current.GetSimplifiedTimeControlMode() == CampaignTimeControlMode.UnstoppableFastForward) ? 0.1f : 2f);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00021727 File Offset: 0x0001F927
		private void ExecuteResetCamera()
		{
			IMapStateHandler mapStateHandler = this._mapStateHandler;
			if (mapStateHandler == null)
			{
				return;
			}
			mapStateHandler.FastMoveCameraToMainParty();
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00021739 File Offset: 0x0001F939
		public void ExecuteArmyManagement()
		{
			this._openArmyManagement();
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00021748 File Offset: 0x0001F948
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = string.Empty;
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = this._latestTutorialElementID;
					if (this._latestTutorialElementID == "PartySpeedLabel" && !this.MapInfo.IsInfoBarExtended)
					{
						this.MapInfo.IsInfoBarExtended = true;
					}
				}
			}
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000217D0 File Offset: 0x0001F9D0
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._mapStateHandler = null;
			MapNavigationVM mapNavigation = this._mapNavigation;
			if (mapNavigation != null)
			{
				mapNavigation.OnFinalize();
			}
			MapTimeControlVM mapTimeControl = this._mapTimeControl;
			if (mapTimeControl != null)
			{
				mapTimeControl.OnFinalize();
			}
			this._mapInfo = null;
			this._mapNavigation = null;
			this._mapTimeControl = null;
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			EventManager eventManager = game.EventManager;
			if (eventManager == null)
			{
				return;
			}
			eventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x00021846 File Offset: 0x0001FA46
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x0002184E File Offset: 0x0001FA4E
		[DataSourceProperty]
		public MapInfoVM MapInfo
		{
			get
			{
				return this._mapInfo;
			}
			set
			{
				if (value != this._mapInfo)
				{
					this._mapInfo = value;
					base.OnPropertyChangedWithValue<MapInfoVM>(value, "MapInfo");
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0002186C File Offset: 0x0001FA6C
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x00021874 File Offset: 0x0001FA74
		[DataSourceProperty]
		public MapTimeControlVM MapTimeControl
		{
			get
			{
				return this._mapTimeControl;
			}
			set
			{
				if (value != this._mapTimeControl)
				{
					this._mapTimeControl = value;
					base.OnPropertyChangedWithValue<MapTimeControlVM>(value, "MapTimeControl");
				}
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00021892 File Offset: 0x0001FA92
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x0002189A File Offset: 0x0001FA9A
		[DataSourceProperty]
		public MapNavigationVM MapNavigation
		{
			get
			{
				return this._mapNavigation;
			}
			set
			{
				if (value != this._mapNavigation)
				{
					this._mapNavigation = value;
					base.OnPropertyChangedWithValue<MapNavigationVM>(value, "MapNavigation");
				}
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x000218B8 File Offset: 0x0001FAB8
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x000218C0 File Offset: 0x0001FAC0
		[DataSourceProperty]
		public bool IsGatherArmyVisible
		{
			get
			{
				return this._isGatherArmyVisible;
			}
			set
			{
				if (value != this._isGatherArmyVisible)
				{
					this._isGatherArmyVisible = value;
					base.OnPropertyChangedWithValue(value, "IsGatherArmyVisible");
				}
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000218DE File Offset: 0x0001FADE
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x000218E6 File Offset: 0x0001FAE6
		[DataSourceProperty]
		public bool IsInInfoMode
		{
			get
			{
				return this._isInInfoMode;
			}
			set
			{
				if (value != this._isInInfoMode)
				{
					this._isInInfoMode = value;
					base.OnPropertyChangedWithValue(value, "IsInInfoMode");
				}
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00021904 File Offset: 0x0001FB04
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x0002190C File Offset: 0x0001FB0C
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0002192A File Offset: 0x0001FB2A
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x00021932 File Offset: 0x0001FB32
		[DataSourceProperty]
		public bool CanGatherArmy
		{
			get
			{
				return this._canGatherArmy;
			}
			set
			{
				if (value != this._canGatherArmy)
				{
					this._canGatherArmy = value;
					base.OnPropertyChangedWithValue(value, "CanGatherArmy");
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00021950 File Offset: 0x0001FB50
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x00021958 File Offset: 0x0001FB58
		[DataSourceProperty]
		public HintViewModel GatherArmyHint
		{
			get
			{
				return this._gatherArmyHint;
			}
			set
			{
				if (value != this._gatherArmyHint)
				{
					this._gatherArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GatherArmyHint");
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x00021976 File Offset: 0x0001FB76
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x0002197E File Offset: 0x0001FB7E
		[DataSourceProperty]
		public bool IsCameraCentered
		{
			get
			{
				return this._isCameraCentered;
			}
			set
			{
				if (value != this._isCameraCentered)
				{
					this._isCameraCentered = value;
					base.OnPropertyChangedWithValue(value, "IsCameraCentered");
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0002199C File Offset: 0x0001FB9C
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x000219A4 File Offset: 0x0001FBA4
		[DataSourceProperty]
		public string CurrentScreen
		{
			get
			{
				return this._currentScreen;
			}
			set
			{
				if (this._currentScreen != value)
				{
					this._currentScreen = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentScreen");
				}
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x000219C7 File Offset: 0x0001FBC7
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x000219CF File Offset: 0x0001FBCF
		[DataSourceProperty]
		public ElementNotificationVM TutorialNotification
		{
			get
			{
				return this._tutorialNotification;
			}
			set
			{
				if (value != this._tutorialNotification)
				{
					this._tutorialNotification = value;
					base.OnPropertyChangedWithValue<ElementNotificationVM>(value, "TutorialNotification");
				}
			}
		}

		// Token: 0x040002C3 RID: 707
		protected INavigationHandler _navigationHandler;

		// Token: 0x040002C4 RID: 708
		private IMapStateHandler _mapStateHandler;

		// Token: 0x040002C5 RID: 709
		private Action _openArmyManagement;

		// Token: 0x040002C6 RID: 710
		private float _refreshTimeSpan;

		// Token: 0x040002C7 RID: 711
		private string _latestTutorialElementID;

		// Token: 0x040002C8 RID: 712
		private bool _isGatherArmyVisible;

		// Token: 0x040002C9 RID: 713
		private MapInfoVM _mapInfo;

		// Token: 0x040002CA RID: 714
		private MapTimeControlVM _mapTimeControl;

		// Token: 0x040002CB RID: 715
		private MapNavigationVM _mapNavigation;

		// Token: 0x040002CC RID: 716
		private bool _isEnabled;

		// Token: 0x040002CD RID: 717
		private bool _isCameraCentered;

		// Token: 0x040002CE RID: 718
		private bool _canGatherArmy;

		// Token: 0x040002CF RID: 719
		private bool _isInInfoMode;

		// Token: 0x040002D0 RID: 720
		private string _currentScreen;

		// Token: 0x040002D1 RID: 721
		private HintViewModel _gatherArmyHint;

		// Token: 0x040002D2 RID: 722
		private ElementNotificationVM _tutorialNotification;
	}
}
