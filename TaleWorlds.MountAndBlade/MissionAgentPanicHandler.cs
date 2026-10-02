using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028E RID: 654
	public class MissionAgentPanicHandler : MissionLogic
	{
		// Token: 0x06002494 RID: 9364 RVA: 0x0008497E File Offset: 0x00082B7E
		public MissionAgentPanicHandler()
		{
			this._panickedAgents = new List<Agent>(256);
			this._panickedFormations = new List<Formation>(24);
			this._panickedTeams = new List<Team>(2);
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000849B0 File Offset: 0x00082BB0
		public override void OnAgentPanicked(Agent agent)
		{
			this._panickedAgents.Add(agent);
			if (agent.Formation != null && agent.Team != null)
			{
				if (!this._panickedFormations.Contains(agent.Formation))
				{
					this._panickedFormations.Add(agent.Formation);
				}
				if (!this._panickedTeams.Contains(agent.Team))
				{
					this._panickedTeams.Add(agent.Team);
				}
			}
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x00084A24 File Offset: 0x00082C24
		public override void OnPreMissionTick(float dt)
		{
			if (this._panickedAgents.Count > 0)
			{
				foreach (Team team in this._panickedTeams)
				{
					team.UpdateCachedEnemyDataForFleeing();
				}
				foreach (Formation formation in this._panickedFormations)
				{
					formation.OnBatchUnitRemovalStart();
				}
				foreach (Agent agent in this._panickedAgents)
				{
					CommonAIComponent commonAIComponent = agent.CommonAIComponent;
					if (commonAIComponent != null)
					{
						commonAIComponent.Retreat(false);
					}
					Mission.Current.OnAgentFleeing(agent);
				}
				foreach (Formation formation2 in this._panickedFormations)
				{
					formation2.OnBatchUnitRemovalEnd();
				}
				this._panickedAgents.Clear();
				this._panickedFormations.Clear();
				this._panickedTeams.Clear();
			}
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x00084B7C File Offset: 0x00082D7C
		public override void OnRemoveBehavior()
		{
			this._panickedAgents.Clear();
			this._panickedFormations.Clear();
			this._panickedTeams.Clear();
			base.OnRemoveBehavior();
		}

		// Token: 0x04000E17 RID: 3607
		private readonly List<Agent> _panickedAgents;

		// Token: 0x04000E18 RID: 3608
		private readonly List<Formation> _panickedFormations;

		// Token: 0x04000E19 RID: 3609
		private readonly List<Team> _panickedTeams;
	}
}
