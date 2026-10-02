using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000079 RID: 121
	public class MissionPlayerMovementFlagsChangeEvent : EventBase
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00024241 File Offset: 0x00022441
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00024249 File Offset: 0x00022449
		public Agent.MovementControlFlag MovementFlag { get; private set; }

		// Token: 0x060004BB RID: 1211 RVA: 0x00024252 File Offset: 0x00022452
		public MissionPlayerMovementFlagsChangeEvent(Agent.MovementControlFlag movementFlag)
		{
			this.MovementFlag = movementFlag;
		}
	}
}
