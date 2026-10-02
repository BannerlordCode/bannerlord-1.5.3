using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200023D RID: 573
	public sealed class ScoreboardHotKeyCategory : GameKeyContext
	{
		// Token: 0x0600216E RID: 8558 RVA: 0x00076C01 File Offset: 0x00074E01
		public ScoreboardHotKeyCategory()
			: base("ScoreboardHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00076C24 File Offset: 0x00074E24
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.F),
				new Key(InputKey.ControllerRUp)
			};
			base.RegisterHotKey(new HotKey("ToggleFastForward", "ScoreboardHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerROption)
			};
			base.RegisterHotKey(new HotKey("TogglePause", "ScoreboardHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("MenuShowContextMenu", "ScoreboardHotKeyCategory", InputKey.RightMouseButton, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.Tab),
				new Key(InputKey.ControllerRRight)
			};
			base.RegisterHotKey(new HotKey("HoldShow", "ScoreboardHotKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.H),
				new Key(InputKey.ControllerRLeft)
			};
			base.RegisterHotKey(new HotKey("ToggleHud", "ScoreboardHotKeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x00076D3D File Offset: 0x00074F3D
		private void RegisterGameKeys()
		{
			base.RegisterGameKey(new GameKey(35, "ShowMouse", "ScoreboardHotKeyCategory", InputKey.MiddleMouseButton, InputKey.ControllerLThumb, GameKeyMainCategories.ActionCategory), true);
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x00076D66 File Offset: 0x00074F66
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000CC2 RID: 3266
		public const string CategoryId = "ScoreboardHotKeyCategory";

		// Token: 0x04000CC3 RID: 3267
		public const int ShowMouse = 35;

		// Token: 0x04000CC4 RID: 3268
		public const string HoldShow = "HoldShow";

		// Token: 0x04000CC5 RID: 3269
		public const string ToggleFastForward = "ToggleFastForward";

		// Token: 0x04000CC6 RID: 3270
		public const string TogglePause = "TogglePause";

		// Token: 0x04000CC7 RID: 3271
		public const string MenuShowContextMenu = "MenuShowContextMenu";

		// Token: 0x04000CC8 RID: 3272
		public const string ToggleHud = "ToggleHud";
	}
}
