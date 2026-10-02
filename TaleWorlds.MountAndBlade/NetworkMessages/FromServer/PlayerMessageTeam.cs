using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000040 RID: 64
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PlayerMessageTeam : GameNetworkMessage
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00004739 File Offset: 0x00002939
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00004741 File Offset: 0x00002941
		public string Message { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000474A File Offset: 0x0000294A
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00004752 File Offset: 0x00002952
		public NetworkCommunicator Player { get; private set; }

		// Token: 0x0600020A RID: 522 RVA: 0x0000475B File Offset: 0x0000295B
		public PlayerMessageTeam(NetworkCommunicator player, string message)
		{
			this.Player = player;
			this.Message = message;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00004771 File Offset: 0x00002971
		public PlayerMessageTeam()
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00004779 File Offset: 0x00002979
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Player);
			GameNetworkMessage.WriteStringToPacket(this.Message);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00004794 File Offset: 0x00002994
		protected override bool OnRead()
		{
			bool flag = true;
			this.Player = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000047BF File Offset: 0x000029BF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000047C4 File Offset: 0x000029C4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Receiving team message: ",
				this.Message,
				" from peer: ",
				this.Player.UserName,
				" index: ",
				this.Player.Index
			});
		}
	}
}
