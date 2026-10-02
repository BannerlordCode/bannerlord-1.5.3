using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BD RID: 189
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSiegeTowerHasArrivedAtTarget : GameNetworkMessage
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0000D145 File Offset: 0x0000B345
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x0000D14D File Offset: 0x0000B34D
		public MissionObjectId SiegeTowerId { get; private set; }

		// Token: 0x06000799 RID: 1945 RVA: 0x0000D156 File Offset: 0x0000B356
		public SetSiegeTowerHasArrivedAtTarget(MissionObjectId siegeTowerId)
		{
			this.SiegeTowerId = siegeTowerId;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0000D165 File Offset: 0x0000B365
		public SetSiegeTowerHasArrivedAtTarget()
		{
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0000D170 File Offset: 0x0000B370
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeTowerId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0000D18D File Offset: 0x0000B38D
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeTowerId);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0000D19A File Offset: 0x0000B39A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeapons;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0000D1A2 File Offset: 0x0000B3A2
		protected override string OnGetLogFormat()
		{
			return "SiegeTower with ID: " + this.SiegeTowerId + " has arrived at its target.";
		}
	}
}
