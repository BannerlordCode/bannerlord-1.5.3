using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000069 RID: 105
	public class MissionAgentContourControllerView : MissionView
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0001E51D File Offset: 0x0001C71D
		private bool _isAllowedByOption
		{
			get
			{
				return !BannerlordConfig.HideBattleUI || GameNetwork.IsMultiplayer;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001E530 File Offset: 0x0001C730
		public MissionAgentContourControllerView()
		{
			this._contourAgents = new List<Agent>();
			this._isMultiplayer = GameNetwork.IsSessionActive;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001E5CE File Offset: 0x0001C7CE
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isAllowedByOption)
			{
				bool getUIDebugMode = NativeConfig.GetUIDebugMode;
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001E5E8 File Offset: 0x0001C7E8
		private void PopulateContourListWithAgents()
		{
			this._contourAgents.Clear();
			Mission mission = base.Mission;
			bool flag;
			if (mission == null)
			{
				flag = null != null;
			}
			else
			{
				Team playerTeam = mission.PlayerTeam;
				flag = ((playerTeam != null) ? playerTeam.PlayerOrderController : null) != null;
			}
			if (flag)
			{
				foreach (Formation formation in Mission.Current.PlayerTeam.PlayerOrderController.SelectedFormations)
				{
					formation.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						if (!agent.IsMainAgent)
						{
							this._contourAgents.Add(agent);
						}
					}, null);
				}
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001E680 File Offset: 0x0001C880
		public override void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
			base.OnFocusGained(agent, focusableObject, isInteractable);
			bool isAllowedByOption = this._isAllowedByOption;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001E692 File Offset: 0x0001C892
		public override void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			base.OnFocusLost(agent, focusableObject);
			if (this._isAllowedByOption)
			{
				this.RemoveContourFromFocusedAgent();
				this._currentFocusedAgent = null;
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001E6B1 File Offset: 0x0001C8B1
		private void AddContourToFocusedAgent()
		{
			if (this._currentFocusedAgent != null && !this._isContourAppliedToFocusedAgent)
			{
				MBAgentVisuals agentVisuals = this._currentFocusedAgent.AgentVisuals;
				if (agentVisuals != null)
				{
					agentVisuals.SetContourColor(new uint?(this._focusedContourColor), true);
				}
				this._isContourAppliedToFocusedAgent = true;
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001E6EC File Offset: 0x0001C8EC
		private void RemoveContourFromFocusedAgent()
		{
			if (this._currentFocusedAgent != null && this._isContourAppliedToFocusedAgent)
			{
				if (this._contourAgents.Contains(this._currentFocusedAgent))
				{
					MBAgentVisuals agentVisuals = this._currentFocusedAgent.AgentVisuals;
					if (agentVisuals != null)
					{
						agentVisuals.SetContourColor(new uint?(this._nonFocusedContourColor), true);
					}
				}
				else
				{
					MBAgentVisuals agentVisuals2 = this._currentFocusedAgent.AgentVisuals;
					if (agentVisuals2 != null)
					{
						agentVisuals2.SetContourColor(null, true);
					}
				}
				this._isContourAppliedToFocusedAgent = false;
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001E768 File Offset: 0x0001C968
		private void ApplyContourToAllAgents()
		{
			if (!this._isContourAppliedToAllAgents)
			{
				foreach (Agent agent in this._contourAgents)
				{
					uint num = ((agent == this._currentFocusedAgent) ? this._focusedContourColor : (this._isMultiplayer ? this._friendlyContourColor : this._nonFocusedContourColor));
					MBAgentVisuals agentVisuals = agent.AgentVisuals;
					if (agentVisuals != null)
					{
						agentVisuals.SetContourColor(new uint?(num), true);
					}
				}
				this._isContourAppliedToAllAgents = true;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001E804 File Offset: 0x0001CA04
		private void RemoveContourFromAllAgents()
		{
			if (this._isContourAppliedToAllAgents)
			{
				foreach (Agent agent in this._contourAgents)
				{
					if (this._currentFocusedAgent == null || agent != this._currentFocusedAgent)
					{
						MBAgentVisuals agentVisuals = agent.AgentVisuals;
						if (agentVisuals != null)
						{
							agentVisuals.SetContourColor(null, true);
						}
					}
				}
				this._isContourAppliedToAllAgents = false;
			}
		}

		// Token: 0x0400025F RID: 607
		private const bool IsEnabled = false;

		// Token: 0x04000260 RID: 608
		private uint _nonFocusedContourColor = new Color(0.85f, 0.85f, 0.85f, 1f).ToUnsignedInteger();

		// Token: 0x04000261 RID: 609
		private uint _focusedContourColor = new Color(1f, 0.84f, 0.35f, 1f).ToUnsignedInteger();

		// Token: 0x04000262 RID: 610
		private uint _friendlyContourColor = new Color(0.44f, 0.83f, 0.26f, 1f).ToUnsignedInteger();

		// Token: 0x04000263 RID: 611
		private List<Agent> _contourAgents;

		// Token: 0x04000264 RID: 612
		private Agent _currentFocusedAgent;

		// Token: 0x04000265 RID: 613
		private bool _isContourAppliedToAllAgents;

		// Token: 0x04000266 RID: 614
		private bool _isContourAppliedToFocusedAgent;

		// Token: 0x04000267 RID: 615
		private bool _isMultiplayer;
	}
}
