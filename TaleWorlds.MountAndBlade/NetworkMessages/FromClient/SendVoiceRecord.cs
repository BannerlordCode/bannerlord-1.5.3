using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x0200003D RID: 61
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SendVoiceRecord : GameNetworkMessage
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00004429 File Offset: 0x00002629
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00004431 File Offset: 0x00002631
		public byte[] Buffer { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000443A File Offset: 0x0000263A
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00004442 File Offset: 0x00002642
		public int BufferLength { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000444B File Offset: 0x0000264B
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00004453 File Offset: 0x00002653
		public List<VirtualPlayer> ReceiverList { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000445C File Offset: 0x0000265C
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00004464 File Offset: 0x00002664
		public bool HasReceiverList { get; private set; }

		// Token: 0x060001EB RID: 491 RVA: 0x0000446D File Offset: 0x0000266D
		public SendVoiceRecord()
		{
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00004475 File Offset: 0x00002675
		public SendVoiceRecord(byte[] buffer, int bufferLength)
		{
			this.Buffer = buffer;
			this.BufferLength = bufferLength;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000448B File Offset: 0x0000268B
		public SendVoiceRecord(byte[] buffer, int bufferLength, List<VirtualPlayer> receiverList)
		{
			this.Buffer = buffer;
			this.BufferLength = bufferLength;
			this.ReceiverList = receiverList;
			this.HasReceiverList = true;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000044B0 File Offset: 0x000026B0
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteByteArrayToPacket(this.Buffer, 0, this.BufferLength);
			int num = 0;
			if (this.ReceiverList != null)
			{
				num = this.ReceiverList.Count;
			}
			GameNetworkMessage.WriteBoolToPacket(this.HasReceiverList);
			GameNetworkMessage.WriteIntToPacket(num, CompressionBasic.PlayerCompressionInfo);
			for (int i = 0; i < num; i++)
			{
				GameNetworkMessage.WriteVirtualPlayerReferenceToPacket(this.ReceiverList[i]);
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00004518 File Offset: 0x00002718
		protected override bool OnRead()
		{
			bool flag = true;
			this.Buffer = new byte[1440];
			this.BufferLength = GameNetworkMessage.ReadByteArrayFromPacket(this.Buffer, 0, 1440, ref flag);
			this.HasReceiverList = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref flag);
			if (this.HasReceiverList)
			{
				this.ReceiverList = new List<VirtualPlayer>();
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						VirtualPlayer virtualPlayer = GameNetworkMessage.ReadVirtualPlayerReferenceToPacket(ref flag, false);
						this.ReceiverList.Add(virtualPlayer);
					}
				}
			}
			return flag;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000045A3 File Offset: 0x000027A3
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.None;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000045A7 File Offset: 0x000027A7
		protected override string OnGetLogFormat()
		{
			return string.Empty;
		}
	}
}
