using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000D1 RID: 209
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class TeamSetIsEnemyOf : GameNetworkMessage
	{
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0000E903 File Offset: 0x0000CB03
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x0000E90B File Offset: 0x0000CB0B
		public int Team1Index { get; private set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x0000E914 File Offset: 0x0000CB14
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x0000E91C File Offset: 0x0000CB1C
		public int Team2Index { get; private set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0000E925 File Offset: 0x0000CB25
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x0000E92D File Offset: 0x0000CB2D
		public bool IsEnemyOf { get; private set; }

		// Token: 0x0600089B RID: 2203 RVA: 0x0000E936 File Offset: 0x0000CB36
		public TeamSetIsEnemyOf(int team1Index, int team2Index, bool isEnemyOf)
		{
			this.Team1Index = team1Index;
			this.Team2Index = team2Index;
			this.IsEnemyOf = isEnemyOf;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0000E953 File Offset: 0x0000CB53
		public TeamSetIsEnemyOf()
		{
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0000E95B File Offset: 0x0000CB5B
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteTeamIndexToPacket(this.Team1Index);
			GameNetworkMessage.WriteTeamIndexToPacket(this.Team2Index);
			GameNetworkMessage.WriteBoolToPacket(this.IsEnemyOf);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0000E980 File Offset: 0x0000CB80
		protected override bool OnRead()
		{
			bool flag = true;
			this.Team1Index = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.Team2Index = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.IsEnemyOf = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0000E9B7 File Offset: 0x0000CBB7
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0000E9C0 File Offset: 0x0000CBC0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				this.Team1Index,
				" is now ",
				this.IsEnemyOf ? "" : "not an ",
				"enemy of ",
				this.Team2Index
			});
		}
	}
}
