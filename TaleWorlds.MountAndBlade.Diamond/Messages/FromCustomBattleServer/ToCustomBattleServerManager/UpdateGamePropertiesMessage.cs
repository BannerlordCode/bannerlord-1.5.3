using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x0200000E RID: 14
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class UpdateGamePropertiesMessage : Message
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000071 RID: 113 RVA: 0x0000263C File Offset: 0x0000083C
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00002644 File Offset: 0x00000844
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000264D File Offset: 0x0000084D
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00002655 File Offset: 0x00000855
		[JsonProperty]
		public string Scene { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000075 RID: 117 RVA: 0x0000265E File Offset: 0x0000085E
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00002666 File Offset: 0x00000866
		[JsonProperty]
		public string UniqueSceneId { get; private set; }

		// Token: 0x06000077 RID: 119 RVA: 0x0000266F File Offset: 0x0000086F
		public UpdateGamePropertiesMessage()
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002677 File Offset: 0x00000877
		public UpdateGamePropertiesMessage(string gameType, string scene, string uniqueSceneId)
		{
			this.GameType = gameType;
			this.Scene = scene;
			this.UniqueSceneId = uniqueSceneId;
		}
	}
}
