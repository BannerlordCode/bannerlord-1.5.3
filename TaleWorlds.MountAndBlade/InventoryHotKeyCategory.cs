using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000236 RID: 566
	public sealed class InventoryHotKeyCategory : GameKeyContext
	{
		// Token: 0x06002155 RID: 8533 RVA: 0x00075E75 File Offset: 0x00074075
		public InventoryHotKeyCategory()
			: base("InventoryHotKeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00075E97 File Offset: 0x00074097
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("SwitchAlternative", "InventoryHotKeyCategory", InputKey.LeftAlt, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x00075EB3 File Offset: 0x000740B3
		private void RegisterGameKeys()
		{
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00075EB5 File Offset: 0x000740B5
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C5D RID: 3165
		public const string CategoryId = "InventoryHotKeyCategory";

		// Token: 0x04000C5E RID: 3166
		public const string SwitchAlternative = "SwitchAlternative";
	}
}
