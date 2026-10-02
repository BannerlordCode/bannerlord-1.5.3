using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000008 RID: 8
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class PlayerMessageAll : GameNetworkMessage
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002344 File Offset: 0x00000544
		// (set) Token: 0x06000017 RID: 23 RVA: 0x0000234C File Offset: 0x0000054C
		public string Message { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002355 File Offset: 0x00000555
		// (set) Token: 0x06000019 RID: 25 RVA: 0x0000235D File Offset: 0x0000055D
		public List<VirtualPlayer> ReceiverList { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002366 File Offset: 0x00000566
		// (set) Token: 0x0600001B RID: 27 RVA: 0x0000236E File Offset: 0x0000056E
		public bool HasReceiverList { get; private set; }

		// Token: 0x0600001C RID: 28 RVA: 0x00002377 File Offset: 0x00000577
		public PlayerMessageAll(string message, List<VirtualPlayer> receiverList)
		{
			this.Message = message;
			this.ReceiverList = receiverList;
			this.HasReceiverList = true;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002394 File Offset: 0x00000594
		public PlayerMessageAll(string message)
		{
			this.Message = message;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023A3 File Offset: 0x000005A3
		public PlayerMessageAll()
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023AC File Offset: 0x000005AC
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteStringToPacket(this.Message);
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

		// Token: 0x06000020 RID: 32 RVA: 0x00002410 File Offset: 0x00000610
		protected override bool OnRead()
		{
			bool flag = true;
			this.Message = GameNetworkMessage.ReadStringFromPacket(ref flag);
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

		// Token: 0x06000021 RID: 33 RVA: 0x0000247F File Offset: 0x0000067F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002483 File Offset: 0x00000683
		protected override string OnGetLogFormat()
		{
			return "Receiving Player message to all: " + this.Message;
		}
	}
}
