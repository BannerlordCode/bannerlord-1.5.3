using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BE RID: 190
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSpawnedFormationCount : GameNetworkMessage
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x0000D1BE File Offset: 0x0000B3BE
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x0000D1C6 File Offset: 0x0000B3C6
		public int NumOfFormationsTeamOne { get; private set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0000D1CF File Offset: 0x0000B3CF
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x0000D1D7 File Offset: 0x0000B3D7
		public int NumOfFormationsTeamTwo { get; private set; }

		// Token: 0x060007A3 RID: 1955 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		public SetSpawnedFormationCount(int numFormationsTeamOne, int numFormationsTeamTwo)
		{
			this.NumOfFormationsTeamOne = numFormationsTeamOne;
			this.NumOfFormationsTeamTwo = numFormationsTeamTwo;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0000D1F6 File Offset: 0x0000B3F6
		public SetSpawnedFormationCount()
		{
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0000D200 File Offset: 0x0000B400
		protected override bool OnRead()
		{
			bool flag = true;
			this.NumOfFormationsTeamOne = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.NumOfFormationsTeamTwo = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0000D234 File Offset: 0x0000B434
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.NumOfFormationsTeamOne, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.NumOfFormationsTeamTwo, CompressionMission.FormationClassCompressionInfo);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0000D256 File Offset: 0x0000B456
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0000D25A File Offset: 0x0000B45A
		protected override string OnGetLogFormat()
		{
			return "Syncing formation count";
		}
	}
}
