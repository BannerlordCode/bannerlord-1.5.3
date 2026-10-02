using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200025C RID: 604
	public class CompassMarker
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x00078D92 File Offset: 0x00076F92
		// (set) Token: 0x06002254 RID: 8788 RVA: 0x00078D9A File Offset: 0x00076F9A
		public string Id { get; private set; }

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x00078DA3 File Offset: 0x00076FA3
		// (set) Token: 0x06002256 RID: 8790 RVA: 0x00078DAB File Offset: 0x00076FAB
		public float Angle { get; private set; }

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x00078DB4 File Offset: 0x00076FB4
		// (set) Token: 0x06002258 RID: 8792 RVA: 0x00078DBC File Offset: 0x00076FBC
		public bool IsPrimary { get; private set; }

		// Token: 0x06002259 RID: 8793 RVA: 0x00078DC5 File Offset: 0x00076FC5
		public CompassMarker(string id, float angle, bool isPrimary)
		{
			this.Id = id;
			this.Angle = angle % 360f;
			this.IsPrimary = isPrimary;
		}
	}
}
