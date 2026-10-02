using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000F5 RID: 245
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
	public sealed class AspMvcActionAttribute : Attribute
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0000FBC4 File Offset: 0x0000DDC4
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x0000FBCC File Offset: 0x0000DDCC
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }

		// Token: 0x0600095E RID: 2398 RVA: 0x0000FBD5 File Offset: 0x0000DDD5
		public AspMvcActionAttribute()
		{
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0000FBDD File Offset: 0x0000DDDD
		public AspMvcActionAttribute(string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}
	}
}
