using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000254 RID: 596
	internal class OnPlatformRequestedMultiplayerJob : Job
	{
		// Token: 0x0600222B RID: 8747 RVA: 0x00077F9C File Offset: 0x0007619C
		public override void DoJob(float dt)
		{
			base.DoJob(dt);
			if (MBGameManager.Current != null)
			{
				MBGameManager.Current.OnPlatformRequestedMultiplayer();
			}
			else if (GameStateManager.Current != null && GameStateManager.Current.ActiveState != null)
			{
				GameStateManager.Current.CleanStates(0);
			}
			base.Finished = true;
		}
	}
}
