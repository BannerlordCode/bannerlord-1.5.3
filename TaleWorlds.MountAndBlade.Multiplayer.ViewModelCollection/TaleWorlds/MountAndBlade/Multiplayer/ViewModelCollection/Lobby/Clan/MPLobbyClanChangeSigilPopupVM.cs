using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000068 RID: 104
	public class MPLobbyClanChangeSigilPopupVM : ViewModel
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x0001F2D9 File Offset: 0x0001D4D9
		public MPLobbyClanChangeSigilPopupVM()
		{
			this.PrepareSigilIconsList();
			this.CanChangeSigil = false;
			this.RefreshValues();
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0001F2F4 File Offset: 0x0001D4F4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=q7VcSSbp}Choose Sigil", null).ToString();
			this.ApplyText = new TextObject("{=BAaS5Dkc}Apply", null).ToString();
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0001F328 File Offset: 0x0001D528
		private void PrepareSigilIconsList()
		{
			this.IconsList = new MBBindingList<MPLobbySigilItemVM>();
			this._selectedSigilIcon = null;
			foreach (BannerIconGroup bannerIconGroup in BannerManager.Instance.BannerIconGroups)
			{
				if (!bannerIconGroup.IsPattern)
				{
					foreach (KeyValuePair<int, BannerIconData> keyValuePair in bannerIconGroup.AvailableIcons)
					{
						MPLobbySigilItemVM mplobbySigilItemVM = new MPLobbySigilItemVM(keyValuePair.Key, new Action<MPLobbySigilItemVM>(this.OnSigilIconSelection));
						this.IconsList.Add(mplobbySigilItemVM);
					}
				}
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0001F3F4 File Offset: 0x0001D5F4
		private void OnSigilIconSelection(MPLobbySigilItemVM sigilIcon)
		{
			if (sigilIcon != this._selectedSigilIcon)
			{
				if (this._selectedSigilIcon != null)
				{
					this._selectedSigilIcon.IsSelected = false;
				}
				this._selectedSigilIcon = sigilIcon;
				if (this._selectedSigilIcon != null)
				{
					this._selectedSigilIcon.IsSelected = true;
					this.CanChangeSigil = true;
				}
			}
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0001F440 File Offset: 0x0001D640
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0001F449 File Offset: 0x0001D649
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0001F454 File Offset: 0x0001D654
		public async void ExecuteChangeSigil()
		{
			BasicCultureObject @object = Game.Current.ObjectManager.GetObject<BasicCultureObject>(NetworkMain.GameClient.ClanInfo.Faction);
			Banner banner = new Banner(@object.Banner, @object.BackgroundColor1, @object.ForegroundColor1);
			banner.SetIconMeshId(this._selectedSigilIcon.IconID);
			TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.ChangeClanSigil(banner.Serialize()).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.ExecuteClosePopup();
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0001F48D File Offset: 0x0001D68D
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0001F4B6 File Offset: 0x0001D6B6
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0001F4C5 File Offset: 0x0001D6C5
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0001F4D4 File Offset: 0x0001D6D4
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x0001F4DC File Offset: 0x0001D6DC
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
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0001F4F9 File Offset: 0x0001D6F9
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0001F501 File Offset: 0x0001D701
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChanged("DoneInputKey");
				}
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0001F51E File Offset: 0x0001D71E
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0001F526 File Offset: 0x0001D726
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0001F543 File Offset: 0x0001D743
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x0001F54B File Offset: 0x0001D74B
		[DataSourceProperty]
		public bool CanChangeSigil
		{
			get
			{
				return this._canChangeSigil;
			}
			set
			{
				if (value != this._canChangeSigil)
				{
					this._canChangeSigil = value;
					base.OnPropertyChanged("CanChangeSigil");
				}
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0001F568 File Offset: 0x0001D768
		// (set) Token: 0x06000A19 RID: 2585 RVA: 0x0001F570 File Offset: 0x0001D770
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0001F592 File Offset: 0x0001D792
		// (set) Token: 0x06000A1B RID: 2587 RVA: 0x0001F59A File Offset: 0x0001D79A
		[DataSourceProperty]
		public string ApplyText
		{
			get
			{
				return this._applyText;
			}
			set
			{
				if (value != this._applyText)
				{
					this._applyText = value;
					base.OnPropertyChanged("ApplyText");
				}
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0001F5BC File Offset: 0x0001D7BC
		// (set) Token: 0x06000A1D RID: 2589 RVA: 0x0001F5C4 File Offset: 0x0001D7C4
		[DataSourceProperty]
		public MBBindingList<MPLobbySigilItemVM> IconsList
		{
			get
			{
				return this._iconsList;
			}
			set
			{
				if (value != this._iconsList)
				{
					this._iconsList = value;
					base.OnPropertyChanged("IconsList");
				}
			}
		}

		// Token: 0x0400049A RID: 1178
		private MPLobbySigilItemVM _selectedSigilIcon;

		// Token: 0x0400049B RID: 1179
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400049C RID: 1180
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400049D RID: 1181
		private bool _isSelected;

		// Token: 0x0400049E RID: 1182
		private bool _canChangeSigil;

		// Token: 0x0400049F RID: 1183
		private string _titleText;

		// Token: 0x040004A0 RID: 1184
		private string _applyText;

		// Token: 0x040004A1 RID: 1185
		private MBBindingList<MPLobbySigilItemVM> _iconsList;
	}
}
