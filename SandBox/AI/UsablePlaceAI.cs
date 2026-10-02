using System;
using TaleWorlds.MountAndBlade;

namespace SandBox.AI
{
	// Token: 0x02000110 RID: 272
	public class UsablePlaceAI : UsableMachineAIBase
	{
		// Token: 0x06000D88 RID: 3464 RVA: 0x00062020 File Offset: 0x00060220
		public UsablePlaceAI(UsableMachine usableMachine)
			: base(usableMachine)
		{
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0006202C File Offset: 0x0006022C
		protected override Agent.AIScriptedFrameFlags GetScriptedFrameFlags(Agent agent)
		{
			if (!this.UsableMachine.GameEntity.HasTag("quest_wanderer_target"))
			{
				return Agent.AIScriptedFrameFlags.DoNotRun;
			}
			return Agent.AIScriptedFrameFlags.None;
		}
	}
}
