using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007A RID: 122
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AssignFormationToPlayer : GameNetworkMessage
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0000815E File Offset: 0x0000635E
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x00008166 File Offset: 0x00006366
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0000816F File Offset: 0x0000636F
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00008177 File Offset: 0x00006377
		public FormationClass FormationClass { get; private set; }

		// Token: 0x06000460 RID: 1120 RVA: 0x00008180 File Offset: 0x00006380
		public AssignFormationToPlayer(NetworkCommunicator peer, FormationClass formationClass)
		{
			this.Peer = peer;
			this.FormationClass = formationClass;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00008196 File Offset: 0x00006396
		public AssignFormationToPlayer()
		{
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000081A0 File Offset: 0x000063A0
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.FormationClass = (FormationClass)GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000081D0 File Offset: 0x000063D0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket((int)this.FormationClass, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000081ED File Offset: 0x000063ED
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000081F4 File Offset: 0x000063F4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Assign formation with index: ",
				(int)this.FormationClass,
				" to NetworkPeer with name: ",
				this.Peer.UserName,
				" and peer-index",
				this.Peer.Index,
				" and make him captain."
			});
		}
	}
}
