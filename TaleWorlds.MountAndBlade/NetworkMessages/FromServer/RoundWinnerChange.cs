using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000072 RID: 114
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RoundWinnerChange : GameNetworkMessage
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00007873 File Offset: 0x00005A73
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x0000787B File Offset: 0x00005A7B
		public BattleSideEnum RoundWinner { get; private set; }

		// Token: 0x060003FC RID: 1020 RVA: 0x00007884 File Offset: 0x00005A84
		public RoundWinnerChange(BattleSideEnum roundWinner)
		{
			this.RoundWinner = roundWinner;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00007893 File Offset: 0x00005A93
		public RoundWinnerChange()
		{
			this.RoundWinner = BattleSideEnum.None;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000078A4 File Offset: 0x00005AA4
		protected override bool OnRead()
		{
			bool flag = true;
			this.RoundWinner = (BattleSideEnum)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000078C6 File Offset: 0x00005AC6
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.RoundWinner, CompressionMission.TeamSideCompressionInfo);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000078D8 File Offset: 0x00005AD8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000078E0 File Offset: 0x00005AE0
		protected override string OnGetLogFormat()
		{
			return "Change round winner to: " + this.RoundWinner.ToString();
		}
	}
}
