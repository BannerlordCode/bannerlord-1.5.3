using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Input
{
	// Token: 0x020000A0 RID: 160
	public class InputKeyItemVM : ViewModel
	{
		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0003F38C File Offset: 0x0003D58C
		// (set) Token: 0x06000F22 RID: 3874 RVA: 0x0003F394 File Offset: 0x0003D594
		public GameKey GameKey { get; private set; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0003F39D File Offset: 0x0003D59D
		// (set) Token: 0x06000F24 RID: 3876 RVA: 0x0003F3A5 File Offset: 0x0003D5A5
		public HotKey HotKey { get; private set; }

		// Token: 0x06000F25 RID: 3877 RVA: 0x0003F3AE File Offset: 0x0003D5AE
		private InputKeyItemVM()
		{
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged += this.OnKeybindsChanged;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0003F3E7 File Offset: 0x0003D5E7
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged -= this.OnKeybindsChanged;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0003F420 File Offset: 0x0003D620
		private void OnGamepadActiveStateChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0003F428 File Offset: 0x0003D628
		private void OnKeybindsChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0003F430 File Offset: 0x0003D630
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ForceRefresh();
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0003F43E File Offset: 0x0003D63E
		public void SetForcedVisibility(bool? isVisible)
		{
			this._forcedVisibility = isVisible;
			this.UpdateVisibility();
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0003F450 File Offset: 0x0003D650
		private void ForceRefresh()
		{
			this.UpdateVisibility();
			this.KeyID = string.Empty;
			this.KeyName = string.Empty;
			if (this._forcedID != null)
			{
				this.KeyID = this._forcedID;
				TextObject forcedName = this._forcedName;
				this.KeyName = ((forcedName != null) ? forcedName.ToString() : null) ?? string.Empty;
				return;
			}
			this.KeyID = this.GetKeyId();
			this.KeyName = this.GetKeyName().ToString();
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0003F4CC File Offset: 0x0003D6CC
		private string GetKeyId()
		{
			if (Input.IsGamepadActive)
			{
				if (this.GameKey != null)
				{
					Key controllerKey = this.GameKey.ControllerKey;
					if (controllerKey == null)
					{
						return null;
					}
					return controllerKey.InputKey.ToString();
				}
				else if (this.HotKey != null)
				{
					Key key = this.HotKey.Keys.Find((Key k) => k.IsControllerInput);
					if (key == null)
					{
						return null;
					}
					return key.InputKey.ToString();
				}
			}
			if (this.GameKey != null)
			{
				Key keyboardKey = this.GameKey.KeyboardKey;
				if (keyboardKey == null)
				{
					return null;
				}
				return keyboardKey.InputKey.ToString();
			}
			else
			{
				if (this.HotKey == null)
				{
					return string.Empty;
				}
				Key key2 = this.HotKey.Keys.Find((Key k) => !k.IsControllerInput);
				if (key2 == null)
				{
					return null;
				}
				return key2.InputKey.ToString();
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0003F5E4 File Offset: 0x0003D7E4
		private TextObject GetKeyName()
		{
			if (this._forcedName != null)
			{
				return this._forcedName;
			}
			if (Game.Current != null)
			{
				if (this.HotKey != null)
				{
					return Game.Current.GameTextManager.FindText("str_key_name", this.HotKey.GroupId + "_" + this.HotKey.Id);
				}
				if (this.GameKey != null)
				{
					return Game.Current.GameTextManager.FindText("str_key_name", this.GameKey.GroupId + "_" + this.GameKey.StringId);
				}
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0003F68C File Offset: 0x0003D88C
		private void UpdateVisibility()
		{
			this.IsVisible = this._forcedVisibility ?? (!this._isVisibleToConsoleOnly || Input.IsGamepadActive);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0003F6C8 File Offset: 0x0003D8C8
		public static InputKeyItemVM CreateFromGameKey(GameKey gameKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0003F6E3 File Offset: 0x0003D8E3
		public static InputKeyItemVM CreateFromHotKey(HotKey hotKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0003F6FE File Offset: 0x0003D8FE
		public static InputKeyItemVM CreateFromHotKeyWithForcedName(HotKey hotKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0003F720 File Offset: 0x0003D920
		public static InputKeyItemVM CreateFromGameKeyWithForcedName(GameKey gameKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0003F742 File Offset: 0x0003D942
		public static InputKeyItemVM CreateFromForcedID(string forcedID, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM._forcedID = forcedID;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0003F764 File Offset: 0x0003D964
		// (set) Token: 0x06000F35 RID: 3893 RVA: 0x0003F76C File Offset: 0x0003D96C
		[DataSourceProperty]
		public string KeyID
		{
			get
			{
				return this._keyID;
			}
			set
			{
				if (value != this._keyID)
				{
					this._keyID = value;
					base.OnPropertyChangedWithValue<string>(value, "KeyID");
				}
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0003F78F File Offset: 0x0003D98F
		// (set) Token: 0x06000F37 RID: 3895 RVA: 0x0003F797 File Offset: 0x0003D997
		[DataSourceProperty]
		public string KeyName
		{
			get
			{
				return this._keyName;
			}
			set
			{
				if (value != this._keyName)
				{
					this._keyName = value;
					base.OnPropertyChangedWithValue<string>(value, "KeyName");
				}
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0003F7BA File Offset: 0x0003D9BA
		// (set) Token: 0x06000F39 RID: 3897 RVA: 0x0003F7C2 File Offset: 0x0003D9C2
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
				}
			}
		}

		// Token: 0x040006CE RID: 1742
		private bool _isVisibleToConsoleOnly;

		// Token: 0x040006CF RID: 1743
		private TextObject _forcedName;

		// Token: 0x040006D0 RID: 1744
		private string _forcedID;

		// Token: 0x040006D1 RID: 1745
		private bool? _forcedVisibility;

		// Token: 0x040006D2 RID: 1746
		private string _keyID;

		// Token: 0x040006D3 RID: 1747
		private string _keyName;

		// Token: 0x040006D4 RID: 1748
		private bool _isVisible;
	}
}
