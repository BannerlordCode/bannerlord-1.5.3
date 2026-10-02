using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000227 RID: 551
	public class FormationPocket
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x0007483D File Offset: 0x00072A3D
		// (set) Token: 0x0600210A RID: 8458 RVA: 0x00074845 File Offset: 0x00072A45
		public Func<Agent, int> PriorityFunction { get; private set; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600210B RID: 8459 RVA: 0x0007484E File Offset: 0x00072A4E
		// (set) Token: 0x0600210C RID: 8460 RVA: 0x00074856 File Offset: 0x00072A56
		public int MaxValue { get; private set; }

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600210D RID: 8461 RVA: 0x0007485F File Offset: 0x00072A5F
		// (set) Token: 0x0600210E RID: 8462 RVA: 0x00074867 File Offset: 0x00072A67
		public int TroopCount { get; private set; }

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600210F RID: 8463 RVA: 0x00074870 File Offset: 0x00072A70
		// (set) Token: 0x06002110 RID: 8464 RVA: 0x00074878 File Offset: 0x00072A78
		public int Index { get; private set; }

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x00074881 File Offset: 0x00072A81
		// (set) Token: 0x06002112 RID: 8466 RVA: 0x00074889 File Offset: 0x00072A89
		public int AddedTroopCount { get; private set; }

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x00074892 File Offset: 0x00072A92
		// (set) Token: 0x06002114 RID: 8468 RVA: 0x0007489A File Offset: 0x00072A9A
		public int ScoreToSeek { get; private set; }

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002115 RID: 8469 RVA: 0x000748A3 File Offset: 0x00072AA3
		// (set) Token: 0x06002116 RID: 8470 RVA: 0x000748AB File Offset: 0x00072AAB
		public int BestScoreSoFar { get; private set; }

		// Token: 0x06002117 RID: 8471 RVA: 0x000748B4 File Offset: 0x00072AB4
		public FormationPocket(Func<Agent, int> priorityFunction, int maxValue, int troopCount, int index)
		{
			this.PriorityFunction = priorityFunction;
			this.MaxValue = maxValue;
			this.TroopCount = troopCount;
			this.Index = index;
			this.AddedTroopCount = 0;
			this.ScoreToSeek = maxValue;
			this.BestScoreSoFar = 0;
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000748F0 File Offset: 0x00072AF0
		public void AddTroop()
		{
			int addedTroopCount = this.AddedTroopCount;
			this.AddedTroopCount = addedTroopCount + 1;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0007490D File Offset: 0x00072B0D
		public bool IsFormationPocketFilled()
		{
			return this.AddedTroopCount >= this.TroopCount;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x00074920 File Offset: 0x00072B20
		public void UpdateScoreToSeek()
		{
			this.ScoreToSeek = this.BestScoreSoFar;
			this.BestScoreSoFar = 0;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x00074935 File Offset: 0x00072B35
		public void SetBestScoreSoFar(int bestScoreSoFar)
		{
			this.BestScoreSoFar = bestScoreSoFar;
		}
	}
}
