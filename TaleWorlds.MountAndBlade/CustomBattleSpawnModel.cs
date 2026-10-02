using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001FF RID: 511
	public class CustomBattleSpawnModel : BattleSpawnModel
	{
		// Token: 0x06001E12 RID: 7698 RVA: 0x00066877 File Offset: 0x00064A77
		public override void OnMissionStart()
		{
			MissionReinforcementsHelper.OnMissionStart();
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x0006687E File Offset: 0x00064A7E
		public override void OnMissionEnd()
		{
			MissionReinforcementsHelper.OnMissionEnd();
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x00066888 File Offset: 0x00064A88
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public override List<ValueTuple<IAgentOriginBase, int>> GetInitialSpawnAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins)
		{
			List<ValueTuple<IAgentOriginBase, int>> list = new List<ValueTuple<IAgentOriginBase, int>>();
			foreach (IAgentOriginBase agentOriginBase in troopOrigins)
			{
				ValueTuple<IAgentOriginBase, int> valueTuple = new ValueTuple<IAgentOriginBase, int>(agentOriginBase, (int)Mission.Current.GetAgentTroopClass(battleSide, agentOriginBase.Troop));
				list.Add(valueTuple);
			}
			return list;
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000668F8 File Offset: 0x00064AF8
		[return: TupleElementNames(new string[] { "origin", "formationIndex" })]
		public override List<ValueTuple<IAgentOriginBase, int>> GetReinforcementAssignments(BattleSideEnum battleSide, List<IAgentOriginBase> troopOrigins)
		{
			return MissionReinforcementsHelper.GetReinforcementAssignments(battleSide, troopOrigins);
		}
	}
}
