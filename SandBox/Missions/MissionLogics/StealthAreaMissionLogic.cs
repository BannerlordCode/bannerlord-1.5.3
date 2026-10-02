using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Objects.AreaMarkers;
using SandBox.Objects.Usables;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics
{
	// Token: 0x02000087 RID: 135
	public class StealthAreaMissionLogic : MissionLogic
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x000232B4 File Offset: 0x000214B4
		public MBReadOnlyList<Agent> AllyTroops
		{
			get
			{
				return this._allyTroops;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x000232BC File Offset: 0x000214BC
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x000232C4 File Offset: 0x000214C4
		public bool AllReinforcementsCalled { get; private set; }

		// Token: 0x06000543 RID: 1347 RVA: 0x000232F8 File Offset: 0x000214F8
		public bool IsSentry(Agent agent)
		{
			foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
			{
				foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
				{
					if (keyValuePair.Value.Contains(agent))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00023394 File Offset: 0x00021594
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			foreach (StealthAreaUsePoint stealthAreaUsePoint in base.Mission.MissionObjects.FindAllWithType<StealthAreaUsePoint>())
			{
				this._stealthAreaData.Add(new StealthAreaMissionLogic.StealthAreaData(stealthAreaUsePoint));
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000233FC File Offset: 0x000215FC
		private MBList<Agent> SpawnReinforcementAllyGroupTroops(StealthAreaMissionLogic.StealthAreaData triggeredStealthAreaData, StealthAreaMarker stealthAreaMarker)
		{
			StealthAreaMissionLogic.SpawnReinforcementAllyTroopsDelegate spawnReinforcementAllyTroopsEvent = this.SpawnReinforcementAllyTroopsEvent;
			return ((spawnReinforcementAllyTroopsEvent != null) ? spawnReinforcementAllyTroopsEvent(triggeredStealthAreaData, stealthAreaMarker) : null) ?? new MBList<Agent>();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0002341B File Offset: 0x0002161B
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			base.OnAgentBuild(agent, banner);
			this.CheckStealthAreaMarkerForAgent(agent);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0002342C File Offset: 0x0002162C
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			base.OnAgentTeamChanged(prevTeam, newTeam, agent);
			this.CheckStealthAreaMarkerForAgent(agent);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00023440 File Offset: 0x00021640
		private void CheckStealthAreaMarkerForAgent(Agent agent)
		{
			if (agent.IsHuman && agent.Team == Mission.Current.PlayerEnemyTeam)
			{
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
				{
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
					{
						if (keyValuePair.Key.IsPositionInRange(agent.Position))
						{
							stealthAreaData.AddAgentToStealthAreaMarker(keyValuePair.Key, agent);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0002350C File Offset: 0x0002170C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectorAgent != null && affectorAgent.IsMainAgent)
			{
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData in this._stealthAreaData)
				{
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
					{
						if (keyValuePair.Value.Contains(affectedAgent))
						{
							stealthAreaData.RemoveAgentFromStealthAreaMarker(keyValuePair.Key, affectedAgent);
						}
					}
				}
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000235C0 File Offset: 0x000217C0
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			if (usedObject is StealthAreaUsePoint)
			{
				if (this.IsInCombat())
				{
					return;
				}
				StealthAreaMissionLogic.StealthAreaData stealthAreaData = null;
				foreach (StealthAreaMissionLogic.StealthAreaData stealthAreaData2 in this._stealthAreaData)
				{
					if (stealthAreaData2.StealthAreaUsePoint == usedObject)
					{
						stealthAreaData = stealthAreaData2;
						break;
					}
				}
				if (stealthAreaData != null)
				{
					stealthAreaData.IsReinforcementCalled = true;
					foreach (KeyValuePair<StealthAreaMarker, List<Agent>> keyValuePair in stealthAreaData.StealthAreaMarkers)
					{
						MBList<Agent> mblist = this.SpawnReinforcementAllyGroupTroops(stealthAreaData, keyValuePair.Key);
						this._allyTroops.AddRange(mblist);
					}
				}
			}
			this.AllReinforcementsCalled = this._stealthAreaData.All<StealthAreaMissionLogic.StealthAreaData>((StealthAreaMissionLogic.StealthAreaData x) => x.IsReinforcementCalled);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x000236C4 File Offset: 0x000218C4
		private bool IsInCombat()
		{
			bool flag = false;
			foreach (Agent agent in Mission.Current.AllAgents)
			{
				if (agent.IsActive())
				{
					Agent.AIStateFlag aistateFlag = Agent.AIStateFlag.Alarmed;
					if ((agent.AIStateFlags & aistateFlag) == aistateFlag)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00023730 File Offset: 0x00021930
		public bool CheckIfAllStealthAreasAreTriggered()
		{
			return this._stealthAreaData.All<StealthAreaMissionLogic.StealthAreaData>((StealthAreaMissionLogic.StealthAreaData x) => x.IsStealthAreaTriggered);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0002375C File Offset: 0x0002195C
		public bool CheckIfAllStealthAreasReinforcementsAreCalled()
		{
			return this.AllReinforcementsCalled;
		}

		// Token: 0x040002C7 RID: 711
		private readonly MBList<StealthAreaMissionLogic.StealthAreaData> _stealthAreaData = new MBList<StealthAreaMissionLogic.StealthAreaData>();

		// Token: 0x040002C8 RID: 712
		private readonly Dictionary<string, Dictionary<string, int>> _agentSpawnTypes = new Dictionary<string, Dictionary<string, int>>();

		// Token: 0x040002C9 RID: 713
		private readonly MBList<Agent> _allyTroops = new MBList<Agent>();

		// Token: 0x040002CA RID: 714
		public StealthAreaMissionLogic.SpawnReinforcementAllyTroopsDelegate SpawnReinforcementAllyTroopsEvent;

		// Token: 0x02000193 RID: 403
		// (Invoke) Token: 0x06000F19 RID: 3865
		public delegate MBList<Agent> SpawnReinforcementAllyTroopsDelegate(StealthAreaMissionLogic.StealthAreaData triggeredStealthAreaData, StealthAreaMarker stealthAreaMarker);

		// Token: 0x02000194 RID: 404
		public class StealthAreaData
		{
			// Token: 0x06000F1C RID: 3868 RVA: 0x00067D68 File Offset: 0x00065F68
			internal StealthAreaData(StealthAreaUsePoint stealthAreaUsePoint)
			{
				this.StealthAreaUsePoint = stealthAreaUsePoint;
				this.StealthAreaMarkers = new Dictionary<StealthAreaMarker, List<Agent>>();
				foreach (WeakGameEntity weakGameEntity in stealthAreaUsePoint.GameEntity.GetChildren())
				{
					if (weakGameEntity.HasScriptOfType<StealthAreaMarker>())
					{
						this.StealthAreaMarkers.Add(weakGameEntity.GetFirstScriptOfType<StealthAreaMarker>(), new List<Agent>());
					}
				}
			}

			// Token: 0x06000F1D RID: 3869 RVA: 0x00067DF0 File Offset: 0x00065FF0
			internal void AddAgentToStealthAreaMarker(StealthAreaMarker stealthAreaMarker, Agent agent)
			{
				this.StealthAreaMarkers[stealthAreaMarker].Add(agent);
			}

			// Token: 0x06000F1E RID: 3870 RVA: 0x00067E04 File Offset: 0x00066004
			internal void RemoveAgentFromStealthAreaMarker(StealthAreaMarker stealthAreaMarker, Agent agent)
			{
				this.StealthAreaMarkers[stealthAreaMarker].Remove(agent);
				if (this.StealthAreaMarkers.All<KeyValuePair<StealthAreaMarker, List<Agent>>>((KeyValuePair<StealthAreaMarker, List<Agent>> x) => x.Value.IsEmpty<Agent>()))
				{
					this.StealthAreaUsePoint.EnableStealthAreaUsePoint();
					this.IsStealthAreaTriggered = true;
				}
			}

			// Token: 0x04000774 RID: 1908
			internal bool IsStealthAreaTriggered;

			// Token: 0x04000775 RID: 1909
			internal bool IsReinforcementCalled;

			// Token: 0x04000776 RID: 1910
			internal readonly StealthAreaUsePoint StealthAreaUsePoint;

			// Token: 0x04000777 RID: 1911
			internal readonly Dictionary<StealthAreaMarker, List<Agent>> StealthAreaMarkers;
		}
	}
}
