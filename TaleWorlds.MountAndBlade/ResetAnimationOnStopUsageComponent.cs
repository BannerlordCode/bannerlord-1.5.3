using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000381 RID: 897
	public class ResetAnimationOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x06003330 RID: 13104 RVA: 0x000D15E4 File Offset: 0x000CF7E4
		public ResetAnimationOnStopUsageComponent(ActionIndexCache successfulResetActionCode, bool alwaysResetWithAction)
		{
			this._successfulResetAction = successfulResetActionCode;
			this._alwaysResetWithAction = alwaysResetWithAction;
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x000D15FA File Offset: 0x000CF7FA
		public void UpdateSuccessfulResetAction(ActionIndexCache successfulResetActionCode)
		{
			this._successfulResetAction = successfulResetActionCode;
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000D1604 File Offset: 0x000CF804
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			ActionIndexCache actionIndexCache = ((isSuccessful || this._alwaysResetWithAction) ? this._successfulResetAction : ActionIndexCache.act_none);
			float num = ((userAgent.Mission.Mode == MissionMode.Deployment) ? 0f : (-0.2f));
			if (actionIndexCache == ActionIndexCache.act_none)
			{
				userAgent.SetActionChannel(1, in actionIndexCache, false, (AnimFlags)72UL, 0f, 1f, num, 0.4f, 0f, false, -0.2f, 0, true);
			}
			userAgent.SetActionChannel(0, in actionIndexCache, false, (AnimFlags)72UL, 0f, 1f, num, 0.4f, 0f, false, -0.2f, 0, true);
		}

		// Token: 0x040015CC RID: 5580
		private ActionIndexCache _successfulResetAction;

		// Token: 0x040015CD RID: 5581
		private readonly bool _alwaysResetWithAction;
	}
}
