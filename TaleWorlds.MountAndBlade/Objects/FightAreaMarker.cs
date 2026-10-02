using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003A9 RID: 937
	public class FightAreaMarker : AreaMarker
	{
		// Token: 0x060035A9 RID: 13737 RVA: 0x000DDA16 File Offset: 0x000DBC16
		public IEnumerable<Agent> GetAgentsInRange(Team team, bool humanOnly = true)
		{
			foreach (Agent agent in team.ActiveAgents)
			{
				if ((!humanOnly || agent.IsHuman) && base.IsPositionInRange(agent.Position))
				{
					yield return agent;
				}
			}
			List<Agent>.Enumerator enumerator = default(List<Agent>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000DDA34 File Offset: 0x000DBC34
		public IEnumerable<Agent> GetAgentsInRange(BattleSideEnum side, bool humanOnly = true)
		{
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.Side == side)
				{
					foreach (Agent agent in this.GetAgentsInRange(team, humanOnly))
					{
						yield return agent;
					}
					IEnumerator<Agent> enumerator2 = null;
				}
			}
			List<Team>.Enumerator enumerator = default(List<Team>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x040016DD RID: 5853
		public int SubAreaIndex = 1;
	}
}
