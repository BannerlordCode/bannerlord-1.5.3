using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews
{
	// Token: 0x0200001B RID: 27
	public class MultiplayerSpectatorSilhouetteView : MissionView
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (MultiplayerSpectatorHelper.IsStreamerModeActive())
			{
				this._reapplyTimer += dt;
				bool flag = this._reapplyTimer >= 0.5f;
				if (flag)
				{
					this._reapplyTimer = 0f;
				}
				this.RefreshContours(flag);
				this._isActive = true;
				return;
			}
			if (this._isActive)
			{
				this.ClearContours();
				this._isActive = false;
				this._reapplyTimer = 0f;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002D2C File Offset: 0x00000F2C
		private void RefreshContours(bool forceReapply)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsActive() && agent.IsHuman && (forceReapply || !this._contouredAgents.Contains(agent)))
				{
					MBAgentVisuals safeAgentVisuals = MultiplayerSpectatorSilhouetteView.GetSafeAgentVisuals(agent);
					if (!(safeAgentVisuals == null))
					{
						uint num = ((agent.Team != null) ? agent.Team.Color : Color.White.ToUnsignedInteger());
						safeAgentVisuals.SetContourColor(new uint?(num), true);
						this._contouredAgents.Add(agent);
					}
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002DEC File Offset: 0x00000FEC
		private void ClearContours()
		{
			foreach (Agent agent in this._contouredAgents)
			{
				MBAgentVisuals safeAgentVisuals = MultiplayerSpectatorSilhouetteView.GetSafeAgentVisuals(agent);
				if (safeAgentVisuals != null)
				{
					safeAgentVisuals.SetContourColor(null, true);
				}
			}
			this._contouredAgents.Clear();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002E5C File Offset: 0x0000105C
		private static MBAgentVisuals GetSafeAgentVisuals(Agent agent)
		{
			if (agent == null || agent.State == AgentState.Deleted)
			{
				return null;
			}
			MBAgentVisuals agentVisuals = agent.AgentVisuals;
			if (agentVisuals == null || !agentVisuals.IsValid())
			{
				return null;
			}
			return agentVisuals;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002E92 File Offset: 0x00001092
		public override void OnAgentDeleted(Agent affectedAgent)
		{
			base.OnAgentDeleted(affectedAgent);
			this._contouredAgents.Remove(affectedAgent);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002EA8 File Offset: 0x000010A8
		public override void OnClearScene()
		{
			this._contouredAgents.Clear();
			this._isActive = false;
			this._reapplyTimer = 0f;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002EC7 File Offset: 0x000010C7
		public override void OnMissionScreenFinalize()
		{
			this._contouredAgents.Clear();
			this._isActive = false;
			this._reapplyTimer = 0f;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x04000003 RID: 3
		private const float ReapplyIntervalSeconds = 0.5f;

		// Token: 0x04000004 RID: 4
		private readonly HashSet<Agent> _contouredAgents = new HashSet<Agent>();

		// Token: 0x04000005 RID: 5
		private bool _isActive;

		// Token: 0x04000006 RID: 6
		private float _reapplyTimer;
	}
}
