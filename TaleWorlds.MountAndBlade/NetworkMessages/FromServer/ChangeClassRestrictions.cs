using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000051 RID: 81
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class ChangeClassRestrictions : GameNetworkMessage
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00005782 File Offset: 0x00003982
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000578A File Offset: 0x0000398A
		public FormationClass ClassToChangeRestriction { get; private set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00005793 File Offset: 0x00003993
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000579B File Offset: 0x0000399B
		public bool NewValue { get; private set; }

		// Token: 0x060002BF RID: 703 RVA: 0x000057A4 File Offset: 0x000039A4
		public ChangeClassRestrictions()
		{
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000057AC File Offset: 0x000039AC
		public ChangeClassRestrictions(FormationClass classToChangeRestriction, bool newValue)
		{
			this.ClassToChangeRestriction = classToChangeRestriction;
			this.NewValue = newValue;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000057C4 File Offset: 0x000039C4
		protected override bool OnRead()
		{
			bool flag = true;
			this.ClassToChangeRestriction = (FormationClass)GameNetworkMessage.ReadIntFromPacket(CompressionMission.FormationClassCompressionInfo, ref flag);
			this.NewValue = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000057F3 File Offset: 0x000039F3
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket((int)this.ClassToChangeRestriction, CompressionMission.FormationClassCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(this.NewValue);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00005810 File Offset: 0x00003A10
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00005818 File Offset: 0x00003A18
		protected override string OnGetLogFormat()
		{
			return string.Format("ChangeClassRestrictions for {0} to be {1}", this.ClassToChangeRestriction, this.NewValue);
		}
	}
}
