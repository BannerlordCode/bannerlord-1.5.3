using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000014 RID: 20
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[DataContract]
	[Serializable]
	public class SetChatFilterListsMessage : Message
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008A RID: 138 RVA: 0x0000273C File Offset: 0x0000093C
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00002744 File Offset: 0x00000944
		[JsonProperty]
		public string[] ProfanityList { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000274D File Offset: 0x0000094D
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00002755 File Offset: 0x00000955
		[JsonProperty]
		public string[] AllowList { get; private set; }

		// Token: 0x0600008E RID: 142 RVA: 0x0000275E File Offset: 0x0000095E
		public SetChatFilterListsMessage()
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002766 File Offset: 0x00000966
		public SetChatFilterListsMessage(string[] profanityList, string[] allowList)
		{
			this.ProfanityList = profanityList;
			this.AllowList = allowList;
		}
	}
}
