using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000363 RID: 867
	public class StandingPointWithAgentLimit : StandingPoint
	{
		// Token: 0x06003201 RID: 12801 RVA: 0x000CBFFB File Offset: 0x000CA1FB
		public void AddValidAgent(Agent agent)
		{
			if (agent != null)
			{
				this._validAgents.Add(agent);
			}
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x000CC00C File Offset: 0x000CA20C
		public void ClearValidAgents()
		{
			this._validAgents.Clear();
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x000CC019 File Offset: 0x000CA219
		public override bool IsDisabledForAgent(Agent agent)
		{
			return !this._validAgents.Contains(agent) || base.IsDisabledForAgent(agent);
		}

		// Token: 0x04001516 RID: 5398
		private readonly List<Agent> _validAgents = new List<Agent>();
	}
}
