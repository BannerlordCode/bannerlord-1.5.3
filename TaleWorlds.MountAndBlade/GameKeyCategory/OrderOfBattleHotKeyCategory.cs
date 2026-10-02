using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GameKeyCategory
{
	// Token: 0x020003FB RID: 1019
	public class OrderOfBattleHotKeyCategory : GameKeyContext
	{
		// Token: 0x06003820 RID: 14368 RVA: 0x000E9051 File Offset: 0x000E7251
		public OrderOfBattleHotKeyCategory()
			: base("OrderOfBattleHotKeyCategory", 0, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x000E9068 File Offset: 0x000E7268
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerRRight)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.Enter),
				new Key(InputKey.NumpadEnter),
				new Key(InputKey.ControllerRLeft)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.ControllerRUp)
			};
			base.RegisterHotKey(new HotKey("Exit", "OrderOfBattleHotKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Confirm", "OrderOfBattleHotKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("AutoDeploy", "OrderOfBattleHotKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x04001833 RID: 6195
		public const string CategoryId = "OrderOfBattleHotKeyCategory";

		// Token: 0x04001834 RID: 6196
		public const string Confirm = "Confirm";

		// Token: 0x04001835 RID: 6197
		public const string Exit = "Exit";

		// Token: 0x04001836 RID: 6198
		public const string AutoDeploy = "AutoDeploy";
	}
}
