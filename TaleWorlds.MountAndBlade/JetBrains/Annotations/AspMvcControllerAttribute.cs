using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000F7 RID: 247
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
	public sealed class AspMvcControllerAttribute : Attribute
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0000FC14 File Offset: 0x0000DE14
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }

		// Token: 0x06000966 RID: 2406 RVA: 0x0000FC25 File Offset: 0x0000DE25
		public AspMvcControllerAttribute()
		{
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0000FC2D File Offset: 0x0000DE2D
		public AspMvcControllerAttribute(string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}
	}
}
