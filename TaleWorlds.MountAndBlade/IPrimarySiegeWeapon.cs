using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000352 RID: 850
	public interface IPrimarySiegeWeapon
	{
		// Token: 0x0600301E RID: 12318
		bool HasCompletedAction();

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x0600301F RID: 12319
		float SiegeWeaponPriority { get; }

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003020 RID: 12320
		int OverTheWallNavMeshID { get; }

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06003021 RID: 12321
		bool HoldLadders { get; }

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003022 RID: 12322
		bool SendLadders { get; }

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003023 RID: 12323
		MissionObject TargetCastlePosition { get; }

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003024 RID: 12324
		FormationAI.BehaviorSide WeaponSide { get; }

		// Token: 0x06003025 RID: 12325
		bool GetNavmeshFaceIds(out List<int> navmeshFaceIds);
	}
}
