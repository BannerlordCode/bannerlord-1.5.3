using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000011 RID: 17
	public class GamepadCursorViewModel : ViewModel
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004F1C File Offset: 0x0000311C
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00004F24 File Offset: 0x00003124
		[DataSourceProperty]
		public bool IsConsoleMouseVisible
		{
			get
			{
				return this._isConsoleMouseVisible;
			}
			set
			{
				if (this._isConsoleMouseVisible != value)
				{
					this._isConsoleMouseVisible = value;
					base.OnPropertyChangedWithValue(value, "IsConsoleMouseVisible");
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004F42 File Offset: 0x00003142
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00004F4A File Offset: 0x0000314A
		[DataSourceProperty]
		public bool IsGamepadCursorVisible
		{
			get
			{
				return this._isGamepadCursorVisible;
			}
			set
			{
				if (this._isGamepadCursorVisible != value)
				{
					this._isGamepadCursorVisible = value;
					base.OnPropertyChangedWithValue(value, "IsGamepadCursorVisible");
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004F68 File Offset: 0x00003168
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004F70 File Offset: 0x00003170
		[DataSourceProperty]
		public float CursorPositionX
		{
			get
			{
				return this._cursorPositionX;
			}
			set
			{
				if (this._cursorPositionX != value)
				{
					this._cursorPositionX = value;
					base.OnPropertyChangedWithValue(value, "CursorPositionX");
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004F8E File Offset: 0x0000318E
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00004F96 File Offset: 0x00003196
		[DataSourceProperty]
		public float CursorPositionY
		{
			get
			{
				return this._cursorPositionY;
			}
			set
			{
				if (this._cursorPositionY != value)
				{
					this._cursorPositionY = value;
					base.OnPropertyChangedWithValue(value, "CursorPositionY");
				}
			}
		}

		// Token: 0x0400005B RID: 91
		private float _cursorPositionX = 960f;

		// Token: 0x0400005C RID: 92
		private float _cursorPositionY = 540f;

		// Token: 0x0400005D RID: 93
		private bool _isConsoleMouseVisible;

		// Token: 0x0400005E RID: 94
		private bool _isGamepadCursorVisible;
	}
}
