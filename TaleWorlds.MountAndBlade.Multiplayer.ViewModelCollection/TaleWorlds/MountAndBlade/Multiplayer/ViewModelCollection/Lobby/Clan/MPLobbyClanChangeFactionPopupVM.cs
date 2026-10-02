using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000067 RID: 103
	public class MPLobbyClanChangeFactionPopupVM : ViewModel
	{
		// Token: 0x060009EE RID: 2542 RVA: 0x0001EF5B File Offset: 0x0001D15B
		public MPLobbyClanChangeFactionPopupVM()
		{
			this.PrepareFactionsList();
			this.CanChangeFaction = false;
			this.RefreshValues();
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0001EF76 File Offset: 0x0001D176
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=ghjSIyIL}Choose Culture", null).ToString();
			this.ApplyText = new TextObject("{=BAaS5Dkc}Apply", null).ToString();
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0001EFAC File Offset: 0x0001D1AC
		private void PrepareFactionsList()
		{
			this._selectedFaction = null;
			this.FactionsList = new MBBindingList<MPCultureItemVM>
			{
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("vlandia").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("sturgia").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("empire").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("battania").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("khuzait").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection)),
				new MPCultureItemVM(Game.Current.ObjectManager.GetObject<BasicCultureObject>("aserai").StringId, new Action<MPCultureItemVM>(this.OnFactionSelection))
			};
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0001F0EC File Offset: 0x0001D2EC
		private void OnFactionSelection(MPCultureItemVM faction)
		{
			if (faction != this._selectedFaction)
			{
				if (this._selectedFaction != null)
				{
					this._selectedFaction.IsSelected = false;
				}
				this._selectedFaction = faction;
				if (this._selectedFaction != null)
				{
					this._selectedFaction.IsSelected = true;
					this.CanChangeFaction = true;
				}
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0001F138 File Offset: 0x0001D338
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001F141 File Offset: 0x0001D341
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0001F14C File Offset: 0x0001D34C
		public async void ExecuteChangeFaction()
		{
			BasicCultureObject @object = Game.Current.ObjectManager.GetObject<BasicCultureObject>(this._selectedFaction.CultureCode);
			Banner banner = new Banner(NetworkMain.GameClient.ClanInfo.Sigil);
			banner.ChangeIconColors(@object.ForegroundColor1);
			banner.ChangePrimaryColor(@object.BackgroundColor1);
			bool flag = await NetworkMain.GameClient.ChangeClanSigil(banner.Serialize());
			bool sigilSuccess = flag;
			bool flag2 = await NetworkMain.GameClient.ChangeClanFaction(this._selectedFaction.CultureCode);
			if (sigilSuccess && flag2)
			{
				this.ExecuteClosePopup();
			}
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001F185 File Offset: 0x0001D385
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

		// Token: 0x060009F6 RID: 2550 RVA: 0x0001F1AE File Offset: 0x0001D3AE
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0001F1BD File Offset: 0x0001D3BD
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0001F1CC File Offset: 0x0001D3CC
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x0001F1D4 File Offset: 0x0001D3D4
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

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x0001F1F1 File Offset: 0x0001D3F1
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x0001F1F9 File Offset: 0x0001D3F9
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

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x0001F216 File Offset: 0x0001D416
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x0001F21E File Offset: 0x0001D41E
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

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0001F23B File Offset: 0x0001D43B
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x0001F243 File Offset: 0x0001D443
		[DataSourceProperty]
		public bool CanChangeFaction
		{
			get
			{
				return this._canChangeFaction;
			}
			set
			{
				if (value != this._canChangeFaction)
				{
					this._canChangeFaction = value;
					base.OnPropertyChanged("CanChangeFaction");
				}
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0001F260 File Offset: 0x0001D460
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x0001F268 File Offset: 0x0001D468
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

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0001F28A File Offset: 0x0001D48A
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x0001F292 File Offset: 0x0001D492
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

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0001F2B4 File Offset: 0x0001D4B4
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0001F2BC File Offset: 0x0001D4BC
		[DataSourceProperty]
		public MBBindingList<MPCultureItemVM> FactionsList
		{
			get
			{
				return this._factionsList;
			}
			set
			{
				if (value != this._factionsList)
				{
					this._factionsList = value;
					base.OnPropertyChanged("FactionsList");
				}
			}
		}

		// Token: 0x04000492 RID: 1170
		private MPCultureItemVM _selectedFaction;

		// Token: 0x04000493 RID: 1171
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000494 RID: 1172
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000495 RID: 1173
		private bool _isSelected;

		// Token: 0x04000496 RID: 1174
		private bool _canChangeFaction;

		// Token: 0x04000497 RID: 1175
		private string _titleText;

		// Token: 0x04000498 RID: 1176
		private string _applyText;

		// Token: 0x04000499 RID: 1177
		private MBBindingList<MPCultureItemVM> _factionsList;
	}
}
