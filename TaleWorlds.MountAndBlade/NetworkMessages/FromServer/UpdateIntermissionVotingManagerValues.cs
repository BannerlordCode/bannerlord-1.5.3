using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x0200006E RID: 110
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class UpdateIntermissionVotingManagerValues : GameNetworkMessage
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000756A File Offset: 0x0000576A
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x00007572 File Offset: 0x00005772
		public bool IsAutomatedBattleSwitchingEnabled { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000757B File Offset: 0x0000577B
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x00007583 File Offset: 0x00005783
		public bool IsMapVoteEnabled { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000758C File Offset: 0x0000578C
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x00007594 File Offset: 0x00005794
		public bool IsCultureVoteEnabled { get; private set; }

		// Token: 0x060003D9 RID: 985 RVA: 0x0000759D File Offset: 0x0000579D
		public UpdateIntermissionVotingManagerValues()
		{
			this.IsAutomatedBattleSwitchingEnabled = MultiplayerIntermissionVotingManager.Instance.IsAutomatedBattleSwitchingEnabled;
			this.IsMapVoteEnabled = MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled;
			this.IsCultureVoteEnabled = MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000075D5 File Offset: 0x000057D5
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000075DD File Offset: 0x000057DD
		protected override string OnGetLogFormat()
		{
			return string.Format("IsAutomatedBattleSwitchingEnabled: {0}, IsMapVoteEnabled: {1}, IsCultureVoteEnabled: {2}", this.IsAutomatedBattleSwitchingEnabled, this.IsMapVoteEnabled, this.IsCultureVoteEnabled);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000760C File Offset: 0x0000580C
		protected override bool OnRead()
		{
			bool flag = true;
			this.IsAutomatedBattleSwitchingEnabled = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsMapVoteEnabled = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			this.IsCultureVoteEnabled = GameNetworkMessage.ReadBoolFromPacket(ref flag);
			return flag;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00007643 File Offset: 0x00005843
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteBoolToPacket(this.IsAutomatedBattleSwitchingEnabled);
			GameNetworkMessage.WriteBoolToPacket(this.IsMapVoteEnabled);
			GameNetworkMessage.WriteBoolToPacket(this.IsCultureVoteEnabled);
		}
	}
}
