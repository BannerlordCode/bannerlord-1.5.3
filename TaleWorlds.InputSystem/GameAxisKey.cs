using System;
using TaleWorlds.Library;

namespace TaleWorlds.InputSystem
{
	// Token: 0x02000005 RID: 5
	public class GameAxisKey
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002756 File Offset: 0x00000956
		// (set) Token: 0x06000063 RID: 99 RVA: 0x0000275E File Offset: 0x0000095E
		public string Id { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002767 File Offset: 0x00000967
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000276F File Offset: 0x0000096F
		public Key AxisKey { get; internal set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002778 File Offset: 0x00000978
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002780 File Offset: 0x00000980
		public Key DefaultAxisKey { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002789 File Offset: 0x00000989
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00002791 File Offset: 0x00000991
		public GameKey PositiveKey { get; internal set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000279A File Offset: 0x0000099A
		// (set) Token: 0x0600006B RID: 107 RVA: 0x000027A2 File Offset: 0x000009A2
		public GameKey NegativeKey { get; internal set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000027AB File Offset: 0x000009AB
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000027B3 File Offset: 0x000009B3
		public GameAxisKey.AxisType Type { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000027BC File Offset: 0x000009BC
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000027C4 File Offset: 0x000009C4
		internal bool IsBinded { get; private set; }

		// Token: 0x06000070 RID: 112 RVA: 0x000027D0 File Offset: 0x000009D0
		public GameAxisKey(string id, InputKey axisKey, GameKey positiveKey, GameKey negativeKey, GameAxisKey.AxisType type = GameAxisKey.AxisType.X)
		{
			this.Id = id;
			this.AxisKey = new Key(axisKey);
			this.DefaultAxisKey = new Key(axisKey);
			this.PositiveKey = positiveKey;
			this.NegativeKey = negativeKey;
			this.Type = type;
			this.IsBinded = this.PositiveKey != null || this.NegativeKey != null;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002833 File Offset: 0x00000A33
		private bool IsKeyAllowed(Key key, bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			return (isKeysAllowed || !key.IsKeyboardInput) && (isMouseButtonAllowed || !key.IsMouseButtonInput) && (isMouseWheelAllowed || !key.IsMouseWheelInput) && (isControllerAllowed || !key.IsControllerInput);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002868 File Offset: 0x00000A68
		public float GetAxisState(bool isKeysAllowed, bool isMouseButtonAllowed, bool isMouseWheelAllowed, bool isControllerAllowed)
		{
			GameKey positiveKey = this.PositiveKey;
			bool flag = positiveKey != null && positiveKey.IsDown(isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed, false);
			GameKey negativeKey = this.NegativeKey;
			bool flag2 = negativeKey != null && negativeKey.IsDown(isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed, false);
			if (flag || flag2)
			{
				return (flag ? 1f : 0f) - (flag2 ? 1f : 0f);
			}
			Vec2 keyState = new Vec2(0f, 0f);
			if (this.AxisKey != null && this.IsKeyAllowed(this.AxisKey, isKeysAllowed, isMouseButtonAllowed, isMouseWheelAllowed, isControllerAllowed))
			{
				keyState = this.AxisKey.GetKeyState();
			}
			if (this.Type == GameAxisKey.AxisType.X)
			{
				return keyState.X;
			}
			if (this.Type == GameAxisKey.AxisType.Y)
			{
				return keyState.Y;
			}
			return 0f;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002930 File Offset: 0x00000B30
		public override string ToString()
		{
			string text = "";
			if (this.AxisKey != null)
			{
				text = this.AxisKey.ToString();
			}
			return text;
		}

		// Token: 0x02000011 RID: 17
		public enum AxisType
		{
			// Token: 0x04000156 RID: 342
			X,
			// Token: 0x04000157 RID: 343
			Y
		}
	}
}
