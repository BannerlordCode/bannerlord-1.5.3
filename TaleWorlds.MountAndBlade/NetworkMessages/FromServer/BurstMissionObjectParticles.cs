using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000080 RID: 128
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BurstMissionObjectParticles : GameNetworkMessage
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0000885D File Offset: 0x00006A5D
		// (set) Token: 0x060004A1 RID: 1185 RVA: 0x00008865 File Offset: 0x00006A65
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x0000886E File Offset: 0x00006A6E
		// (set) Token: 0x060004A3 RID: 1187 RVA: 0x00008876 File Offset: 0x00006A76
		public bool DoChildren { get; private set; }

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000887F File Offset: 0x00006A7F
		public BurstMissionObjectParticles(MissionObjectId missionObjectId, bool doChildren)
		{
			this.MissionObjectId = missionObjectId;
			this.DoChildren = doChildren;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00008895 File Offset: 0x00006A95
		public BurstMissionObjectParticles()
		{
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000088A0 File Offset: 0x00006AA0
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.DoChildren = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000088CA File Offset: 0x00006ACA
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.DoChildren);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000088E2 File Offset: 0x00006AE2
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.Particles;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000088EA File Offset: 0x00006AEA
		protected override string OnGetLogFormat()
		{
			return "Burst MissionObject particles on MissionObject with ID: " + this.MissionObjectId;
		}
	}
}
