using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x02000065 RID: 101
	public class MapTimeControlVM : ViewModel
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x00022978 File Offset: 0x00020B78
		// (set) Token: 0x060006FC RID: 1788 RVA: 0x00022980 File Offset: 0x00020B80
		public bool IsInBattleSimulation { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00022989 File Offset: 0x00020B89
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x00022991 File Offset: 0x00020B91
		public bool IsInRecruitment { get; set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0002299A File Offset: 0x00020B9A
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x000229A2 File Offset: 0x00020BA2
		public bool IsEncyclopediaOpen { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x000229AB File Offset: 0x00020BAB
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x000229B3 File Offset: 0x00020BB3
		public bool IsInArmyManagement { get; set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x000229BC File Offset: 0x00020BBC
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x000229C4 File Offset: 0x00020BC4
		public bool IsInTownManagement { get; set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x000229CD File Offset: 0x00020BCD
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x000229D5 File Offset: 0x00020BD5
		public bool IsInHideoutTroopManage { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x000229DE File Offset: 0x00020BDE
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x000229E6 File Offset: 0x00020BE6
		public bool IsInMap { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x000229EF File Offset: 0x00020BEF
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x000229F7 File Offset: 0x00020BF7
		public bool IsInCampaignOptions { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00022A00 File Offset: 0x00020C00
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x00022A08 File Offset: 0x00020C08
		public bool IsEscapeMenuOpened { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00022A11 File Offset: 0x00020C11
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x00022A19 File Offset: 0x00020C19
		public bool IsMarriageOfferPopupActive { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x00022A22 File Offset: 0x00020C22
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x00022A2A File Offset: 0x00020C2A
		public bool IsHeirSelectionPopupActive { get; set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00022A33 File Offset: 0x00020C33
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x00022A3B File Offset: 0x00020C3B
		public bool IsMapCheatsActive { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00022A44 File Offset: 0x00020C44
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x00022A4C File Offset: 0x00020C4C
		public bool IsMapIncidentActive { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00022A55 File Offset: 0x00020C55
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x00022A5D File Offset: 0x00020C5D
		public bool IsOverlayContextMenuEnabled { get; set; }

		// Token: 0x06000717 RID: 1815 RVA: 0x00022A68 File Offset: 0x00020C68
		public MapTimeControlVM(Func<MapBarShortcuts> getMapBarShortcuts, Action onTimeFlowStateChange, Action onCameraResetted)
		{
			this._onTimeFlowStateChange = onTimeFlowStateChange;
			this._getMapBarShortcuts = getMapBarShortcuts;
			this._onCameraReset = onCameraResetted;
			this.IsCenterPanelEnabled = false;
			this._lastSetDate = CampaignTime.Zero;
			this.PlayHint = new BasicTooltipViewModel();
			this.FastForwardHint = new BasicTooltipViewModel();
			this.PauseHint = new BasicTooltipViewModel();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			CampaignEvents.OnSaveStartedEvent.AddNonSerializedListener(this, new Action(this.OnSaveStarted));
			CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(this, new Action<bool, string>(this.OnSaveOver));
			this.RefreshValues();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00022B20 File Offset: 0x00020D20
		public override void RefreshValues()
		{
			base.RefreshValues();
			this._shortcuts = this._getMapBarShortcuts();
			if (Input.IsGamepadActive)
			{
				this.PlayHint.SetHintCallback(() => GameTexts.FindText("str_play", null).ToString());
				this.FastForwardHint.SetHintCallback(() => GameTexts.FindText("str_fast_forward", null).ToString());
				this.PauseHint.SetHintCallback(() => GameTexts.FindText("str_pause", null).ToString());
			}
			else
			{
				this.PlayHint.SetHintCallback(delegate
				{
					GameTexts.SetVariable("TEXT", GameTexts.FindText("str_play", null).ToString());
					GameTexts.SetVariable("HOTKEY", this._shortcuts.PlayHotkey);
					return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
				});
				this.FastForwardHint.SetHintCallback(delegate
				{
					GameTexts.SetVariable("TEXT", GameTexts.FindText("str_fast_forward", null).ToString());
					GameTexts.SetVariable("HOTKEY", this._shortcuts.FastForwardHotkey);
					return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
				});
				this.PauseHint.SetHintCallback(delegate
				{
					GameTexts.SetVariable("TEXT", GameTexts.FindText("str_pause", null).ToString());
					GameTexts.SetVariable("HOTKEY", this._shortcuts.PauseHotkey);
					return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
				});
			}
			this.RefreshPausedText();
			this.Date = CampaignTime.Now.ToString();
			this._lastSetDate = CampaignTime.Now;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00022C40 File Offset: 0x00020E40
		private void RefreshPausedText()
		{
			MobileParty mainParty = MobileParty.MainParty;
			if (mainParty == null)
			{
				Debug.FailedAssert("Main party is null when refreshing pause text", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Map\\MapBar\\MapTimeControlVM.cs", "RefreshPausedText", 107);
				this.PausedText = GameTexts.FindText("str_paused_capital", null).ToString();
				return;
			}
			if (this.IsCurrentlyPausedOnMap)
			{
				this.PausedText = GameTexts.FindText("str_paused_capital", null).ToString();
				return;
			}
			if (!MobileParty.MainParty.IsTransitionInProgress)
			{
				this.PausedText = string.Empty;
				return;
			}
			if (mainParty.IsCurrentlyAtSea)
			{
				this.PausedText = new TextObject("{=g1op0Thi}DISEMBARKING", null).ToString();
				return;
			}
			this.PausedText = new TextObject("{=Lt0PzKHN}EMBARKING", null).ToString();
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00022CF0 File Offset: 0x00020EF0
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			this._onTimeFlowStateChange = null;
			this._getMapBarShortcuts = null;
			this._onCameraReset = null;
			CampaignEvents.OnSaveStartedEvent.ClearListeners(this);
			CampaignEvents.OnSaveOverEvent.ClearListeners(this);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00022D4E File Offset: 0x00020F4E
		private void OnGamepadActiveStateChanged()
		{
			this.RefreshValues();
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00022D56 File Offset: 0x00020F56
		private void OnSaveStarted()
		{
			this._isSaving = true;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00022D5F File Offset: 0x00020F5F
		private void OnSaveOver(bool wasSuccessful, string saveName)
		{
			this._isSaving = false;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00022D68 File Offset: 0x00020F68
		public void Tick()
		{
			this.TimeFlowState = (int)Campaign.Current.GetSimplifiedTimeControlMode();
			this.IsCurrentlyPausedOnMap = (this.TimeFlowState == 0 || this.TimeFlowState == 6) && this.IsCenterPanelEnabled && !this.IsEscapeMenuOpened && !this._isSaving;
			this.IsCenterPanelEnabled = !this.IsInBattleSimulation && !this.IsInRecruitment && !this.IsEncyclopediaOpen && !this.IsInTownManagement && !this.IsInArmyManagement && this.IsInMap && !this.IsInCampaignOptions && !this.IsInHideoutTroopManage && !this.IsMarriageOfferPopupActive && !this.IsHeirSelectionPopupActive && !this.IsMapCheatsActive && !this.IsMapIncidentActive && !this.IsOverlayContextMenuEnabled;
			if (MobileParty.MainParty.IsTransitionInProgress != this._mainPartyPreviousTransitioning)
			{
				this._mainPartyPreviousTransitioning = MobileParty.MainParty.IsTransitionInProgress;
				this.RefreshPausedText();
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00022E54 File Offset: 0x00021054
		public void Refresh()
		{
			if (!this._lastSetDate.StringSameAs(CampaignTime.Now))
			{
				this.Date = CampaignTime.Now.ToString();
				this._lastSetDate = CampaignTime.Now;
			}
			this.Time = CampaignTime.Now.ToHours % (double)CampaignTime.HoursInDay;
			this.TimeOfDayHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetTimeOfDayAndResetCameraTooltip());
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00022EDB File Offset: 0x000210DB
		private void SetTimeSpeed(int speed)
		{
			Campaign.Current.SetTimeSpeed(speed);
			this._onTimeFlowStateChange();
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00022EF4 File Offset: 0x000210F4
		public void ExecuteTimeControlChange(int selectedTimeSpeed)
		{
			if (Campaign.Current.CurrentMenuContext == null || (Campaign.Current.CurrentMenuContext.GameMenu.IsWaitActive && !Campaign.Current.TimeControlModeLock))
			{
				int num = selectedTimeSpeed;
				if (this._timeFlowState == 3 && num == 2)
				{
					num = 4;
				}
				else if (this._timeFlowState == 4 && num == 1)
				{
					num = 3;
				}
				else if (this._timeFlowState == 2 && num == 0)
				{
					num = 6;
				}
				if (num != this._timeFlowState)
				{
					this.TimeFlowState = num;
					this.SetTimeSpeed(selectedTimeSpeed);
				}
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00022F78 File Offset: 0x00021178
		public void ExecuteResetCamera()
		{
			this._onCameraReset();
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00022F85 File Offset: 0x00021185
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x00022F8D File Offset: 0x0002118D
		[DataSourceProperty]
		public BasicTooltipViewModel TimeOfDayHint
		{
			get
			{
				return this._timeOfDayHint;
			}
			set
			{
				if (value != this._timeOfDayHint)
				{
					this._timeOfDayHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TimeOfDayHint");
				}
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00022FAB File Offset: 0x000211AB
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x00022FB3 File Offset: 0x000211B3
		[DataSourceProperty]
		public bool IsCurrentlyPausedOnMap
		{
			get
			{
				return this._isCurrentlyPausedOnMap;
			}
			set
			{
				if (value != this._isCurrentlyPausedOnMap)
				{
					this._isCurrentlyPausedOnMap = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentlyPausedOnMap");
					this.RefreshPausedText();
				}
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00022FD7 File Offset: 0x000211D7
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x00022FDF File Offset: 0x000211DF
		[DataSourceProperty]
		public bool IsCenterPanelEnabled
		{
			get
			{
				return this._isCenterPanelEnabled;
			}
			set
			{
				if (value != this._isCenterPanelEnabled)
				{
					this._isCenterPanelEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCenterPanelEnabled");
				}
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00022FFD File Offset: 0x000211FD
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x00023005 File Offset: 0x00021205
		[DataSourceProperty]
		public double Time
		{
			get
			{
				return this._time;
			}
			set
			{
				if (this._time != value)
				{
					this._time = value;
					base.OnPropertyChangedWithValue(value, "Time");
				}
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00023023 File Offset: 0x00021223
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0002302B File Offset: 0x0002122B
		[DataSourceProperty]
		public string PausedText
		{
			get
			{
				return this._pausedText;
			}
			set
			{
				if (this._pausedText != value)
				{
					this._pausedText = value;
					base.OnPropertyChangedWithValue<string>(value, "PausedText");
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0002304E File Offset: 0x0002124E
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00023056 File Offset: 0x00021256
		[DataSourceProperty]
		public string Date
		{
			get
			{
				return this._date;
			}
			set
			{
				if (value != this._date)
				{
					this._date = value;
					base.OnPropertyChangedWithValue<string>(value, "Date");
				}
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00023079 File Offset: 0x00021279
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00023081 File Offset: 0x00021281
		[DataSourceProperty]
		public int TimeFlowState
		{
			get
			{
				return this._timeFlowState;
			}
			set
			{
				if (value != this._timeFlowState)
				{
					this._timeFlowState = value;
					base.OnPropertyChangedWithValue(value, "TimeFlowState");
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0002309F File Offset: 0x0002129F
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x000230A7 File Offset: 0x000212A7
		[DataSourceProperty]
		public BasicTooltipViewModel PauseHint
		{
			get
			{
				return this._pauseHint;
			}
			set
			{
				if (value != this._pauseHint)
				{
					this._pauseHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PauseHint");
				}
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x000230C5 File Offset: 0x000212C5
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x000230CD File Offset: 0x000212CD
		[DataSourceProperty]
		public BasicTooltipViewModel PlayHint
		{
			get
			{
				return this._playHint;
			}
			set
			{
				if (value != this._playHint)
				{
					this._playHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PlayHint");
				}
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x000230EB File Offset: 0x000212EB
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x000230F3 File Offset: 0x000212F3
		[DataSourceProperty]
		public BasicTooltipViewModel FastForwardHint
		{
			get
			{
				return this._fastForwardHint;
			}
			set
			{
				if (value != this._fastForwardHint)
				{
					this._fastForwardHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "FastForwardHint");
				}
			}
		}

		// Token: 0x0400030B RID: 779
		private bool _mainPartyPreviousTransitioning;

		// Token: 0x0400030C RID: 780
		private Action _onTimeFlowStateChange;

		// Token: 0x0400030D RID: 781
		private Func<MapBarShortcuts> _getMapBarShortcuts;

		// Token: 0x0400030E RID: 782
		private MapBarShortcuts _shortcuts;

		// Token: 0x0400030F RID: 783
		private Action _onCameraReset;

		// Token: 0x04000310 RID: 784
		private CampaignTime _lastSetDate;

		// Token: 0x04000311 RID: 785
		private bool _isSaving;

		// Token: 0x04000312 RID: 786
		private int _timeFlowState = -1;

		// Token: 0x04000313 RID: 787
		private double _time;

		// Token: 0x04000314 RID: 788
		private string _date;

		// Token: 0x04000315 RID: 789
		private string _pausedText;

		// Token: 0x04000316 RID: 790
		private bool _isCurrentlyPausedOnMap;

		// Token: 0x04000317 RID: 791
		private bool _isCenterPanelEnabled;

		// Token: 0x04000318 RID: 792
		private BasicTooltipViewModel _pauseHint;

		// Token: 0x04000319 RID: 793
		private BasicTooltipViewModel _playHint;

		// Token: 0x0400031A RID: 794
		private BasicTooltipViewModel _fastForwardHint;

		// Token: 0x0400031B RID: 795
		private BasicTooltipViewModel _timeOfDayHint;
	}
}
