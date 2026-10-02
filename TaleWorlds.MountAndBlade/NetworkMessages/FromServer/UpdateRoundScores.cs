using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D3 RID: 211
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UpdateRoundScores : GameNetworkMessage
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0000EA7D File Offset: 0x0000CC7D
		// (set) Token: 0x060008AA RID: 2218 RVA: 0x0000EA85 File Offset: 0x0000CC85
		public int AttackerTeamScore { get; private set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0000EA8E File Offset: 0x0000CC8E
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x0000EA96 File Offset: 0x0000CC96
		public int DefenderTeamScore { get; private set; }

		// Token: 0x060008AD RID: 2221 RVA: 0x0000EA9F File Offset: 0x0000CC9F
		public UpdateRoundScores(int attackerTeamScore, int defenderTeamScore)
		{
			this.AttackerTeamScore = attackerTeamScore;
			this.DefenderTeamScore = defenderTeamScore;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000EAB5 File Offset: 0x0000CCB5
		public UpdateRoundScores()
		{
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
		protected override bool OnRead()
		{
			bool flag = true;
			this.AttackerTeamScore = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamScoreCompressionInfo, ref flag);
			this.DefenderTeamScore = GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamScoreCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.AttackerTeamScore, CompressionMission.TeamScoreCompressionInfo);
			GameNetworkMessage.WriteIntToPacket(this.DefenderTeamScore, CompressionMission.TeamScoreCompressionInfo);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0000EB16 File Offset: 0x0000CD16
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission | MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0000EB1E File Offset: 0x0000CD1E
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Update round score. Attackers: ", this.AttackerTeamScore, ", defenders: ", this.DefenderTeamScore });
		}
	}
}
