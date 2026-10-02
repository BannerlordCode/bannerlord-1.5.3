using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200008A RID: 138
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class CreateMissionObject : GameNetworkMessage
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00009D2D File Offset: 0x00007F2D
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x00009D35 File Offset: 0x00007F35
		public MissionObjectId ObjectId { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x00009D3E File Offset: 0x00007F3E
		// (set) Token: 0x06000560 RID: 1376 RVA: 0x00009D46 File Offset: 0x00007F46
		public string Prefab { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00009D4F File Offset: 0x00007F4F
		// (set) Token: 0x06000562 RID: 1378 RVA: 0x00009D57 File Offset: 0x00007F57
		public MatrixFrame Frame { get; private set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00009D60 File Offset: 0x00007F60
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x00009D68 File Offset: 0x00007F68
		public List<MissionObjectId> ChildObjectIds { get; private set; }

		// Token: 0x06000565 RID: 1381 RVA: 0x00009D71 File Offset: 0x00007F71
		public CreateMissionObject(MissionObjectId objectId, string prefab, MatrixFrame frame, List<MissionObjectId> childObjectIds)
		{
			this.ObjectId = objectId;
			this.Prefab = prefab;
			this.Frame = frame;
			this.ChildObjectIds = childObjectIds;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00009D96 File Offset: 0x00007F96
		public CreateMissionObject()
		{
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00009DA0 File Offset: 0x00007FA0
		protected override bool OnRead()
		{
			bool flag = true;
			this.ObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Prefab = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Frame = GameNetworkMessage.ReadMatrixFrameFromPacket(ref flag);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.EntityChildCountCompressionInfo, ref flag);
			if (flag)
			{
				this.ChildObjectIds = new List<MissionObjectId>(num);
				for (int i = 0; i < num; i++)
				{
					if (flag)
					{
						this.ChildObjectIds.Add(GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag));
					}
				}
			}
			return flag;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00009E14 File Offset: 0x00008014
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.ObjectId);
			GameNetworkMessage.WriteStringToPacket(this.Prefab);
			GameNetworkMessage.WriteMatrixFrameToPacket(this.Frame);
			GameNetworkMessage.WriteIntToPacket(this.ChildObjectIds.Count, CompressionBasic.EntityChildCountCompressionInfo);
			foreach (MissionObjectId missionObjectId in this.ChildObjectIds)
			{
				GameNetworkMessage.WriteMissionObjectIdToPacket(missionObjectId);
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00009E9C File Offset: 0x0000809C
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00009EA4 File Offset: 0x000080A4
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Create a MissionObject with index: ", this.ObjectId, " from prefab: ", this.Prefab, " at frame: ", this.Frame });
		}
	}
}
