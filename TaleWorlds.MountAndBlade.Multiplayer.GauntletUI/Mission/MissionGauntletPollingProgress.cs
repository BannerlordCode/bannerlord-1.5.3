using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000019 RID: 25
	[OverrideView(typeof(MultiplayerPollProgressUIHandler))]
	public class MissionGauntletPollingProgress : MissionView
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000774E File Offset: 0x0000594E
		private InputContext _input
		{
			get
			{
				return base.MissionScreen.SceneLayer.Input;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007760 File Offset: 0x00005960
		public MissionGauntletPollingProgress()
		{
			this.ViewOrderPriority = 24;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00007770 File Offset: 0x00005970
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._multiplayerPollComponent = base.Mission.GetMissionBehavior<MultiplayerPollComponent>();
			MultiplayerPollComponent multiplayerPollComponent = this._multiplayerPollComponent;
			multiplayerPollComponent.OnKickPollOpened = (Action<MissionPeer, MissionPeer, bool>)Delegate.Combine(multiplayerPollComponent.OnKickPollOpened, new Action<MissionPeer, MissionPeer, bool>(this.OnKickPollOpened));
			MultiplayerPollComponent multiplayerPollComponent2 = this._multiplayerPollComponent;
			multiplayerPollComponent2.OnPollUpdated = (Action<int, int>)Delegate.Combine(multiplayerPollComponent2.OnPollUpdated, new Action<int, int>(this.OnPollUpdated));
			MultiplayerPollComponent multiplayerPollComponent3 = this._multiplayerPollComponent;
			multiplayerPollComponent3.OnPollClosed = (Action)Delegate.Combine(multiplayerPollComponent3.OnPollClosed, new Action(this.OnPollClosed));
			MultiplayerPollComponent multiplayerPollComponent4 = this._multiplayerPollComponent;
			multiplayerPollComponent4.OnPollCancelled = (Action)Delegate.Combine(multiplayerPollComponent4.OnPollCancelled, new Action(this.OnPollClosed));
			this._dataSource = new MultiplayerPollProgressVM();
			this._gauntletLayer = new GauntletLayer("MultiplayerPollingProgress", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerPollingProgress", this._dataSource);
			this._input.RegisterHotKeyCategory(HotKeyManager.GetCategory("PollHotkeyCategory"));
			this._dataSource.AddKey(HotKeyManager.GetCategory("PollHotkeyCategory").GetGameKey(108));
			this._dataSource.AddKey(HotKeyManager.GetCategory("PollHotkeyCategory").GetGameKey(109));
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000078C8 File Offset: 0x00005AC8
		public override void OnMissionScreenFinalize()
		{
			MultiplayerPollComponent multiplayerPollComponent = this._multiplayerPollComponent;
			multiplayerPollComponent.OnKickPollOpened = (Action<MissionPeer, MissionPeer, bool>)Delegate.Remove(multiplayerPollComponent.OnKickPollOpened, new Action<MissionPeer, MissionPeer, bool>(this.OnKickPollOpened));
			MultiplayerPollComponent multiplayerPollComponent2 = this._multiplayerPollComponent;
			multiplayerPollComponent2.OnPollUpdated = (Action<int, int>)Delegate.Remove(multiplayerPollComponent2.OnPollUpdated, new Action<int, int>(this.OnPollUpdated));
			MultiplayerPollComponent multiplayerPollComponent3 = this._multiplayerPollComponent;
			multiplayerPollComponent3.OnPollClosed = (Action)Delegate.Remove(multiplayerPollComponent3.OnPollClosed, new Action(this.OnPollClosed));
			MultiplayerPollComponent multiplayerPollComponent4 = this._multiplayerPollComponent;
			multiplayerPollComponent4.OnPollCancelled = (Action)Delegate.Remove(multiplayerPollComponent4.OnPollCancelled, new Action(this.OnPollClosed));
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			MultiplayerPollProgressVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._dataSource = null;
			base.MissionScreen.SetDisplayDialog(false);
			base.OnMissionScreenFinalize();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000079B4 File Offset: 0x00005BB4
		private void OnKickPollOpened(MissionPeer initiatorPeer, MissionPeer targetPeer, bool isBanRequested)
		{
			this._isActive = true;
			this._isVoteOpenForMyPeer = NetworkMain.GameClient.PlayerID == targetPeer.Peer.Id;
			this._dataSource.OnKickPollOpened(initiatorPeer, targetPeer, isBanRequested);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000079EB File Offset: 0x00005BEB
		private void OnPollUpdated(int votesAccepted, int votesRejected)
		{
			this._dataSource.OnPollUpdated(votesAccepted, votesRejected);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000079FA File Offset: 0x00005BFA
		private void OnPollClosed()
		{
			this._isActive = false;
			this._dataSource.OnPollClosed();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007A10 File Offset: 0x00005C10
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._isActive && !this._isVoteOpenForMyPeer)
			{
				if (this._input.IsGameKeyPressed(108))
				{
					this._isActive = false;
					this._multiplayerPollComponent.Vote(true);
					this._dataSource.OnPollOptionPicked();
					return;
				}
				if (this._input.IsGameKeyPressed(109))
				{
					this._isActive = false;
					this._multiplayerPollComponent.Vote(false);
					this._dataSource.OnPollOptionPicked();
				}
			}
		}

		// Token: 0x04000081 RID: 129
		private MultiplayerPollComponent _multiplayerPollComponent;

		// Token: 0x04000082 RID: 130
		private MultiplayerPollProgressVM _dataSource;

		// Token: 0x04000083 RID: 131
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000084 RID: 132
		private bool _isActive;

		// Token: 0x04000085 RID: 133
		private bool _isVoteOpenForMyPeer;
	}
}
