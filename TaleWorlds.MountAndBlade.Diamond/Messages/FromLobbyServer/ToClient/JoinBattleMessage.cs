using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004B RID: 75
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class JoinBattleMessage : Message
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00003181 File Offset: 0x00001381
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00003189 File Offset: 0x00001389
		[JsonProperty]
		public BattleServerInformationForClient BattleServerInformation { get; private set; }

		// Token: 0x06000189 RID: 393 RVA: 0x00003192 File Offset: 0x00001392
		public JoinBattleMessage()
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000319A File Offset: 0x0000139A
		public JoinBattleMessage(BattleServerInformationForClient battleServerInformation)
		{
			this.BattleServerInformation = battleServerInformation;
		}
	}
}
