using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E7 RID: 231
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class StartBattleMessage : Message
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00004F3C File Offset: 0x0000313C
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00004F44 File Offset: 0x00003144
		[JsonProperty]
		public string SceneName { get; private set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00004F4D File Offset: 0x0000314D
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00004F55 File Offset: 0x00003155
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00004F5E File Offset: 0x0000315E
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00004F66 File Offset: 0x00003166
		[JsonProperty]
		public Guid BattleId { get; private set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00004F6F File Offset: 0x0000316F
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00004F77 File Offset: 0x00003177
		[JsonProperty]
		public string Faction1 { get; private set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00004F80 File Offset: 0x00003180
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00004F88 File Offset: 0x00003188
		[JsonProperty]
		public string Faction2 { get; private set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00004F91 File Offset: 0x00003191
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00004F99 File Offset: 0x00003199
		[JsonProperty]
		public int MinRequiredPlayerCountToStartBattle { get; private set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00004FA2 File Offset: 0x000031A2
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00004FAA File Offset: 0x000031AA
		[JsonProperty]
		public int BattleSize { get; private set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00004FB3 File Offset: 0x000031B3
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x00004FBB File Offset: 0x000031BB
		[JsonProperty]
		public int RoundThreshold { get; private set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00004FC4 File Offset: 0x000031C4
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x00004FCC File Offset: 0x000031CC
		[JsonProperty]
		public float MoraleThreshold { get; private set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00004FD5 File Offset: 0x000031D5
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x00004FDD File Offset: 0x000031DD
		[JsonProperty]
		public bool UseAnalytics { get; private set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00004FE6 File Offset: 0x000031E6
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x00004FEE File Offset: 0x000031EE
		[JsonProperty]
		public bool CaptureMovementData { get; private set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00004FF7 File Offset: 0x000031F7
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00004FFF File Offset: 0x000031FF
		[JsonProperty]
		public string AnalyticsServiceAddress { get; private set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00005008 File Offset: 0x00003208
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x00005010 File Offset: 0x00003210
		[JsonProperty]
		public int MaxFriendlyKillCount { get; private set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00005019 File Offset: 0x00003219
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x00005021 File Offset: 0x00003221
		[JsonProperty]
		public float MaxFriendlyDamage { get; private set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000502A File Offset: 0x0000322A
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x00005032 File Offset: 0x00003232
		[JsonProperty]
		public float MaxFriendlyDamagePerSingleRound { get; private set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000503B File Offset: 0x0000323B
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00005043 File Offset: 0x00003243
		[JsonProperty]
		public float RoundFriendlyDamageLimit { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000504C File Offset: 0x0000324C
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00005054 File Offset: 0x00003254
		[JsonProperty]
		public int MaxRoundsOverLimitCount { get; private set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0000505D File Offset: 0x0000325D
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x00005065 File Offset: 0x00003265
		[JsonProperty]
		public bool IsPremadeGame { get; private set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0000506E File Offset: 0x0000326E
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x00005076 File Offset: 0x00003276
		[JsonProperty]
		public string[] ProfanityList { get; private set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0000507F File Offset: 0x0000327F
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x00005087 File Offset: 0x00003287
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00005090 File Offset: 0x00003290
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x00005098 File Offset: 0x00003298
		[JsonProperty]
		public string[] AllowList { get; private set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x000050A1 File Offset: 0x000032A1
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x000050A9 File Offset: 0x000032A9
		[JsonProperty]
		public PlayerId[] AssignedPlayers { get; private set; }

		// Token: 0x06000474 RID: 1140 RVA: 0x000050B2 File Offset: 0x000032B2
		public StartBattleMessage()
		{
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000050BC File Offset: 0x000032BC
		public StartBattleMessage(Guid battleId, string sceneName, string gameType, string faction1, string faction2, int minRequiredPlayerCountToStartBattle, int battleSize, int roundThreshold, float moraleThreshold, bool useAnalytics, bool captureMovementData, string analyticsServiceAddress, int maxFriendlyKillCount, float maxFriendlyDamage, float maxFriendlyDamagePerSingleRound, float roundFriendlyDamageLimit, int maxRoundsOverLimitCount, bool isPremadeGame, PremadeGameType premadeGameType, string[] profanityList, string[] allowList, PlayerId[] assignedPlayers)
		{
			this.SceneName = sceneName;
			this.GameType = gameType;
			this.BattleId = battleId;
			this.Faction1 = faction1;
			this.Faction2 = faction2;
			this.MinRequiredPlayerCountToStartBattle = minRequiredPlayerCountToStartBattle;
			this.BattleSize = battleSize;
			this.UseAnalytics = useAnalytics;
			this.CaptureMovementData = captureMovementData;
			this.AnalyticsServiceAddress = analyticsServiceAddress;
			this.RoundThreshold = roundThreshold;
			this.MoraleThreshold = moraleThreshold;
			this.MaxFriendlyKillCount = maxFriendlyKillCount;
			this.MaxFriendlyDamage = maxFriendlyDamage;
			this.MaxFriendlyDamagePerSingleRound = maxFriendlyDamagePerSingleRound;
			this.RoundFriendlyDamageLimit = roundFriendlyDamageLimit;
			this.MaxRoundsOverLimitCount = maxRoundsOverLimitCount;
			this.IsPremadeGame = isPremadeGame;
			this.PremadeGameType = premadeGameType;
			this.ProfanityList = profanityList;
			this.AllowList = allowList;
			this.AssignedPlayers = assignedPlayers;
		}
	}
}
