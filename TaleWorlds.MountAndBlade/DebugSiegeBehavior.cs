using System;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000376 RID: 886
	public static class DebugSiegeBehavior
	{
		// Token: 0x0600330F RID: 13071 RVA: 0x000D13E8 File Offset: 0x000CF5E8
		public static void SiegeDebug()
		{
			if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtRam"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.DebugDefendersToRam;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtSt"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.DebugDefendersToTower;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtBallistas2"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.DebugDefendersToBallistae;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtMangonels2"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.DebugDefendersToMangonels;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtNone2"))
			{
				DebugSiegeBehavior.DebugDefendState = DebugSiegeBehavior.DebugStateDefender.None;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtBallistas"))
			{
				DebugSiegeBehavior.DebugAttackState = DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToBallistae;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtMangonels"))
			{
				DebugSiegeBehavior.DebugAttackState = DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToMangonels;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtBattlements"))
			{
				DebugSiegeBehavior.DebugAttackState = DebugSiegeBehavior.DebugStateAttacker.DebugAttackersToBattlements;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyAimAtNone"))
			{
				DebugSiegeBehavior.DebugAttackState = DebugSiegeBehavior.DebugStateAttacker.None;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyTargetDebugActive"))
			{
				DebugSiegeBehavior.ToggleTargetDebug = true;
			}
			else if (Input.DebugInput.IsHotKeyPressed("DebugSiegeBehaviorHotkeyTargetDebugDisactive"))
			{
				DebugSiegeBehavior.ToggleTargetDebug = false;
			}
			bool toggleTargetDebug = DebugSiegeBehavior.ToggleTargetDebug;
		}

		// Token: 0x040015B3 RID: 5555
		public static bool ToggleTargetDebug;

		// Token: 0x040015B4 RID: 5556
		public static DebugSiegeBehavior.DebugStateAttacker DebugAttackState;

		// Token: 0x040015B5 RID: 5557
		public static DebugSiegeBehavior.DebugStateDefender DebugDefendState;

		// Token: 0x02000656 RID: 1622
		public enum DebugStateAttacker
		{
			// Token: 0x040021C7 RID: 8647
			None,
			// Token: 0x040021C8 RID: 8648
			DebugAttackersToBallistae,
			// Token: 0x040021C9 RID: 8649
			DebugAttackersToMangonels,
			// Token: 0x040021CA RID: 8650
			DebugAttackersToBattlements
		}

		// Token: 0x02000657 RID: 1623
		public enum DebugStateDefender
		{
			// Token: 0x040021CC RID: 8652
			None,
			// Token: 0x040021CD RID: 8653
			DebugDefendersToBallistae,
			// Token: 0x040021CE RID: 8654
			DebugDefendersToMangonels,
			// Token: 0x040021CF RID: 8655
			DebugDefendersToRam,
			// Token: 0x040021D0 RID: 8656
			DebugDefendersToTower
		}
	}
}
