using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000F4 RID: 244
	[AttributeUsage(AttributeTargets.Parameter)]
	public class PathReferenceAttribute : Attribute
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x0000FB9C File Offset: 0x0000DD9C
		public PathReferenceAttribute()
		{
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0000FBA4 File Offset: 0x0000DDA4
		[UsedImplicitly]
		public PathReferenceAttribute([PathReference] string basePath)
		{
			this.BasePath = basePath;
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0000FBB3 File Offset: 0x0000DDB3
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x0000FBBB File Offset: 0x0000DDBB
		[UsedImplicitly]
		public string BasePath { get; private set; }
	}
}
