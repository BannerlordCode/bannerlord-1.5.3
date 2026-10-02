using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014A RID: 330
	[JsonConverter(typeof(PlayerStatsBaseJsonConverter))]
	[Serializable]
	public class PlayerStatsBase
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0000D7DC File Offset: 0x0000B9DC
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x0000D7E4 File Offset: 0x0000B9E4
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0000D7ED File Offset: 0x0000B9ED
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x0000D7F5 File Offset: 0x0000B9F5
		[JsonProperty]
		public int KillCount { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0000D7FE File Offset: 0x0000B9FE
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0000D806 File Offset: 0x0000BA06
		[JsonProperty]
		public int DeathCount { get; set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0000D80F File Offset: 0x0000BA0F
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0000D817 File Offset: 0x0000BA17
		[JsonProperty]
		public int AssistCount { get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0000D820 File Offset: 0x0000BA20
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x0000D828 File Offset: 0x0000BA28
		[JsonProperty]
		public int WinCount { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0000D831 File Offset: 0x0000BA31
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x0000D839 File Offset: 0x0000BA39
		[JsonProperty]
		public int LoseCount { get; set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0000D842 File Offset: 0x0000BA42
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x0000D84A File Offset: 0x0000BA4A
		[JsonProperty]
		public int ForfeitCount { get; set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x0000D853 File Offset: 0x0000BA53
		[JsonIgnore]
		public float AverageKillPerDeath
		{
			get
			{
				return (float)this.KillCount / (float)((this.DeathCount != 0) ? this.DeathCount : 1);
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0000D86F File Offset: 0x0000BA6F
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x0000D877 File Offset: 0x0000BA77
		[JsonProperty]
		public string GameType { get; set; }

		// Token: 0x06000938 RID: 2360 RVA: 0x0000D888 File Offset: 0x0000BA88
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount)
		{
			this.PlayerId = playerId;
			this.KillCount = killCount;
			this.DeathCount = deathCount;
			this.AssistCount = assistCount;
			this.WinCount = winCount;
			this.LoseCount = loseCount;
			this.ForfeitCount = forfeitCount;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
		public virtual void Update(BattlePlayerStatsBase battleStats, bool won)
		{
			this.KillCount += battleStats.Kills;
			this.DeathCount += battleStats.Deaths;
			this.AssistCount += battleStats.Assists;
			int num;
			if (won)
			{
				num = this.WinCount;
				this.WinCount = num + 1;
				return;
			}
			num = this.LoseCount;
			this.LoseCount = num + 1;
		}
	}
}
