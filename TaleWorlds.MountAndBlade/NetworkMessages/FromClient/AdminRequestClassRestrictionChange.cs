using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000013 RID: 19
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class AdminRequestClassRestrictionChange : GameNetworkMessage
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002BC7 File Offset: 0x00000DC7
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002BCF File Offset: 0x00000DCF
		public FormationClass ClassToChangeRestriction { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002BD8 File Offset: 0x00000DD8
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002BE0 File Offset: 0x00000DE0
		public bool NewValue { get; private set; }

		// Token: 0x06000084 RID: 132 RVA: 0x00002BE9 File Offset: 0x00000DE9
		public AdminRequestClassRestrictionChange()
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002BF1 File Offset: 0x00000DF1
		public AdminRequestClassRestrictionChange(FormationClass classToChangeRestriction, bool newValue)
		{
			this.ClassToChangeRestriction = classToChangeRestriction;
			this.NewValue = newValue;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002C08 File Offset: 0x00000E08
		protected override bool OnRead()
		{
			bool flag = true;
			this.ClassToChangeRestriction = (FormationClass)GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.NewValue = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002C37 File Offset: 0x00000E37
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.ClassToChangeRestriction, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.NewValue);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002C54 File Offset: 0x00000E54
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002C5C File Offset: 0x00000E5C
		protected override string OnGetLogFormat()
		{
			return string.Format("AdminRequestClassRestrictionChange for {0} to be {1}", this.ClassToChangeRestriction, this.NewValue);
		}
	}
}
