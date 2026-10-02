using System;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000017 RID: 23
	public class MissionRecentPlayersComponent : MissionNetwork
	{
		// Token: 0x0600015D RID: 349 RVA: 0x00005B34 File Offset: 0x00003D34
		public override void AfterStart()
		{
			base.AfterStart();
			MissionPeer.OnTeamChanged += this.TeamChange;
			MissionPeer.OnPlayerKilled += this.OnPlayerKilled;
			this._myId = NetworkMain.GameClient.PlayerID;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005B6E File Offset: 0x00003D6E
		private void TeamChange(NetworkCommunicator player, Team oldTeam, Team nextTeam)
		{
			if (player.VirtualPlayer.Id != this._myId)
			{
				RecentPlayersManager.AddOrUpdatePlayerEntry(player.VirtualPlayer.Id, player.UserName, InteractionType.InGameTogether, player.ForcedAvatarIndex);
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005BA8 File Offset: 0x00003DA8
		private void OnPlayerKilled(MissionPeer killerPeer, MissionPeer killedPeer)
		{
			if (killerPeer != null && killedPeer != null && killerPeer.Peer != null && killedPeer.Peer != null)
			{
				PlayerId id = killerPeer.Peer.Id;
				PlayerId id2 = killedPeer.Peer.Id;
				if (id == this._myId && id2 != this._myId)
				{
					RecentPlayersManager.AddOrUpdatePlayerEntry(id2, killedPeer.Name, InteractionType.Killed, killedPeer.GetNetworkPeer().ForcedAvatarIndex);
					return;
				}
				if (id2 == this._myId && id != this._myId)
				{
					RecentPlayersManager.AddOrUpdatePlayerEntry(id, killerPeer.Name, InteractionType.KilledBy, killerPeer.GetNetworkPeer().ForcedAvatarIndex);
				}
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005C58 File Offset: 0x00003E58
		public override void OnRemoveBehavior()
		{
			MissionPeer.OnTeamChanged -= this.TeamChange;
			MissionPeer.OnPlayerKilled -= this.OnPlayerKilled;
			base.OnRemoveBehavior();
		}

		// Token: 0x0400003E RID: 62
		private PlayerId _myId;
	}
}
