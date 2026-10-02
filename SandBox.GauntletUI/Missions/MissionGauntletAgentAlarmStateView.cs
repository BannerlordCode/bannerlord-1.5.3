using System;
using SandBox.View.Missions;
using SandBox.ViewModelCollection.Missions;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Missions
{
	// Token: 0x02000019 RID: 25
	[OverrideView(typeof(MissionAgentAlarmStateView))]
	public class MissionGauntletAgentAlarmStateView : MissionAgentAlarmStateView
	{
		// Token: 0x06000171 RID: 369 RVA: 0x0000A65F File Offset: 0x0000885F
		public MissionGauntletAgentAlarmStateView()
		{
			this._dataSource = new MissionAgentAlarmStateVM();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A674 File Offset: 0x00008874
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource.Initialize(base.Mission, base.MissionScreen.CombatCamera);
			this._layer = new GauntletLayer("MissionAlarmState", 10, false);
			this._layer.LoadMovie("AgentAlarmStateMissionView", this._dataSource);
			base.MissionScreen.AddLayer(this._layer);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A6DE File Offset: 0x000088DE
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.MissionScreen.RemoveLayer(this._layer);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._layer = null;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A710 File Offset: 0x00008910
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			base.OnAgentBuild(agent, banner);
			MissionAgentAlarmStateVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentBuild(agent, banner);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A72C File Offset: 0x0000892C
		public override void OnAgentTeamChanged(Team prevTeam, Team newTeam, Agent agent)
		{
			base.OnAgentTeamChanged(prevTeam, newTeam, agent);
			MissionAgentAlarmStateVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentTeamChanged(prevTeam, newTeam, agent);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000A74A File Offset: 0x0000894A
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			MissionAgentAlarmStateVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnAgentRemoved(affectedAgent);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A768 File Offset: 0x00008968
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			MissionAgentAlarmStateVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.Update();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A781 File Offset: 0x00008981
		protected override void OnResumeView()
		{
			base.OnResumeView();
			ScreenManager.SetSuspendLayer(this._layer, false);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000A795 File Offset: 0x00008995
		protected override void OnSuspendView()
		{
			base.OnSuspendView();
			ScreenManager.SetSuspendLayer(this._layer, true);
		}

		// Token: 0x04000071 RID: 113
		private GauntletLayer _layer;

		// Token: 0x04000072 RID: 114
		private MissionAgentAlarmStateVM _dataSource;
	}
}
