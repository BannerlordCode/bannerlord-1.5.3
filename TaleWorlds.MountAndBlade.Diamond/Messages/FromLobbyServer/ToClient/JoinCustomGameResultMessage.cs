using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004C RID: 76
	[Serializable]
	public class JoinCustomGameResultMessage : Message
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000031A9 File Offset: 0x000013A9
		// (set) Token: 0x0600018C RID: 396 RVA: 0x000031B1 File Offset: 0x000013B1
		[JsonProperty]
		public JoinGameData JoinGameData { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000031BA File Offset: 0x000013BA
		// (set) Token: 0x0600018E RID: 398 RVA: 0x000031C2 File Offset: 0x000013C2
		[JsonProperty]
		public bool Success { get; private set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000031CB File Offset: 0x000013CB
		// (set) Token: 0x06000190 RID: 400 RVA: 0x000031D3 File Offset: 0x000013D3
		[JsonProperty]
		public CustomGameJoinResponse Response { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000031DC File Offset: 0x000013DC
		// (set) Token: 0x06000192 RID: 402 RVA: 0x000031E4 File Offset: 0x000013E4
		[JsonProperty]
		public string MatchId { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000031ED File Offset: 0x000013ED
		// (set) Token: 0x06000194 RID: 404 RVA: 0x000031F5 File Offset: 0x000013F5
		[JsonProperty]
		public CustomGameJoinType JoinType { get; private set; }

		// Token: 0x06000195 RID: 405 RVA: 0x000031FE File Offset: 0x000013FE
		public JoinCustomGameResultMessage()
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00003206 File Offset: 0x00001406
		private JoinCustomGameResultMessage(JoinGameData joinGameData, bool success, CustomGameJoinResponse response, string matchId, CustomGameJoinType joinType)
		{
			this.JoinGameData = joinGameData;
			this.Success = success;
			this.Response = response;
			this.MatchId = matchId;
			this.JoinType = joinType;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00003233 File Offset: 0x00001433
		public static JoinCustomGameResultMessage CreateSuccess(JoinGameData joinGameData, string matchId, CustomGameJoinType joinType)
		{
			return new JoinCustomGameResultMessage(joinGameData, true, CustomGameJoinResponse.Success, matchId, joinType);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000323F File Offset: 0x0000143F
		public static JoinCustomGameResultMessage CreateFailed(CustomGameJoinResponse response)
		{
			return new JoinCustomGameResultMessage(null, false, response, null, CustomGameJoinType.Player);
		}
	}
}
