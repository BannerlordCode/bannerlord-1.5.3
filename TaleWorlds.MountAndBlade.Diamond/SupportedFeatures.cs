using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000160 RID: 352
	[Serializable]
	public class SupportedFeatures
	{
		// Token: 0x060009E0 RID: 2528 RVA: 0x0000F4A8 File Offset: 0x0000D6A8
		public SupportedFeatures()
		{
			this.Features = -1;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0000F4B7 File Offset: 0x0000D6B7
		public SupportedFeatures(int features)
		{
			this.Features = features;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0000F4C8 File Offset: 0x0000D6C8
		public bool SupportsFeatures(Features feature)
		{
			return (this.Features & (int)feature) == (int)feature;
		}

		// Token: 0x040004E3 RID: 1251
		public int Features;
	}
}
