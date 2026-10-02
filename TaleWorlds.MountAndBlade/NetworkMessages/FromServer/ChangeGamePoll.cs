using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000053 RID: 83
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ChangeGamePoll : GameNetworkMessage
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002CF RID: 719 RVA: 0x000058F5 File Offset: 0x00003AF5
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x000058FD File Offset: 0x00003AFD
		public string GameType { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x00005906 File Offset: 0x00003B06
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0000590E File Offset: 0x00003B0E
		public string Map { get; private set; }

		// Token: 0x060002D3 RID: 723 RVA: 0x00005917 File Offset: 0x00003B17
		public ChangeGamePoll(string gameType, string map)
		{
			this.GameType = gameType;
			this.Map = map;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000592D File Offset: 0x00003B2D
		public ChangeGamePoll()
		{
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00005938 File Offset: 0x00003B38
		protected override bool OnRead()
		{
			bool flag = true;
			this.GameType = GameNetworkMessage.ReadStringFromPacket(ref flag);
			this.Map = GameNetworkMessage.ReadStringFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00005962 File Offset: 0x00003B62
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.GameType);
			GameNetworkMessage.WriteStringToPacket(this.Map);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000597A File Offset: 0x00003B7A
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00005982 File Offset: 0x00003B82
		protected override string OnGetLogFormat()
		{
			return "Poll started: Change Map to: " + this.Map + " and GameType to: " + this.GameType;
		}
	}
}
