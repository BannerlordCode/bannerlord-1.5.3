using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Input
{
	// Token: 0x02000049 RID: 73
	public class InputKeyItemVM : ViewModel
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0001673C File Offset: 0x0001493C
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x00016744 File Offset: 0x00014944
		public GameKey GameKey { get; private set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001674D File Offset: 0x0001494D
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x00016755 File Offset: 0x00014955
		public HotKey HotKey { get; private set; }

		// Token: 0x06000617 RID: 1559 RVA: 0x0001675E File Offset: 0x0001495E
		private InputKeyItemVM()
		{
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged += this.OnKeybindsChanged;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00016797 File Offset: 0x00014997
		public override void OnFinalize()
		{
			base.OnFinalize();
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
			HotKeyManager.OnKeybindsChanged -= this.OnKeybindsChanged;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000167D0 File Offset: 0x000149D0
		private void OnGamepadActiveStateChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000167D8 File Offset: 0x000149D8
		private void OnKeybindsChanged()
		{
			this.ForceRefresh();
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000167E0 File Offset: 0x000149E0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ForceRefresh();
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000167EE File Offset: 0x000149EE
		public void SetForcedVisibility(bool? isVisible)
		{
			this._forcedVisibility = isVisible;
			this.UpdateVisibility();
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00016800 File Offset: 0x00014A00
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

		// Token: 0x0600061E RID: 1566 RVA: 0x0001687C File Offset: 0x00014A7C
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

		// Token: 0x0600061F RID: 1567 RVA: 0x00016994 File Offset: 0x00014B94
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

		// Token: 0x06000620 RID: 1568 RVA: 0x00016A3C File Offset: 0x00014C3C
		private void UpdateVisibility()
		{
			this.IsVisible = this._forcedVisibility ?? (!this._isVisibleToConsoleOnly || Input.IsGamepadActive);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00016A78 File Offset: 0x00014C78
		public static InputKeyItemVM CreateFromGameKey(GameKey gameKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00016A93 File Offset: 0x00014C93
		public static InputKeyItemVM CreateFromHotKey(HotKey hotKey, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00016AAE File Offset: 0x00014CAE
		public static InputKeyItemVM CreateFromHotKeyWithForcedName(HotKey hotKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.HotKey = hotKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00016AD0 File Offset: 0x00014CD0
		public static InputKeyItemVM CreateFromGameKeyWithForcedName(GameKey gameKey, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM.GameKey = gameKey;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00016AF2 File Offset: 0x00014CF2
		public static InputKeyItemVM CreateFromForcedID(string forcedID, TextObject forcedName, bool isConsoleOnly)
		{
			InputKeyItemVM inputKeyItemVM = new InputKeyItemVM();
			inputKeyItemVM._forcedID = forcedID;
			inputKeyItemVM._forcedName = forcedName;
			inputKeyItemVM._isVisibleToConsoleOnly = isConsoleOnly;
			inputKeyItemVM.ForceRefresh();
			return inputKeyItemVM;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00016B14 File Offset: 0x00014D14
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x00016B1C File Offset: 0x00014D1C
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

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00016B3F File Offset: 0x00014D3F
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x00016B47 File Offset: 0x00014D47
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

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00016B6A File Offset: 0x00014D6A
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x00016B72 File Offset: 0x00014D72
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

		// Token: 0x040002BC RID: 700
		private bool _isVisibleToConsoleOnly;

		// Token: 0x040002BD RID: 701
		private TextObject _forcedName;

		// Token: 0x040002BE RID: 702
		private string _forcedID;

		// Token: 0x040002BF RID: 703
		private bool? _forcedVisibility;

		// Token: 0x040002C0 RID: 704
		private string _keyID;

		// Token: 0x040002C1 RID: 705
		private string _keyName;

		// Token: 0x040002C2 RID: 706
		private bool _isVisible;
	}
}
