using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x020000A5 RID: 165
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class SetBatteringRamHasArrivedAtTarget : GameNetworkMessage
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0000B9EA File Offset: 0x00009BEA
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x0000B9F2 File Offset: 0x00009BF2
		public MissionObjectId BatteringRamId { get; private set; }

		// Token: 0x06000698 RID: 1688 RVA: 0x0000B9FB File Offset: 0x00009BFB
		public SetBatteringRamHasArrivedAtTarget(MissionObjectId batteringRamId)
		{
			this.BatteringRamId = batteringRamId;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0000BA0A File Offset: 0x00009C0A
		public SetBatteringRamHasArrivedAtTarget()
		{
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0000BA14 File Offset: 0x00009C14
		protected override bool OnRead()
		{
			bool flag = true;
			this.BatteringRamId = GameNetworkMessage.ReadMissionObjectIdFromPacket(ref flag);
			return flag;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0000BA31 File Offset: 0x00009C31
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteMissionObjectIdToPacket(this.BatteringRamId);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0000BA3E File Offset: 0x00009C3E
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.SiegeWeaponsDetailed;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0000BA46 File Offset: 0x00009C46
		protected override string OnGetLogFormat()
		{
			return "Battering Ram with ID: " + this.BatteringRamId + " has arrived at its target.";
		}
	}
}
