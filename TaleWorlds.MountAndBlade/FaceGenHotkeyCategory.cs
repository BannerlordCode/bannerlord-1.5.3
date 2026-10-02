using System;
using System.Linq;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022F RID: 559
	public sealed class FaceGenHotkeyCategory : GameKeyContext
	{
		// Token: 0x0600213C RID: 8508 RVA: 0x0007556F File Offset: 0x0007376F
		public FaceGenHotkeyCategory()
			: base("FaceGenHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x00075594 File Offset: 0x00073794
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("Ascend", "FaceGenHotkeyCategory", InputKey.MiddleMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Rotate", "FaceGenHotkeyCategory", InputKey.LeftMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Zoom", "FaceGenHotkeyCategory", InputKey.RightMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Copy", "FaceGenHotkeyCategory", InputKey.C, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Paste", "FaceGenHotkeyCategory", InputKey.V, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0007562C File Offset: 0x0007382C
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(56, "ControllerZoomIn", "FaceGenHotkeyCategory", InputKey.Invalid, InputKey.ControllerRTrigger, ""), true);
			base.RegisterGameKey(new GameKey(57, "ControllerZoomOut", "FaceGenHotkeyCategory", InputKey.Invalid, InputKey.ControllerLTrigger, ""), true);
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x00075680 File Offset: 0x00073880
		private void RegisterGameAxisKeys()
		{
			GameAxisKey gameAxisKey = GenericGameKeyContext.Current.RegisteredGameAxisKeys.First<GameAxisKey>((GameAxisKey g) => g.Id.Equals("CameraAxisX"));
			GameAxisKey gameAxisKey2 = GenericGameKeyContext.Current.RegisteredGameAxisKeys.First<GameAxisKey>((GameAxisKey g) => g.Id.Equals("CameraAxisY"));
			base.RegisterGameAxisKey(gameAxisKey, true);
			base.RegisterGameAxisKey(gameAxisKey2, true);
		}

		// Token: 0x04000BAC RID: 2988
		public const string CategoryId = "FaceGenHotkeyCategory";

		// Token: 0x04000BAD RID: 2989
		public const string Zoom = "Zoom";

		// Token: 0x04000BAE RID: 2990
		public const string Rotate = "Rotate";

		// Token: 0x04000BAF RID: 2991
		public const string Ascend = "Ascend";

		// Token: 0x04000BB0 RID: 2992
		public const string ControllerRotationAxis = "CameraAxisX";

		// Token: 0x04000BB1 RID: 2993
		public const string ControllerCameraUpDownAxis = "CameraAxisY";

		// Token: 0x04000BB2 RID: 2994
		public const string Copy = "Copy";

		// Token: 0x04000BB3 RID: 2995
		public const string Paste = "Paste";

		// Token: 0x04000BB4 RID: 2996
		public const int ControllerZoomIn = 56;

		// Token: 0x04000BB5 RID: 2997
		public const int ControllerZoomOut = 57;
	}
}
