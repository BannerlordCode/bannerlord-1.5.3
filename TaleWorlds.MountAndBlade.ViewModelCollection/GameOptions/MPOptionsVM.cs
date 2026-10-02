using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000070 RID: 112
	public class MPOptionsVM : OptionsVM
	{
		// Token: 0x060008C3 RID: 2243 RVA: 0x0001D4D1 File Offset: 0x0001B6D1
		public MPOptionsVM(bool autoHandleClose, Action onChangeBrightnessRequest, Action onChangeExposureRequest, Action<KeyOptionVM> onKeybindRequest)
			: base(autoHandleClose, OptionsVM.OptionsMode.Multiplayer, onKeybindRequest, onChangeBrightnessRequest, onChangeExposureRequest)
		{
			this.RefreshValues();
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001D507 File Offset: 0x0001B707
		public MPOptionsVM(Action onClose, Action<KeyOptionVM> onKeybindRequest)
			: base(OptionsVM.OptionsMode.Multiplayer, onClose, onKeybindRequest, null, null)
		{
			this.RefreshValues();
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001D53C File Offset: 0x0001B73C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ApplyText = new TextObject("{=BAaS5Dkc}Apply", null).ToString();
			this.RevertText = new TextObject("{=Npqlj5Ln}Revert Changes", null).ToString();
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001D570 File Offset: 0x0001B770
		public new void ExecuteCancel()
		{
			base.ExecuteCancel();
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001D578 File Offset: 0x0001B778
		public void ExecuteApply()
		{
			bool flag = base.IsOptionsChanged();
			base.OnDone();
			InformationManager.DisplayMessage(new InformationMessage(flag ? this._changesAppliedTextObject.ToString() : this._noChangesMadeTextObject.ToString()));
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001D5AA File Offset: 0x0001B7AA
		public void ForceCancel()
		{
			base.HandleCancel(false);
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001D5B3 File Offset: 0x0001B7B3
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x0001D5BB File Offset: 0x0001B7BB
		[DataSourceProperty]
		public bool AreHotkeysEnabled
		{
			get
			{
				return this._areHotkeysEnabled;
			}
			set
			{
				if (value != this._areHotkeysEnabled)
				{
					this._areHotkeysEnabled = value;
					base.OnPropertyChangedWithValue(value, "AreHotkeysEnabled");
				}
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0001D5D9 File Offset: 0x0001B7D9
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x0001D5E1 File Offset: 0x0001B7E1
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

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0001D5FF File Offset: 0x0001B7FF
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x0001D607 File Offset: 0x0001B807
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
					base.OnPropertyChangedWithValue<string>(value, "ApplyText");
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0001D62A File Offset: 0x0001B82A
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0001D632 File Offset: 0x0001B832
		[DataSourceProperty]
		public string RevertText
		{
			get
			{
				return this._revertText;
			}
			set
			{
				if (value != this._revertText)
				{
					this._revertText = value;
					base.OnPropertyChangedWithValue<string>(value, "RevertText");
				}
			}
		}

		// Token: 0x040003F2 RID: 1010
		private TextObject _changesAppliedTextObject = new TextObject("{=SfsnlbyK}Changes applied.", null);

		// Token: 0x040003F3 RID: 1011
		private TextObject _noChangesMadeTextObject = new TextObject("{=jS5rrX8M}There are no changes to apply.", null);

		// Token: 0x040003F4 RID: 1012
		private bool _areHotkeysEnabled;

		// Token: 0x040003F5 RID: 1013
		private bool _isEnabled;

		// Token: 0x040003F6 RID: 1014
		private string _applyText;

		// Token: 0x040003F7 RID: 1015
		private string _revertText;
	}
}
