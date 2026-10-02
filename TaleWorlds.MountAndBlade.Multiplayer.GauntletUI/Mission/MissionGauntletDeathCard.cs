using System;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200000D RID: 13
	[OverrideView(typeof(MissionMultiplayerDeathCardUIHandler))]
	public class MissionGauntletDeathCard : MissionView
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00005064 File Offset: 0x00003264
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			MissionMultiplayerGameModeBaseClient missionBehavior = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._dataSource = new MPDeathCardVM(missionBehavior.GameType);
			this._gauntletLayer = new GauntletLayer("MultiplayerDeathCard", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerDeathCard", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>().OnMyAgentVisualSpawned += this.OnMainAgentVisualSpawned;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000050F0 File Offset: 0x000032F0
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (GameNetwork.MyPeer != null && this._myPeer == null)
			{
				this._myPeer = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			}
			MissionPeer myPeer = this._myPeer;
			if (myPeer != null && myPeer.WantsToSpawnAsBot && this._dataSource.IsActive)
			{
				this._dataSource.Deactivate();
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000514F File Offset: 0x0000334F
		private void OnMainAgentVisualSpawned()
		{
			this._dataSource.Deactivate();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000515C File Offset: 0x0000335C
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);
			if (affectedAgent.IsMine && blow.DamageType != DamageTypes.Invalid)
			{
				this._dataSource.OnMainAgentRemoved(affectorAgent, blow);
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000518C File Offset: 0x0000338C
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			this._dataSource.OnFinalize();
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>().OnMyAgentVisualSpawned -= this.OnMainAgentVisualSpawned;
			this._dataSource = null;
			this._gauntletLayer = null;
		}

		// Token: 0x0400003F RID: 63
		private MPDeathCardVM _dataSource;

		// Token: 0x04000040 RID: 64
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000041 RID: 65
		private MissionPeer _myPeer;
	}
}
