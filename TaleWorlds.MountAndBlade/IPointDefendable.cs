using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015A RID: 346
	public interface IPointDefendable
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001224 RID: 4644
		IEnumerable<DefencePoint> DefencePoints { get; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001225 RID: 4645
		FormationAI.BehaviorSide DefenseSide { get; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001226 RID: 4646
		WorldFrame MiddleFrame { get; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001227 RID: 4647
		WorldFrame DefenseWaitFrame { get; }
	}
}
