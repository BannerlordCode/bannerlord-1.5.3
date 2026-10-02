using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D0 RID: 208
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SyncObjectHitpoints : GameNetworkMessage
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0000E829 File Offset: 0x0000CA29
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x0000E831 File Offset: 0x0000CA31
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0000E83A File Offset: 0x0000CA3A
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x0000E842 File Offset: 0x0000CA42
		public float Hitpoints { get; private set; }

		// Token: 0x0600088F RID: 2191 RVA: 0x0000E84B File Offset: 0x0000CA4B
		public SyncObjectHitpoints(MissionObjectId missionObjectId, float hitpoints)
		{
			this.MissionObjectId = missionObjectId;
			this.Hitpoints = hitpoints;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0000E861 File Offset: 0x0000CA61
		public SyncObjectHitpoints()
		{
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0000E86C File Offset: 0x0000CA6C
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Hitpoints = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.UsableGameObjectHealthCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0000E89B File Offset: 0x0000CA9B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteFloatToPacket(MathF.Max(this.Hitpoints, 0f), CompressionMission.UsableGameObjectHealthCompressionInfo);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0000E8C2 File Offset: 0x0000CAC2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0000E8CA File Offset: 0x0000CACA
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Synchronize HitPoints: ", this.Hitpoints, " of MissionObject with Id: ", this.MissionObjectId });
		}
	}
}
