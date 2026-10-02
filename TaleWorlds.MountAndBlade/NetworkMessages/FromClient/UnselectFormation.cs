using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200003B RID: 59
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class UnselectFormation : GameNetworkMessage
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000433F File Offset: 0x0000253F
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00004347 File Offset: 0x00002547
		public int FormationIndex { get; private set; }

		// Token: 0x060001D5 RID: 469 RVA: 0x00004350 File Offset: 0x00002550
		public UnselectFormation(int formationIndex)
		{
			this.FormationIndex = formationIndex;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000435F File Offset: 0x0000255F
		public UnselectFormation()
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00004368 File Offset: 0x00002568
		protected override bool OnRead()
		{
			bool flag = true;
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000438A File Offset: 0x0000258A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000439C File Offset: 0x0000259C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Formations;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000043A1 File Offset: 0x000025A1
		protected override string OnGetLogFormat()
		{
			return "Deselect Formation with index: " + this.FormationIndex;
		}
	}
}
