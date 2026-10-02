using System;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000026 RID: 38
	public class OverrideView : Attribute
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00008194 File Offset: 0x00006394
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000819C File Offset: 0x0000639C
		public Type BaseType { get; private set; }

		// Token: 0x06000117 RID: 279 RVA: 0x000081A5 File Offset: 0x000063A5
		public OverrideView(Type baseType)
		{
			this.BaseType = baseType;
		}
	}
}
