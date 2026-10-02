using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000065 RID: 101
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class PeerMostUsedWeaponChange : GameNetworkMessage
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00006DA9 File Offset: 0x00004FA9
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00006DB1 File Offset: 0x00004FB1
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00006DBA File Offset: 0x00004FBA
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00006DC2 File Offset: 0x00004FC2
		public WeaponClass WeaponClass { get; private set; }

		// Token: 0x06000386 RID: 902 RVA: 0x00006DCB File Offset: 0x00004FCB
		public PeerMostUsedWeaponChange(NetworkCommunicator peer, WeaponClass weaponClass)
		{
			this.Peer = peer;
			this.WeaponClass = weaponClass;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00006DE1 File Offset: 0x00004FE1
		public PeerMostUsedWeaponChange()
		{
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00006DEC File Offset: 0x00004FEC
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.WeaponClass = (WeaponClass)GameNetworkMessage.ReadIntFromPacket(CompressionMission.WeaponClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00006E1C File Offset: 0x0000501C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteIntToPacket((int)this.WeaponClass, CompressionMission.WeaponClassCompressionInfo);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00006E39 File Offset: 0x00005039
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00006E44 File Offset: 0x00005044
		protected override string OnGetLogFormat()
		{
			object[] array = new object[4];
			array[0] = "Peer most-used weapon change for peer: ";
			int num = 1;
			NetworkCommunicator peer = this.Peer;
			array[num] = ((peer != null) ? peer.UserName : null) ?? "NULL";
			array[2] = " weapon class: ";
			array[3] = this.WeaponClass;
			return string.Concat(array);
		}
	}
}
