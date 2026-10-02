using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006C RID: 108
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncPerksForCurrentlySelectedTroop : GameNetworkMessage
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003BF RID: 959 RVA: 0x000072E0 File Offset: 0x000054E0
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x000072E8 File Offset: 0x000054E8
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x000072F1 File Offset: 0x000054F1
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x000072F9 File Offset: 0x000054F9
		public int[] PerkIndices { get; private set; }

		// Token: 0x060003C3 RID: 963 RVA: 0x00007302 File Offset: 0x00005502
		public SyncPerksForCurrentlySelectedTroop()
		{
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000730A File Offset: 0x0000550A
		public SyncPerksForCurrentlySelectedTroop(NetworkCommunicator peer, int[] perkIndices)
		{
			this.Peer = peer;
			this.PerkIndices = perkIndices;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00007320 File Offset: 0x00005520
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			for (int i = 0; i < 3; i++)
			{
				GameNetworkMessage.WriteIntToPacket(this.PerkIndices[i], CompressionMission.PerkIndexCompressionInfo);
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00007358 File Offset: 0x00005558
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.PerkIndices = new int[3];
			for (int i = 0; i < 3; i++)
			{
				this.PerkIndices[i] = GameNetworkMessage.ReadIntFromPacket(CompressionMission.PerkIndexCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000073A2 File Offset: 0x000055A2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000073AC File Offset: 0x000055AC
		protected override string OnGetLogFormat()
		{
			string text = "";
			for (int i = 0; i < 3; i++)
			{
				text += string.Format("[{0}]", this.PerkIndices[i]);
			}
			return string.Concat(new string[]
			{
				"Selected perks for ",
				this.Peer.UserName,
				" has been updated as ",
				text,
				"."
			});
		}
	}
}
