using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000154 RID: 340
	[Serializable]
	public class PremadeGameEntry
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0000E0E5 File Offset: 0x0000C2E5
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0000E0ED File Offset: 0x0000C2ED
		[JsonProperty]
		public Guid Id { get; private set; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0000E0F6 File Offset: 0x0000C2F6
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x0000E0FE File Offset: 0x0000C2FE
		[JsonProperty]
		public string Name { get; private set; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x0000E107 File Offset: 0x0000C307
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x0000E10F File Offset: 0x0000C30F
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0000E118 File Offset: 0x0000C318
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x0000E120 File Offset: 0x0000C320
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0000E129 File Offset: 0x0000C329
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0000E131 File Offset: 0x0000C331
		[JsonProperty]
		public string MapName { get; private set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0000E13A File Offset: 0x0000C33A
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0000E142 File Offset: 0x0000C342
		[JsonProperty]
		public string FactionA { get; private set; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0000E14B File Offset: 0x0000C34B
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x0000E153 File Offset: 0x0000C353
		[JsonProperty]
		public string FactionB { get; private set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0000E15C File Offset: 0x0000C35C
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x0000E164 File Offset: 0x0000C364
		[JsonProperty]
		public bool IsPasswordProtected { get; private set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0000E16D File Offset: 0x0000C36D
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x0000E175 File Offset: 0x0000C375
		[JsonProperty]
		public bool IsSpectatorPasswordProtected { get; private set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0000E17E File Offset: 0x0000C37E
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0000E186 File Offset: 0x0000C386
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x060009A3 RID: 2467 RVA: 0x0000E18F File Offset: 0x0000C38F
		public PremadeGameEntry()
		{
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0000E198 File Offset: 0x0000C398
		public PremadeGameEntry(Guid id, string name, string region, string gameType, string mapName, string factionA, string factionB, bool isPasswordProtected, PremadeGameType premadeGameType, bool isSpectatorPasswordProtected = false)
		{
			this.Id = id;
			this.Name = name;
			this.Region = region;
			this.GameType = gameType;
			this.MapName = mapName;
			this.FactionA = factionA;
			this.FactionB = factionB;
			this.IsPasswordProtected = isPasswordProtected;
			this.PremadeGameType = premadeGameType;
			this.IsSpectatorPasswordProtected = isSpectatorPasswordProtected;
		}
	}
}
