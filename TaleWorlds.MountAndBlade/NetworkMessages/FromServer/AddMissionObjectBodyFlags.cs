using System;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000074 RID: 116
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AddMissionObjectBodyFlags : GameNetworkMessage
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00007A5C File Offset: 0x00005C5C
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x00007A64 File Offset: 0x00005C64
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00007A6D File Offset: 0x00005C6D
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x00007A75 File Offset: 0x00005C75
		public BodyFlags BodyFlags { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00007A7E File Offset: 0x00005C7E
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x00007A86 File Offset: 0x00005C86
		public bool ApplyToChildren { get; private set; }

		// Token: 0x06000414 RID: 1044 RVA: 0x00007A8F File Offset: 0x00005C8F
		public AddMissionObjectBodyFlags(MissionObjectId missionObjectId, BodyFlags bodyFlags, bool applyToChildren)
		{
			this.MissionObjectId = missionObjectId;
			this.BodyFlags = bodyFlags;
			this.ApplyToChildren = applyToChildren;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00007AAC File Offset: 0x00005CAC
		public AddMissionObjectBodyFlags()
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00007AB4 File Offset: 0x00005CB4
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.BodyFlags = (BodyFlags)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.FlagsCompressionInfo, ref flag);
			this.ApplyToChildren = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00007AF0 File Offset: 0x00005CF0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket((int)this.BodyFlags, CompressionBasic.FlagsCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.ApplyToChildren);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00007B18 File Offset: 0x00005D18
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00007B20 File Offset: 0x00005D20
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Add bodyflags: ",
				this.BodyFlags,
				" to MissionObject with ID: ",
				this.MissionObjectId,
				this.ApplyToChildren ? "" : " and to all of its children."
			});
		}
	}
}
