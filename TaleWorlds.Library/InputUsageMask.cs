using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000041 RID: 65
	[Flags]
	public enum InputUsageMask
	{
		// Token: 0x040000E5 RID: 229
		Invalid = 0,
		// Token: 0x040000E6 RID: 230
		MouseButtons = 1,
		// Token: 0x040000E7 RID: 231
		MouseWheels = 2,
		// Token: 0x040000E8 RID: 232
		Keyboardkeys = 4,
		// Token: 0x040000E9 RID: 233
		BlockEverythingWithoutHitTest = 8,
		// Token: 0x040000EA RID: 234
		Mouse = 3,
		// Token: 0x040000EB RID: 235
		All = 7
	}
}
