using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu
{
	// Token: 0x0200004C RID: 76
	public class InitialMenuVM : ViewModel
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x00017200 File Offset: 0x00015400
		public InitialMenuVM(InitialState initialState)
		{
			this.MenuOptions = new MBBindingList<InitialMenuOptionVM>();
			this.Announcement = new InitialMenuAnnouncementVM();
			if (HotKeyManager.ShouldNotifyDocumentVersionDifferent())
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=0Itt3bZM}Current keybind document version is outdated. Keybinds have been reverted to defaults.", null), 0, null, null, "");
			}
			this.RefreshValues();
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00017250 File Offset: 0x00015450
		public override void RefreshValues()
		{
			base.RefreshValues();
			MBBindingList<InitialMenuOptionVM> menuOptions = this.MenuOptions;
			if (menuOptions != null)
			{
				menuOptions.ApplyActionOnAllItems(delegate(InitialMenuOptionVM o)
				{
					o.RefreshValues();
				});
			}
			this.Announcement.Refresh();
			this.SelectProfileText = new TextObject("{=wubDWOlh}Select Profile", null).ToString();
			this.DownloadingText = new TextObject("{=i4Oo6aoM}Downloading Content...", null).ToString();
			this.CurrentLanguageString = BannerlordConfig.Language;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000172D5 File Offset: 0x000154D5
		public void Tick()
		{
			InitialMenuAnnouncementVM announcement = this.Announcement;
			if (announcement == null)
			{
				return;
			}
			announcement.Tick();
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000172E8 File Offset: 0x000154E8
		public void RefreshMenuOptions()
		{
			this.MenuOptions.ApplyActionOnAllItems(delegate(InitialMenuOptionVM x)
			{
				x.OnFinalize();
			});
			this.MenuOptions.Clear();
			GameState activeState = GameStateManager.Current.ActiveState;
			foreach (InitialStateOption initialStateOption in Module.CurrentModule.GetInitialStateOptions())
			{
				this.MenuOptions.Add(new InitialMenuOptionVM(initialStateOption));
			}
			this.IsDownloadingContent = Utilities.IsOnlyCoreContentEnabled();
			this.IsNavalDLCEnabled = ModuleHelper.IsModuleActive("NavalDLC");
			this.Announcement.Refresh();
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000173AC File Offset: 0x000155AC
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.MenuOptions.ApplyActionOnAllItems(delegate(InitialMenuOptionVM x)
			{
				x.OnFinalize();
			});
			this.MenuOptions.Clear();
			this.Announcement.OnFinalize();
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x000173FF File Offset: 0x000155FF
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x00017407 File Offset: 0x00015607
		[DataSourceProperty]
		public MBBindingList<InitialMenuOptionVM> MenuOptions
		{
			get
			{
				return this._menuOptions;
			}
			set
			{
				if (value != this._menuOptions)
				{
					this._menuOptions = value;
					base.OnPropertyChangedWithValue<MBBindingList<InitialMenuOptionVM>>(value, "MenuOptions");
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00017425 File Offset: 0x00015625
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x0001742D File Offset: 0x0001562D
		[DataSourceProperty]
		public InitialMenuAnnouncementVM Announcement
		{
			get
			{
				return this._announcement;
			}
			set
			{
				if (value != this._announcement)
				{
					this._announcement = value;
					base.OnPropertyChangedWithValue<InitialMenuAnnouncementVM>(value, "Announcement");
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001744B File Offset: 0x0001564B
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x00017453 File Offset: 0x00015653
		[DataSourceProperty]
		public string DownloadingText
		{
			get
			{
				return this._downloadingText;
			}
			set
			{
				if (value != this._downloadingText)
				{
					this._downloadingText = value;
					base.OnPropertyChangedWithValue<string>(value, "DownloadingText");
				}
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00017476 File Offset: 0x00015676
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x0001747E File Offset: 0x0001567E
		[DataSourceProperty]
		public string SelectProfileText
		{
			get
			{
				return this._selectProfileText;
			}
			set
			{
				if (value != this._selectProfileText)
				{
					this._selectProfileText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectProfileText");
				}
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x000174A1 File Offset: 0x000156A1
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x000174A9 File Offset: 0x000156A9
		[DataSourceProperty]
		public string ProfileName
		{
			get
			{
				return this._profileName;
			}
			set
			{
				if (value != this._profileName)
				{
					this._profileName = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfileName");
				}
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x000174CC File Offset: 0x000156CC
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x000174D4 File Offset: 0x000156D4
		[DataSourceProperty]
		public bool IsProfileSelectionEnabled
		{
			get
			{
				return this._isProfileSelectionEnabled;
			}
			set
			{
				if (value != this._isProfileSelectionEnabled)
				{
					this._isProfileSelectionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsProfileSelectionEnabled");
				}
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x000174F2 File Offset: 0x000156F2
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x000174FA File Offset: 0x000156FA
		[DataSourceProperty]
		public bool IsDownloadingContent
		{
			get
			{
				return this._isDownloadingContent;
			}
			set
			{
				if (value != this._isDownloadingContent)
				{
					this._isDownloadingContent = value;
					base.OnPropertyChangedWithValue(value, "IsDownloadingContent");
				}
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00017518 File Offset: 0x00015718
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x00017520 File Offset: 0x00015720
		[DataSourceProperty]
		public bool IsNavalDLCEnabled
		{
			get
			{
				return this._isNavalDLCEnabled;
			}
			set
			{
				if (value != this._isNavalDLCEnabled)
				{
					this._isNavalDLCEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNavalDLCEnabled");
				}
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0001753E File Offset: 0x0001573E
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x00017546 File Offset: 0x00015746
		[DataSourceProperty]
		public string CurrentLanguageString
		{
			get
			{
				return this._currentLanguageString;
			}
			set
			{
				if (value != this._currentLanguageString)
				{
					this._currentLanguageString = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentLanguageString");
				}
			}
		}

		// Token: 0x040002CD RID: 717
		private MBBindingList<InitialMenuOptionVM> _menuOptions;

		// Token: 0x040002CE RID: 718
		private InitialMenuAnnouncementVM _announcement;

		// Token: 0x040002CF RID: 719
		private bool _isProfileSelectionEnabled;

		// Token: 0x040002D0 RID: 720
		private bool _isDownloadingContent;

		// Token: 0x040002D1 RID: 721
		private bool _isNavalDLCEnabled;

		// Token: 0x040002D2 RID: 722
		private string _selectProfileText;

		// Token: 0x040002D3 RID: 723
		private string _profileName;

		// Token: 0x040002D4 RID: 724
		private string _downloadingText;

		// Token: 0x040002D5 RID: 725
		private string _currentLanguageString;
	}
}
