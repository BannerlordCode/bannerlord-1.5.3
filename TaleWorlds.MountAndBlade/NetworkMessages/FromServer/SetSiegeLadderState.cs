using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000BA RID: 186
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetSiegeLadderState : GameNetworkMessage
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0000CED5 File Offset: 0x0000B0D5
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x0000CEDD File Offset: 0x0000B0DD
		public MissionObjectId SiegeLadderId { get; private set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0000CEE6 File Offset: 0x0000B0E6
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x0000CEEE File Offset: 0x0000B0EE
		public SiegeLadder.LadderState State { get; private set; }

		// Token: 0x0600077D RID: 1917 RVA: 0x0000CEF7 File Offset: 0x0000B0F7
		public SetSiegeLadderState(MissionObjectId siegeLadderId, SiegeLadder.LadderState state)
		{
			this.SiegeLadderId = siegeLadderId;
			this.State = state;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0000CF0D File Offset: 0x0000B10D
		public SetSiegeLadderState()
		{
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0000CF18 File Offset: 0x0000B118
		protected override bool OnRead()
		{
			bool flag = true;
			this.SiegeLadderId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			this.State = (SiegeLadder.LadderState)GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeLadderStateCompressionInfo, ref flag);
			return flag;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0000CF47 File Offset: 0x0000B147
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.SiegeLadderId);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.SiegeLadderStateCompressionInfo);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0000CF64 File Offset: 0x0000B164
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0000CF6C File Offset: 0x0000B16C
		protected override string OnGetLogFormat()
		{
			return string.Concat(new object[] { "Set SiegeLadder State to: ", this.State, " on SiegeLadderState with ID: ", this.SiegeLadderId });
		}
	}
}
