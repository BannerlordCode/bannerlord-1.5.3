using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D7 RID: 215
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleStartedMessage : Message
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00004C3C File Offset: 0x00002E3C
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x00004C44 File Offset: 0x00002E44
		[JsonProperty]
		public bool Report { get; private set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00004C4D File Offset: 0x00002E4D
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00004C55 File Offset: 0x00002E55
		[JsonProperty]
		public Dictionary<string, int> PlayerTeams { get; private set; }

		// Token: 0x06000404 RID: 1028 RVA: 0x00004C5E File Offset: 0x00002E5E
		public BattleStartedMessage()
		{
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00004C66 File Offset: 0x00002E66
		public BattleStartedMessage(bool report)
		{
			this.Report = report;
			this.PlayerTeams = null;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00004C7C File Offset: 0x00002E7C
		public BattleStartedMessage(bool report, Dictionary<string, int> playerTeams)
		{
			this.Report = report;
			this.PlayerTeams = playerTeams;
		}
	}
}
