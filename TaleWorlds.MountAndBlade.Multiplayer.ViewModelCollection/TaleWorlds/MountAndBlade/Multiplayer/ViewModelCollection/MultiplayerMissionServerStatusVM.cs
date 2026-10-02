using System;
using NetworkMessages.FromServer;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000012 RID: 18
	public class MultiplayerMissionServerStatusVM : ViewModel
	{
		// Token: 0x06000103 RID: 259 RVA: 0x0000576B File Offset: 0x0000396B
		public void UpdatePacketLossRatio(float v)
		{
			if (v >= 0.02f)
			{
				this.PacketLossState = 2;
				return;
			}
			if (v >= 0.01f)
			{
				this.PacketLossState = 1;
				return;
			}
			this.PacketLossState = 0;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005794 File Offset: 0x00003994
		public void UpdatePeerPing(double averagePingInMilliseconds)
		{
			if (averagePingInMilliseconds >= 110.0)
			{
				this.PingState = 2;
				return;
			}
			if (averagePingInMilliseconds >= 90.0)
			{
				this.PingState = 1;
				return;
			}
			this.PingState = 0;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000057C5 File Offset: 0x000039C5
		public void UpdateServerPerformanceState(ServerPerformanceState serverPerformanceState)
		{
			switch (serverPerformanceState)
			{
			default:
				this.ServerPerformanceState = 0;
				return;
			case NetworkMessages.FromServer.ServerPerformanceState.Medium:
				this.ServerPerformanceState = 1;
				return;
			case NetworkMessages.FromServer.ServerPerformanceState.Low:
			case NetworkMessages.FromServer.ServerPerformanceState.Count:
				this.ServerPerformanceState = 2;
				return;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000057F4 File Offset: 0x000039F4
		public void ResetStates()
		{
			this.PacketLossState = 0;
			this.PingState = 0;
			this.ServerPerformanceState = 0;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000580B File Offset: 0x00003A0B
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00005813 File Offset: 0x00003A13
		[DataSourceProperty]
		public int PacketLossState
		{
			get
			{
				return this._packetLossState;
			}
			set
			{
				if (value != this._packetLossState)
				{
					this._packetLossState = value;
					base.OnPropertyChangedWithValue(value, "PacketLossState");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005831 File Offset: 0x00003A31
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00005839 File Offset: 0x00003A39
		[DataSourceProperty]
		public int PingState
		{
			get
			{
				return this._pingState;
			}
			set
			{
				if (value != this._pingState)
				{
					this._pingState = value;
					base.OnPropertyChangedWithValue(value, "PingState");
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005857 File Offset: 0x00003A57
		// (set) Token: 0x0600010C RID: 268 RVA: 0x0000585F File Offset: 0x00003A5F
		[DataSourceProperty]
		public int ServerPerformanceState
		{
			get
			{
				return this._serverPerformanceState;
			}
			set
			{
				if (value != this._serverPerformanceState)
				{
					this._serverPerformanceState = value;
					base.OnPropertyChangedWithValue(value, "ServerPerformanceState");
				}
			}
		}

		// Token: 0x04000094 RID: 148
		private int _packetLossState;

		// Token: 0x04000095 RID: 149
		private int _pingState;

		// Token: 0x04000096 RID: 150
		private int _serverPerformanceState;

		// Token: 0x020000C3 RID: 195
		private enum StatusTypes
		{
			// Token: 0x0400083D RID: 2109
			Good,
			// Token: 0x0400083E RID: 2110
			Average,
			// Token: 0x0400083F RID: 2111
			Poor
		}
	}
}
