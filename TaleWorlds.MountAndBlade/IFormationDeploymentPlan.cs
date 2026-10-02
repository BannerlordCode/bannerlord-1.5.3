using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021C RID: 540
	public interface IFormationDeploymentPlan
	{
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001F7F RID: 8063
		FormationClass Class { get; }

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001F80 RID: 8064
		FormationClass SpawnClass { get; }

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001F81 RID: 8065
		float PlannedWidth { get; }

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001F82 RID: 8066
		float PlannedDepth { get; }

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001F83 RID: 8067
		int PlannedTroopCount { get; }

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001F84 RID: 8068
		bool HasDimensions { get; }

		// Token: 0x06001F85 RID: 8069
		bool HasFrame();

		// Token: 0x06001F86 RID: 8070
		MatrixFrame GetFrame();

		// Token: 0x06001F87 RID: 8071
		Vec3 GetPosition();

		// Token: 0x06001F88 RID: 8072
		Vec2 GetDirection();

		// Token: 0x06001F89 RID: 8073
		WorldPosition CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache);
	}
}
