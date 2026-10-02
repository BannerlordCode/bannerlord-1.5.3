using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C1 RID: 193
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetUsableMissionObjectIsDisabledForPlayers : GameNetworkMessage
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0000D40D File Offset: 0x0000B60D
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0000D415 File Offset: 0x0000B615
		public MissionObjectId UsableGameObjectId { get; private set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0000D41E File Offset: 0x0000B61E
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0000D426 File Offset: 0x0000B626
		public bool IsDisabledForPlayers { get; private set; }

		// Token: 0x060007C1 RID: 1985 RVA: 0x0000D42F File Offset: 0x0000B62F
		public SetUsableMissionObjectIsDisabledForPlayers(MissionObjectId usableGameObjectId, bool isDisabledForPlayers)
		{
			this.UsableGameObjectId = usableGameObjectId;
			this.IsDisabledForPlayers = isDisabledForPlayers;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0000D445 File Offset: 0x0000B645
		public SetUsableMissionObjectIsDisabledForPlayers()
		{
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0000D450 File Offset: 0x0000B650
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableGameObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.IsDisabledForPlayers = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0000D47A File Offset: 0x0000B67A
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableGameObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.IsDisabledForPlayers);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0000D492 File Offset: 0x0000B692
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0000D49C File Offset: 0x0000B69C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set IsDisabled for player: ",
				this.IsDisabledForPlayers ? "True" : "False",
				" on UsableMissionObject with ID: ",
				this.UsableGameObjectId
			});
		}
	}
}
