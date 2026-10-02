using System;
using System.Linq;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022E RID: 558
	public sealed class CraftingHotkeyCategory : GameKeyContext
	{
		// Token: 0x06002138 RID: 8504 RVA: 0x000753E5 File Offset: 0x000735E5
		public CraftingHotkeyCategory()
			: base("CraftingHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00075408 File Offset: 0x00073608
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("Ascend", "CraftingHotkeyCategory", InputKey.MiddleMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Rotate", "CraftingHotkeyCategory", InputKey.LeftMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Zoom", "CraftingHotkeyCategory", InputKey.RightMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Copy", "CraftingHotkeyCategory", InputKey.C, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Paste", "CraftingHotkeyCategory", InputKey.V, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000754A0 File Offset: 0x000736A0
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(56, "ControllerZoomIn", "CraftingHotkeyCategory", InputKey.Invalid, InputKey.ControllerRTrigger, ""), true);
			base.RegisterGameKey(new GameKey(57, "ControllerZoomOut", "CraftingHotkeyCategory", InputKey.Invalid, InputKey.ControllerLTrigger, ""), true);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000754F4 File Offset: 0x000736F4
		private void RegisterGameAxisKeys()
		{
			GameAxisKey gameAxisKey = GenericGameKeyContext.Current.RegisteredGameAxisKeys.First<GameAxisKey>((GameAxisKey g) => g.Id.Equals("CameraAxisX"));
			GameAxisKey gameAxisKey2 = GenericGameKeyContext.Current.RegisteredGameAxisKeys.First<GameAxisKey>((GameAxisKey g) => g.Id.Equals("CameraAxisY"));
			base.RegisterGameAxisKey(gameAxisKey, true);
			base.RegisterGameAxisKey(gameAxisKey2, true);
		}

		// Token: 0x04000BA1 RID: 2977
		public const string CategoryId = "CraftingHotkeyCategory";

		// Token: 0x04000BA2 RID: 2978
		public const string Zoom = "Zoom";

		// Token: 0x04000BA3 RID: 2979
		public const string Rotate = "Rotate";

		// Token: 0x04000BA4 RID: 2980
		public const string Ascend = "Ascend";

		// Token: 0x04000BA5 RID: 2981
		public const string ResetCamera = "ResetCamera";

		// Token: 0x04000BA6 RID: 2982
		public const string Copy = "Copy";

		// Token: 0x04000BA7 RID: 2983
		public const string Paste = "Paste";

		// Token: 0x04000BA8 RID: 2984
		public const string ControllerRotationAxisX = "CameraAxisX";

		// Token: 0x04000BA9 RID: 2985
		public const string ControllerRotationAxisY = "CameraAxisY";

		// Token: 0x04000BAA RID: 2986
		public const int ControllerZoomIn = 56;

		// Token: 0x04000BAB RID: 2987
		public const int ControllerZoomOut = 57;
	}
}
