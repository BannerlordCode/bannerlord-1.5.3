using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000DE RID: 222
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SynchronizingDone : GameNetworkMessage
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0000F33F File Offset: 0x0000D53F
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x0000F347 File Offset: 0x0000D547
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x0000F350 File Offset: 0x0000D550
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x0000F358 File Offset: 0x0000D558
		public bool Synchronized { get; private set; }

		// Token: 0x06000913 RID: 2323 RVA: 0x0000F361 File Offset: 0x0000D561
		public SynchronizingDone(NetworkCommunicator peer, bool synchronized)
		{
			this.Peer = peer;
			this.Synchronized = synchronized;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0000F377 File Offset: 0x0000D577
		public SynchronizingDone()
		{
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0000F380 File Offset: 0x0000D580
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Synchronized = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0000F3AB File Offset: 0x0000D5AB
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteBoolToPacket(this.Synchronized);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0000F3C3 File Offset: 0x0000D5C3
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0000F3C8 File Offset: 0x0000D5C8
		protected override string OnGetLogFormat()
		{
			string text = string.Concat(new object[]
			{
				"peer with name: ",
				this.Peer.UserName,
				", and index: ",
				this.Peer.Index
			});
			if (!this.Synchronized)
			{
				return "Synchronized: FALSE for " + text + " (Peer will not receive broadcasted messages)";
			}
			return "Synchronized: TRUE for " + text + " (received all initial data from the server and will now receive broadcasted messages)";
		}
	}
}
