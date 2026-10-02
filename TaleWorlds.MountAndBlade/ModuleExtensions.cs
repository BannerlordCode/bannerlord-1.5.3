using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E1 RID: 737
	public static class ModuleExtensions
	{
		// Token: 0x06002B26 RID: 11046 RVA: 0x000A65D4 File Offset: 0x000A47D4
		public static IEnumerable<UsableMachine> GetUsedMachines(this Formation formation)
		{
			return from d in formation.Detachments
				select d as UsableMachine into u
				where u != null
				select u;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000A662F File Offset: 0x000A482F
		public static void StartUsingMachine(this Formation formation, UsableMachine usable, bool isPlayerOrder = false)
		{
			if (isPlayerOrder || (formation.IsAIControlled && !Mission.Current.IsMissionEnding))
			{
				formation.JoinDetachment(usable);
			}
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000A664F File Offset: 0x000A484F
		public static void StopUsingMachine(this Formation formation, UsableMachine usable, bool isPlayerOrder = false)
		{
			if (isPlayerOrder || formation.IsAIControlled)
			{
				formation.LeaveDetachment(usable);
			}
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000A6663 File Offset: 0x000A4863
		public static WorldPosition ToWorldPosition(this Vec3 rawPosition)
		{
			return new WorldPosition(Mission.Current.Scene, rawPosition);
		}
	}
}
