using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000063 RID: 99
	[Flags]
	public enum TroopUsageFlags : ushort
	{
		// Token: 0x040003C6 RID: 966
		Undefined = 0,
		// Token: 0x040003C7 RID: 967
		OnFoot = 1,
		// Token: 0x040003C8 RID: 968
		Mounted = 2,
		// Token: 0x040003C9 RID: 969
		Melee = 4,
		// Token: 0x040003CA RID: 970
		Ranged = 8,
		// Token: 0x040003CB RID: 971
		OneHandedUser = 16,
		// Token: 0x040003CC RID: 972
		ShieldUser = 32,
		// Token: 0x040003CD RID: 973
		TwoHandedUser = 64,
		// Token: 0x040003CE RID: 974
		PolearmUser = 128,
		// Token: 0x040003CF RID: 975
		BowUser = 256,
		// Token: 0x040003D0 RID: 976
		ThrownUser = 512,
		// Token: 0x040003D1 RID: 977
		CrossbowUser = 1024,
		// Token: 0x040003D2 RID: 978
		Any = 65535
	}
}
