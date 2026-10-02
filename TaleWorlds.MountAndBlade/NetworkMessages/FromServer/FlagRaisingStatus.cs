using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000046 RID: 70
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class FlagRaisingStatus : GameNetworkMessage
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00004B99 File Offset: 0x00002D99
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00004BA1 File Offset: 0x00002DA1
		public float Progress { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00004BAA File Offset: 0x00002DAA
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00004BB2 File Offset: 0x00002DB2
		public CaptureTheFlagFlagDirection Direction { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00004BBB File Offset: 0x00002DBB
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00004BC3 File Offset: 0x00002DC3
		public float Speed { get; private set; }

		// Token: 0x06000245 RID: 581 RVA: 0x00004BCC File Offset: 0x00002DCC
		public FlagRaisingStatus()
		{
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00004BD4 File Offset: 0x00002DD4
		public FlagRaisingStatus(float currProgress, CaptureTheFlagFlagDirection direction, float speed)
		{
			this.Progress = currProgress;
			this.Direction = direction;
			this.Speed = speed;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00004BF4 File Offset: 0x00002DF4
		protected override bool OnRead()
		{
			bool flag = true;
			this.Progress = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagClassicProgressCompressionInfo, ref flag);
			this.Direction = (CaptureTheFlagFlagDirection)GameNetworkMessage.ReadIntFromPacket(CompressionMission.FlagDirectionEnumCompressionInfo, ref flag);
			if (flag && this.Direction != CaptureTheFlagFlagDirection.None && this.Direction != CaptureTheFlagFlagDirection.Static)
			{
				this.Speed = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.FlagSpeedCompressionInfo, ref flag);
			}
			return flag;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00004C50 File Offset: 0x00002E50
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteFloatToPacket(this.Progress, CompressionMission.FlagClassicProgressCompressionInfo);
			GameNetworkMessage.WriteIntToPacket((int)this.Direction, CompressionMission.FlagDirectionEnumCompressionInfo);
			if (this.Direction != CaptureTheFlagFlagDirection.None && this.Direction != CaptureTheFlagFlagDirection.Static)
			{
				GameNetworkMessage.WriteFloatToPacket(this.Speed, CompressionMission.FlagSpeedCompressionInfo);
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00004C9F File Offset: 0x00002E9F
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.GameMode;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00004CA8 File Offset: 0x00002EA8
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Updating flag movement: Progress: ", this.Progress, ", Direction: ", this.Direction, ", Speed: ", this.Speed });
		}
	}
}
