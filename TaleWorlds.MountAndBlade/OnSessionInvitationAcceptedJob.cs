using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000255 RID: 597
	internal class OnSessionInvitationAcceptedJob : Job
	{
		// Token: 0x0600222D RID: 8749 RVA: 0x00077FF0 File Offset: 0x000761F0
		public OnSessionInvitationAcceptedJob(SessionInvitationType sessionInvitationType)
		{
			this._sessionInvitationType = sessionInvitationType;
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x00078000 File Offset: 0x00076200
		public override void DoJob(float dt)
		{
			base.DoJob(dt);
			if (MBGameManager.Current != null)
			{
				MBGameManager.Current.OnSessionInvitationAccepted(this._sessionInvitationType);
			}
			else if (GameStateManager.Current != null && GameStateManager.Current.ActiveState != null)
			{
				GameStateManager.Current.CleanStates(0);
			}
			base.Finished = true;
		}

		// Token: 0x04000D1F RID: 3359
		private readonly SessionInvitationType _sessionInvitationType;
	}
}
