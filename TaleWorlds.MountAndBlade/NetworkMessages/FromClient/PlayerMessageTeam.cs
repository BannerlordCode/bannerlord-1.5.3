using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000009 RID: 9
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class PlayerMessageTeam : GameNetworkMessage
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002495 File Offset: 0x00000695
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000249D File Offset: 0x0000069D
		public string Message { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000024A6 File Offset: 0x000006A6
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000024AE File Offset: 0x000006AE
		public List<VirtualPlayer> ReceiverList { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000024B7 File Offset: 0x000006B7
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000024BF File Offset: 0x000006BF
		public bool HasReceiverList { get; private set; }

		// Token: 0x06000029 RID: 41 RVA: 0x000024C8 File Offset: 0x000006C8
		public PlayerMessageTeam(string message, List<VirtualPlayer> receiverList)
		{
			this.Message = message;
			this.ReceiverList = receiverList;
			this.HasReceiverList = true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024E5 File Offset: 0x000006E5
		public PlayerMessageTeam(string message)
		{
			this.Message = message;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000024F4 File Offset: 0x000006F4
		public PlayerMessageTeam()
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000024FC File Offset: 0x000006FC
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

		// Token: 0x0600002D RID: 45 RVA: 0x00002560 File Offset: 0x00000760
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

		// Token: 0x0600002E RID: 46 RVA: 0x000025CF File Offset: 0x000007CF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000025D3 File Offset: 0x000007D3
		protected override string OnGetLogFormat()
		{
			return "Receiving Player message to team: " + this.Message;
		}
	}
}
