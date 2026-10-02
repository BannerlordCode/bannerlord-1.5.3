using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x0200000D RID: 13
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class UpdateCustomGameData : Message
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000025E4 File Offset: 0x000007E4
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000025EC File Offset: 0x000007EC
		[JsonProperty]
		public string NewGameType { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000025F5 File Offset: 0x000007F5
		// (set) Token: 0x0600006C RID: 108 RVA: 0x000025FD File Offset: 0x000007FD
		[JsonProperty]
		public string NewMap { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002606 File Offset: 0x00000806
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000260E File Offset: 0x0000080E
		[JsonProperty]
		public int NewMaxNumberOfPlayers { get; private set; }

		// Token: 0x0600006F RID: 111 RVA: 0x00002617 File Offset: 0x00000817
		public UpdateCustomGameData()
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000261F File Offset: 0x0000081F
		public UpdateCustomGameData(string newGameType, string newMap, int newMaxNumberOfPlayers)
		{
			this.NewGameType = newGameType;
			this.NewMap = newMap;
			this.NewMaxNumberOfPlayers = newMaxNumberOfPlayers;
		}
	}
}
