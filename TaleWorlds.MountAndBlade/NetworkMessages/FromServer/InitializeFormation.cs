using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000093 RID: 147
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class InitializeFormation : GameNetworkMessage
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0000A8D7 File Offset: 0x00008AD7
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x0000A8DF File Offset: 0x00008ADF
		public int FormationIndex { get; private set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0000A8E8 File Offset: 0x00008AE8
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x0000A8F0 File Offset: 0x00008AF0
		public int TeamIndex { get; private set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0000A8F9 File Offset: 0x00008AF9
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x0000A901 File Offset: 0x00008B01
		public string BannerCode { get; private set; }

		// Token: 0x060005D0 RID: 1488 RVA: 0x0000A90A File Offset: 0x00008B0A
		public InitializeFormation(Formation formation, int teamIndex, string bannerCode)
		{
			this.FormationIndex = (int)formation.FormationIndex;
			this.TeamIndex = teamIndex;
			this.BannerCode = ((!string.IsNullOrEmpty(bannerCode)) ? bannerCode : string.Empty);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0000A93B File Offset: 0x00008B3B
		public InitializeFormation()
		{
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0000A944 File Offset: 0x00008B44
		protected override bool OnRead()
		{
			bool flag = true;
			this.FormationIndex = GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0000A980 File Offset: 0x00008B80
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.FormationIndex, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0000A9A8 File Offset: 0x00008BA8
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Peers;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0000A9AC File Offset: 0x00008BAC
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Initialize formation with index: ", this.FormationIndex, ", for team: ", this.TeamIndex });
		}
	}
}
