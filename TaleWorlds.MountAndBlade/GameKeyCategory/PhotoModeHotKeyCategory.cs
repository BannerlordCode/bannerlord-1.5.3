using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GameKeyCategory
{
	// Token: 0x020003FC RID: 1020
	public sealed class PhotoModeHotKeyCategory : GameKeyContext
	{
		// Token: 0x06003822 RID: 14370 RVA: 0x000E912B File Offset: 0x000E732B
		public PhotoModeHotKeyCategory()
			: base("PhotoModeHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000E9150 File Offset: 0x000E7350
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.LeftShift),
				new Key(InputKey.ControllerRTrigger)
			};
			base.RegisterHotKey(new HotKey("FasterCamera", "PhotoModeHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x000E919C File Offset: 0x000E739C
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(92, "HideUI", "PhotoModeHotKeyCategory", InputKey.H, InputKey.ControllerRUp, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(93, "CameraRollLeft", "PhotoModeHotKeyCategory", InputKey.Q, InputKey.ControllerLBumper, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(94, "CameraRollRight", "PhotoModeHotKeyCategory", InputKey.E, InputKey.ControllerRBumper, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(97, "ToggleCameraFollowMode", "PhotoModeHotKeyCategory", InputKey.V, InputKey.ControllerRLeft, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(95, "TakePicture", "PhotoModeHotKeyCategory", InputKey.Enter, InputKey.ControllerRDown, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(96, "TakePictureWithAdditionalPasses", "PhotoModeHotKeyCategory", InputKey.BackSpace, InputKey.ControllerRBumper, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(98, "ToggleMouse", "PhotoModeHotKeyCategory", InputKey.C, InputKey.ControllerLThumb, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(99, "ToggleVignette", "PhotoModeHotKeyCategory", InputKey.X, InputKey.ControllerRThumb, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(100, "ToggleCharacters", "PhotoModeHotKeyCategory", InputKey.B, InputKey.ControllerRRight, GameKeyMainCategories.PhotoModeCategory), true);
			base.RegisterGameKey(new GameKey(107, "Reset", "PhotoModeHotKeyCategory", InputKey.T, InputKey.ControllerLOption, GameKeyMainCategories.PhotoModeCategory), true);
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000E9311 File Offset: 0x000E7511
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04001837 RID: 6199
		public const string CategoryId = "PhotoModeHotKeyCategory";

		// Token: 0x04001838 RID: 6200
		public const int HideUI = 92;

		// Token: 0x04001839 RID: 6201
		public const int CameraRollLeft = 93;

		// Token: 0x0400183A RID: 6202
		public const int CameraRollRight = 94;

		// Token: 0x0400183B RID: 6203
		public const int ToggleCameraFollowMode = 97;

		// Token: 0x0400183C RID: 6204
		public const int TakePicture = 95;

		// Token: 0x0400183D RID: 6205
		public const int TakePictureWithAdditionalPasses = 96;

		// Token: 0x0400183E RID: 6206
		public const int ToggleMouse = 98;

		// Token: 0x0400183F RID: 6207
		public const int ToggleVignette = 99;

		// Token: 0x04001840 RID: 6208
		public const int ToggleCharacters = 100;

		// Token: 0x04001841 RID: 6209
		public const int Reset = 107;

		// Token: 0x04001842 RID: 6210
		public const string FasterCamera = "FasterCamera";
	}
}
