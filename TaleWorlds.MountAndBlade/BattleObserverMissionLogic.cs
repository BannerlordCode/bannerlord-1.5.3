using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000282 RID: 642
	public class BattleObserverMissionLogic : MissionLogic
	{
		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x00080167 File Offset: 0x0007E367
		// (set) Token: 0x060023E6 RID: 9190 RVA: 0x0008016F File Offset: 0x0007E36F
		public IBattleObserver BattleObserver { get; private set; }

		// Token: 0x060023E7 RID: 9191 RVA: 0x00080178 File Offset: 0x0007E378
		public void SetObserver(IBattleObserver observer)
		{
			this.BattleObserver = observer;
			foreach (Agent agent in this._onAgentBuildCache)
			{
				this.BattleObserver.TroopNumberChanged(agent.Team.Side, agent.Origin.BattleCombatant, agent.Character, 1, 0, 0, 0, 0, 0);
				this._builtAgentCountForSides[(int)agent.Team.Side]++;
			}
			this._onAgentBuildCache.Clear();
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x00080220 File Offset: 0x0007E420
		public override void EarlyStart()
		{
			base.EarlyStart();
			this._builtAgentCountForSides = new int[2];
			this._removedAgentCountForSides = new int[2];
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x00080240 File Offset: 0x0007E440
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsHuman)
			{
				if (this.BattleObserver != null && agent.Team != Team.Invalid)
				{
					BattleSideEnum side = agent.Team.Side;
					this.BattleObserver.TroopNumberChanged(side, agent.Origin.BattleCombatant, agent.Character, 1, 0, 0, 0, 0, 0);
					this._builtAgentCountForSides[(int)side]++;
					return;
				}
				this._onAgentBuildCache.Add(agent);
			}
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x000802B8 File Offset: 0x0007E4B8
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent.IsHuman && affectedAgent.Team != Team.Invalid)
			{
				BattleSideEnum side = affectedAgent.Team.Side;
				switch (agentState)
				{
				case AgentState.Routed:
					this.BattleObserver.TroopNumberChanged(side, affectedAgent.Origin.BattleCombatant, affectedAgent.Character, -1, 0, 0, 1, 0, 0);
					break;
				case AgentState.Unconscious:
					this.BattleObserver.TroopNumberChanged(side, affectedAgent.Origin.BattleCombatant, affectedAgent.Character, -1, 0, 1, 0, 0, 0);
					break;
				case AgentState.Killed:
					this.BattleObserver.TroopNumberChanged(side, affectedAgent.Origin.BattleCombatant, affectedAgent.Character, -1, 1, 0, 0, 0, 0);
					break;
				default:
					throw new ArgumentOutOfRangeException("agentState", agentState, null);
				}
				this._removedAgentCountForSides[(int)side]++;
				if (affectorAgent != null && affectorAgent.IsHuman && (agentState == AgentState.Unconscious || agentState == AgentState.Killed))
				{
					this.BattleObserver.TroopNumberChanged(affectorAgent.Team.Side, affectorAgent.Origin.BattleCombatant, affectorAgent.Character, 0, 0, 0, 0, 1, 0);
				}
			}
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x000803D4 File Offset: 0x0007E5D4
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			if (prevTeam == Team.Invalid && agent.IsHuman && newTeam != null && newTeam != Team.Invalid)
			{
				this.BattleObserver.TroopNumberChanged(agent.Team.Side, agent.Origin.BattleCombatant, agent.Character, 1, 0, 0, 0, 0, 0);
				this._builtAgentCountForSides[(int)agent.Team.Side]++;
			}
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x00080444 File Offset: 0x0007E644
		public override void OnMissionResultReady(MissionResult missionResult)
		{
			if (missionResult.PlayerVictory)
			{
				this.BattleObserver.BattleResultsReady();
			}
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x00080459 File Offset: 0x0007E659
		public float GetDeathToBuiltAgentRatioForSide(BattleSideEnum side)
		{
			return (float)this._removedAgentCountForSides[(int)side] / (float)this._builtAgentCountForSides[(int)side];
		}

		// Token: 0x04000DD0 RID: 3536
		private int[] _builtAgentCountForSides;

		// Token: 0x04000DD1 RID: 3537
		private int[] _removedAgentCountForSides;

		// Token: 0x04000DD2 RID: 3538
		private List<Agent> _onAgentBuildCache = new List<Agent>();
	}
}
