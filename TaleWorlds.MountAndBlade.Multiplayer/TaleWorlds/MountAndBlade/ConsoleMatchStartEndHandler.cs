using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.PlatformService;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000015 RID: 21
	public class ConsoleMatchStartEndHandler : MissionNetwork
	{
		// Token: 0x0600014C RID: 332 RVA: 0x00005440 File Offset: 0x00003640
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._activeOtherPlayers = new List<VirtualPlayer>();
			this._matchState = ConsoleMatchStartEndHandler.MatchState.NotPlaying;
			this._gameModeClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._visualSpawnComponent = base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			this._visualSpawnComponent.OnMyAgentSpawnedFromVisual += this.AgentVisualSpawnComponentOnOnMyAgentVisualSpawned;
			MissionPeer.OnTeamChanged += this.OnTeamChange;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000054AF File Offset: 0x000036AF
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			MissionPeer.OnTeamChanged -= this.OnTeamChange;
			if (this._matchState == ConsoleMatchStartEndHandler.MatchState.Playing)
			{
				this._matchState = ConsoleMatchStartEndHandler.MatchState.NotPlaying;
				PlatformServices.MultiplayerGameStateChanged(false);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000054DE File Offset: 0x000036DE
		private void AgentVisualSpawnComponentOnOnMyAgentVisualSpawned()
		{
			this._visualSpawnComponent.OnMyAgentSpawnedFromVisual -= this.AgentVisualSpawnComponentOnOnMyAgentVisualSpawned;
			this._inGameCheckActive = true;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005500 File Offset: 0x00003700
		private void OnTeamChange(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (newTeam.Side == BattleSideEnum.None)
			{
				if (peer.IsMine)
				{
					this._visualSpawnComponent.OnMyAgentVisualSpawned += this.AgentVisualSpawnComponentOnOnMyAgentVisualSpawned;
					this._inGameCheckActive = false;
					PlatformServices.MultiplayerGameStateChanged(false);
					return;
				}
				int num = this._activeOtherPlayers.IndexOf(peer.VirtualPlayer);
				if (num >= 0)
				{
					this._activeOtherPlayers.RemoveAt(num);
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005568 File Offset: 0x00003768
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.MissionPeer != null && !agent.MissionPeer.IsMine && !this._activeOtherPlayers.Contains(agent.MissionPeer.Peer))
			{
				this._activeOtherPlayers.Add(agent.MissionPeer.Peer);
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000055B8 File Offset: 0x000037B8
		public override void OnMissionTick(float dt)
		{
			this._playingCheckTimer -= dt;
			if (this._playingCheckTimer <= 0f)
			{
				this._playingCheckTimer += 1f;
				if (this._inGameCheckActive)
				{
					if (this._activeOtherPlayers.Count > 0)
					{
						if (this._matchState == ConsoleMatchStartEndHandler.MatchState.NotPlaying)
						{
							this._matchState = ConsoleMatchStartEndHandler.MatchState.Playing;
							PlatformServices.MultiplayerGameStateChanged(true);
							return;
						}
					}
					else if (this._matchState == ConsoleMatchStartEndHandler.MatchState.Playing)
					{
						this._matchState = ConsoleMatchStartEndHandler.MatchState.NotPlaying;
						PlatformServices.MultiplayerGameStateChanged(false);
					}
				}
			}
		}

		// Token: 0x04000036 RID: 54
		private MissionMultiplayerGameModeBaseClient _gameModeClient;

		// Token: 0x04000037 RID: 55
		private MultiplayerMissionAgentVisualSpawnComponent _visualSpawnComponent;

		// Token: 0x04000038 RID: 56
		private ConsoleMatchStartEndHandler.MatchState _matchState;

		// Token: 0x04000039 RID: 57
		private bool _inGameCheckActive;

		// Token: 0x0400003A RID: 58
		private float _playingCheckTimer;

		// Token: 0x0400003B RID: 59
		private List<VirtualPlayer> _activeOtherPlayers;

		// Token: 0x0200009B RID: 155
		private enum MatchState
		{
			// Token: 0x04000194 RID: 404
			NotPlaying,
			// Token: 0x04000195 RID: 405
			Playing
		}
	}
}
