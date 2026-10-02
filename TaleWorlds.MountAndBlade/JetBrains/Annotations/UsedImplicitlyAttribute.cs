using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000ED RID: 237
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class UsedImplicitlyAttribute : Attribute
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x0000FAD0 File Offset: 0x0000DCD0
		[UsedImplicitly]
		public UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0000FADA File Offset: 0x0000DCDA
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0000FAFA File Offset: 0x0000DCFA
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x0000FB04 File Offset: 0x0000DD04
		// (set) Token: 0x06000949 RID: 2377 RVA: 0x0000FB0C File Offset: 0x0000DD0C
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; private set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x0000FB15 File Offset: 0x0000DD15
		// (set) Token: 0x0600094B RID: 2379 RVA: 0x0000FB1D File Offset: 0x0000DD1D
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}
}
