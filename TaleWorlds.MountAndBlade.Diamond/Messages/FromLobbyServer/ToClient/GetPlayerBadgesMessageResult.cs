using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003C RID: 60
	[Serializable]
	public class GetPlayerBadgesMessageResult : FunctionResult
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00002E36 File Offset: 0x00001036
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00002E3E File Offset: 0x0000103E
		[JsonProperty]
		public string[] Badges { get; private set; }

		// Token: 0x06000139 RID: 313 RVA: 0x00002E47 File Offset: 0x00001047
		public GetPlayerBadgesMessageResult()
		{
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00002E4F File Offset: 0x0000104F
		public GetPlayerBadgesMessageResult(string[] badges)
		{
			this.Badges = badges;
		}
	}
}
