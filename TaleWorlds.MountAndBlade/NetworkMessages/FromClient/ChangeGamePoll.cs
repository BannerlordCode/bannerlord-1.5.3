using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000016 RID: 22
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class ChangeGamePoll : GameNetworkMessage
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00002F27 File Offset: 0x00001127
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00002F2F File Offset: 0x0000112F
		public string GameType { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00002F38 File Offset: 0x00001138
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00002F40 File Offset: 0x00001140
		public string Map { get; private set; }

		// Token: 0x060000A1 RID: 161 RVA: 0x00002F49 File Offset: 0x00001149
		public ChangeGamePoll(string gameType, string map)
		{
			this.GameType = gameType;
			this.Map = map;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002F5F File Offset: 0x0000115F
		public ChangeGamePoll()
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002F68 File Offset: 0x00001168
		protected override bool OnRead()
		{
			bool flag = true;
			this.GameType = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Map = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00002F92 File Offset: 0x00001192
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.GameType);
			GameNetworkMessage.WriteStringToPacket(this.Map);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00002FAA File Offset: 0x000011AA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00002FB2 File Offset: 0x000011B2
		protected override string OnGetLogFormat()
		{
			return "Poll Requested: Change Map to: " + this.Map + " and GameType to: " + this.GameType;
		}
	}
}
