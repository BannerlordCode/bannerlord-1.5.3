using System;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016E RID: 366
	public abstract class GameBadgeTracker
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x00010612 File Offset: 0x0000E812
		public virtual void OnPlayerJoin(PlayerData playerData)
		{
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00010614 File Offset: 0x0000E814
		public virtual void OnKill(KillData killData)
		{
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00010616 File Offset: 0x0000E816
		public virtual void OnStartingNextBattle()
		{
		}
	}
}
