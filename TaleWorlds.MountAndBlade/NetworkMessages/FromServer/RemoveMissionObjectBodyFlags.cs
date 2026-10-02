using System;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200009B RID: 155
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class RemoveMissionObjectBodyFlags : GameNetworkMessage
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0000AF29 File Offset: 0x00009129
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x0000AF31 File Offset: 0x00009131
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0000AF3A File Offset: 0x0000913A
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x0000AF42 File Offset: 0x00009142
		public BodyFlags BodyFlags { get; private set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0000AF4B File Offset: 0x0000914B
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x0000AF53 File Offset: 0x00009153
		public bool ApplyToChildren { get; private set; }

		// Token: 0x06000624 RID: 1572 RVA: 0x0000AF5C File Offset: 0x0000915C
		public RemoveMissionObjectBodyFlags(MissionObjectId missionObjectId, BodyFlags bodyFlags, bool applyToChildren)
		{
			this.MissionObjectId = missionObjectId;
			this.BodyFlags = bodyFlags;
			this.ApplyToChildren = applyToChildren;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0000AF79 File Offset: 0x00009179
		public RemoveMissionObjectBodyFlags()
		{
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0000AF84 File Offset: 0x00009184
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.BodyFlags = (BodyFlags)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.FlagsCompressionInfo, ref flag);
			this.ApplyToChildren = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0000AFC0 File Offset: 0x000091C0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket((int)this.BodyFlags, CompressionBasic.FlagsCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.ApplyToChildren);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0000AFE8 File Offset: 0x000091E8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0000AFF0 File Offset: 0x000091F0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Remove bodyflags: ",
				this.BodyFlags,
				" from MissionObject with ID: ",
				this.MissionObjectId,
				this.ApplyToChildren ? "" : " and from all of its children."
			});
		}
	}
}
