using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000295 RID: 661
	public abstract class MissionDeploymentPlanningLogic : MissionLogic, IMissionDeploymentPlan
	{
		// Token: 0x060024E6 RID: 9446 RVA: 0x00086E78 File Offset: 0x00085078
		public virtual void Initialize()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x00086E7F File Offset: 0x0008507F
		public virtual void ClearAll()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x00086E86 File Offset: 0x00085086
		public virtual void MakeDefaultDeploymentPlans()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x00086E8D File Offset: 0x0008508D
		public virtual void MakeDeploymentPlan(Team team, float spawnPathOffset = 0f, float targetPathOffset = 0f)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x00086E94 File Offset: 0x00085094
		public virtual bool RemakeDeploymentPlan(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x00086E9B File Offset: 0x0008509B
		public virtual void ClearDeploymentPlan(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00086EA2 File Offset: 0x000850A2
		public virtual bool IsPlanMade(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x00086EA9 File Offset: 0x000850A9
		public virtual bool IsPlanMade(Team team, out bool isFirstPlan)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x00086EB0 File Offset: 0x000850B0
		public virtual bool IsPositionInsideDeploymentBoundaries(Team team, in Vec2 position)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x00086EB7 File Offset: 0x000850B7
		public virtual bool HasDeploymentBoundaries(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x00086EBE File Offset: 0x000850BE
		[return: TupleElementNames(new string[] { "id", "points" })]
		public virtual MBReadOnlyList<ValueTuple<string, MBList<Vec2>>> GetDeploymentBoundaries(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x00086EC5 File Offset: 0x000850C5
		public virtual bool SupportsReinforcements()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x00086ECC File Offset: 0x000850CC
		public virtual void UpdateReinforcementPlan(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x00086ED3 File Offset: 0x000850D3
		public virtual bool SupportsNavmesh(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x00086EDA File Offset: 0x000850DA
		public virtual bool HasPlayerSpawnFrame(BattleSideEnum battleSide)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x00086EE1 File Offset: 0x000850E1
		public virtual bool GetPlayerSpawnFrame(BattleSideEnum battleSide, out WorldPosition position, out Vec2 direction)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x00086EE8 File Offset: 0x000850E8
		public virtual Vec2 GetClosestDeploymentBoundaryPosition(Team team, in Vec2 position)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x00086EEF File Offset: 0x000850EF
		public virtual void ProjectPositionToDeploymentBoundaries(Team team, ref WorldPosition position)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x00086EF6 File Offset: 0x000850F6
		public virtual bool GetPathDeploymentBoundaryIntersection(Team team, in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition foundPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x00086EFD File Offset: 0x000850FD
		public virtual MatrixFrame GetDeploymentZoneFrame(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x00086F04 File Offset: 0x00085104
		public virtual MatrixFrame GetFormationsCenterFrameAndExtents(Team team, out Vec2 halfExtents, bool ignoreDimensionlessFormations = true)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x00086F0B File Offset: 0x0008510B
		public virtual IFormationDeploymentPlan GetFormationPlan(Team team, FormationClass fClass, bool isReinforcement = false)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x00086F12 File Offset: 0x00085112
		public virtual float GetSpawnPathOffset(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x00086F19 File Offset: 0x00085119
		public virtual MatrixFrame GetZoomFocusFrame(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x00086F20 File Offset: 0x00085120
		public virtual float GetZoomOffset(Team team, float fovAngle)
		{
			throw new NotImplementedException();
		}
	}
}
