using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200008E RID: 142
	public interface IMissionSiegeWeapon
	{
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060008AF RID: 2223
		int Index { get; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060008B0 RID: 2224
		SiegeEngineType Type { get; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060008B1 RID: 2225
		float Health { get; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060008B2 RID: 2226
		float InitialHealth { get; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060008B3 RID: 2227
		float MaxHealth { get; }
	}
}
