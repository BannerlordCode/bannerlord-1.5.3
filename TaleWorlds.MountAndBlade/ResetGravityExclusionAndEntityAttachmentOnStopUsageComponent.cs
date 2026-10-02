using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000382 RID: 898
	public class ResetGravityExclusionAndEntityAttachmentOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x06003333 RID: 13107 RVA: 0x000D16A8 File Offset: 0x000CF8A8
		public ResetGravityExclusionAndEntityAttachmentOnStopUsageComponent(Action<Agent> onUseAction)
		{
			this.OnUseAction = onUseAction;
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000D16B7 File Offset: 0x000CF8B7
		protected internal override void OnUse(Agent userAgent)
		{
			this.OnUseAction(userAgent);
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000D16C5 File Offset: 0x000CF8C5
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			userAgent.SetExcludedFromGravity(false, false);
			userAgent.SetForceAttachedEntity(WeakGameEntity.Invalid);
		}

		// Token: 0x040015CE RID: 5582
		public Action<Agent> OnUseAction;
	}
}
