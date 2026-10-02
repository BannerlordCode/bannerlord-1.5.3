using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200037D RID: 893
	public interface ITargetable
	{
		// Token: 0x06003322 RID: 13090
		TargetFlags GetTargetFlags();

		// Token: 0x06003323 RID: 13091
		float GetTargetValue(List<Vec3> referencePositions);

		// Token: 0x06003324 RID: 13092
		WeakGameEntity GetTargetEntity();

		// Token: 0x06003325 RID: 13093
		Vec3 GetTargetingOffset();

		// Token: 0x06003326 RID: 13094
		BattleSideEnum GetSide();

		// Token: 0x06003327 RID: 13095
		Vec3 GetTargetGlobalVelocity();

		// Token: 0x06003328 RID: 13096
		bool IsDestructable();

		// Token: 0x06003329 RID: 13097
		WeakGameEntity Entity();

		// Token: 0x0600332A RID: 13098
		ValueTuple<Vec3, Vec3> ComputeGlobalPhysicsBoundingBoxMinMax();
	}
}
