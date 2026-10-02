using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000234 RID: 564
	public sealed class GenericGameKeyContext : GameKeyContext
	{
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x00075984 File Offset: 0x00073B84
		// (set) Token: 0x0600214A RID: 8522 RVA: 0x0007598B File Offset: 0x00073B8B
		public static GenericGameKeyContext Current { get; private set; }

		// Token: 0x0600214B RID: 8523 RVA: 0x00075993 File Offset: 0x00073B93
		public GenericGameKeyContext()
			: base("Generic", 116, GameKeyContext.GameKeyContextType.Default)
		{
			GenericGameKeyContext.Current = this;
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000759BB File Offset: 0x00073BBB
		private void RegisterHotKeys()
		{
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000759C0 File Offset: 0x00073BC0
		private void RegisterGameKeys()
		{
			GameKey gameKey = new GameKey(0, "Up", "Generic", InputKey.W, InputKey.ControllerLStickUp, GameKeyMainCategories.ActionCategory);
			GameKey gameKey2 = new GameKey(1, "Down", "Generic", InputKey.S, InputKey.ControllerLStickDown, GameKeyMainCategories.ActionCategory);
			GameKey gameKey3 = new GameKey(2, "Left", "Generic", InputKey.A, InputKey.ControllerLStickLeft, GameKeyMainCategories.ActionCategory);
			GameKey gameKey4 = new GameKey(3, "Right", "Generic", InputKey.D, InputKey.ControllerLStickRight, GameKeyMainCategories.ActionCategory);
			base.RegisterGameKey(gameKey, true);
			base.RegisterGameKey(gameKey2, true);
			base.RegisterGameKey(gameKey3, true);
			base.RegisterGameKey(gameKey4, true);
			base.RegisterGameKey(new GameKey(4, "Leave", "Generic", InputKey.Tab, InputKey.ControllerRRight, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameKey(new GameKey(5, "ShowIndicators", "Generic", InputKey.LeftAlt, InputKey.ControllerLBumper, GameKeyMainCategories.ActionCategory), true);
			base.RegisterGameAxisKey(new GameAxisKey("MovementAxisX", InputKey.ControllerLStick, gameKey4, gameKey3, GameAxisKey.AxisType.X), true);
			base.RegisterGameAxisKey(new GameAxisKey("MovementAxisY", InputKey.ControllerLStick, gameKey, gameKey2, GameAxisKey.AxisType.Y), true);
			base.RegisterGameAxisKey(new GameAxisKey("CameraAxisX", InputKey.ControllerRStick, null, null, GameAxisKey.AxisType.X), true);
			base.RegisterGameAxisKey(new GameAxisKey("CameraAxisY", InputKey.ControllerRStick, null, null, GameAxisKey.AxisType.Y), true);
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00075B0B File Offset: 0x00073D0B
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C42 RID: 3138
		public const string CategoryId = "Generic";

		// Token: 0x04000C43 RID: 3139
		public const int Up = 0;

		// Token: 0x04000C44 RID: 3140
		public const int Down = 1;

		// Token: 0x04000C45 RID: 3141
		public const int Right = 3;

		// Token: 0x04000C46 RID: 3142
		public const int Left = 2;

		// Token: 0x04000C47 RID: 3143
		public const string MovementAxisX = "MovementAxisX";

		// Token: 0x04000C48 RID: 3144
		public const string MovementAxisY = "MovementAxisY";

		// Token: 0x04000C49 RID: 3145
		public const string CameraAxisX = "CameraAxisX";

		// Token: 0x04000C4A RID: 3146
		public const string CameraAxisY = "CameraAxisY";

		// Token: 0x04000C4B RID: 3147
		public const int Leave = 4;

		// Token: 0x04000C4C RID: 3148
		public const int ShowIndicators = 5;
	}
}
