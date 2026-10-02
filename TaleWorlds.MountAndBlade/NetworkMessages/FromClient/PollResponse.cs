using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200001B RID: 27
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class PollResponse : GameNetworkMessage
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00003272 File Offset: 0x00001472
		// (set) Token: 0x060000CE RID: 206 RVA: 0x0000327A File Offset: 0x0000147A
		public bool Accepted { get; private set; }

		// Token: 0x060000CF RID: 207 RVA: 0x00003283 File Offset: 0x00001483
		public PollResponse(bool accepted)
		{
			this.Accepted = accepted;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003292 File Offset: 0x00001492
		public PollResponse()
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000329C File Offset: 0x0000149C
		protected override bool OnRead()
		{
			bool flag = true;
			this.Accepted = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000032B9 File Offset: 0x000014B9
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.Accepted);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000032C6 File Offset: 0x000014C6
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000032CE File Offset: 0x000014CE
		protected override string OnGetLogFormat()
		{
			return "Receiving poll response: " + (this.Accepted ? "Accepted." : "Not accepted.");
		}
	}
}
