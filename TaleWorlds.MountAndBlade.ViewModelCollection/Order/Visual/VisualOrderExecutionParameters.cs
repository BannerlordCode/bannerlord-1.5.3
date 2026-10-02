using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000028 RID: 40
	public readonly struct VisualOrderExecutionParameters
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0000BE24 File Offset: 0x0000A024
		public VisualOrderExecutionParameters(Agent agent = null, Formation formation = null, WorldPosition? worldPosition = null)
		{
			this.HasWorldPosition = worldPosition != null;
			this.WorldPosition = ((worldPosition != null) ? worldPosition.Value : WorldPosition.Invalid);
			this.HasAgent = agent != null;
			this.Agent = agent;
			this.HasFormation = formation != null;
			this.Formation = formation;
		}

		// Token: 0x0400016E RID: 366
		public readonly bool HasWorldPosition;

		// Token: 0x0400016F RID: 367
		public readonly WorldPosition WorldPosition;

		// Token: 0x04000170 RID: 368
		public readonly bool HasAgent;

		// Token: 0x04000171 RID: 369
		public readonly Agent Agent;

		// Token: 0x04000172 RID: 370
		public readonly bool HasFormation;

		// Token: 0x04000173 RID: 371
		public readonly Formation Formation;
	}
}
