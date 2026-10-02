using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000380 RID: 896
	public class OverrideStrikeAndDeathActionDuringUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x0600332D RID: 13101 RVA: 0x000D1588 File Offset: 0x000CF788
		public OverrideStrikeAndDeathActionDuringUsageComponent(in ActionIndexCache strikeAction, in ActionIndexCache deathAction)
		{
			this._strikeAction = strikeAction;
			this._deathAction = deathAction;
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000D15BE File Offset: 0x000CF7BE
		protected internal override void OnUse(Agent userAgent)
		{
			userAgent.SetOverridenStrikeAndDeathAction(in this._strikeAction, in this._deathAction);
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000D15D2 File Offset: 0x000CF7D2
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			userAgent.SetOverridenStrikeAndDeathAction(in ActionIndexCache.act_none, in ActionIndexCache.act_none);
		}

		// Token: 0x040015CA RID: 5578
		private readonly ActionIndexCache _strikeAction = ActionIndexCache.act_none;

		// Token: 0x040015CB RID: 5579
		private readonly ActionIndexCache _deathAction = ActionIndexCache.act_none;
	}
}
