using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000035 RID: 53
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SelectFormation : GameNetworkMessage
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00003FD8 File Offset: 0x000021D8
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00003FE0 File Offset: 0x000021E0
		public int FormationIndex { get; private set; }

		// Token: 0x0600019F RID: 415 RVA: 0x00003FE9 File Offset: 0x000021E9
		public SelectFormation(int formationIndex)
		{
			this.FormationIndex = formationIndex;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00003FF8 File Offset: 0x000021F8
		public SelectFormation()
		{
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00004000 File Offset: 0x00002200
		protected override bool OnRead()
		{
			bool flag = true;
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00004022 File Offset: 0x00002222
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004034 File Offset: 0x00002234
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00004039 File Offset: 0x00002239
		protected override string OnGetLogFormat()
		{
			return "Select Formation with ID: " + this.FormationIndex;
		}
	}
}
