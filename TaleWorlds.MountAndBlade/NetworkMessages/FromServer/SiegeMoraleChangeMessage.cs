using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000073 RID: 115
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SiegeMoraleChangeMessage : GameNetworkMessage
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000790B File Offset: 0x00005B0B
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00007913 File Offset: 0x00005B13
		public int AttackerMorale { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000791C File Offset: 0x00005B1C
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00007924 File Offset: 0x00005B24
		public int DefenderMorale { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000792D File Offset: 0x00005B2D
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x00007935 File Offset: 0x00005B35
		public int[] CapturePointRemainingMoraleGains { get; private set; }

		// Token: 0x06000408 RID: 1032 RVA: 0x0000793E File Offset: 0x00005B3E
		public SiegeMoraleChangeMessage()
		{
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00007946 File Offset: 0x00005B46
		public SiegeMoraleChangeMessage(int attackerMorale, int defenderMorale, int[] capturePointRemainingMoraleGains)
		{
			this.AttackerMorale = attackerMorale;
			this.DefenderMorale = defenderMorale;
			this.CapturePointRemainingMoraleGains = capturePointRemainingMoraleGains;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00007964 File Offset: 0x00005B64
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.AttackerMorale, CompressionMission.SiegeMoraleCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.DefenderMorale, CompressionMission.SiegeMoraleCompressionInfo);
			int[] capturePointRemainingMoraleGains = this.CapturePointRemainingMoraleGains;
			for (int i = 0; i < capturePointRemainingMoraleGains.Length; i++)
			{
				GameNetworkMessage.WriteIntToPacket(capturePointRemainingMoraleGains[i], CompressionMission.SiegeMoralePerFlagCompressionInfo);
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000079B4 File Offset: 0x00005BB4
		protected override bool OnRead()
		{
			bool flag = true;
			this.AttackerMorale = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeMoraleCompressionInfo, ref flag);
			this.DefenderMorale = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeMoraleCompressionInfo, ref flag);
			this.CapturePointRemainingMoraleGains = new int[7];
			for (int i = 0; i < this.CapturePointRemainingMoraleGains.Length; i++)
			{
				this.CapturePointRemainingMoraleGains[i] = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeMoralePerFlagCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00007A1B File Offset: 0x00005C1B
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00007A23 File Offset: 0x00005C23
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Morale synched. A: ", this.AttackerMorale, " D: ", this.DefenderMorale });
		}
	}
}
