using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BC RID: 188
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSiegeTowerGateState : GameNetworkMessage
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0000D075 File Offset: 0x0000B275
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x0000D07D File Offset: 0x0000B27D
		public MissionObjectId SiegeTowerId { get; private set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0000D086 File Offset: 0x0000B286
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0000D08E File Offset: 0x0000B28E
		public SiegeTower.GateState State { get; private set; }

		// Token: 0x06000791 RID: 1937 RVA: 0x0000D097 File Offset: 0x0000B297
		public SetSiegeTowerGateState(MissionObjectId siegeTowerId, SiegeTower.GateState state)
		{
			this.SiegeTowerId = siegeTowerId;
			this.State = state;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0000D0AD File Offset: 0x0000B2AD
		public SetSiegeTowerGateState()
		{
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeTowerId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.State = (SiegeTower.GateState)GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeTowerGateStateCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0000D0E7 File Offset: 0x0000B2E7
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeTowerId);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.SiegeTowerGateStateCompressionInfo);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0000D104 File Offset: 0x0000B304
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0000D10C File Offset: 0x0000B30C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set SiegeTower State to: ", this.State, " on SiegeTower with ID: ", this.SiegeTowerId });
		}
	}
}
