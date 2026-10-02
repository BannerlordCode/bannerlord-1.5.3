using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000CE RID: 206
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SynchronizeMissionObject : GameNetworkMessage
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0000E515 File Offset: 0x0000C715
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x0000E51D File Offset: 0x0000C71D
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0000E526 File Offset: 0x0000C726
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x0000E52E File Offset: 0x0000C72E
		public int RecordTypeIndex { get; private set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0000E537 File Offset: 0x0000C737
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x0000E53F File Offset: 0x0000C73F
		public ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> RecordPair { get; private set; }

		// Token: 0x06000873 RID: 2163 RVA: 0x0000E548 File Offset: 0x0000C748
		public SynchronizeMissionObject(SynchedMissionObject synchedMissionObject)
		{
			this._synchedMissionObject = synchedMissionObject;
			this.MissionObjectId = synchedMissionObject.Id;
			this.RecordTypeIndex = GameNetwork.GetSynchedMissionObjectReadableRecordIndexFromType(synchedMissionObject.GetType());
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0000E574 File Offset: 0x0000C774
		public SynchronizeMissionObject()
		{
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0000E57C File Offset: 0x0000C77C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteIntToPacket(this.RecordTypeIndex, CompressionMission.SynchedMissionObjectReadableRecordTypeIndex);
			this._synchedMissionObject.WriteToNetwork();
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0000E5A4 File Offset: 0x0000C7A4
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.RecordTypeIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SynchedMissionObjectReadableRecordTypeIndex, ref flag);
			this.RecordPair = BaseSynchedMissionObjectReadableRecord.CreateFromNetworkWithTypeIndex(this.RecordTypeIndex);
			return flag;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0000E5E4 File Offset: 0x0000C7E4
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0000E5EC File Offset: 0x0000C7EC
		protected override string OnGetLogFormat()
		{
			return "Synchronize MissionObject with Id: " + this.MissionObjectId;
		}

		// Token: 0x040001EA RID: 490
		private SynchedMissionObject _synchedMissionObject;
	}
}
