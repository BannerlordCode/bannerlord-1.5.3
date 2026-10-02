using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Input
{
	// Token: 0x02000057 RID: 87
	public class InputKeyItemVM : ViewModel
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00014EFF File Offset: 0x000130FF
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00014F07 File Offset: 0x00013107
		public GameKey GameKey { get; private set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00014F10 File Offset: 0x00013110
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00014F18 File Offset: 0x00013118
		public HotKey HotKey { get; private set; }

		// Token: 0x0600057F RID: 1407 RVA: 0x00014F21 File Offset: 0x00013121
		private InputKeyItemVM()
		{
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged += this.OnKeybindsChanged;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00014F5A File Offset: 0x0001315A
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged -= this.OnKeybindsChanged;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00014F93 File Offset: 0x00013193
		private void OnGamepadActiveStateChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00014F9B File Offset: 0x0001319B
		private void OnKeybindsChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00014FA3 File Offset: 0x000131A3
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ForceRefresh();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00014FB1 File Offset: 0x000131B1
		public void SetForcedVisibility(bool? isVisible)
		{
			this._forcedVisibility = isVisible;
			this.UpdateVisibility();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00014FC0 File Offset: 0x000131C0
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

		// Token: 0x06000586 RID: 1414 RVA: 0x0001503C File Offset: 0x0001323C
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

		// Token: 0x06000587 RID: 1415 RVA: 0x00015154 File Offset: 0x00013354
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

		// Token: 0x06000588 RID: 1416 RVA: 0x000151FC File Offset: 0x000133FC
		private void UpdateVisibility()
		{
			this.IsVisible = this._forcedVisibility ?? (!this._isVisibleToConsoleOnly || Input.IsGamepadActive);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00015238 File Offset: 0x00013438
		public static InputKeyItemVM CreateFromGameKey(GameKey gameKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00015253 File Offset: 0x00013453
		public static InputKeyItemVM CreateFromHotKey(HotKey hotKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001526E File Offset: 0x0001346E
		public static InputKeyItemVM CreateFromHotKeyWithForcedName(HotKey hotKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00015290 File Offset: 0x00013490
		public static InputKeyItemVM CreateFromGameKeyWithForcedName(GameKey gameKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x000152B2 File Offset: 0x000134B2
		public static InputKeyItemVM CreateFromForcedID(string forcedID, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM._forcedID = forcedID;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x000152D4 File Offset: 0x000134D4
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x000152DC File Offset: 0x000134DC
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

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x000152FF File Offset: 0x000134FF
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x00015307 File Offset: 0x00013507
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

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001532A File Offset: 0x0001352A
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x00015332 File Offset: 0x00013532
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

		// Token: 0x040002C4 RID: 708
		private bool _isVisibleToConsoleOnly;

		// Token: 0x040002C5 RID: 709
		private TextObject _forcedName;

		// Token: 0x040002C6 RID: 710
		private string _forcedID;

		// Token: 0x040002C7 RID: 711
		private bool? _forcedVisibility;

		// Token: 0x040002C8 RID: 712
		private string _keyID;

		// Token: 0x040002C9 RID: 713
		private string _keyName;

		// Token: 0x040002CA RID: 714
		private bool _isVisible;
	}
}
