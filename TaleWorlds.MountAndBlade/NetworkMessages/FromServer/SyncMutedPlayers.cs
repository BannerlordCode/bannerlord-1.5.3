using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006B RID: 107
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncMutedPlayers : GameNetworkMessage
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000713B File Offset: 0x0000533B
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00007143 File Offset: 0x00005343
		public int MutedPlayerCount { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000714C File Offset: 0x0000534C
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00007154 File Offset: 0x00005354
		public List<PlayerId> MutedPlayerIds { get; private set; }

		// Token: 0x060003B9 RID: 953 RVA: 0x0000715D File Offset: 0x0000535D
		public SyncMutedPlayers()
		{
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00007165 File Offset: 0x00005365
		public SyncMutedPlayers(List<PlayerId> mutedPlayerIds)
		{
			this.MutedPlayerIds = mutedPlayerIds;
			List<PlayerId> mutedPlayerIds2 = this.MutedPlayerIds;
			this.MutedPlayerCount = ((mutedPlayerIds2 != null) ? mutedPlayerIds2.Count : 0);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000718C File Offset: 0x0000538C
		protected override bool OnRead()
		{
			bool flag = true;
			this.MutedPlayerIds = new List<PlayerId>();
			this.MutedPlayerCount = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.IntermissionVoterCountCompressionInfo, ref flag);
			for (int i = 0; i < this.MutedPlayerCount; i++)
			{
				ulong num = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
				ulong num2 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
				ulong num3 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
				ulong num4 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
				this.MutedPlayerIds.Add(new PlayerId(num, num2, num3, num4));
			}
			return flag;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00007218 File Offset: 0x00005418
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.MutedPlayerCount, CompressionBasic.IntermissionVoterCountCompressionInfo);
			for (int i = 0; i < this.MutedPlayerCount; i++)
			{
				GameNetworkMessage.WriteUlongToPacket(this.MutedPlayerIds[i].Part1, CompressionBasic.DebugULongNonCompressionInfo);
				GameNetworkMessage.WriteUlongToPacket(this.MutedPlayerIds[i].Part2, CompressionBasic.DebugULongNonCompressionInfo);
				GameNetworkMessage.WriteUlongToPacket(this.MutedPlayerIds[i].Part3, CompressionBasic.DebugULongNonCompressionInfo);
				GameNetworkMessage.WriteUlongToPacket(this.MutedPlayerIds[i].Part4, CompressionBasic.DebugULongNonCompressionInfo);
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000072C1 File Offset: 0x000054C1
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000072C9 File Offset: 0x000054C9
		protected override string OnGetLogFormat()
		{
			return string.Format("SyncMutedPlayers {0} muted players.", this.MutedPlayerCount);
		}
	}
}
