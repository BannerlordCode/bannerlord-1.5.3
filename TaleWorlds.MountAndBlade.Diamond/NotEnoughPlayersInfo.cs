using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000107 RID: 263
	[Serializable]
	public class NotEnoughPlayersInfo
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x000071C4 File Offset: 0x000053C4
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x000071CC File Offset: 0x000053CC
		[JsonProperty]
		public int CurrentPlayerCount { get; private set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x000071D5 File Offset: 0x000053D5
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x000071DD File Offset: 0x000053DD
		[JsonProperty]
		public int RequiredPlayerCount { get; private set; }

		// Token: 0x060005AA RID: 1450 RVA: 0x000071E6 File Offset: 0x000053E6
		public NotEnoughPlayersInfo(int currentPlayerCount, int requiredPlayerCount)
		{
			this.CurrentPlayerCount = currentPlayerCount;
			this.RequiredPlayerCount = requiredPlayerCount;
		}
	}
}
