using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000216 RID: 534
	public class DefaultFormationDeploymentPlan : IFormationDeploymentPlan
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001F0B RID: 7947 RVA: 0x0006B97B File Offset: 0x00069B7B
		public FormationClass Class
		{
			get
			{
				return this._class;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x0006B983 File Offset: 0x00069B83
		public FormationClass SpawnClass
		{
			get
			{
				return this._spawnClass;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001F0D RID: 7949 RVA: 0x0006B98B File Offset: 0x00069B8B
		public float PlannedWidth
		{
			get
			{
				return this._plannedWidth;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x0006B993 File Offset: 0x00069B93
		public float PlannedDepth
		{
			get
			{
				return this._plannedDepth;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x0006B99B File Offset: 0x00069B9B
		public int PlannedTroopCount
		{
			get
			{
				return this._plannedFootTroopCount + this._plannedMountedTroopCount;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x0006B9AA File Offset: 0x00069BAA
		public int PlannedFootTroopCount
		{
			get
			{
				return this._plannedFootTroopCount;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x0006B9B2 File Offset: 0x00069BB2
		public int PlannedMountedTroopCount
		{
			get
			{
				return this._plannedMountedTroopCount;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x0006B9BA File Offset: 0x00069BBA
		public bool HasDimensions
		{
			get
			{
				return this._plannedWidth >= 1E-05f && this._plannedDepth >= 1E-05f;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x0006B9DB File Offset: 0x00069BDB
		public bool HasSignificantMountedTroops
		{
			get
			{
				return DefaultMissionDeploymentPlan.HasSignificantMountedTroops(this._plannedFootTroopCount, this._plannedMountedTroopCount);
			}
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x0006B9EE File Offset: 0x00069BEE
		public DefaultFormationDeploymentPlan(FormationClass fClass)
		{
			this._class = fClass;
			this._spawnClass = fClass;
			this.Clear();
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0006BA0A File Offset: 0x00069C0A
		public bool HasFrame()
		{
			return this._spawnFrame.IsValid;
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0006BA17 File Offset: 0x00069C17
		public FormationDeploymentFlank GetDefaultFlank(int formationTroopCount, bool teamPlanHasAnyFootTroops, bool spawnWithHorses = false)
		{
			return DefaultFormationDeploymentPlan.GetFormationDefaultFlankAux(this._class, formationTroopCount, teamPlanHasAnyFootTroops, this.HasSignificantMountedTroops, spawnWithHorses);
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x0006BA2D File Offset: 0x00069C2D
		public FormationDeploymentOrder GetFlankDeploymentOrder(int offset = 0)
		{
			return FormationDeploymentOrder.GetDeploymentOrder(this._class, offset);
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0006BA3B File Offset: 0x00069C3B
		public MatrixFrame GetFrame()
		{
			return this._spawnFrame.ToGroundMatrixFrame();
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x0006BA48 File Offset: 0x00069C48
		public Vec3 GetPosition()
		{
			return this._spawnFrame.Origin.GetGroundVec3();
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0006BA5C File Offset: 0x00069C5C
		public Vec2 GetDirection()
		{
			return this._spawnFrame.Rotation.f.AsVec2.Normalized();
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0006BA88 File Offset: 0x00069C88
		public WorldPosition CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
			{
				return new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, this._spawnFrame.Origin.GetNavMeshVec3(), false);
			}
			if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.GroundVec3)
			{
				return this._spawnFrame.Origin;
			}
			return new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, this._spawnFrame.Origin.GetGroundVec3(), false);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0006BAF6 File Offset: 0x00069CF6
		public void Clear()
		{
			this._plannedWidth = 0f;
			this._plannedDepth = 0f;
			this._plannedFootTroopCount = 0;
			this._plannedMountedTroopCount = 0;
			this._spawnFrame = WorldFrame.Invalid;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0006BB27 File Offset: 0x00069D27
		public void SetPlannedTroopCount(int footTroopCount, int mountedTroopCount)
		{
			this._plannedFootTroopCount = footTroopCount;
			this._plannedMountedTroopCount = mountedTroopCount;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0006BB37 File Offset: 0x00069D37
		public void SetPlannedDimensions(float width, float depth)
		{
			this._plannedWidth = MathF.Max(0f, width);
			this._plannedDepth = MathF.Max(0f, depth);
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0006BB5B File Offset: 0x00069D5B
		public void SetFrame(in WorldFrame frame)
		{
			this._spawnFrame = frame;
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0006BB69 File Offset: 0x00069D69
		public void SetSpawnClass(FormationClass spawnClass)
		{
			this._spawnClass = spawnClass;
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0006BB74 File Offset: 0x00069D74
		public static FormationDeploymentFlank GetFormationDefaultFlankAux(FormationClass formationClass, int formationTroopCount, bool teamPlanHasAnyFootTroops, bool hasSignificantMountedTroops, bool canSpawnWithHorses)
		{
			FormationDeploymentFlank formationDeploymentFlank;
			if (!formationClass.IsMounted() && formationTroopCount == 0)
			{
				formationDeploymentFlank = FormationDeploymentFlank.Rear;
			}
			else if (hasSignificantMountedTroops && (!canSpawnWithHorses || !teamPlanHasAnyFootTroops))
			{
				if (formationTroopCount == 0 || formationClass == FormationClass.LightCavalry || formationClass == FormationClass.HorseArcher)
				{
					formationDeploymentFlank = FormationDeploymentFlank.Rear;
				}
				else
				{
					formationDeploymentFlank = FormationDeploymentFlank.Front;
				}
			}
			else
			{
				switch (formationClass)
				{
				case FormationClass.Ranged:
				case FormationClass.NumberOfRegularFormations:
				case FormationClass.Bodyguard:
				case FormationClass.NumberOfAllFormations:
					return FormationDeploymentFlank.Rear;
				case FormationClass.Cavalry:
				case FormationClass.HeavyCavalry:
					return FormationDeploymentFlank.Left;
				case FormationClass.HorseArcher:
				case FormationClass.LightCavalry:
					return FormationDeploymentFlank.Right;
				}
				formationDeploymentFlank = FormationDeploymentFlank.Front;
			}
			return formationDeploymentFlank;
		}

		// Token: 0x04000AA3 RID: 2723
		private WorldFrame _spawnFrame;

		// Token: 0x04000AA4 RID: 2724
		private FormationClass _spawnClass;

		// Token: 0x04000AA5 RID: 2725
		private readonly FormationClass _class;

		// Token: 0x04000AA6 RID: 2726
		private float _plannedWidth;

		// Token: 0x04000AA7 RID: 2727
		private float _plannedDepth;

		// Token: 0x04000AA8 RID: 2728
		private int _plannedFootTroopCount;

		// Token: 0x04000AA9 RID: 2729
		private int _plannedMountedTroopCount;
	}
}
