using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F1 RID: 241
	[Serializable]
	public class BattlePlayerEntry
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00005639 File Offset: 0x00003839
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00005641 File Offset: 0x00003841
		public PlayerId PlayerId { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0000564A File Offset: 0x0000384A
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x00005652 File Offset: 0x00003852
		public int TeamNo { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x0000565B File Offset: 0x0000385B
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00005663 File Offset: 0x00003863
		public Guid Party { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0000566C File Offset: 0x0000386C
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00005674 File Offset: 0x00003874
		public BattlePlayerStatsBase PlayerStats { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0000567D File Offset: 0x0000387D
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00005685 File Offset: 0x00003885
		public int PlayTime { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0000568E File Offset: 0x0000388E
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x00005696 File Offset: 0x00003896
		public DateTime LastJoinTime { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0000569F File Offset: 0x0000389F
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x000056A7 File Offset: 0x000038A7
		public bool Disconnected { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x000056B0 File Offset: 0x000038B0
		// (set) Token: 0x060004CF RID: 1231 RVA: 0x000056B8 File Offset: 0x000038B8
		public string GameType { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x000056C1 File Offset: 0x000038C1
		// (set) Token: 0x060004D1 RID: 1233 RVA: 0x000056C9 File Offset: 0x000038C9
		public bool Won { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x000056D2 File Offset: 0x000038D2
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x000056DA File Offset: 0x000038DA
		public BattleJoinType BattleJoinType { get; set; }
	}
}
