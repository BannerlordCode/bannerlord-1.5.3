using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000032 RID: 50
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class RequestUseObject : GameNetworkMessage
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00003EF7 File Offset: 0x000020F7
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00003EFF File Offset: 0x000020FF
		public MissionObjectId UsableMissionObjectId { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00003F08 File Offset: 0x00002108
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00003F10 File Offset: 0x00002110
		public int UsedObjectPreferenceIndex { get; private set; }

		// Token: 0x0600018D RID: 397 RVA: 0x00003F19 File Offset: 0x00002119
		public RequestUseObject(MissionObjectId usableMissionObjectId, int usedObjectPreferenceIndex)
		{
			this.UsableMissionObjectId = usableMissionObjectId;
			this.UsedObjectPreferenceIndex = usedObjectPreferenceIndex;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00003F2F File Offset: 0x0000212F
		public RequestUseObject()
		{
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00003F38 File Offset: 0x00002138
		protected override bool OnRead()
		{
			bool flag = true;
			this.UsableMissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.UsedObjectPreferenceIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.WieldSlotCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00003F67 File Offset: 0x00002167
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.UsableMissionObjectId);
			GameNetworkMessage.WriteIntToPacket(this.UsedObjectPreferenceIndex, CompressionMission.WieldSlotCompressionInfo);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00003F84 File Offset: 0x00002184
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00003F8C File Offset: 0x0000218C
		protected override string OnGetLogFormat()
		{
			return "Request to use UsableMissionObject with ID: " + this.UsableMissionObjectId;
		}
	}
}
