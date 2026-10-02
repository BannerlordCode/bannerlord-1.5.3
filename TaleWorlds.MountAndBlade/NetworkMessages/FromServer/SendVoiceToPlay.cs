using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000DF RID: 223
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SendVoiceToPlay : GameNetworkMessage
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x0000F43B File Offset: 0x0000D63B
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x0000F443 File Offset: 0x0000D643
		public NetworkCommunicator Peer { get; private set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0000F44C File Offset: 0x0000D64C
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x0000F454 File Offset: 0x0000D654
		public byte[] Buffer { get; private set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0000F45D File Offset: 0x0000D65D
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x0000F465 File Offset: 0x0000D665
		public int BufferLength { get; private set; }

		// Token: 0x0600091F RID: 2335 RVA: 0x0000F46E File Offset: 0x0000D66E
		public SendVoiceToPlay()
		{
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0000F476 File Offset: 0x0000D676
		public SendVoiceToPlay(NetworkCommunicator peer, byte[] buffer, int bufferLength)
		{
			this.Peer = peer;
			this.Buffer = buffer;
			this.BufferLength = bufferLength;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0000F493 File Offset: 0x0000D693
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteNetworkPeerReferenceToPacket(this.Peer);
			GameNetworkMessage.WriteByteArrayToPacket(this.Buffer, 0, this.BufferLength);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
		protected override bool OnRead()
		{
			bool flag = true;
			this.Peer = GameNetworkMessage.ReadNetworkPeerReferenceFromPacket(ref flag, false);
			this.Buffer = new byte[1440];
			this.BufferLength = GameNetworkMessage.ReadByteArrayFromPacket(this.Buffer, 0, 1440, ref flag);
			return flag;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0000F4FB File Offset: 0x0000D6FB
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0000F4FF File Offset: 0x0000D6FF
		protected override string OnGetLogFormat()
		{
			return string.Empty;
		}
	}
}
