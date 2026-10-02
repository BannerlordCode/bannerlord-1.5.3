using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000055 RID: 85
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class InitializeLobbyPeer : GameNetworkMessage
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00005AC4 File Offset: 0x00003CC4
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x00005ACC File Offset: 0x00003CCC
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00005AD5 File Offset: 0x00003CD5
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00005ADD File Offset: 0x00003CDD
		public PlayerId ProvidedId { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00005AE6 File Offset: 0x00003CE6
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x00005AEE File Offset: 0x00003CEE
		public string BannerCode { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00005AF7 File Offset: 0x00003CF7
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x00005AFF File Offset: 0x00003CFF
		public BodyProperties BodyProperties { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x00005B08 File Offset: 0x00003D08
		// (set) Token: 0x060002EA RID: 746 RVA: 0x00005B10 File Offset: 0x00003D10
		public int ChosenBadgeIndex { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00005B19 File Offset: 0x00003D19
		// (set) Token: 0x060002EC RID: 748 RVA: 0x00005B21 File Offset: 0x00003D21
		public int ForcedAvatarIndex { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00005B2A File Offset: 0x00003D2A
		// (set) Token: 0x060002EE RID: 750 RVA: 0x00005B32 File Offset: 0x00003D32
		public bool IsFemale { get; private set; }

		// Token: 0x060002EF RID: 751 RVA: 0x00005B3C File Offset: 0x00003D3C
		public InitializeLobbyPeer(NetworkCommunicator peer, VirtualPlayer virtualPlayer, int forcedAvatarIndex)
		{
			this.Peer = peer;
			this.ProvidedId = virtualPlayer.Id;
			this.BannerCode = ((virtualPlayer.BannerCode != null) ? virtualPlayer.BannerCode : string.Empty);
			this.BodyProperties = virtualPlayer.BodyProperties;
			this.ChosenBadgeIndex = virtualPlayer.ChosenBadgeIndex;
			this.IsFemale = virtualPlayer.IsFemale;
			this.ForcedAvatarIndex = forcedAvatarIndex;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00005BA8 File Offset: 0x00003DA8
		public InitializeLobbyPeer()
		{
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00005BB0 File Offset: 0x00003DB0
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			ulong num = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num2 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num3 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num4 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			string text = GameNetworkMessage.ReadStringFromPacket(ref flag);
			if (flag)
			{
				this.ProvidedId = new PlayerId(num, num2, num3, num4);
				BodyProperties bodyProperties;
				if (BodyProperties.FromString(text, out bodyProperties))
				{
					this.BodyProperties = bodyProperties;
				}
				else
				{
					flag = false;
				}
			}
			this.ChosenBadgeIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerChosenBadgeCompressionInfo, ref flag);
			this.ForcedAvatarIndex = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.ForcedAvatarIndexCompressionInfo, ref flag);
			this.IsFemale = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00005C74 File Offset: 0x00003E74
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteUlongToPacket(this.ProvidedId.Part1, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.ProvidedId.Part2, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.ProvidedId.Part3, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.ProvidedId.Part4, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
			GameNetworkMessage.WriteStringToPacket(this.BodyProperties.ToString());
			GameNetworkMessage.WriteIntToPacket(this.ChosenBadgeIndex, CompressionBasic.PlayerChosenBadgeCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.ForcedAvatarIndex, CompressionBasic.ForcedAvatarIndexCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.IsFemale);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00005D3B File Offset: 0x00003F3B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00005D3F File Offset: 0x00003F3F
		protected override string OnGetLogFormat()
		{
			return "Initialize LobbyPeer from Peer: " + this.Peer.UserName;
		}
	}
}
