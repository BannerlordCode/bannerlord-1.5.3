using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001DD RID: 477
	public struct MBParticleSystem
	{
		// Token: 0x06001C7B RID: 7291 RVA: 0x00061E85 File Offset: 0x00060085
		internal MBParticleSystem(int i)
		{
			this.index = i;
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00061E8E File Offset: 0x0006008E
		public bool Equals(MBParticleSystem a)
		{
			return this.index == a.index;
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00061E9E File Offset: 0x0006009E
		public override int GetHashCode()
		{
			return this.index;
		}

		// Token: 0x04000990 RID: 2448
		private int index;
	}
}
