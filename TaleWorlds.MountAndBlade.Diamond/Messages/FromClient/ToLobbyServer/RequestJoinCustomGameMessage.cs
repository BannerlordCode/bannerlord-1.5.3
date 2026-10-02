using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C1 RID: 193
	[MessageDescription("Client", "LobbyServer", false)]
	[Serializable]
	public class RequestJoinCustomGameMessage : Message
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000385 RID: 901 RVA: 0x000046B2 File Offset: 0x000028B2
		// (set) Token: 0x06000386 RID: 902 RVA: 0x000046BA File Offset: 0x000028BA
		[JsonProperty]
		public CustomBattleId CustomBattleId { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000387 RID: 903 RVA: 0x000046C3 File Offset: 0x000028C3
		// (set) Token: 0x06000388 RID: 904 RVA: 0x000046CB File Offset: 0x000028CB
		[JsonProperty]
		public string Password { get; private set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000389 RID: 905 RVA: 0x000046D4 File Offset: 0x000028D4
		// (set) Token: 0x0600038A RID: 906 RVA: 0x000046DC File Offset: 0x000028DC
		[JsonProperty]
		public CustomGameJoinType JoinType { get; private set; }

		// Token: 0x0600038B RID: 907 RVA: 0x000046E5 File Offset: 0x000028E5
		public RequestJoinCustomGameMessage()
		{
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000046ED File Offset: 0x000028ED
		public RequestJoinCustomGameMessage(CustomBattleId customBattleId, CustomGameJoinType joinType, string password)
		{
			this.CustomBattleId = customBattleId;
			this.Password = password;
			this.JoinType = joinType;
		}
	}
}
