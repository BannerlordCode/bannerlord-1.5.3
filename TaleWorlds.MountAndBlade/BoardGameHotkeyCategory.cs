using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022A RID: 554
	public sealed class BoardGameHotkeyCategory : GameKeyContext
	{
		// Token: 0x06002128 RID: 8488 RVA: 0x00074AB8 File Offset: 0x00072CB8
		public BoardGameHotkeyCategory()
			: base("BoardGameHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x00074ADC File Offset: 0x00072CDC
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.LeftMouseButton),
				new Key(InputKey.ControllerRDown)
			};
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.Space),
				new Key(InputKey.ControllerRBumper)
			};
			base.RegisterHotKey(new HotKey("BoardGamePawnSelect", "BoardGameHotkeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("BoardGamePawnDeselect", "BoardGameHotkeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("BoardGameDragPreview", "BoardGameHotkeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("BoardGameRollDice", "BoardGameHotkeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x00074BE2 File Offset: 0x00072DE2
		private void RegisterGameKeys()
		{
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00074BE4 File Offset: 0x00072DE4
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000B66 RID: 2918
		public const string CategoryId = "BoardGameHotkeyCategory";

		// Token: 0x04000B67 RID: 2919
		public const string BoardGamePawnSelect = "BoardGamePawnSelect";

		// Token: 0x04000B68 RID: 2920
		public const string BoardGamePawnDeselect = "BoardGamePawnDeselect";

		// Token: 0x04000B69 RID: 2921
		public const string BoardGameDragPreview = "BoardGameDragPreview";

		// Token: 0x04000B6A RID: 2922
		public const string BoardGameRollDice = "BoardGameRollDice";
	}
}
