using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000285 RID: 645
	public class CasualtyHandler : MissionLogic
	{
		// Token: 0x060023FC RID: 9212 RVA: 0x00080971 File Offset: 0x0007EB71
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			this.RegisterCasualty(affectedAgent);
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x0008097A File Offset: 0x0007EB7A
		public override void OnAgentFleeing(Agent affectedAgent)
		{
			this.RegisterCasualty(affectedAgent);
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x00080984 File Offset: 0x0007EB84
		public int GetCasualtyCountOfFormation(Formation formation)
		{
			int num;
			if (!this._casualtyCounts.TryGetValue(formation, out num))
			{
				num = 0;
				this._casualtyCounts[formation] = 0;
			}
			return num;
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000809B4 File Offset: 0x0007EBB4
		public float GetCasualtyPowerLossOfFormation(Formation formation)
		{
			float num;
			if (!this._powerLoss.TryGetValue(formation, out num))
			{
				num = 0f;
				this._powerLoss[formation] = 0f;
			}
			return num;
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x000809EC File Offset: 0x0007EBEC
		private void RegisterCasualty(Agent agent)
		{
			Formation formation = agent.Formation;
			if (formation != null)
			{
				if (this._casualtyCounts.ContainsKey(formation))
				{
					Dictionary<Formation, int> casualtyCounts = this._casualtyCounts;
					Formation formation2 = formation;
					int num = casualtyCounts[formation2];
					casualtyCounts[formation2] = num + 1;
				}
				else
				{
					this._casualtyCounts[formation] = 1;
				}
				if (this._powerLoss.ContainsKey(formation))
				{
					Dictionary<Formation, float> powerLoss = this._powerLoss;
					Formation formation2 = formation;
					powerLoss[formation2] += agent.Character.GetPower();
					return;
				}
				this._powerLoss[formation] = agent.Character.GetPower();
			}
		}

		// Token: 0x04000DD8 RID: 3544
		private readonly Dictionary<Formation, int> _casualtyCounts = new Dictionary<Formation, int>();

		// Token: 0x04000DD9 RID: 3545
		private readonly Dictionary<Formation, float> _powerLoss = new Dictionary<Formation, float>();
	}
}
