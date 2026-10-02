using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000407 RID: 1031
	public abstract class BattleSpawnModel : MBGameModel<BattleSpawnModel>
	{
		// Token: 0x0600387C RID: 14460 RVA: 0x000E956F File Offset: 0x000E776F
		public virtual void OnMissionStart()
		{
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x000E9571 File Offset: 0x000E7771
		public virtual void OnMissionEnd()
		{
		}

		// Token: 0x0600387E RID: 14462
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public abstract List<ValueTuple<IAgentOriginBase, int>> GetInitialSpawnAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins);

		// Token: 0x0600387F RID: 14463
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public abstract List<ValueTuple<IAgentOriginBase, int>> GetReinforcementAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins);
	}
}
