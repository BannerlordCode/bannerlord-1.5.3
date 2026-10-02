using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000039 RID: 57
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class SyncRelevantGameOptionsToServer : GameNetworkMessage
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000420D File Offset: 0x0000240D
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00004215 File Offset: 0x00002415
		public bool SendMeBloodEvents { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000421E File Offset: 0x0000241E
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00004226 File Offset: 0x00002426
		public bool SendMeSoundEvents { get; private set; }

		// Token: 0x060001C5 RID: 453 RVA: 0x0000422F File Offset: 0x0000242F
		public SyncRelevantGameOptionsToServer()
		{
			this.SendMeBloodEvents = true;
			this.SendMeSoundEvents = true;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00004245 File Offset: 0x00002445
		public void InitializeOptions()
		{
			this.SendMeBloodEvents = BannerlordConfig.ShowBlood;
			this.SendMeSoundEvents = NativeOptions.GetConfig(NativeOptions.NativeOptionsType.SoundVolume) > 0.01f && NativeOptions.GetConfig(NativeOptions.NativeOptionsType.MasterVolume) > 0.01f;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00004278 File Offset: 0x00002478
		protected override bool OnRead()
		{
			bool flag = true;
			this.SendMeBloodEvents = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.SendMeSoundEvents = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000042A2 File Offset: 0x000024A2
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.SendMeBloodEvents);
			GameNetworkMessage.WriteBoolToPacket(this.SendMeSoundEvents);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000042BA File Offset: 0x000024BA
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.General;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000042BE File Offset: 0x000024BE
		protected override string OnGetLogFormat()
		{
			return "SyncRelevantGameOptionsToServer";
		}
	}
}
