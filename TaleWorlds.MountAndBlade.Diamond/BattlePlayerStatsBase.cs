using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F3 RID: 243
	[JsonConverter(typeof(BattlePlayerStatsBaseJsonConverter))]
	[Serializable]
	public class BattlePlayerStatsBase
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x000056EB File Offset: 0x000038EB
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x000056F3 File Offset: 0x000038F3
		public string GameType { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x000056FC File Offset: 0x000038FC
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00005704 File Offset: 0x00003904
		public int Kills { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000570D File Offset: 0x0000390D
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00005715 File Offset: 0x00003915
		public int Assists { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0000571E File Offset: 0x0000391E
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00005726 File Offset: 0x00003926
		public int Deaths { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000572F File Offset: 0x0000392F
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00005737 File Offset: 0x00003937
		public int PlayTime { get; set; }
	}
}
