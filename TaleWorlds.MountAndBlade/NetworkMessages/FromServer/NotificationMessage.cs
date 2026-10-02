using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000DC RID: 220
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class NotificationMessage : GameNetworkMessage
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0000F08B File Offset: 0x0000D28B
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x0000F093 File Offset: 0x0000D293
		public int Message { get; private set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0000F09C File Offset: 0x0000D29C
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x0000F0A4 File Offset: 0x0000D2A4
		public int ParameterOne { get; private set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x0000F0AD File Offset: 0x0000D2AD
		// (set) Token: 0x060008FC RID: 2300 RVA: 0x0000F0B5 File Offset: 0x0000D2B5
		public int ParameterTwo { get; private set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x0000F0BE File Offset: 0x0000D2BE
		private bool HasParameterOne
		{
			get
			{
				return this.ParameterOne != -1;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0000F0CC File Offset: 0x0000D2CC
		private bool HasParameterTwo
		{
			get
			{
				return this.ParameterOne != -1;
			}
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0000F0DA File Offset: 0x0000D2DA
		public NotificationMessage(int message, int param1, int param2)
		{
			this.Message = message;
			this.ParameterOne = param1;
			this.ParameterTwo = param2;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0000F0F7 File Offset: 0x0000D2F7
		public NotificationMessage()
		{
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0000F100 File Offset: 0x0000D300
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.Message, CompressionMission.MultiplayerNotificationCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.HasParameterOne);
			if (this.HasParameterOne)
			{
				GameNetworkMessage.WriteIntToPacket(this.ParameterOne, CompressionMission.MultiplayerNotificationParameterCompressionInfo);
				GameNetworkMessage.WriteBoolToPacket(this.HasParameterTwo);
				if (this.HasParameterTwo)
				{
					GameNetworkMessage.WriteIntToPacket(this.ParameterTwo, CompressionMission.MultiplayerNotificationParameterCompressionInfo);
				}
			}
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0000F164 File Offset: 0x0000D364
		protected override bool OnRead()
		{
			bool flag = true;
			this.ParameterOne = (this.ParameterTwo = -1);
			this.Message = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MultiplayerNotificationCompressionInfo, ref flag);
			if (GameNetworkMessage.ReadBoolFromPacket(ref flag))
			{
				this.ParameterOne = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MultiplayerNotificationParameterCompressionInfo, ref flag);
				if (GameNetworkMessage.ReadBoolFromPacket(ref flag))
				{
					this.ParameterTwo = GameNetworkMessage.ReadIntFromPacket(CompressionMission.MultiplayerNotificationParameterCompressionInfo, ref flag);
				}
			}
			return flag;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0000F1CC File Offset: 0x0000D3CC
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Messaging;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0000F1D0 File Offset: 0x0000D3D0
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Receiving message: ",
				this.Message,
				this.HasParameterOne ? (" With first parameter: " + this.ParameterOne) : "",
				this.HasParameterTwo ? (" and second parameter: " + this.ParameterTwo) : ""
			});
		}
	}
}
