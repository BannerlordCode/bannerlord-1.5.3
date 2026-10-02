using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000117 RID: 279
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class Feature : Attribute
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x000080E6 File Offset: 0x000062E6
		// (set) Token: 0x06000630 RID: 1584 RVA: 0x000080EE File Offset: 0x000062EE
		public Features FeatureFlag { get; private set; }

		// Token: 0x06000631 RID: 1585 RVA: 0x000080F7 File Offset: 0x000062F7
		public Feature(Features flag)
		{
			this.FeatureFlag = flag;
		}
	}
}
