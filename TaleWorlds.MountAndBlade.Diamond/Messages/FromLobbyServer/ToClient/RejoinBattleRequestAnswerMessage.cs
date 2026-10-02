using System;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000063 RID: 99
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class RejoinBattleRequestAnswerMessage : Message
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000036AF File Offset: 0x000018AF
		// (set) Token: 0x06000201 RID: 513 RVA: 0x000036B7 File Offset: 0x000018B7
		public bool IsRejoinAccepted { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000202 RID: 514 RVA: 0x000036C0 File Offset: 0x000018C0
		// (set) Token: 0x06000203 RID: 515 RVA: 0x000036C8 File Offset: 0x000018C8
		public bool IsSuccessful { get; set; }

		// Token: 0x06000204 RID: 516 RVA: 0x000036D1 File Offset: 0x000018D1
		public RejoinBattleRequestAnswerMessage()
		{
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000036D9 File Offset: 0x000018D9
		public RejoinBattleRequestAnswerMessage(bool isRejoinAccepted, bool isSuccessful)
		{
			this.IsRejoinAccepted = isRejoinAccepted;
			this.IsSuccessful = isSuccessful;
		}
	}
}
