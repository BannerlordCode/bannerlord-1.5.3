using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000C0 RID: 192
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetUsableMissionObjectIsDeactivated : GameNetworkMessage
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0000D331 File Offset: 0x0000B531
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0000D339 File Offset: 0x0000B539
		public MissionObjectId UsableGameObjectId { get; private set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0000D342 File Offset: 0x0000B542
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0000D34A File Offset: 0x0000B54A
		public bool IsDeactivated { get; private set; }

		// Token: 0x060007B7 RID: 1975 RVA: 0x0000D353 File Offset: 0x0000B553
		public SetUsableMissionObjectIsDeactivated(MissionObjectId usableGameObjectId, bool isDeactivated)
		{
			this.UsableGameObjectId = usableGameObjectId;
			this.IsDeactivated = isDeactivated;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0000D369 File Offset: 0x0000B569
		public SetUsableMissionObjectIsDeactivated()
		{
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0000D374 File Offset: 0x0000B574
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableGameObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.IsDeactivated = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0000D39E File Offset: 0x0000B59E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableGameObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.IsDeactivated);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0000D3B6 File Offset: 0x0000B5B6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set IsDeactivated: ",
				this.IsDeactivated ? "True" : "False",
				" on UsableMissionObject with ID: ",
				this.UsableGameObjectId
			});
		}
	}
}
