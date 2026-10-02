using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012D RID: 301
	[Serializable]
	public class PlayerJoinGameResponseDataFromHost
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0000BE1C File Offset: 0x0000A01C
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0000BE24 File Offset: 0x0000A024
		public PlayerId PlayerId { get; set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0000BE2D File Offset: 0x0000A02D
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x0000BE35 File Offset: 0x0000A035
		public int PeerIndex { get; set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0000BE3E File Offset: 0x0000A03E
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x0000BE46 File Offset: 0x0000A046
		public int SessionKey { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0000BE4F File Offset: 0x0000A04F
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x0000BE57 File Offset: 0x0000A057
		public CustomGameJoinType JoinType { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0000BE60 File Offset: 0x0000A060
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x0000BE68 File Offset: 0x0000A068
		public CustomGameJoinResponse CustomGameJoinResponse { get; set; }
	}
}
