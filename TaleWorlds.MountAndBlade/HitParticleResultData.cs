using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019B RID: 411
	[EngineStruct("Hit_particle_result_data", false, null)]
	public struct HitParticleResultData
	{
		// Token: 0x060015AD RID: 5549 RVA: 0x00050A7F File Offset: 0x0004EC7F
		public void Reset()
		{
			this.StartHitParticleIndex = -1;
			this.ContinueHitParticleIndex = -1;
			this.EndHitParticleIndex = -1;
		}

		// Token: 0x04000663 RID: 1635
		public int StartHitParticleIndex;

		// Token: 0x04000664 RID: 1636
		public int ContinueHitParticleIndex;

		// Token: 0x04000665 RID: 1637
		public int EndHitParticleIndex;
	}
}
