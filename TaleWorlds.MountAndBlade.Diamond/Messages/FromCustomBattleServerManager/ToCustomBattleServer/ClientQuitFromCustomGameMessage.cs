using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x0200000F RID: 15
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[Serializable]
	public class ClientQuitFromCustomGameMessage : Message
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002694 File Offset: 0x00000894
		// (set) Token: 0x0600007A RID: 122 RVA: 0x0000269C File Offset: 0x0000089C
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x0600007B RID: 123 RVA: 0x000026A5 File Offset: 0x000008A5
		public ClientQuitFromCustomGameMessage()
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000026AD File Offset: 0x000008AD
		public ClientQuitFromCustomGameMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
