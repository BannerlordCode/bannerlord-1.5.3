using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x02000002 RID: 2
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class AddGameLogsMessage : Message
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		[JsonProperty]
		public GameLog[] GameLogs { get; private set; }

		// Token: 0x06000003 RID: 3 RVA: 0x00002059 File Offset: 0x00000259
		public AddGameLogsMessage()
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002061 File Offset: 0x00000261
		public AddGameLogsMessage(GameLog[] gameLogs)
		{
			this.GameLogs = gameLogs;
		}
	}
}
