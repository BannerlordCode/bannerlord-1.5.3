using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000147 RID: 327
	[Serializable]
	public class PlayerData
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0000D18E File Offset: 0x0000B38E
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x0000D196 File Offset: 0x0000B396
		public PlayerId PlayerId { get; set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0000D19F File Offset: 0x0000B39F
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x0000D1A7 File Offset: 0x0000B3A7
		public PlayerId OwnerPlayerId { get; set; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x0000D1B8 File Offset: 0x0000B3B8
		public string Sigil { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0000D1C1 File Offset: 0x0000B3C1
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x0000D1C9 File Offset: 0x0000B3C9
		public BodyProperties BodyProperties
		{
			get
			{
				return this._bodyProperties;
			}
			set
			{
				this.SetBodyProperties(value);
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0000D1D2 File Offset: 0x0000B3D2
		private void SetBodyProperties(BodyProperties bodyProperties)
		{
			this._bodyProperties = bodyProperties.ClampForMultiplayer();
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0000D1E1 File Offset: 0x0000B3E1
		[JsonIgnore]
		public int ShownBadgeIndex
		{
			get
			{
				Badge byId = BadgeManager.GetById(this.ShownBadgeId);
				if (byId == null)
				{
					return -1;
				}
				return byId.Index;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0000D1F9 File Offset: 0x0000B3F9
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x0000D201 File Offset: 0x0000B401
		public PlayerStatsBase[] Stats { get; set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0000D20A File Offset: 0x0000B40A
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x0000D212 File Offset: 0x0000B412
		public int Race { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0000D21B File Offset: 0x0000B41B
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x0000D223 File Offset: 0x0000B423
		public bool IsFemale { get; set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0000D22C File Offset: 0x0000B42C
		[JsonIgnore]
		public int KillCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.KillCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x0000D268 File Offset: 0x0000B468
		[JsonIgnore]
		public int DeathCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.DeathCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0000D2A4 File Offset: 0x0000B4A4
		[JsonIgnore]
		public int AssistCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.AssistCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x0000D2E0 File Offset: 0x0000B4E0
		[JsonIgnore]
		public int WinCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.WinCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0000D31C File Offset: 0x0000B51C
		[JsonIgnore]
		public int LoseCount
		{
			get
			{
				int num = 0;
				if (this.Stats != null)
				{
					foreach (PlayerStatsBase playerStatsBase in this.Stats)
					{
						num += playerStatsBase.LoseCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0000D356 File Offset: 0x0000B556
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x0000D35E File Offset: 0x0000B55E
		public int Experience { get; set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x0000D367 File Offset: 0x0000B567
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x0000D36F File Offset: 0x0000B56F
		public string LastPlayerName { get; set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0000D378 File Offset: 0x0000B578
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x0000D380 File Offset: 0x0000B580
		public string Username { get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0000D389 File Offset: 0x0000B589
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x0000D391 File Offset: 0x0000B591
		public int UserId { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0000D39A File Offset: 0x0000B59A
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x0000D3A2 File Offset: 0x0000B5A2
		public bool IsUsingClanSigil { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x0000D3AB File Offset: 0x0000B5AB
		// (set) Token: 0x060008FC RID: 2300 RVA: 0x0000D3B3 File Offset: 0x0000B5B3
		public string LastRegion { get; set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x0000D3C4 File Offset: 0x0000B5C4
		public string[] LastGameTypes { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x0000D3CD File Offset: 0x0000B5CD
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x0000D3D5 File Offset: 0x0000B5D5
		public DateTime? LastLogin { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x0000D3DE File Offset: 0x0000B5DE
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x0000D3E6 File Offset: 0x0000B5E6
		public int Playtime { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0000D3EF File Offset: 0x0000B5EF
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x0000D3F7 File Offset: 0x0000B5F7
		public string ShownBadgeId { get; set; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0000D400 File Offset: 0x0000B600
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x0000D408 File Offset: 0x0000B608
		public int Gold { get; set; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x0000D411 File Offset: 0x0000B611
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x0000D419 File Offset: 0x0000B619
		public bool IsMuted { get; set; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x0000D424 File Offset: 0x0000B624
		[JsonIgnore]
		public int Level
		{
			get
			{
				return new PlayerDataExperience(this.Experience).Level;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0000D444 File Offset: 0x0000B644
		[JsonIgnore]
		public int ExperienceToNextLevel
		{
			get
			{
				return new PlayerDataExperience(this.Experience).ExperienceToNextLevel;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x0000D464 File Offset: 0x0000B664
		[JsonIgnore]
		public int ExperienceInCurrentLevel
		{
			get
			{
				return new PlayerDataExperience(this.Experience).ExperienceInCurrentLevel;
			}
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0000D48C File Offset: 0x0000B68C
		public void FillWith(PlayerId playerId, PlayerId ownerPlayerId, BodyProperties bodyProperties, bool isFemale, string sigil, int experience, string lastPlayerName, string username, int userId, string lastRegion, string[] lastGameTypes, DateTime? lastLogin, int playtime, string shownBadgeId, int gold, PlayerStatsBase[] stats, bool shouldLog, bool isUsingClanSigil)
		{
			this.PlayerId = playerId;
			this.OwnerPlayerId = ownerPlayerId;
			this.BodyProperties = bodyProperties;
			this.IsFemale = isFemale;
			this.Sigil = sigil;
			this.IsUsingClanSigil = isUsingClanSigil;
			this.Experience = experience;
			this.LastPlayerName = lastPlayerName;
			this.Username = username;
			this.UserId = userId;
			this.LastRegion = lastRegion;
			this.LastGameTypes = lastGameTypes;
			this.LastLogin = lastLogin;
			this.Playtime = playtime;
			this.ShownBadgeId = shownBadgeId;
			this.Gold = gold;
			this.Stats = stats;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0000D520 File Offset: 0x0000B720
		public void FillWithNewPlayer(PlayerId playerId, PlayerId ownerPlayerId, string[] gameTypes)
		{
			this.Stats = new PlayerStatsBase[0];
			this.PlayerId = playerId;
			this.OwnerPlayerId = ownerPlayerId;
			this.Sigil = "11.8.1.4345.4345.770.774.1.0.0.158.7.5.512.512.770.769.1.0.0";
			this.IsUsingClanSigil = false;
			this.LastGameTypes = gameTypes;
			this.Username = null;
			this.UserId = -1;
			this.Gold = 0;
			BodyProperties bodyProperties;
			if (BodyProperties.FromString("<BodyProperties version='4' age='36.35' weight='0.1025' build='0.7'  key='001C380CC000234B88E68BBA1372B7578B7BB5D788BC567878966669835754B604F926450F67798C000000000000000000000000000000000000000000DC10C4' />", out bodyProperties))
			{
				this.BodyProperties = bodyProperties;
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0000D58A File Offset: 0x0000B78A
		public bool HasGameStats(string gameType)
		{
			return this.GetGameStats(gameType) != null;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0000D598 File Offset: 0x0000B798
		public PlayerStatsBase GetGameStats(string gameType)
		{
			if (this.Stats != null)
			{
				foreach (PlayerStatsBase playerStatsBase in this.Stats)
				{
					if (playerStatsBase.GameType == gameType)
					{
						return playerStatsBase;
					}
				}
			}
			return null;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0000D5D8 File Offset: 0x0000B7D8
		public void UpdateGameStats(PlayerStatsBase playerGameTypeStats)
		{
			bool flag = false;
			if (this.Stats != null)
			{
				for (int i = 0; i < this.Stats.Length; i++)
				{
					if (this.Stats[i].GameType == playerGameTypeStats.GameType)
					{
						this.Stats[i] = playerGameTypeStats;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				List<PlayerStatsBase> list = new List<PlayerStatsBase>();
				if (this.Stats != null)
				{
					list.AddRange(this.Stats);
				}
				list.Add(playerGameTypeStats);
				this.Stats = list.ToArray();
			}
		}

		// Token: 0x040003DF RID: 991
		private const string DefaultBodyProperties1 = "<BodyProperties version='4' age='36.35' weight='0.1025' build='0.7'  key='001C380CC000234B88E68BBA1372B7578B7BB5D788BC567878966669835754B604F926450F67798C000000000000000000000000000000000000000000DC10C4' />";

		// Token: 0x040003E0 RID: 992
		private const string DefaultBodyProperties2 = "<BodyProperties version='4' age='46.35' weight='0.1025' build='0.7'  key='001C380CC000234B88E68BBA1372B7578B7BB5D788BC567878966669835754B604F926450F67798C000000000000000000000000000000000000000000DC10C4' />";

		// Token: 0x040003E1 RID: 993
		public const string DefaultSigil = "11.8.1.4345.4345.770.774.1.0.0.158.7.5.512.512.770.769.1.0.0";

		// Token: 0x040003E5 RID: 997
		private BodyProperties _bodyProperties;
	}
}
