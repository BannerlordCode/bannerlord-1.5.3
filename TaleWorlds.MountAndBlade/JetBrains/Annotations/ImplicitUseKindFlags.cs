using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000EF RID: 239
	[Flags]
	public enum ImplicitUseKindFlags
	{
		// Token: 0x0400022F RID: 559
		Default = 7,
		// Token: 0x04000230 RID: 560
		Access = 1,
		// Token: 0x04000231 RID: 561
		Assign = 2,
		// Token: 0x04000232 RID: 562
		InstantiatedWithFixedConstructorSignature = 4,
		// Token: 0x04000233 RID: 563
		InstantiatedNoFixedConstructorSignature = 8
	}
}
