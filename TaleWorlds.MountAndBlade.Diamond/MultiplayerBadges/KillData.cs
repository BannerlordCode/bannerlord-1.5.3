using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016D RID: 365
	public struct KillData
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x000105AC File Offset: 0x0000E7AC
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x000105B4 File Offset: 0x0000E7B4
		public PlayerId KillerId { get; set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x000105BD File Offset: 0x0000E7BD
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x000105C5 File Offset: 0x0000E7C5
		public PlayerId VictimId { get; set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x000105CE File Offset: 0x0000E7CE
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x000105D6 File Offset: 0x0000E7D6
		public string KillerFaction { get; set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x000105DF File Offset: 0x0000E7DF
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x000105E7 File Offset: 0x0000E7E7
		public string VictimFaction { get; set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x000105F0 File Offset: 0x0000E7F0
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x000105F8 File Offset: 0x0000E7F8
		public string KillerTroop { get; set; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00010601 File Offset: 0x0000E801
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x00010609 File Offset: 0x0000E809
		public string VictimTroop { get; set; }
	}
}
