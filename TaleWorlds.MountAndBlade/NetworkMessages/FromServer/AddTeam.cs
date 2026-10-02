using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000076 RID: 118
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class AddTeam : GameNetworkMessage
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00007C99 File Offset: 0x00005E99
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x00007CA1 File Offset: 0x00005EA1
		public int TeamIndex { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00007CAA File Offset: 0x00005EAA
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x00007CB2 File Offset: 0x00005EB2
		public BattleSideEnum Side { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00007CBB File Offset: 0x00005EBB
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x00007CC3 File Offset: 0x00005EC3
		public uint Color { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00007CCC File Offset: 0x00005ECC
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x00007CD4 File Offset: 0x00005ED4
		public uint Color2 { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00007CDD File Offset: 0x00005EDD
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00007CE5 File Offset: 0x00005EE5
		public string BannerCode { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00007CEE File Offset: 0x00005EEE
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x00007CF6 File Offset: 0x00005EF6
		public bool IsPlayerGeneral { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x00007CFF File Offset: 0x00005EFF
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x00007D07 File Offset: 0x00005F07
		public bool IsPlayerSergeant { get; private set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00007D10 File Offset: 0x00005F10
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x00007D18 File Offset: 0x00005F18
		public bool IsSpectatorTeam { get; private set; }

		// Token: 0x06000436 RID: 1078 RVA: 0x00007D24 File Offset: 0x00005F24
		public AddTeam(int teamIndex, BattleSideEnum side, uint color, uint color2, string bannerCode, bool isPlayerGeneral, bool isPlayerSergeant, bool isSpectatorTeam = false)
		{
			this.TeamIndex = teamIndex;
			this.Side = side;
			this.Color = color;
			this.Color2 = color2;
			this.BannerCode = bannerCode;
			this.IsPlayerGeneral = isPlayerGeneral;
			this.IsPlayerSergeant = isPlayerSergeant;
			this.IsSpectatorTeam = isSpectatorTeam;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00007D74 File Offset: 0x00005F74
		public AddTeam()
		{
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00007D7C File Offset: 0x00005F7C
		protected override bool OnRead()
		{
			bool flag = true;
			this.TeamIndex = GameNetworkMessage.ReadTeamIndexFromPacket(ref flag);
			this.Side = (BattleSideEnum)GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamSideCompressionInfo, ref flag);
			this.Color = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			this.Color2 = GameNetworkMessage.ReadUintFromPacket(CompressionBasic.ColorCompressionInfo, ref flag);
			this.BannerCode = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.IsPlayerGeneral = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsPlayerSergeant = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsSpectatorTeam = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00007E04 File Offset: 0x00006004
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteTeamIndexToPacket(this.TeamIndex);
			GameNetworkMessage.WriteIntToPacket((int)this.Side, CompressionMission.TeamSideCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.Color, CompressionBasic.ColorCompressionInfo);
			GameNetworkMessage.WriteUintToPacket(this.Color2, CompressionBasic.ColorCompressionInfo);
			GameNetworkMessage.WriteStringToPacket(this.BannerCode);
			GameNetworkMessage.WriteBoolToPacket(this.IsPlayerGeneral);
			GameNetworkMessage.WriteBoolToPacket(this.IsPlayerSergeant);
			GameNetworkMessage.WriteBoolToPacket(this.IsSpectatorTeam);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00007E78 File Offset: 0x00006078
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Mission;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00007E80 File Offset: 0x00006080
		protected override string OnGetLogFormat()
		{
			return "Add team with side: " + this.Side;
		}
	}
}
