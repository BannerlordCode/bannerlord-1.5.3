using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000047 RID: 71
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class InitializeSessionResponse : LoginResultObject
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000301E File Offset: 0x0000121E
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00003026 File Offset: 0x00001226
		[JsonProperty]
		public PlayerData PlayerData { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000302F File Offset: 0x0000122F
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00003037 File Offset: 0x00001237
		[JsonProperty]
		public ServerStatus ServerStatus { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00003040 File Offset: 0x00001240
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00003048 File Offset: 0x00001248
		[JsonProperty]
		public AvailableScenes AvailableScenes { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00003051 File Offset: 0x00001251
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00003059 File Offset: 0x00001259
		[JsonProperty]
		public SupportedFeatures SupportedFeatures { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00003062 File Offset: 0x00001262
		// (set) Token: 0x06000170 RID: 368 RVA: 0x0000306A File Offset: 0x0000126A
		[JsonProperty]
		public bool HasPendingRejoin { get; private set; }

		// Token: 0x06000171 RID: 369 RVA: 0x00003073 File Offset: 0x00001273
		public InitializeSessionResponse()
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000307B File Offset: 0x0000127B
		public InitializeSessionResponse(PlayerData playerData, ServerStatus serverStatus, AvailableScenes availableScenes, SupportedFeatures supportedFeatures, bool hasPendingRejoin)
		{
			this.PlayerData = playerData;
			this.ServerStatus = serverStatus;
			this.AvailableScenes = availableScenes;
			this.SupportedFeatures = supportedFeatures;
			this.HasPendingRejoin = hasPendingRejoin;
		}
	}
}
