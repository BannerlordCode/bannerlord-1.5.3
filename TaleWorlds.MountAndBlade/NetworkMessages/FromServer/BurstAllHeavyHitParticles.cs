using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200007F RID: 127
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class BurstAllHeavyHitParticles : GameNetworkMessage
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x000087E9 File Offset: 0x000069E9
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x000087F1 File Offset: 0x000069F1
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x0600049A RID: 1178 RVA: 0x000087FA File Offset: 0x000069FA
		public BurstAllHeavyHitParticles(MissionObjectId missionObjectId)
		{
			this.MissionObjectId = missionObjectId;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00008809 File Offset: 0x00006A09
		public BurstAllHeavyHitParticles()
		{
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00008814 File Offset: 0x00006A14
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00008831 File Offset: 0x00006A31
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000883E File Offset: 0x00006A3E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjectsDetailed | MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00008846 File Offset: 0x00006A46
		protected override string OnGetLogFormat()
		{
			return "Bursting all heavy-hit particles for the DestructableComponent of MissionObject with Id: " + this.MissionObjectId;
		}
	}
}
