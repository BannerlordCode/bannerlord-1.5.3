using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.ProfileSelection
{
	// Token: 0x02000017 RID: 23
	public class ProfileSelectionVM : ViewModel
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x000069D8 File Offset: 0x00004BD8
		public ProfileSelectionVM(bool isDirectPlayPossible)
		{
			this.SelectProfileText = new TextObject("{=wubDWOlh}Select Profile", null).ToString();
			this.SelectProfileKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("SelectProfile"), false);
			this.PlayKey = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Play"), false);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006A44 File Offset: 0x00004C44
		public void OnActivate(bool isDirectPlayPossible)
		{
			this.IsPlayEnabled = isDirectPlayPossible;
			if (!string.IsNullOrEmpty(PlatformServices.Instance.UserDisplayName))
			{
				this.PlayText = new TextObject("{=FTXx0aRp}Play as", null).ToString() + PlatformServices.Instance.UserDisplayName;
				return;
			}
			this.PlayText = new TextObject("{=playgame}Play", null).ToString();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00006AA5 File Offset: 0x00004CA5
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.SelectProfileKey.OnFinalize();
			this.PlayKey.OnFinalize();
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00006AC3 File Offset: 0x00004CC3
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00006ACB File Offset: 0x00004CCB
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

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00006AEE File Offset: 0x00004CEE
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00006AF6 File Offset: 0x00004CF6
		[DataSourceProperty]
		public bool IsPlayEnabled
		{
			get
			{
				return this._isPlayEnabled;
			}
			set
			{
				if (value != this._isPlayEnabled)
				{
					this._isPlayEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPlayEnabled");
				}
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00006B14 File Offset: 0x00004D14
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00006B1C File Offset: 0x00004D1C
		[DataSourceProperty]
		public string PlayText
		{
			get
			{
				return this._playText;
			}
			set
			{
				if (value != this._playText)
				{
					this._playText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayText");
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00006B3F File Offset: 0x00004D3F
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00006B47 File Offset: 0x00004D47
		[DataSourceProperty]
		public InputKeyItemVM SelectProfileKey
		{
			get
			{
				return this._selectProfileKey;
			}
			set
			{
				if (value != this._selectProfileKey)
				{
					this._selectProfileKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "SelectProfileKey");
				}
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00006B65 File Offset: 0x00004D65
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00006B6D File Offset: 0x00004D6D
		[DataSourceProperty]
		public InputKeyItemVM PlayKey
		{
			get
			{
				return this._playKey;
			}
			set
			{
				if (value != this._playKey)
				{
					this._playKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PlayKey");
				}
			}
		}

		// Token: 0x040000D4 RID: 212
		private bool _isPlayEnabled;

		// Token: 0x040000D5 RID: 213
		private string _selectProfileText;

		// Token: 0x040000D6 RID: 214
		private string _playText;

		// Token: 0x040000D7 RID: 215
		private InputKeyItemVM _playKey;

		// Token: 0x040000D8 RID: 216
		private InputKeyItemVM _selectProfileKey;
	}
}
