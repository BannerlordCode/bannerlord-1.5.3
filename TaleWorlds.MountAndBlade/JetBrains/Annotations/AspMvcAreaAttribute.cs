using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000F6 RID: 246
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AspMvcAreaAttribute : PathReferenceAttribute
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0000FBEC File Offset: 0x0000DDEC
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x0000FBF4 File Offset: 0x0000DDF4
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }

		// Token: 0x06000962 RID: 2402 RVA: 0x0000FBFD File Offset: 0x0000DDFD
		[UsedImplicitly]
		public AspMvcAreaAttribute()
		{
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0000FC05 File Offset: 0x0000DE05
		public AspMvcAreaAttribute(string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}
	}
}
