using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace StoryMode.ViewModelCollection.Missions
{
	// Token: 0x02000004 RID: 4
	public class TrainingObjectiveKeyVM : ViewModel
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002728 File Offset: 0x00000928
		public TrainingObjectiveKeyVM(TrainingObjectiveKeyVM.MouseAndClickInput mouseAndClickInput)
		{
			this.MovementType = (int)mouseAndClickInput.CurrentMovementType;
			this.MouseClick = (int)mouseAndClickInput.CurrentClickType;
			this.InputType = 0;
			switch (this.MouseClick)
			{
			case 0:
				this.ForcedKeyId = "mouse_left_click";
				return;
			case 1:
				this.ForcedKeyId = "mouse_middle_click";
				return;
			case 2:
				this.ForcedKeyId = "mouse_right_click";
				return;
			default:
				return;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002797 File Offset: 0x00000997
		public TrainingObjectiveKeyVM(TrainingObjectiveKeyVM.ControllerStickInput controllerStickInput)
		{
			this.MovementType = (int)controllerStickInput.CurrentMovementType;
			this.InputType = 2;
			this.OnForcedKeyNameChanged(controllerStickInput);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027B9 File Offset: 0x000009B9
		public TrainingObjectiveKeyVM(TrainingObjectiveKeyVM.KeyInput keyInput)
		{
			this.Key = keyInput.InputKeyItemVM;
			this.InputType = 1;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000027D4 File Offset: 0x000009D4
		private void OnForcedKeyNameChanged(TrainingObjectiveKeyVM.ControllerStickInput controllerStickInput)
		{
			string text = (controllerStickInput.IsLeftStick ? new TextObject("{=leftstickabbreviated}LS", null).ToString() : new TextObject("{=rightstickabbreviated}RS", null).ToString());
			this.ForcedKeyName = text;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002813 File Offset: 0x00000A13
		// (set) Token: 0x0600002F RID: 47 RVA: 0x0000281B File Offset: 0x00000A1B
		[DataSourceProperty]
		public InputKeyItemVM Key
		{
			get
			{
				return this._key;
			}
			set
			{
				if (value != this._key)
				{
					this._key = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "Key");
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002839 File Offset: 0x00000A39
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002841 File Offset: 0x00000A41
		[DataSourceProperty]
		public string ForcedKeyId
		{
			get
			{
				return this._forcedKeyId;
			}
			set
			{
				if (value != this._forcedKeyId)
				{
					this._forcedKeyId = value;
					base.OnPropertyChangedWithValue<string>(value, "ForcedKeyId");
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002864 File Offset: 0x00000A64
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000286C File Offset: 0x00000A6C
		[DataSourceProperty]
		public string ForcedKeyName
		{
			get
			{
				return this._forcedKeyName;
			}
			set
			{
				if (value != this._forcedKeyName)
				{
					this._forcedKeyName = value;
					base.OnPropertyChangedWithValue<string>(value, "ForcedKeyName");
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000288F File Offset: 0x00000A8F
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002897 File Offset: 0x00000A97
		[DataSourceProperty]
		public int MovementType
		{
			get
			{
				return this._movementType;
			}
			set
			{
				if (value != this._movementType)
				{
					this._movementType = value;
					base.OnPropertyChangedWithValue(value, "MovementType");
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000028B5 File Offset: 0x00000AB5
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000028BD File Offset: 0x00000ABD
		[DataSourceProperty]
		public int MouseClick
		{
			get
			{
				return this._mouseClick;
			}
			set
			{
				if (value != this._mouseClick)
				{
					this._mouseClick = value;
					base.OnPropertyChangedWithValue(value, "MouseClick");
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000028DB File Offset: 0x00000ADB
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000028E3 File Offset: 0x00000AE3
		[DataSourceProperty]
		public int InputType
		{
			get
			{
				return this._inputType;
			}
			set
			{
				if (value != this._inputType)
				{
					this._inputType = value;
					base.OnPropertyChangedWithValue(value, "InputType");
				}
			}
		}

		// Token: 0x04000014 RID: 20
		private InputKeyItemVM _key;

		// Token: 0x04000015 RID: 21
		private string _forcedKeyId;

		// Token: 0x04000016 RID: 22
		private string _forcedKeyName;

		// Token: 0x04000017 RID: 23
		private int _movementType;

		// Token: 0x04000018 RID: 24
		private int _mouseClick;

		// Token: 0x04000019 RID: 25
		private int _inputType;

		// Token: 0x02000007 RID: 7
		public enum MovementTypes
		{
			// Token: 0x0400001E RID: 30
			None,
			// Token: 0x0400001F RID: 31
			MoveLeft,
			// Token: 0x04000020 RID: 32
			MoveRight,
			// Token: 0x04000021 RID: 33
			MoveUp,
			// Token: 0x04000022 RID: 34
			MoveDown
		}

		// Token: 0x02000008 RID: 8
		public enum InputTypes
		{
			// Token: 0x04000024 RID: 36
			MouseAndClick,
			// Token: 0x04000025 RID: 37
			Key,
			// Token: 0x04000026 RID: 38
			ControllerStick
		}

		// Token: 0x02000009 RID: 9
		public struct MouseAndClickInput
		{
			// Token: 0x0600003F RID: 63 RVA: 0x00002983 File Offset: 0x00000B83
			public MouseAndClickInput(TrainingObjectiveKeyVM.MovementTypes movementType, TrainingObjectiveKeyVM.MouseClickTypes mouseClickType)
			{
				this.CurrentMovementType = movementType;
				this.CurrentClickType = mouseClickType;
			}

			// Token: 0x04000027 RID: 39
			public TrainingObjectiveKeyVM.MovementTypes CurrentMovementType;

			// Token: 0x04000028 RID: 40
			public TrainingObjectiveKeyVM.MouseClickTypes CurrentClickType;
		}

		// Token: 0x0200000A RID: 10
		public struct KeyInput
		{
			// Token: 0x06000040 RID: 64 RVA: 0x00002994 File Offset: 0x00000B94
			public KeyInput(int gameKeyDefinition, bool isCombatHotKey)
			{
				GameKey gameKey;
				if (isCombatHotKey)
				{
					gameKey = HotKeyManager.GetCategory("CombatHotKeyCategory").GetGameKey(gameKeyDefinition);
				}
				else
				{
					gameKey = HotKeyManager.GetCategory("Generic").GetGameKey(gameKeyDefinition);
				}
				this.InputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, false);
			}

			// Token: 0x04000029 RID: 41
			public InputKeyItemVM InputKeyItemVM;
		}

		// Token: 0x0200000B RID: 11
		public struct ControllerStickInput
		{
			// Token: 0x06000041 RID: 65 RVA: 0x000029D5 File Offset: 0x00000BD5
			public ControllerStickInput(TrainingObjectiveKeyVM.MovementTypes movementType, bool isLeftStick)
			{
				this.CurrentMovementType = movementType;
				this.IsLeftStick = isLeftStick;
			}

			// Token: 0x0400002A RID: 42
			public TrainingObjectiveKeyVM.MovementTypes CurrentMovementType;

			// Token: 0x0400002B RID: 43
			public bool IsLeftStick;
		}

		// Token: 0x0200000C RID: 12
		public enum MouseClickTypes
		{
			// Token: 0x0400002D RID: 45
			Left,
			// Token: 0x0400002E RID: 46
			Middle,
			// Token: 0x0400002F RID: 47
			Right
		}
	}
}
