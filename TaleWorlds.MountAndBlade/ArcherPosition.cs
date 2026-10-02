using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000183 RID: 387
	public class ArcherPosition
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0004BF00 File Offset: 0x0004A100
		public GameEntity Entity { get; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0004BF08 File Offset: 0x0004A108
		public TacticalPosition TacticalArcherPosition { get; }

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0004BF10 File Offset: 0x0004A110
		// (set) Token: 0x060014B4 RID: 5300 RVA: 0x0004BF18 File Offset: 0x0004A118
		public int ConnectedSides
		{
			get
			{
				return this._connectedSides;
			}
			private set
			{
				this._connectedSides = value;
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0004BF21 File Offset: 0x0004A121
		public Formation GetLastAssignedFormation(int teamIndex)
		{
			if (teamIndex >= 0)
			{
				return this._lastAssignedFormations[teamIndex];
			}
			return null;
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0004BF34 File Offset: 0x0004A134
		public ArcherPosition(GameEntity _entity, SiegeQuerySystem siegeQuerySystem, BattleSideEnum battleSide)
		{
			this.Entity = _entity;
			this.TacticalArcherPosition = this.Entity.GetFirstScriptOfType<TacticalPosition>();
			this._siegeQuerySystem = siegeQuerySystem;
			this.DetermineArcherPositionSide(battleSide);
			this._lastAssignedFormations = new Formation[Mission.Current.Teams.Count];
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0004BF87 File Offset: 0x0004A187
		private static int ConvertToBinaryPow(int pow)
		{
			return 1 << pow;
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0004BF8F File Offset: 0x0004A18F
		public bool IsArcherPositionRelatedToSide(FormationAI.BehaviorSide side)
		{
			return (ArcherPosition.ConvertToBinaryPow((int)side) & this.ConnectedSides) != 0;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0004BFA1 File Offset: 0x0004A1A1
		public FormationAI.BehaviorSide GetArcherPositionClosestSide()
		{
			return this._closestSide;
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0004BFA9 File Offset: 0x0004A1A9
		public void OnDeploymentFinished(SiegeQuerySystem siegeQuerySystem, BattleSideEnum battleSide)
		{
			this._siegeQuerySystem = siegeQuerySystem;
			this.DetermineArcherPositionSide(battleSide);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0004BFBC File Offset: 0x0004A1BC
		private void DetermineArcherPositionSide(BattleSideEnum battleSide)
		{
			this.ConnectedSides = 0;
			if (this.TacticalArcherPosition != null)
			{
				int tacticalPositionSide = (int)this.TacticalArcherPosition.TacticalPositionSide;
				if (tacticalPositionSide < 3)
				{
					this._closestSide = this.TacticalArcherPosition.TacticalPositionSide;
					this.ConnectedSides = ArcherPosition.ConvertToBinaryPow(tacticalPositionSide);
				}
			}
			if (this.ConnectedSides == 0)
			{
				if (battleSide == BattleSideEnum.Defender)
				{
					ArcherPosition.CalculateArcherPositionSideUsingDefenderLanes(this._siegeQuerySystem, this.Entity.GlobalPosition, out this._closestSide, out this._connectedSides);
					return;
				}
				ArcherPosition.CalculateArcherPositionSideUsingAttackerRegions(this._siegeQuerySystem, this.Entity.GlobalPosition, out this._closestSide, out this._connectedSides);
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0004C058 File Offset: 0x0004A258
		private static void CalculateArcherPositionSideUsingAttackerRegions(SiegeQuerySystem siegeQuerySystem, Vec3 position, out FormationAI.BehaviorSide _closestSide, out int ConnectedSides)
		{
			float num = position.DistanceSquared(siegeQuerySystem.LeftAttackerOrigin);
			float num2 = position.DistanceSquared(siegeQuerySystem.MiddleAttackerOrigin);
			float num3 = position.DistanceSquared(siegeQuerySystem.RightAttackerOrigin);
			FormationAI.BehaviorSide behaviorSide;
			if (num < num2 && num < num3)
			{
				behaviorSide = FormationAI.BehaviorSide.Left;
			}
			else if (num3 < num2)
			{
				behaviorSide = FormationAI.BehaviorSide.Right;
			}
			else
			{
				behaviorSide = FormationAI.BehaviorSide.Middle;
			}
			_closestSide = behaviorSide;
			ConnectedSides = ArcherPosition.ConvertToBinaryPow((int)behaviorSide);
			Vec2 vec = position.AsVec2 - siegeQuerySystem.LeftDefenderOrigin.AsVec2;
			if (vec.DotProduct(siegeQuerySystem.LeftToMidDir) >= 0f && vec.DotProduct(siegeQuerySystem.LeftToMidDir.RightVec()) >= 0f)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow(0);
			}
			else
			{
				vec = position.AsVec2 - siegeQuerySystem.MidDefenderOrigin.AsVec2;
				if (vec.DotProduct(siegeQuerySystem.MidToLeftDir) >= 0f && vec.DotProduct(siegeQuerySystem.MidToLeftDir.RightVec()) >= 0f)
				{
					ConnectedSides |= ArcherPosition.ConvertToBinaryPow(0);
				}
			}
			vec = position.AsVec2 - siegeQuerySystem.MidDefenderOrigin.AsVec2;
			if (vec.DotProduct(siegeQuerySystem.LeftToMidDir) >= 0f && vec.DotProduct(siegeQuerySystem.LeftToMidDir.LeftVec()) >= 0f)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow(1);
			}
			else
			{
				vec = position.AsVec2 - siegeQuerySystem.RightDefenderOrigin.AsVec2;
				if (vec.DotProduct(siegeQuerySystem.RightToMidDir) >= 0f && vec.DotProduct(siegeQuerySystem.RightToMidDir.RightVec()) >= 0f)
				{
					ConnectedSides |= ArcherPosition.ConvertToBinaryPow(1);
				}
			}
			vec = position.AsVec2 - siegeQuerySystem.RightDefenderOrigin.AsVec2;
			if (vec.DotProduct(siegeQuerySystem.MidToRightDir) >= 0f && vec.DotProduct(siegeQuerySystem.MidToRightDir.LeftVec()) >= 0f)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow(2);
				return;
			}
			vec = position.AsVec2 - siegeQuerySystem.RightDefenderOrigin.AsVec2;
			if (vec.DotProduct(siegeQuerySystem.RightToMidDir) >= 0f && vec.DotProduct(siegeQuerySystem.RightToMidDir.LeftVec()) >= 0f)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow(2);
			}
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0004C2D0 File Offset: 0x0004A4D0
		private static void CalculateArcherPositionSideUsingDefenderLanes(SiegeQuerySystem siegeQuerySystem, Vec3 position, out FormationAI.BehaviorSide _closestSide, out int ConnectedSides)
		{
			float num = position.DistanceSquared(siegeQuerySystem.LeftDefenderOrigin);
			float num2 = position.DistanceSquared(siegeQuerySystem.MidDefenderOrigin);
			float num3 = position.DistanceSquared(siegeQuerySystem.RightDefenderOrigin);
			FormationAI.BehaviorSide behaviorSide;
			if (num < num2 && num < num3)
			{
				behaviorSide = FormationAI.BehaviorSide.Left;
			}
			else if (num3 < num2)
			{
				behaviorSide = FormationAI.BehaviorSide.Right;
			}
			else
			{
				behaviorSide = FormationAI.BehaviorSide.Middle;
			}
			FormationAI.BehaviorSide behaviorSide2 = FormationAI.BehaviorSide.BehaviorSideNotSet;
			switch (behaviorSide)
			{
			case FormationAI.BehaviorSide.Left:
				if ((position.AsVec2 - siegeQuerySystem.LeftDefenderOrigin.AsVec2).Normalized().DotProduct(siegeQuerySystem.DefenderLeftToDefenderMidDir) > 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Middle;
				}
				break;
			case FormationAI.BehaviorSide.Middle:
				if ((position.AsVec2 - siegeQuerySystem.MidDefenderOrigin.AsVec2).Normalized().DotProduct(siegeQuerySystem.DefenderMidToDefenderRightDir) > 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Right;
				}
				else
				{
					behaviorSide2 = FormationAI.BehaviorSide.Left;
				}
				break;
			case FormationAI.BehaviorSide.Right:
				if ((position.AsVec2 - siegeQuerySystem.RightDefenderOrigin.AsVec2).Normalized().DotProduct(siegeQuerySystem.DefenderMidToDefenderRightDir) < 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Middle;
				}
				break;
			}
			_closestSide = behaviorSide;
			ConnectedSides = ArcherPosition.ConvertToBinaryPow((int)behaviorSide);
			if (behaviorSide2 != FormationAI.BehaviorSide.BehaviorSideNotSet)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow((int)behaviorSide2);
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0004C41B File Offset: 0x0004A61B
		public void SetLastAssignedFormation(int teamIndex, Formation formation)
		{
			if (teamIndex >= 0)
			{
				this._lastAssignedFormations[teamIndex] = formation;
			}
		}

		// Token: 0x04000580 RID: 1408
		private FormationAI.BehaviorSide _closestSide;

		// Token: 0x04000581 RID: 1409
		private int _connectedSides;

		// Token: 0x04000582 RID: 1410
		private SiegeQuerySystem _siegeQuerySystem;

		// Token: 0x04000583 RID: 1411
		private readonly Formation[] _lastAssignedFormations;
	}
}
