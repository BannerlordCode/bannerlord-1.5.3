using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000229 RID: 553
	public sealed class ArmyManagementHotkeyCategory : GameKeyContext
	{
		// Token: 0x06002126 RID: 8486 RVA: 0x00074A83 File Offset: 0x00072C83
		public ArmyManagementHotkeyCategory()
			: base("ArmyManagementHotkeyCategory", 116, GameKeyContext.GameKeyContextType.Default)
		{
			this.RegisterHotKeys();
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x00074A99 File Offset: 0x00072C99
		private void RegisterHotKeys()
		{
			base.RegisterHotKey(new HotKey("RemoveParty", "ArmyManagementHotkeyCategory", InputKey.ControllerRBumper, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x04000B64 RID: 2916
		public const string CategoryId = "ArmyManagementHotkeyCategory";

		// Token: 0x04000B65 RID: 2917
		public const string RemoveParty = "RemoveParty";
	}
}
