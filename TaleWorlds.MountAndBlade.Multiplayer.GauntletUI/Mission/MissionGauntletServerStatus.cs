using System;
using NetworkMessages.FromServer;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200001A RID: 26
	[OverrideView(typeof(MissionMultiplayerServerStatusUIHandler))]
	public class MissionGauntletServerStatus : MissionView
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00007A8F File Offset: 0x00005C8F
		private bool IsOptionEnabled
		{
			get
			{
				return BannerlordConfig.EnableNetworkAlertIcons;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007A98 File Offset: 0x00005C98
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new MultiplayerMissionServerStatusVM();
			this._gauntletLayer = new GauntletLayer("MultiplayerServerStatus", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerServerStatus", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			NetworkCommunicator.OnPeerAveragePingUpdated += this.OnPeerPingUpdated;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007B06 File Offset: 0x00005D06
		private void OnPeerPingUpdated(NetworkCommunicator obj)
		{
			if (this.IsOptionEnabled && obj.IsMine)
			{
				this._dataSource.UpdatePeerPing(obj.AveragePingInMilliseconds);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00007B2C File Offset: 0x00005D2C
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this.IsOptionEnabled && GameNetwork.IsClient && GameNetwork.IsMyPeerReady)
			{
				this._dataSource.UpdatePacketLossRatio((GameNetwork.MyPeer != null) ? ((float)GameNetwork.MyPeer.AverageLossPercent) : 0f);
				MultiplayerMissionServerStatusVM dataSource = this._dataSource;
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				dataSource.UpdateServerPerformanceState((myPeer != null) ? myPeer.ServerPerformanceProblemState : ServerPerformanceState.High);
				return;
			}
			this._dataSource.ResetStates();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007BA2 File Offset: 0x00005DA2
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			NetworkCommunicator.OnPeerAveragePingUpdated -= this.OnPeerPingUpdated;
		}

		// Token: 0x04000086 RID: 134
		private MultiplayerMissionServerStatusVM _dataSource;

		// Token: 0x04000087 RID: 135
		private GauntletLayer _gauntletLayer;
	}
}
