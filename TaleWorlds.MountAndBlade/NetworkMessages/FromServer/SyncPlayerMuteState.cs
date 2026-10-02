using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006D RID: 109
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncPlayerMuteState : GameNetworkMessage
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0000741E File Offset: 0x0000561E
		// (set) Token: 0x060003CA RID: 970 RVA: 0x00007426 File Offset: 0x00005626
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0000742F File Offset: 0x0000562F
		// (set) Token: 0x060003CC RID: 972 RVA: 0x00007437 File Offset: 0x00005637
		public bool IsMuted { get; private set; }

		// Token: 0x060003CD RID: 973 RVA: 0x00007440 File Offset: 0x00005640
		public SyncPlayerMuteState()
		{
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00007448 File Offset: 0x00005648
		public SyncPlayerMuteState(PlayerId playerId, bool isMuted)
		{
			this.PlayerId = playerId;
			this.IsMuted = isMuted;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00007460 File Offset: 0x00005660
		protected override bool OnRead()
		{
			bool flag = true;
			ulong num = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num2 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num3 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			ulong num4 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref flag);
			if (flag)
			{
				this.PlayerId = new PlayerId(num, num2, num3, num4);
			}
			this.IsMuted = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000074C8 File Offset: 0x000056C8
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteUlongToPacket(this.PlayerId.Part1, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.PlayerId.Part2, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.PlayerId.Part3, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(this.PlayerId.Part4, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.IsMuted);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00007540 File Offset: 0x00005740
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00007548 File Offset: 0x00005748
		protected override string OnGetLogFormat()
		{
			return string.Format("SyncPlayerMuteState Player:{0}, IsMuted:{1}", this.PlayerId, this.IsMuted);
		}
	}
}
