using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000AD RID: 173
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetMissionObjectDisabled : GameNetworkMessage
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0000C1D9 File Offset: 0x0000A3D9
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0000C1E1 File Offset: 0x0000A3E1
		public MissionObjectId MissionObjectId { get; private set; }

		// Token: 0x060006F3 RID: 1779 RVA: 0x0000C1EA File Offset: 0x0000A3EA
		public SetMissionObjectDisabled(MissionObjectId missionObjectId)
		{
			this.MissionObjectId = missionObjectId;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0000C1F9 File Offset: 0x0000A3F9
		public SetMissionObjectDisabled()
		{
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0000C204 File Offset: 0x0000A404
		protected override bool OnRead()
		{
			bool flag = true;
			this.MissionObjectId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0000C221 File Offset: 0x0000A421
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.MissionObjectId);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0000C22E File Offset: 0x0000A42E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.MissionObjects;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0000C236 File Offset: 0x0000A436
		protected override string OnGetLogFormat()
		{
			return "Mission Object with ID: " + this.MissionObjectId + " has been disabled.";
		}
	}
}
