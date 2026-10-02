using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AB RID: 171
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectAnimationPaused : GameNetworkMessage
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0000C016 File Offset: 0x0000A216
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x0000C01E File Offset: 0x0000A21E
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0000C027 File Offset: 0x0000A227
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x0000C02F File Offset: 0x0000A22F
		public bool IsPaused { get; private set; }

		// Token: 0x060006DF RID: 1759 RVA: 0x0000C038 File Offset: 0x0000A238
		public SetMissionObjectAnimationPaused(MissionObjectId missionObjectId, bool isPaused)
		{
			this.MissionObjectId = missionObjectId;
			this.IsPaused = isPaused;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0000C04E File Offset: 0x0000A24E
		public SetMissionObjectAnimationPaused()
		{
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0000C058 File Offset: 0x0000A258
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.IsPaused = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0000C082 File Offset: 0x0000A282
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.IsPaused);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0000C09A File Offset: 0x0000A29A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set animation to be: ",
				this.IsPaused ? "Paused" : "Not paused",
				" on MissionObject with ID: ",
				this.MissionObjectId
			});
		}
	}
}
