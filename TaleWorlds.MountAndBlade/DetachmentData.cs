using System;
using System.Collections.Generic;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000150 RID: 336
	public class DetachmentData
	{
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x0003199B File Offset: 0x0002FB9B
		public int AgentCount
		{
			get
			{
				return this.joinedFormations.SumQ<Formation>((Formation f) => f.CountOfDetachableNonPlayerUnits) + this.MovingAgentCount + this.DefendingAgentCount;
			}
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x000319D8 File Offset: 0x0002FBD8
		public bool IsPrecalculated()
		{
			int count = this.agentScores.Count;
			return count > 0 && count >= this.AgentCount;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00031A03 File Offset: 0x0002FC03
		public DetachmentData()
		{
			this.firstTime = Mission.Current.CurrentTime;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00031A34 File Offset: 0x0002FC34
		public void RemoveScoreOfAgent(Agent agent)
		{
			for (int i = this.agentScores.Count - 1; i >= 0; i--)
			{
				if (this.agentScores[i].Item1 == agent)
				{
					this.agentScores.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04000404 RID: 1028
		public List<Formation> joinedFormations = new List<Formation>();

		// Token: 0x04000405 RID: 1029
		public List<ValueTuple<Agent, List<float>>> agentScores = new List<ValueTuple<Agent, List<float>>>();

		// Token: 0x04000406 RID: 1030
		public int MovingAgentCount;

		// Token: 0x04000407 RID: 1031
		public int DefendingAgentCount;

		// Token: 0x04000408 RID: 1032
		public float firstTime;
	}
}
