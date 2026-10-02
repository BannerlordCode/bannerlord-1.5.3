using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000B5 RID: 181
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectVisibility : GameNetworkMessage
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0000CAD1 File Offset: 0x0000ACD1
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x0000CAD9 File Offset: 0x0000ACD9
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0000CAE2 File Offset: 0x0000ACE2
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0000CAEA File Offset: 0x0000ACEA
		public bool Visible { get; private set; }

		// Token: 0x0600074D RID: 1869 RVA: 0x0000CAF3 File Offset: 0x0000ACF3
		public SetMissionObjectVisibility(MissionObjectId missionObjectId, bool visible)
		{
			this.MissionObjectId = missionObjectId;
			this.Visible = visible;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0000CB09 File Offset: 0x0000AD09
		public SetMissionObjectVisibility()
		{
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0000CB14 File Offset: 0x0000AD14
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.Visible = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0000CB3E File Offset: 0x0000AD3E
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
			GameNetworkMessage.WriteBoolToPacket(this.Visible);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0000CB56 File Offset: 0x0000AD56
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0000CB60 File Offset: 0x0000AD60
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[]
			{
				"Set Visibility of MissionObject with ID: ",
				this.MissionObjectId,
				" to: ",
				this.Visible ? "True" : "False"
			});
		}
	}
}
