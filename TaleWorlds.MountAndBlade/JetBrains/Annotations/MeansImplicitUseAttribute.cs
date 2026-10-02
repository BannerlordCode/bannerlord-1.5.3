using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000EE RID: 238
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class MeansImplicitUseAttribute : Attribute
	{
		// Token: 0x0600094C RID: 2380 RVA: 0x0000FB26 File Offset: 0x0000DD26
		[UsedImplicitly]
		public MeansImplicitUseAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0000FB30 File Offset: 0x0000DD30
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0000FB46 File Offset: 0x0000DD46
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0000FB50 File Offset: 0x0000DD50
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0000FB5A File Offset: 0x0000DD5A
		// (set) Token: 0x06000951 RID: 2385 RVA: 0x0000FB62 File Offset: 0x0000DD62
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; private set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0000FB6B File Offset: 0x0000DD6B
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x0000FB73 File Offset: 0x0000DD73
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}
}
