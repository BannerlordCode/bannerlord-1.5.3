using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.InputSystem
{
	// Token: 0x02000007 RID: 7
	public class HotKey
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002D30 File Offset: 0x00000F30
		private bool _isDoublePressActive
		{
			get
			{
				int num = Environment.TickCount - this._doublePressTime;
				return num < 500 && num >= 0;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00002D5B File Offset: 0x00000F5B
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00002D63 File Offset: 0x00000F63
		public List<Key> Keys { get; internal set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00002D6C File Offset: 0x00000F6C
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00002D74 File Offset: 0x00000F74
		public List<Key> DefaultKeys { get; private set; }

		// Token: 0x0600008C RID: 140 RVA: 0x00002D80 File Offset: 0x00000F80
		public HotKey(string id, string groupId, List<Key> keys, HotKey.Modifiers modifiers = HotKey.Modifiers.None, HotKey.Modifiers negativeModifiers = HotKey.Modifiers.None)
		{
			this.Id = id;
			this.GroupId = groupId;
			this.Keys = keys;
			this.DefaultKeys = new List<Key>();
			for (int i = 0; i < this.Keys.Count; i++)
			{
				this.DefaultKeys.Add(new Key(this.Keys[i].InputKey));
			}
			this._modifiers = modifiers;
			this._negativeModifiers = negativeModifiers;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002DFC File Offset: 0x00000FFC
		public HotKey(string id, string groupId, InputKey inputKey, HotKey.Modifiers modifiers = HotKey.Modifiers.None, HotKey.Modifiers negativeModifiers = HotKey.Modifiers.None)
		{
			this.Id = id;
			this.GroupId = groupId;
			this.Keys = new List<Key>
			{
				new Key(inputKey)
			};
			this.DefaultKeys = new List<Key>
			{
				new Key(inputKey)
			};
			this._modifiers = modifiers;
			this._negativeModifiers = negativeModifiers;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002E5B File Offset: 0x0000105B
		private bool IsKeyAllowed(Key key, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			return (isKeysAllowed || !key.IsKeyboardInput) && (isMouseButtonAllowed || !key.IsMouseButtonInput) && (isMouseWheelAllowed || !key.IsMouseWheelInput) && (isControllerAllowed || !key.IsControllerInput);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002E90 File Offset: 0x00001090
		private bool CheckModifiers()
		{
			bool flag = Input.IsKeyDown(InputKey.LeftControl) || Input.IsKeyDown(InputKey.RightControl);
			bool flag2 = Input.IsKeyDown(InputKey.LeftAlt) || Input.IsKeyDown(InputKey.RightAlt);
			bool flag3 = Input.IsKeyDown(InputKey.LeftShift) || Input.IsKeyDown(InputKey.RightShift);
			bool flag4 = true;
			bool flag5 = true;
			bool flag6 = true;
			if (this._modifiers.HasAnyFlag(HotKey.Modifiers.Control))
			{
				flag4 = flag;
			}
			if (this._modifiers.HasAnyFlag(HotKey.Modifiers.Alt))
			{
				flag5 = flag2;
			}
			if (this._modifiers.HasAnyFlag(HotKey.Modifiers.Shift))
			{
				flag6 = flag3;
			}
			if (this._negativeModifiers.HasAnyFlag(HotKey.Modifiers.Control))
			{
				flag4 = !flag;
			}
			if (this._negativeModifiers.HasAnyFlag(HotKey.Modifiers.Alt))
			{
				flag5 = !flag2;
			}
			if (this._negativeModifiers.HasAnyFlag(HotKey.Modifiers.Shift))
			{
				flag6 = !flag3;
			}
			return flag4 && flag5 && flag6;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002F5B File Offset: 0x0000115B
		private bool IsDown(Key key, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			return this.IsKeyAllowed(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed) && (this._modifiers == HotKey.Modifiers.None || this.CheckModifiers()) && key.IsDown();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002F88 File Offset: 0x00001188
		internal bool IsDown(bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			foreach (Key key in this.Keys)
			{
				if (this.IsDown(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002FEC File Offset: 0x000011EC
		private bool IsDownImmediate(Key key, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			return this.IsKeyAllowed(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed) && (this._modifiers == HotKey.Modifiers.None || this.CheckModifiers()) && key.IsDownImmediate();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003018 File Offset: 0x00001218
		internal bool IsDownImmediate(bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			foreach (Key key in this.Keys)
			{
				if (this.IsDownImmediate(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000307C File Offset: 0x0000127C
		private bool IsDoublePressed(Key key, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			if (!this.IsKeyAllowed(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed))
			{
				return false;
			}
			if (this._modifiers != HotKey.Modifiers.None && !this.CheckModifiers())
			{
				return false;
			}
			if (key.IsPressed())
			{
				if (this._isDoublePressActive)
				{
					this._doublePressTime = 0;
					return true;
				}
				this._doublePressTime = Environment.TickCount;
			}
			return false;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000030D4 File Offset: 0x000012D4
		internal bool IsDoublePressed(bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			foreach (Key key in this.Keys)
			{
				if (this.IsDoublePressed(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003138 File Offset: 0x00001338
		private bool IsPressed(Key key, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			return this.IsKeyAllowed(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed) && (this._modifiers == HotKey.Modifiers.None || this.CheckModifiers()) && key.IsPressed();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003164 File Offset: 0x00001364
		internal bool IsPressed(bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			foreach (Key key in this.Keys)
			{
				if (this.IsPressed(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000031C8 File Offset: 0x000013C8
		private bool IsReleased(Key key, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			return this.IsKeyAllowed(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed) && (this._modifiers == HotKey.Modifiers.None || this.CheckModifiers()) && key.IsReleased();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000031F4 File Offset: 0x000013F4
		internal bool IsReleased(bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			foreach (Key key in this.Keys)
			{
				if (this.IsReleased(key, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003258 File Offset: 0x00001458
		public bool HasModifier(HotKey.Modifiers modifier)
		{
			return this._modifiers.HasAnyFlag(modifier);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003266 File Offset: 0x00001466
		public bool HasSameModifiers(HotKey other)
		{
			return this._modifiers == other._modifiers;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003278 File Offset: 0x00001478
		public override string ToString()
		{
			string text = "";
			bool isGamepadActive = Input.IsGamepadActive;
			for (int i = 0; i < this.Keys.Count; i++)
			{
				if ((!isGamepadActive && !this.Keys[i].IsControllerInput) || (isGamepadActive && this.Keys[i].IsControllerInput))
				{
					return this.Keys[i].ToString();
				}
			}
			return text;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000032E8 File Offset: 0x000014E8
		public override bool Equals(object obj)
		{
			HotKey hotKey = obj as HotKey;
			return hotKey != null && hotKey.Id.Equals(this.Id) && hotKey.GroupId.Equals(this.GroupId) && hotKey.Keys.SequenceEqual<Key>(this.Keys);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003338 File Offset: 0x00001538
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000016 RID: 22
		private const int DOUBLE_PRESS_TIME = 500;

		// Token: 0x04000017 RID: 23
		private int _doublePressTime;

		// Token: 0x04000018 RID: 24
		public string Id;

		// Token: 0x04000019 RID: 25
		public string GroupId;

		// Token: 0x0400001C RID: 28
		private HotKey.Modifiers _modifiers;

		// Token: 0x0400001D RID: 29
		private HotKey.Modifiers _negativeModifiers;

		// Token: 0x02000013 RID: 19
		[Flags]
		public enum Modifiers
		{
			// Token: 0x0400015E RID: 350
			None = 0,
			// Token: 0x0400015F RID: 351
			Shift = 1,
			// Token: 0x04000160 RID: 352
			Alt = 2,
			// Token: 0x04000161 RID: 353
			Control = 4
		}
	}
}
