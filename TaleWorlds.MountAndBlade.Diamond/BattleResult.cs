using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FB RID: 251
	[Serializable]
	public class BattleResult
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x000059BC File Offset: 0x00003BBC
		public BattleResult()
		{
			this.PlayerEntries = new Dictionary<string, BattlePlayerEntry>();
			this.IsCancelled = false;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000059D8 File Offset: 0x00003BD8
		public void AddOrUpdatePlayerEntry(PlayerId playerId, int teamNo, string gameMode, Guid party, int overriddenInitialPlayTime = -1)
		{
			BattlePlayerEntry battlePlayerEntry;
			if (this.PlayerEntries.TryGetValue(playerId.ToString(), out battlePlayerEntry))
			{
				battlePlayerEntry.TeamNo = teamNo;
				battlePlayerEntry.Party = party;
				battlePlayerEntry.GameType = gameMode;
				if (battlePlayerEntry.Disconnected)
				{
					battlePlayerEntry.Disconnected = false;
					battlePlayerEntry.LastJoinTime = DateTime.Now;
					return;
				}
			}
			else
			{
				BattlePlayerStatsBase battlePlayerStatsBase = this.CreatePlayerBattleStats(gameMode);
				battlePlayerEntry = new BattlePlayerEntry();
				battlePlayerEntry.PlayerId = playerId;
				battlePlayerEntry.TeamNo = teamNo;
				battlePlayerEntry.Party = party;
				battlePlayerEntry.GameType = gameMode;
				battlePlayerEntry.PlayerStats = battlePlayerStatsBase;
				battlePlayerEntry.LastJoinTime = DateTime.Now;
				battlePlayerEntry.PlayTime = ((overriddenInitialPlayTime != -1) ? overriddenInitialPlayTime : 0);
				battlePlayerEntry.Disconnected = false;
				this.PlayerEntries.Add(playerId.ToString(), battlePlayerEntry);
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00005AA2 File Offset: 0x00003CA2
		public bool TryGetPlayerEntry(PlayerId playerId, out BattlePlayerEntry battlePlayerEntry)
		{
			return this.PlayerEntries.TryGetValue(playerId.ToString(), out battlePlayerEntry);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public void HandlePlayerDisconnect(PlayerId playerId)
		{
			BattlePlayerEntry battlePlayerEntry;
			if (this.PlayerEntries.TryGetValue(playerId.ToString(), out battlePlayerEntry))
			{
				battlePlayerEntry.Disconnected = true;
				battlePlayerEntry.PlayTime += (int)(DateTime.Now - battlePlayerEntry.LastJoinTime).TotalSeconds;
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00005B18 File Offset: 0x00003D18
		public void DebugPrint()
		{
			Debug.Print("-----PRINTING BATTLE RESULT-----", 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (BattlePlayerEntry battlePlayerEntry in this.PlayerEntries.Values)
			{
				Debug.Print("Player: " + battlePlayerEntry.PlayerId + "[DEBUG] ", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print("Kill: " + battlePlayerEntry.PlayerStats.Kills + "[DEBUG] ", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print("Death: " + battlePlayerEntry.PlayerStats.Deaths + "[DEBUG] ", 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print("----", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			Debug.Print("-----PRINTING OVER-----", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00005C3C File Offset: 0x00003E3C
		public void SetBattleFinished(int winnerTeamNo, bool isPremadeGame, PremadeGameType premadeGameType)
		{
			this.WinnerTeamNo = winnerTeamNo;
			this.IsPremadeGame = isPremadeGame;
			this.PremadeGameType = premadeGameType;
			foreach (BattlePlayerEntry battlePlayerEntry in this.PlayerEntries.Values)
			{
				battlePlayerEntry.Won = battlePlayerEntry.TeamNo == winnerTeamNo;
				if (!battlePlayerEntry.Disconnected)
				{
					battlePlayerEntry.PlayTime += (int)(DateTime.Now - battlePlayerEntry.LastJoinTime).TotalSeconds;
				}
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00005CE0 File Offset: 0x00003EE0
		public void SetBattleCancelled()
		{
			this.IsCancelled = true;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00005CEC File Offset: 0x00003EEC
		private BattlePlayerStatsBase CreatePlayerBattleStats(string gameType)
		{
			if (gameType == "Skirmish")
			{
				return new BattlePlayerStatsSkirmish();
			}
			if (gameType == "Captain")
			{
				return new BattlePlayerStatsCaptain();
			}
			if (gameType == "Siege")
			{
				return new BattlePlayerStatsSiege();
			}
			if (gameType == "TeamDeathmatch")
			{
				return new BattlePlayerStatsTeamDeathmatch();
			}
			if (gameType == "Duel")
			{
				return new BattlePlayerStatsDuel();
			}
			if (gameType == "Battle")
			{
				return new BattlePlayerStatsBattle();
			}
			return new BattlePlayerStatsBase();
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00005D70 File Offset: 0x00003F70
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x00005D78 File Offset: 0x00003F78
		[JsonProperty]
		public bool IsCancelled { get; private set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00005D81 File Offset: 0x00003F81
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x00005D89 File Offset: 0x00003F89
		[JsonProperty]
		public int WinnerTeamNo { get; private set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00005D92 File Offset: 0x00003F92
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x00005D9A File Offset: 0x00003F9A
		[JsonProperty]
		public bool IsPremadeGame { get; private set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00005DA3 File Offset: 0x00003FA3
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x00005DAB File Offset: 0x00003FAB
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00005DB4 File Offset: 0x00003FB4
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x00005DBC File Offset: 0x00003FBC
		[JsonProperty]
		public Dictionary<string, BattlePlayerEntry> PlayerEntries { get; private set; }
	}
}
